using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.InjectableNamespace;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.Dependency
{
    public interface IDependantTwo { }

    class DependantTwo : IDependantTwo
    {
        public DependantTwo(IInjectableOne one, IInjectableTwo two, IInjectableThree three, IDependantOne oneAgain)
        { }
    }
}
