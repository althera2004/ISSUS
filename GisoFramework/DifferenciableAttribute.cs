// --------------------------------
// <copyright file="DifferenciableAttribute.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework
{
    using System;

    /// <summary>Implementation of the Differenciable attribute for class properties</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DifferenciableAttribute : Attribute
    {
        /// <summary>Indicates if apply</summary>
        private bool apply;

        /// <summary>Initializes a new instance of the DifferenciableAttribute class.</summary>
        /// <param name="apply">Indicates if apply</param>
        public DifferenciableAttribute(bool apply)
        {
            this.apply = apply;
        }

        /// <summary>Initializes a new instance of the DifferenciableAttribute class.</summary>
        public DifferenciableAttribute()
        {
            this.apply = true;
        }

        /// <summary>Gets a value indicating whether if apply</summary>
        public bool Apply
        {
            get
            {
                return this.apply;
            }
        }
    }
}