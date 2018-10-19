using System;


namespace W2ItemHelp
{
    public static class MyExtension
    {
        public static string Hex(this int value)
        {
            return string.Format("{0:X}", value);
        }
    }
}
