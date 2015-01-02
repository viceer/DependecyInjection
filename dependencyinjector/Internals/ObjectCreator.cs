using System;
using System.Collections.Generic;
using System.Reflection;

namespace DependencyInjector.Internals
{
    internal class ObjectCreator : IDisposable
    {
        readonly ConstructorsManager _constructors;
        readonly PropertiesManager _properties;
        readonly InjectionsStorage _injections;
        readonly SingleInstancesManager _singleInstances;

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
            var constructor = _constructors.GetConstructor(type);
            var parameters = CreateInjectableParamethers(constructor);
            var obj = Activator.CreateInstance(type, parameters.ToArray(), null);
            return obj;
        }

        internal object Create(Type type, dynamic constructorParamethers)
        {
            var constructor = _constructors.GetConstructor(type);
            AnonymousTypesHelper.CheckThrow(constructor, constructorParamethers);
            List<object> parameters = CreateInjectableParamethers(constructor, constructorParamethers);
            var obj = Activator.CreateInstance(type, parameters.ToArray(), null);
            return obj;
        }

        internal void InjectProperties(Object obj)
        {
            var type = obj.GetType();
            var properties = _properties.GetSupportedProperties(type);
            var values = CreateInjectableObjects(properties);
            SetProperties(obj, properties, values);
        }

        internal void InjectProperties(Object obj, dynamic customProperties)
        {
            var type = obj.GetType();
            var supportedProperties = _properties.GetSupportedProperties(type);
            AnonymousTypesHelper.CheckThrow(supportedProperties, customProperties);
            Object[] values = CreateInjectableObjects(supportedProperties, customProperties);
            SetProperties(obj, supportedProperties, values);
        }
        #endregion

        internal object Get(Type type, dynamic constructorParamethers = null, dynamic customProperties = null)
        {
            var obj = GetSingleton(type);
            if (obj != null) { return obj; }
            Type typeToCreate = type.IsInterface ? _injections.GetInjection(type).Type : type;

            if (constructorParamethers == null)
            {
                obj = Create(typeToCreate);
            }
            else
            {
                obj = Create(typeToCreate, constructorParamethers);
            }

            if (customProperties == null)
            { 
                InjectProperties(obj); 
            }
            else 
            { 
                InjectProperties(obj, customProperties); 
            }

            return obj;
        }

        object GetSingleton(Type type)
        {
            if (_injections.IsIsngleton(type))
            {
                object instance = null;
                return _singleInstances.TryGetValue(type, out instance) ? instance : CreateSingleton(type);
            }
            return null;
        }

        readonly Object _singletonCreationLocker = new Object();

        object CreateSingleton(Type type)
        {
            object instance;
            lock (_singletonCreationLocker)
            {
                if (_singleInstances.TryGetValue(type, out instance))
                {
                    return instance;
                }
                var injection = _injections.GetInjection(type);
                instance = Create(injection.Type);
                System.Threading.Thread.MemoryBarrier();
                _singleInstances.RegisterSingleInstance(type, instance);
            }
            return instance;
        }

        static void SetProperties(Object obj, List<PropertyInfo> properties, Object[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                properties[i].SetValue(obj, values[i]);
            }
        }

        List<object> CreateInjectableParamethers(ConstructorInfo constructor)
        {
            var paramsList = new List<object>();
            var paramethers = constructor.GetParameters();
            for (var i = 0; i < paramethers.Length; i++)
            {
                paramsList.Add(Get(paramethers[i].ParameterType));
            }
            return paramsList;
        }

        List<object> CreateInjectableParamethers(ConstructorInfo constructor, dynamic constructorParamethers)
        {
            var paramsList = new List<object>();
            var paramethers = constructor.GetParameters();
            for (var i = 0; i < paramethers.Length; i++)
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
            var values = new Object[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                values[i] = Get(properties[i].PropertyType);
            }
            return values;
        }

        Object[] CreateInjectableObjects(List<PropertyInfo> properties, dynamic customProperties)
        {
            return customProperties != null ? CreateUpdateInjectableObjects(properties, customProperties) : CreateInjectableObjects(properties);
        }

        Object[] CreateUpdateInjectableObjects(List<PropertyInfo> properties, dynamic customProperties)
        {
            var objects = new Object[properties.Count];
            for (var i = 0; i < objects.Length; i++)
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

        public void Dispose()
        {
            _singleInstances.Dispose();
        }
    }
}
