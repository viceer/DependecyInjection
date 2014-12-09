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
    internal class Registrator
    {
        InjectionsStorage _storage;
        ConcurrentDictionary<Type, Injection> _tempInjectionStorage = new ConcurrentDictionary<Type, Injection>();
        Dictionary<string, Assembly> _tempAssembliesStorage = new Dictionary<string, Assembly>();
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

        internal void Add<I, C>(bool isSingleInstance) where C : class,I
        {
            Type type = typeof(I);
            Injection injection = new Injection { Type = typeof(C), SingleInstance = isSingleInstance };
            AddUpdateTempInjection(type, injection);
        }

        internal void Add(Type from, Type to, bool isSingleInstance = false)
        {
            Injection injection = new Injection { Type = to, SingleInstance = isSingleInstance };
            AddUpdateTempInjection(from, injection);
        }

        internal void ResolveAllFromNamespace<T>() where T : class
        {
            Type type = typeof(T);
            Assembly assembly = type.Assembly;
            string name = type.Namespace;
            _tempAssembliesStorage.Add(name, assembly);
        }

        internal void ResolveAllFromNamespace(string name)
        {
            Assembly assembly = null;
            try
            {
                string assemblyName = GetAssemblyNameFromFullNamespace(name);
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
            Injection currentInjection = null;
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
                injection.SingleInstance = HasSingletonArttribute(type) ? true : injection.SingleInstance;
                _tempInjectionStorage.GetOrAdd(type, injection);
                _last = injection;
            }
        }

        void UpdateSingleInstance(Injection current, Injection injection)
        {
            if (injection.SingleInstance && !current.SingleInstance)
            {
                current.SingleInstance = true;
            }
        }

        bool HasSingletonArttribute(Type type)
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
            Parallel.ForEach (_tempAssembliesStorage, item =>
            {
                RegisterAssembly(item.Value, item.Key);
            });
        }
        #endregion

        #region NAMESPACE REGISTRATION
        void RegisterAssembly(Assembly assembly, string name)
        {
            var types = assembly.GetTypes().Where(t => t.Namespace == name);
            var interfaces = types.Where(t => (t.IsInterface) && (t.Namespace == name)).ToArray();
            foreach (var inter in interfaces)
            {
                if (FollowsInterfaceConvention(inter.Name))
                {
                    string possibleClassName = GetClassNameFromInterface(inter.Name);
                    var classes = types.Where(mytype => mytype.GetInterfaces().Contains(inter) && mytype.Name == possibleClassName).ToList();
                    if (classes.Count > 1)
                    {
                        throw new AmbiguousMatchException(String.Format("Multiple classes found {0} interface.", inter.Name));
                    }
                    else if (classes.Count == 1)
                    {
                        AddUpdateTempInjection(inter, new Injection { Type = classes[0], SingleInstance = false });
                    }
                }
            }
        }

        string GetAssemblyNameFromFullNamespace(string name)
        {
            int index = name.IndexOf('.');
            return name.Substring(0, index == -1 ? name.Length : index);
        }

        bool FollowsInterfaceConvention(string name)
        {
            return name.StartsWith("I") && name.Length > 1;
        }

        string GetClassNameFromInterface(string name)
        {
            return name.Substring(1);
        }
        #endregion
    }
}
