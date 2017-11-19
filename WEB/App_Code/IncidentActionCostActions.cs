// --------------------------------
// <copyright file="IncidentActionCostActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for IncidentActionCostActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class IncidentActionCostActions : WebService {

    public IncidentActionCostActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(IncidentActionCost incidentActionCost, int userId)
    {
        return incidentActionCost.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(IncidentActionCost newIncidentActionCost, IncidentActionCost oldIncidentActionCost, int userId)
    {
        string differences = IncidentActionCost.Differences(oldIncidentActionCost, newIncidentActionCost);
        return newIncidentActionCost.Update(userId, differences);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long incidentId, int userId, int companyId)
    {
        return IncidentActionCost.Delete(incidentId, userId, companyId);
    }
}

