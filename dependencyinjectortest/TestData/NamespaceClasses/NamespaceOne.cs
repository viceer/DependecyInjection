using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.InjectableNamespace;

namespace DependencyInjectorTest.TestData.NamespaceClasses
{
    class NamespaceOne
    {
        public IInjectableInNamespaceOne InjectableInNamespaceOne;
        public NamespaceOne(IInjectableInNamespaceOne injectableInNamespaceOne)
        {
            InjectableInNamespaceOne = injectableInNamespaceOne;
        }
    }
}
