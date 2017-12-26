﻿using System;
using System.Collections.Generic;
using System.Linq;
using GeoGen.Analyzer.Objects;
using GeoGen.Analyzer.Theorems;
using GeoGen.Core.Configurations;
using GeoGen.Core.Theorems;
using GeoGen.Core.Utilities;
using GeoGen.Utilities;

namespace GeoGen.Analyzer
{
    /// <summary>
    /// A default implementation of <see cref="IGradualAnalyzer"/>.
    /// This class is thread-safe.
    /// </summary>
    internal sealed class GradualAnalyzer : IGradualAnalyzer
    {
        #region Private fields

        private readonly ITheoremsVerifier _verifier;

        private readonly ITheoremsContainer _container;

        #endregion

        #region Constructor

        public GradualAnalyzer(ITheoremsVerifier verifier, ITheoremsContainer container)
        {
            _verifier = verifier ?? throw new ArgumentNullException(nameof(verifier));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #endregion

        #region IGradualAnalyzer methods

        /// <summary>
        /// Analyses a given configuration provided as two lists, or old and new configuration
        /// objects.
        /// </summary>
        /// <param name="oldObjects">The old objects.</param>
        /// <param name="newObjects">The new objects.</param>
        /// <returns>The analyzer output.</returns>
        public List<Theorem> Analyze(List<ConfigurationObject> oldObjects, List<ConstructedConfigurationObject> newObjects)
        {
            if (oldObjects == null)
                throw new ArgumentNullException(nameof(oldObjects));

            if (newObjects == null)
                throw new ArgumentNullException(nameof(newObjects));

            return _verifier.FindTheorems(oldObjects, newObjects)
                    .Where(theorem => !_container.Contains(theorem))
                    .ToList();
        }

        #endregion
    }
}