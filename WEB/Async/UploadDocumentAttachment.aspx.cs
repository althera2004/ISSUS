// --------------------------------
// <copyright file="UploadDocumentAttachment.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using System.Globalization;

public partial class Async_UploadDocumentAttachment : Page
{
    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpPostedFile file = this.Request.Files[0];
        string path = Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }

        long ItemId = Convert.ToInt64(this.Request.Form["ItemId"].ToString());
        int companyId = Convert.ToInt32(this.Request.Form["CompanyId"].ToString());
        string description = this.Request.Form["Description"].ToString();
        string version = this.Request.Form["Version"].ToString();
        int applicationUserId = Convert.ToInt32(this.Request.Form["ApplicationUserId"].ToString());
        string extension = Path.GetExtension(file.FileName).Replace(".", string.Empty);

        DocumentAttach uploadFile = new DocumentAttach()
        {
            DocumentId = ItemId,
            Description = description,
            CompanyId = companyId,
            Version = Convert.ToInt32(version),
            Extension = extension,
            Active = true,
            CreatedBy = new ApplicationUser() { Id = applicationUserId },
            CreatedOn = DateTime.Now,
            ModifiedBy = new ApplicationUser() { Id = applicationUserId },
            ModifiedOn = DateTime.Now
        };

        ActionResult res = uploadFile.Insert(applicationUserId);

        // Document_7_V12_1
        string fileName = string.Format(@"Document_{0}_V{1}_{2}.{3}", ItemId, version, uploadFile.Id, extension);

        string folder = string.Format(CultureInfo.InvariantCulture, @"{0}DOCS\{1}\", path, companyId);
        string filePattern = string.Format(CultureInfo.InvariantCulture, @"Document_{0}_V{1}_*.*", ItemId, version);
        string[] files = Directory.GetFiles(folder, filePattern);
        foreach(string fileVictim in files)
        {
            if(File.Exists(fileVictim))
            {
                File.Delete(fileVictim);
            }
        }

        file.SaveAs(string.Format(@"{0}DOCS\{1}\Document_{2}_V{3}_{4}.{5}", path, companyId, ItemId, version, uploadFile.Id, extension));

        this.Response.Clear();
        this.Response.ContentType = "application/json";
        this.Response.Write(res.MessageError);
        this.Response.End();
    }
}