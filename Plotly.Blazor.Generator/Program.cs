using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Plotly.Blazor.Generator.Schema;
using Plotly.Blazor.Generator.Templates.Class;
using Plotly.Blazor.Generator.Templates.Enumerated;
using Plotly.Blazor.Generator.Templates.Flag;
using Plotly.Blazor.Generator.Templates.InterfaceData;
using Stubble.Core;
using Stubble.Core.Builders;
using WeCantSpell.Hunspell;
using Property = Plotly.Blazor.Generator.Templates.Class.Property;

namespace Plotly.Blazor.Generator
{
    /// <summary>
    ///     This program generates a blazor wrapper for the plotly.js library.
    ///     Currently Supported: Enums for Config, Traces
    /// </summary>
    internal class Program
    {
        private const string Namespace = "Plotly.Blazor";

        private static SchemaRoot _schema;
        private static StubbleVisitorRenderer _stubble;
        private static WordList _dictionary;

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static async Task Main(string[] args)
        {
            await using var dictionaryStream = File.OpenRead(@"English (American).dic");
            await using var affixStream = File.OpenRead(@"English (American).aff");
            _dictionary = await WordList.CreateFromStreamsAsync(dictionaryStream, affixStream);

            _schema = await GetPlotlySchemaAsync();
            _stubble = new StubbleBuilder().Configure(settings =>
            {
                settings.SetIgnoreCaseOnKeyLookup(true);
                settings.SetEncodingFunction((s) => s);
            })
                .Build();

            var tasks = new[]
            {
                GenerateAnimationsAsync(),
                GenerateTransformsAsync(),
                GenerateFramesAsync(),
                GenerateLayoutAsync(),
                GenerateConfigAsync(),
                GenerateTracesAsync()
            };

            Task.WaitAll(tasks);

            await File.WriteAllLinesAsync("UnknownWords.txt", Helper.UnknownWords.Distinct());
        }

        /// <summary>
        ///     Gets the schema.
        /// </summary>
        /// <returns>Schema Root.</returns>
        private static async Task<SchemaRoot> GetPlotlySchemaAsync()
        {
            using var httpClient = new HttpClient();

            var schemaJson = await httpClient.GetStringAsync(
                "https://raw.githubusercontent.com/plotly/plotly.js/master/dist/plot-schema.json");

            // Write latest .js-File
            var outputDir = @".\src\wwwroot";
            Directory.CreateDirectory(outputDir);
            await File.WriteAllTextAsync($"{outputDir}\\plotly-latest.min.js", await httpClient.GetStringAsync("https://cdn.plot.ly/plotly-latest.min.js"));

            var serializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<SchemaRoot>(schemaJson, serializerOptions);
        }

        /// <summary>
        ///     Generates the animations.
        /// </summary>
        private static async Task GenerateAnimationsAsync()
        {
            foreach (var (key, value) in _schema.Animation)
            {
                await GenerateAsync(key, value, $"{Namespace}.AnimationLib");
            }

            await GenerateClassFileAsync("Animation", _schema.Animation, $"{Namespace}", hasNestedComplexAttributes: true);
        }

        /// <summary>
        ///     Generates the configuration.
        /// </summary>
        private static async Task GenerateConfigAsync()
        {
            foreach (var (key, value) in _schema.Config)
            {
                await GenerateAsync(key, value, $"{Namespace}.ConfigLib");
            }
            await GenerateClassFileAsync("Config", _schema.Config, $"{Namespace}", hasNestedComplexAttributes: true);
        }

        /// <summary>
        /// Generates the transform interface.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private static async Task GenerateTransformInterfaceAsync()
        {
            var interfaceData = new InterfaceData
            {
                Name = "ITransform",
                Namespace = $"{Namespace}",
                Description = new[] { "The transform interface." },
                Properties = new[]{new Templates.InterfaceData.Property
                {
                    PropertyDescription = new []{"The type of the transform."},
                    TypeName = "TransformTypeEnum",
                    PropertyName = "Type",
                    DisplayName = "type",
                    IsReadOnly = true
                }}
            };
            await RenderFileAsync(interfaceData.Namespace, interfaceData.Name, @".\Templates\Interface\Interface.txt",
                interfaceData);
        }

