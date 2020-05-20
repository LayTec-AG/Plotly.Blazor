using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Plotly.Blazor.Generator.Schema;
using Plotly.Blazor.Generator.Templates.Enumerated;
using Stubble.Core;
using Stubble.Core.Builders;
using WeCantSpell.Hunspell;

namespace Plotly.Blazor.Generator
{
    /// <summary>
    ///     This program generates a blazor wrapper for the plotly.js library.
    ///     Currently Supported: Enums for Config, Traces
    /// </summary>
    internal class Program
    {
        private const string Namespace = "Plotly.Blazor";

        private static SchemaRoot schema;
        private static StubbleVisitorRenderer stubble;
        private static WordList dictionary;

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static async Task Main(string[] args)
        {
            await using var dictionaryStream = File.OpenRead(@"English (American).dic");
            await using var affixStream = File.OpenRead(@"English (American).aff");
            dictionary = await WordList.CreateFromStreamsAsync(dictionaryStream, affixStream);

            schema = await GetSchema();
            stubble = new StubbleBuilder().Build();
            await GenerateConfig(schema);
            await GenerateTraces(schema);
        }

        /// <summary>
        ///     Gets the schema.
        /// </summary>
        /// <returns>Schema Root.</returns>
        private static async Task<SchemaRoot> GetSchema()
        {
            using var httpClient = new HttpClient();

            var schemaJson = await httpClient.GetStringAsync(
                "https://raw.githubusercontent.com/plotly/plotly.js/master/dist/plot-schema.json");

            var serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<SchemaRoot>(schemaJson, serializerOptions);
        }

        /// <summary>
        ///     Generates the configuration.
        /// </summary>
        private static async Task GenerateConfig(SchemaRoot schema)
        {
            foreach (var (key, value) in schema.Config)
            {
                await GenerateFile(key, value, $"{Namespace}.Config");
            }
        }

        private static async Task GenerateTraces(SchemaRoot schema)
        {
            foreach (var (key, value) in schema.Traces)
            {
                foreach (var (attributeKey, attributeValue) in value.Attributes.OtherAttributes)
                {
                    if (!attributeValue.TryToObject<AttributeDescription>(out var attributeDescription))
                    {
                        continue;
                    }

                    await GenerateFile(attributeKey, attributeDescription,
                        $"{Namespace}.Traces.{key.ToCamelCase(dictionary)}");
                }
            }
        }


        private static async Task GenerateFile(string name, AttributeDescription attributeDescription,
            string customNamespace = Namespace)
        {
            // Call it recursively for all nested attributes if its an object
            if (attributeDescription.Role == "object")
            {
                foreach (var (key, value) in attributeDescription.OtherAttributes)
                {
                    if (value.TryToObject<AttributeDescription>(out var otherAttribute))
                    {
                        await GenerateFile(key, otherAttribute, $"{customNamespace}.{key.ToCamelCase(dictionary)}");
                    }
                }
            }

            switch (attributeDescription.ValType)
            {
                case "enumerated":
                    await GenerateEnum(name, attributeDescription, $"{customNamespace}");
                    break;
            }
        }

        private static async Task GenerateEnum(string name, AttributeDescription attributeDescription,
            string customNamespace)
        {
            var enumeratedData = new EnumeratedData
            {
                Name = $"{name.ToCamelCase(dictionary)}Enum",
                Namespace = customNamespace,
                Description = attributeDescription.Description.SplitByCharCountIfWhitespace(70)
            };

            if (attributeDescription.Default != null)
            {
                enumeratedData.DefaultValue = new Value
                {
                    DisplayName = attributeDescription.Default.ToString()?.ToLower(),
                    EnumName = attributeDescription.Default.ToString()?.ToDotNetFriendlyName(dictionary)
                };
            }

            // Get values as string representation (casting does not work!)
            var valuesAsString = attributeDescription.Values.Select(s => s.ToString()).Distinct().ToArray();


            enumeratedData.Values = valuesAsString.Select(s =>
                {
                    // Check for duplicates (it's possible that e and E exists as a valid different enum!)
                    var hasDuplicates = valuesAsString.Where(v => v != s)
                        .Any(v => string.Equals(v, s, StringComparison.CurrentCultureIgnoreCase));

                    return new Value
                    {
                        DisplayName = s,
                        EnumName = s.ToDotNetFriendlyName(dictionary, hasDuplicates)
                    };
                })
                .Where(v => v.EnumName != enumeratedData.DefaultValue?.EnumName);

            var outputDir = customNamespace.GetOutputPathByNameSpace();
            Directory.CreateDirectory(outputDir);

            using var streamReader = new StreamReader(@".\Templates\Enumerated\Enumerated.mustache", Encoding.UTF8);
            var output = await stubble.RenderAsync(await streamReader.ReadToEndAsync(), enumeratedData);
            await File.WriteAllTextAsync($"{outputDir}\\{enumeratedData.Name}.cs", output);
        }
    }
}