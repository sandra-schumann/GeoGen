﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GeoGen.Core.Configurations;
using GeoGen.Core.Constructions.Arguments;
using GeoGen.Core.Utilities;
using GeoGen.Generator.ConstructingConfigurations.IdsFixing;
using GeoGen.Generator.ConstructingConfigurations.LeastConfigurationFinding;
using GeoGen.Generator.ConstructingObjects;
using GeoGen.Generator.ConstructingObjects.Arguments.Container;

namespace GeoGen.Generator.ConstructingConfigurations
{
    internal class ConfigurationConstructor : IConfigurationConstructor
    {
        private readonly ILeastConfigurationFinder _leastConfigurationFinder;

        private readonly IIdsFixerFactory _idsFixerFactory;

        private readonly IArgumentsListContainerFactory _argumentsListContainerFactory;

        public ConfigurationConstructor
        (
            ILeastConfigurationFinder leastConfigurationFinder,
            IIdsFixerFactory idsFixerFactory,
            IArgumentsListContainerFactory argumentsListContainerFactory
        )
        {
            _leastConfigurationFinder = leastConfigurationFinder ?? throw new ArgumentNullException(nameof(leastConfigurationFinder));
            _idsFixerFactory = idsFixerFactory ?? throw new ArgumentNullException(nameof(idsFixerFactory));
            _argumentsListContainerFactory = argumentsListContainerFactory ?? throw new ArgumentNullException(nameof(argumentsListContainerFactory));
        }

        public static Stopwatch s_balast = new Stopwatch();
        public static Stopwatch s_leastResolver = new Stopwatch();
        public static Stopwatch s_cloningConfig = new Stopwatch();
        public static Stopwatch s_arguments = new Stopwatch();
        public static Stopwatch s_typeMap = new Stopwatch();

        public ConfigurationWrapper ConstructWrapper(ConstructorOutput constructorOutput)
        {
            s_balast.Start();
            if (constructorOutput == null)
                throw new ArgumentNullException(nameof(constructorOutput));

            var configuration = constructorOutput.InitialConfiguration;

            var newObjects = constructorOutput.ConstructedObjects;

            var looseObjects = configuration.Configuration.LooseObjects;

            var allConstructedObjects = configuration.Configuration
                    .ConstructedObjects
                    .Union(newObjects)
                    .ToList();

            var currentConfiguration = new Configuration(looseObjects, allConstructedObjects);
            s_balast.Stop();
            s_leastResolver.Start();
            var leastResolver = _leastConfigurationFinder.FindLeastConfiguration(currentConfiguration);

            var idsFixer = _idsFixerFactory.CreateFixer(leastResolver);
            s_leastResolver.Stop();
            s_cloningConfig.Start();
            currentConfiguration = CloneConfiguration(currentConfiguration, idsFixer);
            s_cloningConfig.Stop();
            s_arguments.Start();
            var forbiddenArguments = CreateNewArguments(idsFixer, configuration.ForbiddenArguments, newObjects);
            s_arguments.Stop();
            s_typeMap.Start();
            var typeToObjectsMap = CreateObjectsMap(configuration, newObjects, idsFixer);
            s_typeMap.Stop();
            return new ConfigurationWrapper
            {
                Configuration = currentConfiguration,
                ForbiddenArguments = forbiddenArguments,
                ConfigurationObjectsMap = typeToObjectsMap
            };
        }

        private static ConfigurationObjectsMap CreateObjectsMap
        (
            ConfigurationWrapper configuration,
            IEnumerable<ConstructedConfigurationObject> newObjects,
            IIdsFixer idsFixer
        )
        {
            // Create a new map
            var objectsMap = new ConfigurationObjectsMap();

            // Add the old map (with the fixer selector)
            objectsMap.AddAll(configuration.ConfigurationObjectsMap, idsFixer.FixObject);

            // Add new objects (with the fixer selector)
            objectsMap.AddAll(newObjects, idsFixer.FixObject);

            return objectsMap;
        }

        private static Configuration CloneConfiguration(Configuration configuration, IIdsFixer idsFixer)
        {
            // Loose objects are going to be still the same
            var looseObjects = configuration.LooseObjects;

            // We let the fixer fix the constructed objects
            var constructedObjects = configuration.ConstructedObjects
                    .Select(idsFixer.FixObject)
                    .Cast<ConstructedConfigurationObject>()
                    .ToList();

            // And return the result
            return new Configuration(looseObjects, constructedObjects);
        }

        private Dictionary<int, IArgumentsListContainer> CreateNewArguments
        (
            IIdsFixer idsFixer,
            Dictionary<int, IArgumentsListContainer> forbiddenArguments,
            IEnumerable<ConstructedConfigurationObject> newObjects
        )
        {
            var result = new Dictionary<int, IArgumentsListContainer>();

            List<ConstructionArgument> FixArguments(IEnumerable<ConstructionArgument> arguments)
            {
                return arguments.Select(idsFixer.FixArgument).ToList();
            }

            foreach (var pair in forbiddenArguments)
            {
                var id = pair.Key;
                var newContainer = _argumentsListContainerFactory.CreateContainer();

                foreach (var arguments in pair.Value)
                {
                    var fixedArguments = FixArguments(arguments);
                    newContainer.AddArguments(fixedArguments);
                }

                result.Add(id, newContainer);
            }

            foreach (var constructedObject in newObjects)
            {
                var id = constructedObject.Construction.Id ?? throw new GeneratorException("Construction must have an id.");

                if (!result.ContainsKey(id))
                {
                    var newContainer = _argumentsListContainerFactory.CreateContainer();
                    result.Add(id, newContainer);
                }

                var fixedArguments = FixArguments(constructedObject.PassedArguments);
                result[id].AddArguments(fixedArguments);
            }

            return result;
        }
    }
}