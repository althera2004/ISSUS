using GisoFramework.Activity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Descripción breve de DashBoardActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DashBoardActions : System.Web.Services.WebService
{

    public DashBoardActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SetFilter(bool owners, bool others)
    {
        ActionResult res = ActionResult.NoAction;
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
