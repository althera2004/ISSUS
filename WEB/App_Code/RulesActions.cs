// --------------------------------
// <copyright file="RulesActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Item;
using GisoFramework.Activity;

/// <summary>
/// Webservice to receive the AJAX queries for Rules
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class RulesActions : WebService
{
    public RulesActions () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Deactivate object in database
    /// </summary>
    /// <param name="RulesId">Object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesDelete(int RulesId, int companyId, int userId)
    {
        ActionResult res = Rules.Delete(RulesId, string.Empty, companyId, userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }
    /// <summary>
    /// Activate item in database
    /// </summary>
    /// <param name="RulesId">Object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesActivate(int RulesId, int companyId, int userId)
    {
        ActionResult res = Rules.Activate(RulesId, string.Empty, companyId, userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }
    /// <summary>
    /// Update item in database
    /// </summary>
    /// <param name="newRules">New object identifier</param>
    /// <param name="oldRules">Old object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesUpdate(Rules newRules, Rules oldRules, int companyId, int userId)
    {
        ActionResult res = newRules.Update(userId);
        if (res.Success)
        {
            //string differences = oldRules.Differences(newRules);
            //res = ActivityLog.Department(Convert.ToInt32(RulesId), userId, companyId, DepartmentLogActions.Modify, differences);
            Session["Company"] = new Company(companyId);
        }

        return res;
    }
    /// <summary>
    /// Insert new item in database
    /// </summary>
    /// <param name="Rules">Object identifier</param>
    /// <param name="companyId">Company identifier</param>
    /// <param name="userId">User identifier</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult RulesInsert(Rules Rules, int companyId, int userId)
    {
        ActionResult res = Rules.Insert(userId);
        if (res.Success)
        {
            //string differences = Rules.Differences(Rules.Empty);
            //ActionResult logRes = ActivityLog.Rules(Convert.ToInt64(res.MessageError), userId, companyId, DepartmentLogActions.Create, differences);
            Session["Company"] = new Company(companyId);
        }
        return res;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult SetLimit(Rules rules, int companyId, int userId)
    {
        ActionResult res = rules.SetLimit(userId);
        if (res.Success)
        {
            //string differences = oldRules.Differences(newRules);
            //res = ActivityLog.Department(Convert.ToInt32(RulesId), userId, companyId, DepartmentLogActions.Modify, differences);
            Session["Company"] = new Company(companyId);
        }

        return res;
    }
}