        /// <summary>
        /// Generates the transform classes.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private static async Task GenerateTransformClassesAsync()
        {
            foreach (var (transformKey, transformValue) in _schema.Transforms)
            {
                var friendlyName = transformKey.ToDotNetFriendlyName(_dictionary);
                await GenerateClassFileAsync(transformKey, transformValue.Attributes, $"{Namespace}.Transforms",
                    "ITransform", new[]
                    {
                        new Property
                        {
                            DisplayName = "type",
                            PropertyName = "Type",
                            TypeName = "TransformTypeEnum",
                            IsReadOnly = true,
                            DefaultValue = $"TransformTypeEnum.{friendlyName}"
                        }
                    }, hasNestedComplexAttributes: true);
            }
        }

        /// <summary>
        /// Generates the transforms interface.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private static async Task GenerateTransformTypeEnumAsync()
        {
            var typeEnumData = new EnumeratedData
            {
                Name = "TransformTypeEnum",
                Namespace = $"{Namespace}",
                Description = new[] { "Determines the type of the transform." },
                Values = _schema.Transforms.Select(keyValue => new EnumeratedValue
                {
                    DisplayName = keyValue.Key,
                    EnumName = keyValue.Key.ToDotNetFriendlyName(_dictionary)
                })
            };

            await RenderFileAsync(typeEnumData.Namespace, typeEnumData.Name, @".\Templates\Enumerated\Enumerated.txt",
                typeEnumData);
        }

        private static async Task RenderFileAsync(string @namespace, string fileName, string templatePath, object data)
        {
            var outputDir = @namespace.GetOutputPathByNameSpace();
            Directory.CreateDirectory(outputDir);
            using var streamReader = new StreamReader(templatePath, Encoding.UTF8);
            var output = await _stubble.RenderAsync(await streamReader.ReadToEndAsync(), data);
            await File.WriteAllTextAsync($"{outputDir}\\{fileName}.cs", output);
        }


        /// <summary>
        ///     Generates the configuration.
        /// </summary>
        private static async Task GenerateTransformsAsync()
        {
            // Generate all nested classes
            foreach (var (transformKey, transformValue) in _schema.Transforms)
            {
                foreach (var (key, value) in transformValue.Attributes)
                {
                    if (value is JsonElement attributeValue)
                    {
                        if (!attributeValue.TryToObject<AttributeDescription>(out var attributeDescription))
                        {
                            continue;
                        }
                        await GenerateAsync(key, attributeDescription, $"{Namespace}.Transforms.{transformKey.ToDotNetFriendlyName(_dictionary)}Lib");
                    }
                }
            }

            await GenerateTransformInterfaceAsync();
            await GenerateTransformTypeEnumAsync();
            await GenerateTransformClassesAsync();
        }

        /// <summary>
        ///     Generates the configuration.
        /// </summary>
        private static async Task GenerateFramesAsync()
        {
            await GenerateClassFileAsync("Frames", _schema.Frames.Items.Frames_Entry.OtherAttributes, $"{Namespace}",
                hasNestedComplexAttributes: false);

        }

        /// <summary>
        ///     Generates the layout.
        /// </summary>
        private static async Task GenerateLayoutAsync()
        {
            foreach (var (key, value) in _schema.Layout.LayoutAttributes)
            {
                if (value.ValueKind != JsonValueKind.Undefined)
                {

                    if (!value.TryToObject<AttributeDescription>(out var attributeDescription))
                    {
                        continue;
                    }

                    await GenerateAsync(key, attributeDescription, $"{Namespace}.LayoutLib");
                }
            }
            await GenerateClassFileAsync("Layout", _schema.Layout.LayoutAttributes, $"{Namespace}", hasNestedComplexAttributes: true);
        }

        private static async Task GenerateTracesAsync()
        {
            foreach (var (key, value) in _schema.Traces)
            {
                foreach (var (attributeKey, attributeValue) in value.Attributes)
                {
                    if (!attributeValue.TryToObject<AttributeDescription>(out var attributeDescription))
                    {
                        continue;
                    }

                    await GenerateAsync(attributeKey, attributeDescription,
                        $"{Namespace}.Traces.{key.ToDotNetFriendlyName(_dictionary)}Lib");
                }
            }

            await GenerateTraceInterfaceAsync();
            await GenerateTraceTypeEnumAsync();
            await GenerateTraceClassesAsync();
        }

