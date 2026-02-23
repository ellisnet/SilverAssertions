//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;
using System.Threading.Tasks;

namespace DennisDoomen.ChillBdd.Common
{
    /// <summary>
    /// Some unit test frameworks (like xUnit) have their own synchronization context
    /// that does not work well with blocking waits and can lead to deadlocks.
    /// These methods create the task in the default synchronization context
    /// and blocks until the task is completed.
    /// </summary>
    internal static class TaskExtensions
    {
        public static void ExecuteInDefaultSynchronizationContext(this Action action)
        {
            using (NoSynchronizationContextScope.Enter())
            {
                action();
            }                
        }

        public static TResult ExecuteInDefaultSynchronizationContext<TResult>(this Func<TResult> action)
        {
            using (NoSynchronizationContextScope.Enter())
            {
                return action();
            }                
        }

    }
}