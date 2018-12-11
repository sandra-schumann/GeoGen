﻿using System.Collections.Generic;
using System.Linq;
using GeoGen.Core;
using static GeoGen.Generator.Test.TestHelpers.ConfigurationObjects;

namespace GeoGen.Generator.Test.TestHelpers
{
    internal static class Configurations
    {
        public static ConfigurationWrapper Configuration(int npoints, int nlines, int ncircles)
        {
            var points = Objects(npoints, ConfigurationObjectType.Point);
            var lines = Objects(nlines, ConfigurationObjectType.Line, npoints + 1);
            var circles = Objects(ncircles, ConfigurationObjectType.Circle, npoints + nlines + 1);

            var objects = new List<LooseConfigurationObject>(points.Concat(lines).Concat(circles));

            var configuration = new Configuration(objects, new List<ConstructedConfigurationObject>());

            var map = new ConfigurationObjectsMap(objects);

            return new ConfigurationWrapper
            {
                WrappedConfiguration = configuration,
            };
        }

        public static Configuration AsConfiguration(IEnumerable<LooseConfigurationObject> objects)
        {
            return new Configuration(new List<LooseConfigurationObject>(objects), new List<ConstructedConfigurationObject>());
        }
    }
}