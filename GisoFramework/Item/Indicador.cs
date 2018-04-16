
// --------------------------------
// <copyright file="Indicador.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Web;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item.Binding;

namespace GisoFramework.Item
{
    public class Indicador : BaseItem
    {
        [DifferenciableAttribute]
        public Process Proceso { get; set; }

        [DifferenciableAttribute]
        public string Calculo { get; set; }

        [DifferenciableAttribute]
        public string MetaComparer { get; set; }

        [DifferenciableAttribute]
        public string AlarmaComparer { get; set; }

        [DifferenciableAttribute]
        public decimal Meta { get; set; }

        [DifferenciableAttribute]
        public decimal? Alarma { get; set; }

        [DifferenciableAttribute]
        public int Periodicity { get; set; }

        [DifferenciableAttribute]
        public DateTime StartDate { get; set; }

        [DifferenciableAttribute]
        public DateTime? EndDate { get; set; }

        [DifferenciableAttribute]
        public string EndReason { get; set; }

        [DifferenciableAttribute]
        public Employee Responsible { get; set; }

        [DifferenciableAttribute]
        public Employee EndResponsible { get; set; }

        [DifferenciableAttribute]
        public Unidad Unidad { get; set; }

        public static Indicador Empty
        {
            get
            {
                return new Indicador()
                {
                    Id = 0,
                    Description = string.Empty,
                    Calculo = string.Empty,
                    Periodicity = 0,
                    MetaComparer = string.Empty,
                    Meta = 0,
                    AlarmaComparer = string.Empty,
                    Alarma = null,
                    // Objetivo = Objetivo.Empty,
                    Proceso = Process.Empty,
                    Unidad = Unidad.Empty,
                    StartDate = DateTime.Now,
                    EndReason = string.Empty,
                    EndResponsible = Employee.Empty,
                    CompanyId = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                    CanBeDeleted = true
                };
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<a href=""IndicadorView.aspx?id={0}"">{1}</a>", this.Id, this.Description);
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        public override string Json
        {
            get
            {
                string endDate = "null";
                if (this.EndDate.HasValue)
                {
                    endDate = string.Format(
                        CultureInfo.InvariantCulture,
                        @"""{0:dd/MM/yyyy}""",
                        this.EndDate);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                    ""CompanyId"":{1},
                    ""Description"":""{2}"",
                    ""Calculo"":""{3}"",
                    ""Proceso"":{4},
                    ""MetaComparer"":""{5}"",
                    {6},
                    ""AlarmaComparer"":""{7}"",
                    {8},
                    ""Unidad"":{9},
                    ""Periodicity"": {10},
                    ""StartDate"": ""{11:dd/MM/yyyy}"",
                    ""EndDate"": {12},
                    ""EndReason"":""{13}"",
                    ""EndResponsible"":{14},
                    ""CreatedBy"":{15},
                    ""CreatedOn"":""{16:dd/MM/yyyy}"",
                    ""ModifiedBy"":{17},
                    ""ModifiedOn"":""{18:dd/MM/yyyy}"",
                    ""Active"":{19}
                    }}",
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.Description),
                    Tools.JsonCompliant(this.Calculo),
                    this.Proceso.JsonKeyValue,
                    this.MetaComparer,
                    Tools.JsonPair("Meta", this.Meta),
                    this.AlarmaComparer,
                    Tools.JsonPair("Alarma", this.Alarma),
                    this.Unidad.JsonKeyValue,
                    this.Periodicity,
                    this.StartDate,
                    endDate,
                    Tools.JsonCompliant(this.EndReason),
                    this.EndResponsible.JsonKeyValue,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? "true" : "false");
            }
        }

        public static ReadOnlyCollection<Indicador> ByCompany(Company company)
        {
            return ByCompany(company.Id);
        }

