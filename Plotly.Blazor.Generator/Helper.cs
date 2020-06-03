using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WeCantSpell.Hunspell;

namespace Plotly.Blazor.Generator
{
    public static class Helper
    {
        /// <summary>
        /// Gets or sets the unknown words, found while using the dictionary.
        /// </summary>
        /// <value>The unknown words.</value>
        public static List<string> UnknownWords { get; set; } = new List<string>();

        /// <summary>
        /// Dictionary to customize the pascal casing for specific words.
        /// </summary>
        /// <value>The custom words.</value>
        private static Dictionary<string, string> CustomWords { get; set; } = File.ReadAllLines("CustomDic.txt")
            .Select(l =>
            {
                var keyValue = l.Split('=');
                return (keyValue[0], keyValue[1]);
            })
            .ToDictionary(k => k.Item1, v => v.Item2);


        /// <summary>
        ///     Converts the input to camelcase. Default: PascalCase
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="dic">The dic.</param>
        /// <param name="suppressUppercase">if set to <c>true</c> [don't upper the first letter].</param>
        /// <returns>System.String.</returns>
        public static string ToCamelCase(this string input, WordList dic, bool suppressUppercase = false)
        {
            // Check for empty string.
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // Return instantly when no letter or digit was found
            if (!input.Any(char.IsLetterOrDigit))
            {
                return input;
            }

            // Check if a divider char exists, run recursively if so
            if (!CustomWords.ContainsKey(input) && Regex.IsMatch(input, "[_+\\-\\s]"))
            {
                return Regex.Split(input, "[_+\\-\\s]+")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.ToCamelCase(dic, suppressUppercase))
                    .Aggregate((s, t) => $"{s}{t}");
            }

            // Default is the input
            var suggestion = input;

            // Check for custom dictionary!
            if (CustomWords.ContainsKey(input))
            {
                suggestion = CustomWords[input];
            }

            // Suggest something new if a spell check failed and no uppercase chars are included
            else if (!dic.Check(input) && !input.Any(char.IsUpper))
            {
                // Get a an array of string[], where one string[] contains extracted words from the input
                var suggestions = dic
                    .Suggest(input) // Otherwise get splitting suggestions without changing the words. If nothing gets suggested --> 
                    .Where(s => string.Equals(
                        input,
                        Regex.Replace(s, "[_+\\-\\s]", string.Empty),
                        StringComparison.OrdinalIgnoreCase))
                    .Select(s => Regex.Split(s, "[_+\\-\\s]+"))
                    .FirstOrDefault();

                // If there is a suggestion, override the default
                if (suggestions != null && suggestions.Length > 0)
                {
                    //var suggestions = suggestionList[maxIndex];
                    suggestion = suggestions.Where(s => !string.IsNullOrWhiteSpace(s))
                        .Select((s, i) =>
                        {
                            if (i == 0)
                            {
                                return suppressUppercase ? s : $"{char.ToUpper(s[0])}{s.Substring(1)}";
                            }
                            return $"{char.ToUpper(s[0])}{s.Substring(1)}";
                        })
                        .Aggregate((s, t) => $"{s}{t}");
                }
                else
                {
                    UnknownWords.Add(input);
                }
            }
            return suppressUppercase ? suggestion : $"{char.ToUpper(suggestion[0])}{suggestion.Substring(1)}";
        }

