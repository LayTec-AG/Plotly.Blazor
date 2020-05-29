using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using Plotly.Blazor;

namespace Plotly.Blazor.Tests
{
    /// <summary>
    /// Class TestClass.
    /// </summary>
    public class TestClass : TestClassAbstract, ITestClass
    {
        [JsonPropertyName(@"testEnum")] 
        [JsonConverter(typeof(EnumConverter))]
        public TestEnum? TestEnum { get; set; }

        [JsonPropertyName(@"testFlag")]
        [JsonConverter(typeof(EnumConverter))]
        public TestFlag? TestFlag { get; set; }

        [JsonPropertyName(@"testProperty")]
        public object TestProperty { get; set; }

        [JsonPropertyName(@"testProperty2")]
        public override object TestProperty2 { get; set; }
    }

    [JsonConverter(typeof(SubplotConverter))]
    public class TestSubplotClass : ITestClass
    {
        public object TestProperty { get; set; }

        [Subplot]
        public IEnumerable<TestClass> Items { get; set; }
    }

    public class TestPolymorphicClass
    {
        [JsonConverter(typeof(PolymorphicConverter))]
        public ITestClass InterfaceProperty { get; set; }
        [JsonConverter(typeof(PolymorphicConverter))]
        public TestClassAbstract AbstractProperty { get; set; }
    }

    public interface ITestClass
    {
        public object TestProperty { get; set; }
    }

    public abstract class TestClassAbstract
    {
        public abstract object TestProperty2 { get; set; }
    }

    /// <summary>
    /// Enum TestEnum
    /// </summary>
    public enum TestEnum
    {
        [EnumMember(Value="default")]
        Enum1,
        [EnumMember(Value="notDefault")]
        Enum2
    }

    /// <summary>
    /// Enum TestFlag
    /// </summary>
    [Flags]
    public enum TestFlag
    {
        [EnumMember(Value="skip")]
        Skip = 0,
        [EnumMember(Value="none")]
        None = 1,
        [EnumMember(Value="enum1")]
        EnumOne = 2,
        [EnumMember(Value="enum2")]
        EnumTwo = 4,
        [EnumMember(Value="enum3")]
        EnumThree = 8,
        [EnumMember(Value="all")]
        All = EnumOne | EnumTwo | EnumThree
    }

    public class ConverterTests
    {
        private JsonSerializerOptions serializerOptions;

        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = null
            };
        }

        /// <summary>
        /// Defines the test method FlagConverterTest.
        /// </summary>
        [Test]
        public void FlagConvertingTest()
        {
            // Test default serialization with EnumMemberAttribute
            var testObject = new TestClass
            {
                TestFlag = TestFlag.EnumOne | TestFlag.EnumTwo
            };
            var expected = "{\"testFlag\":\"enum1\\u002Benum2\"}";
            var actual = JsonSerializer.Serialize(testObject, serializerOptions);
            Assert.AreEqual(expected, actual,  "Joining the flags failed!");

            // Test "all" value
            testObject = new TestClass
            {
                TestFlag = TestFlag.EnumOne | TestFlag.EnumTwo | TestFlag.EnumThree
            };
            expected = "{\"testFlag\":\"all\"}";
            actual = JsonSerializer.Serialize(testObject, serializerOptions);
            Assert.AreEqual(expected, actual, "Read/Write the 'all' flag failed!");
        }

        [Test]
        public void EnumConvertingTest()
        {
            // Test default serialization with EnumMemberAttribute
            var testObj = new TestClass
            {
                TestEnum = 0
            };
            var expectedJson = "{\"testEnum\":\"default\"}";
            var actualJson = JsonSerializer.Serialize(testObj, serializerOptions);
            Assert.AreEqual(expectedJson, actualJson, "Serializing enum with custom EnumMemberAttribute failed!");

            // Test read, write for other value 
            testObj = new TestClass
            {
                TestEnum = TestEnum.Enum2
            };
            var actual = JsonSerializer.Deserialize<TestClass>(JsonSerializer.Serialize(testObj, serializerOptions));
            Assert.AreEqual(testObj.TestEnum, actual.TestEnum, "Read/Write the enum failed!");
        }

        [Test]
        public void SubplotConverterTest()
        {
            var testObj = new TestSubplotClass
            {
                Items = new []{new TestClass{TestFlag = TestFlag.All}, new TestClass()}
            };

            var actual = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(testObj, serializerOptions));
            Assert.NotNull(actual.GetProperty("Items"));
            Assert.NotNull(actual.GetProperty("Items2"));
        }

        [Test]
        public void PolymorphicConverterTest()
        {
            var testObj = new TestPolymorphicClass
            {
                InterfaceProperty = new TestClass{TestProperty = "TestInterfaceProperty", TestProperty2 = "TestAbstractProperty"},
                AbstractProperty = new TestClass{TestProperty = "TestInterfaceProperty", TestProperty2 = "TestAbstractProperty"}
            };

            var actual = JsonSerializer.Serialize(testObj, serializerOptions);

            var deserialized = JsonSerializer.Deserialize<JsonElement>(actual);

            var interfaceProperty = deserialized.GetProperty("InterfaceProperty");
            var abstractProperty = deserialized.GetProperty("AbstractProperty");

            Assert.IsNotNull(interfaceProperty.GetProperty("testProperty"));
            Assert.IsNotNull(interfaceProperty.GetProperty("testProperty2"));

            Assert.IsNotNull(abstractProperty.GetProperty("testProperty"));
            Assert.IsNotNull(abstractProperty.GetProperty("testProperty2"));
        }
    }
}