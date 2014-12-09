using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.SimpleClasses
{
    class SimpleOne
    {
        public IInjectableOne InjectableOne { get; set; }

        public SimpleOne(IInjectableOne injectableOne)
        {
            InjectableOne = injectableOne;
        }
    }
}
