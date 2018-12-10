// --------------------------------
// <copyright file="JobPositionActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Implements job position asynchronous actions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class JobPositionActions : WebService
{
    /// <summary>Initializes a new instance of the JobPositionActions class.</summary>
    public JobPositionActions()
    {
    }

    /// <summary>Call the job position insert</summary>
    /// <param name="newJobPosition">New job position data</param>
    /// <param name="userId">Identifier of logged user</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(JobPosition newJobPosition, int userId)
    {
        if(newJobPosition==null)
        {
            return ActionResult.NoAction;
        }

        var res = newJobPosition.Insert(userId);
        if (res.Success)
        {
            // Añadir traza
            res = ActivityLog.JobPosition(newJobPosition.Id, userId, newJobPosition.CompanyId, JobPositionLogActions.Create, string.Empty);
            res.SetSuccess(newJobPosition.Id);
        }

        return res;
    }

    /// <summary>Call the job position update</summary>
    /// <param name="oldJobPosition">Old job position data</param>
    /// <param name="newJobPosition">New job position data</param>
    /// <param name="userId">Identifier of logged user</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(int oldJobPositionId, JobPosition newJobPosition, int userId)
    {
        if(newJobPosition == null)
        {
            return ActionResult.NoAction;
        }

        var oldJobPosition = new JobPosition(oldJobPositionId, newJobPosition.CompanyId);
        var res = ActionResult.NoAction;
        string extraData = JobPosition.Differences(oldJobPosition, newJobPosition);
        if (!string.IsNullOrEmpty(extraData))
        {
            res = newJobPosition.Update(userId);
            if (res.Success)
            {
                res = ActivityLog.JobPosition(newJobPosition.Id, userId, newJobPosition.CompanyId, JobPositionLogActions.Update, extraData);
            }
        }
        else
        {
            res.SetSuccess();
        }

        return res;
    }

    /// <summary>Asynchronous call to delete job position</summary>
    /// <param name="jobPositionId">Job position identifier</param>
    /// <param name="companyId">Company id</param>
    /// <param name="userId">Identifier of user that performs action</param>
    /// <param name="reason">Reason of change</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Unlink(int employeeId, int jobPositionId)
    {
        return JobPosition.Unlink(employeeId, jobPositionId);
    }

    /// <summary>Asynchronous call to delete job position</summary>
    /// <param name="jobPositionId">Job position identifier</param>
    /// <param name="companyId">Company id</param>
    /// <param name="userId">Identifier of user that performs action</param>
    /// <param name="reason">Reason of change</param>
    /// <returns>Result of action</returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int jobPositionId, int companyId, int userId, string reason)
    {
        return JobPosition.Delete(jobPositionId, companyId, userId, reason);
    }
}