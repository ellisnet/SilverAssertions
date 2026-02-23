//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Represents a container to be used to build-up the subject-under-test as well as providing
    /// any dependencies.
    /// </summary>
    public interface IChillContainer : IDisposable
    {
        /// <summary>
        /// Registers a concrete type at the container. 
        /// </summary>
        /// <remarks>
        /// For example, Autofac cannot create objects until you register them.
        /// </remarks>
        void RegisterType<T>() where T : class;

        /// <summary>
        /// Gets a instance of the specified <typeparamref name="T"/> from the container, optionally
        /// registered under <paramref name="key"/>.
        /// </summary>
        /// <returns>
        /// Returns an instance or implementation of the registered type or <c>null</c> if no such type exists in the container. 
        /// </returns>
        T Get<T>(string key = null) where T : class;

        /// <summary>
        /// Sets a value in the container, so that from now on, it will be returned when you call <see cref="Get{T}"/>
        /// </summary>
        T Set<T>(T valueToSet, string key = null) where T : class;

        /// <summary>
        /// Determines whether an instance of this type is currently .
        /// </summary>
        bool IsRegistered(Type type);
    }
}
