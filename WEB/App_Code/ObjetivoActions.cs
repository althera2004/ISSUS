// --------------------------------
// <copyright file="ObjetivoActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;
using System.Text;
using GisoFramework;
using System;

/// <summary>Implements asynchronous actions for objetivo</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ObjetivoActions : WebService {

    /// <summary>Initializes a new instance of the ObjetivoActions class</summary>
    public ObjetivoActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, DateTime? from, DateTime? to, int status)
    {
        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
        filter.Append(Tools.JsonPair("from", from)).Append(",");
        filter.Append(Tools.JsonPair("to", to)).Append(",");
        filter.Append(Tools.JsonPair("status", status)).Append("}");
        Session["ObjetivoFilter"] = filter.ToString();
        return Objetivo.FilterList(companyId, from, to, status);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Save(Objetivo objetivo, int applicationUserId)
    {
        return objetivo.Save(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Anulate(int objetivoId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
    {
        return Objetivo.Anulate(objetivoId, companyId, applicationUserId, reason, date, responsible);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Restore(int objetivoId, int companyId, int applicationUserId)
    {
        return Objetivo.Restore(objetivoId, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Activate(int objetivoId, int companyId, int applicationUserId)
    {
        return Objetivo.Activate(objetivoId, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Inactivate(int objetivoId, int companyId, int applicationUserId)
    {
        return Objetivo.Inactivate(objetivoId, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ObjetivoRegistroSave(ObjetivoRegistro registro, int applicationUserId)
    {
        return registro.Save(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ObjetivoIndicadorRegistroSave(IndicadorRegistro registro, int applicationUserId)
    {
        return registro.Save(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DeleteRegistro(int registroId, int companyId, int applicationUserId)
    {
        return ObjetivoRegistro.Inactivate(registroId, companyId, applicationUserId);
    }    
}