﻿// --------------------------------
// <copyright file="IncidentCostActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for IncidentCostActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class IncidentCostActions : WebService {

    /// <summary>Initializes a new instance of the IncidentCostActions class</summary>
    public IncidentCostActions ()
    { 
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(IncidentCost incidentCost, int userId)
    {
        return incidentCost.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(IncidentCost newIncidentCost, IncidentCost oldIncidentCost, int userId)
    {
        return newIncidentCost.Update(userId, oldIncidentCost.Differences(newIncidentCost));
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long incidentId, int userId, int companyId)
    {
        return IncidentCost.Delete(incidentId, userId, companyId);
    }
}