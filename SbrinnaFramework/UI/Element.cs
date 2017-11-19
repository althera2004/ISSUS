// -----------------------------------------------------------------------
// <copyright file="Element.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace SbrinnaCoreFramework.UI
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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
