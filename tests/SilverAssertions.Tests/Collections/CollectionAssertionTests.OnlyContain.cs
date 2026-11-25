using System;
using System.Collections.Generic;
using System.Linq;
using SilverAssertions.Execution;
using Xunit;
using Xunit.Sdk;

namespace SilverAssertions.Tests.Collections;

/// <content>
/// The OnlyContain specs.
/// </content>
public partial class CollectionAssertionTests
{
    public class OnlyContain
    {
        [Fact]
        public void When_a_collection_does_not_contain_the_unexpected_items_it_should_not_be_enumerated_twice()
        {
            // Arrange
            var collection = new OneTimeEnumerable<int>(1, 2, 3);

            // Act
            Action act = () => collection.Should().OnlyContain(i => i > 3);

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain only items matching*");
        }

        [Fact]
        public void When_injecting_a_null_predicate_into_OnlyContain_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new int[] { };

            // Act
            Action act = () => collection.Should().OnlyContain(predicate: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("predicate");
        }

        [Fact]
        public void When_a_collection_contains_items_not_matching_a_predicate_it_should_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 2, 12, 3, 11, 2 };

            // Act
            Action act = () => collection.Should().OnlyContain(i => i <= 10, "10 is the maximum");

            // Assert
            act.Should().Throw<XunitException>().WithMessage(
                "Expected collection to contain only items matching (i <= 10) because 10 is the maximum, but {12, 11} do(es) not match.");
        }

        [Fact]
        public void When_a_collection_is_empty_and_should_contain_only_items_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<string> strings = Enumerable.Empty<string>();

            // Act
            Action act = () => strings.Should().OnlyContain(e => e.Length > 0);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_empty_array_should_contain_only_items_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            int[] collection = new int[] { };

            // Act
            Action act = () => collection.Should().OnlyContain(i => i > 10, "we want to test the empty case");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_empty_dictionary_should_contain_only_items_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            var dictionary = new Dictionary<int, string>();

            // Act
            Action act = () => dictionary.Should().OnlyContain(pair => pair.Key > 0);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_collection_is_empty_the_predicate_passed_to_OnlyContain_should_not_be_invoked()
        {
            // Arrange
            IEnumerable<int> collection = new OneTimeEnumerable<int>();
            int invocations = 0;

            Func<int, bool> countingPredicate = _ =>
            {
                invocations++;
                return true;
            };

            // Act
            collection.Should().OnlyContain(i => countingPredicate(i));

            // Assert
            invocations.Should().Be(0);
        }

        [Fact]
        public void When_a_collection_is_empty_OnlyContain_should_allow_chaining()
        {
            // Arrange
            IEnumerable<int> collection = Enumerable.Empty<int>();

            // Act
            Action act = () => collection.Should().OnlyContain(i => i > 0).And.BeEmpty();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_collection_contains_only_items_matching_a_predicate_it_should_not_throw()
        {
            // Arrange
            IEnumerable<int> collection = new[] { 2, 9, 3, 8, 2 };

            // Act
            Action act = () => collection.Should().OnlyContain(i => i <= 10);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_a_collection_is_null_then_only_contains_should_fail()
        {
            // Arrange
            int[] collection = null;

            // Act
            Action act = () =>
            {
                using var _ = new AssertionScope();
                collection.Should().OnlyContain(i => i <= 10, "we want to test the failure {0}", "message");
            };

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage(
                    "Expected collection to contain only items matching (i <= 10) *failure message*," +
                    " but the collection is <null>.");
        }
    }
}
