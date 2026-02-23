//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;
using System.Threading;

namespace DennisDoomen.ChillBdd.Common
{
    internal static class NoSynchronizationContextScope
    {
        public static DisposingAction Enter()
        {
            var context = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            return new DisposingAction(() => SynchronizationContext.SetSynchronizationContext(context));
        }

        internal class DisposingAction : IDisposable
        {
            private readonly Action action;

            public DisposingAction(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                action();
            }
        }
    }
}