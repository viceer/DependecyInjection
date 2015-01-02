using System;
using DependencyInjector.Internals;

/*
 TO DO: 
 * 
 * refactor registration
 * IDisposable
 * safe singleton pattern
 * inject collections
 * 
 * 
 * ensure correct mapping for multi app pools
 * 
 * ? custom naming conventions
 * ? Get support for custom injections patterns for target types
 */

namespace DependencyInjector
{
    public class Resolver
    {
        readonly InjectionsStorage _injections;
        readonly ConstructorsManager _constructors;
        readonly PropertiesManager _properties;
        readonly ObjectCreator _creator;
        readonly Registrator _registrator;

        public Resolver()
        {
            _injections = new InjectionsStorage();
            _registrator = new Registrator(_injections);
            _constructors = new ConstructorsManager(_injections);
            _properties = new PropertiesManager();
            _creator = new ObjectCreator(_injections, _constructors, _properties);
        }

        public T Resolve<T>() where T : class
        {
            T cretedObject = (T)_creator.Get(typeof(T));
            return cretedObject;
        }

        public T Resolve<T>(dynamic constructorParamethers = null, dynamic customProperties = null) where T : class
        {
            return (T)_creator.Get(typeof(T), constructorParamethers, customProperties);
        }

        public void ResolveProperties(Object obj)
        {
            _creator.InjectProperties(obj);
        }

        public void ResolveProperties(Object obj, dynamic customProperties)
        {
            _creator.InjectProperties(obj, customProperties);
        }

        public Resolver Register<I, C>(bool singleton = false) where C : class,I
        {
            _registrator.Add<I, C>(singleton);
            return this;
        }

        public Resolver AsSingleton() 
        {
            _registrator.LastAsSingleton();
            return this;
        }

        public Resolver RegisterAllFromNamespace<T>() where T : class
        {
            _registrator.ResolveAllFromNamespace<T>();
            return this;
        }

        public Resolver RegisterAllFromNamespace(string name)
        {
            _registrator.ResolveAllFromNamespace(name);
            return this;
        }

        public Resolver ExecuteAllRegistrations()
        {
            _registrator.Execute();
            return this;
        }

        public Resolver CheckCohesion()
        {
            var cohesionChecker = new CohesionChecker(_injections, _constructors, _properties);
            cohesionChecker.CheckCohesion();
            return this;
        }
    }
}
