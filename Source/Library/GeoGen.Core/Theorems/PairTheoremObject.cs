﻿using GeoGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoGen.Core
{
    /// <summary>
    /// Represents a theorem object that consists of two unordered theorem objects of some type.
    /// </summary>
    public abstract class PairTheoremObject : TheoremObject
    {
        #region Public properties

        /// <summary>
        /// Gets the first object of this pair.
        /// </summary>
        public TheoremObject Object1 { get; }

        /// <summary>
        /// Gets the second object of this pair.
        /// </summary>
        public TheoremObject Object2 { get; }

        /// <summary>
        /// Gets the inner objects of the pair object as a set.
        /// </summary>
        public IReadOnlyHashSet<TheoremObject> ObjectsSet { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PairTheoremObject"/> class.
        /// </summary>
        /// <param name="object1">The first object of the pair.</param>
        /// <param name="object2">The second object of the pair.</param>
        protected PairTheoremObject(TheoremObject object1, TheoremObject object2)
        {
            // Set the objects
            Object1 = object1 ?? throw new ArgumentNullException(nameof(object1));
            Object2 = object2 ?? throw new ArgumentNullException(nameof(object2));

            // Create the objects set
            ObjectsSet = new HashSet<TheoremObject> { Object1, Object2 }.AsReadOnly();
        }

        #endregion

        #region Public abstract methods implementation

        /// <inheritdoc/>
        public override IEnumerable<ConfigurationObject> GetInnerConfigurationObjects()
            // Return the merged inner objects
            => Object1.GetInnerConfigurationObjects().Concat(Object2.GetInnerConfigurationObjects());

        #endregion

        #region Protected helper methods

        /// <summary>
        /// Remaps the inner objects with respect to the provided mapping. If either
        /// of the objects cannot be mapped, then returns the default value.
        /// </summary>
        /// <param name="mapping">The dictionary representing the mapping.</param>
        /// <param name="flattenObjectsFromPoints">Indicates whether explicit objects LineFromPoints or Circumcircle should be made implicit.</param>
        /// <returns>The remapped objects, if mapping can be done; (null, null) otherwise.</returns>
        protected (TheoremObject, TheoremObject) RemapObjects(IReadOnlyDictionary<ConfigurationObject, ConfigurationObject> mapping, bool flattenObjectsFromPoints = false)
        {
            // Map particular objects
            var mappedObject1 = Object1.Remap(mapping, flattenObjectsFromPoints);
            var mappedObject2 = Object2.Remap(mapping, flattenObjectsFromPoints);

            // Return tuple only if none of them is null
            return mappedObject1 != null && mappedObject2 != null ? (mappedObject1, mappedObject2) : default;
        }

        #endregion

        #region HashCode and Equals

        /// <inheritdoc/>
        public override int GetHashCode() => ObjectsSet.GetHashCode();

        /// <inheritdoc/>
        public override bool Equals(object otherObject)
            // Either the references are equal
            => this == otherObject
                // Or the object is not null
                || otherObject != null
                // And it is a pair object
                && otherObject is PairTheoremObject pairObject
                // And their object sets are equal
                && ObjectsSet.Equals(pairObject.ObjectsSet);

        #endregion

        #region Debug-only to string

#if DEBUG

        /// <inheritdoc/>
        public override string ToString() => new[] { Object1, Object2 }.Select(theoremObject => theoremObject.ToString()).Ordered().ToJoinedString();

#endif

        #endregion
    }
}