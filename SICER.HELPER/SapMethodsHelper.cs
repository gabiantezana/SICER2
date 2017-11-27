using System;
using System.Linq;
using SAPbobsCOM;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.SAPUSERMODEL;

namespace SAPADDON.HELPER
{
    public static class SapMethodsHelper
    {
        public static void CreateUdo(Company company, SAPUDOEntity udo)
        {
            _CreateUDO(company, udo);
        }

        public static void CreateTable(Company company, SAPTableEntity table)
        {
            CreateTable(company, table.TableName, table.TableDescription, table.TableType);
        }

        public static void CreateField(Company company, SapFieldEntity userField)
        {
            CreateField(company, userField.TableName, userField.FieldName, userField.FieldDescription, userField.FieldType, userField.FieldSubType, userField.FieldSize, userField.IsRequired, userField.ValidValues, userField.ValidDescription, userField.DefaultValue, userField.VinculatedTable);
        }

        private static void _CreateUDO(Company company, SAPUDOEntity udo)
        {
            //CreateUDOMD(company, udo.Code, udo.Name, udo.HeaderTableName, udo.|, udo.ChildTableNameList, udo.CanCancel, udo.CanClose, udo.CanDelete, udo.CanCreateDefaultForm, udo.FormColumns, udo.CanFind, udo.CanLog, udo.ObjectType, udo.ManageSeries, udo.EnableEnhancedForm, udo.RebuildEnhancedForm, udo.ChildFormColumns);
        }

        private static void CreateTable(Company _Company, String tableName, String tableDescription, SAPbobsCOM.BoUTBTableType tableType)
        {
            SAPbobsCOM.UserTablesMD oUserTablesMD = (SAPbobsCOM.UserTablesMD)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            try
            {
                if (!oUserTablesMD.GetByKey(tableName))
                {
                    oUserTablesMD.TableName = tableName;
                    oUserTablesMD.TableDescription = tableDescription;
                    oUserTablesMD.TableType = tableType;

                    if (oUserTablesMD.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                        throw new SapException();
                }
            }
            catch (Exception ex) {  throw; }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD);
                oUserTablesMD = null;
                GC.Collect();
            }

        }

