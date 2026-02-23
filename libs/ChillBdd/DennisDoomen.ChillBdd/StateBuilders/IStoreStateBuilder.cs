//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

namespace DennisDoomen.ChillBdd.StateBuilders
{
    /// <summary>
    /// Interface for 'storestatebuilders'. This is the fluent syntax for storing state in the container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStoreStateBuilder<T>
    {
        /// <summary>
        /// Testclass that's used in the tests. 
        /// </summary>
        TestBase TestBase { get; set; }

        /// <summary>
        /// Set the specified value in the container. 
        /// </summary>
        /// <param name="valueToSet"></param>
        /// <returns></returns>
        TestBase To(T valueToSet);

    }
}