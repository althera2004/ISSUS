// --------------------------------
// <copyright file="ProbabilitySeverityRange.cs" company="Sbrinna">
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
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>
    /// Object that represents a probability or a severity with a number (Code) from 1 to 5
    /// </summary>
    public class ProbabilitySeverityRange : BaseItem
    {
        /// <summary>
        /// ProbabilitySeverityType of ProbabilitySeverityRange
        /// </summary>
        public enum ProbabilitySeverityType
        {
            /// <summary>
            /// Specifies that the current item is a probability
            /// </summary>
            Probability = 0,

            /// <summary>
            /// Specifies that the current item is a severity
            /// </summary>
            Severity = 1
        }

        /// <summary>
        /// Gets an empty ProbabilitySeverityRange
        /// </summary>
        public static ProbabilitySeverityRange Empty
        {
            get
            {
                return new ProbabilitySeverityRange()
                {
                    Id = -1,
                    Description = string.Empty,
                    Active = false,
                    CompanyId = -1,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Code = -1,
                    Type = 0
                };
            }
        }

        /*public static string Severities
        {
            get
            {
                StringBuilder res = new StringBuilder("[");
                using (SqlCommand cmd = new SqlCommand("Severity_GetAll"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                    try
                    {
                        cmd.Connection.Open();
                        bool first = true;
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
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
                                    CultureInfo.GetCultureInfo("en-us"),
                                    @"{{""Id"":{0},""Description"":""{1}"",""Code"":{2}}}",
                                    rdr.GetInt64(0),
                                    Tools.JsonCompliant(rdr.GetString(1)),
                                    rdr.GetInt32(2));
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

                res.Append("]");
                return res.ToString();
            }
        }*/

        /// <summary>
        /// Gets or sets the code of ProbabilitySeverityRange
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wheter if ProbabilitySeverityRange is deletable
        /// </summary>
        public ProbabilitySeverityType Type { get; set; }

        /// <summary>
        /// Gets a hyper link to ProbabilitySeverityRange profile page
        /// </summary>
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
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}""}}",
                    this.Id,
                    this.Description);
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}"",""Code"":{2},""Type"":{3}}}",
                    this.Id,
                    this.Description,
                    this.Code,
                    (int)this.Type);
            }
        }

        /// <summary>
        /// Retrieves a collection of the ProbabilitySeverityRange objects on the database
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>ReadOnlyCollection of ProbabilitySeverityRange objects</returns>
        public static ReadOnlyCollection<ProbabilitySeverityRange> GetAll(int companyId)
        {
            return GetAll(companyId, false);
        }

        /// <summary>
        /// Retrieves a collection of the ProbabilitySeverityRange objects on the database
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="isOnlyActive">Set the flag to retrieve only active objects</param>
        /// <returns>ReadOnlyCollection of ProbabilitySeverityRange objects</returns>
        public static ReadOnlyCollection<ProbabilitySeverityRange> GetAll(int companyId, bool isOnlyActive)
        {
            string source = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"GetAll({0}, {1})",
                companyId,
                isOnlyActive);
            List<ProbabilitySeverityRange> res = new List<ProbabilitySeverityRange>();
            string query = isOnlyActive ? "ProbabilitySeverityRange_GetActive" : "ProbabilitySeverityRange_GetAll";
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProbabilitySeverityRange newRange = new ProbabilitySeverityRange()
                        {
                            Id = rdr.GetInt64(ColumnsProbabilitySeverityRangeGetAll.Id),
                            Description = rdr.GetString(ColumnsProbabilitySeverityRangeGetAll.Description),
                            Code = rdr.GetInt32(ColumnsProbabilitySeverityRangeGetAll.Code),
                            Type = (ProbabilitySeverityRange.ProbabilitySeverityType)rdr.GetInt32(ColumnsProbabilitySeverityRangeGetAll.Type),
                            CompanyId = companyId,
                            CreatedOn = rdr.GetDateTime(ColumnsProbabilitySeverityRangeGetAll.CreatedOn),
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsProbabilitySeverityRangeGetAll.CreatedBy),
                                UserName = rdr.GetString(ColumnsProbabilitySeverityRangeGetAll.CreatedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsProbabilitySeverityRangeGetAll.ModifiedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsProbabilitySeverityRangeGetAll.ModifiedBy),
                                UserName = rdr.GetString(ColumnsProbabilitySeverityRangeGetAll.ModifiedByName)
                            },
                            Active = rdr.GetBoolean(ColumnsProbabilitySeverityRangeGetAll.Active),
                        };

                        res.Add(newRange);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NotSupportedException ex)
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

            return new ReadOnlyCollection<ProbabilitySeverityRange>(res);
        }

        /// <summary>
        /// Retrieves a collection of the active ProbabilitySeverityRange objects on the database
        /// </summary>
        /// <param name="companyId">Company Identifier</param>
        /// <returns>ReadOnlyCollection of ProbabilitySeverityRange objects</returns>
        public static ReadOnlyCollection<ProbabilitySeverityRange> GetActive(int companyId)
        {
            return new ReadOnlyCollection<ProbabilitySeverityRange>(GetAll(companyId, true));
        }
    }
}
