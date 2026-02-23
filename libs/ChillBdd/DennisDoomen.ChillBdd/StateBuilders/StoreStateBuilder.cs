//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

namespace DennisDoomen.ChillBdd.StateBuilders
{
    /// <summary>
    /// The state builder provides a fluent syntax for writing values in the container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StoreStateBuilder<T> : IStoreStateBuilder<T>
        where T: class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StoreStateBuilder{T}"/> class.
        /// </summary>
        /// <param name="testBase">The test base.</param>
        public StoreStateBuilder(TestBase testBase)
        {
            TestBase = testBase;
        }


        /// <summary>
        /// Write a value in the container
        /// </summary>
        /// <param name="valueToSet"></param>
        /// <returns></returns>
        public virtual TestBase To(T valueToSet)
        {
            TestBase.Decorator.Set(valueToSet);
            return TestBase;
        }

        /// <summary>
        /// A reference to the test class
        /// </summary>
        public TestBase TestBase { get; set; }
    }
}