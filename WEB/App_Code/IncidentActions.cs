// --------------------------------
// <copyright file="IncidentActions.cs" company="Sbrinna">
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

/// <summary>
/// Summary description for IncidentActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class IncidentActions : WebService
{
    public IncidentActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Incident incident, int userId)
    {
        if (incident.Department.Id != 0)
        {
            incident.Origin = 1;
        }
        else if (incident.Provider.Id != 0)
        {
            incident.Origin = 2;
        }
        else if (incident.Customer.Id != 0)
        {
            incident.Origin = 3;
        }

        return incident.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Incident newIncident, Incident oldIncident, int userId)
    {
        string differences = Incident.Differences(oldIncident, newIncident);
        return newIncident.Update(userId, differences);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long incidentId, int userId, int companyId)
    {
        return Incident.Delete(incidentId, userId, companyId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, DateTime? from, DateTime? to, bool statusIdnetified, bool statusAnalyzed, bool statusInProgress, bool statusClose, int origin, int departmentId, int providerId, int customerId)
    {
        StringBuilder filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
        filter.Append(Tools.JsonPair("from", from)).Append(",");
        filter.Append(Tools.JsonPair("to", to)).Append(",");
        filter.Append(Tools.JsonPair("statusIdnetified", statusIdnetified)).Append(",");
        filter.Append(Tools.JsonPair("statusAnalyzed", statusAnalyzed)).Append(",");
        filter.Append(Tools.JsonPair("statusInProgress", statusInProgress)).Append(",");
        filter.Append(Tools.JsonPair("statusClose", statusClose)).Append(",");
        filter.Append(Tools.JsonPair("origin", origin)).Append(",");
        filter.Append(Tools.JsonPair("departmentId", departmentId)).Append(",");
        filter.Append(Tools.JsonPair("providerId", providerId)).Append(",");
        filter.Append(Tools.JsonPair("customerId", customerId)).Append("}");
        Session["IncidentFilter"] = filter.ToString();

        return Incident.FilterList(companyId, from, to, statusIdnetified, statusAnalyzed, statusInProgress, statusClose, origin, departmentId, providerId, customerId);
    }
}
