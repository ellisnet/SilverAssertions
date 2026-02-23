//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System.Collections.Generic;
using System.Reflection;
using System;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Interface for classes that initialize chill containers. 
    /// </summary>
    public interface IChillContainerInitializer
    {
        /// <summary>
        /// Builds the chill container.
        /// </summary>
        /// <param name="test">The test.</param>
        /// <returns></returns>
        IChillContainer BuildChillContainer(TestBase test);

        /// <summary>
        /// Initializes the container.
        /// </summary>
        /// <param name="test">The test.</param>
        void InitializeContainer(TestBase test);
    }
}