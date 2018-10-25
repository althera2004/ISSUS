// --------------------------------
// <copyright file="equipment.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements Equipment class</summary>
    public class Equipment : BaseItem
    {
        public static Equipment Empty
        {
            get
            {
                return new Equipment
                {
                    Id = 0,
                    Responsible = Employee.EmptySimple,
                    MeasureUnit = EquipmentScaleDivision.Empty,
                    InternalCalibration = EquipmentCalibrationDefinition.Empty,
                    ExternalCalibration = EquipmentCalibrationDefinition.Empty,
                    InternalVerification = EquipmentVerificationDefinition.Empty,
                    ExternalVerification = EquipmentVerificationDefinition.Empty,
                    Image = "/images/noimage.jpg",
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        [DifferenciableAttribute]
        public string Code { get; set; }

        [DifferenciableAttribute]
        public string Trademark { get; set; }

        [DifferenciableAttribute]
        public string Model { get; set; }

        [DifferenciableAttribute]
        public string SerialNumber { get; set; }

        [DifferenciableAttribute]
        public string Location { get; set; }

        [DifferenciableAttribute]
        public string MeasureRange { get; set; }

        [DifferenciableAttribute]
        public decimal? ScaleDivisionValue { get; set; }

        /*[DifferenciableAttribute]
        public decimal? ScaleDivision { get; set; }*/

        [DifferenciableAttribute]
        public EquipmentScaleDivision MeasureUnit { get; set; }

        [DifferenciableAttribute]
        public Employee Responsible { get; set; }

        [DifferenciableAttribute]
        public bool IsCalibration { get; set; }

        [DifferenciableAttribute]
        public bool IsVerification { get; set; }

        [DifferenciableAttribute]
        public bool IsMaintenance { get; set; }

        [DifferenciableAttribute]
        public string Observations { get; set; }

        public EquipmentCalibrationDefinition InternalCalibration { get; set; }

        public EquipmentCalibrationDefinition ExternalCalibration { get; set; }

        public EquipmentVerificationDefinition InternalVerification { get; set; }

        public EquipmentVerificationDefinition ExternalVerification { get; set; }

        [DifferenciableAttribute]
        public string Notes { get; set; }

        [DifferenciableAttribute]
        public string Image { get; set; }

        [DifferenciableAttribute]
        public DateTime? StartDate { get; set; }

        [DifferenciableAttribute]
        public DateTime? EndDate { get; set; }

        [DifferenciableAttribute]
        public Employee EndResponsible { get; set; }

        [DifferenciableAttribute]
        public string EndReason { get; set; }

        public bool HasAttachments { get; set; }

        public decimal TotalCost { get; set; }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var res = new StringBuilder("{").Append(Environment.NewLine);
                res.Append("\t\"Id\":").Append(this.Id).Append(",").Append(Environment.NewLine);
                res.Append("\t\"CompanyId\":").Append(this.CompanyId).Append(",").Append(Environment.NewLine);
                res.Append("\t\"Code\":\"").Append(Tools.JsonCompliant(this.Code)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"Description\":\"").Append(Tools.JsonCompliant(this.Description)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"TradeMark\":\"").Append(Tools.JsonCompliant(this.Trademark)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"Model\":\"").Append(Tools.JsonCompliant(this.Model)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"SerialNumber\":\"").Append(Tools.JsonCompliant(this.SerialNumber)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"Location\":\"").Append(Tools.JsonCompliant(this.Location)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"MeasureRange\":\"").Append(Tools.JsonCompliant(this.MeasureRange)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"ScaleDivision\":\"").Append(this.ScaleDivisionValue.HasValue ? this.ScaleDivisionValue.Value.ToString("#0.0000", CultureInfo.GetCultureInfo("en-us")) : string.Empty).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"MeasureUnit\":").Append(this.MeasureUnit == null ? "null" : this.MeasureUnit.JsonKeyValue).Append(",").Append(Environment.NewLine);
                res.Append("\t\"Responsible\":").Append(this.Responsible.JsonSimple).Append(",").Append(Environment.NewLine);
                res.Append("\t\"IsCalibration\":").Append(this.IsCalibration ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("\t\"IsVerification\":").Append(this.IsVerification ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("\t\"IsMaintenance\":").Append(this.IsMaintenance ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("\t\"Notes\":\"").Append(Tools.JsonCompliant(this.Notes)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"Image\":\"").Append(Tools.JsonCompliant(this.Image)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"Observations\":\"").Append(Tools.JsonCompliant(this.Observations)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\"InternalCalibration\":").Append(this.InternalCalibration.Json).Append(",").Append(Environment.NewLine);
                res.Append("\t\"ExternalCalibration\":").Append(this.ExternalCalibration.Json).Append(",").Append(Environment.NewLine);
                res.Append("\t\"InternalVerification\":").Append(this.InternalVerification.Json).Append(",").Append(Environment.NewLine);
                res.Append("\t\"ExternalVerification\":").Append(this.ExternalVerification.Json).Append(",").Append(Environment.NewLine);
                
                res.Append("\t").Append(Tools.JsonPair("StartDate", this.StartDate)).Append(",").Append(Environment.NewLine);
                res.Append("\t").Append(Tools.JsonPair("EndDate", this.EndDate)).Append(",").Append(Environment.NewLine);
                res.Append("\t").Append(Tools.JsonPair("EndResponsible", this.EndResponsible)).Append(",").Append(Environment.NewLine);
                res.Append("\t").Append(Tools.JsonPair("EndReason", this.EndReason)).Append(",").Append(Environment.NewLine);

                res.Append("\t").Append(Tools.JsonPair("Active", this.Active)).Append(Environment.NewLine);
                res.Append("};");
                return res.ToString();
            }
        }

        public string CodeLink
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""EquipmentView.aspx?id={0}"">{1}</a>",
                    this.Id,
                    this.Code);
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""EquipmentView.aspx?id={0}"">{1}</a>",
                    this.Id,
                    this.Description);
            }
        }

        public string FullName
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", this.Code, this.Description);
            }
        }

        public static Equipment ById(long equipmentId, Company company)
        {
            return ById(equipmentId, company.Id);
        }

        public static Equipment ById(long equipmentId, int companyId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Equipment::GetById(Id:{0}, CompanyId:{1})", equipmentId, companyId);
            /* CREATE PROCEDURE Equipment_GetById
             *   @EquipmentId bigint,
             *   @CompanyId int */
            var res = Equipment.Empty;
            using (var cmd = new SqlCommand("Equipment_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", equipmentId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = new Equipment
                                {
                                    Id = equipmentId,
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentGetById.CompanyId),
                                    Code = rdr.GetString(ColumnsEquipmentGetById.Code),
                                    Description = rdr[ColumnsEquipmentGetById.Description].ToString(),
                                    Trademark = rdr[ColumnsEquipmentGetById.Trademark].ToString(),
                                    Model = rdr[ColumnsEquipmentGetById.Model].ToString(),
                                    SerialNumber = rdr[ColumnsEquipmentGetById.SerialNumber].ToString(),
                                    Location = rdr[ColumnsEquipmentGetById.Location].ToString(),
                                    MeasureRange = rdr.IsDBNull(ColumnsEquipmentGetById.MeasureRange) ? null : rdr[ColumnsEquipmentGetById.MeasureRange].ToString(),
                                    MeasureUnit = rdr.IsDBNull(ColumnsEquipmentGetById.ScaleDivisionId) ? null : new EquipmentScaleDivision()
                                    {
                                        Id = rdr.GetInt64(ColumnsEquipmentGetById.ScaleDivisionId),
                                        Description = rdr.GetString(ColumnsEquipmentGetById.ScaleDivisionDescription),
                                    },
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentGetById.ResponsibleId),
                                        Name = rdr[ColumnsEquipmentGetById.ResponsibleName].ToString(),
                                        LastName = rdr[ColumnsEquipmentGetById.ResponsibleLastName].ToString(),
                                        CompanyId = rdr.GetInt32(ColumnsEquipmentGetById.CompanyId)
                                    },
                                    IsCalibration = rdr.GetBoolean(ColumnsEquipmentGetById.IsCalibration),
                                    IsVerification = rdr.GetBoolean(ColumnsEquipmentGetById.IsVerification),
                                    IsMaintenance = rdr.GetBoolean(ColumnsEquipmentGetById.IsMaintenance),
                                    Notes = rdr.GetString(ColumnsEquipmentGetById.Notes),
                                    Observations = rdr[ColumnsEquipmentGetById.Observations].ToString(),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentGetById.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentGetById.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentGetById.ModifiedOn)
                                };

                                if (rdr.IsDBNull(ColumnsEquipmentGetById.ScaleDivisionValue))
                                {
                                    res.ScaleDivisionValue = null;
                                }
                                else
                                {
                                    res.ScaleDivisionValue = rdr.GetDecimal(ColumnsEquipmentGetById.ScaleDivisionValue);
                                }

                                if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + @"images\equipments\" + res.Id.ToString(CultureInfo.GetCultureInfo("en-us")) + ".jpg"))
                                {
                                    res.Image = res.Id + ".jpg";
                                }
                                else
                                {
                                    res.Image = "noimage.jpg";
                                }

                                if (!rdr.IsDBNull(ColumnsEquipmentGetById.StartDate))
                                {
                                    res.StartDate = rdr.GetDateTime(ColumnsEquipmentGetById.StartDate);
                                }

                                if (!rdr.IsDBNull(ColumnsEquipmentGetById.EndDate))
                                {
                                    res.EndDate = rdr.GetDateTime(ColumnsEquipmentGetById.EndDate);
                                }

                                if (!rdr.IsDBNull(ColumnsEquipmentGetById.EndResponsible))
                                {
                                    res.EndResponsible = new Employee
                                    {
                                        Id = Convert.ToInt64(rdr.GetInt32(ColumnsEquipmentGetById.EndResponsible)),
                                        Name = rdr.GetString(ColumnsEquipmentGetById.EndResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentGetById.EndResponsibleLastName)
                                    };
                                }
                                else
                                {
                                    res.EndResponsible = Employee.Empty;
                                }

                                if (!rdr.IsDBNull(ColumnsEquipmentGetById.EndReason))
                                {
                                    res.EndReason = rdr.GetString(ColumnsEquipmentGetById.EndReason);
                                }

                                res.ObtainCalibrationDefinitions();
                                res.ObtainVerificationDefinitions();

                                res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
                            }
                        }
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
                    catch (InvalidCastException ex)
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

        public static string GetListJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var equipment in ByCompany(companyId))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.AppendFormat(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                    ""Codigo"":""{1}"",
                    ""Descripcion"":""{2}"",
                    ""Ubicacion"":""{3}"",
                    ""Responsable"": {{""Id"":{4},""FullName"":""{5}""}},
                    ""Adjuntos"":{6},
                    ""Calibracion"":{7},
                    ""Verificacion"":{8},
                    ""Mantenimiento"":{9},
                    ""Activo"":{10},
                    ""Coste"": {11}}}",
                    equipment.Id,
                    equipment.Code,
                    Tools.JsonCompliant(equipment.Description),
                    Tools.JsonCompliant(equipment.Location),
                    equipment.Responsible.Id,
                    Tools.JsonCompliant(equipment.Responsible.FullName),
                    equipment.HasAttachments ? "true": "false",
                    equipment.IsCalibration ? "true" : "false",
                    equipment.IsVerification ? "true" : "false",
                    equipment.IsMaintenance ? "true" : "false",
                    equipment.EndDate.HasValue ? "false" : "true",
                    equipment.TotalCost);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<Equipment> ByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<Equipment>(new List<Equipment>());
            }

            return ByCompany(company.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns></returns>
        public static ReadOnlyCollection<Equipment> ByCompany(int companyId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Equipment::GetList({0})", companyId);
            var res = new List<Equipment>();
            /* CREATE PROCEDURE Equipment_GetList
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Equipment_GetList"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var equipment = new Equipment
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentGetList.Id),
                                    Code = rdr.GetString(ColumnsEquipmentGetList.Code),
                                    Description = rdr.GetString(ColumnsEquipmentGetList.Description),
                                    Location = rdr.GetString(ColumnsEquipmentGetList.Location),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentGetList.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentGetList.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentGetList.ResponsibleLastName)
                                    },
                                    IsCalibration = rdr.GetBoolean(ColumnsEquipmentGetList.IsCalibration),
                                    IsVerification = rdr.GetBoolean(ColumnsEquipmentGetList.IsVerification),
                                    IsMaintenance = rdr.GetBoolean(ColumnsEquipmentGetList.IsMaintenance),
                                    HasAttachments = rdr.GetBoolean(ColumnsEquipmentGetList.HasAttachments),
                                    TotalCost = 0
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentGetList.EndDate))
                                {
                                    equipment.EndDate = rdr.GetDateTime(ColumnsEquipmentGetList.EndDate);
                                }

                                res.Add(equipment);
                            }
                        }
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
                    catch (InvalidCastException ex)
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

            return new ReadOnlyCollection<Equipment>(res);
        }

        /*/// <summary>Get a list of company's equipments</summary>
        /// <param name="company">Equipment's company</param>
        /// <param name="grantWrite">User grant to write</param>
        /// <param name="grantDelete">User grant to delete</param>
        /// <param name="grantEmployee">User grant to employees</param>
        /// <param name="dictionary">Interface dictionary</param>
        /// <returns>List of company's equipments</returns>
        public static string List(Company company, bool grantWrite, bool grantDelete, bool grantEmployee, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            return List(Equipment.ByCompany(company), grantWrite, grantDelete, grantEmployee, dictionary);
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="grantWrite">User grant to write</param>
        /// <param name="grantDelete">User grant to delete equipments</param>
        /// <param name="grantEmployee">User grant to employees</param>
        /// <param name="dictionary">Interface dictionary</param>
        /// <returns></returns>
        public static string List(ReadOnlyCollection<Equipment> list, bool grantWrite, bool grantDelete, bool grantEmployee, Dictionary<string, string> dictionary)
        {
            if (list == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            var res = new StringBuilder();
            foreach (var equipment in list)
            {
                res.Append(equipment.ListRow(dictionary, grantWrite, grantDelete, grantEmployee));
            }

            return res.ToString();
        }

        public static ActionResult Restore(int equipmentId, int companyId, int applicationUserId)
        {
            string source = string.Format(
               CultureInfo.InvariantCulture,
               @"Equipment::Restore({0}, {1})",
               equipmentId,
               applicationUserId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Equipment_Restore]
             *   @EquipmentId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Equipment_Restore"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", equipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(equipmentId);
                    }
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
                catch (InvalidCastException ex)
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

            return res;
        }

        public static ActionResult Anulate(int equipmentId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Equipment::Anulate({0}, {1})",
                equipmentId,
                applicationUserId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Equipment_Anulate]
             *   @EquipmentId int,
             *   @CompanyId int,
             *   @EndDate datetime,
             *   @EndReason nvarchar(500),
             *   @EndResponsable int,
             *   @UnidadId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Equipment_Anulate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", equipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", date));
                        cmd.Parameters.Add(DataParameter.Input("@EndReason", reason, 500));
                        cmd.Parameters.Add(DataParameter.Input("@EndResponsible", responsible));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(equipmentId);
                    }
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
                catch (InvalidCastException ex)
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

            return res;
        }

        public ActionResult Delete(int userId, string reason)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"Equipment::Delete Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Equipment_Delete
             *   @EquipmentId bigint,
             *   @Reason nvarchar(50),
             *   @UserId int,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Equipment_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", reason));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex.Message);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex.Message);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        res.SetFail(ex.Message);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        res.SetFail(ex.Message);
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex.Message);
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

        public ActionResult Update(int userId, string trace)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Equipment_Update
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Code nvarchar(50),
             *   @Description nvarchar(50),
             *   @TradeMark nvarchar(50),
             *   @Model nvarchar(50),
             *   @SerialNumber nvarchar(50),
             *   @Location varchar(50),
             *   @MeasureRange nvarchar(50),
             *   @ScaleDivision numeric(18,3),
             *   @MeasureUnit bigint,
             *   @Responsable int,
             *   @IsCalibration bit,
             *   @IsVerification bit,
             *   @IsMaintenance bit,
             *   @Observations nvarchar(255),
             *   @Notes text,
             *   @Image nvarchar(50),
             *   @Active bit,
             *   @UserId int,
             *   @Trace nvarchar(50)
             *   @StartDate datetime */
            using (var cmd = new SqlCommand("Equipment_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Code", this.Code));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@TradeMark", this.Trademark, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Model", this.Model, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@SerialNumber", this.SerialNumber, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Location", this.Location, Constant.DefaultDatabaseVarChar));
                        cmd.Parameters.Add(DataParameter.Input("@MeasureRange", string.IsNullOrEmpty(this.MeasureRange) ? string.Empty : this.MeasureRange));
                        cmd.Parameters.Add(DataParameter.Input("@ScaleDivision", this.ScaleDivisionValue));
                        cmd.Parameters.Add(DataParameter.Input("@MeasureUnit", this.MeasureUnit.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Responsable", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@IsCalibration", this.IsCalibration));
                        cmd.Parameters.Add(DataParameter.Input("@IsVerification", this.IsVerification));
                        cmd.Parameters.Add(DataParameter.Input("@IsMaintenance", this.IsMaintenance));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 500));
                        cmd.Parameters.Add(DataParameter.Input("@Active", this.Active));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@Trace", trace));
                        cmd.Parameters.Add(DataParameter.Input("@StartDate", this.StartDate));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex.Message);
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

        public ActionResult Insert(int userId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, @"EquipmentVerificationAct::Insert Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Equipment_Insert
             *   @EquipmentId bigint output,
             *   @CompanyId int,
             *   @Code nvarchar(50),
             *   @Description nvarchar(150),
             *   @TradeMark nvarchar(50),
             *   @Model nvarchar(50),
             *   @SerialNumber nvarchar(50),
             *   @Location nvarchar(50),
             *   @MeasureRange nvarchar(50),
             *   @ScaleDivision numeric(18,3),
             *   @MeasureUnit bigint,
             *   @Responsable int,
             *   @IsCalibration bit,
             *   @IsVerification bit,
             *   @IsMaintenance bit,
             *   @Observations nvarchar(255),
             *   @Active bit,
             *   @UserId int,
             *   @Notes text
             *   @StartDate datetime */
            using (var cmd = new SqlCommand("Equipment_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@EquipmentId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Code", this.Code));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@TradeMark", this.Trademark, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Model", this.Model, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@SerialNumber", this.SerialNumber, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Location", this.Location, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@MeasureRange", string.IsNullOrEmpty(this.MeasureRange) ? string.Empty : this.MeasureRange));
                        cmd.Parameters.Add(DataParameter.Input("@ScaleDivision", this.ScaleDivisionValue));
                        cmd.Parameters.Add(DataParameter.Input("@MeasureUnit", this.MeasureUnit.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Responsable", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@IsCalibration", this.IsCalibration));
                        cmd.Parameters.Add(DataParameter.Input("@IsVerification", this.IsVerification));
                        cmd.Parameters.Add(DataParameter.Input("@IsMaintenance", this.IsMaintenance));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.Notes, 500));
                        cmd.Parameters.Add(DataParameter.Input("@StartDate", this.StartDate));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@EquipmentId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        res.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));

                        if (this.InternalVerification.Id > 0)
                        {
                            this.InternalVerification.EquipmentId = this.Id;
                            res = this.InternalVerification.Insert(userId);
                        }

                        if (res.Success && this.ExternalVerification.Id > 0)
                        {
                            this.ExternalVerification.EquipmentId = this.Id;
                            res = this.ExternalVerification.Insert(userId);
                        }

                        if (res.Success && this.InternalCalibration.Id > 0)
                        {
                            this.InternalCalibration.EquipmentId = this.Id;
                            res = this.InternalCalibration.Insert(userId);
                        }

                        if (res.Success && this.ExternalCalibration.Id > 0)
                        {
                            this.ExternalCalibration.EquipmentId = this.Id;
                            res = this.ExternalCalibration.Insert(userId);
                        }
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

        public void ObtainVerificationDefinitions()
        {
            /* CREATE PROCEDURE Equipment_GetVerificationDefinition
             *   @EquipmentId bigint,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Equipment_GetVerificationDefinition"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var verification = new EquipmentVerificationDefinition
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentGetVerificationDefinition.Id),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentGetVerificationDefinition.EquipmentId),
                                    CompanyId = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.CompanyId),
                                    VerificationType = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.VerificationType),
                                    Description = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.Operation),
                                    Periodicity = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.Periodicity),
                                    Pattern = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.Pattern),
                                    Range = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.Range),
                                    Notes = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.Notes),
                                    Responsible = new Employee
                                    {
                                        CompanyId = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.CompanyId),
                                        Id = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.ResponsibleLastName)
                                    },
                                    Provider = new Provider
                                    {
                                        CompanyId = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.CompanyId),
                                        Id = rdr.GetInt64(ColumnsEquipmentGetVerificationDefinition.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.ProviderDescription)
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        CompanyId = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.CompanyId),
                                        Id = rdr.GetInt32(ColumnsEquipmentGetVerificationDefinition.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentGetVerificationDefinition.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentGetVerificationDefinition.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentGetVerificationDefinition.Cost))
                                {
                                    verification.Cost = rdr.GetDecimal(ColumnsEquipmentGetVerificationDefinition.Cost);
                                }

                                if (!rdr.IsDBNull(ColumnsEquipmentGetVerificationDefinition.Uncertainty))
                                {
                                    verification.Uncertainty = rdr.GetDecimal(ColumnsEquipmentGetVerificationDefinition.Uncertainty);
                                }

                                if (verification.VerificationType == 0)
                                {
                                    this.InternalVerification = verification;
                                }
                                else
                                {
                                    this.ExternalVerification = verification;
                                }
                            }
                        }

                        if (this.InternalVerification == null)
                        {
                            this.InternalVerification = EquipmentVerificationDefinition.Empty;
                        }

                        if (this.ExternalVerification == null)
                        {
                            this.ExternalVerification = EquipmentVerificationDefinition.Empty;
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetVerificationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetVerificationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetVerificationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetVerificationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetVerificationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId));
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetVerificationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId));
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

        public void ObtainCalibrationDefinitions()
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Equipment::GetCalibrationDefinitions(Id:{0}, CompanyId:{1})", this.Id, this.CompanyId);
            /* CREATE PROCEDURE Equipment_GetCalibrationDefinition
             *   @EquipmentId bigint,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Equipment_GetCalibrationDefinition"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                int companyId = rdr.GetInt32(ColumnsEquipmentGetCalibrationDefinition.CompanyId);
                                var calibration = new EquipmentCalibrationDefinition
                                {
                                    Id = rdr.GetInt64(ColumnsEquipmentGetCalibrationDefinition.Id),
                                    EquipmentId = rdr.GetInt64(ColumnsEquipmentGetCalibrationDefinition.EquipmentId),
                                    CompanyId = companyId,
                                    CalibrationType = rdr.GetInt32(ColumnsEquipmentGetCalibrationDefinition.CalibrationType),
                                    Description = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.Operation),
                                    Periodicity = rdr.GetInt32(ColumnsEquipmentGetCalibrationDefinition.Periodicity),
                                    Pattern = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.Pattern),
                                    Range = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.Range),
                                    Uncertainty = rdr.GetDecimal(ColumnsEquipmentGetCalibrationDefinition.Uncertainty),
                                    Notes = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.Notes),
                                    Provider = new Provider
                                    {
                                        CompanyId = companyId,
                                        Id = rdr.GetInt64(ColumnsEquipmentGetCalibrationDefinition.ProviderId),
                                        Description = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.ProviderDescription)
                                    },
                                    Responsible = new Employee
                                    {
                                        CompanyId = companyId,
                                        Id = rdr.GetInt32(ColumnsEquipmentGetCalibrationDefinition.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.ResponsibleLastName)
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        CompanyId = companyId,
                                        Id = rdr.GetInt32(ColumnsEquipmentGetCalibrationDefinition.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsEquipmentGetCalibrationDefinition.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsEquipmentGetCalibrationDefinition.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentGetCalibrationDefinition.Cost))
                                {
                                    calibration.Cost = rdr.GetDecimal(ColumnsEquipmentGetCalibrationDefinition.Cost);
                                }

                                if (calibration.CalibrationType == 0)
                                {
                                    this.InternalCalibration = calibration;
                                }
                                else
                                {
                                    this.ExternalCalibration = calibration;
                                }
                            }
                        }

                        if (this.InternalCalibration == null)
                        {
                            this.InternalCalibration = EquipmentCalibrationDefinition.Empty;
                        }

                        if (this.ExternalCalibration == null)
                        {
                            this.ExternalCalibration = EquipmentCalibrationDefinition.Empty;
                        }
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
                    catch (InvalidCastException ex)
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
        }

        /// <summary>Generates HTML code for a Equipment row</summary>
        /// <param name="dictionary">dictionary for fixed labels</param>
        /// <param name="grantWrite">Indicates grant to write</param>
        /// <param name="grantEquipment">Indicates grants for Equipment item</param>
        /// <param name="grantEmployee">Indicates grants for Employee item</param>
        /// <returns>HTML code for a Equipment row</returns>
        public string ListRow(Dictionary<string, string> dictionary, bool grantWrite, bool grantEquipment, bool grantEmployee)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool attach = UploadFile.HasAttachemnts(11, this.Id, this.CompanyId);
            string iconEditAlt = grantEquipment ? dictionary["Common_Edit"] : dictionary["Common_View"];
            string iconAttach = string.Empty;

            if(attach)
            {
                iconAttach = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<i class=""icon-paperclip"" title=""{4}"" style=""cursor:pointer;"" onclick=""document.location='EquipmentView.aspx?id={0}&Tab=TabuploadFiles';""></i>{1}{2}{3}",
                    this.Id,
                    this.IsCalibration ? "c" : string.Empty,
                    this.IsVerification ? "v" : string.Empty,
                    this.IsMaintenance ? "m" : string.Empty,
                    dictionary["Item_Equipment_Message_Attachs"]);
            }

            string iconEditFigure = grantWrite ? "icon-edit" : "icon-eye-open";

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""document.location='EquipmentView.aspx?id={0}';""><i class=""{3} bigger-120""></i></span>",
                this.Id,
                Tools.SetTooltip(this.Description),
                Tools.JsonCompliant(iconEditAlt),
                iconEditFigure);

            string iconDelete = string.Empty;
            if (grantEquipment)
            {
                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""EquipmentDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>",
                    this.Id,
                    Tools.SetTooltip(this.Description),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"<tr><td class=""hidden-480"" style=""width:110px;"">{0}</td><td>{1}</td><td class=""hidden-480"" style=""width:120px;"">{2}</td><td class=""hidden-480"" style=""width:250px;"">{3}</td><td class=""hidden-480"" style=""width:35px;text-align:center;"">{7}</td><td style=""width:90px;"">{4}&nbsp;{5}</td></tr>{6}",
                this.CodeLink,
                this.Link,
                Tools.SetHtml(this.Location),
                grantEmployee ? this.Responsible.Link : this.Responsible.FullName,
                iconEdit,
                iconDelete,
                Environment.NewLine,
                iconAttach);
        }
    }
}