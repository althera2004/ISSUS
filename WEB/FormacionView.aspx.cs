// --------------------------------
// <copyright file="FormacionView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class FormacionView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private FormFooter formFooter;
    
    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    private int learningId;
    private Learning learning;
    private StringBuilder jsonAssistance;

    private StringBuilder assistans;

    public string MesPrevisto
    {
        get
        {
            if (this.learning.Status != 2)
            {
                return string.Empty;
            }

            string res = string.Empty;
            switch (this.learning.DateEstimated.Month)
            {
                case 1: res = "Common_MonthName_January"; break;
                case 2: res = "Common_MonthName_February"; break;
                case 3: res = "Common_MonthName_March"; break;
                case 4: res = "Common_MonthName_April"; break;
                case 5: res = "Common_MonthName_May"; break;
                case 6: res = "Common_MonthName_June"; break;
                case 7: res = "Common_MonthName_July"; break;
                case 8: res = "Common_MonthName_August"; break;
                case 9: res = "Common_MonthName_September"; break;
                case 10: res = "Common_MonthName_October"; break;
                case 11: res = "Common_MonthName_November"; break;
                case 12: res = "Common_MonthName_December"; break;
            }

            if (!string.IsNullOrEmpty(res))
            {
                return this.dictionary[res];
            }

            return string.Empty;
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string Assistants
    {
        get
        {
            return this.assistans.ToString();
        }
    }

    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

    public string JsonAssistance
    {
        get
        {
            return this.jsonAssistance.ToString();
        }
    }

    public Learning Learning
    {
        get
        {
            return this.learning;
        }
    }

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
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"], out test))
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
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
        this.user = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.Request.QueryString["id"] != null)
        {
            this.learningId = Convert.ToInt32(this.Request.QueryString["id"]);               
        }

        if (this.learningId != -1)
        {
            this.learning = new Learning(this.learningId, this.company.Id);
            if (this.learning.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.learning = Learning.Empty;
            }

            this.formFooter.ModifiedBy = this.learning.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.learning.ModifiedOn;

            this.learning.ObtainAssistance();
            this.RenderDocuments();
        }
        else
        {
            this.learning = Learning.Empty;
        }

        string label =  string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Learning"], this.learning.Description);
        if (this.learningId == -1)
        {
            label = "Item_Learning_ToolTip_New";
        }

        this.master = this.Master as Giso;
        this.master.TitleInvariant = this.learningId != -1;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Learnings", "FormacionList.aspx", false);
        this.master.AddBreadCrumb("Item_Learning_Edit");
        this.master.Titulo = label;

        var tableAssistance = new StringBuilder();
        jsonAssistance = new StringBuilder("[");
        this.assistans = new StringBuilder();
        bool first = true;
        foreach (var assistance in this.learning.Assistance)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                jsonAssistance.Append(",");
                this.assistans.Append(",");
            }

            tableAssistance.Append(assistance.LearningRow(this.dictionary, this.learning.Status));
            jsonAssistance.Append(string.Format(@"{{""EmployeeId"":{0},""AssistantId"":{1}}}", assistance.Employee.Id, assistance.Id));
            this.assistans.Append(assistance.Id);
        }

        jsonAssistance.Append("]");
        this.TableAssistance.Text = tableAssistance.ToString();

        this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.learningId, TargetType.Learning);

        this.LtYearPrevistos.Text = string.Format(CultureInfo.InvariantCulture, @"<option value="""">{0}</option>", this.dictionary["Common_Year"]);

        for (int x = DateTime.Now.Year; x < DateTime.Now.Year + 3; x++)
        {
            string selected = string.Empty;
            if (this.learning.Id > 0)
                if (this.learning.DateEstimated.Year == x)
                {
                    selected = " selected=\"selected\"";
                }

            this.LtYearPrevistos.Text += string.Format(@"<option value=""{0}""{1}>{0}</option>", x, selected);
        }
    }
    
    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(10, this.learningId, this.company.Id);
        var res = new StringBuilder();
        var resList = new StringBuilder();
        int contCells = 0;
        var extensions = ToolsFile.ExtensionToShow;
        foreach (var file in files)
        {
            decimal finalSize = ToolsFile.FormatSize((decimal)file.Size);
            string fileShowed = string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description;
            if (fileShowed.Length > 15)
            {
                fileShowed = fileShowed.Substring(0, 15) + "...";
            }

            string viewButton = string.Format(
                CultureInfo.InvariantCulture,
                @"<div class=""col-sm-2 btn-success"" onclick=""ShowPDF('{0}');""><i class=""icon-eye-open bigger-120""></i></div>",
                file.FileName
                );

            string listViewButton = string.Format(
                CultureInfo.InvariantCulture,
                @"<span class=""btn btn-xs btn-success"" onclick=""ShowPDF('{0}');"">
                            <i class=""icon-eye-open bigger-120""></i>
                        </span>",
                file.FileName);

            var fileExtension = Path.GetExtension(file.FileName);

            if (!extensions.Contains(fileExtension))
            {
                viewButton = "<div class=\"col-sm-2\">&nbsp;</div>";
                listViewButton = "<span style=\"margin-left:30px;\">&nbsp;</span>";
            }

            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<div id=""{0}"" class=""col-sm-3 document-container"">
                        <div class=""col-sm-6"">&nbsp</div>
                        {10}
                        <div class=""col-sm-2 btn-info""><a class=""icon-download bigger-120"" href=""/DOCS/{3}/{4}"" target=""_blank"" style=""color:#fff;""></a></div>
                        <div class=""col-sm-2 btn-danger"" onclick=""DeleteUploadFile({0},'{1}');""><i class=""icon-trash bigger-120""></i></div>
                        <div class=""col-sm-12 iconfile"" style=""max-width: 100%;"">
                            <div class=""col-sm-4""><img src=""/images/FileIcons/{2}.png"" /></div>
                            <div class=""col-sm-8 document-name"">
                                <strong title=""{1}"">{9}</strong><br />
                                {7}: {5:dd/MM/yyyy}
                                {8}: {6:#,##0.00} MB
                            </div>
                        </div>
                    </div>",
                    file.Id,
                    fileShowed, //string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                    file.Extension,
                    this.company.Id,
                    file.FileName,
                    file.CreatedOn,
                    finalSize,
                    this.Dictionary["Item_Attachment_Header_CreateDate"],
                    this.dictionary["Item_Attachment_Header_Size"],
                    fileShowed,
                    viewButton);

            resList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr id=""tr{2}"">
                    <td>{1}</td>
                    <td align=""center"" style=""width:90px;"">{4:dd/MM/yyyy}</td>
                    <td align=""right"" style=""width:120px;"">{5:#,##0.00} MB</td>
                    <td style=""width:150px;"">
                        {6}
                        <span class=""btn btn-xs btn-info"">
                            <a class=""icon-download bigger-120"" href=""/DOCS/{3}/{0}"" target=""_blank"" style=""color:#fff;""></a>
                        </span>
                        <span class=""btn btn-xs btn-danger"" onclick=""DeleteUploadFile({2},'{1}');"">
                            <i class=""icon-trash bigger-120""></i>
                        </span>
                    </td>
                </tr>",
                file.FileName,
                fileShowed,// string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                file.Id,
                this.company.Id,
                file.CreatedOn,
                finalSize,
                listViewButton);

            contCells++;
            if (contCells == 4)
            {
                contCells = 0;
                res.Append("<div style=\"clear:both\">&nbsp;</div>");
            }
        }

        this.LtDocuments.Text = res.ToString();
        this.LtDocumentsList.Text = resList.ToString();
    }
}