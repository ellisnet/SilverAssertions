//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Defines which Chill Test initializer (and which container) to use for an Assembly or Class. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class)]
    public class ChillContainerInitializerAttribute : Attribute
    {
        /// <summary>
        /// Assigns a Chill Test Initializer to a class / assembly
        /// </summary>
        /// <param name="chillContainerInitializerType"></param>
        public ChillContainerInitializerAttribute(Type chillContainerInitializerType)
        {
            ChillContainerInitializerType = chillContainerInitializerType;
        }

        /// <summary>
        /// The type of test initializer to user. 
        /// </summary>
        public Type ChillContainerInitializerType { get; private set; }
    }
}