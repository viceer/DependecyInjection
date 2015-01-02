using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectorTest.TestData.SimpleClasses;
using DependencyInjectorTest.TestData.Injectable;
using DependencyInjector;

namespace DependencyInjectorTest
{
    [TestClass]
    public class BasicCreationTest
    {
        [TestMethod]
        public void CreateEmpty()
        {
            var injector = new Resolver();

            var testClass = injector.Resolve<SimpleNoConstructor>();

            Assert.IsNotNull(testClass);
            Assert.AreEqual(testClass.GetType(), typeof(SimpleNoConstructor));
        }

        [TestMethod]
        public void CreateInjectOne()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<SimpleOne>();

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));
        }

        [TestMethod]
        public void CreateInjectTwo()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<SimpleTwo>();

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));

            Assert.IsNotNull(testClass.InjectableTwo);
            Assert.AreEqual(testClass.InjectableTwo.GetType(), typeof(InjectableTwo));
        }

        [TestMethod]
        public void CreateInjectThree()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .Register<IInjectableThree, InjectableThree>()
                .Register<IInjectableWithSingletonAttribute, InjectableWithSingletonAttribute>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<SimpleThree>();

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));

            Assert.IsNotNull(testClass.InjectableTwo);
            Assert.AreEqual(testClass.InjectableTwo.GetType(), typeof(InjectableTwo));

            Assert.IsNotNull(testClass.InjectableThree);
            Assert.AreEqual(testClass.InjectableThree.GetType(), typeof(InjectableThree));

            Assert.IsNotNull(testClass.InjectableWithSingletonAttribute);
            Assert.AreEqual(testClass.InjectableWithSingletonAttribute.GetType(), typeof(InjectableWithSingletonAttribute));
        }

        [TestMethod]
        public void ThrowUnknowTypeExeption()
        {
            var injector = new Resolver();
            try
            {
                var testClass = injector.Resolve<SimpleOne>();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                var error = string.Format("No aprpriate constructor found for {0} type.", typeof(SimpleOne));
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void DirectResolve()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<IInjectableOne>();

            Assert.IsNotNull(testClass);
            Assert.AreEqual(testClass.GetType(), typeof(InjectableOne));
        }
    }
}
