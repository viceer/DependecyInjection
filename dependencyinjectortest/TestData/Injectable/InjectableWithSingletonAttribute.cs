using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjector.Attributes;

namespace DependencyInjectorTest.TestData.Injectable
{
    [Singleton]
    public interface IInjectableWithSingletonAttribute { }

    class InjectableWithSingletonAttribute : IInjectableWithSingletonAttribute
    {
    }
}
