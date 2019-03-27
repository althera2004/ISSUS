// --------------------------------
// <copyright file="DashBoardActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
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
    public ActionResult SetFilter(bool owners, bool others, bool passed)
    {
        var res = ActionResult.NoAction;
        string filter = string.Format(
            CultureInfo.InvariantCulture,
            @"{{""Owners"":{0},""Others"":{1},""Passed"":{2}}}",
            owners ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
            others ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
            passed ? Constant.JavaScriptTrue : Constant.JavaScriptFalse);
        Session["DashBoardFilter"] = filter;
        res.SetSuccess();
        return res;
    }
}