        /// <summary>
        /// Generates the trace interface.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private static async Task GenerateTraceInterfaceAsync()
        {
            var interfaceData = new InterfaceData
            {
                Name = "ITrace",
                Namespace = $"{Namespace}",
                Description = new[] { "The trace interface." },
                Properties = new[]{new Templates.InterfaceData.Property
                {
                    PropertyDescription = new []{"The type of the trace."},
                    TypeName = "TraceTypeEnum",
                    PropertyName = "Type",
                    DisplayName = "type",
                    IsReadOnly = true
                }}
            };
            await RenderFileAsync(interfaceData.Namespace, interfaceData.Name, @".\Templates\Interface\Interface.txt",
                interfaceData);
        }

        /// <summary>
        /// Generates the transforms interface.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private static async Task GenerateTraceTypeEnumAsync()
        {
            var typeEnumData = new EnumeratedData
            {
                Name = "TraceTypeEnum",
                Namespace = $"{Namespace}",
                Description = new[] { "Determines the type of the trace." },
                Values = _schema.Traces.Select(keyValue => new EnumeratedValue
                {
                    DisplayName = keyValue.Value.Attributes.ContainsKey("type") ? keyValue.Value.Attributes["type"].GetString() : keyValue.Key,
                    EnumName = keyValue.Key.ToDotNetFriendlyName(_dictionary)
                })
            };

            await RenderFileAsync(typeEnumData.Namespace, typeEnumData.Name, @".\Templates\Enumerated\Enumerated.txt",
                typeEnumData);
        }

        /// <summary>
        /// Generates the transform classes.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private static async Task GenerateTraceClassesAsync()
        {
            foreach (var (traceKey, traceValue) in _schema.Traces)
            {
                var friendlyName = traceKey.ToDotNetFriendlyName(_dictionary);
                await GenerateClassFileAsync(traceKey, traceValue.Attributes, $"{Namespace}.Traces",
                    "ITrace", new[]
                    {
                        new Property
                        {
                            DisplayName = "type",
                            PropertyName = "Type",
                            TypeName = "TraceTypeEnum",
                            IsReadOnly = true,
                            DefaultValue = $"TraceTypeEnum.{friendlyName}"
                        }
                    }, hasNestedComplexAttributes: true);
            }
        }


        private static async Task GenerateAsync(string name, AttributeDescription attributeDescription,
            string customNamespace = Namespace)
        {
            var hasNestedComplexAttributes = false;

            // Call it recursively for all nested attributes if its an array
            if (attributeDescription.IsArray)
            {
                var (key, value) = GetArrayType(attributeDescription);
                if (value != null)
                {
                    hasNestedComplexAttributes = value.Role == "object" && !value.IsArray  
                                                 || value.ValType == "flaglist" || value.ValType == "enumerated";
                }

                await GenerateAsync(key, value, $"{customNamespace}");
            }

            // Call it recursively for all nested attributes if its an object
            else if (attributeDescription.Role == "object")
            {
                if (attributeDescription.OtherAttributes != null)
                {
                    hasNestedComplexAttributes = attributeDescription
                        .OtherAttributes
                        .Any(a => a.Value.TryToObject<AttributeDescription>(out var attribute)
                                  && (attribute.Role == "object" && !attribute.IsArray) ||
                                  attribute?.ValType == "flaglist" || attribute?.ValType == "enumerated");

                    foreach (var (key, value) in attributeDescription.OtherAttributes)
                    {
                        if (!value.TryToObject<AttributeDescription>(out var otherAttribute)) continue;

                        if (otherAttribute.ValType == "enumerated")
                        {
                            if(otherAttribute.OtherAttributes["values"].EnumerateArray()
                                .Any(elem => elem.ValueKind == JsonValueKind.String && elem.GetString().StartsWith("/^")))
                            {
                                continue;
                            }
                        }

                        await GenerateAsync(key, otherAttribute, $"{customNamespace}.{name.ToDotNetFriendlyName(_dictionary)}Lib");
                    }
                }
            }

            // Generate files for enums, flags, objects
            if (attributeDescription.ValType == "enumerated")
            {
                await GenerateEnumFileAsync(name, attributeDescription, $"{customNamespace}");
                return;
            }
            if (attributeDescription.ValType == "flaglist")
            {
                await GenerateFlagFileAsync(name, attributeDescription, $"{customNamespace}");
                return;
            }

            if (attributeDescription.Role == "object" && !attributeDescription.IsArray)
            {
                await GenerateClassFileAsync(name, attributeDescription.OtherAttributes, $"{customNamespace}", hasNestedComplexAttributes: hasNestedComplexAttributes);
            }

        }

