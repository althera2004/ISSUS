// --------------------------------
// <copyright file="RulesActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Item;
using GisoFramework.Activity;

/// <summary>Webservice to receive the AJAX queries for Rules</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class RulesActions : WebService
{
    public RulesActions ()
    {
    }

    /// <summary>Deactivate object in database</summary>
    /// <param name="rulesId">Object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesDelete(int rulesId, int companyId, int userId)
    {
        var res = Rules.Delete(rulesId, string.Empty, companyId, userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }    

    /// <summary>Activate item in database</summary>
    /// <param name="rulesId">Object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesActivate(int rulesId, int companyId, int userId)
    {
        var res = Rules.Activate(rulesId, string.Empty, companyId, userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    /// <summary>Update item in database</summary>
    /// <param name="newRules">New object identifier</param>
    /// <param name="oldRules">Old object identifier</param>
    /// <param name="reason">Reason of changes</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesUpdate(Rules newRules, Rules oldRules,string reason, int companyId, int userId)
    {
        var res = newRules.Update(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        if(newRules.Limit != oldRules.Limit)
        {
            var history = new RuleHistory
            {
                RuleId = newRules.Id,
                Active = true,
                CompanyId = newRules.CompanyId,
                Reason = reason,
                IPR = Convert.ToInt32(newRules.Limit)
            };

            history.Insert(userId);
        }

        return res;
    }

    /// <summary>Insert new item in database</summary>
    /// <param name="rules">Object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesInsert(Rules rules, int companyId, int userId)
    {
        var res = rules.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        var history = new RuleHistory
        {
            Active = true,
            CompanyId = rules.CompanyId,
            Reason = "Insert",
            IPR = Convert.ToInt32(rules.Limit)
        };

        history.Insert(userId);

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SetLimit(Rules rules, int companyId, int userId)
    {
        var res = rules.SetLimit(userId);
        if (res.Success)
        {
            //string differences = oldRules.Differences(newRules);
            //res = ActivityLog.Department(Convert.ToInt32(RulesId), userId, companyId, DepartmentLogActions.Modify, differences);
            Session["Company"] = new Company(companyId);
        }

        return res;
    }
}