// --------------------------------
// <copyright file="IncidentActionsActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Implements actions for IncidentActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class IncidentActionsActions : WebService
{
    /// <summary>Initializes a new instance of the IndicentActionActions class</summary>
    public IncidentActionsActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Save(IncidentAction incidentAction, int userId)
    {
        var res = ActionResult.NoAction;
        if (incidentAction.Id < 1)
        {
            res = incidentAction.Insert(userId);
        }
        else
        {
            res = incidentAction.Update(userId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SaveAndCopy(IncidentAction incidentAction, bool applyAction, long businessRiskId, int userId)
    {
        var res = ActionResult.NoAction;
        if (incidentAction.Id < 1)
        {
            res = incidentAction.Insert(userId);
        }
        else
        {
            res = incidentAction.Update(userId);
        }

        if (applyAction)
        {
            var risk = BusinessRisk.GetById(incidentAction.CompanyId, businessRiskId);
            incidentAction.BusinessRiskId = risk.Id;
            incidentAction.WhatHappenedOn = risk.DateStart;
            incidentAction.Causes = risk.Causes;
            incidentAction.ClosedBy = null;
            incidentAction.ClosedExecutor = null;
            incidentAction.ClosedExecutorOn = null;
            incidentAction.ClosedOn = null;
            res = incidentAction.Insert(userId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long incidentActionId, int companyId, int userId)
    {
        return new IncidentAction { Id = incidentActionId, CompanyId = companyId }.Delete(userId);
    }
    
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, DateTime? from, DateTime? to, bool statusIdnetified, bool statusAnalyzed, bool statusInProgress, bool statusClose, bool typeImprovement, bool typeFix, bool typePrevent, int origin, int reporter)
    {
        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
        filter.Append(Tools.JsonPair("from", from)).Append(",");
        filter.Append(Tools.JsonPair("to", to)).Append(",");
        filter.Append(Tools.JsonPair("statusIdnetified", statusIdnetified)).Append(",");
        filter.Append(Tools.JsonPair("statusAnalyzed", statusAnalyzed)).Append(",");
        filter.Append(Tools.JsonPair("statusInProgress", statusInProgress)).Append(",");
        filter.Append(Tools.JsonPair("statusClose", statusClose)).Append(",");
        filter.Append(Tools.JsonPair("typeImprovement", typeImprovement)).Append(",");
        filter.Append(Tools.JsonPair("typeFix", typeFix)).Append(",");
        filter.Append(Tools.JsonPair("typePrevent", typePrevent)).Append(",");
        filter.Append(Tools.JsonPair("reporter", reporter)).Append(",");
        filter.Append(Tools.JsonPair("origin", origin)).Append("}");
        Session["IncidentActionFilter"] = filter.ToString();        
        return IncidentAction.FilterList(companyId, from, to, statusIdnetified, statusAnalyzed, statusInProgress, statusClose, typeImprovement, typeFix, typePrevent, origin, reporter);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Anulate(int incidentActionId, int companyId, int applicationUserId, DateTime date, int responsible)
    {
        return IncidentAction.Anulate(incidentActionId, companyId, applicationUserId, date, responsible);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Restore(int incidentActionId, int companyId, int applicationUserId)
    {
        return IncidentAction.Restore(incidentActionId, companyId, applicationUserId);
    }
}