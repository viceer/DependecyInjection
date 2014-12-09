using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjectorTest.TestData.InjectableNamespace;
using DependencyInjectorTest.TestData.Injectable;

namespace DependencyInjectorTest.TestData.Dependency
{
    public interface ICircularOne { }

    class CircularOne : ICircularOne
    {
        public CircularOne(ICircularTwo two)
        { }
    }
}
