﻿// --------------------------------
// <copyright file="EmployeeActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item;

/// <summary>
/// Summary description for EmployeeActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EmployeeActions : WebService {

    public EmployeeActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult GrantChanged(int userTargetId, int companyId, int securityGroup, bool grant, int userId)
    {
        ActionResult res = ActionResult.NoAction;

        if (grant)
        {
            res = ApplicationUser.SetGrant(userTargetId, companyId, securityGroup, userId);
        }
        else
        {
            res = ApplicationUser.RevokeGrant(userTargetId, companyId, securityGroup, userId);
        }

        if (res.Success)
        {
            res.SetSuccess(securityGroup.ToString() + "|" + grant.ToString().ToLowerInvariant());
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult EmployeeSkillsInsert(EmployeeSkills skills, int userId)
    {
        return skills.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult EmployeeSkillsUpdate(EmployeeSkills oldSkills, EmployeeSkills newSkills, int userId)
    {
        ActionResult res = ActionResult.NoAction;
        //string extradata = EmployeeSkills.Differences(oldSkills, newSkills);
        res = newSkills.Update(userId);

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Restore(int employeeId, int companyId, int userId)
    {
        ActionResult res = Employee.Restore(employeeId, companyId, userId);
        if(res.Success)
        {
            Company companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Disable(int employeeId, int companyId, int userId, DateTime endDate)
    {
        ActionResult res = Employee.Disable(employeeId, companyId, userId, endDate); ;
        if (res.Success)
        {
            Company companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult AsociateNewDepartment(int companyId, int employeeId, string departmentName, int userId)
    {
        ActionResult result = new ActionResult() { Success = false, MessageError = "No action" };
        Department department = new Department() { Id = -1, CompanyId = companyId, Description = departmentName };
        result = ActivityLog.Department(department.Id, 1, companyId, DepartmentLogActions.Create, string.Empty);
        result = department.Insert(userId);
        if (result.Success)
        {
            int departmentId = Convert.ToInt32(result.MessageError);
            result = Employee.AssociateToDepartment(companyId, employeeId, departmentId);
            if (result.Success)
            {

                result = ActivityLog.Employee(employeeId, 1, companyId, EmployeeLogActions.AssociateToDepartment, string.Empty);
            }

            result.MessageError = departmentId.ToString().Trim();
        }

        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult AssociateDepartment(int companyId, int employeeId, int departmentId)
    {
        ActionResult res = Employee.AssociateToDepartment(companyId, employeeId, departmentId);
        if (res.Success)
        {
            res = ActivityLog.Employee(employeeId, 1, companyId, EmployeeLogActions.AssociateToDepartment, string.Empty);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DesassociateDepartment(int employeeId, int companyId, int departmentId)
    {
        ActionResult res = Employee.DisassociateToDepartment(companyId, employeeId, departmentId);
        if (res.Success)
        {
            res = ActivityLog.Employee(employeeId, 1, companyId, EmployeeLogActions.DisassociateDepartment, string.Empty);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult AssociateJobPosition(int companyId, int employeeId, int jobPositionId, DateTime date, int userId)
    {
        //return Employee.AssignateJobPosition(employeeId, Convert.ToInt64(jobPositionId), companyId, userId);
        ActionResult res = ActionResult.NoAction;
        /* CREATE PROCEDURE Employee_AsignateJobPosition
         * @EmployeeId int,
         * @JobPosition bigint,
         * @CompanyId int,
         * @UserId int */
        using (SqlCommand cmd = new SqlCommand("Employee_AsignateJobPosition"))
        {
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                cmd.Parameters.Add(DataParameter.Input("@JobPositionId", jobPositionId));
                cmd.Parameters.Add(DataParameter.Input("@Date", date));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                res.SetSuccess();
            }
            catch (SqlException ex)
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

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DesassociateJobPosition(int companyId, int employeeId, int jobPositionId, DateTime date, int userId)
    {
        //return Employee.UnassignateJobPosition(employeeId, Convert.ToInt64(jobPositionId), companyId, userId);
        ActionResult res = ActionResult.NoAction;
        using (SqlCommand cmd = new SqlCommand("Employee_UnasignateJobPosition"))
        {
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                cmd.Parameters.Add(DataParameter.Input("@JobPositionId", jobPositionId));
                cmd.Parameters.Add(DataParameter.Input("@Date", date));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                res.SetSuccess();
            }
            catch (SqlException ex)
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

        return res;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult DeleteJobPosition(int employeeId, int jobPositionId)
    {
        return Employee.DeleteJobPosition(employeeId, jobPositionId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Employee newEmployee, int userId)
    {
        return newEmployee.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Employee oldEmployee, Employee newEmployee, int userId)
    {
        ActionResult res = ActionResult.NoAction;
        string extraData = newEmployee.Differences(oldEmployee);

        res = newEmployee.Update(userId);
        if (res.Success)
        {
            res = ActivityLog.Employee(newEmployee.Id, userId, newEmployee.CompanyId, EmployeeLogActions.Modify, extraData);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult EmployeeDelete(int employeeId, int companyId, string reason, int userId)
    {
        return Employee.Delete(employeeId, reason, companyId, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult UserDelete(int userItemId, int companyId, string reason, int userId)
    {
        return ApplicationUser.Delete(userItemId, companyId, userId);
    }

    [WebMethod(EnableSession=true)]
    [ScriptMethod]
    public ActionResult Substitute(string data, int userId, int companyId, int actualEmployee, int newEmployee)
    {
        ActionResult res = ActionResult.NoAction;
        string[] actions = data.Split('|');

        using (SqlCommand cmd = new SqlCommand())
        {
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(DataParameter.Input("@CompanyId", Convert.ToInt32(companyId)));
            cmd.Parameters.Add(DataParameter.Input("@UserId", Convert.ToInt32(userId)));
            cmd.Parameters.Add(DataParameter.Input("@NewEmployee", Convert.ToInt64("0")));
            cmd.Parameters.Add(DataParameter.Input("@ItemId", Convert.ToInt64("0")));
            try
            {
                cmd.Connection.Open();
                string procedure = string.Empty;
                foreach (string action in actions)
                {
                    if (!string.IsNullOrEmpty(action))
                    {
                        string item = action.Split('-')[0];
                        string itemId = action.Split('-')[1];
                        string newEmployeeId = newEmployee.ToString();

                        switch (item)
                        {
                            case "E": procedure = "Equipment_SubtituteEmployee"; break;
                            case "ECDE":
                            case "ECDI": procedure = "EquipmentCalibrationDefinition_SubtituteEmployee"; break;
                            case "EVDE":
                            case "EVDI": procedure = "EquipmentVerificationDefinition_SubtituteEmployee"; break;
                            case "EMDE":
                            case "EMDI": procedure = "EquipmentMaintenanceDefinition_SubtituteEmployee"; break;
                            case "IAE": procedure = "IncidenAction_SusbtituteExecutor"; break;
                        }

                        if (!string.IsNullOrEmpty(procedure))
                        {
                            cmd.CommandText = procedure;
                            cmd.Parameters["@NewEmployee"].Value = Convert.ToInt64(newEmployeeId);
                            cmd.Parameters["@ItemId"].Value = Convert.ToInt64(itemId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                //// res = Employee.Delete(Convert.ToInt32(actualEmployee), string.Empty, Convert.ToInt32(companyId), Convert.ToInt32(userId));
                res = Employee.Disable(actualEmployee, companyId, userId, DateTime.Now);
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

        return res;
    }
}
