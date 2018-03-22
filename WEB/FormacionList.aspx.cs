// --------------------------------
// <copyright file="FormacionList.aspx.cs" company="Sbrinna">
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

    /// <summary>
    /// Gets dictionary for fixed labels
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
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
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
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
        foreach (Learning learning in this.learningFilter.Filter())
        {
            res.Append(learning.ListRow(this.dictionary, this.user.Admin));
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
            total).Replace('.',',');
    

        //this.LtTotal.Text = string.Format(CultureInfo.InvariantCulture,@"{0:#,##0.00}",total).Replace('.',',');
		this.LtTotal.Text = string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:#,##0.00}",total);	//GTK
	}
}