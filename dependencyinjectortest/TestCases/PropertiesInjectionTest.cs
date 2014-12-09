using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectorTest.TestData.SimpleClasses;
using DependencyInjectorTest.TestData.Injectable;
using DependencyInjector;
using DependencyInjectorTest.TestData.PropertiesClasses;

namespace DependencyInjectorTest
{
    [TestClass]
    public class PropertiesInjectionTest
    {
        [TestMethod]
        public void NoPropertiesToinject()
        {
            var injector = new Resolver();
            injector.ResolveProperties(new SimpleNoConstructor());
        }

        [TestMethod]
        public void PropertyInjectOneAuto()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<PropertyOne>();

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));
        }

        [TestMethod]
        public void PropertyInjectOneManual()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var testClass = new PropertyOne();
            injector.ResolveProperties(testClass);

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));
        }

        [TestMethod]
        public void CreateInjectTwoAuto()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<PropertyTwo>();

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));

            Assert.IsNotNull(testClass.InjectableTwo);
            Assert.AreEqual(testClass.InjectableTwo.GetType(), typeof(InjectableTwo));
        }

        [TestMethod]
        public void CreateInjectTwoManual()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .ExecuteAllRegistrations();

            var testClass = new PropertyTwo();
            injector.ResolveProperties(testClass);

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreEqual(testClass.InjectableOne.GetType(), typeof(InjectableOne));

            Assert.IsNotNull(testClass.InjectableTwo);
            Assert.AreEqual(testClass.InjectableTwo.GetType(), typeof(InjectableTwo));
        }

        [TestMethod]
        public void CreateInjectThreeAuto()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .Register<IInjectableThree, InjectableThree>()
                .Register<IInjectableWithSingletonAttribute, InjectableWithSingletonAttribute>()
                .ExecuteAllRegistrations();

            var testClass = injector.Resolve<PropertyThree>();

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
        public void CreateInjectThreeManual()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .Register<IInjectableThree, InjectableThree>()
                .Register<IInjectableWithSingletonAttribute, InjectableWithSingletonAttribute>()
                .ExecuteAllRegistrations();

            var testClass = new PropertyThree();
            injector.ResolveProperties(testClass);

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
        public void ThrowUnknowTypeExeptionAuto()
        {
            var injector = new Resolver();
            try
            {
                var testClass = injector.Resolve<PropertyOne>();
                Assert.Fail("An exception should have been thrown");
            }
            catch (NullReferenceException ae)
            {
                var error = string.Format("No injection found for {0} type.", typeof(IInjectableOne));
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void ThrowUnknowTypeExeptionManual()
        {
            var injector = new Resolver();
            var testClass = new PropertyOne();
            try
            {
                injector.ResolveProperties(testClass);
                Assert.Fail("An exception should have been thrown");
            }
            catch (NullReferenceException ae)
            {
                var error = string.Format("No injection found for {0} type.", typeof(IInjectableOne));
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
    }
}
