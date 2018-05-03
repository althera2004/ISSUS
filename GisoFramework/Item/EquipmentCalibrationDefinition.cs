// --------------------------------
// <copyright file="EquipmentCalibrationDefinition.cs" company="Sbrinna">
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

    /// <summary>Implements EquipmentCalibrationDefinition class</summary>
    public class EquipmentCalibrationDefinition : BaseItem
    {
        public static EquipmentCalibrationDefinition Empty
        {
            get
            {
                return new EquipmentCalibrationDefinition
                {
                    Id = 0,
                    Description = string.Empty
                };
            }
        }

        public long EquipmentId { get; set; }

        public int CalibrationType { get; set; }

        [DifferenciableAttribute]
        public int Periodicity { get; set; }

        [DifferenciableAttribute]
        public decimal Uncertainty { get; set; }

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

        /*
        public override string Differences(BaseItem item1)
        {
            if(item1 == null)
            {
                return string.Empty;
            }

            EquipmentCalibrationDefinition item = item1 as EquipmentCalibrationDefinition;
            StringBuilder res = new StringBuilder();
            if (item.Description != this.Description)
            {
                res.Append("Description:").Append(this.Description).Append(";");
            }
            if (item.Cost != this.Cost)
            {
                res.Append("Cost:").Append(this.Cost).Append(";");
            }
            if (item.Pattern != this.Pattern)
            {
                res.Append("Pattern:").Append(this.Pattern).Append(";");
            }
            if (item.Periodicity != this.Periodicity)
            {
                res.Append("Periodicity:").Append(this.Periodicity).Append(";");
            }
            if (item.Range != this.Range)
            {
                res.Append("Range:").Append(this.Range).Append(";");
            }
            if (item.Provider != this.Provider)
            {
                res.Append("Provider:").Append(this.Provider.Description).Append(";");
            }
            if (item.Range != this.Range)
            {
                res.Append("Range:").Append(this.Range).Append(";");
            }
            if (item.Responsible != this.Responsible)
            {
                res.Append("Responsible:").Append(this.Responsible.FullName).Append(";");
            }

            return res.ToString();
        }
        */
        public override string Link
        {
            get { return string.Empty; }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get { return string.Empty; }
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

                var res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",");
                res.Append(Tools.JsonPair("EquipmentId", this.EquipmentId)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("CalibrationType", this.CalibrationType)).Append(",");
                res.Append(Tools.JsonPair("Operation", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Periodicity", this.Periodicity)).Append(",");
                res.Append(Tools.JsonPair("Uncertainty", this.Uncertainty, 6)).Append(",");
                res.Append(Tools.JsonPair("Range", this.Range)).Append(",");
                res.Append(Tools.JsonPair("Pattern", this.Pattern)).Append(",");
                res.Append(Tools.JsonPair("Cost", this.Cost)).Append(",");
                res.Append(Tools.JsonPair("Notes", this.Notes)).Append(",");
                res.Append("\"Provider\":").Append(this.Provider.JsonKeyValue).Append(",");
                res.Append("\"Responsible\":").Append(this.Responsible.JsonSimple);
                res.Append("}");
                return res.ToString();
            }
        }

        public ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE EquipmentCalibrationDefinition_Insert
             *   @EquipmentCalibrationDefinitionId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @CalibrationType int,
             *   @Periodicity int,
             *   @Uncertainty numeric(18,0),
             *   @Range nvarchar(50),
             *   @Pattern nvarchar(50),
             *   @Cost numeric(18,3),
             *   @Notes text,
             *   @Responsable int,
             *   @Provider int,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentCalibrationDefinition_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentCalibrationDefinitionId"));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@CalibrationType", this.CalibrationType));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@Uncertainty", this.Uncertainty));
                        cmd.Parameters.Add(DataParameter.Input("@Range", this.Range));
                        cmd.Parameters.Add(DataParameter.Input("@Pattern", this.Pattern));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes));
                        cmd.Parameters.Add(DataParameter.Input("@Responsable", this.Responsible.Id));
                        if (this.Provider == null)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@Provider"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@Provider", this.Provider.Id));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentCalibrationDefinitionId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        res.Success = true;
                        res.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"EquipmentCalibrationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description));
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

        public ActionResult Update(int userId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"EquipmentCalibrationDefinition::Update Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description);
            /* CREATE PROCEDURE EquipmentCalibrationDefinition_Update
             *   @EquipmentCalibrationDefinitionId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @CalibrationType int,
             *   @Periodicity int,
             *   @Uncertainty numeric(18,0),
             *   @Range nvarchar(50),
             *   @Pattern nvarchar(50),
             *   @Cost numeric(18,3),
             *   @Notes text,
             *   @Responsable int,
             *   @Provider int,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentCalibrationDefinition_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentCalibrationDefinitionId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.EquipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Operation", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@CalibrationType", this.CalibrationType));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@Uncertainty", this.Uncertainty));
                        cmd.Parameters.Add(DataParameter.Input("@Range", this.Range));
                        cmd.Parameters.Add(DataParameter.Input("@Pattern", this.Pattern));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes));
                        cmd.Parameters.Add(DataParameter.Input("@Responsable", this.Responsible.Id));
                        if (this.Provider == null)
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@Provider"));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.Input("@Provider", this.Provider.Id));
                        }

                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
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

        public ActionResult Delete(int userId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"EquipmentCalibrationDefinition::Delete Id:{0} User:{1} Company:{2} Description:""{3}""", this.Id, userId, this.CompanyId, this.Description);
            /* CREATE PROCEDURE EquipmentCalibrationDefinition_Delete
             *   @EquipmentCalibrationDefinitionId bigint,
             *   @UserId int */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("EquipmentCalibrationDefinition_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentCalibrationDefinitionId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
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