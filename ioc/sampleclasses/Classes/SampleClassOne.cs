using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOC.SampleClasses.Injectables;
using DependencyInjector.Attributes;

namespace IOC.SampleClasses.Classes
{
    class SampleClassOne
    {
        public IInjectableOne One { get; set; }

        [Injectable]
        public IInjectableTwo Two { get; set; }

        public SampleClassOne(IInjectableOne one)
        {
            One = one;
        }
    }
}
