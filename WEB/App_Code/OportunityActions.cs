using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Descripción breve de OportunityActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class OportunityActions : WebService {

    public OportunityActions () {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Anulate (int oportunityId, int companyId, DateTime anulateDate, int anulateBy, string anulateReason, int applicationUserId)
    {
        return Oportunity.Anulate(oportunityId, companyId, anulateBy, anulateDate, anulateReason, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SetLayout(int type)
    {
        Session["BusinnessListLayout"] = type;
        var res = ActionResult.NoAction;
        res.SetSuccess();
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Activate(int oportunityId, int companyId, int applicationUserId)
    {
        return Oportunity.Activate(oportunityId, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Inactivate(int oportunityId, int companyId, int applicationUserId)
    {
        return Oportunity.Inactivate(oportunityId, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Oportunity oportunity, int applicationUserId)
    {
        return oportunity.Insert(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Oportunity oportunity, int applicationUserId)
    {
        return oportunity.Update(applicationUserId);
    }

    /// <summary>Obtain oportunities by filter</summary>
    /// <param name="companyId">Company identifier</param>
    /// <param name="from">Date from</param>
    /// <param name="to">Date to</param>
    /// <param name="rulesId">Rules identifier</param>
    /// <param name="processId">Process identifier</param>
    /// <returns>Risk compliant by filter</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, DateTime? from, DateTime? to, long rulesId, long processId)
    {
        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(", ");
        filter.Append(Tools.JsonPair("from", from)).Append(", ");
        filter.Append(Tools.JsonPair("to", to)).Append(", ");
        filter.Append(Tools.JsonPair("rulesId", rulesId)).Append(", ");
        filter.Append(Tools.JsonPair("processId", processId)).Append("}");
        this.Session["OportunityFilter"] = filter.ToString();
        return Oportunity.FilterList(companyId, from, to, rulesId, processId);
    }
}