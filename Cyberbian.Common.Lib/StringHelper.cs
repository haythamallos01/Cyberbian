using System;
using System.Linq;

namespace Cyberbian.Common.Lib
{
    public class StringHelper
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
