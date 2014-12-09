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
