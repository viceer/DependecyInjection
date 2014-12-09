using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DependencyInjector.Internals
{
    internal class CohesionChecker
    {
        readonly InjectionsStorage _storage;
        readonly ConstructorsManager _constructors;
        readonly PropertiesManager _properties;

        internal CohesionChecker(InjectionsStorage storage, ConstructorsManager constructors, PropertiesManager properties)
        {
            _storage = storage;
            _constructors = constructors;
            _properties = properties;
        }

        internal void CheckCohesion()
        {          
            CheckForCircularDependency();
            CheckAllInjectionsAreCovered();
        }

        void CheckAllInjectionsAreCovered()
        {
            Parallel.ForEach (_storage.Injections, item =>
            {
                if (IsCovered(item.Value.Type)) return;
                var error = string.Format("No Injection found for '{0}'", item.Value.Type);
                throw new ArgumentException(error);
            });
        }

        void CheckForCircularDependency()
        {
            Parallel.ForEach(_storage.Injections, item => CheckRequiredTypes(item.Key, item.Key));
        }

        bool IsCovered(Type type)
        {
            var requiredTypes = GetAllRequiredIntertfaces(type);
            return requiredTypes.All(HasInjection);
        }

        bool HasInjection(Type type)
        {
            return _storage.Injections.Any(item => type == item.Key);
        }

        void CheckRequiredTypes(Type inter, Type baseInterface)
        {
            var requiredTypes = GetAllRequiredIntertfaces(ToBaseType(inter));
            ThrowIfContainsBaseType(requiredTypes, baseInterface);
            foreach (var requiredType in requiredTypes)
            {
                CheckRequiredTypes(requiredType, baseInterface);
            }
        }

        void ThrowIfContainsBaseType(List<Type> types, Type baseType)
        {
            if (!types.Contains(baseType)) return;
            var error = string.Format("Circular Dependency found for '{0}'", baseType);
            throw new ArgumentException(error);
        }

        List<Type> GetAllRequiredIntertfaces(Type type)
        {
            ConstructorInfo constructor = _constructors.GetConstructor(type);
            var requiredTypes = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToList();
            var properties = _properties.GetSupportedProperties(type);
            requiredTypes.AddRange(properties.Select(property => property.PropertyType));

            return requiredTypes;
        }

        Type ToBaseType(Type type)
        {
            if (type.IsInterface)
            {
                type = _storage.GetInjection(type).Type;
            }
            return type;
        }
    }
}
