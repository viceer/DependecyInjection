using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.PropertiesClasses
{
    class PropertyThree
    {
        [DependencyInjector.Attributes.Injectable]
        public IInjectableOne InjectableOne { get; set; }

        [DependencyInjector.Attributes.Injectable]
        public IInjectableTwo InjectableTwo { get; set; }

        [DependencyInjector.Attributes.Injectable]
        public IInjectableThree InjectableThree { get; set; }

        [DependencyInjector.Attributes.Injectable]
        public IInjectableWithSingletonAttribute InjectableWithSingletonAttribute { get; set; }
    }
}
