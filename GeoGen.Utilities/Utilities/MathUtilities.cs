﻿using System;
using System.Collections.Generic;

namespace GeoGen.Utilities
{
    /// <summary>
    /// A static helper class for math functions.
    /// </summary>
    public static class MathUtilities
    {
        public static decimal Sqrt(decimal x)
        {
            if (x < 0) throw new OverflowException("Cannot calculate square root from a negative number");

            decimal current = (decimal)Math.Sqrt((double)x), previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + x / previous) / 2;
            }
            while (Math.Abs(previous - current) > 0);
            return current;
        }

        /// <summary>
        /// Solves a given quadratic equation. This method internally uses <see cref="RoundedDecimal"/>s.
        /// </summary>
        /// <param name="a">The a coefficient of the equation ax^2 + bx + c = 0.</param>
        /// <param name="b">The b coefficient of the equation ax^2 + bx + c = 0.</param>
        /// <param name="c">The c coefficient of the equation ax^2 + bx + c = 0.</param>
        /// <returns>The list of solutions. If there is no solution, an empty list.</returns>
        public static List<decimal> SolveQuadraticEquation(decimal a, decimal b, decimal c)
        {
            // Calculate and round the discriminant
            var d = (RoundedDecimal) (b * b - 4 * a * c);

            // If it less than zero, then we have no solutions
            if (d < RoundedDecimal.Zero)
                return new List<decimal>();

            // If it's exactly zero, then we have exactly one solution
            if (d == RoundedDecimal.Zero)
                return new List<decimal> {-b / (2 * a)};

            // Otherwise we have 2 solution. Let's calculate sqrt(d).
            var squareRoot = DecimalMath.Sqrt(d);

            // The solutions are then give by the formula (-b +- sqrt(d)) / (2a)
            // This can be easily concluded using the completion to square.
            var root1 = (-b - squareRoot) / (2 * a);
            var root2 = (-b + squareRoot) / (2 * a);

            // Finally we wrap the solutions as list.
            return new List<decimal> {root1, root2};
        }

        /// <summary>
        /// Converts a given angle to radius. 
        /// </summary>
        /// <param name="angleInDegrees">The angel in degrees.</param>
        /// <returns>The angel in radians.</returns>
        public static decimal ToRadians(decimal angleInDegrees)
        {
            return angleInDegrees * (DecimalMath.PI / 180);
        }
    }
}