using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectorTest.TestData.SimpleClasses;
using DependencyInjectorTest.TestData.Injectable;
using DependencyInjector;
using DependencyInjectorTest.TestData.PropertiesClasses;
using DependencyInjectorTest.TestData.NamespaceClasses;
using DependencyInjectorTest.TestData.InjectableNamespace;
using DependencyInjectorTest.TestData.Dependency;

namespace DependencyInjectorTest
{
    [TestClass]
    public class CohesionTest
    {
        [TestMethod]
        public void CheckAllRequiredOk()
        {
            var injector = new Resolver()
                .RegisterAllFromNamespace("DependencyInjectorTest.TestData.InjectableNamespace")
                .RegisterAllFromNamespace<InjectableOne>()
                .ExecuteAllRegistrations()
                .CheckCohesion();
        }

        [TestMethod]
        public void CheckAllRequiredDependantOk()
        {
            var injector = new Resolver()
                    .RegisterAllFromNamespace<InjectableOne>()
                    .Register<IDependantOne,DependantOne>()
                    .ExecuteAllRegistrations()
                    .CheckCohesion();
        }

        [TestMethod]
        public void CheckAllRequiredDependantFail()
        {
            try
            {
                var injector = new Resolver()
                    .RegisterAllFromNamespace<InjectableOne>()
                    //.AddInjection<IDependantOne, DependantOne>()
                    .Register<IDependantTwo, DependantTwo>()
                    .Register<IDependantThree, DependantThree>()
                    .ExecuteAllRegistrations()
                    .CheckCohesion();
                Assert.Fail("An exception should have been thrown");
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException == null) throw ae;
                if (!(ae.InnerException is ArgumentException)) throw ae;
                string error = "No aprpriate constructor found for ";
                Assert.IsTrue(ae.InnerException.Message.Contains(error));
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void CheckCircularDependencyFail()
        {
            try
            {
                var injector = new Resolver()
                    .Register<ICircularOne, CircularOne>()
                    .Register<ICircularTwo, CircularTwo>()
                    .ExecuteAllRegistrations()
                    .CheckCohesion();
                Assert.Fail("An exception should have been thrown");
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException == null) throw ae;
                if (!(ae.InnerException is ArgumentException)) throw ae;
                string error = "Circular Dependency found for";
                Assert.IsTrue(ae.InnerException.Message.Contains(error));
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void CheckDeepCircularDependencyFail()
        {
            try
            {
                var injector = new Resolver()
                    .Register<IDeepDependantOne, DeepDependantOne>()
                    .Register<IDeepDependantTwo, DeepDependantTwo>()
                    .Register<IDeepDependantThree, DeepDependantThree>()
                    .Register<IDeepDependantFour, DeepDependantFour>()
                    .ExecuteAllRegistrations()
                    .CheckCohesion();
                Assert.Fail("An exception should have been thrown");
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException == null) throw ae;
                if (!(ae.InnerException is ArgumentException)) throw ae;
                string error = "Circular Dependency found for";
                Assert.IsTrue(ae.InnerException.Message.Contains(error));
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
    }
}
