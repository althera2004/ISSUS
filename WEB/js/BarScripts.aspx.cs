// --------------------------------
// <copyright file="BarScripts.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SbrinnaCoreFramework.UI;

/// <summary>
/// Implements BAR javascript code generator
/// </summary>
public partial class js_BarScripts : Page
{
    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.Clear();
        this.Response.ContentType = "text/javascript";

        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.Response.Write(
            new BarPopup()
            {
                Id = "EquipmentScaleDivision",
                DeleteMessage = this.dictionary["Common_DeleteMessage"],
                BarWidth = 600,
                UpdateWidth = 600,
                DeleteWidth = 600,
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                Description = "Divisón de escala",
                FieldName = this.dictionary["Common_Name"],
                BarTitle = "División de escala"
            }.RenderScriptsDelete);
    }
}