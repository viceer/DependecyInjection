using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjector;
using IOC.SampleClasses.Classes;
using IOC.SampleClasses.Injectables;
using IOC.SampleClasses.SampleNamespaceOne;
using IOC.SampleClasses.SampleNamespaceTwo;

namespace IOC
{
    class Program
    {
        static Resolver _resolver;

        static void Main(string[] args)
        {
            _resolver = new Resolver()
                .Register<IInjectableOne, InjectableOne>()
                .Register<IInjectableTwo, InjectableTwo>().AsSingleton()
                .Register<IInjectableSingletonAttr, InjectableSingletonAttr>()
                .RegisterAllFromNamespace("IOC.SampleClasses.SampleNamespaceOne")
                .Register<INamespaceOneUnknown, NamespaceOneUnknown>().AsSingleton()
                .RegisterAllFromNamespace<FirstClassInNamespaceTwo>()
                .ExecuteAllRegistrations()       // execute all previous registration - mandatory step
                .CheckCohesion();       // check data cohesion and throw exeption if necesasry - optional (yet recommended) step

            // to check data injection please debug through below methods
            for (int i = 0; i < 1000; i++)
            {
                ResolveNornal();
                Resolvesingleton();
                ResolveProperties();
                OverrideInConstructor();
                OverrideInProperties();
                OverrideInConstructorAndProperties();
                OverrideInInjectableProperties();
                Console.WriteLine(i);
            }
            Console.ReadLine();
        }

        static void ResolveNornal()
        {
            SampleClassOne sample = _resolver.Resolve<SampleClassOne>();
        }

        static void Resolvesingleton()
        {
            SampleClassOne sampleOne = _resolver.Resolve<SampleClassOne>();
            SampleClassOne sampleTwo = _resolver.Resolve<SampleClassOne>();
            if (sampleOne.Two == sampleTwo.Two)
            {
                // same singleton class
            }

            if (sampleOne.One != sampleTwo.One)
            {
                // new class per instance class
            }
        }

        static void ResolveProperties()
        {
            SamplePropertiesOnly sample = new SamplePropertiesOnly();
            _resolver.ResolveProperties(sample);
        }

        static void OverrideInConstructor()
        {
            InjectableOne injectableOne = new InjectableOne();
            SampleClassOne sample = _resolver.Resolve<SampleClassOne>(new { one = injectableOne });
            if (injectableOne == sample.One)
            {
                // same class
            }
        }

        static void OverrideInProperties()
        {
            InjectableTwo injectableTwo = new InjectableTwo();
            SampleClassOne sample = _resolver.Resolve<SampleClassOne>(null, new { Two = injectableTwo });
            if (injectableTwo == sample.Two)
            {
                // same class
            }
        }

        static void OverrideInConstructorAndProperties()
        {
            InjectableOne injectableOne = new InjectableOne();
            InjectableTwo injectableTwo = new InjectableTwo();
            SampleClassOne sample = _resolver.Resolve<SampleClassOne>(new { one = injectableOne }, new { Two = injectableTwo });
            if (injectableOne == sample.One)
            {
                if (injectableTwo == sample.Two)
                {
                    // both calasses overriden
                }
            }
        }

        static void OverrideInInjectableProperties()
        {
            SamplePropertiesOnly sample = new SamplePropertiesOnly();
            InjectableOne injectableOne = new InjectableOne();
            InjectableTwo injectableTwo = new InjectableTwo();
            _resolver.ResolveProperties(sample, new { One = injectableOne, Two = injectableTwo });
            if (injectableOne == sample.One)
            {
                if (injectableTwo == sample.Two)
                {
                    // both calasses overriden
                }
            }
        }
    }
}
