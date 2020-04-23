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
        private const string REPLACE_OPEN_BRACKET = "<";
        private const string REPLACE_CLOSE_BRACKET = ">";

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
            if (str.StartsWith("\""))
            {
                str = str.Remove(0, 1);
            }
            if (str.EndsWith(",") || str.EndsWith(":"))
            {
                str = str.Remove(str.Length - 1, 1);
            }
            if (str.EndsWith("\""))
            {
                str = str.Remove(str.Length - 1, 1);
            }

            return str;
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
            if (str.Length == 0 || str == "OK")
            {
                return str;
            }

            str = str.Replace('_', ' ');
            char firstLetter = str[0];
            str = str.Substring(1);
            str = str.ToLower();
            str = firstLetter + str;

            return str;
        }

        /// <summary>
        /// Может проверяют строку без открывающей '{'
        /// </summary>
        public static bool CheckStringForJsonFormat(string jsonString)
        {
            int index;
            jsonString = jsonString.Replace(" ", string.Empty);
            jsonString = jsonString.Replace("\n", string.Empty);

            if (jsonString.StartsWith("{"))
            {
                jsonString = jsonString.Remove(0, 1);
            }

            if (jsonString == "}")
            {
                return true;
            }

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

        public static string DeleteFirstAndLastChar(string strData)
        {
            if (strData.Length < 2)
            {
                return "";
            }
            strData = strData.Remove(0, 1);
            return strData.Remove(strData.Length - 1, 1);
        }

        /// <summary>
        /// {"key":"value", "key1":[1,2,3]} -> {<0>:<1>, <2>:<3>}
        /// </summary>
        public static string ReplaceHighObjectWithMark(string str, List<string> subObjectList)
        {
            bool presentObjectValue = true;
            while (presentObjectValue)
            {
                // Выбор ближайших скобок
                char objectCloseBracket = '"';
                int indexOpenBracket = str.IndexOfAny(new char[] { '{', '[', '"' });
                if (indexOpenBracket == -1)
                {
                    break;
                }

                char objectOpenBracket = str[indexOpenBracket];
                switch (objectOpenBracket)
                {
                    case '{':
                        objectCloseBracket = '}';
                        break;
                    case '[':
                        objectCloseBracket = ']';
                        break;
                }

                presentObjectValue = false;
                int flag = 1;
                int index = indexOpenBracket;
                while (-1 != (index = str.IndexOfAny(new char[] { objectOpenBracket, objectCloseBracket }, index + 1)))
                {
                    presentObjectValue = true;
                    if (str[index] == objectCloseBracket)
                    {
                        flag--;
                    }
                    else
                    {
                        flag++;
                    }

                    if (flag == 0)
                    {
                        string highObject = str.Substring(indexOpenBracket, index - indexOpenBracket + 1);
                        str = str.Replace(highObject, REPLACE_OPEN_BRACKET + subObjectList.Count.ToString() + REPLACE_CLOSE_BRACKET);
                        subObjectList.Add(highObject);
                        break;
                    }
                }
            }

            return str;
        }

        public static string ReplaceMarkWithHighObject(string str, List<string> subObjectList)
        {
            for (int count = 0; count < subObjectList.Count; count++)
            {
                string mark = REPLACE_OPEN_BRACKET + count.ToString() + REPLACE_CLOSE_BRACKET;
                str = str.Replace(mark, subObjectList[count]);
            }

            return str;
        }

        public static string[] ReplaceMarkWithHighObject(string[] wordArr, List<string> subObjectList)
        {
            for (int wordCount = 0; wordCount < wordArr.Length; wordCount++)
            {
                for (int subObjectCount = 0; subObjectCount < subObjectList.Count; subObjectCount++)
                {
                    string mark = REPLACE_OPEN_BRACKET + subObjectCount.ToString() + REPLACE_CLOSE_BRACKET;
                    wordArr[wordCount] = wordArr[wordCount].Replace(mark, subObjectList[subObjectCount]);
                }
            }

            return wordArr;
        }

        /// <summary>
        /// Не сплитим json - {}, списки - []
        /// </summary>
        public static string[] SmartSplit(string str, char[] separatorArr)
        {
            var subObjectList = new List<string>();

            str = ReplaceHighObjectWithMark(str, subObjectList);

            // +-
            str = str.Replace(" ", string.Empty);

            string[] wordArr = str.Split(separatorArr, StringSplitOptions.RemoveEmptyEntries);

            wordArr = ReplaceMarkWithHighObject(wordArr, subObjectList);

            return wordArr;
        }
    }
}
