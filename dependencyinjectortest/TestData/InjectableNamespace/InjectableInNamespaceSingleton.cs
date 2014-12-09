using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjector.Attributes;

namespace DependencyInjectorTest.TestData.InjectableNamespace
{
    [Singleton]
    public interface IInjectableInNamespaceSingleton { }

    class InjectableInNamespaceSingleton : IInjectableInNamespaceSingleton
    {
    }
}
