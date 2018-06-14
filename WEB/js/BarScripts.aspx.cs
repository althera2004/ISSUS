// --------------------------------
// <copyright file="BarScripts.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Web.UI;
using SbrinnaCoreFramework.UI;

/// <summary>Implements BAR javascript code generator</summary>
public partial class jsBarScripts : Page
{
    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Response.Clear();
        this.Response.ContentType = "text/javascript";

        var dictionary = Session["Dictionary"] as Dictionary<string, string>;

        this.Response.Write(
            new BarPopup
            {
                Id = "EquipmentScaleDivision",
                DeleteMessage = dictionary["Common_DeleteMessage"],
                BarWidth = 600,
                UpdateWidth = 600,
                DeleteWidth = 600,
                Required = true,
                RequiredMessage = dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = dictionary["Common_Error_NameAlreadyExists"],
                Description = "Divisón de escala",
                FieldName = dictionary["Common_Name"],
                BarTitle = "División de escala"
            }.RenderScriptsDelete);
    }
}