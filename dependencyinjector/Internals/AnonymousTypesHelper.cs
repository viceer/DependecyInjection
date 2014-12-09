using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DependencyInjector.Internals
{
    internal static class AnonymousTypesHelper
    {
        internal static Object GetFromAnonymus(dynamic anonymusObject, string propertyName)
        {
            var properties = anonymusObject.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name == propertyName)
                {
                    return properties[i].GetValue(anonymusObject);
                }
            }
            return null;
        }

        internal static bool ShouldOverride(string name, dynamic anonymousObject)
        {
            var properties = anonymousObject.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        internal static void CheckThrow(List<PropertyInfo> supportedProperties, dynamic customProperties)
        {
            var customProps = customProperties.GetType().GetProperties();
            for (int i = 0; i < customProps.Length; i++)
            {
                int index = -1; ;
                for (int j = 0; j < supportedProperties.Count; j++)
                {
                    if (customProps[i].Name == supportedProperties[j].Name)
                    {
                        index = j;
                        CheckThrowTypeExeption(customProps[i], supportedProperties[j]);
                    }
                }
                if (index == -1)
                {
                    string error = string.Format("There is no Property called '{0}'.", customProps[i].Name);
                    throw new ArgumentException(error);
                }
            }
        }

        internal static void CheckThrow(ConstructorInfo constructor, dynamic constructorParamethers)
        {
            var properties = constructorParamethers.GetType().GetProperties();
            var paramethers = constructor.GetParameters();
            for (int i = 0; i < properties.Length; i++)
            {
                int index = -1; ;
                for (int j = 0; j < paramethers.Length; j++)
                {
                    if (properties[i].Name == paramethers[j].Name)
                    {
                        index = j;
                        CheckThrowTypeExeption(properties[i], paramethers[j]);
                    }
                }
                if (index == -1)
                {
                    string error = string.Format("There is no paramether called '{0}'.", properties[i].Name);
                    throw new ArgumentException(error);
                }
            }
        }

        internal static void CheckThrowTypeExeption(PropertyInfo property, ParameterInfo paramether)
        {
            if (!paramether.ParameterType.IsAssignableFrom(property.PropertyType))
            {
                string error = string.Format(
                    "Paramether '{0}' is not assignable from '{1}' type",
                    paramether.Name,
                    property.PropertyType);
                throw new ArgumentException(error);
            }
        }

        internal static void CheckThrowTypeExeption(PropertyInfo customProperty, PropertyInfo property)
        {
            if (!property.PropertyType.IsAssignableFrom(customProperty.PropertyType))
            {
                string error = string.Format(
                    "Property '{0}' is not assignable from '{1}' type",
                    property.Name,
                    customProperty.PropertyType);
                throw new ArgumentException(error);
            }
        }
    }
}
