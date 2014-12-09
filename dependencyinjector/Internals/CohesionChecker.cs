using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjector.Internals
{
    internal class CohesionChecker
    {
        InjectionsStorage _storage;
        ConstructorsManager _constructors;
        PropertiesManager _properties;

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
                if (!IsCovered(item.Value.Type))
                {
                    string error = string.Format("No Injection found for '{0}'", item.Value.Type);
                    throw new ArgumentException(error);
                }
            });
        }

        void CheckForCircularDependency()
        {
            Parallel.ForEach(_storage.Injections, item =>
            {
                CheckRequiredTypes(item.Key, item.Key);
            });
        }

        bool IsCovered(Type type)
        {
            List<Type> requiredTypes = GetAllRequiredIntertfaces(type);
            foreach (var neededType in requiredTypes)
            {
                if (!HasInjection(neededType))
                {
                    return false;
                }
            }
            return true;
        }

        bool HasInjection(Type type)
        {
            foreach (var item in _storage.Injections)
            {
                if (type == item.Key)
                {
                    return true;
                }
            }
            return false;
        }

        void CheckRequiredTypes(Type inter, Type baseInterface)
        {
            List<Type> requiredTypes = GetAllRequiredIntertfaces(ToBaseType(inter));
            ThrowIfContainsBaseType(requiredTypes, baseInterface);
            foreach (Type requiredType in requiredTypes)
            {
                CheckRequiredTypes(requiredType, baseInterface);
            }
        }

        void ThrowIfContainsBaseType(List<Type> types, Type baseType)
        {
            if (types.Contains(baseType))
            {
                string error = string.Format("Circular Dependency found for '{0}'", baseType);
                throw new ArgumentException(error);
            }
        }

        List<Type> GetAllRequiredIntertfaces(Type type)
        {
            var requiredTypes = new List<Type>();
            ConstructorInfo constructor = _constructors.GetConstructor(type);
            foreach (var parameter in constructor.GetParameters())
            {
                requiredTypes.Add(parameter.ParameterType);
            }

            var properties = _properties.GetSupportedProperties(type);
            foreach (var property in properties)
            {
                requiredTypes.Add(property.PropertyType);
            }

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