        private static async Task GenerateEnumFileAsync(string name, AttributeDescription attributeDescription,
            string customNamespace)
        {
            var enumeratedData = new EnumeratedData()
            {
                Name = $"{name.ToDotNetFriendlyName(_dictionary)}Enum",
                Namespace = customNamespace,
                Description = attributeDescription.Description.HtmlEncode().SplitByCharCountIfWhitespace()
            };

            if (attributeDescription.Default != null)
            {
                enumeratedData.DefaultValue = new EnumeratedValue
                {
                    DisplayName = attributeDescription.Default.ToString()?.ToLower(),
                    EnumName = attributeDescription.Default.ToString()?.ToDotNetFriendlyName(_dictionary)
                };
            }

            // Get values as string representation (casting does not work!)

            var valuesAsString = attributeDescription.OtherAttributes["values"].ToObject<object[]>().Select(s => s.ToString()).Distinct().ToArray();

            enumeratedData.Values = valuesAsString.Select(s =>
                {
                    // Check for duplicates (it's possible that e and E exists as a valid different enum!)
                    var hasDuplicates = valuesAsString.Where(v => v != s)
                        .Any(v => string.Equals(v, s, StringComparison.CurrentCultureIgnoreCase));

                    return new EnumeratedValue
                    {
                        DisplayName = s,
                        EnumName = s.ToDotNetFriendlyName(_dictionary, hasDuplicates)
                    };
                })
                .Where(v => v.EnumName != enumeratedData.DefaultValue?.EnumName);

            var outputDir = customNamespace.GetOutputPathByNameSpace();
            Directory.CreateDirectory(outputDir);

            using var streamReader = new StreamReader(@".\Templates\Enumerated\Enumerated.txt", Encoding.UTF8);
            var output = await _stubble.RenderAsync(await streamReader.ReadToEndAsync(), enumeratedData);
            await File.WriteAllTextAsync($"{outputDir}\\{enumeratedData.Name}.cs", output);
        }

        private static async Task GenerateFlagFileAsync(string name, AttributeDescription attributeDescription,
            string customNamespace)
        {
            var flagData = new FlagData
            {
                Name = $"{name.ToDotNetFriendlyName(_dictionary)}Flag",
                Namespace = customNamespace,
                Description = attributeDescription.Description.HtmlEncode().SplitByCharCountIfWhitespace()
            };

            // Get values as string representation (casting does not work!)
            var flagsAsString = attributeDescription.Flags.Select(s => s.ToString()).Distinct().ToArray();

            flagData.Flags = flagsAsString.Select(s =>
            {
                // Check for duplicates (it's possible that e and E exists as a valid different enum!)
                var hasDuplicates = flagsAsString.Where(v => v != s)
                    .Any(v => string.Equals(v, s, StringComparison.CurrentCultureIgnoreCase));

                return new Templates.Flag.FlagValue
                {
                    DisplayName = s,
                    EnumName = s.ToDotNetFriendlyName(_dictionary, hasDuplicates)
                };
            });


            var flagsExtrasAsString = attributeDescription.Extras?.Select(s => s.ToString()).ToArray();

            if (flagsExtrasAsString != null)
            {
                flagData.Extras = flagsExtrasAsString.Select(s =>
                {
                    // Check for duplicates (it's possible that e and E exists as a valid different enum!)
                    var hasDuplicates = flagsExtrasAsString.Where(v => v != s)
                        .Any(v => string.Equals(v, s, StringComparison.CurrentCultureIgnoreCase));

                    return new Templates.Flag.FlagValue
                    {
                        DisplayName = s,
                        EnumName = s.ToDotNetFriendlyName(_dictionary, hasDuplicates)
                    };
                });
            }

            var outputDir = customNamespace.GetOutputPathByNameSpace();
            Directory.CreateDirectory(outputDir);

            using var streamReader = new StreamReader(@".\Templates\Flag\Flag.txt", Encoding.UTF8);
            var output = await _stubble.RenderAsync(await streamReader.ReadToEndAsync(), flagData);
            await File.WriteAllTextAsync($"{outputDir}\\{flagData.Name}.cs", output);
        }


