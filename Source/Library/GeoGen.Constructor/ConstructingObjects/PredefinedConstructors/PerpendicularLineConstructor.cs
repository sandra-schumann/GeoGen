﻿using GeoGen.AnalyticGeometry;
using GeoGen.Core;

namespace GeoGen.Constructor
{
    /// <summary>
    /// The <see cref="IObjectConstructor"/> for <see cref="PredefinedConstructionType.PerpendicularLine"/>>.
    /// </summary>
    public class PerpendicularLineConstructor : PredefinedConstructorBase
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleWithCenterThroughPointConstructor"/> class.
        /// </summary>
        /// <param name="tracer">The tracer for unexpected analytic exceptions.</param>
        public PerpendicularLineConstructor(IConstructorFailureTracer tracer)
            : base(tracer)
        {
        }

        #endregion

        #region Construct implementation

        /// <inheritdoc/>
        protected override IAnalyticObject Construct(IAnalyticObject[] input)
        {
            // Get the point and the line
            var A = (Point)input[0];
            var l = (Line)input[1];

            // Construct the result
            return l.PerpendicularLineThroughPoint(A);
        }

        #endregion
    }
}