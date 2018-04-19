// --------------------------------
// <copyright file="DocumentVersionRow.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Web;

    public class DocumentVersionRow
    {
        /// <summary>Gets or sets document version identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets document identifier</summary>
        public long DocumentId { get; set; }

        /// <summary>Gets or sets document version</summary>
        public int Version { get; set; }

        /// <summary>Gets or sets date of version change </summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets reason of chane</summary>
        public string Reason { get; set; }

        /// <summary>Gets or sets document attach</summary>
        public string Attach { get; set; }

        /// <summary>Gets or sets who aproves document version</summary>
        public string AprovedBy { get; set; }

        /// <summary>Gets or sets company identifier</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets document extension</summary>
        public string Extension { get; set; }

        /// <summary>Gets the html code for row of document's version table</summary>
        public string Render(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Customer);

            string iconView = string.Empty;
            string iconEdit = string.Empty;
            string iconDelete = string.Empty;

            /*
             * <td style="width:150px;">
                        <span class="btn btn-xs btn-success" onclick="ShowPDF('Equipments_2_peñazo.png');">
                            <i class="icon-eye-open bigger-120"></i>
                        </span>
                        <span class="btn btn-xs btn-info">
                            <a class="icon-download bigger-120" href="/DOCS/12/Equipments_2_peñazo.png" target="_blank" style="color:#fff;"></a>
                        </span>
                        <span class="btn btn-xs btn-danger" onclick="DeleteUploadFile(9,'ostia!');">
                            <i class="icon-trash bigger-120"></i>
                        </span>
                    </td>*/

            if (grantWrite && this.DocumentId > 0)
            {
                string validDescription = Tools.LiteralQuote(Tools.JsonCompliant(this.Attach));
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "DocumentAttachDelete({0},'{1}');", this.DocumentId, this.Attach);
                iconView = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} {1}"" class=""btn btn-xs btn-success"" onclick=""ShowPDF({0});""><i class=""icon-eye-open bigger-120""></i></span>", this.DocumentId, validDescription, Tools.JsonCompliant(dictionary["Common_View"]));
                iconEdit = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span class=""btn btn-xs btn-info"" onclick=""window.open('/DOCS/{0}/Document_{1}_v{2}_{3}.{4}');""><i class=""icon-edit bigger-120""></i></span>", this.CompanyId, this.Id, this.Version, this.DocumentId, this.Extension);
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", deleteFunction, validDescription, Tools.JsonCompliant(dictionary["Common_Delete"]));
            }

            string pattern = @"
                                <tr>
                                    <td style=""width:80px;"">{0}{8}</td>
                                    <td style=""width:90px;"">{1:dd/MM/yyyy}</td>
                                    <td>{2}</td>
                                    <td style=""width:120px;"" id=""DOC{0}"">{3}</td>
                                    <td style=""width:80px;"">{4}</td>
                                    <td style=""width:150px;"" id=""Icons{0}"">{5}&nbsp;{6}&nbsp;{7}</td>
                                </tr>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                this.Version,
                this.Date,
                this.Reason,
                this.Attach,
                this.AprovedBy,
                iconView,
                iconEdit,
                iconDelete,
                string.Empty);
        }
    }
}