// --------------------------------
// <copyright file="FormacionList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class FormacionList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    public string DateFrom { get; private set; }
    public string DateTo { get; private set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    public string UserLanguage
    {
        get
        {
            return this.user.Language;
        }
    }

    private LearningFilter learningFilter;

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
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

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        DateTime? yearFrom = null;
        DateTime? yearTo = null;
        int mode = 3;

        if (this.Request.QueryString["yearfrom"] != null)
        {
            if (this.Request.QueryString["yearfrom"] != "0")
            {
                yearFrom = GisoFramework.Tools.TextToDate(this.Request.QueryString["yearfrom"]);
                DateFrom = this.Request.QueryString["yearfrom"];
            }
        }

        if (this.Request.QueryString["yearto"] != null)
        {
            if (this.Request.QueryString["yearto"] != "0")
            {
                yearTo = GisoFramework.Tools.TextToDate(this.Request.QueryString["yearto"]);
                DateTo = this.Request.QueryString["yearto"];
            }
        }

        if (this.Request.QueryString["mode"] != null)
        {
            mode = Convert.ToInt32(this.Request.QueryString["mode"], CultureInfo.InvariantCulture);
        }

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];

        switch (mode)
        {
            case 0:
                this.status0.Attributes.Add("checked", "cheked");
                break;
            case 1:
                this.status1.Attributes.Add("checked", "cheked");
                break;
            case 2:
                this.status2.Attributes.Add("checked", "cheked");
                break;
            case 3:
                this.status3.Attributes.Add("checked", "cheked");
                break;
        }

        if (!this.Page.IsPostBack)
        {
            this.learningFilter = new LearningFilter(this.company.Id) { Mode = mode, YearFrom = yearFrom, YearTo = yearTo };
        }

        this.dictionary = Session["Dictionary"] as Dictionary<string, string>; 
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Learnings");
        this.master.Titulo = "Item_Learning";
        this.RenderLearningData();
        
        if (this.user.HasGrantToWrite(ApplicationGrant.Learning))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Learning_Button_New", "FormacionView.aspx");
        }
    }

    private void RenderLearningData()
    {
        decimal total = 0;
        int count = 0;
        var res = new StringBuilder();
        var searchedItems = new List<string>();
        foreach (var learning in this.learningFilter.Filter())
        {
            res.Append(ListRow(this.dictionary, learning));
            if (!searchedItems.Contains(learning.Description))
            {
                searchedItems.Add(learning.Description);
            }

            count++;
            total += learning.Amount;
        } 

        searchedItems.Sort();
        bool first = true;
        var sea = new StringBuilder();
        foreach (string item in searchedItems)
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

        this.master.SearcheableItems = sea.ToString();
        this.LtLearningTable.Text = res.ToString();
        this.LtCount.Text = count.ToString();
        this.LtTotal.Text = string.Format(
            CultureInfo.InvariantCulture,
            //@"{0:#0.00}",
			@"{0:#,##0.00}",
            total).Replace(',','*').Replace('.',',').Replace('*','.');
    

        //this.LtTotal.Text = string.Format(CultureInfo.InvariantCulture,@"{0:#,##0.00}",total).Replace('.',',');
		this.LtTotal.Text = string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:#,##0.00}",total);	//GTK
	}

    /// <summary>Gets a row of learning for learnings list table</summary>
    /// <param name="dictionary">Dictionary for fixed labels</param>
    /// <param name="admin">Indicates if user that views table has administration role</param>
    /// <returns>Html code to render learning row</returns>
    public string ListRow(Dictionary<string, string> dictionary, Learning formacion)
    {
        string month = GisoFramework.Tools.TranslatedMonth(formacion.DateEstimated.Month, dictionary);

        var res = new StringBuilder();
        string iconDeleteAction = "LearningDelete";

        // @alex: al poner la descripcion sustiuir ' por \' para evitar un javascript mal formado 
        string iconUpdate = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""LearningUpdate({0});""><i class=""icon-edit bigger-120""></i></span>", formacion.Id, formacion.Description, dictionary["Common_Edit"]);
        formacion.Description = formacion.Description.Replace('\'', '´');
        string iconDelete = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""{3}({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", formacion.Id, formacion.Description, dictionary["Common_Delete"], iconDeleteAction);

        res.Append("<tr>");
        res.Append("<td>").Append(formacion.Link).Append("</td>");
        res.Append("<td align=\"center\" style=\"width:100px;white-space: nowrap;\">");
        res.AppendFormat(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", formacion.DateEstimated);
        res.Append("<td align=\"center\" style=\"width:100px;white-space: nowrap;\">");
        res.AppendFormat(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", formacion.RealFinish);

        string amountText = string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", formacion.Amount).Replace(".", ",");

        res.Append("</td>");

        string statusText = string.Empty;
        switch (formacion.Status)
        {
            case 0:
                statusText = dictionary["Item_Learning_Status_InProgress"];
                break;
            case 1:
                statusText = dictionary["Item_Learning_Status_Finished"];
                break;
            case 2:
                statusText = dictionary["Item_Learning_Status_Evaluated"];
                break;
            default:
                statusText = string.Empty;
                break;
        }

        res.Append("<td align=\"center\" class=\"hidden-480\" style=\"width:100px;white-space: nowrap;\">").Append(statusText).Append("</td>");
        res.Append("<td align=\"right\" class=\"hidden-480\" style=\"width:150px;white-space: nowrap;\">").Append(amountText).Append("</td>");
        res.Append("<td class=\"hidden-480\" style=\"width:90px;white-space: nowrap;\">");
        res.Append(iconUpdate).Append("&nbsp;").Append(iconDelete);
        res.Append("</td>");
        res.Append("</tr>");
        return res.ToString();
    }
}