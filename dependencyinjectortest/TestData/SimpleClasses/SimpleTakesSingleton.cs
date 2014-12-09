using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.SimpleClasses
{
    class SimpleTakesSingleton
    {
        public IInjectableWithSingletonAttribute InjectableWithSingletonAttribute { get; set; }
        public SimpleTakesSingleton(IInjectableWithSingletonAttribute injectableWithSingletonAttribute)
        {
            InjectableWithSingletonAttribute = injectableWithSingletonAttribute;
        }
    }
}
