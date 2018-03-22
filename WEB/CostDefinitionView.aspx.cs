// --------------------------------
// <copyright file="CostDefinitionView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
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

/// <summary>
/// Implements page for cost definition view
/// </summary>
public partial class CostDefinitionView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private int costDefinitionId;
    private CostDefinition costDefinition;
    private FormFooter formFooter;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    private TabBar tabBar = new TabBar() { Id = "CostDefinitionTabBar" };

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
            return new FormText()
            {
                Name = "TxtName",
                Value = this.costDefinition.Description,
                ColumnSpan = 11,
                Placeholder = this.dictionary["Item_CostDefinition_Field_Description"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Provider),
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
            return this.formFooter.Render(this.dictionary);
        }
    }

    public string CostDefinitions
    {
        get
        {
            StringBuilder res = new StringBuilder();
            bool first = true;
            foreach (CostDefinition costDefinition in CostDefinition.GetByCompany(((Company)Session["Company"]).Id).Where(cd => cd.Active == true))
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

    public CostDefinition CostDefinitionItem
    {
        get
        {
            return this.costDefinition;
        }
    }

    public Company Company
    {
        get
        {
            return this.company;
        }
    }

    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public new ApplicationUser User
    {
        get
        {
            return this.user;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
        {
             this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                int test = 0;
                if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
                {
                    this.Response.Redirect("NoAccesible.aspx", true);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }

            this.Go();
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.user = Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.costDefinitionId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        string label = "Item_CostDefinition";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_CostDefinitions", "CostDefinitionList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.Dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.costDefinitionId != -1)
        {
            this.costDefinition = CostDefinition.GetById(this.costDefinitionId, this.company.Id);
            if (this.costDefinition.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.costDefinition = CostDefinition.Empty;
            }

            this.formFooter.ModifiedBy = this.CostDefinitionItem.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.CostDefinitionItem.ModifiedOn;

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Provider"], this.costDefinition.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.costDefinition = CostDefinition.Empty;
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Available = true, Active = true, Selected = true, Label = this.dictionary["Item_CostDefinition_Tab_Principal"] });
    }
}