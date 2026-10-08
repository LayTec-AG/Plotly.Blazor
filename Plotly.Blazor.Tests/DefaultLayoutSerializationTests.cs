using System.Text.Json;
using NUnit.Framework;

namespace Plotly.Blazor.Tests;

public class DefaultLayoutSerializationTests
{
    [Test]
    public void DefaultJsonSerializerShouldSerializeAnEmptyLayout()
    {
        var json = JsonSerializer.Serialize(new Layout());

        using var document = JsonDocument.Parse(json);
        Assert.That(document.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Object));
    }
}
