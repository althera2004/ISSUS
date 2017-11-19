// --------------------------------
// <copyright file="UploadFile.aspx.cs" company="Sbrinna">
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
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;

/// <summary>
/// Implements upload action form attach files
/// </summary>
public partial class Async_UploadFile : Page
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

        int ItemLinked = Convert.ToInt32(this.Request.Form["ItemLinked"].ToString());
        long ItemId = Convert.ToInt64(this.Request.Form["ItemId"].ToString());
        int companyId = Convert.ToInt32(this.Request.Form["CompanyId"].ToString());
        string description = this.Request.Form["Description"].ToString();
        int applicationUserId = Convert.ToInt32(this.Request.Form["ApplicationUserId"].ToString());
        string itemLinkedText = UploadFile.ResolveItemLinked(ItemLinked);
        string fileName = string.Format(@"{0}_{1}_{2}", itemLinkedText, ItemId, Path.GetFileName(file.FileName)).Replace("/", "-").Replace("/", "-");
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
        UploadFile uploadFile = new UploadFile()
        {
            FileName = fileName,
            Description = description,
            CompanyId = companyId,
            Extension = Path.GetExtension(file.FileName).Replace(".", string.Empty),
            ItemId = ItemId,
            ItemLinked = ItemLinked,
            Active = true,
            CreatedBy = new ApplicationUser() { Id = applicationUserId },
            CreatedOn = DateTime.Now,
            ModifiedBy = new ApplicationUser() { Id = applicationUserId },
            ModifiedOn = DateTime.Now,
            Size = length
        };

        ActionResult res = uploadFile.Insert(applicationUserId);
        this.Response.Clear();
        this.Response.ContentType = "application/json";
        this.Response.Write(res.MessageError);
        this.Response.Flush();
    }    
}