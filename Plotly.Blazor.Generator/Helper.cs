using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using WeCantSpell.Hunspell;

namespace Plotly.Blazor.Generator
{
    public static class Helper
    {
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
            if (Regex.IsMatch(input, "[_+\\-\\s]"))
            {
                return Regex.Split(input, "[_+\\-\\s]+")
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.ToCamelCase(dic, suppressUppercase))
                    .Aggregate((s, t) => $"{s}{t}");
            }

            // Default is the input
            var suggestion = input;

            // Suggest something new if a spell check failed and no uppercase chars are included
            if (!dic.Check(input) && !input.Any(char.IsUpper))
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
                        .Select((s,i) => {
                            if (i == 0)
                            {
                                return suppressUppercase ? s : $"{char.ToUpper(s[0])}{s.Substring(1)}";
                            }
                            return $"{char.ToUpper(s[0])}{s.Substring(1)}";
                        })
                        .Aggregate((s, t) => $"{s}{t}");
                }
            }
            return suppressUppercase? suggestion : $"{char.ToUpper(suggestion[0])}{suggestion.Substring(1)}";
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
                .ReplaceSpecialChars(),
                "(\\d+)", m => int.Parse(m.Value).ConvertNumberToString());
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
        ///     Converts the digit to string.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private static string ConvertDigitToString(int i)
        {
            return i switch
            {
                0 => "",
                1 => "One",
                2 => "Two",
                3 => "Three",
                4 => "Four",
                5 => "Five",
                6 => "Six",
                7 => "Seven",
                8 => "Eight",
                9 => "Nine",
                _ => throw new IndexOutOfRangeException($"{i} not a digit"),
            };
        }

        /// <summary>
        ///     Converts the teens to string.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private static string ConvertTeensToString(int n)
        {
            return n switch
            {
                10 => "Ten",
                11 => "Eleven",
                12 => "Twelve",
                13 => "Thirteen",
                14 => "Fourteen",
                15 => "Fiveteen",
                16 => "Sixteen",
                17 => "Seventeen",
                18 => "Eighteen",
                19 => "Nineteen",
                _ => throw new IndexOutOfRangeException($"{n} not a teen"),
            };
        }

        /// <summary>
        ///     Converts the high tens to string.
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private static string ConvertHighTensToString(int n)
        {
            var tensDigit = (int)Math.Floor(n / 10.0);
            string tensStr = tensDigit switch
            {
                2 => "Twenty",
                3 => "Thirty",
                4 => "Forty",
                5 => "Fifty",
                6 => "Sixty",
                7 => "Seventy",
                8 => "Eighty",
                9 => "Ninety",
                _ => throw new IndexOutOfRangeException($"{n} not in range 20-99"),
            };
            if (n % 10 == 0)
            {
                return tensStr;
            }

            var onesStr = ConvertDigitToString(n - tensDigit * 10);
            return tensStr + onesStr;
        }

        /// <summary>
        ///     This is the primary conversion method which can convert any integer bigger than 99
        /// </summary>
        /// <param name="n">The numeric value of the integer to be translated ("textified")</param>
        /// <param name="baseNum">Represents the order of magnitude of the number (e.g., 100 or 1000 or 1e6, etc)</param>
        /// <param name="baseNumStr">The string representation of the base number (e.g. "hundred", "thousand", or "million", etc)</param>
        /// <returns>Textual representation of any integer</returns>
        private static string ConvertBigNumberToString(int n, int baseNum, string baseNumStr)
        {
            // Strategy: translate the first portion of the number, then recursively translate the remaining sections.
            // Step 1: strip off first portion, and convert it to string:
            var bigPart = (int)Math.Floor((double)n / baseNum);
            var bigPartStr = ConvertNumberToString(bigPart) + baseNumStr;
            // Step 2: check to see whether we're done:
            if (n % baseNum == 0)
            {
                return bigPartStr;
            }

            // Step 3: concatenate 1st part of string with recursively generated remainder:
            var restOfNumber = n - bigPart * baseNum;
            return bigPartStr + ConvertNumberToString(restOfNumber);
        }

        /// <summary>
        ///     Converts the number to string. (source:
        ///     https://www.codeproject.com/Articles/1164635/Converting-Numbers-to-Text-in-Csharp)
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="NotSupportedException">negative numbers not supported</exception>
        /// <exception cref="NotSupportedException">Bigger numbers are not supported</exception>
        public static string ConvertNumberToString(this int n)
        {
            if (n < 0)
            {
                throw new NotSupportedException("negative numbers not supported");
            }

            if (n == 0)
            {
                return "Zero";
            }

            if (n < 10)
            {
                return ConvertDigitToString(n);
            }

            if (n < 20)
            {
                return ConvertTeensToString(n);
            }

            if (n < 100)
            {
                return ConvertHighTensToString(n);
            }

            if (n < 1000)
            {
                return ConvertBigNumberToString(n, (int)1e2, "Hundred");
            }

            if (n < 1e6)
            {
                return ConvertBigNumberToString(n, (int)1e3, "Thousand");
            }

            if (n < 1e9)
            {
                return ConvertBigNumberToString(n, (int)1e6, "Million");
            }

            if (n < 1e12)
            {
                return ConvertBigNumberToString(n, (int)1e9, "Billion");
            }

            throw new NotSupportedException("Bigger numbers are not supported");
        }

        /// <summary>
        ///     Splits the given string by character count. Splits only full words.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="count">The count.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        public static IEnumerable<string> SplitByCharCountIfWhitespace(this string input, int count = 60)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                yield return "TODO";
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
                return nameSpace;
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
    }
}