// --------------------------------
// <copyright file="CustomersView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class CustomersView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    private int customerId;
    private Customer customer;
    private FormFooter formFooter;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    private TabBar tabBar = new TabBar() { Id = "CustomerTabBar" };

    public string TxtName
    {
        get
        {
            return new FormText()
            {
                Name = "TxtName",
                Value = this.customer.Description,
                ColumnSpan = 11,
                Placeholder = this.dictionary["Item_Customer"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Customer),
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

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public string Items
    {
        get
        {
            StringBuilder res = new StringBuilder();
            bool first = true;
            foreach (var customer in Customer.GetByCompany(((Company)Session["Company"]).Id))
            {
                if (customer.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(customer.JsonKeyValue);
                }
            }

            return res.ToString();
        }
    }

    public int CustomerId
    {
        get
        {
            return this.customerId;
        }
    }

    public Customer CustomerItem
    {
        get
        {
            return this.customer;
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
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
            {
                this.Response.Redirect("NoAccesible.aspx", true);
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
        this.company = this.Session["company"] as Company;
        this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.user = this.Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.customerId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        string label = "Item_Customer_Detail";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Customers", "CustomersList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;

        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Action = "success", Text = this.dictionary["Common_Accept"] });
        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.customerId != -1)
        {
            this.customer = Customer.GetById(this.customerId, this.company.Id);
            if (this.customer.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.customer = Customer.Empty;
            }

            this.formFooter.ModifiedBy = this.customer.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.customer.ModifiedOn;

            StringBuilder tableActions = new StringBuilder();
            ReadOnlyCollection<CustomerIncidentActions> actions = CustomerIncidentActions.GetByCustomer(this.customer);
            foreach (CustomerIncidentActions action in actions)
            {
                tableActions.Append(action.Row(this.dictionary, this.user.Grants));
            }

            this.TableActions.Text = tableActions.ToString();
            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Customer"], this.customer.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
            //// this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.customerId, TargetType.Customer);
        }
        else
        {
            this.customer = Customer.Empty;
            this.TableActions.Text = string.Empty;
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Available = true, Active = true, Selected = true, Label = this.dictionary["Item_Customers_Tab_Principal"] });
        //// this.tabBar.AddTab(new Tab() { Id = "trazas", Label = this.dictionary["Item_Customers_Tab_Traces"], Active = this.customerId > 0, Available = this.user.HasTraceGrant() });
    }
}