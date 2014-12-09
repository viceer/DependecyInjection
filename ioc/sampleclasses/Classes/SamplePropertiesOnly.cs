using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOC.SampleClasses.Injectables;
using DependencyInjector.Attributes;

namespace IOC.SampleClasses.Classes
{
    class SamplePropertiesOnly
    {
        [Injectable]
        public IInjectableOne One { get; set; }

        [Injectable]
        public IInjectableTwo Two { get; set; }
    }
}
