using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectorTest.TestData.SimpleClasses;
using DependencyInjectorTest.TestData.Injectable;
using DependencyInjector;

namespace DependencyInjectorTest.TestData
{
    [TestClass]
    public class SingletonTest
    {
        [TestMethod]
        public void CreateInjectSingleton()
        {
            var injector = new Resolver()
                .Register<IInjectableWithSingletonAttribute, InjectableWithSingletonAttribute>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<SimpleTakesSingleton>();
            var testClass2 = injector.Resolve<SimpleTakesSingleton>();

            Assert.IsNotNull(testClass.InjectableWithSingletonAttribute);

            Assert.AreSame(testClass.InjectableWithSingletonAttribute, testClass2.InjectableWithSingletonAttribute);
        }

        [TestMethod]
        public void CreateInjectNonSingleton()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<SimpleOne>();
            var testClass2 = injector.Resolve<SimpleOne>();

            Assert.IsNotNull(testClass.InjectableOne);

            Assert.AreNotSame(testClass.InjectableOne, testClass2.InjectableOne);
        }
    }
}
