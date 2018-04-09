// --------------------------------
// <copyright file="BusinessRiskActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// -------------------------------- 
namespace GISOWeb
{
    using System;
    using System.Text;
    using System.Web.Script.Services;
    using System.Web.Services;
    using GisoFramework;
    using GisoFramework.Activity;
    using GisoFramework.Item;

    /// <summary>Webservice to receive the AJAX queries for BusinessRisk</summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class BusinessRiskActions : WebService
    {
        /// <summary>Initializes a new instance of the BusinessRiskActions class.</summary>
        public BusinessRiskActions()
        {
        }

        /// <summary>Deactivate item in database</summary>
        /// <param name="businessRiskId">Object identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult BusinessRiskDelete(int businessRiskId, int companyId, int userId)
        {
            ActionResult res = BusinessRisk.Delete(businessRiskId, string.Empty, companyId, userId);
            if (res.Success)
            {
                this.Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>Activate item in database</summary>
        /// <param name="businessRiskId">Object identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult BusinessRiskActivate(int businessRiskId, int companyId, int userId)
        {
            ActionResult res = BusinessRisk.Activate(businessRiskId, string.Empty, companyId, userId);
            if (res.Success)
            {
                this.Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>Update item in database</summary>
        /// <param name="newBusinessRisk">Risk to update</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult BusinessRiskUpdate(BusinessRisk newBusinessRisk, int companyId, int userId)
        {
            ActionResult res = newBusinessRisk.Update(userId);
            if (res.Success)
            {
                if (newBusinessRisk.FinalDate.HasValue && newBusinessRisk.FinalAction == 3)
                {
                    BusinessRisk newBusinessRiskEvaluated = new BusinessRisk()
                    {
                        Description = newBusinessRisk.Description,
                        Code = newBusinessRisk.Code,
                        DateStart = newBusinessRisk.FinalDate.Value,
                        Notes = newBusinessRisk.Notes,
                        ItemDescription = newBusinessRisk.ItemDescription,
                        Causes = newBusinessRisk.Causes,
                        FinalSeverity = 0,
                        FinalProbability = 0,
                        FinalResult = 0,
                        FinalDate = null,
                        StartProbability = newBusinessRisk.FinalProbability,
                        StartSeverity = newBusinessRisk.FinalSeverity,
                        StartResult = newBusinessRisk.FinalResult,
                        StartAction = 3,
                        Active = true,
                        CompanyId = newBusinessRisk.CompanyId,
                        PreviousBusinessRiskId = newBusinessRisk.Id,
                        Result = Convert.ToInt32(newBusinessRisk.FinalResult),
                        Process = newBusinessRisk.Process,
                        ProcessId = newBusinessRisk.Process.Id,
                        Rules = newBusinessRisk.Rules
                    };

                    res = newBusinessRiskEvaluated.Insert(userId);

                    ApplicationUser newUser = ApplicationUser.GetById(userId, companyId);

                    IncidentAction newAction = new IncidentAction()
                    {
                        IncidentId = 0,
                        BusinessRiskId = newBusinessRiskEvaluated.Id,
                        Description = newBusinessRisk.Description,
                        WhatHappened = newBusinessRisk.ItemDescription,
                        WhatHappenedBy = newUser.Employee,
                        WhatHappenedOn = newBusinessRisk.FinalDate,
                        Causes = newBusinessRiskEvaluated.Causes,
                        CausesBy = newUser.Employee,
                        CausesOn = newBusinessRisk.FinalDate,
                        CompanyId = newBusinessRiskEvaluated.CompanyId,
                        ReporterType = 1,
                        Origin = 4,
                        ActionType = 3
                    };

                    newAction.Insert(userId);
                }

                this.Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>Insert item in database</summary>
        /// <param name="businessRisk">Risk to insert</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult BusinessRiskInsert(BusinessRisk businessRisk, int companyId, int userId)
        {
            ActionResult res = businessRisk.Insert(userId);
            if (res.Success)
            {
                //// string differences = businessRisk.Differences(BusinessRisk.Empty);
                //// ActionResult logRes = ActivityLog.BusinessRisk(Convert.ToInt64(res.MessageError), userId, companyId, DepartmentLogActions.Create, differences);
                this.Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>Obtain risk by filter</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="from">Date from</param>
        /// <param name="to">Date to</param>
        /// <param name="rulesId">Rules identifier</param>
        /// <param name="processId">Process identifier</param>
        /// <param name="type">Risk type</param>
        /// <returns>Risk compliant by filter</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public string GetFilter(int companyId, DateTime? from, DateTime? to, long rulesId, long processId, int type)
        {
            StringBuilder filter = new StringBuilder("{");
            filter.Append(Tools.JsonPair("companyId", companyId)).Append(",");
            filter.Append(Tools.JsonPair("from", from)).Append(",");
            filter.Append(Tools.JsonPair("to", to)).Append(",");
            filter.Append(Tools.JsonPair("rulesId", rulesId)).Append(",");
            filter.Append(Tools.JsonPair("processId", processId)).Append(",");
            filter.Append(Tools.JsonPair("type", type)).Append("}");
            this.Session["BusinessRiskFilter"] = filter.ToString();

            return BusinessRisk.FilterList(companyId, from, to, rulesId, processId, type);
        }
    }
}