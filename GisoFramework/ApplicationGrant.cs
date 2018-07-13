// --------------------------------
// <copyright file="ApplicationGrant.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    /// <summary>Implements ApplicationGrant</summary>
    public sealed class ApplicationGrant
    {
        /// <summary>Initializes a new instance of the ApplicationGrant class.</summary>
        public ApplicationGrant()
        {
        }

        /// <summary>Gets a Gets a grant for company</summary>
        public static ApplicationGrant CompanyProfile
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.CompanyProfile }; }
        }

        /// <summary>Gets a grant for job position</summary>
        public static ApplicationGrant JobPosition
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.JobPosition }; }
        }

        /// <summary>Gets a grant for department</summary>
        public static ApplicationGrant Department
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Department }; }
        }

        /// <summary>Gets a grant for company</summary>
        public static ApplicationGrant Employee
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Employee }; }
        }

        /// <summary>Gets a grant for user</summary>
        public static ApplicationGrant User
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.User }; }
        }

        /// <summary>Gets a grant for trace</summary>
        public static ApplicationGrant Trace
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Trace }; }
        }

        /// <summary>Gets a grant for document</summary>
        public static ApplicationGrant Document
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Document }; }
        }

        /// <summary>Gets a grant for process</summary>
        public static ApplicationGrant Process
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Proccess }; }
        }

        /// <summary>Gets a grant for learning</summary>
        public static ApplicationGrant Learning
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Learning }; }
        }

        /// <summary>Gets a grant for equipment</summary>
        public static ApplicationGrant Equipment
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Equipment }; }
        }

        /// <summary>Gets a grant for incident</summary>
        public static ApplicationGrant Incident
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Incident }; }
        }

        /// <summary>Gets a grant for incident action</summary>
        public static ApplicationGrant IncidentActions
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.IncidentActions }; }
        }

        /// <summary>Gets a grant for provider</summary>
        public static ApplicationGrant Provider
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Provider }; }
        }

        /// <summary>Gets a grant for customer</summary>
        public static ApplicationGrant Customer
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Customer }; }
        }

        /// <summary>Gets a grant for cost</summary>
        public static ApplicationGrant Cost
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Cost }; }
        }

        /// <summary>Gets a grant for business risk</summary>
        public static ApplicationGrant BusinessRisk
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.BusinessRisk }; }
        }

        /// <summary>Gets a grant for oportunity</summary>
        public static ApplicationGrant Oportunity
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Oportunity }; }
        }

        /// <summary>Gets a grant for rule</summary>
        public static ApplicationGrant Rule
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Rules }; }
        }

        /// <summary>Gets a grant for rule</summary>
        public static ApplicationGrant CostDefinition
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.CostDefinition }; }
        }

        /// <summary>Gets a grant for objective</summary>
        public static ApplicationGrant Objetivo
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Objetivo }; }
        }

        /// <summary>Gets a grant for Indicator</summary>
        public static ApplicationGrant Indicador
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Indicador }; }
        }

        /// <summary>Gets a grant for Unidad</summary>
        public static ApplicationGrant Unidad
        {
            get { return new ApplicationGrant { Code = ItemIdentifiers.Unidad }; }
        }

        /// <summary>Gets or sets the code of item</summary>
        public int Code { get; set; }

        /// <summary>Gets or sets the description of item</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the page of item</summary>
        public string Page { get; set; }

        /// <summary>Gets a grant from item code</summary>
        /// <param name="value">Item code</param>
        /// <returns>Grant for item based on code</returns>
        public static ApplicationGrant FromInteger(int value)
        {
            return new ApplicationGrant { Code = value, Description = GetLabel(value) };
        }

        /// <summary>Gets a grant from code and url</summary>
        /// <param name="value">Item code</param>
        /// <param name="listPage">Item page url</param>
        /// <returns>Grant for item based on code and url</returns>
        public static ApplicationGrant FromIntegerUrl(int value, string listPage)
        {
            return new ApplicationGrant { Code = value, Description = GetLabel(value), Page = listPage };
        }

        /// <summary>Gets the label of an item</summary>
        /// <param name="value">Code of item</param>
        /// <returns>The label of an item based on code</returns>
        public static string GetLabel(int value)
        {
            switch (value)
            {
                case 1: return "DashBoard";
                case ItemIdentifiers.CompanyProfile: return "CompanyProfile";
                case ItemIdentifiers.JobPosition: return "JobPosition";
                case ItemIdentifiers.Department: return "Department";
                case ItemIdentifiers.Employee: return "Employee";
                case ItemIdentifiers.User: return "User";
                case ItemIdentifiers.Trace: return "Trace";
                case ItemIdentifiers.Document: return "Document";
                case ItemIdentifiers.Proccess: return "Proccess";
                case ItemIdentifiers.Learning: return "Learning";
                case ItemIdentifiers.Equipment: return "Equipment";
                case ItemIdentifiers.Incident: return "Incident";
                case ItemIdentifiers.IncidentActions: return "IncidentActions";
                case ItemIdentifiers.Provider: return "Provider";
                case ItemIdentifiers.Customer: return "Customer";
                case ItemIdentifiers.Cost: return "Cost";
                case ItemIdentifiers.BusinessRisk: return "BusinessRisk";
                case ItemIdentifiers.Rules: return "Rules";
                case ItemIdentifiers.CostDefinition: return "CostDefinition";
                case ItemIdentifiers.Objetivo: return "Objetivo";
                case ItemIdentifiers.Indicador: return "Indicador";
                case ItemIdentifiers.Unidad: return "Unidad";
                case ItemIdentifiers.Oportunity: return "Oportunity";
                default: return "undefined";
            }
        }
    }
}