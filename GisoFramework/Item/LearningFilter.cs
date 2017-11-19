// --------------------------------
// <copyright file="LearningFilter.cs" company="Sbrinna">
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
    using GisoFramework.Item.Binding;

    /// <summary>
    /// Implements LearningFilter class
    /// </summary>
    public class LearningFilter
    {
        /// <summary>
        /// Initializes a new instance of the LearningFilter class.
        /// </summary>
        /// <param name="companyId">Compnay identifier</param>
        public LearningFilter(int companyId)
        {
            this.CompanyId = companyId;
            this.Mode = 2;
            this.YearFrom = DateTime.Now.Year;
            this.YearTo = DateTime.Now.Year;
        }

        /// <summary>
        /// Gets or sets compnay identifier
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets year of start period
        /// </summary>
        public int? YearFrom { get; set; }

        /// <summary>
        /// Gets or sets year of finish period
        /// </summary>
        public int? YearTo { get; set; }

        /// <summary>
        /// Gets or sets the mode of searched learnings
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// Gets a JSON structure of learning filter
        /// </summary>
        public string Json
        {
            get
            {
                string yearFromText = "null";
                string yearToText = "null";

                if (this.YearFrom.HasValue)
                {
                    yearFromText = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0000}", this.YearFrom.Value);
                }

                if (this.YearTo.HasValue)
                {
                    yearToText = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:0000}", this.YearTo.Value);
                }

                string pattern = @"{{
                        ""YearFrom"":{0},
                        ""YearTo"":{1},
                        ""Mode"":{2},
                        ""CompanyId"":{3}
                    }}";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    yearFromText,
                    yearToText,
                    this.Mode,
                    this.CompanyId);
            }
        }

        /// <summary>
        /// Obtain compnay's learning by filter
        /// </summary>
        /// <returns>List of learnings</returns>
        public ReadOnlyCollection<Learning> Filter()
        {
            List<Learning> res = new List<Learning>();
            using (SqlCommand cmd = new SqlCommand("Learning_Filter"))
            {
                /* CREATE PROCEDURE Learning_Filter
                 * @YearFrom int,
                 * @YearTo int,
                 * @Mode int,
                 * @CompanyId int */
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@YearFrom", SqlDbType.Int);
                    cmd.Parameters.Add("@YearTo", SqlDbType.Int);
                    cmd.Parameters.Add("@Mode", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);

                    if (this.YearFrom.HasValue)
                    {
                        cmd.Parameters["@YearFrom"].Value = this.YearFrom.Value;
                    }
                    else
                    {
                        cmd.Parameters["@YearFrom"].Value = DBNull.Value;
                    }

                    if (this.YearTo.HasValue)
                    {
                        cmd.Parameters["@YearTo"].Value = this.YearTo.Value;
                    }
                    else
                    {
                        cmd.Parameters["@YearTo"].Value = DBNull.Value;
                    }

                    cmd.Parameters["@Mode"].Value = this.Mode;
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new Learning()
                            {
                                Id = rdr.GetInt32(ColumnsLearningFilter.LearningId),
                                Description = rdr.GetString(ColumnsLearningFilter.LearningDescription),
                                DateEstimated = rdr.GetDateTime(ColumnsLearningFilter.LearningEstimatedDate),
                                Amount = rdr.GetDecimal(ColumnsLearningFilter.LearningAmmount),
                                CompanyId = this.CompanyId
                            });
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
