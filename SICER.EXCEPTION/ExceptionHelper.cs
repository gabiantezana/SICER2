using System;
using System.IO;
using System.Web;
using System.Configuration;
using SICER.MODEL;

namespace SICER.EXCEPTION
{
    public static class ExceptionHelper
    {
        public static void LogException(Exception exc)
        {

            var fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            var logFile = @"D:\" + fileName;

            if (!File.Exists(logFile))
                File.Create(logFile).Close();

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);

            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Stack Trace: ");
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }

            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

        public static void LogException(Exception exc, DataContext dataContext)
        {
            String ruta = ConfigurationManager.AppSettings["RUTA_LOG"];
            if (!String.IsNullOrEmpty(ruta))
                logFile = ruta.ToString() + fileName;

            if (!File.Exists(logFile))
                File.Create(logFile).Close();

            using (StreamWriter sw = new StreamWriter(logFile, true))
            {
                sw.WriteLine("******************************************* {0} *****************************************", DateTime.Now);

                sw.WriteLine("Browser: " + dataContext.Browser.Browser);
                sw.WriteLine("Browser Id: " + dataContext.Browser.Id);
                sw.WriteLine("Browser Platform: " + dataContext.Browser.Platform);
                sw.WriteLine("Browser Version: " + dataContext.Browser.Version);

                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }

                sw.WriteLine("Session Values: ");
                int sessionLength = dataContext.Session.Contents.Count;
                for (int i = 0; i < sessionLength; i++)
                {
                    if (dataContext.Session[i] != null)
                        sw.WriteLine("Key: " + dataContext.Session.Keys[i] + " Value: " + dataContext.Session[i].ToString());
                }

                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);

                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.Close();
            }
            //StoreException(exc, dataContext, DateTime.Now);
            GC.Collect();
        }

        /*
        private static void StoreException(Exception ex, InformationContext dataContext, DateTime timeStamp)
        {
            try
            {
                Excepcion excepcion = new Excepcion();
                excepcion.excepcionInterna = ex.InnerException != null ? ex.InnerException.Message : String.Empty;
                excepcion.mensaje = ex.Message;
                excepcion.navegador = dataContext.Browser.Browser;
                excepcion.pila = ex.StackTrace;
                excepcion.tipo = ex.GetType().ToString();

                int sessionLength = dataContext.Session.Contents.Count;
                string sessionKeys = string.Empty;
                for (int i = 0; i < sessionLength; i++)
                {
                    var dataSession = dataContext.Session[i];
                    if (dataSession != null)
                    {
                        var dataToString = dataSession.ToString();
                        sessionKeys = "Key: " + dataContext.Session.Keys[i] + " Value: " +
                            dataToString.Substring(0, dataToString.Length > 20 ? 20 : dataToString.Length) + " | ";
                    }
                }


                sessionKeys = sessionKeys.Remove(sessionKeys.Length - 3);//Solo para quitar el ultimo '|' de la cadena.
                excepcion.sessionKeys = sessionKeys;
                excepcion.timeStamp = timeStamp.ToString();
                dataContext.Context.Excepcion.Add(excepcion);
                dataContext.Context.SaveChanges();
            }
            catch (Exception intraEx)
            {
                LogException(intraEx);
            }
        }
        */

        private static String fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        private static String logFile = @"D:\" + fileName;

    }
}
