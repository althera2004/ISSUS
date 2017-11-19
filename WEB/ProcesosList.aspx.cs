// --------------------------------
// <copyright file="ProcesosList.aspx.cs" company="Sbrinna">
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
using GisoFramework.Item;
using SbrinnaCoreFramework;

/// <summary>
/// Implements Process list page
/// </summary>
public partial class ProcesosList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

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
        this.user = (ApplicationUser)Session["User"];
        this.company = Session["Company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Processes");
        this.master.Titulo = "Item_Processes";
        this.RenderProcesosData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = new SbrinnaCoreFramework.UI.UIButton()
            {
                Text = this.dictionary["Item_Process_Button_New"],
                Action = "success",
                Icon = "icon-plus-sign",
                Id = "BtnNewDocument",
                EventClick = "document.location='ProcesosView.aspx?id=-1';"
            };
        }
    }

    /// <summary>
    /// Render HTML code for process list content
    /// </summary>
    private void RenderProcesosData()
    {
        ReadOnlyCollection<ProcessType> procesos = ProcessType.ObtainByCompany(this.company.Id, this.dictionary);
        StringBuilder res = new StringBuilder();
        ReadOnlyCollection<Process> processList = Process.GetByCompany(((Company)Session["Company"]).Id);
        List<string> s = new List<string>();
        bool first = true;
        this.ProcesosDataTotal.Text = processList.Count.ToString();

        
            bool grantWrite = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.Process);
            bool grantDelete = UserGrant.HasDeleteGrant(this.user.Grants, ApplicationGrant.Process);

        foreach (Process process in processList)
        {
            string resposableDescription = string.Empty;
            string processTypeName = string.Empty;
            foreach (ProcessType processType in procesos)
            {
                if (processType.Id == process.ProcessType)
                {
                    processTypeName = processType.Description;
                    break;
                }
            }

            if (process.JobPosition != null)
            {
                resposableDescription = process.JobPosition.Link;

                if (!s.Contains(process.JobPosition.Description))
                {
                    s.Add(process.JobPosition.Description);
                }
            }
            else
            {
                resposableDescription = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span style=""color:#f00;"">{0}</span>", this.dictionary["Item_Process_Status_WhitoutResponsible"]);
            }

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                if (process.CanBeDeleted)
                {
                    iconDelete = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<span title=""{0} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""ProcessDelete(this);""><i class=""icon-trash bigger-120""></i></span>",
                        this.dictionary["Common_Delete"],
                        process.Description);
                }
                else
                {
                    iconDelete = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<span title=""{0} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""ProcessNoDelete(this);""><i class=""icon-trash bigger-120""></i></span>",
                        this.dictionary["Common_Delete"],
                        process.Description);
                }
            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='ProcesosView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                process.Id,
                this.dictionary["Common_View"],
                process.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='ProcesosView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                process.Id,
                this.dictionary["Common_Edit"],
                process.Description);

            }

            string pattern = @"<tr id=""{0}""><td vertical-align=""middle""><a href=""ProcesosView.aspx?id={0}"">{1}</a></td><td style=""width:180px;padding-left:4px;""><div title='{6}' style=""width:165px;"" class=""truncate"">{2}</div></td><td style=""width:180px;padding-left:4px;""><div title='{3}' style=""width:165px;"" class=""truncate"">{3}</td><td style=""width:90px;"">{4}&nbsp;{5}</td></tr>";
            res.Append(string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                process.Id,
                process.Description,
                resposableDescription,
                processTypeName,
                iconEdit,
                iconDelete,
                process.JobPosition.Description));

            s.Add(process.Description);
        }

        s.Sort();
        first = true;

        StringBuilder sea = new StringBuilder();
        foreach (string item in s)
        {
            if (!string.IsNullOrEmpty(item))
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
        }

        this.ProcesosData.Text = res.ToString();
        this.master.SearcheableItems = sea.ToString();
    }
}