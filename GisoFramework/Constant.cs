
namespace GisoFramework
{
    using System;
    using System.Globalization;

    public static class Constant
    {
        public const string JavascriptTrue = "true";
        public const string JavascriptFalse = "false";

        public const int ColumnSpan2 = 2;
        public const int ColumnSpan3 = 3;
        public const int ColumnSpan4 = 4;
        public const int ColumnSpan6 = 6;
        public const int ColumnSpan8 = 8;
        public const int ColumnSpan12 = 12;

        public const int MaximumTextAreaLength = 2000;

        public const string EmptyJsonList = "[]";

        public const string EmptyJsonObject = "{}";

        public const bool NotLeaft = false;

        public static DateTime Now
        {
            get
            {
                return DateTime.Now.ToLocalTime();
            }
        }

        public static string NowText
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:dd/MM/yyyy}", Now);
            }
        }

        public const bool EndResponse = true;
    }
}
