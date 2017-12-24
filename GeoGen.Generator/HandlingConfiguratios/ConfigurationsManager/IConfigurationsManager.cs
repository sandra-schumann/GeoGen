﻿using System.Collections.Generic;
using GeoGen.Core.Configurations;

namespace GeoGen.Generator
{
    /// <summary>
    /// Represents a container for all generated configurations. The configurations
    /// can be submitted to the container from the <see cref="ConstructorOutput"/>
    /// and it is supposed to take care of processing the output and updating the
    /// <see cref="CurrentLayer"/> property.
    /// </summary>
    internal interface IConfigurationsManager
    {
        /// <summary>
        /// Gets the current layer of unprocessed configurations
        /// </summary>
        List<ConfigurationWrapper> CurrentLayer { get; }

        /// <summary>
        /// Processes a new layer of constructors outputs and lazily returns the configurations.
        /// </summary>
        /// <param name="output">The new layer output.</param>
        /// <returns>The configuration wrappers.</returns>
        IEnumerable<ConfigurationWrapper> AddLayer(IEnumerable<ConstructorOutput> output);

        /// <summary>
        /// Forbids all configurations that contains a given configuration object.
        /// Use cases for these are when we have a non-constructible or duplicate objects.
        /// </summary>
        /// <param name="configurationObject">The configuration object.</param>
        void ForbidConfigurationsContaining(ConfigurationObject configurationObject);
    }
}