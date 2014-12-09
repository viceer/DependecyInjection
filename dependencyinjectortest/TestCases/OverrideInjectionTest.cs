using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectorTest.TestData.SimpleClasses;
using DependencyInjectorTest.TestData.Injectable;
using DependencyInjector;
using DependencyInjectorTest.TestData.PropertiesClasses;
using DependencyInjectorTest.TestData;

namespace DependencyInjectorTest
{
    [TestClass]
    public class OverrideInjectionTest
    {
        [TestMethod]
        public void OverrideInConstructor()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var overridenValue = new InjectableOne();
            var testClass = injector.Resolve<SimpleOne>(new { injectableOne = overridenValue }, null);

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreSame(testClass.InjectableOne, overridenValue);
        }

        [TestMethod]
        public void OverrideInPropertyAuto()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var overridenValue = new InjectableOne();
            var testClass = injector.Resolve<PropertyOne>(null, new { InjectableOne = overridenValue });

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreSame(testClass.InjectableOne, overridenValue);
        }

        [TestMethod]
        public void OverrideInPropertyManual()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var overridenValue = new InjectableOne();
            var testClass = new PropertyOne();
            injector.ResolveProperties(testClass, new { InjectableOne = overridenValue });

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreSame(testClass.InjectableOne, overridenValue);
        }

        [TestMethod]
        public void OverrideInPropertyAndConstructor()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>()
                .ExecuteAllRegistrations();

            var overridenValue1 = new InjectableOne();
            var overridenValue2 = new InjectableTwo();
            var testClass = injector.Resolve<MixedInjections>(new { injectableOne = overridenValue1 }, new { InjectableTwoProperty = overridenValue2 });

            Assert.IsNotNull(testClass.InjectableOne);
            Assert.AreSame(testClass.InjectableOne, overridenValue1);

            Assert.IsNotNull(testClass.InjectableTwo);
            Assert.AreSame(testClass.InjectableTwoProperty, overridenValue2);
        }

        [TestMethod]
        public void OverrideInConstructorFail()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var overridenValue = new InjectableOne();

            try
            {
                var testClass = injector.Resolve<SimpleOne>(new { injectableOneWrong = overridenValue }, null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "There is no paramether called 'injectableOneWrong'.";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void OverrideInConstructorFailType()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var overridenValue = new InjectableTwo();

            try
            {
                var testClass = injector.Resolve<SimpleOne>(new { injectableOne = overridenValue }, null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "Paramether 'injectableOne' is not assignable from 'DependencyInjectorTest.TestData.Injectable.InjectableTwo' type";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void OverrideInPropertyFailManual()
        {
            var injector = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .ExecuteAllRegistrations();

            var overridenValue = new InjectableOne();
            var testClass = new PropertyOne();
            try
            {
                injector.ResolveProperties(testClass, new { injectableOneWrong = overridenValue });
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "There is no Property called 'injectableOneWrong'.";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void OverrideInPropertyFailAuto()
        {
            var injector = new Resolver()
                 .Register<IInjectableOne, InjectableOne>()
                 .ExecuteAllRegistrations();

            var overridenValue = new InjectableOne();

            try
            {
                var testClass = injector.Resolve<PropertyOne>(null, new { injectableOneWrong = overridenValue });
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "There is no Property called 'injectableOneWrong'.";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void OverrideInPropertyFailTypeManual()
        {
            var injector = new Resolver()
                 .Register<IInjectableOne, InjectableOne>()
                 .ExecuteAllRegistrations();

            var overridenValue = new InjectableTwo();
            var testClass = new PropertyOne();
            try
            {
                injector.ResolveProperties(testClass, new { InjectableOne = overridenValue });
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "Property 'InjectableOne' is not assignable from 'DependencyInjectorTest.TestData.Injectable.InjectableTwo' type";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void OverrideInPropertyFailTypeAuto()
        {
            var injector = new Resolver()
                 .Register<IInjectableOne, InjectableOne>()
                 .ExecuteAllRegistrations();

            var overridenValue = new InjectableTwo();

            try
            {
                var testClass = injector.Resolve<PropertyOne>(null, new { InjectableOne = overridenValue });
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentException ae)
            {
                string error = "Property 'InjectableOne' is not assignable from 'DependencyInjectorTest.TestData.Injectable.InjectableTwo' type";
                Assert.AreEqual(error, ae.Message);
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
    }
}

