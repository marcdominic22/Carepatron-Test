using System;

namespace Application.Common.Helpers
{
    public class StringHelper
    {
        public static string ConvertStringToBase64(string value)
        {
            if (value == null) throw new ArgumentNullException(value);

            var stringToBytes = System.Text.Encoding.UTF8.GetBytes(value);
            var bytesToBase64 = Convert.ToBase64String(stringToBytes);

            return bytesToBase64;
        }
    }
}