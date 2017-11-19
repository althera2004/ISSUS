// --------------------------------
// <copyright file="LearningAssistance.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;    

    /// <summary>
    /// Implementation of LearningAssistance class.
    /// </summary>
    public class LearningAssistance
    {
        #region Properties
        /// <summary>
        /// Gets or sets the date of assistance
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the idenfier of learning assistance
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the learning of assistance
        /// </summary>
        public Learning Learning { get; set; }

        /// <summary>
        /// Gets or sets the company identifier
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the job position of assitance
        /// </summary>
        public JobPosition JobPosition { get; set; }

        /// <summary>
        /// Gets or sets the employee of assistance
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets if employye has succesfull finish the learning
        /// </summary>
        public bool? Success { get; set; }

        /// <summary>
        /// Gets or sets if employee has finish the learning
        /// </summary>
        public bool? Completed { get; set; }

        /// <summary>
        /// Gets a Json structure of learning assistance
        /// </summary>
        public string Json
        {
            get
            {
                string completedText = "null";
                string successText = "null";

                if (this.Completed.HasValue)
                {
                    completedText = this.Completed.Value ? "true" : "false";
                }

                if (this.Success.HasValue)
                {
                    successText = this.Success.Value ? "true" : "false";
                }

                string pattern = @"
                    {{
                        ""Id"":{0},
                        ""CompanyId"":{1},
                        ""JobPosition"":{{""Id"":{2},""Description"":""{3}""}},
                        ""Employee"":{{""Id"":{4}}},
                        ""Success"":{5},
                        ""Completed"":{6},
                        ""Date"":""{7}""
                    }}
                    ";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.CompanyId,
                    this.JobPosition.Id,
                    this.JobPosition.Description,
                    this.Employee.Id,
                    successText,
                    completedText,
                    this.Date);
            }
        }
        #endregion

        /// <summary>
        /// Render the HTML code for a row to assistance table of a learning profile
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="status">Status of learning</param>
        /// <returns>HTML code</returns>
        public string LearningRow(Dictionary<string, string> dictionary, int status)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string successSpan = string.Empty;
            string completedSpan = string.Empty;
            string completedText = "-";
            string successText = "-";
            string completedColor = "#000";
            string successColor = "#000";
            string completedStatus = "0";
            string successStatus = "0";
            if (this.Completed.HasValue)
            {
                completedText = this.Completed.Value ? dictionary["Common_Yes"] : dictionary["Common_No"];
                completedColor = this.Completed.Value ? "green" : "red";
                completedStatus = this.Completed.Value ? "1" : "2";

                if (this.Success.HasValue)
                {
                    successText = this.Success.Value ? dictionary["Common_Yes"] : dictionary["Common_No"];
                    successColor = this.Success.Value ? "green" : "red";
                    successStatus = this.Success.Value ? "1" : "2";
                }
            }

            if (this.Learning.Status == 1)
            {
                completedSpan = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span style=""color:{0};cursor:pointer;"" onclick=""Toggle(this, 'Completed', {2}, {3});"">{1}</span>", completedColor, completedText, this.Id, completedStatus);
                successSpan = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span style=""color:{0};cursor:pointer;"" onclick=""Toggle(this, 'Success', {2}, {3});"">{1}</span>", successColor, successText, this.Id, successStatus);
            }

            if (this.Learning.Status == 2)
            {
                completedSpan = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span style=""color:{0}"">{1}</span>", completedColor, completedText);
                successSpan = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span style=""color:{0}"">{1}</span>", successColor, successText);
            }

            string res = string.Empty;

            switch (status)
            {
                case 0:
                    string iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""EmployeeDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", this.Id, this.Employee.FullName, dictionary["Common_Delete"]);
                    res = string.Format(
                         CultureInfo.GetCultureInfo("en-us"),
                         @"<tr><!--<td><input type=""checkbox"" id=""chk{0}"" /></td>--><td>{0}</td><td align=""center"">{2}</td></tr>",
                         this.Employee.FullName,
                         this.JobPosition.Link,
                         iconDelete);
                    break;
                case 1:
                    res = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"<tr id=""{3}|{4}""><td><input type=""checkbox"" /></td><td>{0}</td><td align=""center"">{1}</td><td class=""hidden-480"" align=""center"">{2}</td></tr>",
                        this.Employee.FullName,
                        completedSpan,
                        successSpan,
                        this.Id,
                        this.Employee.Id);
                    break;
                case 2:
                    res = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"<tr><td>{0}</td><td align=""center"">{1}</td><td class=""hidden-480"" align=""center"">{2}</td></tr>",
                        this.Employee.FullName,
                        completedSpan,
                        successSpan);
                    break;
            }

            return res;
        }

        /// <summary>
        /// Render the HTML code to get a learning row for the learning table of employee profile
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <returns>HTML cdoe</returns>
        public string TableRowProfile(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string estado = string.Empty;
            if (this.Learning.Status == 0 || !this.Learning.RealFinish.HasValue)
            {
                estado = dictionary["Item_LearningAssistant_Status_InProgress"];
            }
            else
            {
                estado = this.Learning.RealFinish.Value.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-us"));
            }

            string pattern = @"<tr><td>{2}</td><td align=""center"">{0}</td><td class=""hidden-480"" align=""center"">{1}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                estado,
                this.Success.HasValue ? (this.Success.Value ? dictionary["Common_Yes"] : dictionary["Common_No"]) : "-",
                this.Learning.Link);
        }
    }
}
