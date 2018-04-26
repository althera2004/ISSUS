// -----------------------------------------------------------------------
// <copyright file="JobPositionAsigment.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// Implements JobPositionAsigment class
    /// </summary>
    public class JobPositionAsigment
    {
        #region Fields
        /// <summary>
        /// Employee assigned
        /// </summary>
        private Employee employee;

        /// <summary>
        /// Job position of assignation 
        /// </summary>
        private JobPosition jobPosition;

        /// <summary>
        /// Date of assignation start
        /// </summary>
        private DateTime startDate;

        /// <summary>
        /// Date of assignation finish
        /// </summary>
        private DateTime? endDate;
        #endregion

        #region Properties
        /// <summary>
        /// Gets an empty assignation
        /// </summary>
        public static JobPositionAsigment Empty
        {
            get
            {
                return new JobPositionAsigment()
                {
                    employee = Employee.EmptySimple,
                    jobPosition = JobPosition.Empty
                };
            }
        }

        /// <summary>
        /// Gets or sets the employee assigned
        /// </summary>
        public Employee Employee
        {
            get
            { 
                return this.employee; 
            }

            set 
            { 
                this.employee = value;
            }
        }

        /// <summary>
        /// Gets or sets the job position of assignation
        /// </summary>
        public JobPosition JobPosition
        {
            get
            { 
                return this.jobPosition; 
            }

            set
            { 
                this.jobPosition = value; 
            }
        }

        /// <summary>
        /// Gets or sets the date of assignation start
        /// </summary>
        public DateTime StartDate
        {
            get 
            { 
                return this.startDate;
            }

            set 
            {
                this.startDate = value; 
            }
        }

        /// <summary>
        /// Gets or sets the date of assignation finish
        /// </summary>
        public DateTime? EndDate
        {
            get 
            { 
                return this.endDate; 
            }

            set 
            { 
                this.endDate = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a JON structure of assignation
        /// </summary>
        public string Json
        {
            get
            {
                string pattern = @"{{""Id"":{0},""Description"":""{1}"",""EndDate"":{2}}}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.jobPosition.Id,
                    this.jobPosition.Description.Replace("\"", "\\\""),
                    this.endDate.HasValue ? "true" : "false");
            }
        }

        /// <summary>
        /// Render the HTML code for a row to job position assigantiosn on employee profile
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grantDelete"></param>
        /// <param name="grantDepartmentsView"></param>
        /// <param name="grantJobPositionView"></param>
        /// <returns>HTML code</returns>
        public string TableRow(Dictionary<string, string> dictionary, bool grantDelete, bool grantJobPositionView, bool grantDepartmentsView)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string endDateCell = string.Empty;
            if (this.endDate.HasValue)
            {
                endDateCell = this.endDate.Value.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-us"));
            }
            else
            {
                endDateCell = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<button class=""btn btn-warning"" type=""button"" style=""padding:0 !important"" onclick=""UnassociatedJobPosition(this);""><i class=""icon-remove bigger-110""></i>{0}</button>", dictionary["Item_Employee_Button_Unlink"]);
            }

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                iconDelete = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span title=""{2}"" class=""btn btn-xs btn-danger"" onclick=""DeleteJobPosition('{0}','{1}');""><i class=""icon-trash bigger-120""></i></span>",
                    this.jobPosition.Id,
                    Tools.JsonCompliant(this.jobPosition.Description),
                    dictionary["Common_Delete"]);
            }

            string responsibleDescription = string.Empty;
            if (this.jobPosition.Responsible != null)
            {
                responsibleDescription = this.jobPosition.Responsible.Description;
            }

            string pattern = @"<tr id=""{5}""><td>{0}</td><td>{1}</td><td>{2}</td><td align=""center"">{3}</td><td align=""center"">{4}</td><td align=""center"">{6}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                grantJobPositionView ? this.JobPosition.Link : this.jobPosition.Description,
                grantDepartmentsView ? this.jobPosition.Department.Link : this.jobPosition.Department.Description,
                responsibleDescription,
                this.startDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-us")),
                endDateCell,
                this.jobPosition.Id,
                iconDelete);
        }
    }
}
