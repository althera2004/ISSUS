// --------------------------------
// <copyright file="QuestionaryActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Asynchronous actions for "questionary" item</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class QuestionaryActions : WebService
{

    public QuestionaryActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Questionary questionary, int userId)
    {
        return questionary.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Questionary questionary, Questionary oldQuestionary, int userId)
    {
        string differences = questionary.Differences(oldQuestionary);
        return questionary.Update(userId, differences);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long questionarioId, int userId, int companyId)
    {
        return Questionary.Inactivate(questionarioId, companyId, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(int companyId, long processId, long ruleId, string apartadoNorma)
    {
        if(apartadoNorma == "-1")
        {
            apartadoNorma = string.Empty;
        }

        var filter = new StringBuilder("{");
        filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
        filter.Append(Tools.JsonPair("processId", processId)).Append(",");
        filter.Append(Tools.JsonPair("ruleId", ruleId)).Append(",");
        filter.Append(Tools.JsonPair("apartadoNorma", apartadoNorma)).Append("}");
        Session["QuestionaryFilter"] = filter.ToString();
        return Questionary.FilterList(companyId, processId, ruleId, apartadoNorma);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Duplicate(long questionaryId, string description, int companyId, int userId)
    {
        return Questionary.Duplicate(questionaryId, description, companyId, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DeleteQuestion(long questionId, int userId, int companyId)
    {
        return QuestionaryQuestion.Inactivate(questionId, companyId, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult UpdateQuestion(long questionaryId, long questionId, string question, int userId, int companyId)
    {
        var questionToUpdate = new QuestionaryQuestion
        {
            Id = questionId,
            Description = question,
            QuestionaryId = questionaryId,
            CompanyId = companyId
        };
        return questionToUpdate.Update(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult InsertQuestion(long questionaryId, long questionId, string question, int userId, int companyId)
    {
        var questionToUpdate = new QuestionaryQuestion
        {
            Id = questionId,
            Description = question,
            QuestionaryId = questionaryId,
            CompanyId = companyId
        };
        return questionToUpdate.Insert(userId);
    }
}