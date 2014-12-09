using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.InjectableNamespace;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.Dependency
{
    public interface IDependantThree { }

    class DependantThree : IDependantThree
    {
        public DependantThree(IDependantOne one, IDependantTwo two)
        { }
    }
}
