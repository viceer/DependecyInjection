using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.PropertiesClasses
{
    class PropertyTwo
    {
        [DependencyInjector.Attributes.Injectable]
        public IInjectableOne InjectableOne { get; set; }

        [DependencyInjector.Attributes.Injectable]
        public IInjectableTwo InjectableTwo { get; set; }
    }
}
