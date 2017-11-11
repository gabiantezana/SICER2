using System;
using SAPbobsCOM;
using SAPWS.HELPER;
using SICER.DATAACCESS;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;

namespace SICER.LOGIC
{
    public class BaseLogic
    {
        public void InitializeApplication(DataContext dataContext)
        {
            new BaseDataAccess(dataContext).GenerateDbSchema();
        }

        #region Company 

        public void ConnectCompany(String xml)
        {
            if (String.IsNullOrEmpty(xml))
                ConnectCompanyFromConstantResource();
            else
                ConnectCompanyFromXML(xml);
        }

        private void ConnectCompanyFromConstantResource()
        {
            CompanyEntity model = GetCompanyEntityFromFile();
            BaseDataAccess.ConnectNewCompany(model);

        }

        private void ConnectCompanyFromXML(String xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                CompanyEntity model = SerializeHelper.XMLToObject(xml, typeof(CompanyEntity));
                BaseDataAccess.ConnectNewCompany(model);
            }
            else
                throw new CustomException("You need to specify a xml string with your company data");
        }

        public void DisconnectCompany()
        {
            BaseDataAccess.Disconnect();
        }

        public Company ConnectAndGetCurrentCompany()
        {
            CompanyEntity model = GetCompanyEntityFromFile();
            BaseDataAccess.Connect(model);
            return BaseDataAccess.GetCommpany();

        }

        public SapExceptionEntity GetLastSapError()
        {
            return BaseDataAccess.GetLastSapError();
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

        public CompanyEntity GetCurrentCompany()
        {
            Company company = BaseDataAccess.GetCommpany();
            //TODOG: Not working on server client, just local
            /*CompanyEntity model = ReflectionHelper.CopyAToB(company, typeof(CompanyEntity), true);*/

            CompanyEntity model = new CompanyEntity();

            if (company != null)
            {
                model.CompanyDB = company.CompanyDB;
                model.DbPassword = company.DbPassword;
                model.DbServerType = company.DbServerType;
                model.DbUserName = company.DbUserName;
                model.language = company.language;
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

        private CompanyEntity GetCompanyEntityFromFile()
        {
            String xml = System.IO.File.ReadAllText(XMLParametersPath);
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
}
