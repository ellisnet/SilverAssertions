//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using System;
using System.Threading.Tasks;

namespace DennisDoomen.ChillBdd
{
    /// <summary>
    /// Extensions that will help to make the Chill syntax a bit more fluent. 
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// The With extension method allows you to further configure an already existing object (or an object created by a mother)
        /// For example:  mother.CreateCustomer().With(x=> x.Name = Erwin).And(x => x.Address = "address") 
        /// </summary>
        /// <typeparam name="TSubject"></typeparam>
        /// <typeparam name="TReturnValue"></typeparam>
        /// <param name="subject"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TSubject With<TSubject, TReturnValue>(this TSubject subject, Func<TSubject, TReturnValue> func)
        {
            func(subject);
            return subject;
        }

        /// <summary>
        /// Exactly the same method as With, but then allows you to chain multiple withs more fluently. 
        /// .With().And().And(); 
        /// </summary>
        /// <typeparam name="TSubject"></typeparam>
        /// <typeparam name="TReturnValue"></typeparam>
        /// <param name="subject"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TSubject And<TSubject, TReturnValue>(this TSubject subject, Func<TSubject, TReturnValue> func)
        {
            func(subject);
            return subject;
        }
    }
}