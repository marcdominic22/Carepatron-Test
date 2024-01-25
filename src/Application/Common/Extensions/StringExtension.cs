using System;
using System.Collections.Generic;

namespace Application.Common.Extensions
{
    public static class StringExtension
    {
        public static string ToLowerFirst(string value) =>
               !string.IsNullOrEmpty(value) ?
               value.ToLower() :
               value;
    }
}