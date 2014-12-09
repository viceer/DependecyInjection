using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.SimpleClasses
{
    class SimpleThree
    {
        public IInjectableOne InjectableOne { get; set; }
        public IInjectableTwo InjectableTwo { get; set; }
        public IInjectableThree InjectableThree { get; set; }
        public IInjectableWithSingletonAttribute InjectableWithSingletonAttribute { get; set; }

        public SimpleThree(IInjectableOne injectableOne, IInjectableTwo injectableTwo, IInjectableThree injectableThree, IInjectableWithSingletonAttribute injectableWithSingletonAttribute)
        {
            InjectableOne = injectableOne;
            InjectableTwo = injectableTwo;
            InjectableThree = injectableThree;
            InjectableWithSingletonAttribute = injectableWithSingletonAttribute;
        }
    }
}
