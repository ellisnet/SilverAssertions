//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

namespace DennisDoomen.ChillBdd.StateBuilders
{
    /// <summary>
    /// Extensions of the State builder
    /// </summary>
    public static class StoreStateBuilderExtensions
    {
        /// <summary>
        /// Allows you to objects in the container under a specified name. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DictionaryStoredStateBuilder<T> Named<T>(this IStoreStateBuilder<T> subject, string name) where T : class
        {
            return new DictionaryStoredStateBuilder<T>(subject.TestBase, name);
        }
    }
}