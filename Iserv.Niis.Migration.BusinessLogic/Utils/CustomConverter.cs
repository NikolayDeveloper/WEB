using System;

namespace Iserv.Niis.Migration.BusinessLogic.Utils
{
    public static class CustomConverter
    {
        private const string StringTrue = "T";
        private const string StringFalse = "F";
        public static bool? StringToNullableBool(string value)
        {
            switch (value)
            {
                case StringTrue:
                    return true;
                case StringFalse:
                    return false;
                default:
                    return false;
            }
        }
        public static bool StringToBool(string value)
        {
            switch (value)
            {
                case StringTrue:
                    return true;
                case StringFalse:
                    return false;
                default:
                    throw new ArgumentNullException(value);
            }
        }

        public static bool? IntToNullableBool(int? value)
        {
            switch (value)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    return null;
            }
        }
        public static bool IntToBool(int value)
        {
            switch (value)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    throw new ArgumentNullException(value.ToString());
            }
        }
    }
}
