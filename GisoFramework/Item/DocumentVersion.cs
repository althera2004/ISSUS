// --------------------------------
// <copyright file="DocumentVersion.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Globalization;

    /// <summary>Enumeration of document status</summary>
    public enum DocumentStatus
    {
        /// <summary>0 - Draft</summary>
        Draft = 0,

        /// <summary>1 - Publish</summary>
        Publish = 1,

        /// <summary>2 - Obsolete</summary>
        Obsolete = 2
    }

    /// <summary>Implementation of DocumentVersion class.</summary>
    public class DocumentVersion
    {
        /// <summary>Initializes a new instance of the DocumentVersion class.</summary>
        public DocumentVersion()
        {
        }

        /// <summary>Gets or sets the state of doucment in this version</summary>
        public DocumentStatus State { get; set; }

        /// <summary>Gets or sets the version identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the identifier of document</summary>
        public long DocumentId { get; set; }

        /// <summary>Gets or sets the number of version</summary>
        public int Version { get; set; }

        /// <summary>Gets or sets the user that performs the last modification of version</summary>
        public ApplicationUser User { get; set; }

        /// <summary>Gets or sets the compnay of version</summary>
        public Company Company { get; set; }

        /// <summary>Gets or sets the date of version</summary>
        public DateTime Date { get; set; }

        /// <summary>Gets or sets the reason of version</summary>
        public string Reason { get; set; }

        /// <summary>Gets or sets the name of creator</summary>
        public string UserCreateName { get; set; }

        /// <summary>Gets the html code for row of document's version table</summary>
        public string TableRow
        {
            get
            {
                string pattern = @"
                                <tr>
                                    <td style=""width:80px;"">{0}</td>
                                    <td style=""width:80px;"">{1}</td>
                                    <td>{2}</td>
                                    <td style=""width:150px;"">{3}</td>
                                </tr>";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"), 
                    pattern,
                    this.Version,
                    string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:dd/MM/yyyy}", this.Date),
                    this.Reason,
                    this.UserCreateName);
            }
        }

        /// <summary>Obtains the version status represented by an integer</summary>
        /// <param name="value">Integer that represents status</param>
        /// <returns>Status of version</returns>
        public static DocumentStatus IntegerToStatus(int value)
        {
            switch (value)
            {
                default:
                case 1:
                    return DocumentStatus.Publish;
                case 2:
                    return DocumentStatus.Obsolete;
            }

            return DocumentStatus.Draft;
        }
    }
}