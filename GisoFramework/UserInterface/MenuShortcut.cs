// --------------------------------
// <copyright file="MenuShortcut.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.UserInterface
{
    /// <summary>Implementation of menu shortcut</summary>
    public struct MenuShortcut
    {
        /// <summary>Gets or sets the green shortcut</summary>
        public Shortcut Green { get; set; }

        /// <summary>Gets or sets the yellow shortcut</summary>
        public Shortcut Yellow { get; set; }

        /// <summary>Gets or sets the blue shortcut</summary>
        public Shortcut Blue { get; set; }

        /// <summary>Gets or sets the red shortcut</summary>
        public Shortcut Red { get; set; }
    }
}
