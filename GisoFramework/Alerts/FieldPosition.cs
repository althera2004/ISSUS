// --------------------------------
// <copyright file="fieldposition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Alerts
{
    using Newtonsoft.Json;

    /// <summary>Implements FieldPosition class</summary>
    public class FieldPosition
    {
        /// <summary>Gets or sets the position of filed</summary>
        [JsonProperty("Position")]
        public int Position { get; set; }

        /// <summary>Gets or sets the name of filed </summary>
        [JsonProperty("FieldName")]
        public string FieldName { get; set; }
    }
}