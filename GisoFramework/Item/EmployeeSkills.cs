// --------------------------------
// <copyright file="EmployeeSkills.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements EmployeeSkills class</summary>
    public class EmployeeSkills : BaseItem
    {
        /// <summary>
        /// Initializes a new instance of the EmployeeSkills class.
        /// The data is searched in data base based on employee identifier and company identifier
        /// </summary>
        public EmployeeSkills()
        {
        }

        /// <summary>
        /// Initializes a new instance of the EmployeeSkills class.
        /// The data is searched in data base based on employee identifier and company identifier
        /// </summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="companyId">Company identifier</param>
        public EmployeeSkills(long employeeId, int companyId)
        {
            /* CREATE PROCEDURE EmployeeSkills_GetByEmployee
             * @EmployeeId int,
             * @CompanyId int */
            using (var cmd = new SqlCommand("EmployeeSkills_GetByEmployee"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                this.Id = rdr.GetInt32(ColumnsGetSkillByEmployee.Id);
                                this.Employee = new Employee
                                {
                                    Id = rdr.GetInt32(ColumnsGetSkillByEmployee.EmployeeId),
                                    CompanyId = rdr.GetInt32(ColumnsGetSkillByEmployee.CompanyId)
                                };

                                this.Academic = rdr.GetString(ColumnsGetSkillByEmployee.Academic);
                                this.Specific = rdr.GetString(ColumnsGetSkillByEmployee.Specific);
                                this.WorkExperience = rdr.GetString(ColumnsGetSkillByEmployee.WorkExperience);
                                this.Ability = rdr.GetString(ColumnsGetSkillByEmployee.Ability);

                                if (!rdr.IsDBNull(ColumnsGetSkillByEmployee.AcademicValid))
                                {
                                    this.AcademicValid = rdr.GetBoolean(ColumnsGetSkillByEmployee.AcademicValid);
                                }

                                if (!rdr.IsDBNull(ColumnsGetSkillByEmployee.SpecificValid))
                                {
                                    this.SpecificValid = rdr.GetBoolean(ColumnsGetSkillByEmployee.SpecificValid);
                                }

                                if (!rdr.IsDBNull(ColumnsGetSkillByEmployee.WorkExperienceValid))
                                {
                                    this.WorkExperienceValid = rdr.GetBoolean(ColumnsGetSkillByEmployee.WorkExperienceValid);
                                }

                                if (!rdr.IsDBNull(ColumnsGetSkillByEmployee.AbilityValid))
                                {
                                    this.AbilityValid = rdr.GetBoolean(ColumnsGetSkillByEmployee.AbilityValid);
                                }

                                this.ModifiedOn = rdr.GetDateTime(ColumnsGetSkillByEmployee.ModifiedOn);
                                this.ModifiedBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsGetSkillByEmployee.ModifiedByUserId),
                                    UserName = rdr.GetString(ColumnsGetSkillByEmployee.ModifiedByUserName)
                                };

                                this.ModifiedBy.Employee = Employee.ByUserId(this.ModifiedBy.Id);
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }

        /// <summary>Gets an empty employees' skills</summary>
        public static EmployeeSkills Empty
        {
            get
            {
                return new EmployeeSkills
                {
                    Id = 0,
                    Employee = Employee.EmptySimple,
                    AcademicValid = null,
                    SpecificValid = null,
                    WorkExperienceValid = null,
                    AbilityValid = null
                };
            }
        }

        /// <summary>Gets or sets the employee of skills</summary>
        public Employee Employee { get; set; }

        /// <summary>Gets or sets the text for academic skills</summary>
        public string Academic { get; set; }

        /// <summary>Gets or sets the text for specific skills</summary>
        public string Specific { get; set; }

        /// <summary>Gets or sets the for for the work experience of employee</summary>
        public string WorkExperience { get; set; }

        /// <summary>Gets or sets the text for employee's abilities</summary>
        public string Ability { get; set; }

        /// <summary>Gets or sets a value indicating whether academic skills are valid</summary>
        public bool? AcademicValid { get; set; }

        /// <summary>Gets or sets a value indicating whether specific skills are valid</summary>
        public bool? SpecificValid { get; set; }

        /// <summary>Gets or sets a value indicating whether work experience of employee are valid</summary>
        public bool? WorkExperienceValid { get; set; }

        /// <summary>Gets or sets a value indicating whether employee's abilities are valid</summary>
        public bool? AbilityValid { get; set; }

        /// <summary>Gets link to Employee Skill page</summary>
        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Name"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = @"
                    {{
                        ""Id"":{0},
                        ""Employee"":{{""Id"":{1},""CompanyId"":{2}}},
                        ""Academic"":""{3}"",
                        ""Specific"":""{4}"",
                        ""WorkExperience"":""{5}"",
                        ""Ability"":""{6}"",
                        ""AcademicValid"":{7},
                        ""SpecificValid"":{8},
                        ""WorkExperienceValid"":{9},
                        ""HabilityValid"":{10}
                    }}";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Employee.Id,
                    this.Employee.CompanyId,
                    Tools.JsonCompliant(this.Academic),
                    Tools.JsonCompliant(this.Specific),
                    Tools.JsonCompliant(this.WorkExperience),
                    Tools.JsonCompliant(this.Ability),
                    Tools.JsonValue(this.AcademicValid.Value),
                    Tools.JsonValue(this.SpecificValid.Value),
                    Tools.JsonValue(this.WorkExperienceValid.Value),
                    Tools.JsonValue(this.AbilityValid.Value));
            }
        }
        
        /// <summary>Insert an employee's skill into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EmployeeSkills_Insert
             * @Id int out,
             * @EmployeeId int,
             * @CompanyId int,
             * @Academic text,
             * @Specific text,
             * @WorkExperience text,
             * @Hability text,
             * @AcademicValid bit,
             * @SpecificValid bit,
             * @WorkExperienceValid bit,
             * @HabilityValid bit,
             * @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EmployeeSkills_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Employee.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Employee.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Academic", this.Academic));
                        cmd.Parameters.Add(DataParameter.Input("@Specific", this.Specific));
                        cmd.Parameters.Add(DataParameter.Input("@WorkExperience", this.WorkExperience));
                        cmd.Parameters.Add(DataParameter.Input("@Hability", this.Ability));
                        cmd.Parameters.Add("@AcademicValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@SpecificValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@WorkExperienceValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@HabilityValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);

                        if (this.AcademicValid.HasValue)
                        {
                            cmd.Parameters["@AcademicValid"].Value = this.AcademicValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@AcademicValid"].Value = DBNull.Value;
                        }

                        if (this.SpecificValid.HasValue)
                        {
                            cmd.Parameters["@SpecificValid"].Value = this.SpecificValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@SpecificValid"].Value = DBNull.Value;
                        }

                        if (this.WorkExperienceValid.HasValue)
                        {
                            cmd.Parameters["@WorkExperienceValid"].Value = this.WorkExperienceValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@WorkExperienceValid"].Value = DBNull.Value;
                        }

                        if (this.AbilityValid.HasValue)
                        {
                            cmd.Parameters["@HabilityValid"].Value = this.AbilityValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@HabilityValid"].Value = DBNull.Value;
                        }

                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(cmd.Parameters["@Id"].Value.ToString());
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>Update employee skills in data base</summary>
        /// <param name="userId">Identifier of user taht performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            /* CREATE PROCEDURE EmployeeSkills_Update
             * @Id int,
             * @EmployeeId int,
             * @CompanyId int,
             * @Academic text,
             * @Specific text,
             * @WorkExperience text,
             * @Hability text,
             * @AcademicValid bit,
             * @SpecificValid bit,
             * @WorkExperienceValid bit,
             * @HabilityValid bit,
             * @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EmployeeSkills_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Employee.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Employee.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Academic", this.Academic));
                        cmd.Parameters.Add(DataParameter.Input("@Specific", this.Specific));
                        cmd.Parameters.Add(DataParameter.Input("@WorkExperience", this.WorkExperience));
                        cmd.Parameters.Add(DataParameter.Input("@Hability", this.Ability));
                        cmd.Parameters.Add("@AcademicValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@SpecificValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@WorkExperienceValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@HabilityValid", SqlDbType.Bit);
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);

                        if (this.AcademicValid.HasValue)
                        {
                            cmd.Parameters["@AcademicValid"].Value = this.AcademicValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@AcademicValid"].Value = DBNull.Value;
                        }

                        if (this.SpecificValid.HasValue)
                        {
                            cmd.Parameters["@SpecificValid"].Value = this.SpecificValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@SpecificValid"].Value = DBNull.Value;
                        }

                        if (this.WorkExperienceValid.HasValue)
                        {
                            cmd.Parameters["@WorkExperienceValid"].Value = this.WorkExperienceValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@WorkExperienceValid"].Value = DBNull.Value;
                        }

                        if (this.AbilityValid.HasValue)
                        {
                            cmd.Parameters["@HabilityValid"].Value = this.AbilityValid.Value;
                        }
                        else
                        {
                            cmd.Parameters["@HabilityValid"].Value = DBNull.Value;
                        }

                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }        
    }
}