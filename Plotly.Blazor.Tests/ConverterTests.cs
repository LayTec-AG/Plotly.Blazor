using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NUnit.Framework;

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
        public string TestProperty { get; set; }

        [JsonPropertyName(@"testProperty2")]
        public override string TestProperty2 { get; set; }
    }

    [JsonConverter(typeof(PlotlyConverter))]
    public class TestSubplotClass : ITestClass
    {
        [JsonPropertyName("testProperty")]
        public string TestProperty { get; set; }

        [Array]
        [JsonPropertyName("testProperty")]
        public IList<string> TestPropertyArray { get; set; }

        [JsonPropertyName("testProperty2")]
        public string TestProperty2 { get; set; }

        [JsonPropertyName("testProperty2")]
        [Array]
#pragma warning disable CA1822 // Mark members as static
        public IList<string> TestProperty2Array => new List<string>();
#pragma warning restore CA1822 // Mark members as static

        [JsonPropertyName("testProperty3")]
        public string TestProperty3 { get; set; }

        [JsonPropertyName("testProperty3")]
        [Array]
#pragma warning disable CA1822 // Mark members as static
        public IList<string> TestProperty3Array => new List<string>();
#pragma warning restore CA1822 // Mark members as static

        [Subplot]
        public IList<TestClass> Items { get; set; }
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
        public string TestProperty { get; set; }
    }

    public abstract class TestClassAbstract
    {
        public abstract string TestProperty2 { get; set; }
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
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = null,
                Converters = { new DateTimeConverter(), new DateTimeOffsetConverter()}
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
        public void PlotlyConverterTest()
        {
            var expected = new TestSubplotClass
            {
                TestProperty = "Test",
                TestProperty2 = "Test2",
                TestProperty3 = "Test3",
                Items = new []{new TestClass{TestFlag = TestFlag.All}, new TestClass()},
            };

            var serialized = JsonSerializer.Serialize(expected, serializerOptions);
            var actual = JsonSerializer.Deserialize<TestSubplotClass>(serialized);

            expected.Should().BeEquivalentTo(actual);
        }

        [Test]
        public void PolymorphicConverterTest()
        {
            var testObj = new TestPolymorphicClass
            {
                InterfaceProperty = new TestClass { TestProperty = "TestInterfaceProperty", TestProperty2 = "TestAbstractProperty" },
                AbstractProperty = new TestClass { TestProperty = "TestInterfaceProperty", TestProperty2 = "TestAbstractProperty" }
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

        [Test]
        public void DateTimeConverterTest()
        {
            Assert.AreEqual("\"2020-05-31\"" , JsonSerializer.Serialize(new DateTime(2020, 5, 31), serializerOptions));
            Assert.AreEqual("\"2020-05-31 12:00:00\"" , JsonSerializer.Serialize(new DateTime(2020, 5, 31, 12, 0, 0), serializerOptions));
        }


        [Test]
        public void DateTimeOffsetConverterTest()
        {
            Assert.AreEqual("\"2020-05-31\"" , JsonSerializer.Serialize(new DateTimeOffset(new DateTime(2020, 5, 31)), serializerOptions));
            Assert.AreEqual("\"2020-05-31 12:00:00\"" , JsonSerializer.Serialize(new DateTimeOffset(new DateTime(2020, 5, 31, 12, 0, 0)), serializerOptions));
        }
    }
}