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

        private string ListToString(object list)
        {
            string result = "";
            const string INT_LIST = "System.Collections.Generic.List`1[System.Int32]";
            const string FLOAT_LIST = "System.Collections.Generic.List`1[System.Single]";
            const string JSON_LIST = "System.Collections.Generic.List`1[SimpleHTTPServer.JSON]";

            switch (list.ToString())
            {
                case INT_LIST:
                    List<int> intList = (List<int>)list;
                    foreach(var val in intList)
                    {
                        result += "    " + val + ",\n";
                    }
                    break;
                case FLOAT_LIST:
                    List<float> floatList = (List<float>)list;
                    foreach (var val in floatList)
                    {
                        string rightFloat = val.ToString().Replace(',', '.');
                        result += "    " + rightFloat + ",\n";
                    }
                    break;
                case JSON_LIST:
                    List<JSON> jsonList = (List<JSON>)list;
                    foreach (var val in jsonList)
                    {
                        result += val + ",\n";
                    }
                    result = result.Replace("\n", "\n    ");
                    result = result.Remove(result.Length - 4, 4);
                    result = "    " + result;
                    break;
                default:
                    break;
            }

            result = result.Remove(result.Length - 2, 2);
            result = "[\n" + result + "\n]";
            return result;
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
                string value = data[key].ToString();

                if (data[key].GetType() == "".GetType())
                {
                    value = "\"" + value + "\"";
                }
                else if (data[key].GetType() == 1.0f.GetType())
                {
                    value = value.Replace(',', '.');
                }
                else if (value.StartsWith("System.Collections.Generic.List"))
                {
                    value = ListToString(data[key]);
                }

                value = value.Replace("\n", "\n    ");
                strData += "\n    \"" + key + "\": " + value;
            }
            strData += "\n}";

            return strData;
        }

        // Private
        // ---------------------------------------------------------

        private void SetDictData(string strData)
        {
            data = new Dictionary<string, object>();
            string key = string.Empty;
            object value = null;
            string[] words = StrManualLib.SmartSplit(strData, new char[] { ':', ',' });

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


                    string[] listElement = StrManualLib.SmartSplit(valueString, new char[] { ',' });
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
