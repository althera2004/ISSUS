// --------------------------------
// <copyright file="Revision.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item.Binding;

namespace GisoFramework.Item
{
    public class Revision : BaseItem
    {
        [DifferenciableAttribute]
        public int RevisionType { get; set; }

        [DifferenciableAttribute]
        public int MonthDay { get; set; }

        [DifferenciableAttribute]
        public int MonthDayOrder { get; set; }

        [DifferenciableAttribute]
        public int MonthDayWeek { get; set; }

        [DifferenciableAttribute]
        public string WeekDays { get; set; }

        [DifferenciableAttribute]
        public int DaysPeriode { get; set; }

        [DifferenciableAttribute]
        public bool Laboral { get; set; }

        public static Revision Empty
        {
            get{
                return new Revision()
                {
                    Id = 0,
                    RevisionType = 0,
                    MonthDayOrder = 0,
                    MonthDay = 0,
                    MonthDayWeek = 0,
                    WeekDays = string.Empty,
                    Laboral = false,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                     CompanyId = 0,
                      DaysPeriode = 0,
                      Description = string.Empty,
                      CanBeDeleted = true
                };
            }
        }

        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                        ""RevisionType"":{1},
                        ""MonthDay"":{2},
                        ""MonthDayOrder"":{3},
                        ""MonthDayWeek"":{4},
                        ""WeekDays"":""{5}"",
                        ""DaysPeriode"":{6},
                        ""Laboral"":{7},
                        ""CreatedBy"":{8},
                        ""CreatedOn"":""{9:dd/MM/yyyy}"",
                        ""ModifiedBy"":{10},
                        ""ModifiedOn"":""{11:dd/MM/yyyy}"",
                        ""Active"":{12}}}",
                    this.Id,
                    this.RevisionType,
                    this.MonthDay,
                    this.MonthDayOrder,
                    this.MonthDayWeek,
                    Tools.JsonCompliant(this.WeekDays),
                    this.DaysPeriode,
                    this.Laboral ? "true" : "false",
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? "true" : "false");
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return this.Json;
            }
        }

        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        public static Revision GetById(int revisionId)
        {
            Revision res = Revision.Empty;
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Revision::GeById({0})",
                revisionId);
            /* CREATE PROCEDURE [issususer].[Revision_GeById]
             *   @RevisionId int */
            using (SqlCommand cmd = new SqlCommand("Revision_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", revisionId));

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();

                                res.Id = revisionId;
                                res.RevisionType = rdr.GetInt32(ColumnsRevisionGet.Type);
                                res.MonthDay = rdr.GetInt32(ColumnsRevisionGet.MonthDay);
                                res.MonthDayOrder = rdr.GetInt32(ColumnsRevisionGet.MonthDayOrder);
                                res.MonthDayWeek = rdr.GetInt32(ColumnsRevisionGet.MonthDayWeek);
                                res.WeekDays = rdr.GetString(ColumnsRevisionGet.WeekDays);
                                res.Laboral = rdr.GetBoolean(ColumnsRevisionGet.Laboral);
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsRevisionGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsRevisionGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsRevisionGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsRevisionGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsRevisionGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsRevisionGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsRevisionGet.Active);
                            }
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

            return res;
        }

        public ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Revision::Revision_Insert({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[Revision_Insert]
             *   @Id int output,
             *   @Type int,
             *   @MonthDay int,
             *   @MonthDayOrder int,
             *   @MonthDayWeek int,
             *   @WeekDays nvarchar(7),
             *   @DaysPeriode int,
             *   @Laboral bit,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Revision_Insert"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@RType", this.RevisionType));
                        cmd.Parameters.Add(DataParameter.Input("@MonthDay", this.MonthDay));
                        cmd.Parameters.Add(DataParameter.Input("@MonthDayOrder", this.MonthDayOrder));
                        cmd.Parameters.Add(DataParameter.Input("@MonthDayWeek", this.MonthDayWeek));
                        cmd.Parameters.Add(DataParameter.Input("@WeekDays", this.WeekDays, 7));
                        cmd.Parameters.Add(DataParameter.Input("@DaysPeriode", this.DaysPeriode));
                        cmd.Parameters.Add(DataParameter.Input("@Laboral", this.Laboral));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@RevisionId"].ToString());
                        res.SetSuccess(this.Id);
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

        public ActionResult Update(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Revision::Revision_Update({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[Revision_Update]
             *   @Id int,
             *   @Type int,
             *   @MonthDay int,
             *   @MonthDayOrder int,
             *   @MonthDayWeek int,
             *   @WeekDays nvarchar(7),
             *   @DaysPeriode int,
             *   @Laboral bit,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Revision_Update"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@RType", this.RevisionType));
                        cmd.Parameters.Add(DataParameter.Input("@MonthDay", this.MonthDay));
                        cmd.Parameters.Add(DataParameter.Input("@MonthDayOrder", this.MonthDayOrder));
                        cmd.Parameters.Add(DataParameter.Input("@MonthDayWeek", this.MonthDayWeek));
                        cmd.Parameters.Add(DataParameter.Input("@WeekDays", this.WeekDays, 7));
                        cmd.Parameters.Add(DataParameter.Input("@DaysPeriode", this.DaysPeriode));
                        cmd.Parameters.Add(DataParameter.Input("@Laboral", this.Laboral));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(this.Id);
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

        public static ActionResult Activate(int revisionId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Revision::Activate({0}, {1})",
                revisionId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Revision_Activate]
             *   @Id int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Revision_Activate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", revisionId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(revisionId);
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

        public static ActionResult Inactivate(int revisionId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Revision::Inactivate({0}, {1})",
                revisionId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Revision_Inactivate]
             *   @Id int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Revision_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", revisionId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(revisionId);
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
    }
}