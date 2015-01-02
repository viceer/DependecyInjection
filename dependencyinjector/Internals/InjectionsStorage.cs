using System;
using System.Collections.Generic;

namespace DependencyInjector.Internals
{
    internal class InjectionsStorage
    {
        readonly Dictionary<Type, Injection> _injections = new Dictionary<Type, Injection>();

        internal bool Contains(Type type)
        {
            return _injections.ContainsKey(type);
        }

        internal void Add(Type type, Injection injection)
        {
            if (_injections.ContainsKey(type))
            {
                throw new ArgumentException(String.Format("{0} type has already been injected.", type));
            }
            _injections.Add(type, injection);
        }

        internal Injection GetInjection(Type type)
        {
            Injection injection;
            if (!_injections.TryGetValue(type, out injection))
            {
                throw new NullReferenceException(String.Format("No injection found for {0} type.", type));
            }
            return injection;
        }

        internal bool IsIsngleton(Type type)
        {
            Injection injection;
            if (!_injections.TryGetValue(type, out injection))
            {
                return false;
            }
            return injection.SingleInstance;
        }

        internal Dictionary<Type, Injection> Injections { get { return _injections; } }
    }
}
