using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Plotly.Blazor.Tests;

public class NestedLayoutPopulateTests
{
    [Test]
    public void PopulateShouldMergeNestedLayoutProperties()
    {
        var extensions = typeof(Layout).Assembly.GetType("Plotly.Blazor.Extensions", throwOnError: true)!;
        var populate = extensions.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Single(method => method.Name == "Populate" && method.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(Layout));
        var layout = new Layout();

        populate.Invoke(null, [layout, "{\"title\":{\"text\":\"Updated\"}}"]);

        Assert.That(layout.Title?.Text, Is.EqualTo("Updated"));

        layout.Title.X = 0.5m;
        populate.Invoke(null, [layout, "{\"title\":{\"text\":\"Updated again\"}}"]);

        Assert.That(layout.Title.Text, Is.EqualTo("Updated again"));
        Assert.That(layout.Title.X, Is.EqualTo(0.5m));
    }
}
