﻿using System;
using System.Collections.Generic;
using System.Linq;
using GeoGen.Analyzer.AnalyticalGeometry;
using GeoGen.Analyzer.Objects;
using GeoGen.Core.Constructions.Arguments;
using GeoGen.Core.Constructions.PredefinedConstructions;
using GeoGen.Core.Theorems;

namespace GeoGen.Analyzer.Constructing.PredefinedConstructors
{
    internal class InteresectionConstructor : IPredefinedConstructor
    {
        public Type PredefinedConstructionType { get; } = typeof(Intersection);

        public ConstructorOutput Apply(IReadOnlyList<ConstructionArgument> arguments, IObjectsContainer container)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            try
            {
                var setArgument1 = (SetConstructionArgument) arguments[0];
                var setArgument2 = (SetConstructionArgument) arguments[1];

                var passedPoints1 = setArgument1.PassedArguments.ToList();
                var passedPoints2 = setArgument2.PassedArguments.ToList();

                var obj1 = ((ObjectConstructionArgument) passedPoints1[0]).PassedObject;
                var obj2 = ((ObjectConstructionArgument) passedPoints1[1]).PassedObject;
                var obj3 = ((ObjectConstructionArgument) passedPoints2[0]).PassedObject;
                var obj4 = ((ObjectConstructionArgument) passedPoints2[1]).PassedObject;

                var point1 = container.Get<Point>(obj1);
                var point2 = container.Get<Point>(obj2);
                var point3 = container.Get<Point>(obj3);
                var point4 = container.Get<Point>(obj4);

                var result = AnalyticalHelpers.IntersectionOfLines(new List<Point> {point1, point2, point3, point4});

                if (result == null)
                    return null;

                var objects1 = new List<TheoremObject>
                {
                    new SingleTheoremObject(point1.ConfigurationObject),
                    new SingleTheoremObject(point2.ConfigurationObject),
                    new SingleTheoremObject(result.ConfigurationObject)
                };
                var objects2 = new List<TheoremObject>
                {
                    new SingleTheoremObject(point3.ConfigurationObject),
                    new SingleTheoremObject(point4.ConfigurationObject),
                    new SingleTheoremObject(result.ConfigurationObject)
                };

                var collinearityTheorem1 = new Theorem(TheoremType.CollinearPoints, objects1);
                var collinearityTheorem2 = new Theorem(TheoremType.CollinearPoints, objects2);

                return new ConstructorOutput
                {
                    Objects = new List<GeometricalObject> {result},
                    Theorems = new List<Theorem> {collinearityTheorem1, collinearityTheorem2}
                };
            }
            catch (Exception)
            {
                throw new AnalyzerException("Incorrect arguments.");
            }
        }
    }
}