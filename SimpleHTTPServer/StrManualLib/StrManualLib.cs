using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    public class StrManualLib
    {
        private static Random random = new Random();

        /// <summary>
        /// 'str': "Input our string" -> "our string",
        /// return: "Input"
        /// </summary>
        public static string GetNextWordWithDelete(ref string str)
        {
            string word = "";
            string newStr = "";
            bool wordFound = false;

            foreach (var ch in str)
            {
                if (!wordFound)
                {
                    if (ch == ' ' || ch == '\r' || ch == '\n')
                    {
                        if (word.Length > 0)
                        {
                            wordFound = true;
                        }
                        continue;
                    }
                    word += ch;
                }
                else
                {
                    newStr += ch;
                }
            }

            str = newStr;
            return word;
        }

        /// <summary>
        /// "\"Input\"," -> "Input"
        /// </summary>
        public static string RemoveSpecialSymbol(string str)
        {
            str = str.Remove(0, 1);
            if (str[str.Length - 1] != '"')
            {
                str = str.Remove(str.Length - 1, 1);
            }

            return str.Remove(str.Length - 1, 1);
        }

        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// "METHOD_NOT_ALLOWED" -> "Method not allowed"
        /// </summary>
        public static string ConstFormatToStringFormat(string str)
        {
            str = str.Replace('_', ' ');
            char firstLetter = str[0];
            str = str.Substring(1);
            str = str.ToLower();
            str = firstLetter + str;

            return str;
        }

        /// <summary>
        /// Проверяют строку без первой скобки
        /// </summary>
        public static bool CheckStringForJsonFormat(string jsonString)
        {
            int index;
            jsonString = jsonString.Replace(" ", string.Empty);
            jsonString = jsonString.Replace("\n", string.Empty);

            string[] words = jsonString.Split(new char[] { ':', ',' });
            for (index = 0; index < words.Length; index++)
            {
                bool result = true;
                if (index % 2 == 0)
                {
                    result = words[index].StartsWith("\"") &&
                        words[index].EndsWith("\"") &&
                        words[index].Length > 2;
                }
                else
                {
                    result = !words[index].StartsWith("}") &&
                        words[index].Length > 0;
                }

                if (!result)
                {
                    return false;
                }
            }

            for (index = 0; index < jsonString.Length;)
            {
                if (!((jsonString[index] == '"') || (jsonString[index] == ':') || (jsonString[index] == ',') || (jsonString[index] == '}')))
                {
                    jsonString = jsonString.Remove(index, 1);
                }
                else
                {
                    index++;
                }
            }

            string jsonPatternStringValue = "\"\":\"\"";
            jsonString = jsonString.Replace(jsonPatternStringValue + ",", string.Empty);
            jsonString = jsonString.Replace(jsonPatternStringValue, "{");
            string jsonPattern = "\"\":";
            jsonString = jsonString.Replace(jsonPattern + ",", string.Empty);
            jsonString = jsonString.Replace(jsonPattern, "{");

            return jsonString == "{}";
        }
    }
}
