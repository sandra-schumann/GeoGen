﻿namespace GeoGen.Generator
{
    /// <summary>
    /// Represents a factory for creating <see cref="IAutocacheFullObjectToStringConverter"/>s
    /// from a given <see cref="IObjectIdResolver"/>. NOTE: The implementation will be auto-generated by NInject. 
    /// </summary>
    internal interface IAutocacheFullObjectToStringConverterFactory
    {
        /// <summary>
        /// Creates an <see cref="IAutocacheFullObjectToStringConverter"/> that
        /// uses a given <see cref="IObjectIdResolver"/>.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <returns>The converter.</returns>
        IAutocacheFullObjectToStringConverter Create(IObjectIdResolver resolver);
    }
}