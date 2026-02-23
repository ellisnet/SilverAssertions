//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Convenience class for object mothers that construct a single type of object. 
    /// </summary>
    public abstract class ObjectMother<TTarget> : IObjectMother
    {
        /// <inheritdoc />
        public bool IsFallback => false;

        /// <inheritdoc />
        public bool Applies(Type type)
        {
            return type == typeof(TTarget);
        }

        /// <inheritdoc />
        public object Create(Type type, IChillObjectResolver container) 
        {
            if (!Applies(type))
            {
                throw new InvalidOperationException("ObjectMother only applies to ");
            }

            return Create();
        }

        /// <summary>
        /// Creates an instance of the requested type.
        /// </summary>
        /// <returns></returns>
        protected abstract TTarget Create();
    }
}    