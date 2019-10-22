﻿using GeoGen.Core;
using GeoGen.Utilities;
using System;

namespace GeoGen.Generator
{
    /// <summary>
    /// Represents an input for the <see cref="IGenerator"/>.
    /// </summary>
    public class GeneratorInput
    {
        #region Public properties

        /// <summary>
        /// The initial configuration from which the generation process starts. 
        /// </summary>
        public Configuration InitialConfiguration { get; }

        /// <summary>
        /// The constructions that are used to create new objects for configurations.
        /// </summary>
        public IReadOnlyHashSet<Construction> Constructions { get; }

        /// <summary>
        /// The number of iterations that are to be performed by the generator.
        /// </summary>
        public int NumberOfIterations { get; }

        /// <summary>
        /// Gets or sets the function that is applied on a <see cref="ConstructedConfigurationObject"/>
        /// that is about to be added to a given configuration. It should return true if and only if
        /// this object is correct and should be added to the configuration.
        /// </summary>
        public Func<ConstructedConfigurationObject, bool> ObjectFilter { get; }

        /// <summary>
        /// Gets or sets the function that is applied on a valid <see cref="GeneratedConfiguration"/>.
        /// It should return true if and only if this configuration should be extended in the next iteration.
        /// </summary>
        public Func<GeneratedConfiguration, bool> ConfigurationFilter { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorInput"/> class.
        /// </summary>
        /// <param name="initialConfiguration">The initial configuration from which the generation process starts. </param>
        /// <param name="constructions">The constructions that are used to create new objects for configurations.</param>
        /// <param name="numberOfIterations">The number of iterations that are to be performed by the generator.</param>
        /// <param name="objectFilter">The function for filtrating constructed objects.</param>
        /// <param name="configurationFilter">The function for filtrating generated configurations.</param>
        public GeneratorInput(Configuration initialConfiguration,
                              IReadOnlyHashSet<Construction> constructions,
                              int numberOfIterations,
                              Func<ConstructedConfigurationObject, bool> objectFilter,
                              Func<GeneratedConfiguration, bool> configurationFilter)
        {
            InitialConfiguration = initialConfiguration ?? throw new ArgumentNullException(nameof(initialConfiguration));
            Constructions = constructions ?? throw new ArgumentNullException(nameof(constructions));
            NumberOfIterations = numberOfIterations;
            ObjectFilter = objectFilter ?? throw new ArgumentNullException(nameof(objectFilter));
            ConfigurationFilter = configurationFilter ?? throw new ArgumentNullException(nameof(configurationFilter));
        }

        #endregion
    }
}