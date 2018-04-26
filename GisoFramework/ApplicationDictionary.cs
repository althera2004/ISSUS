// --------------------------------
// <copyright file="ApplicationDictionary.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;

    /// <summary>Implements dictionary class</summary>
    public static class ApplicationDictionary
    {
        /// <summary>Loads the new language.</summary>
        /// <param name="language">Language code</param>
        /// <returns>A dictionary class with text for fixed labels</returns>
        public static Dictionary<string, string> LoadNewLanguage(string language)
        {
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            using (var input = new StreamReader(string.Format(CultureInfo.GetCultureInfo("en-us"), @"{0}\\dicc\\{1}.dicc", HttpContext.Current.Request.PhysicalApplicationPath, language)))
            {
                string linea = input.ReadLine();
                while (!string.IsNullOrEmpty(linea))
                {
                    if (linea.IndexOf("::", StringComparison.Ordinal) != -1)
                    {
                        linea = linea.Replace("::", "^");
                        string key = linea.Split('^')[0];
                        string value = linea.Split('^')[1];

                        if (string.IsNullOrEmpty(value))
                        {
                            value = key;
                        }

                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, value.Replace('\'', '´'));
                        }
                        else
                        {
                            dictionary[key] = value.Replace('\'', '´');
                        }
                    }

                    linea = input.ReadLine();
                }
            }

            var items = from pair in dictionary
                        orderby pair.Key ascending
                        select pair;
            var dictionaryFinal = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in items)
            {
                dictionaryFinal.Add(item.Key, item.Value);
            }

            HttpContext.Current.Session["Dictionary"] = dictionaryFinal;
            return dictionaryFinal;
        }

        /// <summary>Load a dictionary</summary>
        /// <param name="language">Code of language</param>
        /// <returns>A dictionary class with text for fixed labels</returns>
        public static Dictionary<string, string> Load(string language)
        {
            // Carga de diccionario
            var dictionary = new Dictionary<string, string>();
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                @"{0}\\dicc\\{1}.dicc",
                HttpContext.Current.Request.PhysicalApplicationPath,
                language);
            using (var input = new StreamReader(fileName, Encoding.UTF7))
            {
                string linea = input.ReadLine();
                while (!string.IsNullOrEmpty(linea))
                {
                    if (linea.IndexOf("::", StringComparison.Ordinal) != -1)
                    {
                        linea = linea.Replace("::", "^");
                        string key = linea.Split('^')[0];
                        string value = linea.Split('^')[1];

                        if (string.IsNullOrEmpty(value))
                        {
                            value = key;
                        }

                        if (!dictionary.ContainsKey(key))
                        {
                            dictionary.Add(key, value.Replace('\'', '´').Replace("\t", string.Empty));
                        }
                    }

                    linea = input.ReadLine();
                }
            }

            var items = from pair in dictionary
                        orderby pair.Key ascending
                        select pair;
            var dictionaryFinal = new Dictionary<string, string>();
            foreach (var item in items)
            {
                dictionaryFinal.Add(item.Key, item.Value);
            }

            HttpContext.Current.Session["Dictionary"] = dictionaryFinal;
            return dictionaryFinal;
        }
    }
}