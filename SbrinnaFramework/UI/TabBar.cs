// --------------------------------
// <copyright file="TabBar.cs" company="OpenFramework">
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

    /// <summary>Implements TabBar class</summary>
    public class TabBar
    {
        private List<Tab> tabs;

        public string Id { get; set; }

        public bool Visible { get; set; }

        public void AddTab(Tab tab)
        {
            if (this.tabs == null)
            {
                this.tabs = new List<Tab>();
            }

            this.tabs.Add(tab);
        }

        public ReadOnlyCollection<Tab> Tabs
        {
            get
            {
                if(this.tabs == null)
                {
                    this.tabs = new List<Tab>();
                }

                return new ReadOnlyCollection<Tab>(this.tabs);
            }
        }

        public TabBar()
        {
            this.Visible = true;
            this.tabs = new List<Tab>();
        }

        public string Render
        {
            get
            {
                if (this.Visible)
                {
                    var res = new StringBuilder("<ul class=\"nav nav-tabs padding-18\">");
                    foreach (var tab in this.Tabs)
                    {
                        res.Append(Environment.NewLine);
                        res.Append(tab.Render);
                    }

                    res.Append("</ul>");
                    return res.ToString();
                }

                return string.Empty;
            }
        }
    }
}