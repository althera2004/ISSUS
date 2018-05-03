// --------------------------------
// <copyright file="Variance.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    /// <summary>Implements Variance class</summary>
    public struct Variance
    {
        /// <summary>Gets or sets the name of property</summary>
        public string PropertyName { get; set; }

        /// <summary>Gets or sets the new value of property</summary>
        public object NewValue { get; set; }

        /// <summary>Gets or sets the old value of property</summary>
        public object OldValue { get; set; }
    }
}