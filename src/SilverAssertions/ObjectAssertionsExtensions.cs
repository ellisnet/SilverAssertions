using SilverAssertions.Common;
using SilverAssertions.Equivalency;
using SilverAssertions.Execution;
using SilverAssertions.Primitives;
using System;
using System.IO;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary; //See note below regarding CreateCloneUsingBinarySerializer()
using System.Xml.Serialization;

namespace SilverAssertions;

public static class ObjectAssertionsExtensions
{
    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="assertions"></param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeBinarySerializable(this ObjectAssertions assertions, string because = "",
        params object[] becauseArgs)
    {
        return BeBinarySerializable<object>(assertions, options => options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="assertions"></param>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyAssertionOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyAssertionOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionOptions"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeBinarySerializable<T>(this ObjectAssertions assertions,
        Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(options);

        try
        {
            //As of 1/25/2025, we can't do this check, because of the lack of BinaryFormatter functionality in .NET 8
            //  - see note below regarding CreateCloneUsingBinarySerializer()

            throw new NotImplementedException("Binary serialization/deserialization is not available at this time,"
                + " because the BinaryFormatter functionality has been deprecated in .NET 8.");

            /*
            object deserializedObject = CreateCloneUsingBinarySerializer(assertions.Subject);

            EquivalencyAssertionOptions<T> defaultOptions = AssertionOptions.CloneDefaults<T>()
                .RespectingRuntimeTypes().IncludingFields().IncludingProperties();

            ((T)deserializedObject).Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
            */
        }
        catch (Exception exc)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                            + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="assertions"></param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeDataContractSerializable(this ObjectAssertions assertions,
        string because = "", params object[] becauseArgs)
    {
        return BeDataContractSerializable<object>(assertions, options => options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="assertions"></param>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyAssertionOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyAssertionOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionOptions"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeDataContractSerializable<T>(this ObjectAssertions assertions,
        Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(options);

        try
        {
            var deserializedObject = CreateCloneUsingDataContractSerializer(assertions.Subject);

            EquivalencyAssertionOptions<T> defaultOptions = AssertionOptions.CloneDefaults<T>()
                .RespectingRuntimeTypes().IncludingFields().IncludingProperties();

            ((T)deserializedObject).Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
        }
        catch (Exception exc)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                            + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    //TODO: .NET 8 does not want us to use the following code, because BinaryFormatter is completely going away.
    //  As of 1/25/2025, I don't know how to support Binary Serialization/Deserialization without using BinaryFormatter,
    //  so I am just throwing a NotImplementedException above.

    //For what it's worth: CoPilot suggests implementing cloning using Json Serialization as follows:
    //  (But then it wouldn't be useful for a BeBinarySerializable() check.)
    /*
    //using System.Text.Json;
    private static T Clone<T>(T subject)
    {
       var options = new JsonSerializerOptions
       {
           WriteIndented = true,
           TypeInfoResolver = new SimpleBinder(subject.GetType())
       };

       using var stream = new MemoryStream();
       JsonSerializer.Serialize(stream, subject, options);
       stream.Position = 0;
       return JsonSerializer.Deserialize<T>(stream, options);
    }
    */

    /*
    private static object CreateCloneUsingBinarySerializer(object subject)
    {
        using var stream = new MemoryStream();

        var binaryFormatter = new BinaryFormatter
        {
            Binder = new SimpleBinder(subject.GetType())
        };

#pragma warning disable SYSLIB0011, CA2300 // BinaryFormatter is obsoleted, GH-issue 1779 tracks the upcoming removal in .NET 8.0
        binaryFormatter.Serialize(stream, subject);
        stream.Position = 0;
        return binaryFormatter.Deserialize(stream);
#pragma warning restore SYSLIB0011, CA2300
    }
    */

    // ReSharper disable once UnusedType.Local
    private sealed class SimpleBinder : SerializationBinder
    {
        private readonly Type _type;

        public SimpleBinder(Type type)
        {
            _type = type;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (_type.FullName == typeName && _type.Assembly.FullName == assemblyName)
            {
                return _type;
            }

            return null;
        }
    }

    private static object CreateCloneUsingDataContractSerializer(object subject)
    {
        using var stream = new MemoryStream();
        var serializer = new DataContractSerializer(subject.GetType());
        serializer.WriteObject(stream, subject);
        stream.Position = 0;
        return serializer.ReadObject(stream);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="assertions"></param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeXmlSerializable(this ObjectAssertions assertions, string because = "",
        params object[] becauseArgs)
    {
        try
        {
            object deserializedObject = CreateCloneUsingXmlSerializer(assertions.Subject);

            deserializedObject.Should().BeEquivalentTo(assertions.Subject,
                options => options.RespectingRuntimeTypes().IncludingFields().IncludingProperties());
        }
        catch (Exception exc)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                            + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    private static object CreateCloneUsingXmlSerializer(object subject)
    {
        using var stream = new MemoryStream();
        var binaryFormatter = new XmlSerializer(subject.GetType());
        binaryFormatter.Serialize(stream, subject);

        stream.Position = 0;
        return binaryFormatter.Deserialize(stream);
    }
}
