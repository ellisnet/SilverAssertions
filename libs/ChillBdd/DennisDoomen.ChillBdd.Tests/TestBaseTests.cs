//IMPORTANT NOTE: The code in this file was retrieved from https://github.com/ChillBDD/Chill on 2/8/2025 - where no open source
//    (or other) code licenses or copyrights are asserted. It seems likely that the code below is in the public domain.
//  However, according NuGet - https://www.nuget.org/packages/Chill - as of 2/8/2025, Chill version 4.1.0 published 12/4/2020 -
//    the code for the Chill package is available via the permissible MIT open source license: https://opensource.org/license/MIT

using DennisDoomen.ChillBdd.Tests.TestSubjects;
using SilverAssertions;
using System;
using System.Threading;
using Xunit;

#pragma warning disable xUnit1006

// ReSharper disable xUnit1025

namespace DennisDoomen.ChillBdd.Tests
{
    public abstract class TestBaseTests : TestBase
    {
        [Fact]
        public void Can_get_mock_for_subject()
        {
            The<ITestService>().Should().NotBeNull();
        }

        [Fact]
        public void Can_get_named_service()
        {
            TheNamed<ITestService>("abc").Should().NotBeNull();
        }

        [Fact]
        public void Can_register_object()
        {
            UseThe(new TestSubjects.AClass("abc"));
            The<TestSubjects.AClass>().Name.Should().Be("abc");
        }

        [Fact]
        public void Can_register_named_object()
        {
            UseThe(new TestSubjects.AClass("abc"));
            The<TestSubjects.AClass>().Name.Should().Be("abc");
        }

        [Fact]
        public void Can_register_object_fluently()
        {
            SetThe<TestSubjects.AClass>().To(new AClass("abc"));
            The<AClass>().Name.Should().Be("abc");
        }

        [Fact]
        public void Can_get_different_named_objects()
        {
            UseThe(new TestSubjects.AClass("abc"), "abcName");
            UseThe(new TestSubjects.AClass("def"), "defName");

            TheNamed<TestSubjects.AClass>("abcName").Name.Should().Be("abc");
            TheNamed<TestSubjects.AClass>("defName").Name.Should().Be("def");
        }


        /// <summary>
        /// Testing if Dispose() is called on our subjects is tricky, because this happens AFTER a test, it's hard to test if it is actually called
        /// Here's a trick i'm doing. I'm incrementing a value in a test and decrementing it in the constructor. Then i'm using locking to ensure these tests
        /// only execute one at a time. 
        /// 
        /// If the dispose method was not called, then 
        ///     1. the tests would likely not complete because the monitor.exit is never called. 
        ///     2. the Alive property will sometimes be true. 
        /// 
        /// The Theory and the InlineData attribute causes the test to be called several times. 
        /// </summary>

        #region Tests_if_disposable_is_called

        [Theory, InlineData()]

        // ReSharper disable once xUnit1006
        public void Will_dispose_object()
        {
            DisposableObject.DisposableObjectAlive.Should().BeFalse();
            var target = new DisposableObject();
            UseThe(target);
        }

        #endregion
    }

    public class DisposableObject : IDisposable
    {
        static object syncroot = new object();
        public static bool DisposableObjectAlive = false;

        public DisposableObject()
        {
            Monitor.Enter(syncroot);
            DisposableObjectAlive = true;
        }

        public void Dispose()
        {
            DisposableObjectAlive = false;
            Monitor.Exit(syncroot);
        }
    }
}
