using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
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
            if (strData.StartsWith("{") && strData[1] != ' ' && strData[1] != '\n')
            {
                strData = strData.Remove(0, 1);
                strData = "{ " + strData;
            }
            if (strData.EndsWith("}") && strData[strData.Length - 2] != ' ' && strData[strData.Length - 2] != '\n')
            {
                strData = strData.Remove(strData.Length - 1, 1);
                strData = strData + " }";
            }

            if ("{" != StrManualLib.GetNextWordWithDelete(ref strData))
            {
                throw new Exception(INCORRECT_JSON_FORMAT);
            }

            data = new Dictionary<string, object>();

            string key;
            while ("}" != (key = StrManualLib.GetNextWordWithDelete(ref strData)) && strData.Length > 0)
            {
                key = StrManualLib.RemoveSpecialSymbol(key);
                string preValue = StrManualLib.GetNextWordWithDelete(ref strData);
                object value;
                if (preValue.StartsWith("\"")) // STRING
                {
                    value = StrManualLib.RemoveSpecialSymbol(preValue);
                }
                else if (preValue.StartsWith("[")) // LIST
                {
                    value = 0;
                }
                else if (preValue.StartsWith("{")) // JSON
                {
                    int index = strData.IndexOf('}');
                    preValue = strData.Remove(index + 1);
                    strData = strData.Remove(0, index + 1);
                    if (strData.StartsWith(","))
                    {
                        strData = strData.Remove(0, 1);
                    }
                    value = new JSON("{ " + preValue);
                }
                else // NUMBER
                {
                    preValue = StrManualLib.RemoveSpecialSymbol(preValue);
                    if (preValue.Contains(".")) // FLOAT
                    {
                        value = float.Parse(preValue, CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else // INTEGER
                    {
                        value = int.Parse(preValue);
                    }
                }

                data.Add(key, value);
            }

            if (key != "}")
            {
                throw new Exception(INCORRECT_JSON_FORMAT);
            }
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
    }
}
