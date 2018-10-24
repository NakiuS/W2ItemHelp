using System;


namespace W2ItemHelp
{
    public static class MyExtension
    {
        public static string Hex(this int value)
        {
            return string.Format("{0:X}", value);
        }

        public static int GetIntFromHex(this string HexID)
        {

            return int.Parse(HexID, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
