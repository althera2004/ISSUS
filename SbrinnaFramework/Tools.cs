// --------------------------------
// <copyright file="tools.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework
{
    using System;
    using System.Globalization;
    using System.Text;

    /// <summary>Framweork tools</summary>
    public static class Tools
    {
        public const bool OnlyFirstLetter = true;

        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
            {
                return string.Empty;
            }

            if (base64EncodedData.EndsWith("%3d", StringComparison.Ordinal))
            {
                base64EncodedData = base64EncodedData.Replace("%3d", "=");
            }

            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GeneratePassword
        {
            get
            {
                return RandomPassword.Generate(5, 8);
            }
        }

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

        public static string JsonCompliant(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("'", "\\'");
        }

        public static string LimitLength(string text, int length)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            if (text.Length < length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        public static string Capitalize(string value)
        {
            return Capitalize(value, OnlyFirstLetter);
        }

        public static string Capitalize(string value, bool onlyFirst)
        {
            if(string.IsNullOrEmpty(value))
            {
                value = string.Empty;
            }

            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length == 1)
            {
                return value.ToUpperInvariant();
            }

            value = value.ToLowerInvariant();

            if (onlyFirst)
            {
                return value.Substring(0, 1).ToUpperInvariant() + value.Substring(1).ToLowerInvariant();
            }

            var res = new StringBuilder();
            foreach (string word in value.Split(' '))
            {
                res.Append(Capitalize(word)).Append(" ");
            }

            return res.ToString().Trim();
        }
    }
}