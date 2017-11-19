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
        int? yearFrom = null;
        int? yearTo = null;
        int mode = 3;

        if (this.Request.QueryString["yearfrom"] != null)
        {
            if (this.Request.QueryString["yearfrom"].ToString() != "0")
            {
                yearFrom = Convert.ToInt32(this.Request.QueryString["yearfrom"], CultureInfo.GetCultureInfo("en-us"));
            }
        }

        if (this.Request.QueryString["yearto"] != null)
        {
            if (this.Request.QueryString["yearto"].ToString() != "0")
            {
                yearTo = Convert.ToInt32(this.Request.QueryString["yearto"], CultureInfo.GetCultureInfo("en-us"));
            }
        }

        if (this.Request.QueryString["mode"] != null)
        {
            mode = Convert.ToInt32(this.Request.QueryString["mode"], CultureInfo.GetCultureInfo("en-us"));
        }

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];

        // Establecer años
        string selectedFrom = yearFrom.HasValue ? string.Empty : " selected=\"selected\"";
        string selectedTo = yearTo.HasValue ? string.Empty : " selected=\"selected\"";
        this.LtYearFrom.Text = string.Format(@"<option value=""0""{0}>-</option>", selectedFrom);
        this.LtYearTo.Text = string.Format(@"<option value=""0""{0}>-</option>", selectedTo);

        for (int x = 2007; x < DateTime.Now.Year + 3; x++)
        {
            selectedFrom = string.Empty;
            selectedTo = string.Empty;

            if (yearFrom.HasValue)
            {
                if (x == yearFrom.Value)
                {
                    selectedFrom = " selected=\"selected\"";
                }
            }

            if (yearTo.HasValue)
            {
                if (x == yearTo.Value)
                {
                    selectedTo = " selected=\"selected\"";
                }
            }

            this.LtYearFrom.Text += string.Format(@"<option value=""{0}""{1}>{0}</option>", x, selectedFrom);
            this.LtYearTo.Text += string.Format(@"<option value=""{0}""{1}>{0}</option>", x, selectedTo);
        }

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
        StringBuilder res = new StringBuilder();
        List<string> s = new List<string>();
        foreach (Learning learning in this.learningFilter.Filter())
        {
            res.Append(learning.ListRow(this.dictionary, this.user.Admin));
            if (!s.Contains(learning.Description))
            {
                s.Add(learning.Description);
            }
        } 

        s.Sort();
        bool first = true;
        StringBuilder sea = new StringBuilder();
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

        this.master.SearcheableItems = sea.ToString();
        this.LtLearningTable.Text = res.ToString();
    }
}