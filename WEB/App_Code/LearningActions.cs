﻿// --------------------------------
// <copyright file="LearningActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Globalization;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for LearningActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class LearningActions : WebService {

    public struct AssistantData
    {
        public int AssistantId { get; set; }
        public int LearningId { get; set; }
        public int EmployeeId { get; set; }
    }

    /// <summary>Initializes a new instance of the LearningActions class</summary>
    public LearningActions ()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Learning oldLearning, Learning newLearning, AssistantData[] newAssistants, int userId, int companyId)
    {
        var res = ActionResult.NoAction;
        string extradata = Learning.Differences(oldLearning, newLearning);
        if (!string.IsNullOrEmpty(extradata))
        {
            // Si se informa la fecha de finalización el estado pasa ser "realizado"
            if (newLearning.RealFinish.HasValue)
            {
                newLearning.Status = 1;
            }

            res = newLearning.Insert(userId);
            if (res.Success)
            {
                ActivityLog.Learning(newLearning.Id, userId, companyId, LearningLogActions.Create, extradata);
            }

            foreach (var assistant in newAssistants)
            {
                var newAssistant = new Assistant
                {
                    CompanyId = companyId,
                    Completed = null,
                    Success = null,
                    Learning = new Learning { Id = newLearning.Id },
                    Employee = new Employee(assistant.EmployeeId, true)
                };

                if (newAssistant.Employee.JobPosition != null)
                {
                    newAssistant.JobPosition = newAssistant.Employee.JobPosition;
                }

                newAssistant.Insert(userId);
            }

            res.SetSuccess(newLearning.Id);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SetFilter(string filter)
    {
        HttpContext.Current.Session["LearningFilter"] = filter;
        var res = ActionResult.NoAction;
        res.SetSuccess("ok");
        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Learning oldLearning, Learning newLearning, int[] newAssistants, int userId, int companyId)
    {
        var res = ActionResult.NoAction;
        string extradata = Learning.Differences(oldLearning, newLearning);
        if (!string.IsNullOrEmpty(extradata))
        {
            res = newLearning.Update(userId);
            if (res.Success)
            {
                ActivityLog.Learning(newLearning.Id, userId, companyId, LearningLogActions.Modified, extradata);
                res.SetSuccess(newLearning.Id.ToString());
            }
        }

        foreach (int assistantId in newAssistants)
        {
            var newAssistant = new Assistant
            {
                CompanyId = companyId,
                Completed = null,
                Success = null,
                Learning = new Learning { Id = newLearning.Id },
                Employee = new Employee(assistantId, true)
            };

            newAssistant.JobPosition = newAssistant.Employee.JobPosition;
            newAssistant.Insert(userId);
        }

        if (res.MessageError == ActionResult.NoAction.MessageError)
        {
            res.SetSuccess();
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int learningId, int companyId, int userId, string reason)
    {
        return Learning.Delete(learningId, companyId, userId, reason);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult InsertAssistant(int employeeId, int learningId, int companyId, int userId)
    {
        return new Assistant
        {
            CompanyId = companyId,
            Employee = new Employee { Id = employeeId },
            Learning = new Learning { Id = learningId }
        }.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DeleteAssistant(int assistantId, int learningId, int companyId, int userId)
    {
        return Assistant.Delete(assistantId, learningId, companyId, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Complete(int companyId, AssistantData[] assistants, int userId)
    {
        var res = ActionResult.NoAction;
        foreach (var assitant in assistants)
        {
            res = Assistant.Complete(assitant.AssistantId, companyId, userId);
            if (!res.Success)
            {
                break;
            }
        }

        if (res.Success)
        {
            string message = "[";
            bool first = true;
            foreach (var data in assistants)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    message += ",";
                }

                message += string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{AssistantId:{0},EmployeeId:{1}}}", data.AssistantId, data.EmployeeId);
            }

            message += "]";
            res.SetSuccess(message);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult CompleteFail(int companyId, AssistantData[] assistants, int userId)
    {
        var res = ActionResult.NoAction;
        foreach (var assitant in assistants)
        {
            res = Assistant.CompleteFail(assitant.AssistantId, companyId, userId);
            if (!res.Success)
            {
                break;
            }
        }

        if (res.Success)
        {
            string message = "[";
            bool first = true;
            foreach (var data in assistants)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    message += ",";
                }

                message += string.Format(CultureInfo.InvariantCulture, @"{{AssistantId:{0},EmployeeId:{1}}}", data.AssistantId, data.EmployeeId);
            }

            message += "]";
            res.SetSuccess(message);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Unevaluated(int companyId, AssistantData[] assistants, int userId)
    {
        var res = ActionResult.NoAction;
        foreach (var assitant in assistants)
        {
            res = Assistant.Unevaluated(assitant.AssistantId, companyId, userId);
            if (!res.Success)
            {
                break;
            }
        }

        if (res.Success)
        {
            string message = "[";
            bool first = true;
            foreach (var data in assistants)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    message += ",";
                }

                message += string.Format(CultureInfo.InvariantCulture, @"{{AssistantId:{0},EmployeeId:{1}}}", data.AssistantId, data.EmployeeId);
            }

            message += "]";
            res.SetSuccess(message);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Success(int companyId, AssistantData[] assistants, int userId)
    {
        var res = ActionResult.NoAction;
        foreach (var assitant in assistants)
        {
            res = Assistant.CompleteAndSuccess(assitant.AssistantId, companyId, userId);
            if (!res.Success)
            {
                break;
            }
        }

        if (res.Success)
        {
            string message = "[";
            bool first = true;
            foreach (var data in assistants)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    message += ",";
                }

                message += string.Format(@"{{AssistantId:{0},EmployeeId:{1}}}", data.AssistantId, data.EmployeeId);
            }

            message += "]";
            res.SetSuccess(message);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SuccessFail(int companyId, AssistantData[] assistants, int userId)
    {
        var res = ActionResult.NoAction;
        foreach (var assitant in assistants)
        {
            res = Assistant.CompleteAndSuccessFail(assitant.AssistantId, companyId, userId);
            if (!res.Success)
            {
                break;
            }
        }

        if (res.Success)
        {
            string message = "[";
            bool first = true;
            foreach (var data in assistants)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    message += ",";
                }

                message += string.Format(CultureInfo.InvariantCulture, @"{{AssistantId:{0},EmployeeId:{1}}}", data.AssistantId, data.EmployeeId);
            }

            message += "]";
            res.SetSuccess(message);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Reset(int companyId, int assistantId, int userId, int completedCode, int successCode)
    {
        bool? completed = null;
        bool? success = null;
        if (completedCode == 2) { completed = false; }
        if (completedCode == 1) { completed = true; }
        if (successCode == 2) { success = false; }
        if (successCode == 1) { success = true; }
        var res = Assistant.SetStatus(assistantId, companyId, userId, completed, success);
        if (res.Success)
        {
            res.SetSuccess(string.Format(@"{{""AssistantId"":{0}, ""Completed"":{1}, ""Success"":{2}}}", assistantId, completedCode, successCode));
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DeleteAssistants(AssistantData[] assistants, int userId, int companyId)
    {
        var res = ActionResult.NoAction;
        foreach(var assistant in assistants)
        {
            res = Assistant.Delete(assistant.AssistantId, assistant.LearningId, companyId, userId);
        }

        return res;
    }
}