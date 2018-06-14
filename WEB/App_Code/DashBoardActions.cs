// --------------------------------
// <copyright file="DashBoardActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;

/// <summary>Descripción breve de DashBoardActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DashBoardActions : WebService
{

    public DashBoardActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SetFilter(bool owners, bool others)
    {
        var res = ActionResult.NoAction;
        string filter = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""Owners"":{0},""Others"":{1}}}",
            owners ? "true" : "false",
            others ? "true" : "false");
        Session["DashBoardFilter"] = filter;
        res.SetSuccess();
        return res;
    }
}