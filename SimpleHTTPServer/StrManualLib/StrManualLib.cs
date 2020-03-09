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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
