﻿namespace GeoGen.Analyzer
{
    /// <summary>
    /// Represents a factory for creating an implementions of <see cref="IObjectsContainersManager"/>.
    /// NOTE: The implementation will be auto-generated by NInject. 
    /// </summary>
    public interface IObjectsContainersManagerFactory
    {
        /// <summary>
        /// Creates a new manager.
        /// </summary>
        /// <returns>The manager.</returns>
        IObjectsContainersManager CreateContainersManager();
    }
}