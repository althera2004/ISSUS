// --------------------------------
// <copyright file="ProcessActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for ProcessActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ProcessActions : WebService {

    /// <summary>
    /// Initializes a new instance of the ProcessActions class.
    /// </summary>
    public ProcessActions()
    { 
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DesactiveProcessType(int processTypeId, string description, int companyId, int userId)
    {
        var victim = new ProcessType { Id = processTypeId, CompanyId = companyId, Description = description };
        var res = victim.Deactive(userId);
        if (res.Success)
        {
            var companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult UpdateProcessType(int processTypeId, string description, int companyId, int userId)
    {
        var victim = new ProcessType { Id = processTypeId, CompanyId = companyId, Description = description, Active = true };
        var res = victim.Update(userId);
        if (res.Success)
        {
            var companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult InsertProcessType(string description, int companyId, int userId)
    {
        var newProcessType = new ProcessType { CompanyId = companyId, Description = description, Active = true };
        var res = newProcessType.Insert(userId);
        if (res.Success)
        {
            var companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    /// <summary>
    /// Call the process insert
    /// </summary>
    /// <param name="newProcess">New job position data</param>
    /// <param name="userId">Identifier of logged user</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Process newProcess, int userId)
    {
        var res = newProcess.Insert(userId);
        if (res.Success)
        {
            res = ActivityLog.Process(newProcess.Id, userId, newProcess.CompanyId, ProcessLogActions.Create, newProcess.ToString());
            res.SetSuccess(newProcess.Id.ToString());
            var companySession = new Company(newProcess.CompanyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    /// <summary>
    /// Call the process update
    /// </summary>
    /// <param name="oldProcess">Old process data</param>
    /// <param name="newProcess">New process data</param>
    /// <param name="userId">Identifier of logged user</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Process oldProcess, Process newProcess, int userId)
    {
        var res = ActionResult.NoAction;
        string extraData = Process.Differences(oldProcess, newProcess);
        if (!string.IsNullOrEmpty(extraData))
        {
            res = newProcess.Update(userId);
            var companySession = new Company(newProcess.CompanyId);
            HttpContext.Current.Session["Company"] = companySession;

            if (res.Success)
            {
                res = ActivityLog.Process(newProcess.Id, userId, newProcess.CompanyId, ProcessLogActions.Modify, extraData);
            }
        }
        else
        {
            res.SetSuccess();
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DesactiveProcess(int processId, int companyId, int userId)
    {
        var res = Process.Deactive(processId, companyId, userId);
        if (res.Success)
        {
            var companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }
}