        /// <summary>
        ///     Converts a string to a .NET friendly name., which can be used for class or property names.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="dic">The dic.</param>
        /// <param name="suppressUppercase">if set to <c>true</c> [suppress uppercase].</param>
        /// <returns>System.String.</returns>
        public static string ToDotNetFriendlyName(this string input, WordList dic, bool suppressUppercase = false)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "Empty";
            }

            return Regex.Replace(
                input
                .ToCamelCase(dic, suppressUppercase)
                .Replace(">=", "GreaterThanOrEqual")
                .Replace("<=", "LessThanOrEqual")
                .Replace("!=", "NotEqual")
                .ReplaceSpecialChars(),
                "^(\\d+)", m => $"_{m.Value}");
        }

        /// <summary>
        ///     Replaces the special chars.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        private static string ReplaceSpecialChars(this string input)
        {
            var sb = new StringBuilder();

            foreach (var c in input)
            {
                switch (c)
                {
                    case '.':
                        sb.Append("Dot");
                        break;
                    case ',':
                        sb.Append("Comma");
                        break;
                    case '+':
                        sb.Append("Plus");
                        break;
                    case '-':
                        sb.Append("Minus");
                        break;
                    case '=':
                        sb.Append("Equal");
                        break;
                    case '<':
                        sb.Append("LessThan");
                        break;
                    case '>':
                        sb.Append("GreaterThan");
                        break;
                    case '(':
                        sb.Append("OpeningRoundBracket");
                        break;
                    case ')':
                        sb.Append("ClosingRoundBracket");
                        break;
                    case '{':
                        sb.Append("OpeningCurlyBracket");
                        break;
                    case '}':
                        sb.Append("ClosingCurlyBracket");
                        break;
                    case '[':
                        sb.Append("OpeningSquareBracket");
                        break;
                    case ']':
                        sb.Append("ClosingSquareBracket");
                        break;
                    case '|':
                        sb.Append("VerticalBar");
                        break;
                    case '/':
                        sb.Append("Slash");
                        break;
                    case '\\':
                        sb.Append("Backslash");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }


        /// <summary>
        ///     Splits the given string by character count. Splits only full words.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="count">The count.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> SplitByCharCountIfWhitespace(this string input, int count = 70)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                yield return null;
            }

            while (input?.Length > 0)
            {
                var br = 0;
                foreach (var c in input)
                {
                    if (br > count && char.IsWhiteSpace(c))
                    {
                        break;
                    }

                    br++;
                }

                yield return new string(input.Take(br).ToArray());
                input = new string(input.Skip(br + 1).ToArray());
            }
        }

        /// <summary>
        ///     Replaces special highlighted words like `text` and *text*.
        ///     Values with spaces are not considered. However, * and ` are replaced with '.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceHighlighting(this string input)
        {
            return input
                .Replace("\\*([^\\*\\s]+)\\*", m => $"<c>{m.Groups[1].Value}</c>")
                .Replace("&#39;(?!&#39;)([^\\s]+)&#39;", m => $"<c>{m.Groups[1].Value}</c>")
                .Replace("`([^`\\s]+)`", m => $"<c>{m.Groups[1].Value}</c>")
                .Replace("&quot;(?!&quot;)([^\\s]+)&quot;", m => $"<c>{m.Groups[1].Value}</c>")
                .Replace("\\*([^\\*]+)\\*", m => $"&#39;{m.Groups[1].Value}&#39;")
                .Replace("\\`([^\\`]+)\\`", m => $"&#39;{m.Groups[1].Value}&#39;");
        }

        private static string Replace(this string input, string pattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(input, pattern, evaluator);
        }

        /// <summary>
        ///     Gets the output path by name space.
        /// </summary>
        /// <param name="nameSpace">The name space.</param>
        /// <param name="refNameSpace">The reference name space.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentException">Input namespace must contain the referenced namespace!</exception>
        public static string GetOutputPathByNameSpace(this string nameSpace, string refNameSpace = "Plotly.Blazor")
        {
            if (nameSpace == refNameSpace)
            {
                return @".\src\";
            }

            if (!nameSpace.Contains($"{refNameSpace}."))
            {
                throw new ArgumentException("Input namespace must contain the referenced namespace!");
            }

            return nameSpace.Replace($"{refNameSpace}.", string.Empty).Split('.')
                .Aggregate(@".\src\", (s1, s2) => $"{s1}\\{s2}");
        }

        /// <summary>
        ///     Converts a JsonElement to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element.</param>
        /// <returns>T.</returns>
        public static T ToObject<T>(this JsonElement element)
        {
            var json = element.GetRawText();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        ///     Tries converts a JsonElement to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">The element.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool TryToObject<T>(this JsonElement element, out T result)
        {
            try
            {
                result = element.ToObject<T>();
                return true;
            }
            catch (JsonException)
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Escapes the XML special chars.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>System.String.</returns>
        public static string HtmlEncode(this string s)

        {
            return WebUtility.HtmlEncode(s);
        }
    }
}