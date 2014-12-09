using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjector.Attributes;

namespace IOC.SampleClasses.Injectables
{
    [Singleton]
    public interface IInjectableSingletonAttr { }
    class InjectableSingletonAttr : IInjectableSingletonAttr { }

}
