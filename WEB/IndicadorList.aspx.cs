// --------------------------------
// <copyright file="IndicadorList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class IndicadorList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public string Filter { get; set; }

    public long ActiondId { get; set; }

    public IncidentAction Action { get; set; }

    public string Procesos
    {
        get
        {
            return Process.GetByCompanyJsonList(this.company.Id);
        }
    }

    public string TiposProceso
    {
        get
        {
            return ProcessType.GetByCompanyJsonList(this.company.Id, this.Dictionary);
        }
    }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            this.ApplicationUser = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.ApplicationUser.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
            }
            else
            {
                this.Go();
            }
        }

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.ApplicationUser = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];

        if (Session["IndicadorFilter"] == null)
        {
            this.Filter = "null";
        }
        else
        {
            this.Filter = Session["IndicadorFilter"].ToString();
        }

        this.Dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AddBreadCrumb("Item_Indicadores");
        this.master.Titulo = "Item_Indicadores";

        if (this.ApplicationUser.HasGrantToWrite(ApplicationGrant.Incident))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Indicador_Button_New", "IndicadorView.aspx");
        }

        this.RenderProcessTypeList();
        this.RenderProcessList();
        this.RenderObjetivoList();
    }

    private void RenderObjetivoList()
    {
        var res = new StringBuilder();
        foreach (var objetivo in Objetivo.GetActive(this.company.Id))
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1}</option>",
                objetivo.Id,
                objetivo.Name);
        }

        this.LtObjetivoList.Text = res.ToString();
    }

    private void RenderProcessList()
    {
        var res = new StringBuilder();
        foreach (var process in Process.ByCompany(this.company.Id))
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1}</option>",
                process.Id,
                process.Description);
        }

        this.LtProcessList.Text = res.ToString();
    }

    private void RenderProcessTypeList()
    {
        var res = new StringBuilder();
        foreach (ProcessType processType in ProcessType.ObtainByCompany(this.company.Id, this.Dictionary))
        {
            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1}</option>",
                processType.Id,
                processType.Description);
        }

        this.LtProcessTypeList.Text = res.ToString();
    }
}