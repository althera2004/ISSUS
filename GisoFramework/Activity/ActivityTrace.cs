// -----------------------------------------------------------------------
// <copyright file="ActivityTrace.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Activity
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using System.Web;

    /// <summary>Available actions for a job position item</summary>
    [FlagsAttribute]
    public enum JobPositionLogActions
    {
        /// <summary>None action - 0</summary>
        None = 0,

        /// <summary>Create action - 1</summary>
        Create = 1,

        /// <summary>Update action - 2</summary>
        Update = 2
    }

    /// <summary>Log actions for providers</summary>
    [FlagsAttribute]
    public enum ProviderLogActions
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Create - 1</summary>
        Create = 1,

        /// <summary>Update - 2</summary>
        Update = 2,

        /// <summary>Delete - 3</summary>
        Delete = 3
    }
    
    /// <summary>Logs actions for customers</summary>
    [FlagsAttribute]
    public enum CustomerLogActions
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Create - 1</summary>
        Create = 1,

        /// <summary>Update - 2</summary>
        Update = 2,

        /// <summary>Delete - 3</summary>
        Delete = 3
    }

    /// <summary>Available actions for a company item</summary>
    [FlagsAttribute]
    public enum CompanyLogActions
    {
        /// <summary>None action - 0</summary>
        None = 0,

        /// <summary>Update action</summary>
        Update = 1,

        /// <summary>New company address action</summary>
        NewCompanyAddress = 2,

        /// <summary>Set default address</summary>
        SetDefaultAddress = 3,

        /// <summary>Delete address</summary>
        DeleteAddress = 4,

        /// <summary>Update address</summary>
        UpdateAddress = 5
    }

    /// <summary>Available actions for a department item</summary>
    [FlagsAttribute]
    public enum DepartmentLogActions
    {
        /// <summary>Undefined action</summary>
        None = 0,

        /// <summary>Department is created</summary>
        Create = 1,

        /// <summary>Department is modified</summary>
        Modify = 2,

        /// <summary>Department is deleted</summary>
        Delete = 3
    }

    /// <summary>Available actions for a document item</summary>
    public enum DocumentLogAction
    {
        /// <summary>None action</summary>
        None = 0,

        /// <summary>Document is created</summary>            
        Create = 1,

        /// <summary>Document is updated</summary>            
        Update = 2,

        /// <summary>Document is deleted</summary>
        Delete = 3,

        /// <summary>Document has a new version</summary>            
        Versioned = 5,

        /// <summary>Document is in draft mode</summary>            
        Draft = 6,

        /// <summary>Document is validated</summary>            
        Validated = 7
    }

    /// <summary>Available actions for a employee item</summary>
    [FlagsAttribute]
    public enum EmployeeLogActions
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Create - 1</summary>
        Create = 1,

        /// <summary>Modify - 2</summary>
        Modify = 2,

        /// <summary>Delete - 3</summary>
        Delete = 3,

        /// <summary>Associate to department - 4</summary>
        AssociateToDepartment = 4,

        /// <summary>Disassociate department - 5</summary>
        DisassociateDepartment = 5,

        /// <summary>Set job position - 6</summary>
        SetJobPosition = 6,

        /// <summary>Set learning - 7</summary>
        SetLearning = 7,

        /// <summary>Unset job position - 8</summary>
        UnsetJobPosition = 8,

        /// <summary>Disabled - 8</summary>
        Disabled = 9,

        /// <summary>Restore - 8</summary>
        Restore = 10
    }

    /// <summary>Available actions for a proccess item</summary>
    [FlagsAttribute]
    public enum ProcessLogActions
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Create - 1</summary>
        Create = 1,

        /// <summary>Modify -2</summary>
        Modify = 2
    }

    /// <summary>Available actions for a learning item</summary>
    [FlagsAttribute]
    public enum LearningLogActions
    {
        /// <summary>None -0</summary>
        None = 0,

        /// <summary>Create - 1</summary>
        Create = 1,

        /// <summary>Modified - 2</summary>
        Modified = 2,

        /// <summary>Add assistant - 3</summary>
        AddAssistant = 3
    }

    /// <summary>Available actions for a learning assistant item</summary>
    [FlagsAttribute]
    public enum LearningAssistantLogActions
    {
        /// <summary>None - 0</summary>
        None = 0,

        /// <summary>Create - 1</summary>
        Create = 1,

        /// <summary>Modified - 2</summary>
        Modified = 2,

        /// <summary>Set complete - 3</summary>
        Completed = 3,

        /// <summary>Set finish - 4</summary>
        Finished = 4
    }

    /// <summary>A class that implements trace of activity in application</summary>
    public class ActivityTrace
    {
        /// <summary>Prefix for trace dictionary prefix</summary>
        public const string ItemTraceDictionaryPrefix = "Item_Trace_";

        #region Fields
        /// <summary>
        /// Description of target
        /// </summary>
        private string target;

        /// <summary>
        /// Description of action
        /// </summary>
        private string action;

        /// <summary>
        /// Date of action
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Description of changes
        /// </summary>
        private string changes;

        /// <summary>
        /// Name of employe that performs actions
        /// </summary>
        private string actionEmployee;

        /// <summary>
        /// Identifier of target
        /// </summary>
        private int targetId;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating activity description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the target identifier
        /// </summary>
        public int TargetId
        {
            get
            {
                return this.targetId;
            }

            set
            {
                this.targetId = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicatong the name of employee that performs action
        /// </summary>
        public string ActionEmployee
        {
            get
            { 
                return this.actionEmployee; 
            }

            set 
            { 
                this.actionEmployee = value; 
            }
        }

        /// <summary>
        /// Gets or sets de type of target
        /// </summary>
        public string Target
        {
            get 
            {
                return this.target; 
            }

            set 
            {
                this.target = value; 
            }
        }

        /// <summary>
        /// Gets or sets tha action ocurred
        /// </summary>
        public string Action
        {
            get 
            {
                return this.action; 
            }

            set
            {
                this.action = value;
            }
        }

        /// <summary>
        /// Gets or sets the date of action
        /// </summary>
        public DateTime Date
        {
            get 
            {
                return this.date; 
            }

            set 
            {
                this.date = value; 
            }
        }

        /// <summary>
        /// Gets or sets a description of changes of action
        /// </summary>
        public string Changes
        {
            get 
            {
                return this.changes;
            }

            set 
            {
                this.changes = value; 
            }
        }

        /// <summary>
        /// Gets the html code that shows a trace
        /// </summary>
        /// <example>
        /// <tr>
        ///     <td>Date</td>
        ///     <td>Target</td>
        ///     <td>Action</td>
        ///     <td>Data changes</td>
        ///     <td>Employee</td>
        /// </tr>
        /// </example>
        public string TableRow
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", this.date, this.target, this.action, this.changes, this.actionEmployee);
            }
        }

        /// <summary>
        /// Gets the html code that shows a trace for principal trace tables
        /// </summary>
        /// <example>
        /// <tr>
        ///     <td>Date</td>
        ///     <td>Target identifier</td>
        ///     <td>Action</td>
        ///     <td>Data changes</td>
        ///     <td>Employee</td>
        /// </tr>
        /// </example>
        public string TableTracesRow
        {
            get
            {
                string description = this.Description;
                if (string.IsNullOrEmpty(description))
                {
                    description = string.Format(CultureInfo.GetCultureInfo("en-us"), "[{0}]", this.targetId);
                }

                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>", this.date, description, this.action, this.changes, this.actionEmployee);
            }
        }

        /// <summary>
        /// Gets the html code that shows a targeted trace
        /// </summary>
        /// <example>
        /// <tr>
        ///     <td>Date</td>
        ///     <td>Action</td>
        ///     <td>Data changes</td>
        ///     <td>Employee</td>
        /// </tr>
        /// </example>
        public string TableTargetedRow
        {
            get
            {
                string pattern = @"<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"), 
                    pattern,
                    this.date,
                    Tools.Translate(ItemTraceDictionaryPrefix + this.action),
                    this.changes,
                    this.actionEmployee);
            }
        }
        #endregion

        /// <summary>
        /// Gets html code for table traces of an item
        /// </summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="targetType">Target type of item</param>
        /// <returns>Html code for table traces of an item</returns>
        public static string RenderTraceTableForItem(int itemId, TargetType targetType)
        {
            var company = (GisoFramework.Item.Company)HttpContext.Current.Session["company"];
            var res = new StringBuilder();
            var activities = ActivityLog.GetActivity(itemId, targetType, company.Id, null, null);
            foreach (ActivityTrace activity in activities)
            {
                res.Append(activity.TableTargetedRow);
            }

            return res.ToString();
        }

        /// <summary>
        /// Gets html code for table traces of an item
        /// </summary>
        /// <param name="itemId">Item identifier</param>
        /// <param name="targetType">Target type of item</param>
        /// <returns>Html code for table traces of an item</returns>
        public static string RenderTraceTableForItem(long itemId, TargetType targetType)
        {
            GisoFramework.Item.Company company = (GisoFramework.Item.Company)HttpContext.Current.Session["company"];
            StringBuilder res = new StringBuilder();
            ReadOnlyCollection<ActivityTrace> activities = ActivityLog.GetActivity(itemId, targetType, company.Id, null, null);
            foreach (ActivityTrace activity in activities)
            {
                res.Append(activity.TableTargetedRow);
            }

            return res.ToString();
        }
    }
}
