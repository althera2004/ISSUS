// --------------------------------
// <copyright file="CodedQueryItem.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework
{
    /// <summary>Implements item of a coded query</summary>
    public class CodedQueryItem
    {
        /// <summary>Initializes a new instance of the CodedQueryItem class.</summary>
        /// <param name="key">Item key</param>
        /// <param name="value">Item value</param>
        public CodedQueryItem(string key, object value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>Gets or sets item key</summary>
        public string Key { get; set; }

        /// <summary>Gets or sets item value</summary>
        public object Value { get; set; }
    }
}