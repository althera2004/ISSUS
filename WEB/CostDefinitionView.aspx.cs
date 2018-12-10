// --------------------------------
// <copyright file="CostDefinitionView.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;

/// <summary>Implements page for cost definition view</summary>
public partial class CostDefinitionView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private int costDefinitionId;
    private FormFooter formFooter;

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

    private TabBar tabBar = new TabBar { Id = "CostDefinitionTabBar" };

    public bool IsActual
    {
        get
        {
            return false;
        }
    }

    public string TxtName
    {
        get
        {
            return new FormText
            {
                Name = "TxtName",
                Value = this.CostDefinitionItem.Description,
                ColumnSpan = 11,
                Placeholder = this.Dictionary["Item_CostDefinition_Field_Description"],
                Required = true,
                RequiredMessage = this.Dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.User.HasGrantToWrite(ApplicationGrant.Provider),
                MaximumLength = 100
            }.Render;
        }
    }

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.Dictionary);
        }
    }

    public string CostDefinitions
    {
        get
        {
            var res = new StringBuilder();
            bool first = true;
            foreach (var costDefinition in CostDefinition.ByCompany(((Company)Session["Company"]).Id).Where(cd => cd.Active == true))
            {
                if (costDefinition.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(costDefinition.JsonKeyValue);
                }
            }

            return res.ToString();
        }
    }

    public int CostDefinitionId
    {
        get
        {
            return this.costDefinitionId;
        }
    }

    public CostDefinition CostDefinitionItem { get; private set; }

    public Dictionary<string, string> Dictionary { get; private set; }

    public new ApplicationUser User { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else
            {
                int test = 0;
                if (!int.TryParse(this.Request.QueryString["id"], out test))
                {
                    this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                }
            }

            this.Go();
        }

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.company = (Company)Session["company"];
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.User = Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.costDefinitionId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        string label = "Item_CostDefinition";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_CostDefinitions", "CostDefinitionList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_CostDefinitions_Title");
        this.master.Titulo = "Item_CostDefinitions_Title";

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.costDefinitionId != -1)
        {
            this.CostDefinitionItem = CostDefinition.ById(this.costDefinitionId, this.company.Id);
            if (this.CostDefinitionItem.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.CostDefinitionItem = CostDefinition.Empty;
            }

            this.formFooter.ModifiedBy = this.CostDefinitionItem.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.CostDefinitionItem.ModifiedOn;

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Cost"], this.CostDefinitionItem.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.CostDefinitionItem = CostDefinition.Empty;
        }

        this.tabBar.AddTab(new Tab { Id = "home", Available = true, Active = true, Selected = true, Label = this.Dictionary["Item_CostDefinition_Tab_Principal"] });
    }
}