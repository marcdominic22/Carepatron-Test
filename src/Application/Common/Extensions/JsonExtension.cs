using System;

namespace Application.Common.Extensions
{
    public static class JsonExtension
    {
        public static bool IsBooleanType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(bool) ||
                type == typeof(int))
                return true;

            return false;
        }

        public static bool IsNumberType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(int) ||
                type == typeof(long))
                return true;

            return false;
        }

        public static bool IsStringType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(string))
                return true;

            return false;
        }
    }
}