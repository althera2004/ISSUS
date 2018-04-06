﻿
namespace GisoFramework
{
    using System;
    using System.Globalization;

    public static class Constant
    {
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