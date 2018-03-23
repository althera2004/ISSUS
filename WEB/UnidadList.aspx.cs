using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

/// <summary>
/// Implements a class for the "UnidadList" page
/// </summary>
public partial class UnidadList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private ApplicationUser user;

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

    /// <summary>
    /// Gets the dictionary for interface texts
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
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

        // Security access control
        if (!this.user.HasGrantToRead(ApplicationGrant.Provider))
        {
            this.Response.Redirect("NoPrivileges.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Unidades");
        this.master.Titulo = "Item_Unidades";
        this.RenderUnidadData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = new SbrinnaCoreFramework.UI.UIButton()
            {
                Text = this.dictionary["Item_Unidad_Btn_New"],
                Action = "success",
                Icon = "icon-plus",
                Id = "BtnNewUnidad"
            };
        }

        this.DataHeader = new UIDataHeader() { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Unidad_ListHeader_Description"], Sortable = true, Filterable = true });
    }

    private void RenderUnidadData()
    {
        StringBuilder res = new StringBuilder();
        StringBuilder sea = new StringBuilder();
        List<string> s = new List<string>();
        bool first = true;
        int contData = 0;
        foreach (Unidad unidad in Unidad.GetActive(((Company)Session["Company"]).Id))
        {
            if (unidad.Active)
            {
                if (!s.Contains(unidad.Description))
                {
                    s.Add(unidad.Description);
                }

                res.Append(unidad.ListRow(this.dictionary, this.user.Grants));
                contData++;
            }
        }

        this.ProviderDataTotal.Text = contData.ToString();

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

        this.UnidadData.Text = res.ToString();
        this.master.SearcheableItems = sea.ToString();
    }
}