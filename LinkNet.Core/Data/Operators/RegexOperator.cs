using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LinkNet.Core.Data.Operators
{
    public class RegexOperator
    {
        public static string? GetFirstWord(string text)
        {
            Regex regex = new Regex(@"[A-Z][a-z]+");

            Match? firstWord = regex.Matches(text).FirstOrDefault();

            return firstWord != null
                ? firstWord.Value
                : null;
        }
    }
}
