using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using Plotly.Blazor.Traces;
using Plotly.Blazor.Traces.ScatterLib;
using Plotly.Blazor.Traces.ScatterLib.ErrorXLib;

namespace Plotly.Blazor.Tests
{
    public class GeneratorTests
    { 
        /// <summary>
        /// Defines the test method EqualsMethodTest.
        /// </summary>
        [Test]
        public void EqualsMethodTest()
        {
            var referenceScatter = new Scatter
            {
                ClipOnAxis = true,
                DX = new decimal(1.23),
                Fill = FillEnum.ToNext,
                HoverTemplateArray = new List<string> {"123", "456"},
                CustomDataSrc = "123",
                ErrorX = new ErrorX
                {
                    CopyYStyle = true,
                    Array = new List<object>(),
                    Type = TypeEnum.Constant
                }
            };

            var equalScatter = new Scatter
            {
                ClipOnAxis = true,
                DX = new decimal(1.23),
                Fill = FillEnum.ToNext,
                HoverTemplateArray = new List<string> {"123", "456"},
                CustomDataSrc = "123",
                ErrorX = new ErrorX
                {
                    CopyYStyle = true,
                    Array = new List<object>(),
                    Type = TypeEnum.Constant
                }
            };

            var notEqualScatter = new Scatter
            {
                ClipOnAxis = null,
                DX = new decimal(1.23),
                Fill = FillEnum.ToNext,
                HoverTemplateArray = new List<string> {"123", "456"},
                CustomDataSrc = "123",
                ErrorX = new ErrorX
                {
                    CopyYStyle = true,
                    Array = new List<object>(),
                    Type = TypeEnum.Constant
                }
            };

            Assert.IsFalse(referenceScatter.Equals(null));

            Assert.IsTrue(referenceScatter.Equals(equalScatter));
            Assert.IsTrue(equalScatter.Equals(referenceScatter));
            Assert.IsTrue(referenceScatter == equalScatter);
            Assert.IsFalse(referenceScatter != equalScatter);

            Assert.IsFalse(referenceScatter.Equals(notEqualScatter));
            Assert.IsFalse(notEqualScatter.Equals(referenceScatter));
            Assert.IsFalse(referenceScatter == notEqualScatter);
            Assert.IsTrue(referenceScatter != notEqualScatter);
        }

    }
}
