using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjector.Internals
{
    internal class SingleInstancesManager
    {
        ConcurrentDictionary<Type, Object> _singleInstances = new ConcurrentDictionary<Type, Object>();

        internal void RegisterSingleInstance(Type type, Object obj)
        {
            _singleInstances.GetOrAdd(type, obj);
        }

        internal bool TryGetValue(Type key, out object obj)
        {
            return _singleInstances.TryGetValue(key, out obj);
        }
    }
}
