// --------------------------------
// <copyright file="AlertDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Alerts
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>Implements alert definition</summary>
    public class AlertDefinition
    {
        /// <summary>Gets or sets de company identifier</summary>
        [JsonProperty("CompanyId")]
        public int CompanyId { get; set; }

        /// <summary>Gets or sets de item type</summary>
        [JsonProperty("ItemType")]
        public int ItemType { get; set; }

        /// <summary>Gets or sets the alert description</summary>
        [JsonProperty("AlertDescription")]
        public string AlertDescription { get; set; }

        /// <summary>Gets or sets the query to extract alert occurrences</summary>
        [JsonProperty("Query")]
        public string Query { get; set; }

        /// <summary>Gets or sets the HTML template for alert menu tag</summary>
        [JsonProperty("Tag")]
        public string Tag { get; set; }

        /// <summary>Gets or sets the HTML template for alert list row</summary>
        [JsonProperty("Row")]
        public string Row { get; set; }

        /// <summary>Gets or sets the url of affected item</summary>
        [JsonProperty("ItemUrl")]
        public string ItemUrl { get; set; }

        /// <summary>Gets or sets the index of field positions</summary>
        [JsonProperty("Index")]
        private FieldPosition[] Index { get; set; }

        /// <summary>Read alert definition from disk</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <returns>Alert definition structure</returns>
        public static ReadOnlyCollection<AlertDefinition> GetFromDisk(Dictionary<string, string> dictionary)
        {
            int companyId = Convert.ToInt32(HttpContext.Current.Session["CompanyId"], CultureInfo.GetCultureInfo("en-us"));
            var res = new List<AlertDefinition>();
            string path = HttpContext.Current.Request.PhysicalApplicationPath + "Alerts";
            if (!path.EndsWith(@"\", StringComparison.Ordinal))
            {
                path += @"\";
            }

            var myFiles = Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly).ToList();

            foreach (string fileName in myFiles)
            {
                res.Add(GetDefinitionByFile(companyId, fileName, dictionary));
            }

            return new ReadOnlyCollection<AlertDefinition>(res);
        }

        /// <summary>Read alert definition from file</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="fileName">File name of alert</param>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <returns>AlertDefinition structure</returns>
        public static AlertDefinition GetDefinitionByFile(int companyId, string fileName, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string userid = HttpContext.Current.Session["UserId"].ToString();

            var alert = new AlertDefinition();
            if (File.Exists(fileName))
            {
                using (var input = new StreamReader(fileName))
                {
                    alert = JsonConvert.DeserializeObject<AlertDefinition>(input.ReadToEnd());

                    if (dictionary.ContainsKey(alert.AlertDescription))
                    {
                        alert.AlertDescription = dictionary[alert.AlertDescription];
                    }

                    alert.Query = alert.Query.Replace("#CompanyId#", companyId.ToString(CultureInfo.GetCultureInfo("en-us")));
                    alert.Query = alert.Query.Replace("#ActualUser#", userid);
                    if (alert.CompanyId != 0 && alert.CompanyId != companyId)
                    {
                        return new AlertDefinition();
                    }
                }
            }

            return alert;
        }

        /// <summary>Creates a HTML code for a alert row of alerts page</summary>
        /// <param name="dictionary">Dictionary for fixed label</param>
        /// <returns>HTML code for a alert row of alerts page</returns>
        public ReadOnlyCollection<string> RenderRow(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            var res = new List<string>();
            var columns = new List<string>();
            foreach (var position in this.Index.OrderBy(x => x.Position))
            {
                columns.Add(position.FieldName);
            }

            using (var cmd = new SqlCommand(this.Query))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new List<string>();
                                foreach (string columnName in columns)
                                {
                                    data.Add(rdr[columnName].ToString());
                                }

                                if (!string.IsNullOrEmpty(this.Row))
                                {
                                    res.Add(string.Format(CultureInfo.InvariantCulture, this.Row.Replace("#AlertDescription#", this.AlertDescription), data.ToArray()));
                                }
                                else
                                {
                                    res.Add("<tr><td>" + this.AlertDescription + "</td><td>" + data[1] + "</td><td><span class=\"btn btn-xs btn-info\" onclick=\"document.location='" + this.ItemUrl + data[0] + "';\"><i class=\"icon-edit bigger-1202\"></i></span></td></tr>");
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
            }

            return new ReadOnlyCollection<string>(res);
        }

        /// <summary>Extract alert ocurrences</summary>
        /// <returns>A list of alert ocurrences</returns>
        public ReadOnlyCollection<string> Extract()
        {
            var res = new List<string>();
            var columns = new List<string>();
            foreach (var position in this.Index.OrderBy(x => x.Position))
            {
                columns.Add(position.FieldName);
            }

            using (var cmd = new SqlCommand(this.Query))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var data = new List<string>();
                                foreach (string columnName in columns)
                                {
                                    data.Add(rdr[columnName].ToString());
                                }

                                res.Add(string.Format(CultureInfo.GetCultureInfo("en-us"), this.Tag.Replace("#AlertDescription#", this.AlertDescription), data.ToArray()));
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

            return new ReadOnlyCollection<string>(res);
        }
    }
}