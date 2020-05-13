using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUnit.Framework;
using Plotly.Blazor.Common;

namespace Plotly.Blazor.Tests
{
    /// <summary>
    /// Class TestClass.
    /// </summary>
    public class TestClass
    {
        [JsonConverter(typeof(FlagConverter))] 
        public TestFlag TestFlag { get; set; } = TestFlag.Enum1 | TestFlag.Enum2;
    }

    /// <summary>
    /// Enum TestEnum
    /// </summary>
    public enum TestEnum
    {
        Enum1
    }

    /// <summary>
    /// Enum TestFlag
    /// </summary>
    [Flags]
    public enum TestFlag
    {
        None = 0,
        Enum1 = 1,
        Enum2 = 2,
        Enum3 = 4
    }

    public class Tests
    {
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Defines the test method FlagConverterTest.
        /// </summary>
        [Test]
        public void FlagConverterTest()
        {
            // Check if non-flag enums are detected
            Assert.DoesNotThrow(() => TestFlag.Enum1.GetComposition());
            Assert.Throws<ArgumentException>(() => TestEnum.Enum1.GetComposition());

            // Test the composition
            Assert.AreEqual("Enum1", TestFlag.Enum1.GetComposition());
            Assert.AreEqual("None", TestFlag.None.GetComposition());
            Assert.AreEqual("Enum1+Enum2", (TestFlag.Enum1 | TestFlag.Enum2).GetComposition());
            Assert.AreEqual("Enum1+Enum2+Enum3", (TestFlag.Enum1 | TestFlag.Enum2 | TestFlag.Enum3).GetComposition());

            // Test converter methods (read, write)
            var expected = new TestClass();
            var actual = JsonSerializer.Deserialize<TestClass>(JsonSerializer.Serialize(expected));
            Assert.AreEqual(expected.TestFlag, actual?.TestFlag);

            // Test none value
            expected.TestFlag = TestFlag.None;
            actual = JsonSerializer.Deserialize<TestClass>(JsonSerializer.Serialize(expected));
            Assert.AreEqual(expected.TestFlag, actual?.TestFlag);
        }
    }
}