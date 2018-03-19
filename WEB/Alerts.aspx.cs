// --------------------------------
// <copyright file="Alerts.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework.Item;
using GisoFramework;
using SbrinnaCoreFramework.UI;
using System.Collections.ObjectModel;
using GisoFramework.Alerts;
using SbrinnaCoreFramework;

public partial class Alerts : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>
    /// Gets the dictionary for interface texts
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return master.Dictionary;
        }
    }

    public UIDataHeader DataHeader { get; set; }


    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
             this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                 this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        this.master.AddBreadCrumb("Item_Alerts");
        this.master.Titulo = "Item_Alerts";

        this.RenderAlerts();
        this.DataHeader = new UIDataHeader() { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Alert_ListHeader_Reason"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Alert_ListHeader_Description"], HiddenMobile = true });
    }

    private void RenderAlerts()
    {
        int cont = 0;
        StringBuilder alertEquipment = new StringBuilder();
        StringBuilder alertActions = new StringBuilder();
        StringBuilder alertIncident = new StringBuilder();
        StringBuilder alertLearning = new StringBuilder();
        StringBuilder alertOther = new StringBuilder();

        ReadOnlyCollection<AlertDefinition> show = Session["AlertsDefinition"] as ReadOnlyCollection<AlertDefinition>;
        if (show.Count() > 0)
        {
            foreach (AlertDefinition alertDefinition in show)
            {
                if (this.user.HasGrantToRead(alertDefinition.ItemType))
                {
                    ReadOnlyCollection<string> alerts = alertDefinition.RenderRow(this.dictionary);
                    foreach (string result in alerts)
                    {
                        alertOther.Append(result);
                    }

                    cont += alerts.Count;
                }
            }

            alertOther.Append("<div></div>");
        }
        
        if(cont == 0)
        {
            alertOther.Append(@"<tr><td colspan=""3"" align=""center"" style=""width:100%;height:200px;background-color:#A3DC99;color:#527D4A""><br /><br /><br /><h3><i class=""icon-check""></i>" + this.dictionary["Common_NoAlerts"] + "</h3></td></tr>");
        }

        this.AlertsData.Text = alertOther.ToString();
    }
}