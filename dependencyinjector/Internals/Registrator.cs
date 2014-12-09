using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using DependencyInjector.Attributes;
using System.Collections.Concurrent;

namespace DependencyInjector.Internals
{
    internal class Registrator
    {
        readonly InjectionsStorage _storage;
        readonly ConcurrentDictionary<Type, Injection> _tempInjectionStorage = new ConcurrentDictionary<Type, Injection>();
        readonly Dictionary<string, Assembly> _tempAssembliesStorage = new Dictionary<string, Assembly>();
        Injection _last;

        internal Registrator(InjectionsStorage storage)
        {
            _storage = storage;
        }

        internal void Execute()
        {
            RegisterDataFromAssemblies();
            TransferDataToStorage();
            _tempInjectionStorage.Clear();
            _tempAssembliesStorage.Clear();
        }

        internal void LastAsSingleton()
        {
            _last.SingleInstance = true;
        }

        internal void Add<T, TC>(bool isSingleInstance) where TC : class,T
        {
            var type = typeof(T);
            var injection = new Injection { Type = typeof(TC), SingleInstance = isSingleInstance };
            AddUpdateTempInjection(type, injection);
        }

        internal void ResolveAllFromNamespace<T>() where T : class
        {
            var type = typeof(T);
            var assembly = type.Assembly;
            var name = type.Namespace;
            _tempAssembliesStorage.Add(name, assembly);
        }

        internal void ResolveAllFromNamespace(string name)
        {
            Assembly assembly;
            try
            {
                var assemblyName = GetAssemblyNameFromFullNamespace(name);
                assembly = Assembly.Load(assemblyName);
            }
            catch 
            {
                throw new ArgumentException("Provied namespase does not exist.");
            }            
            _tempAssembliesStorage.Add(name, assembly);
        }

        #region PRIVATE IMPLEMENTATION
        void AddUpdateTempInjection(Type type, Injection injection)
        {
            Injection currentInjection;
            if (_tempInjectionStorage.TryGetValue(type, out currentInjection))
            {
                if (injection.Type != currentInjection.Type)
                {
                    throw new AmbiguousMatchException(String.Format("Multiple classes are injected for {0} interface.", type.Name));
                }
                UpdateSingleInstance(currentInjection, injection);
            }
            else
            {
                injection.SingleInstance = HasSingletonArttribute(type) || injection.SingleInstance;
                _tempInjectionStorage.GetOrAdd(type, injection);
                _last = injection;
            }
        }

        static void UpdateSingleInstance(Injection current, Injection injection)
        {
            if (injection.SingleInstance && !current.SingleInstance)
            {
                current.SingleInstance = true;
            }
        }

        static bool HasSingletonArttribute(Type type)
        {
            return type.IsDefined(typeof(Singleton));
        }

        void TransferDataToStorage()
        {
            foreach (var inj in _tempInjectionStorage)
            {
                _storage.Add(inj.Key, inj.Value);
            }
        }

        void RegisterDataFromAssemblies()
        {
            Parallel.ForEach (_tempAssembliesStorage, item => RegisterAssembly(item.Value, item.Key));
        }
        #endregion

        #region NAMESPACE REGISTRATION
        void RegisterAssembly(Assembly assembly, string name)
        {
            var types = assembly.GetTypes().Where(t => t.Namespace == name);
            var enumerableTypes = types as Type[] ?? types.ToArray();
            var interfaces = enumerableTypes.Where(t => (t.IsInterface) && (t.Namespace == name)).ToArray();
            foreach (var inter in interfaces)
            {
                if (FollowsInterfaceConvention(inter.Name))
                {
                    string possibleClassName = GetClassNameFromInterface(inter.Name);
                    var classes = enumerableTypes.Where(mytype => mytype.GetInterfaces().Contains(inter) && mytype.Name == possibleClassName).ToList();
                    if (classes.Count > 1)
                    {
                        throw new AmbiguousMatchException(String.Format("Multiple classes found {0} interface.", inter.Name));
                    }
                    if (classes.Count == 1)
                    {
                        AddUpdateTempInjection(inter, new Injection { Type = classes[0], SingleInstance = false });
                    }
                }
            }
        }

        static string GetAssemblyNameFromFullNamespace(string name)
        {
            var index = name.IndexOf('.');
            return name.Substring(0, index == -1 ? name.Length : index);
        }

        static bool FollowsInterfaceConvention(string name)
        {
            return name.StartsWith("I") && name.Length > 1;
        }

        static string GetClassNameFromInterface(string name)
        {
            return name.Substring(1);
        }
        #endregion
    }
}
