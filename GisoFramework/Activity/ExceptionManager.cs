// -----------------------------------------------------------------------
// <copyright file="ExceptionManager.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Activity
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Implements the ExceptionManager class
    /// </summary>
    public sealed class ExceptionManager
    {
        /// <summary>
        /// Prevents a default instance of the ExceptionManager class from being created.
        /// </summary>
        private ExceptionManager()
        {
        }

        /// <summary>
        /// Write a trace line on a log daily file
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        public static void Trace(Exception ex, string source)
        {
            Trace(ex, source, string.Empty);
        }

        /// <summary>
        /// Trace a exception into log file
        /// </summary>
        /// <param name="ex">Exception occurred</param>
        /// <param name="source">Source of exception</param>
        /// <param name="extraData">Data extra of exception</param>
        public static void Trace(Exception ex, string source, string extraData)
        {
            string message = string.Empty;
            if(ex == null)
            {
                message = string.Empty;
            }

            if (ex != null)
            {
                message = ex.Message;
            }

            if (string.IsNullOrEmpty(source))
            {
                source = string.Empty;
            }

            if (string.IsNullOrEmpty(extraData))
            {
                extraData = string.Empty;
            }

            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\", StringComparison.Ordinal))
            {
                path = string.Format(CultureInfo.InstalledUICulture, @"{0}\Log\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
            }
            else
            {
                path = string.Format(CultureInfo.InstalledUICulture, @"{0}Log\Errors_{1}.txt", path, DateTime.Now.ToString("yyyyMMdd", CultureInfo.GetCultureInfo("es-es")));
            }

            string line = string.Format(CultureInfo.InstalledUICulture, "{0}::{1}::{2}::{3}", DateTime.Now.ToString("hh:mm:ss", CultureInfo.GetCultureInfo("es-es")), ex.Message, source, extraData);

            using (StreamWriter output = new StreamWriter(path, true))
            {
                output.WriteLine(line);
                output.Flush();
            }
        }
    }
}