        private static async Task GenerateClassFileAsync(string name, IDictionary<string, JsonElement> attributes,
            string customNamespace, string interfaceName = null, IEnumerable<Property> interfaceProperties = null, bool hasNestedComplexAttributes=false)
        {
            await GenerateClassFileAsync(name, attributes
                ?.Select(pair =>
                    !pair.Value.TryToObject<AttributeDescription>(out var description)
                        ? default
                        : new KeyValuePair<string, AttributeDescription>(pair.Key, description))
                .Where(pair => !pair.Equals(default(KeyValuePair<string, AttributeDescription>)))
                .ToDictionary(pair => pair.Key, pair => pair.Value),
                customNamespace,
                interfaceName,
                interfaceProperties,
                hasNestedComplexAttributes
                );
        }

        private static async Task GenerateClassFileAsync(string name, IDictionary<string, AttributeDescription> attributes,
            string customNamespace, string interfaceName = null, IEnumerable<Property> interfaceProperties = null, bool hasNestedComplexAttributes=false)
        {
            var friendlyName = name.ToDotNetFriendlyName(_dictionary);

            var properties = attributes?.Select(pair =>
                {

                    // GET NAME WITH EDGE CASE THAT THE CLASS NAME EQUALS THE PROPERTY NAME
                    var propertyFriendlyName = pair.Key.ToDotNetFriendlyName(_dictionary);

                    // HANDLE THE CASE WHEN THE PROPERTY NAME EQUALS THE CLASS NAME
                    if (propertyFriendlyName == friendlyName)
                    {
                        propertyFriendlyName = $"_{propertyFriendlyName}";
                    }

                    string typeName;

                    try
                    {
                        typeName = GetTypeByAttributeDescription(pair.Value, pair.Key);
                    }
                    catch(ArgumentException)
                    {
                        // Don't include this property, because it's not supported
                        return null;
                    }

                    return new Property
                    {
                        DisplayName = pair.Key,
                        PropertyName = propertyFriendlyName,
                        TypeName = typeName,
                        PropertyDescription = string.IsNullOrWhiteSpace(pair.Value.Description)
                            ? new[] { $"Gets or sets the {propertyFriendlyName}." }
                            : pair.Value.Description.SplitByCharCountIfWhitespace(),
                        IsSubplot = pair.Value.IsSubplotObj
                    };
                })
                .Where(p => p != null)
                .ToList();

            if (interfaceName != null && interfaceProperties != null)
            {
                var enumerable = interfaceProperties as Property[] ?? interfaceProperties.ToArray();
                foreach (var interfaceProperty in enumerable)
                {
                    interfaceProperty.IsInherited = true;
                }
                properties?.InsertRange(0, enumerable);
            }

            var classData = new ClassData
            {
                Name = friendlyName,
                Interface = interfaceName,
                Namespace = customNamespace,
                Description = new[] { $"The {friendlyName} class." },
                Properties = properties,
                HasNestedComplexAttributes = hasNestedComplexAttributes,
                ReferencesTransform = true
            };

            if (classData.Properties != null && classData.Properties.Any())
            {
                await RenderFileAsync(classData.Namespace, classData.Name, @".\Templates\Class\Class.txt", classData);
            }
        }

        private static async Task GenerateClassFileAsync(string name, AttributeDescription attributeDescription,
            string customNamespace, string interfaceName = null, IEnumerable<Property> interfaceProperties = null, bool hasNestedComplexAttributes=false)
        {
            await GenerateClassFileAsync(name, attributeDescription.OtherAttributes, customNamespace, interfaceName,
                interfaceProperties, hasNestedComplexAttributes);
        }

