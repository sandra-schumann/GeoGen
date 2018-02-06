using System;
using System.Collections.Generic;

namespace GeoGen.Analyzer
{
    /// <summary>
    /// Represents a manager of all available <see cref="IObjectsContainer"/>s. 
    /// It takes care of resolving possible inconsistencies (see the documentation of 
    /// <see cref="InconsistentContainersException"/>).
    /// </summary>
    internal interface IObjectsContainersManager : IEnumerable<IObjectsContainer>
    {
        /// <summary>
        /// Performs a given function and handles the <see cref="InconsistentContainersException"/>.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="function">The function.</param>
        /// <returns>The result of the function.</returns>
        T ExecuteAndResolvePossibleIncosistencies<T>(Func<T> function);
    }
}