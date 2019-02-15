// --------------------------------
// <copyright file="AuditoryActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Globalization;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Asynchronous actions for "adutory" item</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class AuditoryActions : WebService
{

    public AuditoryActions()
    {

        //Elimine la marca de comentario de la línea siguiente si utiliza los componentes diseñados 
        //InitializeComponent(); 
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Auditory auditory, int userId)
    {
        return auditory.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Auditory auditory, Auditory oldAuditory, int userId)
    {
        string differences = auditory.Differences(oldAuditory);
        return auditory.Update(userId, differences);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long auditoryId, int userId, int companyId)
    {
        return Auditory.Inactivate(auditoryId, companyId, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, DateTime? from, DateTime? to, bool interna, bool externa, bool provider, bool status0, bool status1, bool status2, bool status3, bool status4, bool status5)
    {
        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
        filter.Append(Tools.JsonPair("from", from)).Append(",");
        filter.Append(Tools.JsonPair("to", to)).Append(",");
        filter.Append(Tools.JsonPair("interna", interna)).Append(",");
        filter.Append(Tools.JsonPair("externa", externa)).Append(",");
        filter.Append(Tools.JsonPair("provider", provider)).Append(",");
        filter.Append(Tools.JsonPair("status0", status0)).Append(",");
        filter.Append(Tools.JsonPair("status1", status1)).Append(",");
        filter.Append(Tools.JsonPair("status2", status2)).Append(",");
        filter.Append(Tools.JsonPair("status3", status3)).Append(",");
        filter.Append(Tools.JsonPair("status4", status4)).Append(",");
        filter.Append(Tools.JsonPair("status5", status5)).Append("}");
        Session["AuditoryFilter"] = filter.ToString();
        return Auditory.FilterList(companyId, from, to, interna, externa, provider, status0, status1, status2, status3, status4, status5);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult PlanningInsert(AuditoryPlanning planning, int applicationUserId)
    {
        return planning.Insert(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult PlanningUpdate(AuditoryPlanning planning, int applicationUserId)
    {
        return planning.Update(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult PlanningDelete(long planningId, int companyId, int applicationUserId)
    {
        return AuditoryPlanning.Inactivate(planningId, companyId, applicationUserId);
    }
}