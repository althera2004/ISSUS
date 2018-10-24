// --------------------------------
// <copyright file="UploadFile.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;

/// <summary>Implements upload action form attach files</summary>
public partial class AsyncUploadFile : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        var file = this.Request.Files[0];
        string path = Request.PhysicalApplicationPath;
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }

        int itemLinked = Convert.ToInt32(this.Request.Form["ItemLinked"]);
        long itemId = Convert.ToInt64(this.Request.Form["ItemId"]);
        int companyId = Convert.ToInt32(this.Request.Form["CompanyId"]);
        string description = this.Request.Form["Description"];
        if (string.IsNullOrEmpty(description))
        {
            description = file.FileName;
        }

        int applicationUserId = Convert.ToInt32(this.Request.Form["ApplicationUserId"]);
        string itemLinkedText = UploadFile.ResolveItemLinked(itemLinked);
        string fileName = string.Format(@"{0}_{1}_{2}", itemLinkedText, itemId, Path.GetFileName(ToolsPdf.NormalizeFileName(file.FileName))).Replace("/", "-").Replace("/", "-");
        string fileDisk = string.Format(@"{0}DOCS\{1}\{2}", path, companyId, fileName);

        bool exists = File.Exists(fileDisk);
        int cont = 1;
        while (exists)
        {
            string fileDiskName1 = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string fileDiskNameAll = Path.GetFileName(fileName);

            if (fileDiskName1.EndsWith(")", StringComparison.OrdinalIgnoreCase))
            {
                string part1 = fileDiskName1.Substring(0, fileDiskName1.LastIndexOf("("));
                fileName = string.Format(CultureInfo.InvariantCulture, "{0} ({1}){2}", part1, cont, extension);
            }
            else
            {
                fileName = string.Format(CultureInfo.InvariantCulture, "{0} ({1}){2}", fileDiskName1, cont, extension);
            }

            fileDisk = string.Format(@"{0}DOCS\{1}\{2}", path, companyId, fileName);
            exists = File.Exists(fileDisk);
            cont++;
        }

        file.SaveAs(fileDisk);
        long length = new FileInfo(fileDisk).Length;
        var uploadFile = new UploadFile
        {
            FileName = fileName,
            Description = description,
            CompanyId = companyId,
            Extension = Path.GetExtension(file.FileName).Replace(".", string.Empty),
            ItemId = itemId,
            ItemLinked = itemLinked,
            Active = true,
            CreatedBy = new ApplicationUser { Id = applicationUserId },
            CreatedOn = Constant.Now,
            ModifiedBy = new ApplicationUser { Id = applicationUserId },
            ModifiedOn = Constant.Now,
            Size = length
        };

        var res = uploadFile.Insert(applicationUserId);
        this.Response.Clear();
        this.Response.ContentType = "application/json";
        this.Response.Write(res.MessageError);
        this.Response.Flush();
    }    
}