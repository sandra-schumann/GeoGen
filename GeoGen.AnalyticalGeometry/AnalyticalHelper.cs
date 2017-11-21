﻿using System;
using System.Collections.Generic;
using System.Linq;
using GeoGen.AnalyticalGeometry.AnalyticalObjects;
using GeoGen.Core.Utilities;

namespace GeoGen.AnalyticalGeometry
{
    /// <summary>
    /// A default implementation of <see cref="IAnalyticalHelper"/>.
    /// </summary>
    internal class AnalyticalHelper : IAnalyticalHelper
    {
        /// <summary>
        /// Finds all intersections of given analytical objects. They must not
        /// contain <see cref="Point"/>s and duplicate objects.
        /// </summary>
        /// <param name="inputObjects">The input objects.</param>
        /// <returns>The set of intersections. An empty set, if there's none.</returns>
        public HashSet<Point> Intersect(IEnumerable<IAnalyticalObject> inputObjects)
        {
            // Local function to find all intersections of two given analytical objects.
            HashSet<Point> Intersect(IAnalyticalObject o1, IAnalyticalObject o2)
            {
                // Safely cast o1 to nullable Line and Circle
                var o1Line = o1 as Line?;
                var o1Circle = o1 as Circle?;

                // The same goes with for o2
                var o2Line = o2 as Line?;
                var o2Circle = o2 as Circle?;

                // If we have two lines
                if (o1Line != null && o2Line != null)
                {
                    // Then we call the intersection method
                    var result = o1Line.Value.IntersectionWith(o2Line.Value);

                    // And if the intersection is null, return an empty set, otherwise the set
                    // containing the result
                    return result == null ? new HashSet<Point>() : new HashSet<Point> {result.Value};
                }

                // If we have two circles
                if (o1Circle != null && o2Circle != null)
                {
                    // Intersect the circles and cast the result to set
                    return o1Circle.Value.IntersectWith(o2Circle.Value).ToSet();
                }

                // Otherwise we have a line and a circle.
                Line line;
                Circle circle;

                // Find the line.
                if (o1Line != null)
                    line = o1Line.Value;
                else if (o2Line != null)
                    line = o2Line.Value;
                else
                    throw new Exception("Unhandled analytical object");

                // Find the circle.
                if (o1Circle != null)
                    circle = o1Circle.Value;
                else if (o2Circle != null)
                    circle = o2Circle.Value;
                else
                    throw new Exception("Unhandled analytical object");

                // And finally intersect the circle and the line
                return circle.IntersectWith(line).ToSet();
            }

            // Enumerate input objects
            var originalObjects = inputObjects.ToList();

            // Enumerate distinct objects
            var distintObjects = originalObjects.Distinct().ToList();

            // Make sure we have don't have duplicates
            if (originalObjects.Count != distintObjects.Count)
                throw new ArgumentException("Duplicate objects");

            // Check if we have enough objects
            if (distintObjects.Count <= 1)
                throw new ArgumentException("There must be at least two distinct objects to intersect.");

            // Pull first two objects
            var first = distintObjects[0];
            var second = distintObjects[1];

            // Make sure we don't have null objects
            if (first == null || second == null)
                throw new ArgumentException("There is a null object.");

            // Make sure we don't have points
            if (first is Point || second is Point)
                throw new ArgumentException("Analytical object can't be a point");

            // Intersect them
            var points = Intersect(first, second);

            // Iterate over remaining objects
            foreach (var analyticalObject in distintObjects.Skip(2))
            {
                // Make sure that object is not null
                if (analyticalObject == null)
                    throw new ArgumentException("There is a null object.");

                // Make sure that object is not a point
                if (analyticalObject is Point)
                    throw new ArgumentException("Analytical object can't be a point");

                // From current intersections remove the ones that doesn't lie on this object
                points.RemoveWhere(point => !LiesOn(analyticalObject, point));
            }

            // Return the resulting points
            return points;
        }

        /// <summary>
        /// Checks if a given point lies on a given analytical object. The object
        /// must not be a points.
        /// </summary>
        /// <param name="analyticalObject">The analytical object.</param>
        /// <param name="point">The point.</param>
        /// <returns>true, if the point lies on the object, false otherwise.</returns>
        public bool LiesOn(IAnalyticalObject analyticalObject, Point point)
        {
            if (analyticalObject == null)
                throw new ArgumentNullException(nameof(analyticalObject));

            if (analyticalObject is Point)
                throw new ArgumentException("Analytical object can't be a point");

            if (analyticalObject is Line line)
                return line.Contains(point);

            if (analyticalObject is Circle circle)
                return circle.Contains(point);

            throw new Exception("Unhandled case");
        }
    }
}