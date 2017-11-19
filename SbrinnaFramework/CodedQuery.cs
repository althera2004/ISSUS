using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SbrinnaCoreFramework
{
    public struct CodedQuery
    {
        /// <summary>Encrypted data</summary>
        private NameValueCollection query;

        /// <summary>Decrypted data</summary>
        private List<KeyValuePair<string, object>> data;

        /// <summary>Indicates if it is parsed</summary>
        private bool parsed;

        /*
        /// <summary>
        /// Initializes a new instance of the CodedQuery class
        /// </summary>
        /// <param name="query">Encrypted data</param>
        public CodedQuery(NameValueCollection query)
        {
            this.query = query;
        }
        */

        /// <summary>Gets a clean CodedQuery data</summary>
        public string CleanQuery
        {
            get
            {
                return Tools.Base64Decode(this.query.ToString());
            }
        }

        /// <summary>Gets the collection of decrypted values</summary>
        public ReadOnlyCollection<KeyValuePair<string, object>> Data
        {
            get
            {
                if (!this.parsed)
                {
                    this.Parse();
                }

                return new ReadOnlyCollection<KeyValuePair<string, object>>(this.data);
            }
        }

        /// <summary>Set the query</summary>
        /// <param name="queryParameters">Query data</param>
        public void SetQuery(NameValueCollection queryParameters)
        {
            this.query = queryParameters;
        }

        /// <summary>Get value of pair by key</summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="key">Key to resolve</param>
        /// <returns>An object of type "T"</returns>
        public T GetByKey<T>(string key)
        {
            if (!this.parsed)
            {
                this.Parse();
            }

            if (this.data == null || this.data.Count == 0)
            {
                return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
            }

            foreach (KeyValuePair<string, object> pair in this.data)
            {
                if (pair.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return (T)Convert.ChangeType(pair.Value, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return (T)Convert.ChangeType(default(T), typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>Parse encrypted data to decrypted KeyValuePair collection</summary>
        private void Parse()
        {
            if (this.data == null)
            {
                List<KeyValuePair<string, object>> res = new List<KeyValuePair<string, object>>();
                string[] parts = this.CleanQuery.Split('&');
                foreach (string part in parts)
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        if (part.IndexOf('=') != -1)
                        {
                            res.Add(new KeyValuePair<string, object>(part.Split('=')[0], part.Split('=')[1]));
                        }
                    }
                }

                this.data = res;
            }
        }
    }
}
