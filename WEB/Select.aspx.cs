// --------------------------------
// <copyright file="Select.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using GisoFramework.Item;

public partial class Select : Page
{
    private string languageBrowser;
    private string ip;
    private string companyCode;

    public long UserId { get; private set; }

    public string BK
    {
        get
        {
            string path = this.Request.PhysicalApplicationPath;
            if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
            {
                path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}WelcomeBackgrounds\", path);

            string[] files = Directory.GetFiles(path);
            Random rnd = new Random();
            int index = rnd.Next(0, files.Count() - 1);
            string res = Path.GetFileName(files[index]);
            Session["BK"] = res;
            return res;
        }
    }

    public string LanguageBrowser
    {
        get 
        {
            return Thread.CurrentThread.CurrentUICulture.LCID.ToString();
        }
    }


    public string IP
    {
        get { return this.ip; }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["Navigation"] != null)
        {
            this.Response.Redirect("Default.aspx");
        }

        if(Session["Companies"] == null)
        {
            this.Response.Redirect("Default.aspx");
        }

        Session["Navigation"] = null;
        string id = this.Request.QueryString["action"].ToString();
        this.UserId = Convert.ToInt64(id.Split('-')[1]);
        this.RenderCompanies();
    }

    private void RenderCompanies()
    {
        StringBuilder res = new StringBuilder();
        List<string> companiesIds = Session["Companies"] as List<string>;
        string cssClass = "odd";
        string pattern = @"<td style=""height:60px;width:100px;border:1px solid #aaa;cursor:pointer;"" align=""center"" onclick=""Go({0},{4});""><strong>{1}</strong><br /><img src=""/Images/Logos/{5}?ac={6}"" class=""logo"" style=""max-width:90%;max-height:30px;"" /><br /></td>";
        int contCells = 0;
        if (companiesIds != null)
        {
            if (companiesIds.Count > 6)
            {
                res.Append("<tr><td><select id=\"CmbCompany\" style=\"width:100%;\">");
                res.Append(@"<option value=""0"">Seleccionar...</option>");
                foreach (string companyId in companiesIds)
                {
                    int cId = Convert.ToInt32(companyId.Split('|')[0]);
                    int uId = Convert.ToInt32(companyId.Split('|')[1]);
                    Company company = new Company(cId);
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        @"<option value=""{0}|{1}"">{2}</option>",
                        cId,
                        uId,
                        company.Name);
                }
                res.Append("</select></tr></td>");
                res.Append("<tr><td align=\"right\" style=\"padding-top:8px;\">");
                res.Append(@"<button type=""button"" class=""width-35 pull-right btn btn-sm btn-primary"" id=""BtnLoginCmb"" onclick=""GoFromCombo();"">Acceder</button>");
                res.Append("</td></tr>");
            }
            else
            {
                foreach (string companyId in companiesIds)
                {
                    int cId = Convert.ToInt32(companyId.Split('|')[0]);
                    int uId = Convert.ToInt32(companyId.Split('|')[1]);

                    string logo = Company.GetLogoFileName(cId);

                    if (contCells == 0)
                    {
                        res.Append("<tr>");
                    }

                    Company company = new Company(cId);
                    res.AppendFormat(
                        CultureInfo.InvariantCulture,
                        pattern,
                        cId,
                        company.Name,
                        cssClass,
                        company.Code,
                        uId,
                        logo,
                        Guid.NewGuid());

                    if (cssClass == "odd")
                    {
                        cssClass = "pair";
                    }
                    else
                    {
                        cssClass = "odd";
                    }

                    contCells++;
                    if (contCells > 2)
                    {
                        contCells = 0;
                        res.Append("</tr>");
                    }
                }

                if (contCells != 0)
                {
                    res.Append("</tr>");
                }
            }
        }

        Session["Companies"] = null;
        this.LtCompanies.Text = res.ToString();
    }
}