using System;
using System.Linq;
using System.Text;

namespace solutions.day10
{
    public static class StringExtensions
    {
        public static byte[] ConvertToAscii(this string input)
        {
            input.Trim(' ');
            return Encoding.ASCII.GetBytes(input);
        }
    }
}