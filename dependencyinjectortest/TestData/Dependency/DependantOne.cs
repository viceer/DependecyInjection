using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.InjectableNamespace;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.Dependency
{
    public interface IDependantOne { }

    class DependantOne :IDependantOne
    {
        public DependantOne(IInjectableOne one, IInjectableTwo two, IInjectableThree three)
        { }
    }
}
