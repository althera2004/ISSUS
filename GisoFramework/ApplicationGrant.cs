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
            get { return new ApplicationGrant() { Code = 2 }; }
        }

        /// <summary>Gets a grant for job position</summary>
        public static ApplicationGrant JobPosition
        {
            get { return new ApplicationGrant() { Code = 3 }; }
        }

        /// <summary>Gets a grant for department</summary>
        public static ApplicationGrant Department
        {
            get { return new ApplicationGrant() { Code = 4 }; }
        }

        /// <summary>Gets a grant for company</summary>
        public static ApplicationGrant Employee
        {
            get { return new ApplicationGrant() { Code = 5 }; }
        }

        /// <summary>Gets a grant for user</summary>
        public static ApplicationGrant User
        {
            get { return new ApplicationGrant() { Code = 6 }; }
        }

        /// <summary>Gets a grant for trace</summary>
        public static ApplicationGrant Trace
        {
            get { return new ApplicationGrant() { Code = 7 }; }
        }

        /// <summary>Gets a grant for document</summary>
        public static ApplicationGrant Document
        {
            get { return new ApplicationGrant() { Code = 8 }; }
        }

        /// <summary>Gets a grant for process</summary>
        public static ApplicationGrant Process
        {
            get { return new ApplicationGrant() { Code = 9 }; }
        }

        /// <summary>Gets a grant for learning</summary>
        public static ApplicationGrant Learning
        {
            get { return new ApplicationGrant() { Code = 10 }; }
        }

        /// <summary>Gets a grant for equipment</summary>
        public static ApplicationGrant Equipment
        {
            get { return new ApplicationGrant() { Code = 11 }; }
        }

        /// <summary>Gets a grant for incident</summary>
        public static ApplicationGrant Incident
        {
            get { return new ApplicationGrant() { Code = 12 }; }
        }

        /// <summary>Gets a grant for incident action</summary>
        public static ApplicationGrant IncidentActions
        {
            get { return new ApplicationGrant() { Code = 13 }; }
        }

        /// <summary>Gets a grant for provider</summary>
        public static ApplicationGrant Provider
        {
            get { return new ApplicationGrant() { Code = 14 }; }
        }

        /// <summary>Gets a grant for customer</summary>
        public static ApplicationGrant Customer
        {
            get { return new ApplicationGrant() { Code = 15 }; }
        }

        /// <summary>Gets a grant for cost</summary>
        public static ApplicationGrant Cost
        {
            get { return new ApplicationGrant() { Code = 16 }; }
        }

        /// <summary>Gets a grant for business risk</summary>
        public static ApplicationGrant BusinessRisk
        {
            get { return new ApplicationGrant() { Code = 18 }; }
        }

        /// <summary>Gets a grant for rule</summary>
        public static ApplicationGrant Rule
        {
            get { return new ApplicationGrant() { Code = 19 }; }
        }

        /// <summary>Gets a grant for rule</summary>
        public static ApplicationGrant CostDefinition
        {
            get { return new ApplicationGrant() { Code = 20 }; }
        }

        /// <summary>Gets a grant for objective</summary>
        public static ApplicationGrant Objetivo
        {
            get { return new ApplicationGrant() { Code = 21 }; }
        }

        /// <summary>Gets a grant for Indicator</summary>
        public static ApplicationGrant Indicador
        {
            get { return new ApplicationGrant() { Code = 22 }; }
        }

        /// <summary>Gets a grant for Unidad</summary>
        public static ApplicationGrant Unidad
        {
            get { return new ApplicationGrant() { Code = 23 }; }
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
            return new ApplicationGrant() { Code = value, Description = GetLabel(value) };
        }

        /// <summary>Gets a grant from code and url</summary>
        /// <param name="value">Item code</param>
        /// <param name="listPage">Item page url</param>
        /// <returns>Grant for item based on code and url</returns>
        public static ApplicationGrant FromIntegerUrl(int value, string listPage)
        {
            return new ApplicationGrant()
            {
                Code = value,
                Description = GetLabel(value),
                Page = listPage
            };
        }

        /// <summary>Gets the label of an item</summary>
        /// <param name="value">Code of item</param>
        /// <returns>The label of an item based on code</returns>
        public static string GetLabel(int value)
        {
            switch (value)
            {
                case 2: return "CompanyProfile";
                case 3: return "JobPosition";
                case 4: return "Department";
                case 5: return "Employee";
                case 6: return "User";
                case 7: return "Trace";
                case 8: return "Document";
                case 9: return "Proccess";
                case 10: return "Learning";
                case 11: return "Equipment";
                case 12: return "Incident";
                case 13: return "IncidentActions";
                case 14: return "Provider";
                case 15: return "Customer";
                case 16: return "Cost";
                case 18: return "BusinessRisk";
                case 19: return "Rules";
                case 20: return "CostDefinition";
                case 21: return "Objetivo";
                case 22: return "Indicador";
                case 23: return "Unidad";
                default: return "undefined";
            }
        }
    }
}