        /// <summary>
        /// Gets the type by attribute description.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="attributeDescription">The attribute description.</param>
        /// <returns>System.String.</returns>
        private static string GetTypeByAttributeDescription(AttributeDescription attributeDescription, string key = default)
        {

            // EDGE CASES
            switch (key)
            {
                case "transform":
                    return "ITransform";
                case "transforms":
                    return "IList<ITransform>";
                case "data":
                    return "IList<ITrace>";
            }

            // HANDLE REGEX
            if (attributeDescription.ValType == "enumerated")
            {
                if(attributeDescription.OtherAttributes["values"].EnumerateArray()
                    .Any(elem => elem.ValueKind == JsonValueKind.String && elem.GetString().StartsWith("/^")))
                {
                    return "string";
                }
            }

            // Call it recursively for all nested attributes if its an array
            if (attributeDescription.IsArray)
            {
                return $"IList<{GetArrayType(attributeDescription).Key}>";
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                if (attributeDescription.IsSubplotObj)
                {
                    return $"IList<{key.ToDotNetFriendlyName(_dictionary)}>";
                }

                if (attributeDescription.Role == "object")
                {
                    return key.ToDotNetFriendlyName(_dictionary);
                }

                if (attributeDescription.ValType == "enumerated")
                {
                    return $"{key.ToDotNetFriendlyName(_dictionary)}Enum?";
                }

                if (attributeDescription.ValType == "flaglist")
                {
                    return $"{key.ToDotNetFriendlyName(_dictionary)}Flag?";
                }
            }

            switch (attributeDescription.ValType)
            {
                case "data_array":
                    return "IList<object>";
                case "boolean":
                    return "bool?";
                case "number":
                    return "float?";
                case "integer":
                    return "int?";
                case "string":
                    return "string";
                case "color":
                    return "object";
                case "colorlist":
                    return "IList<object>";
                case "colorscale":
                    return "object";
                case "subplotid":
                    return "string";
                case "angle":
                    return "float?";
                case "any":
                    return "object";
                case "info_array":
                    return "IList<object>";

                default:
                    throw new ArgumentException($"ValType {attributeDescription.ValType} not supported");

            }
        }

        /// <summary>
        /// Gets the type of the array as a keyValuePair (TypeName, Description).
        /// </summary>
        /// <param name="attributeDescription">The attribute description.</param>
        /// <returns>System.Collections.Generic.KeyValuePair&lt;System.String, Plotly.Blazor.Generator.Schema.AttributeDescription&gt;.</returns>
        private static KeyValuePair<string, AttributeDescription> GetArrayType(AttributeDescription attributeDescription)
        {

            if (!attributeDescription.IsArray) throw new ArgumentException("Expected attributeDescription with type array");
            JsonElement attrAsJsonElement;

            switch (attributeDescription.Items.ValueKind)
            {
                case JsonValueKind.Array:
                    {
                        // Try to deserialize as an enumerable of json elements
                        if (!attributeDescription.Items.TryToObject<IEnumerable<JsonElement>>(out var asJsonElementEnumerable)) throw new ArgumentException("Couldn't parse attribute description array.");

                        // Check if it's
                        attrAsJsonElement = asJsonElementEnumerable.FirstOrDefault();
                        break;
                    }
                case JsonValueKind.Object:
                    attrAsJsonElement = attributeDescription.Items;
                    break;
                default:
                    throw new ArgumentException("Expected attribute description as object or array.");
            }

            // Could be keyValuePair
            if (attrAsJsonElement.TryToObject<Dictionary<string, AttributeDescription>>(out var dic))
            {
                var keyValue = dic.FirstOrDefault();
                return new KeyValuePair<string, AttributeDescription>(keyValue.Key.ToDotNetFriendlyName(_dictionary), keyValue.Value);
            }

            // Or a simple attributeDescription
            if (attributeDescription.Items.TryToObject<AttributeDescription>(out var description))
            {
                var keyValuePair = description.OtherAttributes.FirstOrDefault();
                keyValuePair.Value.TryToObject<AttributeDescription>(out var _);

                return new KeyValuePair<string, AttributeDescription>(keyValuePair.Key.ToDotNetFriendlyName(_dictionary), description);
            }
            else
            {

            }

            // Otherwise throw
            throw new ArgumentException("Couldn't parse attribute description element.");
        }
    }

}