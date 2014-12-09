using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData
{
    class MixedInjections
    {
        public IInjectableOne InjectableOne { get; set; }
        public IInjectableTwo InjectableTwo { get; set; }

        public MixedInjections(IInjectableOne injectableOne, IInjectableTwo injectableTwo)
        {
            InjectableOne = injectableOne;
            InjectableTwo = injectableTwo;
        }

        [DependencyInjector.Attributes.Injectable]
        public IInjectableOne InjectableOneProperty { get; set; }
        [DependencyInjector.Attributes.Injectable]
        public IInjectableTwo InjectableTwoProperty { get; set; }
    }
}
