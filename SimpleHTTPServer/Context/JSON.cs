using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    public enum ReturnValueType
    {
        STRING,
        JSON,
        LIST,
        FLOAT,
        INTEGER,
        UNKNOWN
    }

    public class JSON
    {
        public Dictionary<string, object> data;

        private const string INCORRECT_JSON_FORMAT = "Incorrect JSON format";

        public JSON(Dictionary<string, object> dictData)
        {
            data = new Dictionary<string, object>(dictData);
        }

        public JSON(string strData)
        {
            strData = strData.Replace(" ", string.Empty);
            strData = strData.Replace("\n", string.Empty);
            if (!strData.StartsWith("{") || !strData.EndsWith("}"))
            {
                throw new Exception(INCORRECT_JSON_FORMAT);
            }
            strData = StrManualLib.DeleteFirstAndLastChar(strData);
            SetDictData(strData);
        }

        public override string ToString()
        {
            string strData = "{";
            foreach (var key in data.Keys)
            {
                if (strData.Length > 1)
                {
                    strData += ",";
                }
                strData += "\n    \"" + key + "\": \"" + data[key] + "\"";
            }
            strData += "}";

            return strData;
        }

        // Private
        // ---------------------------------------------------------

        private void SetDictData(string strData)
        {
            data = new Dictionary<string, object>();
            string key = string.Empty;
            object value = null;
            string[] words = StrManualLib.SmartSplit(strData, new char[] { ':', ',' }); // strData.Split(new char[] { ':', ',' });

            for (int count = 0; count < words.Length; count += 2)
            {
                key = StrManualLib.DeleteFirstAndLastChar(words[count]);
                value = GetValue(words[count + 1]);
                data.Add(key, value);
            }
        }

        private object GetValue(string valueString)
        {
            ReturnValueType type = ReturnValueType.UNKNOWN;
            return GetValue(valueString, ref type);
        }

        private object GetTypeList(ReturnValueType valueType, string[] listElement)
        {
            switch (valueType)
            {
                case ReturnValueType.STRING:
                    var stringList = new List<string>();
                    foreach(var elem in listElement)
                    {
                        string value = elem;
                        stringList.Add(value);
                    }
                    return stringList;
                case ReturnValueType.JSON:
                    var jsonList = new List<JSON>();
                    foreach (var elem in listElement)
                    {
                        JSON value = new JSON(elem);
                        jsonList.Add(value);
                    }
                    return jsonList;
                case ReturnValueType.LIST:
                    return new List<object>(); // !!!
                case ReturnValueType.FLOAT:
                    var floatList = new List<float>();
                    foreach (var elem in listElement)
                    {
                        float value = float.Parse(elem, CultureInfo.InvariantCulture.NumberFormat);
                        floatList.Add(value);
                    }
                    return floatList;
                case ReturnValueType.INTEGER:
                    var intList = new List<int>();
                    foreach (var elem in listElement)
                    {
                        int value = int.Parse(elem);
                        intList.Add(value);
                    }
                    return intList;
                default:
                    return new List<object>();
            }
        }

        private object GetValue(string valueString, ref ReturnValueType valueType)
        {
            object value = null;

            switch (valueString[0])
            {
                case '"': // STRING
                    valueType = ReturnValueType.STRING;
                    value = StrManualLib.DeleteFirstAndLastChar(valueString);
                    break;
                case '{': // JSON
                    valueType = ReturnValueType.JSON;
                    value = new JSON(valueString);
                    break;
                case '[': // LIST
                    valueType = ReturnValueType.LIST;
                    valueString = StrManualLib.DeleteFirstAndLastChar(valueString);
                    if (valueString.Length == 0)
                    {
                        value = new List<object>();
                        break;
                    }

                    string[] listElement = valueString.Split(new char[] { ',' });
                    ReturnValueType type = ReturnValueType.UNKNOWN;
                    GetValue(listElement[0], ref type);

                    value = GetTypeList(type, listElement);
                    break;
                default: // NUMBER
                    if (valueString.Contains(".")) // FLOAT
                    {
                        valueType = ReturnValueType.FLOAT;
                        value = float.Parse(valueString, CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else // INTEGER
                    {
                        valueType = ReturnValueType.INTEGER;
                        value = int.Parse(valueString);
                    }
                    break;
            }

            return value;
        }
    }
}
