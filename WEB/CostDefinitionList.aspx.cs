using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class CostDefinitionList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

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

    /// <summary>
    /// Gets a random value to prevents static cache files
    /// </summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
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
        this.user = (ApplicationUser)this.Session["User"];
        this.dictionary = this.Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        this.master.AddBreadCrumb("Item_CostDefinition");
        this.master.Titulo = "Item_CostDefinitions";

        this.RenderDepartmentData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_CostDefinition_Button_New", "CostDefinitionView.aspx");
        }

        this.DataHeader = new UIDataHeader() { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_CostDefinition_ListHeader_Name"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_CostDefinition_ListHeader_Amount"], HiddenMobile = true });
    }

    private void RenderDepartmentData()
    {
        StringBuilder res = new StringBuilder();
        StringBuilder sea = new StringBuilder();
        List<string> s = new List<string>();
        bool first = true;
        int cont = 0;
        foreach (CostDefinition cost in CostDefinition.GetByCompany(((Company)Session["Company"]).Id))
        {
            if (cost.Active)
            {
                if (!s.Contains(cost.Description))
                {
                    s.Add(cost.Description);
                }

                res.Append(cost.ListRow(this.dictionary, this.user.Grants));
                cont++;
            }
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

        this.CostDefinitionData.Text = res.ToString();
        this.master.SearcheableItems = sea.ToString();
        this.CostDefinitionDataTotal.Text = cont.ToString();
    }
}