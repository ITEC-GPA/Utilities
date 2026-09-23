using System;

namespace GPC.Utilities.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Returns the input string with the first character converted to uppercase
        /// </summary>
        /// <exception cref="ArgumentException">If string is null or empty</exception>

        public static string FirstLetterToUpperCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("String null or empty");

            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string FirstLetterToUpperCaseOrConvertNullToEmptyString(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            char[] chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
    }
}