using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CopiarFotos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Dictionary<long, long> pertenencia = new Dictionary<long, long>();

        using(var cmd = new SqlCommand("Select Id,CompanyId from equipment"))
        {
            using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.Text;
                try
                {
                    cmd.Connection.Open();
                    using(var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            pertenencia.Add(rdr.GetInt64(0), rdr.GetInt32(1));
                        }
                    }
                }
                finally
                {
                    if(cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        var src = this.Request.PhysicalApplicationPath + "\\images\\equipments";

        var fotos = Directory.GetFiles(src, "*.jpg");
        foreach(var file in fotos)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            this.ltfotos.Text += name + "- " + file;

            long companyId = -1;
            long equipmentId = -1;
            var test = long.TryParse(name,out equipmentId);
            if (test)
            {
                if (pertenencia.Any(p => p.Key == equipmentId))
                {
                    this.ltfotos.Text += "- OK";
                    companyId = pertenencia.First(p => p.Key == equipmentId).Value;
                    var path = this.Request.PhysicalApplicationPath + "\\DOCS\\" + companyId + "\\Equipments";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var finalFile = path + "\\" + Path.GetFileName(file);
                    if (!File.Exists(finalFile))
                    {
                        File.Copy(file, finalFile);
                    }
                }

            }

            this.ltfotos.Text += "<br>";
        }
    }
}