        private static void CreateField(Company _Company, String tableName, String fieldName, String fieldDescription, SAPbobsCOM.BoFieldTypes fieldType, SAPbobsCOM.BoFldSubTypes fieldSubType, Int32? fieldSize, SAPbobsCOM.BoYesNoEnum isRequired, String[] validValues, String[] validDescription, String defaultValue, String vinculatedTable)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
            try
            {
                tableName = tableName ?? String.Empty;
                fieldName = fieldName ?? String.Empty;
                fieldDescription = fieldDescription ?? String.Empty;
                fieldSize = fieldSize ?? ConstantHelper.DefaultFieldSize;
                validValues = validValues ?? new String[] { };
                validDescription = validDescription ?? new String[] { };
                defaultValue = defaultValue ?? String.Empty;
                vinculatedTable = vinculatedTable ?? String.Empty;

                //string _tableName = "@" + tableName;
                //int iFieldID = GetUserFieldId(_Company, tableName, fieldName);
                //if (!oUserFieldsMD.GetByKey(tableName, iFieldID))

                //CUFD udf = new SBODemoCLEntities().CUFD.FirstOrDefault(x => x.TableID == tableName && x.AliasID.ToString() == fieldName);
                //if (udf == null)
                {
                    oUserFieldsMD.TableName = tableName;
                    oUserFieldsMD.Name = fieldName;
                    oUserFieldsMD.Description = fieldDescription;
                    oUserFieldsMD.Type = fieldType;
                    if (fieldType != SAPbobsCOM.BoFieldTypes.db_Date) oUserFieldsMD.EditSize = fieldSize.Value;
                    oUserFieldsMD.SubType = fieldSubType;

                    if (vinculatedTable != "") oUserFieldsMD.LinkedTable = vinculatedTable;
                    else
                    {
                        if (validValues.Length > 0)
                        {
                            for (Int32 i = 0; i <= (validValues.Length - 1); i++)
                            {
                                oUserFieldsMD.ValidValues.Value = validValues[i];
                                if (validDescription.Length >= i)
                                    oUserFieldsMD.ValidValues.Description = validDescription[i];
                                else
                                    oUserFieldsMD.ValidValues.Description = validValues[i];

                                oUserFieldsMD.ValidValues.Add();
                            }
                        }
                        oUserFieldsMD.Mandatory = isRequired;
                        if (defaultValue != "") oUserFieldsMD.DefaultValue = defaultValue;
                    }

                    if (oUserFieldsMD.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                        throw new SapException();
                }
            }
            catch (Exception ex) { throw; }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();
            }
        }

        private static void CreateUDOMD(Company _Company, String sCode, String sName, String sTableName, String[] sFindColumns,
            String[] sChildTables, SAPbobsCOM.BoYesNoEnum eCanCancel, SAPbobsCOM.BoYesNoEnum eCanClose,
            SAPbobsCOM.BoYesNoEnum eCanDelete, SAPbobsCOM.BoYesNoEnum eCanCreateDefaultForm, String[] sFormColumns,
            SAPbobsCOM.BoYesNoEnum eCanFind, SAPbobsCOM.BoYesNoEnum eCanLog, SAPbobsCOM.BoUDOObjType eObjectType,
            SAPbobsCOM.BoYesNoEnum eManageSeries, SAPbobsCOM.BoYesNoEnum eEnableEnhancedForm,
            SAPbobsCOM.BoYesNoEnum eRebuildEnhancedForm, String[] sChildFormColumns)
        {
            SAPbobsCOM.UserObjectsMD oUserObjectMD = (SAPbobsCOM.UserObjectsMD)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            try
            {
                if (oUserObjectMD.GetByKey(sCode)) return;
                oUserObjectMD.Code = sCode;
                oUserObjectMD.Name = sName;
                oUserObjectMD.ObjectType = eObjectType;
                oUserObjectMD.TableName = sTableName;
                oUserObjectMD.CanCancel = eCanCancel;
                oUserObjectMD.CanClose = eCanClose;
                oUserObjectMD.CanDelete = eCanDelete;
                oUserObjectMD.CanCreateDefaultForm = eCanCreateDefaultForm;
                oUserObjectMD.EnableEnhancedForm = eEnableEnhancedForm;
                oUserObjectMD.RebuildEnhancedForm = eRebuildEnhancedForm;
                oUserObjectMD.CanFind = eCanFind;
                oUserObjectMD.CanLog = eCanLog;
                oUserObjectMD.ManageSeries = eManageSeries;

                if (sFindColumns != null)
                {
                    foreach (var t in sFindColumns)
                    {
                        oUserObjectMD.FindColumns.ColumnAlias = t;
                        oUserObjectMD.FindColumns.Add();
                    }
                }
                if (sChildTables != null)
                {
                    foreach (string t in sChildTables)
                    {
                        oUserObjectMD.ChildTables.TableName = t;
                        oUserObjectMD.ChildTables.Add();
                    }
                }
                if (sFormColumns != null)
                {
                    oUserObjectMD.UseUniqueFormType = SAPbobsCOM.BoYesNoEnum.tYES;

                    foreach (var t in sFormColumns)
                    {
                        oUserObjectMD.FormColumns.FormColumnAlias = t;
                        oUserObjectMD.FormColumns.Add();
                    }
                }
                if (sChildFormColumns != null)
                {
                    if (sChildTables != null)
                    {
                        foreach (var t in sChildFormColumns)
                        {
                            oUserObjectMD.FormColumns.SonNumber = 1;
                            oUserObjectMD.FormColumns.FormColumnAlias = t;
                            oUserObjectMD.FormColumns.Add();
                        }
                    }
                }
                if (oUserObjectMD.Add() != ConstantHelper.DefaulSuccessSAPNumber)
                    throw new SapException();
            }
            catch (Exception ex) { throw; }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
                oUserObjectMD = null;
                GC.Collect();
            }
        }

        /*
        private static void RegistrarAutorizaciones(Company _Company, String s_PermissionID, String s_PermissionName, SAPPermissionType oPermissionType, String s_FatherID, String s_FormTypeEx)
        {
            SAPbobsCOM.UserPermissionTree oUserPermissionTree = null;

            oUserPermissionTree = (SAPbobsCOM.UserPermissionTree)_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserPermissionTree);
            if (oUserPermissionTree.GetByKey(s_PermissionID)) return;
            oUserPermissionTree.PermissionID = s_PermissionID;
            oUserPermissionTree.Name = s_PermissionName;
            oUserPermissionTree.Options = SAPbobsCOM.BoUPTOptions.bou_FullReadNone;
            if (oPermissionType == SAPPermissionType.pt_child)
            {
                oUserPermissionTree.UserPermissionForms.FormType = s_FormTypeEx;
                oUserPermissionTree.ParentID = s_FatherID;
            }
            if (oUserPermissionTree.Add() != ConstantHelper.DefaulSuccessSAPNumber)
            {
                throw new SapException();
            }
        }*/

        private static int GetUserFieldId(Company company, string tableName, string fieldName)
        {
            var iRetVal = -1;
            var sboRec = (SAPbobsCOM.Recordset)company.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            { //TODO:
                //sboRec.DoQuery(new QueryHelper(company.DbServerType).GetUserFieldId());
                //if (!sboRec.EoF) iRetVal = Convert.ToInt32(sboRec.Fields.Item("FieldID").Value.ToString());
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sboRec);
                sboRec = null;
                GC.Collect();
            }
            return iRetVal;
        }

        private static void CreaCampoMd(Company company, string NombreTabla, string NombreCampo, string DescCampo, SAPbobsCOM.BoFieldTypes TipoCampo, SAPbobsCOM.BoFldSubTypes SubTipo, int Tamano, SAPbobsCOM.BoYesNoEnum Obligatorio, string[] validValues, string[] validDescription, string valorPorDef, string tablaVinculada)
        {
            try
            {
                if (NombreTabla == null) NombreTabla = "";
                if (NombreCampo == null) NombreCampo = "";
                if (Tamano == 0) Tamano = 10;
                if (validValues == null) validValues = new string[0];
                if (validDescription == null) validDescription = new string[0];
                if (valorPorDef == null) valorPorDef = "";
                if (tablaVinculada == null) tablaVinculada = "";

                var oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NombreTabla;
                oUserFieldsMD.Name = NombreCampo;
                oUserFieldsMD.Description = DescCampo;
                oUserFieldsMD.Type = TipoCampo;
                if (TipoCampo != SAPbobsCOM.BoFieldTypes.db_Date) oUserFieldsMD.EditSize = Tamano;
                oUserFieldsMD.SubType = SubTipo;

                if (tablaVinculada != "") oUserFieldsMD.LinkedTable = tablaVinculada;
                else
                {
                    if (validValues.Length > 0)
                    {
                        for (int i = 0; i <= (validValues.Length - 1); i++)
                        {
                            oUserFieldsMD.ValidValues.Value = validValues[i];
                            oUserFieldsMD.ValidValues.Description = validDescription.Length > 0 ? validDescription[i] : validValues[i];
                            oUserFieldsMD.ValidValues.Add();
                        }
                    }
                    oUserFieldsMD.Mandatory = Obligatorio;
                    if (valorPorDef != "") oUserFieldsMD.DefaultValue = valorPorDef;
                }

                int sf = oUserFieldsMD.Add();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        public static string GetUserFieldDBName(String fieldName)
        {
            return "U_" + fieldName;
        }
    }


}
