﻿using System.Collections.Generic;
using GeoGen.Core.Configurations;

namespace GeoGen.Generator
{
    /// <summary>
    /// Represents a container for <see cref="ConfigurationObject"/>. It's 
    /// supposed to take care of having each object exactly once. It implements
    /// the <see cref="IEnumerable{T}"/> interface, where T is <see cref="ConfigurationObject"/>.
    /// </summary>
    internal interface IConfigurationObjectsContainer : IEnumerable<ConfigurationObject>
    {
        /// <summary>
        /// Gets the loose objects that are present in the container. 
        /// </summary>
        IEnumerable<LooseConfigurationObject> LooseObjects { get; }

        /// <summary>
        /// Gets the object with a given id. Throws an <see cref="KeyNotFoundException"/>, 
        /// if not present.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The configuration object with the given id.</returns>
        ConfigurationObject this[int id] { get; }

        /// <summary>
        /// Adds a given constructed configuration object to the container. 
        /// The current id of the object will be ignored. If an equal version 
        /// of the object  is present in the container, it will return instance of 
        /// this internal object. Otherwise it will return this object with set id.
        /// </summary>
        /// <param name="constructedObject">The constructed configuration object.</param>
        /// <returns>The identified version of the constructed configuration object.</returns>
        ConstructedConfigurationObject Add(ConstructedConfigurationObject constructedObject);
    }
}