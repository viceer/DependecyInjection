using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DependencyInjector.Internals
{
    internal class ObjectCreator
    {
        ConstructorsManager _constructors;
        PropertiesManager _properties;
        InjectionsStorage _injections;
        SingleInstancesManager _singleInstances;

        internal ObjectCreator(InjectionsStorage storage, ConstructorsManager constructors, PropertiesManager properties)
        {
            _injections = storage;
            _singleInstances = new SingleInstancesManager();
            _properties = properties;
            _constructors = constructors;
        }

        #region PUBLIC METHODS
        internal object Create(Type type)
        {
            ConstructorInfo constructor = _constructors.GetConstructor(type);
            List<object> parameters = CreateInjectableParamethers(constructor);
            var obj = Activator.CreateInstance(type, parameters.ToArray(), null);
            return obj;
        }

        internal object Create(Type type, dynamic constructorParamethers)
        {
            ConstructorInfo constructor = _constructors.GetConstructor(type);
            AnonymousTypesHelper.CheckThrow(constructor, constructorParamethers);
            List<object> parameters = CreateInjectableParamethers(constructor, constructorParamethers);
            var obj = Activator.CreateInstance(type, parameters.ToArray(), null);
            return obj;
        }

        internal void InjectProperties(Object obj)
        {
            Type type = obj.GetType();
            var properties = _properties.GetSupportedProperties(type);
            var values = CreateInjectableObjects(properties);
            SetProperties(obj, properties, values);
        }

        internal void InjectProperties(Object obj, dynamic customProperties)
        {
            Type type = obj.GetType();
            var supportedProperties = _properties.GetSupportedProperties(type);
            AnonymousTypesHelper.CheckThrow(supportedProperties, customProperties);
            Object[] values = CreateInjectableObjects(supportedProperties, customProperties);
            SetProperties(obj, supportedProperties, values);
        }
        #endregion

        object Get(Type type)
        {
            Injection injection = _injections.GetInjection(type);
            if (injection.SingleInstance)
            {
                object instance = null;
                if (_singleInstances.TryGetValue(type, out instance))
                {
                    return instance;
                }
                return CreateSingleton(type, injection);
            }
            else
            {
                Object obj = Create(injection.Type);
                InjectProperties(obj);
                return obj;
            }
        }

        Object _singletonCreationLocker = new Object();

        object CreateSingleton(Type type, Injection injection)
        {
            object instance = null;
            lock (_singletonCreationLocker)
            {
                if (_singleInstances.TryGetValue(type, out instance))
                {
                    return instance;
                }
                instance = Create(injection.Type);
                _singleInstances.RegisterSingleInstance(type, instance);
            }
            return instance;
        }

        void SetProperties(Object obj, List<PropertyInfo> properties, Object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                properties[i].SetValue(obj, values[i]);
            }
        }

        List<object> CreateInjectableParamethers(ConstructorInfo constructor)
        {
            var paramsList = new List<object>();
            ParameterInfo[] paramethers = constructor.GetParameters();
            for (int i = 0; i < paramethers.Length; i++)
            {
                paramsList.Add(Get(paramethers[i].ParameterType));
            }
            return paramsList;
        }

        List<object> CreateInjectableParamethers(ConstructorInfo constructor, dynamic constructorParamethers)
        {
            var paramsList = new List<object>();
            var paramethers = constructor.GetParameters();
            for (int i = 0; i < paramethers.Length; i++)
            {
                if (AnonymousTypesHelper.ShouldOverride(paramethers[i].Name, constructorParamethers))
                {
                    paramsList.Add(AnonymousTypesHelper.GetFromAnonymus(constructorParamethers, paramethers[i].Name));
                }
                else
                {
                    paramsList.Add(Get(paramethers[i].ParameterType));
                }
            }
            return paramsList;
        }

        Object[] CreateInjectableObjects(List<PropertyInfo> properties)
        {
            Object[] values = new Object[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                values[i] = Get(properties[i].PropertyType);
            }
            return values;
        }

        Object[] CreateInjectableObjects(List<PropertyInfo> properties, dynamic customProperties)
        {
            if (customProperties != null)
            {
                return CreateUpdateInjectableObjects(properties, customProperties);
            }
            else
            {
                return CreateInjectableObjects(properties);
            }
        }

        Object[] CreateUpdateInjectableObjects(List<PropertyInfo> properties, dynamic customProperties)
        {
            Object[] objects = new Object[properties.Count];
            for (int i = 0; i < objects.Length; i++)
            {
                if (AnonymousTypesHelper.ShouldOverride(properties[i].Name, customProperties))
                {
                    objects[i] = AnonymousTypesHelper.GetFromAnonymus(customProperties, properties[i].Name);
                }
                else
                {
                    objects[i] = Get(properties[i].PropertyType);
                }
            }
            return objects;
        }
    }
}
