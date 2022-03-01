using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

#pragma warning disable 1591

namespace Plotly.Blazor
{
    public static class Extensions
    {
        /// <summary>
        ///     Adds data to an IList.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (list is List<T> asList)
            {
                asList.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Add(item);
                }
            }
        }

        /// <summary>
        ///     Inserts data to an IList by given index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public static void InsertRange<T>(this IList<T> list, int index, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (list is List<T> asList)
            {
                asList.InsertRange(index, items);
            }
            else
            {
                foreach (var item in items)
                {
                    list.Insert(0, item);
                }
            }
        }

        /// <summary>
        ///     Prepares an object for js interop operations, converting the object to a dictionary.
        ///     This operation can be customized using own serializer options.
        ///     Currently it's not possible to define serializer options for the JSRuntime directly.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="serializerOptions">Optional serializerOptions.</param>
        /// <returns></returns>
        public static object PrepareJsInterop<T>(this T obj, JsonSerializerOptions serializerOptions = null)
        {
            var type = obj?.GetType();

            // Handle simple types
            if (obj == null || type.IsPrimitive || type == typeof(string))
            {
                return obj;
            }

            // Handle jsonElements
            if (obj is JsonElement jsonElement)
            {
                return jsonElement.PrepareJsonElement();
            }

            // Set default serializer options if necessary
            serializerOptions ??= new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = null
            };

            // Handle all kind of complex objects
            return JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize<object>(obj, serializerOptions))
                .PrepareJsonElement();
        }

        /// <summary>
        ///     Updates the properties of a given object, using a json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="jsonString"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static T Populate<T>(this T obj, string jsonString, JsonSerializerOptions options) where T : class
        {
            options ??= new JsonSerializerOptions();

            var newObj = JsonSerializer.Deserialize(jsonString, obj.GetType(), options);

            foreach (var property in newObj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Where(p => p.CanWrite))
            {
                if (!obj.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                    .Where(p => p.CanWrite).Any(x => x.Name == property.Name && property.GetValue(newObj) != default))
                {
                    continue;
                }

                if (property.GetType().IsClass &&
                    property.PropertyType.Assembly.FullName == typeof(T).Assembly.FullName)
                {
                    var mapMethod = typeof(Extensions).GetMethod("Populate");
                    if (mapMethod == null)
                    {
                        throw new NullReferenceException(nameof(mapMethod));
                    }

                    var genericMethod = mapMethod.MakeGenericMethod(property.GetValue(newObj).GetType());
                    var obj2 = genericMethod.Invoke(null,
                        new[]
                        {
                            property.GetValue(newObj), JsonSerializer.Serialize(property.GetValue(newObj).ToString())
                        });

                    foreach (var property2 in obj2.GetType().GetProperties())
                    {
                        if (property2.GetValue(obj2) != null)
                        {
                            property?.GetValue(obj)?.GetType()?.GetProperty(property2.Name)
                                ?.SetValue(property.GetValue(obj), property2.GetValue(obj2));
                        }
                    }
                }
                else
                {
                    property.SetValue(obj, property.GetValue(newObj));
                }
            }

            return obj;
        }

        /// <summary>
        ///     Updates the properties of a given object, using a json string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        internal static T Populate<T>(this T obj, string jsonString) where T : class
        {
            return obj.Populate(jsonString, null);
        }


        private static object PrepareJsonElement(this JsonElement obj)
        {
            switch (obj.ValueKind)
            {
                case JsonValueKind.Object:
                    IDictionary<string, object> expando = new ExpandoObject();
                    foreach (var property in obj.EnumerateObject())
                    {
                        expando.Add(property.Name, property.Value.PrepareJsonElement());
                    }

                    return expando;
                case JsonValueKind.Array:
                    return obj.EnumerateArray().Select(jsonElement => jsonElement.PrepareJsonElement());
                case JsonValueKind.String:
                    return obj.GetString();
                case JsonValueKind.Number:
                    return obj.GetDecimal();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.Undefined:
                    return null;
                default:
                    throw new ArgumentException();
            }
        }

        #region DeepClone (Source: https://github.com/Burtsev-Alexey/net-object-deep-copy)

        private static readonly MethodInfo CloneMethod =
            typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        ///     Determines if the given type is primitive.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>bool</returns>
        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }

            return type.IsValueType & type.IsPrimitive;
        }

        /// <summary>
        ///     Creates a copy of the given object.
        /// </summary>
        /// <param name="originalObject">object</param>
        /// <returns>object</returns>
        public static object Copy(this object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null)
            {
                return null;
            }

            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect))
            {
                return originalObject;
            }

            if (visited.ContainsKey(originalObject))
            {
                return visited[originalObject];
            }

            if (typeof(Delegate).IsAssignableFrom(typeToReflect))
            {
                return null;
            }

            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    var clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) =>
                        array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }

            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject,
            IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType == null)
            {
                return;
            }

            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
            CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType,
                BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject,
            IReflect typeToReflect,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                                        BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false)
                {
                    continue;
                }

                if (IsPrimitive(fieldInfo.FieldType))
                {
                    continue;
                }

                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        /// <summary>
        ///     Creates a deep copy of the given object of type T.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="original">T</param>
        /// <returns>T</returns>
        public static T Copy<T>(this T original)
        {
            return (T)Copy((object)original);
        }

        private class ReferenceEqualityComparer : EqualityComparer<object>
        {
            public override bool Equals(object x, object y)
            {
                return ReferenceEquals(x, y);
            }

            public override int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }

        private static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0)
            {
                return;
            }

            var walker = new ArrayTraverse(array);
            do
            {
                action(array, walker.Position);
            } while (walker.Step());
        }

        private class ArrayTraverse
        {
            private readonly int[] maxLengths;
            public readonly int[] Position;

            public ArrayTraverse(Array array)
            {
                maxLengths = new int[array.Rank];
                for (var i = 0; i < array.Rank; ++i)
                {
                    maxLengths[i] = array.GetLength(i) - 1;
                }

                Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (var i = 0; i < Position.Length; ++i)
                {
                    if (Position[i] >= maxLengths[i])
                    {
                        continue;
                    }

                    Position[i]++;
                    for (var j = 0; j < i; j++)
                    {
                        Position[j] = 0;
                    }

                    return true;
                }

                return false;
            }
        }

        #endregion
    }
}