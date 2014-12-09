using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.SimpleClasses
{
    class SimpleTwo
    {
        public IInjectableOne InjectableOne { get; set; }
        public IInjectableTwo InjectableTwo { get; set; }

        public SimpleTwo(IInjectableOne injectableOne, IInjectableTwo injectableTwo)
        {
            InjectableOne = injectableOne;
            InjectableTwo = injectableTwo;
        }
    }
}
