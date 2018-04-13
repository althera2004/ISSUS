using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using iTS = iTextSharp.text;
using iTSpdf = iTextSharp.text.pdf;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using PDF_Tests;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using System.Data.SqlClient;
using System.Data;

public partial class Export_ExportBackUp : Page
{
    private struct ItemConfiguration
    {
        public string ItemName;
        public string ItemTable;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public static ActionResult Go(int companyId)
    {
        var res = ActionResult.NoAction;
        var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
        var company = new Company(companyId);
        string fileName = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}_{1}_{2:yyyyMMddhhmmss}.xls",
            dictionary["Item_Backup"],
            company.Name,
            DateTime.Now);

        var wb = HSSFWorkbook.Create(InternalWorkbook.CreateWorkbook());

        var items = new List<ItemConfiguration>
        {
            new ItemConfiguration() { ItemName = "User", ItemTable = "ApplicationUser" },
            new ItemConfiguration() { ItemName = "Process", ItemTable = "Proceso" },
            new ItemConfiguration() { ItemName = "Department", ItemTable = "Department" },
            new ItemConfiguration() { ItemName = "Employee", ItemTable = "Employee" },
            new ItemConfiguration() { ItemName = "EmployeeSkills", ItemTable = "EmployeeSkills" },
            new ItemConfiguration() { ItemName = "Learning", ItemTable = "Learning" },
            new ItemConfiguration() { ItemName = "LearningAssistant", ItemTable = "LearningAssistant" },
            new ItemConfiguration() { ItemName = "Customer", ItemTable = "Customer" },
            new ItemConfiguration() { ItemName = "Provider", ItemTable = "Provider" },
            new ItemConfiguration() { ItemName = "Unidad", ItemTable = "Unidad" },
            new ItemConfiguration() { ItemName = "CostDefinition", ItemTable = "CostDefinition" },
            new ItemConfiguration() { ItemName = "Equipment", ItemTable = "Equipment" },
            new ItemConfiguration() { ItemName = "EquipmentScaleDivision", ItemTable = "EquipmentScaleDivision" },
            new ItemConfiguration() { ItemName = "EquipmentCalibrationDefinition", ItemTable = "EquipmentCalibrationDefinition" },
            new ItemConfiguration() { ItemName = "EquipmentCalibrationAct", ItemTable = "EquipmentCalibrationAct" },
            new ItemConfiguration() { ItemName = "EquipmentVerificationDefinition", ItemTable = "EquipmentVerificationDefinition" },
            new ItemConfiguration() { ItemName = "EquipmentVerificationAct", ItemTable = "EquipmentVerificationAct" },
            new ItemConfiguration() { ItemName = "EquipmentMaintenanceDefinition", ItemTable = "EquipmentMaintenanceDefinition" },
            new ItemConfiguration() { ItemName = "EquipmentMaintenanceAct", ItemTable = "EquipmentMaintenanceAct" },
            new ItemConfiguration() { ItemName = "EquipmentRepair", ItemTable = "EquipmentRepair" },
            new ItemConfiguration() { ItemName = "JobPosition", ItemTable = "Cargos" },
            new ItemConfiguration() { ItemName = "Document", ItemTable = "Document" },
            new ItemConfiguration() { ItemName = "Document_Category", ItemTable = "Document_Category" },
            new ItemConfiguration() { ItemName = "Incident", ItemTable = "Incident" },
            new ItemConfiguration() { ItemName = "IncidentCost", ItemTable = "IncidentCost" },
            new ItemConfiguration() { ItemName = "IncidentAction", ItemTable = "IncidentAction" },
            new ItemConfiguration() { ItemName = "IncidentActionCost", ItemTable = "IncidentActionCost" },
            new ItemConfiguration() { ItemName = "BusinessRisk", ItemTable = "BusinessRisk3" },
            new ItemConfiguration() { ItemName = "Indicador", ItemTable = "Indicador" },
            new ItemConfiguration() { ItemName = "Objetivo", ItemTable = "Objetivo" }
        };

        foreach (var item in items)
        {
            var sh = (HSSFSheet)wb.CreateSheet(dictionary["Item_" + item.ItemName].Replace('/','-'));
            string query = "Select * from " + item.ItemTable + " WHERE CompanyId = " + companyId.ToString();
            using(var cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            bool first = true;
                            int row = 1;
                            while (rdr.Read())
                            {
                                if (first)
                                {
                                    if (sh.GetRow(0) == null) { sh.CreateRow(0); }
                                    for (int x = 0; x < rdr.FieldCount; x++)
                                    {
                                        if (sh.GetRow(0).GetCell(x) == null) { sh.GetRow(0).CreateCell(x); }
                                        sh.GetRow(0).GetCell(x).SetCellValue(rdr.GetName(x));
                                    }

                                    first = false;
                                }

                                if (sh.GetRow(row) == null) { sh.CreateRow(row); }
                                for (int x = 0; x < rdr.FieldCount; x++)
                                {
                                    if (!rdr.IsDBNull(x))
                                    {
                                        if (sh.GetRow(row).GetCell(x) == null) { sh.GetRow(row).CreateCell(x); }
                                        sh.GetRow(row).GetCell(x).SetCellValue(rdr[x].ToString());
                                    }
                                }

                                row++;
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


        }

        string path = HttpContext.Current.Request.PhysicalApplicationPath;

        if (!path.EndsWith(@"\", StringComparison.OrdinalIgnoreCase))
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}\Temp\", path);
        }
        else
        {
            path = string.Format(CultureInfo.InvariantCulture, @"{0}Temp\", path);
        }

        using (var fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create, FileAccess.Write))
        {
            wb.Write(fs);
        }
        res.SetSuccess(string.Format("/Temp/{0}", fileName));
        return res;
    }
}