using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Plotly.Blazor.LayoutLib.XAxisLib;

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
        
        // ensure that the regex does not match "items"
        [JsonPropertyName("notItems")]
        public string NotItems { get; set; }
    }

    [JsonConverter(typeof(PlotlyConverter))]
    public class TestSubplotClass2
    {
        [JsonPropertyName("someItems")]
        public string SomeItems { get; set; }

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
        Enum2,
        [EnumMember(Value=@"true")]
        True,
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
        [EnumMember(Value="false")]
        False = 16,
        [EnumMember(Value="all")]
        All = EnumOne | EnumTwo | EnumThree | False
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
                TestFlag = TestFlag.EnumOne | TestFlag.EnumTwo | TestFlag.False
            };
            var expected = "{\"testFlag\":\"enum1\\u002Benum2\\u002Bfalse\"}";
            var actual = JsonSerializer.Serialize(testObject, serializerOptions);
            Assert.That(actual, Is.EqualTo(expected));

            // Test "all" value
            testObject = new TestClass
            {
                TestFlag = TestFlag.EnumOne | TestFlag.EnumTwo | TestFlag.EnumThree | TestFlag.False
            };
            expected = "{\"testFlag\":\"all\"}";
            actual = JsonSerializer.Serialize(testObject, serializerOptions);
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [TestCase(TestEnum.Enum1, "{\"testEnum\":\"default\"}")]
        [TestCase(TestEnum.True, "{\"testEnum\":true}")]
        public void EnumSerializationTest(
	        TestEnum testEnum,
	        string expectedJson)
        {
            var testObj = new TestClass
            {
                TestEnum = testEnum
            };
            
            var actualJson = JsonSerializer.Serialize(testObj, serializerOptions);
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        [TestCase("{\"testEnum\":\"notDefault\"}", TestEnum.Enum2)]
        [TestCase("{\"testEnum\":true}", TestEnum.True)]
        public void EnumDeserializationTest(
	        string json,
	        TestEnum expectedEnum)
        {
            var actual = JsonSerializer.Deserialize<TestClass>(json, serializerOptions);
            Assert.That(expectedEnum, Is.EqualTo(actual.TestEnum), "Read/Write the enum failed!");
        }

        [Test]
        public void PlotlyConverterTest()
        {
            var expected = new TestSubplotClass
            {
                TestProperty = "Test",
                TestProperty2 = "Test2",
                TestProperty3 = "Test3",
                Items = new []
                {
	                new TestClass{TestFlag = TestFlag.All}, 
	                new TestClass()
                },
                NotItems = "Test4"
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

            ClassicAssert.IsNotNull(interfaceProperty.GetProperty("testProperty"));
            ClassicAssert.IsNotNull(interfaceProperty.GetProperty("testProperty2"));

            ClassicAssert.IsNotNull(abstractProperty.GetProperty("testProperty"));
            ClassicAssert.IsNotNull(abstractProperty.GetProperty("testProperty2"));
        }

        [Test]
        public void DateTimeConverterTest()
        {
            ClassicAssert.AreEqual("\"2020-05-31\"" , JsonSerializer.Serialize(new DateTime(2020, 5, 31), serializerOptions));
            ClassicAssert.AreEqual("\"2020-05-31 12:00:00\"" , JsonSerializer.Serialize(new DateTime(2020, 5, 31, 12, 0, 0), serializerOptions));
        }


        [Test]
        public void DateTimeOffsetConverterTest()
        {
            ClassicAssert.AreEqual("\"2020-05-31\"" , JsonSerializer.Serialize(new DateTimeOffset(new DateTime(2020, 5, 31)), serializerOptions));
            ClassicAssert.AreEqual("\"2020-05-31 12:00:00\"" , JsonSerializer.Serialize(new DateTimeOffset(new DateTime(2020, 5, 31, 12, 0, 0)), serializerOptions));
        }
    }
}