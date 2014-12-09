using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.InjectableNamespace;

namespace DependencyInjectorTest.TestData.NamespaceClasses
{
    class NamespaceAll
    {
        public IInjectableInNamespaceOne InjectableInNamespaceOne;
        public IInjectableInNamespaceTwo InjectableInNamespaceTwo;
        public IInjectableInNamespaceThree InjectableInNamespaceThree;
        public IInjectableInNamespaceSingleton InjectableInNamespaceSingleton;

        public NamespaceAll(IInjectableInNamespaceOne injectableInNamespaceOne, IInjectableInNamespaceTwo injectableInNamespaceTwo, IInjectableInNamespaceThree injectableInNamespaceThree, IInjectableInNamespaceSingleton injectableInNamespaceSingleton)
        {
            InjectableInNamespaceOne = injectableInNamespaceOne;
            InjectableInNamespaceTwo = injectableInNamespaceTwo;
            InjectableInNamespaceThree = injectableInNamespaceThree;
            InjectableInNamespaceSingleton = injectableInNamespaceSingleton;
        }
    }
}
