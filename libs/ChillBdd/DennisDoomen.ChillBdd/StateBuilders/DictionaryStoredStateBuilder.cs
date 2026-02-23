//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;
using System.Collections.Generic;

namespace DennisDoomen.ChillBdd.StateBuilders
{
    /// <summary>
    /// Allows you to set a named value the container using a fluent syntax. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DictionaryStoredStateBuilder<T> : StoreStateBuilder<T> where T : class
    {
        /// <summary>
        /// The name specified
        /// </summary>
        public string Named { get; set; }

        /// <summary>
        /// Creates a <see cref="DictionaryStoredStateBuilder{T}"/>
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="named"></param>
        public DictionaryStoredStateBuilder(TestBase testBase, string named) : base(testBase)
        {
            Named = named;
        }

        /// <summary>
        /// Set's the specified named value to the container. 
        /// </summary>
        /// <param name="valueToSet"></param>
        /// <returns></returns>
        public override TestBase To(T valueToSet)
        {
            AppendToDictionary(valueToSet);

            AppendToList(valueToSet);
            return TestBase;
        }

        private void AppendToList(T valueToSet)
        {
            TestBase.Decorator.AddToList(valueToSet);
        }

        private void AppendToDictionary(T valueToSet)
        {
            var dictionary = TestBase.Decorator.Get<Dictionary<Tuple<Type, string>, object>>();

            var key = Tuple.Create(typeof(T), Named);

            if (dictionary == null || dictionary.Count == 0)
            {
                dictionary = new Dictionary<Tuple<Type, string>, object>();
            }

            dictionary[key] = valueToSet;
            TestBase.Decorator.Set(dictionary);
        }
    }
}