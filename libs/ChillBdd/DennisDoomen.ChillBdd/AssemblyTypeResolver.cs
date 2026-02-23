//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Class that helps to resolve assembly types. If an error occurs while loading assembly types, it will catch and continue
    /// 
    /// This class was adapted from the DefaultReflector from FluentAssertions by Dennis Doomen.
    /// https://github.com/dennisdoomen/fluentassertions/blob/develop/FluentAssertions.Net40/Common/DefaultReflectionProvider.cs
    /// </summary>
    internal static class AssemblyTypeResolver
    {
        public static IEnumerable<Type> GetAllTypesFromAssemblies(IEnumerable<Assembly> assemblies )
        {
            return assemblies
                .Where(a => !IsDynamic(a))
                .SelectMany(GetExportedTypes).ToArray();
        }

        private static bool IsDynamic(Assembly assembly)
        {
            return assembly.GetType().FullName.Contains("InternalAssemblyBuilder");
        }

        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.DefinedTypes.Select(dt => dt.AsType()).ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types;
            }
            catch (Exception)
            {
                return new Type[0];
            }
        }
    }
}