using DependencyInjector.Attributes;

namespace IOC.SampleClasses.Injectables
{
    [Singleton]
    public interface IInjectableSingletonAttr { }
    class InjectableSingletonAttr : IInjectableSingletonAttr { }

}
