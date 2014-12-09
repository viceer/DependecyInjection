using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectorTest.TestData.Injectable;
using DependencyInjector;
using DependencyInjectorTest.TestData.PropertiesClasses;

namespace DependencyInjectorTest
{
    [TestClass]
    public class PropertiesSingletonTest
    {
        [TestMethod]
        public void CreateInjectSingleton()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .Register<IInjectableThree, InjectableThree>()
                .Register<IInjectableWithSingletonAttribute, InjectableWithSingletonAttribute>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<PropertyThree>();
            var testClass2 = injector.Resolve<PropertyThree>();

            Assert.IsNotNull(testClass.InjectableWithSingletonAttribute);

            Assert.AreSame(testClass.InjectableWithSingletonAttribute, testClass2.InjectableWithSingletonAttribute);
        }

        [TestMethod]
        public void CreateInjectNonSingleton()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<PropertyOne>();
            var testClass2 = injector.Resolve<PropertyOne>();

            Assert.IsNotNull(testClass.InjectableOne);

            Assert.AreNotSame(testClass.InjectableOne, testClass2.InjectableOne);
        }
    }
}
