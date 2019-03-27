// --------------------------------
// <copyright file="DepartmentActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for DeparmentActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DepartmentActions : WebService
{
    public DepartmentActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DepartmentDelete(int companyId, int departmentId)
    {
        var res = Department.Delete(companyId, departmentId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DepartmentUpdate(int companyId, int departmentId, string name, int userId)
    {
        var department = new Department { Id = departmentId, CompanyId = companyId, Description = name };
        var res = department.Update(userId);
        if (res.Success)
        {
            string differences = department.Differences(Department.Empty);
            res = ActivityLog.Department(Convert.ToInt32(departmentId), userId, companyId, DepartmentLogActions.Modify, differences);
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DepartmentInsert(int companyId, string name, int userId)
    {
        var department = new Department() { Id = -1, CompanyId = companyId, Description = name };
        var res = department.Insert(userId);
        if (res.Success)
        {
            string differences = department.Differences(Department.Empty);
            var logRes = ActivityLog.Department(Convert.ToInt64(res.MessageError), userId, companyId, DepartmentLogActions.Create, differences);
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DepartmentDelete(int departmentId, int companyId, int userId)
    {
        var res = Department.Delete(departmentId, string.Empty, companyId, userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }
}