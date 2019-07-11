// --------------------------------
// <copyright file="LearningFilter.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements LearningFilter class</summary>
    public class LearningFilter
    {
        /// <summary>Initializes a new instance of the LearningFilter class.</summary>
        /// <param name="companyId">Compnay identifier</param>
        public LearningFilter(int companyId)
        {
            this.CompanyId = companyId;
            this.Pendent = true;
            this.Started = true;
            this.Finished = true;
            this.Evaluated = true;
        }

        /// <summary>Gets or sets compnay identifier.</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets year of start period</summary>
        public DateTime? YearFrom { get; set; }

        /// <summary>Gets or sets year of finish period</summary>
        public DateTime? YearTo { get; set; }

        /// <summary>Gets or sets the mode of searched learnings</summary>
        public bool Pendent { get; set; }

        /// <summary>Gets or sets the mode of searched learnings</summary>
        public bool Started { get; set; }

        /// <summary>Gets or sets the mode of searched learnings</summary>
        public bool Finished { get; set; }

        /// <summary>Gets or sets the mode of searched learnings</summary>
        public bool Evaluated { get; set; }

        /// <summary>Gets a JSON structure of learning filter</summary>
        public string Json
        {
            get
            {
                string yearFromText = Constant.JavaScriptNull;
                string yearToText = Constant.JavaScriptNull;

                if (this.YearFrom.HasValue)
                {
                    yearFromText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.YearFrom.Value);
                }

                if (this.YearTo.HasValue)
                {
                    yearToText = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.YearTo.Value);
                }

                string pattern = @"{{
                        ""YearFrom"":{0},
                        ""YearTo"":{1},
                        ""Pendent"":{2},
                        ""Started"":{3},
                        ""Finished"":{4},
                        ""Evaluated"":{5},
                        ""CompanyId"":{6}
                    }}";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    yearFromText,
                    yearToText,
                    this.Pendent ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.Started   ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.Finished ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.Evaluated ? Constant.JavaScriptTrue : Constant.JavaScriptFalse,
                    this.CompanyId);
            }
        }

        /// <summary>Obtain compnay's learning by filter</summary>
        /// <returns>List of learnings</returns>
        public ReadOnlyCollection<Learning> Filter()
        {
            var res = new List<Learning>();
            using (var cmd = new SqlCommand("Learning_Filter"))
            {
                /* CREATE PROCEDURE Learning_Filter
                 * @YearFrom int,
                 * @YearTo int,
                 * @Pendent bit,
                 * @Started bit,
                 * @Finisehd bit,
                 * @Evaluated bit,
                 * @CompanyId int */
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@YearFrom", this.YearFrom));
                    cmd.Parameters.Add(DataParameter.Input("@YearTo", this.YearTo));
                    cmd.Parameters.Add(DataParameter.Input("@Pendent", this.Pendent));
                    cmd.Parameters.Add(DataParameter.Input("@Started", this.Started));
                    cmd.Parameters.Add(DataParameter.Input("@Finished", this.Finished));
                    cmd.Parameters.Add(DataParameter.Input("@Evaluated", this.Evaluated));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));

                    cmd.Connection.Open();
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var data = new Learning
                        {
                            Id = rdr.GetInt32(ColumnsLearningFilter.Id),
                            Description = rdr.GetString(ColumnsLearningFilter.CourseName),
                            DateEstimated = rdr.GetDateTime(ColumnsLearningFilter.EstimatedDate),
                            Amount = rdr.GetDecimal(ColumnsLearningFilter.Ammount),
                            CompanyId = this.CompanyId,
                            Status = rdr.GetInt32(ColumnsLearningFilter.Status)
                        };

                        if (!rdr.IsDBNull(ColumnsLearningFilter.FinishDate)){
                            data.RealFinish = rdr.GetDateTime(ColumnsLearningFilter.FinishDate);
                        }

                        res.Add(data);
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

            return new ReadOnlyCollection<Learning>(res);
        }
    }
}