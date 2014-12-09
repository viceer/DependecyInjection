using DependencyInjector.Attributes;

namespace IOC.SampleClasses.SampleNamespaceTwo
{
    [Singleton]
    public interface ISingletonInNamespaceTwo { }
    class SingletonInNamespaceTwo : ISingletonInNamespaceTwo { }
}
