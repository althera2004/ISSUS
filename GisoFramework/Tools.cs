// --------------------------------
// <copyright file="Tools.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.Item;

    /// <summary>Implements Tools class.</summary>
    public static class Tools
    {
        public static ActionResult DeleteAttachs(int companyId, string itemName, long itemId)
        {
            var res =  ActionResult.NoAction;
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            path = string.Format(@"{0}DOCS\{1}", path, companyId);
            var searchPattenr = string.Format("{0}_{1}_*.*", itemName, itemId);
            var filePaths = Directory.GetFiles(path, searchPattenr);
            {
                foreach(var fileName in filePaths)
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
            }
            return res;
        }

        /// <summary>Gets a date from text</summary>
        /// <param name="text">Text to convert</param>
        /// <returns>Date from text</returns>
        public static DateTime? TextToDateYYYYMMDD(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var parts = text.Split('/');
            if (parts.Length == 3)
            {
                return DateTime.ParseExact(text, "yyyyMMdd", CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>Gets a date from text</summary>
        /// <param name="text">Text to convert</param>
        /// <returns>Date from text</returns>
        public static DateTime? TextToDate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var parts = text.Split('/');
            if (parts.Length == 3)
            {
                return DateTime.ParseExact(text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>Gets a text to represent a money value for pdf content</summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Text to represent a money value for pdf content</returns>
        public static string PdfMoneyFormat(decimal value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", value).Replace(",", "*").Replace(".", ",").Replace("*", ".");
        }

        /// <summary>Make a string with decimal format</summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string NumberFormat(decimal? value)
        {
            if (value.HasValue)
            {
                return NumberFormat(value.Value);
            }

            return string.Empty;
        }

        /// <summary>Make a string with decimal format</summary>
        /// <param name="value">Decimal value</param>
        /// <returns>String with decimal format</returns>
        public static string NumberFormat(decimal value)
        {
            string res = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.000000}", value);
            while (res.EndsWith("0"))
            {
                res = res.Substring(0, res.Length - 1);
            }

            if (res.EndsWith("."))
            {
                res = res.Substring(0, res.Length - 1);
            }

            return res.Replace(".", ",");
        }

        /// <summary>Creates resume for a text</summary>
        /// <param name="text">Text to resume</param>
        /// <param name="length">Maximum length</param>
        /// <returns>Resume of text</returns>
        public static string Resume(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            text = text.Substring(0, length);
            int pos = text.LastIndexOf(' ');
            if (pos != -1)
            {
                text = text.Substring(0, pos);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}...", text);
        }

        /// <summary>Prepare a texto to be HTML compliant</summary>
        /// <param name="text">Text to Htmlize</param>
        /// <returns>Html compliant text</returns>
        public static string SetHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string res = text.Replace("\"", "&quot;");
            res = res.Replace("&", "&amp;");
            return res;
        }

        /// <summary>Prepares a text to be a tooltip</summary>
        /// <param name="text">Texto to prepare</param>
        /// <returns>Text ready for to be a tooltip</returns>
        public static string SetTooltip(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("\"", "˝");
        }

        /// <summary>Create a JSON pair key/value from a BaseItem value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">BaseItem value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, BaseItem value)
        {
            if (value == null)
            {
                return "\"" + key + "\":null";
            }

            return "\"" + key + "\":" + value.JsonKeyValue;
        }

        /// <summary>Create a JSON pair key/value from a string value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, string value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": ""{1}""", key, JsonCompliant(value));
        }

        /// <summary>Create a JSON pair key/value from a string value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, bool value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1}", key, value == true ? "true" : "false");
        }

        /// <summary>Create a JSON pair key/value from a DateTime value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">DateTime value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, DateTime value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1:yyyyMMdd}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a integer value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Integer value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, int value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a long value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Long value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, long value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a decimal value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal value)
        {
            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1:#0.00}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a decimal value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <param name="decimals">Number of decimals</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal value, int decimals)
        {
            string pattern = @"""{0}"": {1:#0.";
            for (int x = 0; x < decimals; x++)
            {
                pattern += "0";
            }

            pattern += "}";
            return string.Format(CultureInfo.InvariantCulture, pattern, key, value);
        }

        /// <summary>Create a JSON pair key/value from a decimal nullable value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal? value)
        {
            if (value.HasValue)
            {
                return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1:#0.00}", key, value);
            }

            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": null", key);
        }

        /// <summary>Create a JSON pair key/value from a decimal nullable value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Decimal value</param>
        /// <param name="decimals">Number of decimals</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, decimal? value, int decimals)
        {
            if (value.HasValue)
            {
                string pattern = @"""{0}"": {1:#0.";
                for (int x = 0; x < decimals; x++)
                {
                    pattern += "0";
                }

                pattern += "}";
                return string.Format(CultureInfo.InvariantCulture, pattern, key, value);
            }

            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": null", key);
        }

        /// <summary>Create a JSON pair key/value from a Double value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">Double value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, double value)
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"""{0}"": {1:#0.00}", key, value);
        }

        /// <summary>Create a JSON pair key/value from a string value</summary>
        /// <param name="key">Name of the key</param>
        /// <param name="value">String value</param>
        /// <returns>Json representation of key/value pair</returns>
        public static string JsonPair(string key, DateTime? value)
        {
            if (value == null)
            {
                return string.Format(CultureInfo.InvariantCulture, @"""{0}"": null", key);
            }

            return string.Format(CultureInfo.InvariantCulture, @"""{0}"": {1:yyyyMMdd}", key, value);
        }

        /// <summary>Replace quote for literal quote</summary>
        /// <param name="text">Original text</param>
        /// <returns>Text with literal quotes</returns>
        public static string LiteralQuote(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace('\'', '´');
        }

        public static string JsonValue(DateTime? value)
        {
            if (value.HasValue)
            {
                return string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", value.Value);
            }

            return Constant.JavaScriptNull;
        }

        /// <summary>Gets Javascript value for boolean value</summary>
        /// <param name="value">Boolean value</param>
        /// <returns>Javascript value</returns>
        public static string JsonValue(bool value)
        {
            return value ? Constant.JavaScriptTrue : Constant.JavaScriptFalse;
        }

        /// <summary>Gets Javascript value for boolean value</summary>
        /// <param name="value">Boolean value</param>
        /// <returns>Javascript value</returns>
        public static string JsonValue(bool? value)
        {
            if (value.HasValue)
            {
                return value.Value ? Constant.JavaScriptTrue : Constant.JavaScriptFalse;
            }

            return Constant.JavaScriptNull;
        }

        /// <summary>Create a JSON compliant text</summary>
        /// <param name="text">Original text</param>
        /// <returns>JSON compliant text</returns>
        public static string JsonCompliant(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace("\n", "\\n");
            text = text.Replace("\r", string.Empty);
            text = text.Replace("\\", "\\\\");
            return text.Replace("\"", "\\\"");
        }

        /// <summary>Gets a limited length string</summary>
        /// <param name="text">Original string</param>
        /// <param name="length">Maximum length</param>
        /// <returns>A limited length string</returns>
        public static string LimitedText(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>Gets trasnlated month name</summary>
        /// <param name="monthNumber">Month number</param>
        /// <param name="dictionary">Dictionary for month names</param>
        /// <returns>Translated month name</returns>
        public static string TranslatedMonth(int monthNumber, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            switch (monthNumber)
            {
                case 1:
                    return dictionary["Common_MonthName_January"];
                case 2:
                    return dictionary["Common_MonthName_February"];
                case 3:
                    return dictionary["Common_MonthName_March"];
                case 4:
                    return dictionary["Common_MonthName_April"];
                case 5:
                    return dictionary["Common_MonthName_May"];
                case 6:
                    return dictionary["Common_MonthName_June"];
                case 7:
                    return dictionary["Common_MonthName_July"];
                case 8:
                    return dictionary["Common_MonthName_August"];
                case 9:
                    return dictionary["Common_MonthName_September"];
                case 10:
                    return dictionary["Common_MonthName_October"];
                case 11:
                    return dictionary["Common_MonthName_November"];
                case 12:
                    return dictionary["Common_MonthName_December"];
                default:
                    return string.Empty;
            }
        }

        /// <summary>Search the text on dictionary</summary>
        /// <param name="text">Dictionary key</param>
        /// <returns>Translated text by dictionary, or original text if not found</returns>
        public static string Translate(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            if (dictionary.ContainsKey(text))
            {
                return dictionary[text];
            }

            return text;
        }
    }
}