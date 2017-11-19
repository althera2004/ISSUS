// --------------------------------
// <copyright file="EquipmentVerificationDefinition.cs" company="Sbrinna">
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

    /// <summary>
    /// Implements EquipmentVerificationDefinition class
    /// </summary>
    public class EquipmentVerificationDefinition : BaseItem
    {
        public long EquipmentId { get; set; }
        [DifferenciableAttribute]
        public int VerificationType { get; set; }
        [DifferenciableAttribute]
        public string Description { get; set; }
        [DifferenciableAttribute]
        public int Periodicity { get; set; }
        [DifferenciableAttribute]
        public decimal? Uncertainty { get; set; }
        [DifferenciableAttribute]
        public string Range { get; set; }
        [DifferenciableAttribute]
        public string Pattern { get; set; }
        [DifferenciableAttribute]
        public decimal? Cost { get; set; }
        [DifferenciableAttribute]
        public string Notes { get; set; }
        [DifferenciableAttribute]
        public Employee Responsible { get; set; }
        [DifferenciableAttribute]
        public Provider Provider { get; set; }

        public static EquipmentVerificationDefinition Empty
        {
            get
            {
                return new EquipmentVerificationDefinition()
                {
                    Id = 0,
                    Description = string.Empty,
                    Responsible = Employee.Empty,
                    Provider = Provider.Empty
                };
            }
        }

        public override string Link
        {
            get { return string.Empty; }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                if (this.Id == 0)
                {
                    return "null";
                }

                StringBuilder res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",");
                res.Append(Tools.JsonPair("EquipmentId", this.EquipmentId)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("CalibrationType", this.VerificationType)).Append(",");
                res.Append(Tools.JsonPair("Operation", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Periodicity", this.Periodicity)).Append(",");
                res.Append(Tools.JsonPair("Uncertainty", this.Uncertainty, 6)).Append(",");
                res.Append(Tools.JsonPair("Range", this.Range)).Append(",");
                res.Append(Tools.JsonPair("Pattern", this.Pattern)).Append(",");
                res.Append(Tools.JsonPair("Cost", this.Cost)).Append(",");
                res.Append(Tools.JsonPair("Notes", this.Notes)).Append(","); ;
                res.Append("\"Provider\":").Append(this.Provider.JsonKeyValue).Append(",");
                res.Append("\"Responsible\":").Append(this.Responsible.JsonSimple);
                res.Append("}");
                return res.ToString();
            }
        }

        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EquipmentVerificationDefinition_Insert
             *   @EquipmentVerificationDefinitionId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @VerificationType int,
             *   @Periodicity int,
             *   @Uncertainty numeric(18,0),
             *   @Range nvarchar(50),
             *   @Pattern nvarchar(50),
             *   @Cost numeric(18,3),
             *   @Notes text,
             *   @Responsable int,
             *   @ProviderId bigint,
             *   @Provider int,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("EquipmentVerificationDefinition_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentVerificationDefinitionId"));
                    cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@VerificationType", this.VerificationType));
                    cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                    cmd.Parameters.Add(DataParameter.Input("@Uncertainty", this.Uncertainty));
                    cmd.Parameters.Add(DataParameter.Input("@Range", this.Range));
                    cmd.Parameters.Add(DataParameter.Input("@Pattern", this.Pattern));
                    cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes));
                    cmd.Parameters.Add(DataParameter.Input("@Responsable", this.Responsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider));

                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentVerificationDefinitionId"].Value, CultureInfo.GetCultureInfo("en-us"));
                    res.Success = true;
                    res.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
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

        public ActionResult Update(int userId)
        {
            /* CREATE PROCEDURE EquipmentVerificationDefinition_Update
             *   @EquipmentVerificationDefinitionId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @VerificationType int,
             *   @Periodicity int,
             *   @Uncertainty numeric(18,0),
             *   @Range nvarchar(50),
             *   @Pattern nvarchar(50),
             *   @Cost numeric(18,3),
             *   @Notes text,
             *   @Responsable int,
             *   @ProviderId bigint,
             *   @Provider int,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("EquipmentVerificationDefinition_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@EquipmentVerificationDefinitionId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@VerificationType", this.VerificationType));
                    cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                    cmd.Parameters.Add(DataParameter.Input("@Uncertainty", this.Uncertainty));
                    cmd.Parameters.Add(DataParameter.Input("@Range", this.Range));
                    cmd.Parameters.Add(DataParameter.Input("@Pattern", this.Pattern));
                    cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                    cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes));
                    cmd.Parameters.Add(DataParameter.Input("@Responsable", this.Responsible.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Provider));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
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

        public ActionResult Delete(int userId)
        {
            /* CREATE PROCEDURE EquipmentVerificationDefinition_Delete
             *   @EquipmentVerificationDefinitionId bigint,
             *   @UserId int */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("EquipmentVerificationDefinition_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@EquipmentVerificationDefinitionId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Delete Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Delete Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Delete Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Delete Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentVerificationDefinition::Delete Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
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
}
