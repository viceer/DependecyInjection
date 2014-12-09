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
