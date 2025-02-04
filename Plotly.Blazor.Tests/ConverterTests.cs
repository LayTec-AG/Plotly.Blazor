using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
        public IList<string> TestPropertyArray { get; init; }

        [JsonPropertyName("testProperty2")]
        public string TestProperty2 { get; set; }

        [JsonPropertyName("testProperty2")]
        [Array]
        public IList<string> TestProperty2Array { get; init; }

        [JsonPropertyName("testProperty3")]
        public string TestProperty3 { get; set; }

        [JsonPropertyName("testProperty3")]
        [Array]
        public IList<string> TestProperty3Array => new List<string>();

        [JsonPropertyName("testProperty4")]
        public string TestProperty4 { get; set; }
        
        [JsonPropertyName("testProperty4")]
        [Array]
        public IList<object> TestProperty4Array { get; init; }

        [Subplot]
        public IList<TestClass> Items { get; set; }
        
        // ensure that the regex does not match "items"
        [JsonPropertyName("notItems")]
        public string NotItems { get; set; }
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
    
    class ClassToTestObjectDeserialization
    {
	    public object ParameterA { get; init; }
	        
	    public IList<object> ParameterB { get; init; }
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
                Converters = { new DateTimeConverter(), new DateTimeOffsetConverter() }
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
                TestProperty2Array = new List<string>() { "abc", null, "1.23", "true" }, //array version of Property2
                TestProperty3 = "Test3", //scalar version of Property3
                Items = new []
                {
	                new TestClass{ TestFlag = TestFlag.All }, 
	                new TestClass()
                },
                NotItems = "Test4"
            };

            var serialized = JsonSerializer.Serialize(expected, serializerOptions);
            var actual = JsonSerializer.Deserialize<TestSubplotClass>(serialized, serializerOptions);

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
        
        /*
         * ObjectTypeResolverConverter
         */
        
        [TestCase("{\"ParameterA\":10,\"ParameterB\":[10,20,null,0]}", 10, new object [] { 10, 20, null, 0 })]
        [TestCase("{\"ParameterA\":1.23456,\"ParameterB\":[1.23,4.56,null,7]}", 1.23456, new object [] { 1.23, 4.56, null, 7 })]
        [TestCase("{\"ParameterA\":true,\"ParameterB\":[true,false,null,true]}", true, new object [] { true, false, null, true })]
        [TestCase("{\"ParameterA\":\"abc\",\"ParameterB\":[\"abc\",\"def\",null,\"ghi\"]}", "abc", new object [] { "abc", "def", null, "ghi", })]
        public void ShouldRoundtripPrimitiveTypes(
	        string json,
	        object expectedParameterValue,
	        IList<object> expectedParameterValues)
        {
	        var options = new JsonSerializerOptions()
	        {
		        Converters = { new ObjectTypeResolverConverter() }
	        };
	        
	        var expected = new ClassToTestObjectDeserialization()
	        {
		        ParameterA = expectedParameterValue,
		        ParameterB = expectedParameterValues
	        };
	        
	        var actual = JsonSerializer.Deserialize<ClassToTestObjectDeserialization>(json, options);
	        expected.Should().BeEquivalentTo(actual);

	        var roundTrippedJson = JsonSerializer.Serialize(expected, options);
	        json.Should().Match(roundTrippedJson);
        }
        
        [Test]
        public void ShouldDeserializeComplexTypesToDictionary()
        {
	        var options = new JsonSerializerOptions()
	        {
		        Converters = { new ObjectTypeResolverConverter() }
	        };

	        var json = "{\"ParameterA\":{\"PropertyA\":\"abc\",\"PropertyB\":true},\"ParameterB\":[{\"PropertyA\":\"def\",\"PropertyB\":2},{\"PropertyA\":\"ghi\",\"PropertyB\":1.23}]}";
	        
	        var expectedObject = new ClassToTestObjectDeserialization()
	        {
		        ParameterA = new Dictionary<string, object>()
		        {
			        ["PropertyA"] = "abc",
			        ["PropertyB"] = true,
		        },
		        ParameterB = new List<object>()
		        {
			        new Dictionary<string, object>()
			        {
				        ["PropertyA"] = "def",
				        ["PropertyB"] = 2,
			        },
			        new Dictionary<string, object>()
			        {
				        ["PropertyA"] = "ghi",
				        ["PropertyB"] = 1.23m,
			        },
		        }
	        };

	        var actualObject = JsonSerializer.Deserialize<ClassToTestObjectDeserialization>(json, options);
	        expectedObject.Should().BeEquivalentTo(actualObject);
	        
	        var roundTrippedJson = JsonSerializer.Serialize(expectedObject, options);
	        json.Should().Match(roundTrippedJson);
        }
        
        [Test]
        public void ShouldRoundTripLists()
        {
	        var options = new JsonSerializerOptions()
	        {
		        Converters = { new ObjectTypeResolverConverter() }
	        };

	        var json = "{\"ParameterA\":{\"PropertyA\":\"abc\",\"PropertyB\":true},\"ParameterB\":[{\"PropertyA\":\"def\",\"PropertyB\":2},null,1.23,\"abc\",true]}";
	        
	        var expectedObject = new ClassToTestObjectDeserialization()
	        {
		        ParameterA = new Dictionary<string, object>()
		        {
			        ["PropertyA"] = "abc",
			        ["PropertyB"] = true,
		        },
		        ParameterB = new List<object>()
		        {
			        new Dictionary<string, object>()
			        {
				        ["PropertyA"] = "def",
				        ["PropertyB"] = 2,
			        },
			        null,
			        1.23m,
			        "abc",
			        true
		        }
	        };

	        var actualObject = JsonSerializer.Deserialize<ClassToTestObjectDeserialization>(json, options);
	        expectedObject.Should().BeEquivalentTo(actualObject);
	        
	        var roundTrippedJson = JsonSerializer.Serialize(expectedObject, options);
	        json.Should().Match(roundTrippedJson);
        }
    }
}