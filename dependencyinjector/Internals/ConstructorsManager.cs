using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Concurrent;

namespace DependencyInjector.Internals
{
    internal class ConstructorsManager
    {
        ConcurrentDictionary<Type, ConstructorInfo> _cacheConstructors = new ConcurrentDictionary<Type, ConstructorInfo>();
        InjectionsStorage _injections;

        internal ConstructorsManager(InjectionsStorage storage)
        {
            _injections = storage;
        }

        internal ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo constructor = null;
            type = ToInjectableType(type);
            if (_cacheConstructors.TryGetValue(type, out constructor))
            {
                return constructor;
            }
            constructor = CreateConstructor(type);
            _cacheConstructors.GetOrAdd(type, constructor);
            return constructor;
        }

        ConstructorInfo CreateConstructor(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            int index = -1;
            for (int i = 0; i < constructors.Length; i++)
            {
                if (constructors[i].GetParameters().Length != 0)
                {
                    if (index != -1)
                    {
                        throw new AmbiguousMatchException(String.Format("Multiple constructors with parameters found for {0} type.", type));
                    }
                    index = i;
                }
            }
            ConstructorInfo constructor = constructors[(index != -1) ? index : 0];
            if (!IsAplicable(constructor))
            {
                throw new ArgumentException(String.Format("No aprpriate constructor found for {0} type.", type));
            }
            return constructor;
        }

        bool IsAplicable(ConstructorInfo constructor)
        {
            var paramethers = constructor.GetParameters();
            for (int i = 0; i < paramethers.Length; i++)
            {
                if (!_injections.Contains(paramethers[i].ParameterType))
                {
                    return false;
                }
            }
            return true;
        }

        Type ToInjectableType(Type type)
        {
            Type injectable = type;
            if (type.IsInterface)
            {
                injectable = _injections.GetInjection(type).Type;
            }
            return injectable;
        }
    }
}
