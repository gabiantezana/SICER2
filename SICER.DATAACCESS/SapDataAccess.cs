using SICER.SAPUSERMODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPADDON.HELPER;
using SAPbobsCOM;
using SICER.EXCEPTION;
using SICER.HELPER;

namespace SICER.DATAACCESS
{
    public class SapDataAccess
    {
        public string GenerateSapDbSchema(Company company)
        {
            var errorMessage = string.Empty;

            var dBSchema = new UserModel().GetDBSchema();
            foreach (var x in dBSchema.TableList)
            {
                try
                {
                    SapMethodsHelper.CreateTable(company, x);
                }
                catch (SapException ex)
                {
                    errorMessage += GetSapError(company);
                }
            }
            foreach (var x in dBSchema.FieldList)
            {
                try
                {
                    SapMethodsHelper.CreateField(company, x);

                }
                catch (SapException ex)
                {
                    errorMessage += GetSapError(company);
                }
            }
            foreach (var x in dBSchema.UDOList)
            {
                try
                {

                    SapMethodsHelper.CreateUdo(company, x);
                }
                catch (SapException ex)
                {
                    errorMessage += GetSapError(company);
                }
            }

            return errorMessage;
        }

        #region Company 

        private static Company Company = null;

        public static Company GetCommpany()
        {
            return Company;
        }

        public static void Connect(CompanyEntity model)
        {
            var company = CreateNewCompanyFromModel(model);
            if (!company.Connected)
                Connect(company);
        }

        public static void ConnectNewCompany(CompanyEntity model)
        {
            Disconnect();
            Company = CreateNewCompanyFromModel(model);
            Connect(Company);
        }

        private static Company CreateNewCompanyFromModel(CompanyEntity model)
        {
            Company _Company = new Company
            {
                DbServerType = model.DbServerType,
                Server = model.Server,
                UseTrusted = false,
                DbUserName = model.DbUserName,
                DbPassword = model.DbPassword,
                CompanyDB = model.CompanyDB,
                UserName = model.UserName,
                Password = model.Password,
                LicenseServer = model.LicenseServer,
            };
            return _Company;
        }

        public static void Disconnect()
        {
            try
            {
                if (Company != null)
                {
                    if (Company.Connected)
                        Company.Disconnect();
                    Company = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void Connect(Company company)
        {
            Int32 resultReturn = company.Connect();
            if (resultReturn != 0)
                throw new SapException();
        }

        public static SapExceptionEntity GetLastSapError(Company Company)
        {
            Int32 errorCode = default(Int32);
            String errorMessage = String.Empty;
            if (Company != null)
            {
                errorCode = Company.GetLastErrorCode();
                errorMessage = Company.GetLastErrorDescription();
            }
            return new SapExceptionEntity { ErrorCode = errorCode, ErrorMessage = errorMessage };
        }

        public static string GetSapError(Company Company)
        {
            var error = GetLastSapError(Company);
            return error.ErrorCode + " " + error.ErrorMessage;
        }



        #endregion
    }
}
