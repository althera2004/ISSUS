// --------------------------------
// <copyright file="Variance.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;

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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}