        public static ReadOnlyCollection<Indicador> ByCompany(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            List<Indicador> res = new List<Indicador>();
            using (SqlCommand cmd = new SqlCommand("Indicador_GetAll"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Indicador indicador = new Indicador()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.Id),
                            Description = rdr.GetString(ColumnsIndicadorGet.Descripcion),
                            CompanyId = companyId,
                            StartDate = rdr.GetDateTime(ColumnsIndicadorGet.StartDate),
                            Calculo = rdr.GetString(ColumnsIndicadorGet.Calculo),
                            MetaComparer = rdr.GetString(ColumnsIndicadorGet.MetaComparer),
                            Meta = rdr.GetDecimal(ColumnsIndicadorGet.Meta),
                            AlarmaComparer = rdr.GetString(ColumnsIndicadorGet.AlarmaComparer)
                        };


                        if (!rdr.IsDBNull(ColumnsIndicadorGet.Alarma))
                        {
                            indicador.Alarma = rdr.GetDecimal(ColumnsIndicadorGet.Alarma);
                        }

                        indicador.Periodicity = rdr.GetInt32(ColumnsIndicadorGet.Periodicity);
                        indicador.Unidad = new Unidad()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.UnidadId),
                            Description = rdr.GetString(ColumnsIndicadorGet.UnidadDescripcion)
                        };

                        indicador.Responsible = new Employee()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleEmployeeId),
                            Name = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeName),
                            LastName = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeLastName),
                            UserId = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                            User = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                UserName = rdr.GetString(ColumnsIndicadorGet.ResponsibleLogin)
                            }
                        };

                        indicador.CreatedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.CreatedBy),
                            UserName = rdr.GetString(ColumnsIndicadorGet.CreatedByName)
                        };
                        indicador.CreatedOn = rdr.GetDateTime(ColumnsIndicadorGet.CreatedOn);
                        indicador.ModifiedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.ModifiedBy),
                            UserName = rdr.GetString(ColumnsIndicadorGet.ModifiedByName)
                        };
                        indicador.ModifiedOn = rdr.GetDateTime(ColumnsIndicadorGet.ModifiedOn);
                        indicador.Active = rdr.GetBoolean(ColumnsIndicadorGet.Active);

                        //if (!rdr.IsDBNull(ColumnsIndicadorGet.ObjetivoId))
                        //{
                        //    indicador.Objetivo = new Objetivo()
                        //    {
                        //        Id = rdr.GetInt32(ColumnsIndicadorGet.ObjetivoId),
                        //        Description = rdr.GetString(ColumnsIndicadorGet.ObjetivoDescription)
                        //    };
                        //}

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.ProcessId))
                        {
                            indicador.Proceso = new Process()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ProcessId),
                                Description = rdr.GetString(ColumnsIndicadorGet.ProcessDescription)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndDate))
                        {
                            indicador.EndDate = rdr.GetDateTime(ColumnsIndicadorGet.EndDate);
                            indicador.EndReason = rdr.GetString(ColumnsIndicadorGet.EndReason);
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndResponsible))
                        {
                            indicador.EndResponsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleEmployeeId),
                                Name = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeName),
                                LastName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeLastName),
                                UserId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                User = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId)
                                }
                            };
                        }

                        res.Add(indicador);
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

            return new ReadOnlyCollection<Indicador>(res);
        }

        public static string PeriodicityByCompany(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            StringBuilder res = new StringBuilder("[");
            using (SqlCommand cmd = new SqlCommand("Indicador_GetAllPeriodicity"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    bool first = true;
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
                            CultureInfo.InvariantCulture,
                            @"{{""Id"":{0},""Periodicity"":{1}}}",
                            rdr.GetInt32(0),
                            rdr.GetInt32(1));
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

        public static ReadOnlyCollection<Indicador> AvailablesByObjetivo(int companyId)
        {
            /* CREATE PROCEDURE Indicador_GetByCompany
             *   @CompanyId int */
            List<Indicador> res = new List<Indicador>();
            using (SqlCommand cmd = new SqlCommand("Indicador_GetAvailablesForObjetivo"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Indicador indicador = new Indicador()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.Id),
                            Description = rdr.GetString(ColumnsIndicadorGet.Descripcion),
                            CompanyId = companyId,
                            Calculo = rdr.GetString(ColumnsIndicadorGet.Calculo),
                            StartDate = rdr.GetDateTime(ColumnsIndicadorGet.StartDate),
                            MetaComparer = rdr.GetString(ColumnsIndicadorGet.MetaComparer),
                            Meta = rdr.GetDecimal(ColumnsIndicadorGet.Meta),
                            AlarmaComparer = rdr.GetString(ColumnsIndicadorGet.AlarmaComparer),
                            Periodicity = rdr.GetInt32(ColumnsIndicadorGet.Periodicity),
                            Unidad = new Unidad()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.UnidadId),
                                Description = rdr.GetString(ColumnsIndicadorGet.UnidadDescripcion)
                            },
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleEmployeeId),
                                Name = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeName),
                                LastName = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeLastName),
                                UserId = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                User = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                    UserName = rdr.GetString(ColumnsIndicadorGet.ResponsibleLogin)
                                }
                            },
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsIndicadorGet.CreatedByName)
                            },
                            CreatedOn = rdr.GetDateTime(ColumnsIndicadorGet.CreatedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsIndicadorGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsIndicadorGet.ModifiedOn),
                            Active = rdr.GetBoolean(ColumnsIndicadorGet.Active)
                        };

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.Alarma))
                        {
                            indicador.Alarma = rdr.GetDecimal(ColumnsIndicadorGet.Alarma);
                        }

                        //if (!rdr.IsDBNull(ColumnsIndicadorGet.ObjetivoId))
                        //{
                        //    indicador.Objetivo = new Objetivo()
                        //    {
                        //        Id = rdr.GetInt32(ColumnsIndicadorGet.ObjetivoId),
                        //        Description = rdr.GetString(ColumnsIndicadorGet.ObjetivoDescription)
                        //    };
                        //}

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.ProcessId) && !rdr.IsDBNull(ColumnsIndicadorGet.ProcessDescription))
                        {
                            indicador.Proceso = new Process()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ProcessId),
                                Description = rdr.GetString(ColumnsIndicadorGet.ProcessDescription)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndDate))
                        {
                            indicador.EndDate = rdr.GetDateTime(ColumnsIndicadorGet.EndDate);
                            indicador.EndReason = rdr.GetString(ColumnsIndicadorGet.EndReason);
                        }

                        /*if (!rdr.IsDBNull(ColumnsIndicadorGet.EndResponsible))
                        {
                            int endResponsibleId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsible);
                            if (endResponsibleId < 0)
                            {
                                indicador.EndResponsible = Employee.Empty;
                            }
                            else
                            {
                                indicador.EndResponsible = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleEmployeeId),
                                    //Name = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeName),
                                    //LastName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeLastName),
                                    UserId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsible),
                                    User = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                        UserName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleLogin)
                                    }
                                };
                            }
                        }*/

                        res.Add(indicador);
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

            return new ReadOnlyCollection<Indicador>(res);
        }

        public static ReadOnlyCollection<Indicador> GetActive(int companyId)
        {
            /* CREATE PROCEDURE Indicador_GetByCompany
             *   @CompanyId int */
            List<Indicador> res = new List<Indicador>();
            using (SqlCommand cmd = new SqlCommand("Indicador_GetActive"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Indicador indicador = new Indicador()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.Id),
                            Description = rdr.GetString(ColumnsIndicadorGet.Descripcion),
                            CompanyId = companyId,
                            StartDate = rdr.GetDateTime(ColumnsIndicadorGet.StartDate),
                            Calculo = rdr.GetString(ColumnsIndicadorGet.Calculo),
                            MetaComparer = rdr.GetString(ColumnsIndicadorGet.MetaComparer),
                            Meta = rdr.GetDecimal(ColumnsIndicadorGet.Meta),
                            AlarmaComparer = rdr.GetString(ColumnsIndicadorGet.AlarmaComparer)
                        };

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.Alarma))
                        {
                            indicador.Alarma = rdr.GetDecimal(ColumnsIndicadorGet.Alarma);
                        }

                        indicador.Periodicity = rdr.GetInt32(ColumnsIndicadorGet.Periodicity);
                        indicador.Unidad = new Unidad()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.UnidadId),
                            Description = rdr.GetString(ColumnsIndicadorGet.UnidadDescripcion)
                        };

                        indicador.Responsible = new Employee()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleEmployeeId),
                            Name = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeName),
                            LastName = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeLastName),
                            UserId = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                            User = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                UserName = rdr.GetString(ColumnsIndicadorGet.ResponsibleLogin)
                            }
                        };

                        indicador.CreatedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.CreatedBy),
                            UserName = rdr.GetString(ColumnsIndicadorGet.CreatedByName)
                        };
                        indicador.CreatedOn = rdr.GetDateTime(ColumnsIndicadorGet.CreatedOn);
                        indicador.ModifiedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.ModifiedBy),
                            UserName = rdr.GetString(ColumnsIndicadorGet.ModifiedByName)
                        };
                        indicador.ModifiedOn = rdr.GetDateTime(ColumnsIndicadorGet.ModifiedOn);
                        indicador.Active = rdr.GetBoolean(ColumnsIndicadorGet.Active);

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.ProcessId) && !rdr.IsDBNull(ColumnsIndicadorGet.ProcessDescription))
                        {
                            indicador.Proceso = new Process()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ProcessId),
                                Description = rdr.GetString(ColumnsIndicadorGet.ProcessDescription)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndDate))
                        {
                            indicador.EndDate = rdr.GetDateTime(ColumnsIndicadorGet.EndDate);
                            indicador.EndReason = rdr.GetString(ColumnsIndicadorGet.EndReason);
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndResponsible))
                        {
                            int endResponsibleId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsible);
                            if (endResponsibleId < 0)
                            {
                                indicador.EndResponsible = Employee.Empty;
                            }
                            else
                            {
                                indicador.EndResponsible = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleEmployeeId),
                                    Name = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeName),
                                    LastName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeLastName),
                                    UserId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                    User = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                        UserName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleLogin)
                                    }
                                };
                            }
                        }

                        res.Add(indicador);
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

            return new ReadOnlyCollection<Indicador>(res);
        }

        public static ReadOnlyCollection<Indicador> ByProcessId(int processId, int companyId)
        {
            /* CREATE PROCEDURE [dbo].[Indicador_GetByProcessId]
             *   @CompanyId int,
             *   @ProcessId int */
            List<Indicador> res = new List<Indicador>();
            using (SqlCommand cmd = new SqlCommand("Indicador_GetByProcessId"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ProcessId", processId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Indicador indicador = new Indicador()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.Id),
                            Description = rdr.GetString(ColumnsIndicadorGet.Descripcion),
                            CompanyId = companyId,
                            StartDate = rdr.GetDateTime(ColumnsIndicadorGet.StartDate),
                            Calculo = rdr.GetString(ColumnsIndicadorGet.Calculo),
                            MetaComparer = rdr.GetString(ColumnsIndicadorGet.MetaComparer),
                            Meta = rdr.GetDecimal(ColumnsIndicadorGet.Meta),
                            AlarmaComparer = rdr.GetString(ColumnsIndicadorGet.AlarmaComparer),
                            Periodicity = rdr.GetInt32(ColumnsIndicadorGet.Periodicity),
                            Unidad = new Unidad()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.UnidadId),
                                Description = rdr.GetString(ColumnsIndicadorGet.UnidadDescripcion)
                            },
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsIndicadorGet.CreatedByName)
                            },
                            CreatedOn = rdr.GetDateTime(ColumnsIndicadorGet.CreatedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsIndicadorGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsIndicadorGet.ModifiedOn),
                            Active = rdr.GetBoolean(ColumnsIndicadorGet.Active),
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleEmployeeId),
                                Name = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeName),
                                LastName = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeLastName),
                                UserId = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                User = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                    UserName = rdr.GetString(ColumnsIndicadorGet.ResponsibleLogin)
                                }
                            }
                        };

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.Alarma))
                        {
                            indicador.Alarma = rdr.GetDecimal(ColumnsIndicadorGet.Alarma);
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.ProcessId))
                        {
                            indicador.Proceso = new Process()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ProcessId),
                                Description = rdr.GetString(ColumnsIndicadorGet.ProcessDescription)
                            };
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndDate))
                        {
                            indicador.EndDate = rdr.GetDateTime(ColumnsIndicadorGet.EndDate);
                            indicador.EndReason = rdr.GetString(ColumnsIndicadorGet.EndReason);
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndResponsible))
                        {
                            int endResponsibleId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsible);
                            if (endResponsibleId < 0)
                            {
                                indicador.EndResponsible = Employee.Empty;
                            }
                            else
                            {
                                indicador.EndResponsible = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleEmployeeId),
                                    Name = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeName),
                                    LastName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeLastName),
                                    UserId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                    User = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                        UserName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleLogin)
                                    }
                                };
                            }
                        }

                        res.Add(indicador);
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

            return new ReadOnlyCollection<Indicador>(res);
        }

        public static string ComparerLabel(string comparerValue, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            switch (comparerValue.ToUpperInvariant())
            {
                case "EQ": return dictionary["Common_Comparer_eq"];
                case "GT": return dictionary["Common_Comparer_gt"];
                case "EQGT": return dictionary["Common_Comparer_eqgt"];
                case "LT": return dictionary["Common_Comparer_lt"];
                case "EQLT": return dictionary["Common_Comparer_eqlt"];
            }

            return string.Empty;
        }

        public static string ComparerLabelSign(string comparerValue, Dictionary<string, string> dictionary)
        {
            if (string.IsNullOrEmpty(comparerValue))
            {
                return string.Empty;
            }

            switch (comparerValue.ToUpperInvariant())
            {
                case "EQ": return dictionary["Common_ComparerSign_eq"];
                case "GT": return dictionary["Common_ComparerSign_gt"];
                case "EQGT": return dictionary["Common_ComparerSign_eqgt"];
                case "LT": return dictionary["Common_ComparerSign_lt"];
                case "EQLT": return dictionary["Common_ComparerSign_eqlt"];
            }

            return string.Empty;
        }

        public static string ByCompanyJson(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Indicador objetivo in ByCompany(companyId))
            {
                if (objetivo.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(objetivo.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static Indicador ById(int indicadorId, int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            Indicador res = Indicador.Empty;
            using (SqlCommand cmd = new SqlCommand("Indicador_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res.Id = rdr.GetInt32(ColumnsIndicadorGet.Id);
                        res.Description = rdr.GetString(ColumnsIndicadorGet.Descripcion);
                        res.CompanyId = companyId;
                        res.Calculo = rdr.GetString(ColumnsIndicadorGet.Calculo);
                        res.MetaComparer = rdr.GetString(ColumnsIndicadorGet.MetaComparer);
                        res.StartDate = rdr.GetDateTime(ColumnsIndicadorGet.StartDate);
                        res.Meta = rdr.GetDecimal(ColumnsIndicadorGet.Meta);
                        res.AlarmaComparer = rdr.GetString(ColumnsIndicadorGet.AlarmaComparer);
                        if (!rdr.IsDBNull(ColumnsIndicadorGet.Alarma))
                        {
                            res.Alarma = rdr.GetDecimal(ColumnsIndicadorGet.Alarma);
                        }

                        res.Periodicity = rdr.GetInt32(ColumnsIndicadorGet.Periodicity);
                        res.Unidad = new Unidad()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.UnidadId),
                            Description = rdr.GetString(ColumnsIndicadorGet.UnidadDescripcion)
                        };

                        res.Responsible = new Employee()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleEmployeeId),
                            Name = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeName),
                            LastName = rdr.GetString(ColumnsIndicadorGet.ResponsibleEmployeeLastName),
                            UserId = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                            User = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsIndicadorGet.ResponsibleId),
                                UserName = rdr.GetString(ColumnsIndicadorGet.ResponsibleLogin)
                            }
                        };

                        res.CreatedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.CreatedBy),
                            UserName = rdr.GetString(ColumnsIndicadorGet.CreatedByName)
                        };
                        res.CreatedOn = rdr.GetDateTime(ColumnsIndicadorGet.CreatedOn);
                        res.ModifiedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsIndicadorGet.ModifiedBy),
                            UserName = rdr.GetString(ColumnsIndicadorGet.ModifiedByName)
                        };
                        res.ModifiedOn = rdr.GetDateTime(ColumnsIndicadorGet.ModifiedOn);
                        res.Active = rdr.GetBoolean(ColumnsIndicadorGet.Active);

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.ProcessId))
                        {
                            int processId = rdr.GetInt32(ColumnsIndicadorGet.ProcessId);
                            if (processId > 0)
                            {
                                res.Proceso = new Process()
                                {
                                    Id = processId,
                                    Description = rdr.GetString(ColumnsIndicadorGet.ProcessDescription)
                                };
                            }
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndDate))
                        {
                            res.EndDate = rdr.GetDateTime(ColumnsIndicadorGet.EndDate);
                            res.EndReason = rdr.GetString(ColumnsIndicadorGet.EndReason);
                        }

                        if (!rdr.IsDBNull(ColumnsIndicadorGet.EndResponsible))
                        {
                            int endResponsibleId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId);
                            if (endResponsibleId > 0)
                            {
                                res.EndResponsible = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleEmployeeId),
                                    Name = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeName),
                                    LastName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleEmployeeLastName),
                                    UserId = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                    User = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorGet.EndResponsibleId),
                                        UserName = rdr.GetString(ColumnsIndicadorGet.EndResponsibleLogin)
                                    }
                                };
                            }
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

            return res;
        }

        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Indicador);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Indicador);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "IndicadorDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>",
                    deleteFunction,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));

            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='IndicadorView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='IndicadorView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            string pattenr = @"<tr><td>{0}</td><td style=""width:250px;"">{1}</td><td align=""center"" style=""width:90px;"">{4:dd/MM/yyyy}</td><td align=""center"" style=""width:90px;"">{5:dd/MM/yyyy}</td><td style=""width:90px;"">{2}&nbsp;{3}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattenr,
                this.Link,
                this.Proceso.Link,
                string.Empty, // this.Objetivo.Link,
                iconEdit,
                iconDelete,
                this.Responsible.FullName,
                this.EndResponsible.FullName);
        }

        public static ActionResult Activate(int indicadorId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Indicador::Activate({0}, {1})",
                indicadorId,
                applicationUserId);
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Indicador_Activate]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Indicador_Activate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(indicadorId);
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

        public static ActionResult Restore(int indicadorId, int companyId, int applicationUserId)
        {
            string source = string.Format(
               CultureInfo.InvariantCulture,
               @"Indicador::Restore({0}, {1})",
               indicadorId,
               applicationUserId);
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Indicador_Restore]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Indicador_Restore"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(indicadorId);
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
        
        public static ActionResult Anulate(int indicadorId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Indicador::Inactivate({0}, {1})",
                indicadorId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE [dbo].[Indicador_Anulate]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @EndDate datetime,
             *   @EndReason nvarchar(500),
             *   @EndResponsable int,
             *   @UnidadId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Indicador_Anulate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", date));
                        cmd.Parameters.Add(DataParameter.Input("@EndReason", reason, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@EndResponsible", responsible));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(indicadorId);
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

        public static ActionResult Inactivate(int indicadorId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Objetivo::Inactivate({0}, {1})",
                indicadorId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Indicador_Inactivate]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Indicador_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(indicadorId);
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

        private ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Indicador::Indicador_Insert({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Indicador_Insert] 
             *   @IndicadorId int output,
             *   @CompanyId int,
             *   @Description nvarchar(150),
             *   @ResponsableId int,
             *   @ProcessId int,
             *   @ObjetivoId int,
             *   @Calculo nvarchar(500),
             *   @MetaComparer nvarchar(10),
             *   @Meta decimal(18,6),
             *   @AlarmaComparer nvarchar(10),
             *   @Alarma decimal(18,6),
             *   @Periodicity int,
             *   @EndDate datetime,
             *   @EndReason nvarchar(500),
             *   @EndResponsible int,
             *   @UnidadId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Indicador_Insert"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputInt("@IndicadorId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Proceso.Id));
                        //cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", this.Objetivo.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Calculo", this.Calculo, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@AlarmaComparer", this.AlarmaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@alarma", this.Alarma));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@StartDate", this.StartDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", this.EndDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndReason", this.EndReason, 500));
                        cmd.Parameters.Add(DataParameter.Input("@EndResponsible", this.EndResponsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UnidadId", this.Unidad.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@IndicadorId"].Value.ToString());
                        res.SetSuccess(this.Id);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
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

        private ActionResult Update(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Indicador::Indicador_Update({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Indicador_Update]
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @Descripcion nvarchar(150),
             *   @ResponsableId int,
             *   @ProcessId int,
             *   @ObjetivoId int,
             *   @Calculo nvarchar(2000),
             *   @MetaComparer nvarchar(10),
             *   @Meta decimal(18,6),
             *   @AlarmaComparer nvarchar(10),
             *   @Alarma decimal(18,6),
             *   @Periodicity int,
             *   @EndDate datetime,
             *   @EndReason nvarchar(500),
             *   @EndResponsable int,
             *   @UnidadId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Indicador_Update"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Descripcion", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ProcessId", this.Proceso.Id));
                        // cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", this.Objetivo.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Calculo", this.Calculo, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@AlarmaComparer", this.AlarmaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@alarma", this.Alarma));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@StartDate", this.StartDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndDate", this.EndDate));
                        cmd.Parameters.Add(DataParameter.Input("@EndReason", this.EndReason, 500));
                        cmd.Parameters.Add(DataParameter.Input("@EndResponsable", this.EndResponsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UnidadId", this.Unidad.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@IndicadorId"].Value.ToString());
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

        public static string FilterList(int companyId, int indicadorType, DateTime? from, DateTime? to, int? processId, int? processTypeId, int? objetivoId, int status)
        {
            ReadOnlyCollection<IndicadorFilterItem> items = Filter(companyId, indicadorType, from, to, processId, processTypeId, objetivoId, status);
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (IndicadorFilterItem item in items)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append("{");
                res.Append(Tools.JsonPair("IndicadorId", item.Indicador.Id)).Append(",");
                res.Append(Tools.JsonPair("IndicadorDescription", item.Indicador.Description)).Append(",");
                res.Append(Tools.JsonPair("StartDate", item.StartDate)).Append(",");
                res.Append(Tools.JsonPair("EndDate", item.EndDate)).Append(",");
                res.Append(Tools.JsonPair("ProcessId", item.Proceso.Id)).Append(",");
                res.Append(Tools.JsonPair("ProcessDescription", item.Proceso.Description)).Append(",");
                res.Append(Tools.JsonPair("ProcessType", item.Proceso.ProcessType)).Append(",");
                res.Append(Tools.JsonPair("ProcessResponsible", item.ProcessResponsible)).Append(",");
                res.Append(Tools.JsonPair("ObjetivoId", item.Objetivo.Id)).Append(",");
                res.Append(Tools.JsonPair("ObjetivoDescription", item.Objetivo.Description)).Append(",");
                res.Append(Tools.JsonPair("ObjetivoResponsible", item.ObjetivoResponsible)).Append(",");
                res.Append(Tools.JsonPair("Status", item.Status)).Append("}");
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IndicadorFilterItem> Filter(int companyId,int indicadorType, DateTime? from, DateTime? to, int? processId, int? processTypeId, int? objetivoId, int status)
        {
            /* CREATE PROCEDURE Indicator_Filter
             *   @CompanyId int,
             *   @IndicadorType int,
             *   @DateFrom datetime,
             *   @DateTo datetime,
             *   @ProcessId int,
             *   @ProcessTypeId int,
             *   @ObjetivoId int
             *   @Closed bit */
            var res = new List<IndicadorFilterItem>();
            using (var cmd = new SqlCommand("Indicator_Filter"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@IndicadorType", indicadorType));
                    cmd.Parameters.Add(DataParameter.Input("@DateFrom", from));
                    cmd.Parameters.Add(DataParameter.Input("@DateTo", to));
                    cmd.Parameters.Add(DataParameter.Input("@ProcessId", processId));
                    cmd.Parameters.Add(DataParameter.Input("@ProcessTypeId", processTypeId));
                    cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                    cmd.Parameters.Add(DataParameter.Input("@Status", status));

                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    int lastIndicador = 0;
                    while (rdr.Read())
                    {
                        int actualIndicador = rdr.GetInt32(ColumnsIndicadorFilter.IndicadorId);
                        if (actualIndicador != lastIndicador)
                        {
                            IndicadorFilterItem item = new IndicadorFilterItem()
                            {
                                Indicador = new Indicador()
                                {
                                    Id = actualIndicador,
                                    Description = rdr.GetString(ColumnsIndicadorFilter.Descripcion)
                                },
                                Proceso = new Process()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorFilter.ProcessId),
                                    Description = rdr.GetString(ColumnsIndicadorFilter.ProcessDescription),
                                    ProcessType = rdr.GetInt32(ColumnsIndicadorFilter.ProcessType)
                                },
                                //Objetivo = Objetivo.Empty,
                                Objetivo = new Objetivo()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorFilter.ObjetivoId),
                                    Description = string.Empty //rdr.GetString(ColumnsIndicadorFilter.ObjetivoDescription)
                                },
                                ProcessResponsible = rdr.GetString(ColumnsIndicadorFilter.ProcesoResponsible),
                                ObjetivoResponsible = string.Format(
                                    CultureInfo.InvariantCulture,
                                    "{0} {1}",
                                    rdr.GetString(ColumnsIndicadorFilter.ObjetivoResponsibleName),
                                    rdr.GetString(ColumnsIndicadorFilter.ObjetivoResponsableLastName)).Trim(),
                                StartDate = rdr.GetDateTime(ColumnsIndicadorFilter.StartDate)
                            };

                            if (!rdr.IsDBNull(ColumnsIndicadorFilter.EndDate))
                            {
                                item.EndDate = rdr.GetDateTime(ColumnsIndicadorFilter.EndDate);
                            }

                            // Calcular estado
                            item.Status = 0;
                            if (!rdr.IsDBNull(ColumnsIndicadorFilter.Value))
                            {
                                decimal value = rdr.GetDecimal(ColumnsIndicadorFilter.Value);
                                string metaComparer = rdr.GetString(ColumnsIndicadorFilter.MetaComparer);
                                decimal meta = rdr.GetDecimal(ColumnsIndicadorFilter.Meta);

                                if (metaComparer.Equals("eq",StringComparison.OrdinalIgnoreCase) && value == meta)
                                {
                                    status = 1;
                                }

                                if (rdr.IsDBNull(ColumnsIndicadorFilter.AlarmaComparer))
                                {
                                    if (metaComparer.Equals("gt", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (value > meta)
                                        {
                                            status = 1;
                                        }
                                        else
                                        {
                                            status = 2;
                                        }
                                    }
                                    else if (metaComparer.Equals("lt", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (value < meta)
                                        {
                                            status = 1;
                                        }
                                        else
                                        {
                                            status = 2;
                                        }
                                    }
                                    else if (metaComparer.Equals("eqlt", StringComparison.OrdinalIgnoreCase) )
                                    {
                                        if (value <= meta)
                                        {
                                            status = 1;
                                        }
                                        else
                                        {
                                            status = 2;
                                        }
                                    } 
                                    else if (metaComparer.Equals("eqgt", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (value >= meta)
                                        {
                                            status = 1;
                                        }
                                        else
                                        {
                                            status = 2;
                                        }
                                    }
                                }
                            }

                            res.Add(item);
                        }

                        lastIndicador = actualIndicador;
                    }
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }

                HttpContext.Current.Session["IndicadorFilterData"] = res;
                return new ReadOnlyCollection<IndicadorFilterItem>(res);
            }
        }

        public string ListRowProcessTab(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Indicador);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Indicador);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "IndicadorDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>",
                    deleteFunction,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));

            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='IndicadorView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='IndicadorView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            string alarmaText = string.Empty;
            if (this.Alarma.HasValue)
            {
                alarmaText = string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0:#0.00}",
                    this.Alarma);
            }

            string pattenr = @"<tr>
                <td>{0}</td>
                <td style=""width:120px;"">{1} {2}</td>
                <td style=""width:120px;"">{3} {4}</td>
                <td style=""width:150px;"">{5}</td>
                <td style=""width:150px;"">{6}</td>
                <td style=""width:90px;"">{7}&nbsp;{8}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattenr,
                grantWrite ? this.Link : this.Description,
                ComparerLabelSign(this.MetaComparer, dictionary),
                this.Meta,
                ComparerLabelSign(this.AlarmaComparer, dictionary),
                alarmaText,
                this.Unidad.Description,
                this.Responsible.FullName,
                iconEdit,
                iconDelete);
        }

        public ActionResult Save(int applicationUserId)
        {
            if (this.Id > 0)
            {
                return this.Update(applicationUserId);
            }

            return this.Insert(applicationUserId);
        }
    }
}
