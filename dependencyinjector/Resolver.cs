﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using DependencyInjector.Internals;

/*
 TO DO: 
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
        InjectionsStorage _injections;
        ConstructorsManager _constructors;
        PropertiesManager _properties;
        ObjectCreator _creator;
        Registrator _registrator;

        public Resolver()
        {
            _injections = new InjectionsStorage();
            _registrator = new Registrator(_injections);
            _constructors = new ConstructorsManager(_injections);
            _properties = new PropertiesManager(_injections);
            _creator = new ObjectCreator(_injections, _constructors, _properties);
        }

        public T Resolve<T>() where T : class
        {
            T cretedObject = (T)_creator.Create(typeof(T));
            ResolveProperties(cretedObject);
            return cretedObject;
        }

        public T Resolve<T>(dynamic constructorParamethers = null, dynamic customProperties = null) where T : class
        {
            T cretedObject;
            if (constructorParamethers == null)
            {
                cretedObject = (T)_creator.Create(typeof(T));               
            }
            else
            {
                cretedObject = (T)_creator.Create(typeof(T), constructorParamethers);
            }

            if (customProperties == null)
            {
                ResolveProperties(cretedObject);
            }
            else
            {
                ResolveProperties(cretedObject, customProperties);
            }         
            return cretedObject;
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
