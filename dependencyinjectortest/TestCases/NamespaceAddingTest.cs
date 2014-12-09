using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjector;
using DependencyInjectorTest.TestData.NamespaceClasses;
using DependencyInjectorTest.TestData.InjectableNamespace;

namespace DependencyInjectorTest
{
    [TestClass]
    public class NamespaceAddingTest
    {
        [TestMethod]
        public void LoadNamespaceFromString()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace("DependencyInjectorTest.TestData.InjectableNamespace")
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceOne>();

            Assert.IsNotNull(testClass.InjectableInNamespaceOne);
            Assert.AreEqual(testClass.InjectableInNamespaceOne.GetType(), typeof(InjectableInNamespaceOne));
        }

        [TestMethod]
        public void LoadNamespaceFromStringFail()
        {           
            try
            {
                var injector = new Resolver()
                    .RegisterAllFromNamespace("DependencyInjectorTestInvalidData")
                    .ExecuteAllRegistrations();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "Provied namespase does not exist.";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void LoadNamespaceFromNullStringFail()
        {
            try
            {
                var injector = new Resolver()
                    .RegisterAllFromNamespace(null)
                    .ExecuteAllRegistrations();
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "Provied namespase does not exist.";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void LoadNamespaceFromType()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace<InjectableInNamespaceOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceOne>();

            Assert.IsNotNull(testClass.InjectableInNamespaceOne);
            Assert.AreEqual(testClass.InjectableInNamespaceOne.GetType(), typeof(InjectableInNamespaceOne));
        }

        [TestMethod]
        public void LoadNamespaceFromStringAll()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace("DependencyInjectorTest.TestData.InjectableNamespace")
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceAll>();

            Assert.IsNotNull(testClass.InjectableInNamespaceOne);
            Assert.AreEqual(testClass.InjectableInNamespaceOne.GetType(), typeof(InjectableInNamespaceOne));

            Assert.IsNotNull(testClass.InjectableInNamespaceTwo);
            Assert.AreEqual(testClass.InjectableInNamespaceTwo.GetType(), typeof(InjectableInNamespaceTwo));

            Assert.IsNotNull(testClass.InjectableInNamespaceThree);
            Assert.AreEqual(testClass.InjectableInNamespaceThree.GetType(), typeof(InjectableInNamespaceThree));

            Assert.IsNotNull(testClass.InjectableInNamespaceSingleton);
            Assert.AreEqual(testClass.InjectableInNamespaceSingleton.GetType(), typeof(InjectableInNamespaceSingleton));
        }

        [TestMethod]
        public void LoadNamespaceFromStringWithsingleton()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace("DependencyInjectorTest.TestData.InjectableNamespace")
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceAll>();
            var testClass2 = injector.Resolve<NamespaceAll>();
            Assert.IsNotNull(testClass.InjectableInNamespaceSingleton);
            Assert.AreEqual(testClass.InjectableInNamespaceSingleton, testClass2.InjectableInNamespaceSingleton);
        }

        [TestMethod]
        public void LoadNamespaceFromTypeAll()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace<InjectableInNamespaceOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceAll>();

            Assert.IsNotNull(testClass.InjectableInNamespaceOne);
            Assert.AreEqual(testClass.InjectableInNamespaceOne.GetType(), typeof(InjectableInNamespaceOne));

            Assert.IsNotNull(testClass.InjectableInNamespaceTwo);
            Assert.AreEqual(testClass.InjectableInNamespaceTwo.GetType(), typeof(InjectableInNamespaceTwo));

            Assert.IsNotNull(testClass.InjectableInNamespaceThree);
            Assert.AreEqual(testClass.InjectableInNamespaceThree.GetType(), typeof(InjectableInNamespaceThree));

            Assert.IsNotNull(testClass.InjectableInNamespaceSingleton);
            Assert.AreEqual(testClass.InjectableInNamespaceSingleton.GetType(), typeof(InjectableInNamespaceSingleton));
        }

        [TestMethod]
        public void LoadNamespaceFromTypeWithSingletons()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace<InjectableInNamespaceOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceAll>();
            var testClass2 = injector.Resolve<NamespaceAll>();
            Assert.IsNotNull(testClass.InjectableInNamespaceSingleton);
            Assert.AreEqual(testClass.InjectableInNamespaceSingleton, testClass2.InjectableInNamespaceSingleton);
        }

        [TestMethod]
        public void OverrideNamespaceSingleton()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace<InjectableInNamespaceOne>()
                .Register<IInjectableInNamespaceOne, InjectableInNamespaceOne>().AsSingleton()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<NamespaceAll>();
            var testClass2 = injector.Resolve<NamespaceAll>();
            Assert.IsNotNull(testClass.InjectableInNamespaceOne);
            Assert.AreEqual(testClass.InjectableInNamespaceOne, testClass2.InjectableInNamespaceOne);
        }
    }
}
