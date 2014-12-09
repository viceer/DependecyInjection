using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectorTest.TestData.Dependency
{
    public interface IDeepDependantOne { }
    class DeepDependantOne : IDeepDependantOne
    {
        public DeepDependantOne(IDeepDependantTwo two) {}
    }
}
