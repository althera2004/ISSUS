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

        if(type == 1)
        {
            this.Session["OportunityFilter"] = null;
        }
        else
        {
            this.Session["BusinessRiskFilter"] = null;
        }

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
        var res = oportunity.Update(applicationUserId);
        if(oportunity.FinalDate.HasValue && oportunity.FinalDate.Value.Year < 2000)
        {
            oportunity.FinalDate = null;
        }

        if (res.Success && oportunity.FinalDate.HasValue)
        {
            var newOportunity = new Oportunity
            {
                Code = oportunity.Code,
                CompanyId = oportunity.CompanyId,
                DateStart = oportunity.FinalDate.Value,
                Cost = oportunity.FinalCost,
                Impact = oportunity.FinalImpact,
                Description = oportunity.Description,
                ApplyAction = oportunity.FinalApplyAction.Value,
                Result = oportunity.FinalResult,
                Causes = oportunity.Causes,
                Notes = oportunity.Notes,
                ItemDescription = oportunity.ItemDescription,
                PreviousOportunityId = oportunity.Id,
                Process = oportunity.Process,
                Rule = oportunity.Rule,
                Control = oportunity.Control
            };

            res = newOportunity.Insert(applicationUserId);
            var employee = Employee.ByUserId(applicationUserId);
            var actualAction = IncidentAction.ByOportunityId(oportunity.Id, oportunity.CompanyId);
            var newAction = new IncidentAction
            {
                Oportunity = newOportunity,
                Origin = actualAction.Origin,
                ReporterType = actualAction.ReporterType,
                WhatHappened = actualAction.WhatHappened,
                WhatHappenedBy = employee,
                WhatHappenedOn = newOportunity.DateStart,
                Causes = actualAction.Causes,
                CausesBy = employee,
                CausesOn = newOportunity.DateStart,
                ActionType = actualAction.ActionType,
                Description = actualAction.Description,
                CompanyId = actualAction.CompanyId,
                Provider = actualAction.Provider,
                Customer = actualAction.Customer,
                Department = actualAction.Department
            };

            res = newAction.Insert(applicationUserId);
        }

        return res;
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
    public string GetFilter(int companyId, DateTime? from, DateTime? to, long rulesId, long processId, int itemType)
    {
        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(", ");
        filter.Append(Tools.JsonPair("from", from)).Append(", ");
        filter.Append(Tools.JsonPair("to", to)).Append(", ");
        filter.Append(Tools.JsonPair("rulesId", rulesId)).Append(", ");
        filter.Append(Tools.JsonPair("itemType", itemType)).Append(", ");
        filter.Append(Tools.JsonPair("processId", processId)).Append("}");
        this.Session["OportunityFilter"] = filter.ToString();
        this.Session["BusinessRiskFilter"] = null;
        return Oportunity.FilterList(companyId, from, to, rulesId, processId);
    }
}