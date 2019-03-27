using System;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for IncidentActionsActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class IndicadorActions : WebService {

    /// <summary>Initializes a new instance of the IncidentActionActions class</summary>
    public IndicadorActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, int indicatorType, DateTime? from, DateTime? to, int? processId, int? processTypeId, int? targetId, int status)
    {
        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
        filter.Append(Tools.JsonPair("indicatorType", indicatorType)).Append(",");
        filter.Append(Tools.JsonPair("from", from)).Append(",");
        filter.Append(Tools.JsonPair("to", to)).Append(",");
        filter.Append(Tools.JsonPair("process", processId)).Append(",");
        filter.Append(Tools.JsonPair("processType", processTypeId)).Append(",");
        filter.Append(Tools.JsonPair("objetivo", targetId)).Append(",");
        filter.Append(Tools.JsonPair("status", status)).Append("}");
        Session["IndicadorFilter"] = filter.ToString();
        return Indicador.FilterList(companyId, indicatorType, from, to, processId, processTypeId, targetId, status);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult InactivateIndicator(int companyId, int indicatorId, int userId)
    {
        return Indicador.Inactivate(indicatorId, companyId, userId);
    }

    /// <summary>Asynchronous function to restore "indicador"</summary>
    /// <param name="indicadorId">Indicador identifier</param>
    /// <param name="companyId">Company iientifier</param>
    /// <param name="applicationUserId">Identifier of user that performs action</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Restore(int indicadorId, int companyId, int applicationUserId)
    {
        return Indicador.Restore(indicadorId, companyId, applicationUserId);
    }

    /// <summary>Asynchronous function to anulate "indicador"</summary>
    /// <param name="indicadorId">Indicador identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="applicationUserId">Identifier of user that performs action</param>
    /// <param name="date">Date of anulation</param>
    /// <param name="reason">Reason for anulation</param>
    /// <param name="responsible">Responsible of anulation</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Anulate(int indicadorId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
    {
        return Indicador.Anulate(indicadorId, companyId, applicationUserId, reason, date, responsible);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Save(Indicador indicador, int applicationUserId)
    {
        return indicador.Save(applicationUserId);
    }

    /*"Id": selectedRecordId,
            "CompanyId": Indicador.CompanyId,
            "Indicador": { "Id": Indicador.Id },
            "Value": StringToNumber($("#TxtRegistroValue").val(), ".", ","),
            "Date": GetDate($("#TxtRecordDate").val(), "/", false),
            "Comments": $("#TxtRegistroComments").val(),
            "MetaComparer": Indicador.MetaComparer,
            "Meta": Indicador.Meta,
            "AlarmaComparer": Indicador.AlarmaComparer,
            "Alarma": Indicador.Alarma,
            "Responsible": { "Id": $("#CmbResponsibleRecord").val() * 1 },
        "applicationUserId": ApplicationUser.Id*/
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SaveRegistro(long Id, long Responsible,decimal Meta, string MetaComparer,int CompanyId, long Indicador, decimal Value, DateTime Date,string AlarmaComparer, string Comments,decimal? Alarma, int applicationUserId)
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
        return IndicadorRegistro.Inactivate(registroId, companyId, applicationUserId);
    }
}
 