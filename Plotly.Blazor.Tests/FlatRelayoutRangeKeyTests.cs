using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Plotly.Blazor.Tests;

public class FlatRelayoutRangeKeyTests
{
    [Test]
    public void FlatAxisRangeKeyShouldNotBeDeserializedAsASubplotObject()
    {
        var extensions = typeof(Layout).Assembly.GetType("Plotly.Blazor.Extensions", throwOnError: true)!;
        var populate = extensions.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(method => method.Name == "Populate" && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(Layout));
        var layout = new Layout();

        populate.Invoke(null, [layout, "{\"xaxis.range[0]\":0}"]);
        Assert.That(layout, Is.Not.Null);
    }
}
