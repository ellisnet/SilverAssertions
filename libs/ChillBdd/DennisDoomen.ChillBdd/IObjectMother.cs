//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Represents an object mother that is automatically discovered by Chill to construct test objects
    /// whenever you request one using <see cref="TestBase.The{T}"/>.
    /// </summary>
    public interface IObjectMother
    {
        /// <summary>
        /// Gets a value indicating whether this object mother is supposed to be used as a fallback in a case
        /// a more specific object mother is not available. 
        /// </summary>
        bool IsFallback { get; }

        /// <summary>
        /// Gets a value indicating whether this object mother can build objects of the specified <paramref name="type"/>. 
        /// </summary>
        bool Applies(Type type);

        /// <summary>
        /// Creates a test instance that is of, inherits from or implements the specified <paramref name="type"/>.
        /// </summary>
        /// <remarks>
        /// The implementation can use the provided <paramref name="objectResolver"/> to get other objects
        /// from either Chill's configured container or another object mother.    
        /// </remarks>
        object Create(Type type, IChillObjectResolver objectResolver);
    }

    /// <summary>
    /// Represents an object that allows resolving an object from the Chill container. 
    /// </summary>
    public interface IChillObjectResolver
    {
        /// <summary>
        /// Gets a value of the specified type from the container, optionally registered under a key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        T Get<T>(string key = null) where T : class;
    }
}