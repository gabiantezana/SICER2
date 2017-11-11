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
using SICER.MODEL;

namespace SICER.DATAACCESS
{
    public class BaseDataAccess
    {
        private DataContext DataContext { get; set; }
        public BaseDataAccess(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public void GenerateDbSchema()
        {
            var dBSchema = new UserModel().GetDBSchema();
            dBSchema.TableList.ForEach(x => SapMethodsHelper.CreateTable(DataContext.Company, x));
            dBSchema.FieldList.ForEach(x => SapMethodsHelper.CreateField(DataContext.Company, x));
            dBSchema.UDOList.ForEach(x => SapMethodsHelper.CreateUdo(DataContext.Company, x));
        }

        #region Company 

        private static Company Company = null;

        public static Company GetCommpany()
        {
            return Company;
        }

        public static void Connect(CompanyEntity model)
        {
            if (Company == null)
                Company = CreateNewCompanyFromModel(model);
            if (!Company.Connected)
                Connect();
        }

        public static void ConnectNewCompany(CompanyEntity model)
        {
            Disconnect();
            Company = CreateNewCompanyFromModel(model);
            Connect();
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

        private static void Connect()
        {
            Int32 resultReturn = Company.Connect();
            if (resultReturn != 0)
                throw new SapException();
        }

        public static void TryConnectCurrentCompany()
        {
            if (Company == null)
                throw new CustomException("Any company for connect.");
            if (!Company.Connected)
                Connect();
        }

        public static SapExceptionEntity GetLastSapError()
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

        #endregion
    }
}
