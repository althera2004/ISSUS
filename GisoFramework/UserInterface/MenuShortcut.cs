// --------------------------------
// <copyright file="MenuShortcut.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => base.ToString();
    }
}
