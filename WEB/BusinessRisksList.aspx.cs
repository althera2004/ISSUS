// --------------------------------
// <copyright file="BusinessRisksList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;

public partial class BusinessRisksList : Page
{
    public string RulesJson
    {
        get
        {
            return Rules.GetAllJson(this.Company.Id);
        }
    }

    public string UserLanguage
    {
        get
        {
            return this.user.Language;
        }
    }

    public Company Company { get; set; }

    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>BusinessRisk of page</summary>
    private BusinessRisk businessRisk;

    /// <summary>BusinessRiskId of page</summary>
    private long businessRiskId;

    /// <summary>Public access to businessRisk</summary>
    public BusinessRisk BusinessRisk
    {
        get
        {
            return this.businessRisk;
        }
    }

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Lista de results</summary>
    public List<int> BRList;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return master.Dictionary;
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string FilterBusinessRisk { get; set; }

    public string FilterOportunity { get; set; }

    /// <summary>Header of the showed data</summary>
    public UIDataHeader DataHeader { get; set; }

    /// <summary>Json containing the items in the BusinessRisk</summary>
    public string RiskJson
    {
        get
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var BusinessRiskGraph in BusinessRisk.GetActive(Company.Id))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }
                res.Append(BusinessRiskGraph.JsonResult);
            }

            res.Append("]");
            return res.ToString();
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
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>Main action to load the page's elements</summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        this.Company = (Company)Session["company"];

        if (Session["BusinessRiskFilter"] == null)
        {
            this.FilterBusinessRisk = "null";
        }
        else
        {
            this.FilterBusinessRisk = Session["BusinessRiskFilter"].ToString();
        }

        if (Session["OportunityFilter"] == null)
        {
            this.FilterOportunity = "null";
        }
        else
        {
            this.FilterOportunity = Session["OportunityFilter"].ToString();
        }

        this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = false;
        this.master.AddBreadCrumb("Item_BusinessRisks");
        this.master.Titulo = "Item_BusinessRisks";

        this.RenderBusinessRiskData();

        if (this.user.HasGrantToWrite(ApplicationGrant.BusinessRisk))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_BusinessRisk_Button_New", "BusinessRiskView.aspx");
        }

        this.DataHeader = new UIDataHeader { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_BusinesRisk_ListHeader_Date"], Sortable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_BusinesRisk_ListHeader_Description"], Filterable = true, Sortable = true});
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th2", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_BusinesRisk_ListHeader_Process"], HiddenMobile = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th3", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_BusinesRisk_ListHeader_Rule"], HiddenMobile = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th4", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_BusinesRisk_ListHeader_StartValue"], HiddenMobile = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th5", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_BusinesRisk_ListHeader_Result"], HiddenMobile = true });
       
        if (this.Request.QueryString["id"] != null)
        {
            this.businessRiskId = Convert.ToInt64(this.Request.QueryString["id"]);
        }

        if (this.businessRiskId != -1)
        {
            this.businessRisk = BusinessRisk.ById(Company.Id, this.businessRiskId);
            if (this.businessRisk.CompanyId != Company.Id)
            {
                Context.ApplicationInstance.CompleteRequest();
                this.businessRisk = BusinessRisk.Empty;
            }
        }
        else
        {
            this.businessRisk = BusinessRisk.Empty;
        }
    }

    /// <summary>Renders the BusinessRisk data from database</summary>
    private void RenderBusinessRiskData()
    {
        /*StringBuilder res = new StringBuilder();
        StringBuilder sea = new StringBuilder();
        List<string> s = new List<string>();
        bool first = true;
        ReadOnlyCollection<BusinessRisk> risks = BusinessRisk.GetActive(((Company)Session["Company"]).Id);
        foreach (BusinessRisk risk in risks)
        {

            if (!s.Contains(risk.Description))
            {
                s.Add(risk.Description);
            }

            res.Append(risk.ListRow(this.dictionary, this.user.Grants));
        }

        BRList = new List<int>();
        risks = BusinessRisk.GetAll(((Company)Session["Company"]).Id);
        foreach (BusinessRisk risk in risks)
        {

            if (!BRList.Contains(risk.Result))
            {
                BRList.Add(risk.Result);
            }

            //res.Append(risk.ListRow(this.dictionary, this.user.Grants));
        }

        s.Sort();
        foreach (string item in s)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sea.Append(",");
            }

            if (item.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@"'{0}'", item));
            }
            else
            {
                sea.Append(string.Format(@"""{0}""", item));
            }
        }

        this.BusinessRiskData.Text = res.ToString();
        this.master.SearcheableItems = string.Empty;// sea.ToString();*/
        this.FillCombos();
    }

    public void FillCombos()
    {
        var processos = Process.ByCompany(this.Company.Id);
        var resp = new StringBuilder(@"<option value=""0"">").Append(this.dictionary["Common_All_Female_Plural"]).Append("</option>");
        foreach(var process in processos)
        {
            resp.AppendFormat(
                CultureInfo.GetCultureInfo("en-us"),
                @"<option value=""{0}"">{1}</option>",
                process.Id,
                process.Description);
        }

        this.LtCmbProcessOptions.Text = resp.ToString();
        this.LtCmbProcessOportunityOptions.Text = resp.ToString();

        var rules = Rules.GetActive(this.Company.Id);
        var resr = new StringBuilder(@"<option value=""0"">").Append(this.dictionary["Common_All_Female_Plural"]).Append("</option>");
        foreach (var rule in rules)
        {
            resr.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<option value=""{0}"">{1}</option>",
                rule.Id,
                rule.Description);
        }

        this.LtCmbRulesOptions.Text = resr.ToString();
        this.LtCmbRulesOportunityOptions.Text = resr.ToString();
    }
}