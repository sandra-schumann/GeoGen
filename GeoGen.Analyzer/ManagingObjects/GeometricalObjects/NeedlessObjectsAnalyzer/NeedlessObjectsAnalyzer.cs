﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using GeoGen.Core;
using GeoGen.Utilities;

namespace GeoGen.Analyzer
{
    /// <summary>
    /// Default implementation of <see cref="INeedlessObjectsAnalyzer"/>.
    /// </summary>
    internal class NeedlessObjectsAnalyzer : INeedlessObjectsAnalyzer
    {
        #region Private fields

        /// <summary>
        /// The combinator used to combine all possible definitions of 
        /// our geometrical objects.
        /// </summary>
        private readonly ICombinator _combinator;

        /// <summary>
        /// The subsets generator used to find all pair / triples of points
        /// to define a line or a circle
        /// </summary>
        private readonly ISubsetsProvider _subsetsProvider;

        /// <summary>
        /// The dictionary mapping ids of objects to the set of
        /// ids of objects that are needed to define this object.
        /// </summary>
        private readonly Dictionary<int, HashSet<int>> _cache;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="combinator">The combinator used to generate all combinations of definitions of objects.</param>
        /// <param name="subsetsProvider">The subsets provided for generating pair and triples of points to define lines / circles.</param>
        public NeedlessObjectsAnalyzer(ICombinator combinator, ISubsetsProvider subsetsProvider)
        {
            _combinator = combinator ?? throw new ArgumentNullException(nameof(combinator));
            _subsetsProvider = subsetsProvider ?? throw new ArgumentNullException(nameof(subsetsProvider));
            _cache = new Dictionary<int, HashSet<int>>();
        }

        #endregion

        #region INeedlessObjectsAnalyzer implementation

        /// <summary>
        /// Finds out if a given configuration contains needless objects that are not
        /// used in the definition of any one a provided geometrical object.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="geometricalObjects">The geometrical objects.</param>
        /// <returns>true, if there is a needless object; false otherwise.</returns>
        public bool ContainsNeedlessObjects(Configuration configuration, IEnumerable<GeometricalObject> geometricalObjects)
        {
            // Find the number of objects in the configuration
            var numberOfObjects = configuration.ConstructedObjects.Count + configuration.LooseObjects.Count;

            // Prepare dictionary for combinator, mapping id of geometrical object
            // to the options list
            var dictionaryForCombiner = geometricalObjects.ToDictionary(o => o.Id, FindAllOptions);

            // Let the combiner combine all possible definitions 
            foreach (var option in _combinator.Combine(dictionaryForCombiner))
            {
                // Find the number of used objects in this option
                var neededObjects = option.Values.SelectMany(set => set).Distinct().Count();

                // If it's the same as the number of all objects in the
                // configuration, then we have the winner
                if (neededObjects == numberOfObjects)
                    return false;
            }

            // Otherwise in any case, there is at least one unused object.
            return true;
        }

        /// <summary>
        /// Finds all possible sets of ids of objects that might be used to define 
        /// a given geometrical object.
        /// </summary>
        /// <param name="geometricalObject">The geometrical object.</param>
        /// <returns>All possible ids sets.</returns>
        private IEnumerable<HashSet<int>> FindAllOptions(GeometricalObject geometricalObject)
        {
            // If the internal configuration object is set, 
            // then we have one possible definition by it
            if (geometricalObject.ConfigurationObject != null)
            {
                // Let the helper method find the ids set
                yield return FindIdsOfNeededObjecs(geometricalObject.ConfigurationObject);
            }

            // If we have a point
            if (geometricalObject is PointObject)
            {
                // Then we can do anything else
                yield break;
            }

            // Otherwise we have a line or circle. In every case, the object is definable by points
            var objectWithPoints = (DefinableByPoints) geometricalObject;

            // Pull the points
            var points = objectWithPoints.Points.ToList();

            // Generate all possible pairs/triples of points (according to the type of the object)
            var definitions = _subsetsProvider.GetSubsets(points, objectWithPoints.NumberOfNeededPoints)
                    // And cast each set of points to the ids set needed to define them
                    .Select(option => option.SelectMany(p => FindIdsOfNeededObjecs(p.ConfigurationObject)).ToSet());

            // Now we just enumerate the definitions
            foreach (var definition in definitions)
            {
                yield return definition;
            }
        }

        /// <summary>
        /// Finds the ids of objects that are needed to define a given configuration object.
        /// </summary>
        /// <param name="configurationObject">The configuration object.</param>
        /// <returns>The ids set.</returns>
        private HashSet<int> FindIdsOfNeededObjecs(ConfigurationObject configurationObject)
        {
            // Pull id of the object
            var id = configurationObject.Id ?? throw new AnalyzerException("Id must be set");

            // Look up the id in the cache
            if (_cache.ContainsKey(id))
                return _cache[id];

            // If it's not there, we have to prepare a new one (with the objects id)
            var ids = new HashSet<int> {id};

            // And cache it
            _cache.Add(id, ids);

            // If the object is loose, we're done with it
            if (configurationObject is LooseConfigurationObject)
                return ids;

            // Otherwise we have a constructed object
            var contructedObject = (ConstructedConfigurationObject) configurationObject;

            // Let's prepare a local function to find ids of internal objects of an argument
            HashSet<int> HandleArgument(ConstructionArgument argument)
            {
                // If the argument is an objects argument
                if (argument is ObjectConstructionArgument objectArgument)
                {
                    // Then we call the upper method for its internal object
                    return FindIdsOfNeededObjecs(objectArgument.PassedObject);
                }

                // Otherwise we have a set argument
                var setArgument = (SetConstructionArgument) argument;

                // We call this function for internal arguments and merge the results
                return setArgument.PassedArguments.SelectMany(HandleArgument).ToSet();
            }

            // Now we simply merge the results from the arguments
            var argumentsIds = contructedObject.PassedArguments.SelectMany(HandleArgument);

            // Add it all to our ids set
            foreach (var idFromArgument in argumentsIds)
            {
                ids.Add(idFromArgument);
            }

            // And return it
            return ids;
        }

        #endregion
    }
}