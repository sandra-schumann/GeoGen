using System.Collections.Generic;
using GeoGen.AnalyticalGeometry;
using GeoGen.Core;

namespace GeoGen.Analyzer
{
    /// <summary>
    /// Represents a container that holds <see cref="GeometricalObject"/>s. This container
    /// is responsible for creating them and mapping them to <see cref="AnalyticalObject"/>s.
    /// </summary>
    internal interface IContextualContainer
    {
        /// <summary>
        /// Gets the geometrical objects of given type from the container.s
        /// </summary>
        /// <typeparam name="T">The type of objects.</typeparam>
        /// <returns>The objects.</returns>
        IEnumerable<T> GetGeometricalObjects<T>() where T : GeometricalObject;

        /// <summary>
        /// Gets the analytical representation of a given geometrical object
        /// in a given objects container.
        /// </summary>
        /// <param name="geometricalObject">The geometrical object.</param>
        /// <param name="objectsContainer">The objects container.</param>
        /// <returns>The analytical object.</returns>
        AnalyticalObject GetAnalyticalObject(GeometricalObject geometricalObject, IObjectsContainer objectsContainer);

        /// <summary>
        /// Finds out if a given object is present in the container.
        /// </summary>
        /// <param name="configurationObject">The configuration object.</param>
        /// <returns>true, if the containers contains the object; false otherwise.</returns>
        bool Contains(ConfigurationObject configurationObject);
    }
}