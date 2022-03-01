using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Plotly.Blazor.Generator.Schema;
using Plotly.Blazor.Generator.Templates;
using Plotly.Blazor.Generator.Templates.Class;
using Plotly.Blazor.Generator.Templates.Enumerated;
using Plotly.Blazor.Generator.Templates.Flag;
using Plotly.Blazor.Generator.Templates.Interface;
using Stubble.Core;
using Stubble.Core.Builders;
using WeCantSpell.Hunspell;
using FlagValue = Plotly.Blazor.Generator.Templates.Flag.FlagValue;

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
        private static readonly IDictionary<string, Job> Jobs = new ConcurrentDictionary<string, Job>();

        #region Main

        private static async Task Main()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await using var dictionaryStream = File.OpenRead(@"English (American).dic");
            await using var affixStream = File.OpenRead(@"English (American).aff");
            _dictionary = await WordList.CreateFromStreamsAsync(dictionaryStream, affixStream);

            _schema = await GetPlotlySchemaAsync();
            _stubble = new StubbleBuilder().Configure(settings =>
                {
                    settings.SetIgnoreCaseOnKeyLookup(true);
                    settings.SetEncodingFunction(s => s);
                })
                .Build();

            Parallel.Invoke(CreateAnimation, CreateTransforms, CreateFrames, CreateLayout, CreateConfig, CreateTraces);

            foreach (var (key, value) in Jobs)
            {
                Console.WriteLine($"Generating {key}.cs");
                await value.Execute(_stubble);
            }

            await File.WriteAllLinesAsync("UnknownWords.txt", Helper.UnknownWords.Distinct());
            stopwatch.Stop();
            Console.WriteLine($"[PERFORMANCE] Generation took {stopwatch.ElapsedMilliseconds/1000.0}s");
        }

        #endregion

        #region Schema

        private static async Task<SchemaRoot> GetPlotlySchemaAsync()
        {
            using var httpClient = new HttpClient();

            var schemaJson = await httpClient.GetStringAsync(
                "https://raw.githubusercontent.com/plotly/plotly.js/master/dist/plot-schema.json");

            // Write latest .js-File
            var outputDir = @".\src\wwwroot";
            Directory.CreateDirectory(outputDir);
            await File.WriteAllTextAsync($"{outputDir}\\plotly-latest.min.js",
                await httpClient.GetStringAsync("https://cdn.plot.ly/plotly-2.9.0.min.js"));

            var serializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<SchemaRoot>(schemaJson, serializerOptions);
        }

        #endregion

        #region Animation

        private static void CreateAnimation()
        {
            foreach (var (key, value) in _schema.Animation)
            {
                AddJob(key, value, $"{Namespace}.AnimationLib");
            }

            AddClassJob("Animation", _schema.Animation, Namespace);
        }

        #endregion

        #region Config

        private static void CreateConfig()
        {
            foreach (var (key, value) in _schema.Config)
            {
                AddJob(key, value, $"{Namespace}.ConfigLib");
            }

            AddClassJob("Config", _schema.Config, Namespace);
        }

        #endregion

        #region Frames

        private static void CreateFrames()
        {
            AddClassJob("Frames", _schema.Frames.Items.Frames_Entry.OtherAttributes, Namespace);
        }


        #endregion

        #region Layout

        private static void CreateLayout()
        {
            var traceLayoutAttributes = _schema.Traces
                .Select(keyValue => keyValue.Value.LayoutAttributes)
                .Where(layoutAttributes => layoutAttributes != null)
                .SelectMany(dictionary => dictionary)
                .ToLookup(pair => pair.Key, pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.First());

            foreach (var (key, value) in _schema.Layout.LayoutAttributes)
            {
                if (!value.TryToObject<AttributeDescription>(out var attributeDescription))
                {
                    continue;
                }

                traceLayoutAttributes.Add(key, attributeDescription);
            }

            foreach (var (key, value) in traceLayoutAttributes)
            {
                if (value != null)
                {
                    AddJob(key, value, $"{Namespace}.LayoutLib");
                }
            }

            AddClassJob("Layout", traceLayoutAttributes, Namespace);
        }

        #endregion

        #region Transforms

        private static void CreateTransforms()
        {
            // Generate all nested classes
            foreach (var (transformKey, transformValue) in _schema.Transforms)
            {
                foreach (var (key, value) in transformValue.Attributes)
                {
                    if (!value.TryToObject<AttributeDescription>(out var attributeDescription))
                    {
                        continue;
                    }

                    AddJob(key, attributeDescription,
                        $"{Namespace}.Transforms.{transformKey.ToDotNetFriendlyName(_dictionary)}Lib");
                }
            }

            CreateTransformInterfaceJob();
            CreateTransformTypeJob();
            CreateTransformsJobs();
        }

        private static void CreateTransformInterfaceJob()
        {
            var interfaceData = new InterfaceData
            {
                Name = "ITransform",
                Namespace = Namespace,
                Description = new[] {"The transform interface."},
                Properties = new List<Property>
                {
                    new Property
                    {
                        PropertyDescription = new[] {"The type of the transform."},
                        TypeName = "TransformTypeEnum?",
                        PropertyName = "Type",
                        DisplayName = "type",
                        IsReadOnly = true,
                        IsInherited = true
                    }
                }
            };
            Jobs.Add($"{interfaceData.Namespace}.{interfaceData.Name}", new Job(interfaceData));
        }

        private static void CreateTransformTypeJob()
        {
            var typeEnumData = new EnumeratedData
            {
                Name = "TransformTypeEnum",
                Namespace = Namespace,
                Description = new[] {"Determines the type of the transform."},
                Values = _schema.Transforms.Select(keyValue => new EnumeratedValue
                {
                    DisplayName = keyValue.Key,
                    EnumName = keyValue.Key.ToDotNetFriendlyName(_dictionary)
                })
            };
            Jobs.Add($"{typeEnumData.Namespace}.{typeEnumData.Name}", new Job(typeEnumData));
        }

        private static void CreateTransformsJobs()
        {
            foreach (var (transformKey, transformValue) in _schema.Transforms)
            {
                var friendlyName = transformKey.ToDotNetFriendlyName(_dictionary);
                var typeProperty = new Property
                {
                    DisplayName = "type",
                    PropertyName = "Type",
                    TypeName = "TransformTypeEnum?",
                    IsReadOnly = true,
                    DefaultValue = $"TransformTypeEnum.{friendlyName}",
                    IsInherited = true
                };
                AddClassJob(transformKey, transformValue.Attributes, $"{Namespace}.Transforms", "ITransform",
                    new[] {typeProperty});
            }
        }

        #endregion

        #region Traces

        private static void CreateTraces()
        {
            CreateTraceInterfaceJob();
            CreateTraceTypeEnumJob();
            CreateTraceJobs();
        }

        private static void CreateTraceInterfaceJob()
        {
            var interfaceData = new InterfaceData
            {
                Name = "ITrace",
                Namespace = $"{Namespace}",
                Description = new[] {"The trace interface."},
                Properties = new List<Property>
                {
                    new Property
                    {
                        PropertyDescription = new[] {"The type of the trace."},
                        TypeName = "TraceTypeEnum?",
                        PropertyName = "Type",
                        DisplayName = "type",
                        IsReadOnly = true,
                        IsInherited = true
                    }
                }
            };

            Jobs.Add($"{interfaceData.Namespace}.{interfaceData.Name}", new Job(interfaceData));
        }

        private static void CreateTraceTypeEnumJob()
        {
            var typeEnumData = new EnumeratedData
            {
                Name = "TraceTypeEnum",
                Namespace = $"{Namespace}",
                Description = new[] {"Determines the type of the trace."},
                Values = _schema.Traces.Select(keyValue => new EnumeratedValue
                {
                    DisplayName = keyValue.Value.Attributes.ContainsKey("type")
                        ? keyValue.Value.Attributes["type"].GetString()
                        : keyValue.Key,
                    EnumName = keyValue.Key.ToDotNetFriendlyName(_dictionary)
                })
            };
            Jobs.Add($"{typeEnumData.Namespace}.{typeEnumData.Name}", new Job(typeEnumData));
        }

        private static void CreateTraceJobs()
        {
            Parallel.ForEach(_schema.Traces, pair =>
            {
                var (traceKey, traceValue) = pair;
                foreach (var (attributeKey, attributeValue) in traceValue.Attributes)
                {
                    if (!attributeValue.TryToObject<AttributeDescription>(out var attributeDescription))
                    {
                        continue;
                    }

                    AddJob(attributeKey, attributeDescription,
                        $"{Namespace}.Traces.{traceKey.ToDotNetFriendlyName(_dictionary)}Lib");
                }

                var friendlyName = traceKey.ToDotNetFriendlyName(_dictionary);
                var typeProperty = new Property
                {
                    DisplayName = "type",
                    PropertyName = "Type",
                    TypeName = "TraceTypeEnum?",
                    IsReadOnly = true,
                    DefaultValue = $"TraceTypeEnum.{friendlyName}",
                    IsInherited = true
                };

                AddClassJob(traceKey, traceValue.Attributes, $"{Namespace}.Traces",
                    "ITrace", new[] {typeProperty});
            });
        }

        #endregion

        #region Jobs

        private static void AddJob(string name, AttributeDescription attributeDescription,
            string customNamespace = Namespace)
        {
            // Call it recursively for all nested attributes if its an array
            if (attributeDescription.IsArray)
            {
                var (key, value) = GetArrayType(attributeDescription);
                AddJob(key, value, $"{customNamespace}");
            }

            // Call it recursively for all nested attributes if its an object
            else if (attributeDescription.Role == "object")
            {
                if (attributeDescription.OtherAttributes != null)
                {
                    foreach (var (key, value) in attributeDescription.OtherAttributes)
                    {
                        if (!value.TryToObject<AttributeDescription>(out var otherAttribute))
                        {
                            continue;
                        }

                        if (otherAttribute.ValType == "enumerated")
                        {
                            if (otherAttribute.OtherAttributes["values"].EnumerateArray()
                                .Any(elem =>
                                    elem.ValueKind == JsonValueKind.String && elem.GetString().StartsWith("/^")))
                            {
                                continue;
                            }
                        }

                        AddJob(key, otherAttribute, $"{customNamespace}.{name.ToDotNetFriendlyName(_dictionary)}Lib");
                    }
                }
            }

            // Generate files for enums, flags, objects
            if (attributeDescription.ValType == "enumerated")
            {
                AddEnumJob(name, attributeDescription, customNamespace);
                return;
            }

            if (attributeDescription.ValType == "flaglist")
            {
                AddFlagJob(name, attributeDescription, customNamespace);
                return;
            }

            if (attributeDescription.Role == "object" && !attributeDescription.IsArray)
            {
                AddClassJob(name, attributeDescription.OtherAttributes, customNamespace);
            }
        }

        private static void AddEnumJob(string name, AttributeDescription attributeDescription, string customNamespace)
        {
            var friendlyName = name.ToDotNetFriendlyName(_dictionary);
            var enumeratedData = new EnumeratedData
            {
                Name = $"{friendlyName}Enum",
                Namespace = customNamespace,
                Description = attributeDescription.Description
                    .HtmlEncode()?.ReplaceHighlighting()?.SplitByCharCountIfWhitespace()
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
            var valuesAsString = attributeDescription.OtherAttributes["values"].ToObject<object[]>()
                .Select(s => s.ToString()).Distinct().ToArray();

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

            Jobs.Add($"{enumeratedData.Namespace}.{enumeratedData.Name}", new Job(enumeratedData));
        }

        private static void AddFlagJob(string name, AttributeDescription attributeDescription,
            string customNamespace)
        {
            var friendlyName = name.ToDotNetFriendlyName(_dictionary);

            var flagData = new FlagData
            {
                Name = $"{name.ToDotNetFriendlyName(_dictionary)}Flag",
                Namespace = customNamespace,
                Description = attributeDescription.Description.HtmlEncode()?.ReplaceHighlighting()
                    ?.SplitByCharCountIfWhitespace()
            };

            // Get values as string representation (casting does not work!)
            var flagsAsString = attributeDescription.Flags.Select(s => s.ToString()).Distinct().ToArray();

            flagData.Flags = flagsAsString.Select(s =>
            {
                // Check for duplicates (it's possible that e and E exists as a valid different enum!)
                var hasDuplicates = flagsAsString.Where(v => v != s)
                    .Any(v => string.Equals(v, s, StringComparison.CurrentCultureIgnoreCase));

                return new FlagValue
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

                    return new FlagValue
                    {
                        DisplayName = s,
                        EnumName = s.ToDotNetFriendlyName(_dictionary, hasDuplicates)
                    };
                });
            }

            Jobs.Add($"{flagData.Namespace}.{flagData.Name}", new Job(flagData));
        }


        private static void AddClassJob(string name, IDictionary<string, JsonElement> attributes,
            string customNamespace, string interfaceName = null, IEnumerable<Property> additionalProperties = null)
        {
            AddClassJob(name, attributes
                    ?.Select(pair =>
                        !pair.Value.TryToObject<AttributeDescription>(out var description)
                            ? default
                            : new KeyValuePair<string, AttributeDescription>(pair.Key, description))
                    .Where(pair => !pair.Equals(default(KeyValuePair<string, AttributeDescription>)))
                    .ToDictionary(pair => pair.Key, pair => pair.Value),
                customNamespace,
                interfaceName,
                additionalProperties
            );
        }

        private static void AddClassJob(string name, IDictionary<string, AttributeDescription> attributes,
            string customNamespace, string interfaceName = null, IEnumerable<Property> additionalProperties = null)
        {
            var friendlyName = name.ToDotNetFriendlyName(_dictionary);

            var properties = attributes?
                .Select(pair => CreateProperty(pair, friendlyName, customNamespace))
                .Where(p => p != null)
                .SelectMany(p => p)
                .Where(p => p != null)
                .ToList();

            if (additionalProperties != null)
            {
                properties?.InsertRange(0, additionalProperties);
            }

            var classData = new ClassData
            {
                Name = friendlyName,
                Interface = interfaceName,
                Namespace = customNamespace,
                Description = new[] {$"The {friendlyName} class."},
                Properties = properties
            };

            if (classData.Properties != null && classData.Properties.Any())
            {
                Jobs.Add($"{classData.Namespace}.{classData.Name}", new Job(classData));
            }
        }

        private static IEnumerable<Property> CreateProperty(KeyValuePair<string, AttributeDescription> pair, string className, string @namespace)
        {
            if (pair.Key == "_deprecated")
            {
                return null;
            }

            var propertyList = new List<Property>();

            // GET NAME
            var propertyFriendlyName = pair.Key.ToDotNetFriendlyName(_dictionary);

            // HANDLE THE CASE WHEN THE PROPERTY NAME EQUALS THE CLASS NAME
            if (propertyFriendlyName == className)
            {
                propertyFriendlyName = $"_{propertyFriendlyName}";
            }

            var typeName =
                GetTypeByAttributeDescription(pair.Value, pair.Key, $"{@namespace}.{className}Lib");

            var property = new Property
            {
                DisplayName = pair.Key,
                PropertyName = propertyFriendlyName,
                TypeName = typeName,
                PropertyDescription = string.IsNullOrWhiteSpace(pair.Value.Description)
                    ? new[] {$"Gets or sets the {propertyFriendlyName}."}
                    : pair.Value.Description.HtmlEncode()?.ReplaceHighlighting()
                        ?.SplitByCharCountIfWhitespace(),
                IsSubplot = pair.Value.IsSubplotObj
            };
            propertyList.Add(property);

            if (!pair.Value.ArrayOk)
            {
                return propertyList;
            }

            var arrayProperty = (Property)property.Clone();
            arrayProperty.TypeName = $"IList<{arrayProperty.TypeName}>";
            arrayProperty.PropertyName = $"{arrayProperty.PropertyName}Array";
            arrayProperty.IsArrayOk = true;
            propertyList.Add(arrayProperty);

            return propertyList;
        }

        #endregion

        #region Helper

         private static string GetTypeByAttributeDescription(AttributeDescription attributeDescription, string key,
            string @namespace)
        {
            // EDGE CASES
            switch (key)
            {
                case "transform":
                    return "ITransform";
                case "transforms":
                    return "IList<ITransform>";
                case "data":
                    return "ITrace";
                case "layout":
                    return "Layout";
            }

            // HANDLE REGEX
            if (attributeDescription.ValType == "enumerated")
            {
                if (attributeDescription.OtherAttributes["values"].EnumerateArray()
                    .Any(elem => elem.ValueKind == JsonValueKind.String && elem.GetString().StartsWith("/^")))
                {
                    return "string";
                }
            }

            // HANDLE ARRAY
            if (attributeDescription.IsArray)
            {
                var (typeKey, typeDescription) = GetArrayType(attributeDescription);
                var type = GetTypeByAttributeDescription(typeDescription, typeKey, @namespace);
                return $"IList<{type}>";
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                if (attributeDescription.IsSubplotObj)
                {
                    var copy = (AttributeDescription) attributeDescription.Clone();
                    copy.IsSubplotObj = false;
                    return $"IList<{GetTypeByAttributeDescription(copy, key, @namespace)}>";
                }

                if (attributeDescription.Role == "object")
                {
                    return $"{@namespace}.{key.ToDotNetFriendlyName(_dictionary)}";
                }

                switch (attributeDescription.ValType)
                {
                    case "enumerated":
                        return $"{@namespace}.{key.ToDotNetFriendlyName(_dictionary)}Enum?";
                    case "flaglist":
                        return $"{@namespace}.{key.ToDotNetFriendlyName(_dictionary)}Flag?";
                }
            }

            return attributeDescription.ValType switch
            {
                "data_array" => "IList<object>",
                "boolean" => "bool?",
                "number" => "decimal?",
                "integer" => "int?",
                "string" => "string",
                "color" => "object",
                "colorlist" => "IList<object>",
                "colorscale" => "object",
                "subplotid" => "string",
                "angle" => "decimal?",
                "any" => "object",
                "info_array" => "IList<object>",
                _ => throw new ArgumentException($"ValType {attributeDescription.ValType} not supported")
            };
        }

        private static KeyValuePair<string, AttributeDescription> GetArrayType(
            AttributeDescription attributeDescription)
        {
            if (!attributeDescription.IsArray)
            {
                throw new ArgumentException("Expected attributeDescription with type array");
            }

            JsonElement attrAsJsonElement;

            switch (attributeDescription.Items.ValueKind)
            {
                case JsonValueKind.Array:
                {
                    // Try to deserialize as an enumerable of json elements
                    if (!attributeDescription.Items.TryToObject<IEnumerable<JsonElement>>(
                        out var asJsonElementEnumerable))
                    {
                        throw new ArgumentException("Couldn't parse attribute description array.");
                    }

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
                var (key, value) = dic.FirstOrDefault();
                return new KeyValuePair<string, AttributeDescription>(key.ToDotNetFriendlyName(_dictionary), value);
            }

            // Or a simple attributeDescription
            if (attributeDescription.Items.TryToObject<AttributeDescription>(out var description))
            {
                var (key, value) = description.OtherAttributes.FirstOrDefault();
                value.TryToObject<AttributeDescription>(out _);

                return new KeyValuePair<string, AttributeDescription>(key.ToDotNetFriendlyName(_dictionary),
                    description);
            }

            // Otherwise throw
            throw new ArgumentException("Couldn't parse attribute description element.");
        }

        #endregion

    }
}