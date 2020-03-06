using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleHTTPServer
{
    public class StrManualLib
    {
        public static string GetNextWordWithDelete(ref string str)
        {
            string word = "";
            string newStr = "";
            bool wordFound = false;

            foreach (char ch in str)
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

        public static string RemoveSpecialSymbol(string str)
        {
            str = str.Remove(0, 1);
            if (str[str.Length - 1] != '"')
            {
                str = str.Remove(str.Length - 1, 1);
            }

            return str.Remove(str.Length - 1, 1);
        }
    }
}
