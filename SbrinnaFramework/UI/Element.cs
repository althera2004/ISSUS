// -----------------------------------------------------------------------
// <copyright file="Element.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace SbrinnaCoreFramework.UI
{
    /// <summary></summary>
    public abstract class Element
    {
        public int Expand { get; set; }
        public string Id { get; set; }

        public virtual string Html
        {
            get
            {
                return string.Empty;
            }
        }
    }
}