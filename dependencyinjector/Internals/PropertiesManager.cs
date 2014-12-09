using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DependencyInjector.Attributes;
using System.Collections.Concurrent;

namespace DependencyInjector.Internals
{
    internal class PropertiesManager
    {
        ConcurrentDictionary<Type, List<PropertyInfo>> _cacheSupportedProperties = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        InjectionsStorage _injections;

        internal PropertiesManager(InjectionsStorage storage)
        {
            _injections = storage;
        }

        internal List<PropertyInfo> GetSupportedProperties(Type type)
        {
            List<PropertyInfo> supportedProperties = null;
            if (_cacheSupportedProperties.TryGetValue(type, out supportedProperties))
            {
                return supportedProperties;
            }
            supportedProperties = CreateSupportedProperties(type);
            _cacheSupportedProperties.GetOrAdd(type, supportedProperties);
            return supportedProperties;
        }

        bool IsSupportedInjection(PropertyInfo property)
        {
            if (property.GetSetMethod(false) == null)
            {
                return false;
            }
            if (!property.PropertyType.IsInterface)
            {
                return false;
            }
            return _injections.Contains(property.PropertyType);
        }
        
        List<PropertyInfo> CreateSupportedProperties(Type type)
        {
            return type.GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(Injectable)))
                .ToList();
        }
    }
}
