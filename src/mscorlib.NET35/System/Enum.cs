// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
namespace System
{
    public static class EnumEx
    {
        public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct
        {
            return TryParse(value, false, out result);
        }

        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
        {
            string strTypeFixed = value.Replace(' ', '_');
            if (Enum.IsDefined(typeof(TEnum), strTypeFixed))
            {
                result = (TEnum)Enum.Parse(typeof(TEnum), strTypeFixed, ignoreCase);
                return true;
            }
            else
            {
                foreach (string str in Enum.GetNames(typeof(TEnum)))
                {
                    if (str.Equals(strTypeFixed, StringComparison.OrdinalIgnoreCase))
                    {
                        result = (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
                        return true;
                    }
                }
                result = default;
                return false;
            }
        }
    }
}
