using System;
using SAPbobsCOM;
using SICER.DATAACCESS;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using Company = SAPbobsCOM.Company;

namespace SICER.LOGIC
{
    public class SapLogic
    {
        public static string GenerateSapDbSchema(Company company)
        {
           return  new SapDataAccess().GenerateSapDbSchema(company);

        }

        #region Company 

        public void ConnectCompany(string xml)
        {
            if (String.IsNullOrEmpty(xml))
                ConnectCompanyFromConstantResource();
            else
                ConnectCompanyFromXML(xml);
        }

        private void ConnectCompanyFromConstantResource()
        {
            CompanyEntity model = GetCompanyEntityFromFile();
            SapDataAccess.ConnectNewCompany(model);

        }

        private void ConnectCompanyFromXML(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                CompanyEntity model = SerializeHelper.XMLToObject(xml, typeof(CompanyEntity));
                SapDataAccess.ConnectNewCompany(model);
            }
            else
                throw new CustomException("You need to specify a xml string with your company data");
        }

        public void DisconnectCompany()
        {
            SapDataAccess.Disconnect();
        }

        public Company ConnectAndGetCurrentCompany()
        {
            CompanyEntity model = GetCompanyEntityFromFile();
            SapDataAccess.Connect(model);
            return SapDataAccess.GetCommpany();
        }

        public String GetLastDocEntry()
        {
            string sNewObjCode = String.Empty;
            try
            {
                ConnectAndGetCurrentCompany().GetNewObjectCode(out sNewObjCode);
                return sNewObjCode;
            }
            catch (Exception ex)
            {
                sNewObjCode = "Error in GetDocEntry()";
            }
            return sNewObjCode;
        }

        public string GetLastError(Company Company)
        {
            Int32 errorCode = default(Int32);
            String errorMessage = String.Empty;
            if (Company != null)
            {
                errorCode = Company.GetLastErrorCode();
                errorMessage = Company.GetLastErrorDescription();
            }
            return errorCode + " - " + errorMessage;
        }

        public CompanyEntity GetCurrentCompany()
        {
            Company company = SapDataAccess.GetCommpany();
            //TODOG: Not working on server client, just local
            /*CompanyEntity model = ReflectionHelper.CopyAToB(company, typeof(CompanyEntity), true);*/

            CompanyEntity model = new CompanyEntity();

            if (company != null)
            {
                model.CompanyDB = company.CompanyDB;
                model.DbPassword = company.DbPassword;
                model.DbServerType = company.DbServerType;
                model.DbUserName = company.DbUserName;
                model.BoSuppLangs = company.language;
                model.LicenseServer = company.LicenseServer;
                model.Password = company.Password;
                model.Server = company.Server;
                model.UserName = company.UserName;
                model.UseTrusted = company.UseTrusted;
                model.XMLAsString = company.XMLAsString;
                model.Connected = company.Connected;
            }
            return model;
        }

        public CompanyEntity GetCompanyEntityFromFile()
        {
            var xml = System.IO.File.ReadAllText(XMLParametersPath);
            CompanyEntity model = SerializeHelper.XMLToObject(xml, typeof(CompanyEntity));
            return model;
        }

        private static string XMLParametersPath
        {
            get
            {
                string pathDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                string pathArch = System.IO.Path.Combine(pathDir, ConstantHelper.ParameterPath);
                var localPath = new Uri(pathArch).LocalPath;
                return localPath;
            }
        }
        
        #endregion
    }
    public static class ExtensionHelper
    {
        //public static BoDataServerTypes GetDataServerTypes(this DataContext dataContext)
        //{
        //    if (dataContext.Company == null)
        //    {
        //        dataContext.Company = new SapLogic().ConnectAndGetCurrentCompany();
        //    }
        //    return dataContext.Company.DbServerType;
        //}
        //public static Company GetAndConnectCurrentCompany(this DataContext dataContext)
        //{
        //    dataContext.Company = new SapLogic().ConnectAndGetCurrentCompany();
        //    return dataContext.Company;
        //}
    }

}
