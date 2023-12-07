using System;

namespace FTS.Tools.ExtensionMethods
{
    public static class RandomNameGenerator
    {
        private static readonly char[] Chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

        private static readonly Random Random = new Random();

        public static string GenerateRandomString(this string self, int length)
        {
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = Chars[Random.Next(Chars.Length)];
            }

            return new string(stringChars);
        }
    }
}