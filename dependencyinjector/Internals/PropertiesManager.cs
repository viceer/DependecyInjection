using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DependencyInjector.Attributes;
using System.Collections.Concurrent;

namespace DependencyInjector.Internals
{
    internal class PropertiesManager
    {
        readonly ConcurrentDictionary<Type, List<PropertyInfo>> _cacheSupportedProperties = new ConcurrentDictionary<Type, List<PropertyInfo>>();

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

        static List<PropertyInfo> CreateSupportedProperties(Type type)
        {
            return type.GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(Injectable)))
                .ToList();
        }
    }
}
