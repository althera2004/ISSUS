// --------------------------------
// <copyright file="ObjetivoActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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

    /* "Id": selectedRecordId,
            "CompanyId": Company.Id,
            "ObjetivoId": ItemData.Id,
            "Value": StringToNumber($("#TxtRegistroValue").val(), ".", ","),
            "Date": GetDate($("#TxtRecordDate").val(), "/", false),
            "Comments": $("#TxtRegistroComments").val(),
            "MetaComparer": metaComparer,
            "Meta": meta,
            "Responsible": $("#CmbResponsibleRecord").val() * 1,
            "applicationUserId": ApplicationUser.Id */
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ObjetivoRegistroSave(long Id, int ObjetivoId, int CompanyId, decimal Value, DateTime Date, string Comments, decimal Meta, string MetaComparer,
        long Responsible, int applicationUserId)
    {
        var registroObjetivo = new ObjetivoRegistro
        {
            Id = Id,
            ObjetivoId = ObjetivoId,
            Meta = Meta,
            MetaComparer = MetaComparer,
            Value = Value,
            Comments = Comments,
            CompanyId = CompanyId,
            Date = Date,
            Responsible = new Employee { Id = Responsible },
            ModifiedBy = new ApplicationUser(applicationUserId),
            ModifiedOn = DateTime.Now
        };
        return registroObjetivo.Save(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ObjetivoIndicadorRegistroSave(long Id, long Responsible, decimal Meta, string MetaComparer, int CompanyId, long Indicador, decimal Value, DateTime Date, string AlarmaComparer, string Comments, decimal? Alarma, int applicationUserId)
    {
        var registro = new IndicadorRegistro
        {
            Active = true,
            Alarma = Alarma,
            AlarmaComparer = AlarmaComparer,
            Comments = Comments,
            CompanyId = CompanyId,
            Date = Date,
            Id = Id,
            Indicador = new Indicador { Id = Indicador },
            Meta = Meta,
            MetaComparer = MetaComparer,
            Responsible = new Employee { Id = Responsible },
            Value = Value
        };

        return registro.Save(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DeleteRegistro(int registroId, int companyId, int applicationUserId)
    {
        return ObjetivoRegistro.Inactivate(registroId, companyId, applicationUserId);
    }    
}
 