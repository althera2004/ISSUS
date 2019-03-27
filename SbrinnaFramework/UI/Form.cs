// --------------------------------
// <copyright file="Form.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary></summary>
    public class Form
    {
        private List<Element> elements;

        public string Title { get; set; }

        public string Html
        {
            get
            {
                var res = new StringBuilder("<!-- Rendered form -->").Append(Environment.NewLine);
                if (!string.IsNullOrEmpty(this.Title))
                {
                    res.Append("<h4>").Append(this.Title).Append("</h4>").Append(Environment.NewLine);
                }

                res.Append(@"<div class=""tab-content no-border padding-24"">
                                                <div id=""home"" class=""tab-pane active"">                                                
                                                    <form class=""form-horizontal"" role=""form"">");

                foreach (var element in this.elements)
                {
                    res.Append(element.Html);
                }

                res.Append("</form></div></div>");
                res.Append("<!-- Edn rendered form -->").Append(Environment.NewLine);

                return res.ToString();
            }
        }

        public ReadOnlyCollection<Element> Elements
        {
            get
            {
                if (this.elements == null)
                {
                    this.elements = new List<Element>();
                }

                return new ReadOnlyCollection<Element>(this.elements);
            }
        }

        public void AddElement(Element element)
        {
            if (this.elements == null)
            {
                this.elements = new List<Element>();
            }

            this.elements.Add(element);
        }
    }
}