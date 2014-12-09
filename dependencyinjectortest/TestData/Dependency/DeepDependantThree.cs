using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectorTest.TestData.Dependency
{
    public interface IDeepDependantThree { }
    class DeepDependantThree : IDeepDependantThree
    {
        public DeepDependantThree(IDeepDependantFour four) { }
    }
}
