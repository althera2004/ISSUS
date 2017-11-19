// --------------------------------
// <copyright file="MenuShortcut.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.UserInterface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MenuShortcut
    {
        /// <summary>Green shortcut</summary>
        private Shortcut green;

        /// <summary>Yellow shortcut</summary>
        private Shortcut yellow;

        /// <summary>Blue shortcut</summary>
        private Shortcut blue;

        /// <summary>Red shortcut</summary>
        private Shortcut red;

        /// <summary>
        /// Gets or sets the green shortcut
        /// </summary>
        public Shortcut Green
        {
            get
            {
                return this.green;
            }

            set
            {
                this.green = value;
            }
        }

        /// <summary>
        /// Gets or sets the yellow shortcut
        /// </summary>
        public Shortcut Yellow
        {
            get
            {
                return this.yellow;
            }

            set
            {
                this.yellow = value;
            }
        }

        /// <summary>
        /// Gets or sets the blue shortcut
        /// </summary>
        public Shortcut Blue
        {
            get
            {
                return this.blue;
            }

            set
            {
                this.blue = value;
            }
        }

        /// <summary>
        /// Gets or sets the red shortcut
        /// </summary>
        public Shortcut Red
        {
            get
            {
                return this.red;
            }

            set
            {
                this.red = value;
            }
        }
    }
}
