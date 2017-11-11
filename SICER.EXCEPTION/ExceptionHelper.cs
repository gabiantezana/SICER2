using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using SICER.MODEL;

namespace SICER.EXCEPTION
{
    public sealed class ExceptionHelper
    {
        private ExceptionHelper() { }

        public static void LogException(Exception exc)
        {
            String route = @"C:\LOG\asdf\" + GetProjectName();
            String fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            String logFile = route + @"\" + fileName;

            System.IO.Directory.CreateDirectory(route);

            if (!System.IO.File.Exists(logFile))
                System.IO.File.Create(logFile).Close();

            using (var sw = new System.IO.StreamWriter(logFile, true))
            {

                sw.WriteLine(" * ********* {0} **********", DateTime.Now);

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
        }

        public static void LogException(Exception exc, DataContext dataContext)
        {
            StoreException(exc, dataContext, DateTime.Now);//TODO
            /*
            String route = @"C:\LOG\" + GetProjectName();
            String fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            String logFile = route + @"\" + fileName;

            System.IO.Directory.CreateDirectory(route);

            if (!System.IO.File.Exists(logFile))
                System.IO.File.Create(logFile).Close();

            System.IO.StreamWriter sw = new System.IO.StreamWriter(logFile, true);
            sw.WriteLine(" * ********* {0} **********", DateTime.Now);


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
            GC.Collect();*/
        }

        private static void StoreException(Exception ex, DataContext dataContext, DateTime timeStamp)
        {
            try
            {
                var excepcion = new Excepcion
                {
                    excepcionInterna = ex.InnerException?.Message ?? string.Empty,
                    mensaje = ex.Message,
                    navegador = dataContext?.Browser?.Browser,
                    pila = ex.StackTrace,
                    tipo = ex.GetType().ToString()
                };

                var sessionLength = dataContext?.Session?.Contents?.Count;
                var sessionKeys = string.Empty;
                for (var i = 0; i < sessionLength; i++)
                {
                    var dataSession = dataContext?.Session[i];
                    if (dataSession == null) continue;
                    var dataToString = dataSession.ToString();
                    sessionKeys = "Key: " + dataContext?.Session?.Keys[i] + " Value: " +
                                  dataToString.Substring(0, dataToString.Length > 20 ? 20 : dataToString.Length) + " | ";
                }
                //sessionKeys = sessionKeys.Remove(sessionKeys.Length - 3);//Solo para quitar el ultimo '|' de la cadena.//TODO: REMOVE WHEN NULL IN HANGFIRE
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


        private static string GetProjectName()
        {
            try
            {
                return Assembly.GetCallingAssembly().GetName().Name;

            }
            catch (Exception)
            {
                return "UNDEFINED";
            }
        }
    }
}
