// --------------------------------
// <copyright file="BreadCrumbItem.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.UserInterface
{
    /// <summary>Implements then BreadCrumbItem class</summary>
    public struct BreadcrumbItem
    {
        /// <summary>Gets or sets a value indicating whether the link of breadcrumb item</summary>
        public string Link { get; set; }

        /// <summary>Gets or sets a value indicating whether the label of breadcrumb item</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets a value indicating whether if item is leaf</summary>
        public bool Leaf { get; set; }

        /// <summary>Gets or sets a value indicating whether if prevents text translation</summary>
        public bool Invariant { get; set; }
    }
}