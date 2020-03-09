using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    class JSON
    {
        public Dictionary<string, string> data;

        private const string INCORRECT_JSON_FORMAT = "Incorrect JSON format";

        public JSON(Dictionary<string, string> dictData)
        {
            data = new Dictionary<string, string>(dictData);
        }

        public JSON(string strData)
        {
            if ("{" != StrManualLib.GetNextWordWithDelete(ref strData))
            {
                throw new Exception(INCORRECT_JSON_FORMAT);
            }

            data = new Dictionary<string, string>();

            string key;
            while ("}" != (key = StrManualLib.GetNextWordWithDelete(ref strData)) && strData.Length > 0)
            {
                string value = StrManualLib.RemoveSpecialSymbol(StrManualLib.GetNextWordWithDelete(ref strData));
                key = StrManualLib.RemoveSpecialSymbol(key);
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
