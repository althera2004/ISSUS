// --------------------------------
// <copyright file="DataHeader.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    /// <summary>Implements DataHeader control</summary>
    public class UIDataHeader
    {
        public string Id { get; set; }
        private List<UIDataHeaderItem> items { get; set; }
        public int ActionsItem { get; set; }

        public ReadOnlyCollection<UIDataHeaderItem> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<UIDataHeaderItem>();
                }

                return new ReadOnlyCollection<UIDataHeaderItem>(this.items);
            }
        }

        public UIDataHeader()
        {
            this.items = new List<UIDataHeaderItem>();
            this.ActionsItem = 0;
        }

        public void AddItem(UIDataHeaderItem item)
        {
            if (this.items == null)
            {
                this.items = new List<UIDataHeaderItem>();
            }

            this.items.Add(item);
        }

        public string Render
        {
            get
            {
                var res = new StringBuilder("<thead class=\"thin-border-bottom\">").Append(Environment.NewLine);
                res.Append("\t\t<tr id=\"").Append(this.Id).Append("\">").Append(Environment.NewLine);
                foreach (var item in this.Items)
                {
                    res.Append("\t\t\t").Append(item.Render).Append(Environment.NewLine);
                }

                if (this.ActionsItem > 0)
                {
                    res.Append("\t\t\t");
                    res.Append(new UIDataHeaderActions { NumberOfActions = this.ActionsItem }.Render);
                    res.Append(Environment.NewLine);
                }

                res.Append("\t\t</tr>").Append(Environment.NewLine);
                res.Append("\t</thead>");
                return res.ToString();
            }
        }

    }
}