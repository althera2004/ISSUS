using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;

namespace SbrinnaCoreFramework
{
    public struct CodedQuery: IEquatable<CodedQuery>
    {
        /// <summary>Encrypted data</summary>
        private NameValueCollection query;

        /// <summary>Decrypted data</summary>
        private List<CodedQueryItem> data;

        /// <summary>Indicates if it is parsed</summary>
        private bool parsed;

        /// <summary>Gets a clean CodedQuery data</summary>
        public string CleanQuery
        {
            get
            {
                return Tools.Base64Decode(this.query.ToString());
            }
        }

        /// <summary>Gets the collection of decrypted values</summary>
        public ReadOnlyCollection<CodedQueryItem> Data
        {
            get
            {
                if (!this.parsed)
                {
                    this.Parse();
                    this.parsed = true;
                }

                return new ReadOnlyCollection<CodedQueryItem>(this.data);
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

            foreach (var pair in this.data)
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
                var res = new List<CodedQueryItem>();
                var parts = this.CleanQuery.Split('&');
                foreach (string part in parts)
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        if (part.IndexOf('=') != -1)
                        {
                            res.Add(new CodedQueryItem(part.Split('=')[0], part.Split('=')[1]));
                        }
                    }
                }

                this.data = res;
            }
        }

        public bool Equals(CodedQuery other)
        {
            throw new NotImplementedException();
        }
    }
}