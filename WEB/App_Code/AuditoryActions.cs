// --------------------------------
// <copyright file="AuditoryActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item;

/// <summary>Asynchronous actions for "auditory" item</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class AuditoryActions : WebService
{
    public AuditoryActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SaveZombie(IncidentActionZombie zombie)
    {
        if(zombie.Id > 0)
        {
            return zombie.Update();
        }

        return zombie.Insert(); 
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Auditory auditory, bool toPlanned, string rules, int applicationUserId)
    {
        foreach(var ruleId in rules.Split('|'))
        {
            if (!string.IsNullOrEmpty(ruleId))
            {
                auditory.AddRule(Convert.ToInt64(ruleId));
            }
        }

        var res = auditory.Insert(applicationUserId, auditory.CompanyId);
        if(res.Success && toPlanned && auditory.Type != 1)
        {
            var resPlanned = Auditory.SetQuestionaries(auditory.Id, applicationUserId);
            if (!resPlanned.Success)
            {
                res.SetFail(resPlanned.MessageError);
            }
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Auditory auditory, bool toPlanned, string rules, Auditory oldAuditory, int applicationUserId)
    {
        foreach (var ruleId in rules.Split('|'))
        {
            if (!string.IsNullOrEmpty(ruleId))
            {
                auditory.AddRule(Convert.ToInt64(ruleId));
            }
        }

        string differences = auditory.Differences(oldAuditory);
        var res = auditory.Update(applicationUserId, auditory.CompanyId, differences);
        if (res.Success && toPlanned && auditory.Type != 1)
        {
            var resPlanned = Auditory.SetQuestionaries(auditory.Id, applicationUserId);
            if (!resPlanned.Success)
            {
                res.SetFail(resPlanned.MessageError);
            }
        }

        if(auditory.Type == 1 && auditory.ReportStart != null)
        {
             /* CREATE PROCEDURE [dbo].[Auditory_SetReportStart]
              *   @AuditoryId bigint,
              *   @CompanyId int,
              *   @ReportStart datetime */
            using(var cmd = new SqlCommand("Auditory_SetReportStart"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@AuditoryId", auditory.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", auditory.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@ReportStart", auditory.ReportStart));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    finally
                    {
                        if(cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Close(long auditoryId, long closedBy,DateTime questionaryStart, DateTime questionaryEnd, DateTime closedOn, int applicationUserId, int companyId)
    {
        return Auditory.Close(auditoryId, questionaryStart, questionaryEnd, closedBy, closedOn, applicationUserId, companyId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Reopen(long auditoryId, int applicationUserId, int companyId)
    {
        return Auditory.Reopen(auditoryId, applicationUserId, companyId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult CloseCuestionarios(long auditoryId, int applicationUserId, DateTime questionaryStart, DateTime questionaryEnd, string puntosFuertes, int companyId)
    {
        return Auditory.CloseCuestionarios(auditoryId, applicationUserId, companyId, questionaryStart, questionaryEnd, puntosFuertes);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ReopenCuestionarios(long auditoryId, int applicationUserId, int companyId, string puntosFuertes)
    {
        return Auditory.ReopenCuestionarios(auditoryId, applicationUserId, companyId, puntosFuertes);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Validate(long auditoryId, long validatedBy, DateTime validatedOn, int applicationUserId, int companyId)
    {
        return Auditory.Validate(auditoryId, validatedBy, validatedOn, applicationUserId, companyId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ValidateExternal(long auditoryId, DateTime questionaryStart, DateTime questionaryEnd, long validatedBy, DateTime validatedOn, int applicationUserId, int companyId)
    {
        return Auditory.ValidateExternal(auditoryId,validatedBy,questionaryStart, questionaryEnd, validatedOn, applicationUserId, companyId);
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

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult QuestionToggle(long questionId, int status)
    {
        var res = ActionResult.NoAction;

        status++;
        if(status == 3)
        {
            status = 0;
        }

        using(var cmd = new SqlCommand("AuditoryCuestionarioPregunta_Toogle"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@PreguntaId", questionId));
                if(status == 0)
                {
                    cmd.Parameters.Add(DataParameter.InputNull("@Compliant"));
                }
                else
                {
                    cmd.Parameters.Add(DataParameter.Input("@Compliant", status == 1));
                }

                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(string.Format(CultureInfo.InvariantCulture, "{0}|{1}", questionId, status));
                }
                catch(Exception ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if(cmd.Connection.State != System.Data.ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult QuestionaryObservationsChange(AuditoryCuestionarioObservations observations, int applicationUserId)
    {
        return observations.Save(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult FoundSave(AuditoryCuestionarioFound found, int applicationUserId)
    {
        if(found.Id > 0)
        {
            return found.Update(applicationUserId);
        }

        return found.Insert(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ImprovementSave(AuditoryCuestionarioImprovement improvement, int applicationUserId)
    {
        if (improvement.Id > 0)
        {
            return improvement.Update(applicationUserId);
        }

        return improvement.Insert(applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult FoundDelete(long id, int companyId, int applicationUserId)
    {
        return AuditoryCuestionarioFound.Inactivate(id, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ImprovementDelete(long id, int companyId, int applicationUserId)
    {
        return AuditoryCuestionarioImprovement.Inactivate(id, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ReportData(long id, int companyId)
    {
        var res = "{\"Cuestionarios\":[";
        res += Auditory.RenderCuestionarios(id, companyId);
        res += "],\"Founds\":";
        res += AuditoryCuestionarioFound.JsonList(AuditoryCuestionarioFound.ByAuditory(id, companyId));
        res += ",\"Improvements\":";
        res += AuditoryCuestionarioImprovement.JsonList(AuditoryCuestionarioImprovement.ByAuditory(id, companyId));
        res += "}";

        return new ActionResult
        {
            Success = true,
            ReturnValue = res,
            MessageError = string.Empty
        };
    }
}