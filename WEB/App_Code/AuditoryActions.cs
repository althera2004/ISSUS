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
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
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

        // Cuando pasa a planificada hay que generar los cuestionarios y enviar los mails
        if (res.Success && toPlanned && auditory.Type != 1)
        {
            // Generar custionarios
            var resPlanned = Auditory.SetQuestionaries(auditory.Id, applicationUserId);
            if (!resPlanned.Success)
            {
                res.SetFail(resPlanned.MessageError);
            }

            // enviar mails
            var planning = AuditoryPlanning.ByAuditory(auditory.Id, auditory.CompanyId);
            if(auditory.Type == 2)
            {
                foreach(var pl in planning.Where(p=>p.SendMail == true))
                {
                    SendPanningMail(pl, auditory.Description);
                }
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
    public ActionResult Close(long auditoryId, long closedBy,DateTime questionaryStart, DateTime questionaryEnd, DateTime closedOn, int applicationUserId, int companyId, string notes, string puntosFuertes)
    {
        return Auditory.Close(auditoryId, questionaryStart, questionaryEnd, closedBy, closedOn, applicationUserId, companyId, notes, puntosFuertes);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Reopen(long auditoryId, int applicationUserId, int companyId)
    {
        return Auditory.Reopen(auditoryId, applicationUserId, companyId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult CloseCuestionarios(long auditoryId, int applicationUserId, DateTime questionaryStart, DateTime questionaryEnd, string puntosFuertes, string notes, int companyId)
    {
        return Auditory.CloseCuestionarios(auditoryId, applicationUserId, companyId, questionaryStart, questionaryEnd, puntosFuertes, notes);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ReopenCuestionarios(long auditoryId, int applicationUserId, int companyId, string puntosFuertes, string notes)
    {
        return Auditory.ReopenCuestionarios(auditoryId, applicationUserId, companyId, puntosFuertes, notes);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Validate(long auditoryId, long validatedBy, DateTime validatedOn, int applicationUserId, int companyId, string notes, string puntosFuertes)
    {
        return Auditory.Validate(auditoryId, validatedBy, validatedOn, applicationUserId, companyId, notes, puntosFuertes);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ValidateExternal(long auditoryId, DateTime questionaryStart, DateTime questionaryEnd, long validatedBy, DateTime validatedOn, int applicationUserId, int companyId, string notes, string puntosFuertes)
    {
        return Auditory.ValidateExternal(auditoryId,validatedBy,questionaryStart, questionaryEnd, validatedOn, applicationUserId, companyId, notes, puntosFuertes);
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
    public ActionResult FoundToggle(long foundId, int actualStatus)
    {
        var res = ActionResult.NoAction;
        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"UPDATE AuditoryCuestionarioFound SET Action = {1} WHERE Id = {0}",
            foundId,
            actualStatus == 1 ? 0 : 1);

        using(var cmd = new SqlCommand(query))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(foundId.ToString() + "|" + (actualStatus == 1 ? "0" : "1"));
                }
                catch(Exception ex)
                {
                    res.SetFail(ex);
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

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult ImprovementToggle(long improvementId, int actualStatus)
    {
        var res = ActionResult.NoAction;
        var query = string.Format(
            CultureInfo.InvariantCulture,
            @"UPDATE AuditoryCuestionarioImprovement SET Action = {1} WHERE Id = {0}",
            improvementId,
            actualStatus == 1 ? 0 : 1);

        using (var cmd = new SqlCommand(query))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(improvementId.ToString() + "|" + (actualStatus == 1 ? "0" : "1"));
                }
                catch (Exception ex)
                {
                    res.SetFail(ex);
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
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

    private ActionResult SendPanningMail(AuditoryPlanning planning, string auditoryName)
    {
        var res = ActionResult.NoAction;

        var dictionary = ApplicationDictionary.Load("ca");
        string path = HttpContext.Current.Request.PhysicalApplicationPath;
        string destino = path;
        if (!path.EndsWith("\\", StringComparison.Ordinal))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\noimage.jpg", path);
        }
        else
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\noimage.jpg", path);
        }

        if (!destino.EndsWith("\\", StringComparison.Ordinal))
        {
            destino = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\{1}.jpg", destino, res.MessageError.Split('|')[0]);
        }
        else
        {
            destino = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\{1}.jpg", destino, res.MessageError.Split('|')[0]);
        }

        //System.IO.File.Copy(path, destino);

        path = HttpContext.Current.Request.PhysicalApplicationPath;
        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\", path);
        }

        var templateName = "InvitationAuditProv.tpl";
        var user = Session["User"] as ApplicationUser;
        if (!string.IsNullOrEmpty(user.Language))
        {
            templateName = "InvitationAuditProv_" + user.Language + ".tpl";
        }

        path = string.Format(CultureInfo.InvariantCulture, @"{0}Templates\{1}", path, templateName);
        string bodyPattern = string.Empty;
        using (var rdr = new StreamReader(path))
        {
            bodyPattern = rdr.ReadToEnd();
            bodyPattern = bodyPattern.Replace("#AUDITAT#", "{0}");
            bodyPattern = bodyPattern.Replace("#EMPRESA#", "{1}");
            bodyPattern = bodyPattern.Replace("#AUDIT#", "{2}");
            bodyPattern = bodyPattern.Replace("#PLANIFICADA#", "{3:dd/MM/yyyy}");
            bodyPattern = bodyPattern.Replace("#HORA#", "{4}");
            bodyPattern = bodyPattern.Replace("#DURADA#", "{5}");
        }

        var company = HttpContext.Current.Session["company"] as Company;

        var hora = planning.Hour;
        var horarioText = string.Empty;
        var horas = 0;
        while(hora > 59)
        {
            hora -= 60;
            horas++;
        }

        horarioText = string.Format(CultureInfo.InvariantCulture, "{0:#0}:{1:00}", horas, hora);

        string subject = string.Format(dictionary["Mail_Message_InivitationAuditProv"], res.MessageError.Split('|')[0]);
        string body = string.Format(
            CultureInfo.InvariantCulture,
            bodyPattern,
            planning.ProviderName,
            company.Name,
            auditoryName,
            planning.Date,
            horarioText,
            planning.Duration);


        var mail = new MailMessage
        {
            From = new MailAddress("issus@scrambotika.com", "ISSUS"),
            IsBodyHtml = true,
            Subject = subject,
            Body = body
        };

        mail.To.Add(planning.ProviderEmail);
        //mail.To.Add("hola@scrambotika.com");
        mail.Bcc.Add("jcastilla@openframework.es");

        var smtpServer = new SmtpClient("smtp.scrambotika.com")
        {
            Port = 587,
            Credentials = new System.Net.NetworkCredential("issus@scrambotika.com", "wtzAsmjENShJU457KkuK")
        };
        smtpServer.Send(mail);

        return res;
    }
}