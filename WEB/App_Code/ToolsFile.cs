using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ToolsFile
/// </summary>
public class ToolsFile
{
    public ToolsFile()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static ReadOnlyCollection<string> ExtensionToShow
    {
        get
        {
            List<string> res = new List<string>();
            res.Add(".txt");
            //res.Add(".pdf");
            res.Add(".png");
            res.Add(".gif");
            res.Add(".jpg");

            return new ReadOnlyCollection<string>(res);
        }
    }

    public static decimal FormatSize(decimal size)
    {
        int res = Convert.ToInt16(((decimal)size / 1024 / 1024) * 100);

        if (res == 0)
        {
            res = 1;
        }

        return (decimal)res / 100;
    }
}