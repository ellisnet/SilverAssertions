//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;

namespace DennisDoomen.ChillBdd
{
    internal class AutofacChillContainer : IChillContainer
    {
        private ILifetimeScope _container;
        private ContainerBuilder _containerBuilder;

        //Added to resolve post-container-build issue described below:
        private bool _isContainerBuilt;

        private readonly Dictionary<string, Type> _registeredTypes = new ();

        public AutofacChillContainer()
            : this(new ContainerBuilder())
        {
        }

        public AutofacChillContainer(ILifetimeScope container)
        {
            _container = container;
            //Added to resolve post-container-build issue described below:
            if (container != null)
            {
                _isContainerBuilt = true;
            }
        }

        public AutofacChillContainer(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        protected ILifetimeScope Container
        {
            get
            {
                if (_container == null)
                {
                    _container = _containerBuilder.Build();
                    //Added to resolve post-container-build issue described below:
                    _isContainerBuilt = true;
                }
                
                return _container;
            }
        }

        public void Dispose()
        {
            Container.Dispose();
        }

        //TODO: The original code below relies (I believe) on functionality that is no longer present in Autofac.
        //  I don't think that injectables can be registered after the container is built, anymore.

        //Original code:
        /*
        public void RegisterType<T>() where T : class
        {
            Container.ComponentRegistry.Register(RegistrationBuilder.ForType<T>().InstancePerLifetimeScope()
                .CreateRegistration());
        }
        */

        public void RegisterType<T>() where T : class
        {
            if (_isContainerBuilt)
            {
                throw new InvalidOperationException("Cannot Register injectables after the container has been built.");
            }

            if (_containerBuilder == null)
            {
                throw new InvalidOperationException(
                    $"There is no {nameof(ContainerBuilder)} instance available for registration.");
            }

            _containerBuilder.RegisterComponent(RegistrationBuilder.ForType<T>().InstancePerLifetimeScope().CreateRegistration());
            var regType = typeof(T);
            _registeredTypes.TryAdd($"{regType.Name}-{string.Empty}", typeof(T));
        }

        public T Get<T>(string key = null) where T : class
        {
            if (key == null)
            {
                return Container.Resolve<T>();
            }
            else
            {
                return Container.ResolveKeyed<T>(key);
            }
        }

        //TODO: See note above, I don't think injectables can be registered after the container has been built.

        //Original code:
        /*
        public T Set<T>(T valueToSet, string key = null) where T : class
        {
            if (key == null)
            {
                Container.ComponentRegistry
                    .Register(RegistrationBuilder.ForDelegate((c, p) => valueToSet)
                        .InstancePerLifetimeScope().CreateRegistration());
            }
            else
            {
                Container.ComponentRegistry
                    .Register(RegistrationBuilder.ForDelegate((c, p) => valueToSet)
                        .As(new KeyedService(key, typeof(T)))
                        .InstancePerLifetimeScope().CreateRegistration());
            }

            return Get<T>(key);
        }
        */

        public T Set<T>(T valueToSet, string key = null) where T : class
        {
            var result = valueToSet;

            if (_isContainerBuilt)
            {
                var tryGet = Get<T>(key);

                result = tryGet 
                         ?? throw new InvalidOperationException("Cannot Register injectables after the container has been built.");
            }
            else
            {
                if (_containerBuilder == null)
                {
                    throw new InvalidOperationException(
                        $"There is no {nameof(ContainerBuilder)} instance available for registration.");
                }

                var regType = typeof(T);

                if (string.IsNullOrWhiteSpace(key))
                {
                    _containerBuilder
                        .RegisterComponent(RegistrationBuilder.ForDelegate((c, p) => valueToSet)
                            .InstancePerLifetimeScope().CreateRegistration());
                    _registeredTypes.TryAdd($"{regType.Name}-{string.Empty}", typeof(T));
                }
                else
                {
                    _containerBuilder
                        .RegisterComponent(RegistrationBuilder.ForDelegate((c, p) => valueToSet)
                            .As(new KeyedService(key, typeof(T)))
                            .InstancePerLifetimeScope().CreateRegistration());
                    _registeredTypes.TryAdd($"{regType.Name}-{key.Trim().ToLowerInvariant()}", typeof(T));
                }
            }

            return result;
        }

        public bool IsRegistered(Type type) => (_isContainerBuilt)
            ? Container.IsRegistered(type)
            : _registeredTypes.ContainsValue(type);
    }
}
