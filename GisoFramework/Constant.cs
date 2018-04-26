﻿// --------------------------------
// <copyright file="Constant.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Globalization;

    /// <summary>Constant application values</summary>
    public static class Constant
    {
        /// <summary>Text for JavaScript true value</summary>
        public const string JavascriptTrue = "true";

        /// <summary>Text for JavaScript false value</summary>
        public const string JavascriptFalse = "false";

        /// <summary>Text for JavaScript null value</summary>
        public const string JavascriptNull = "null";

        /// <summary>Value for a two span division on bootstrap grid</summary>
        public const int ColumnSpan2 = 2;

        /// <summary>Value for a three span division on bootstrap grid</summary>
        public const int ColumnSpan3 = 3;

        /// <summary>Value for a four span division on bootstrap grid</summary>
        public const int ColumnSpan4 = 4;

        /// <summary>Value for a six span division on bootstrap grid</summary>
        public const int ColumnSpan6 = 6;

        /// <summary>Value for a eight span division on bootstrap grid</summary>
        public const int ColumnSpan8 = 8;

        /// <summary>Value for a twelve span division on bootstrap grid</summary>
        public const int ColumnSpan12 = 12;

        /// <summary>Maximum length for textarea fields in database</summary>
        public const int MaximumTextAreaLength = 2000;

        /// <summary>Text for JavaScript empty list value</summary>
        public const string EmptyJsonList = "[]";

        /// <summary>Text for JavaScript empty json object value</summary>
        public const string EmptyJsonObject = "{}";

        /// <summary>Menu option is not leaft</summary>
        public const bool NotLeaft = false;

        /// <summary>Actual date time</summary>
        public static DateTime Now
        {
            get
            {
                return DateTime.Now.ToLocalTime();
            }
        }

        /// <summary>Formatted text for actual date time</summary>
        public static string NowText
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:dd/MM/yyyy}", Now);
            }
        }

        /// <summary>Page execution has end response</summary>
        public const bool EndResponse = true;
    }
}