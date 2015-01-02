using System;
using System.Collections.Concurrent;

namespace DependencyInjector.Internals
{
    internal class SingleInstancesManager :IDisposable
    {
        readonly ConcurrentDictionary<Type, Object> _singleInstances = new ConcurrentDictionary<Type, Object>();

        internal void RegisterSingleInstance(Type type, Object obj)
        {
            _singleInstances.GetOrAdd(type, obj);
        }

        internal bool TryGetValue(Type key, out object obj)
        {
            return _singleInstances.TryGetValue(key, out obj);
        }

        public void Dispose()
        {
            foreach (var instance in _singleInstances)
            {
                if (instance.Value is IDisposable)
                {
                    var disposable = instance.Value as IDisposable;
                    disposable.Dispose();
                }
            }
        }
    }
}
