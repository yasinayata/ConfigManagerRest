using ConfigManagerRest.Database;
using ConfigManagerRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.General
{
    #region Common
    [Serializable]    
    public static class Common
    {
        public static bool isServiceActive = false;
        public static DatabaseProcess dbProvider;

        internal enum WebApiExecType
        {
            GET,PUT, POST, LIST, DELETE, ADDANDUPDATE
        }

        #region Variables
        public static byte[] Key = { 8, 7, 6, 5, 4, 3, 2, 1 };
        public static byte[] IV = { 50, 40, 30, 20, 10, 80, 70, 60, 80, 70, 60, 50, 40, 30, 20, 10, 00 };
        public static string stringKey = "1Qaz2Wsx";
        #endregion

        #region StringToByte
        public static byte[] StringToByte(string Data)
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            return ByteConverter.GetBytes(Data);
        }
        #endregion

        #region Byte8
        public static byte[] Byte8(string Data)
        {
            char[] arrayChar = Data.ToCharArray();
            byte[] arrayByte = new byte[arrayChar.Length];
            for (int i = 0; i < arrayByte.Length; i++)
            {
                arrayByte[i] = Convert.ToByte(arrayChar[i]);
            }
            return arrayByte;
        }
        #endregion       

        #region CreateProvider
        public static OperationResult CreateProvider()
        {
            OperationResult op = new OperationResult();
            MsSqlProcess sql;
            IDatabaseProcess dbProvider = null;

            try
            {
                switch (ConfigProcess.DBProvider)
                {
                    case "MsSql":
                        sql = new MsSqlProcess { SqlConnectionString  = ConfigProcess.SqlConnectionString };
                        sql.SqlConnectionString = ConfigProcess.SqlConnectionString;
                        dbProvider = sql;
                        break;
                    case "MySql":
                        //dbProvider = new MsSqlProcess();
                        break;
                    case "Mongo":
                        //dbProvider = new MsSqlProcess();
                        break;
                    default:
                        //dbProvider = new MsSqlProcess();
                        break;
                }
                Common.dbProvider = new DatabaseProcess(dbProvider);

            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion
    }
    #endregion   
}
