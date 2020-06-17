using ConfigManagerRest.General;
using ConfigManagerRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.Encryption
{
    #region HashEncryption
    public static class HashEncryption
    {
        #region MD5
        public static OperationResult MD5(string Data)
        {
            OperationResult opr = new OperationResult();

            try
            {
                if (Data == "" || Data == null)
                {
                    opr.Message = Data;
                }
                else
                {
                    MD5CryptoServiceProvider password = new MD5CryptoServiceProvider();
                    byte[] arrayPassword = Common.StringToByte(Data);
                    byte[] arrayHash = password.ComputeHash(arrayPassword);
                    opr.Message = BitConverter.ToString(arrayHash);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
                opr.Result = false;
                opr.Message = Data;
            }
            return opr;
        }
        #endregion
    }
    #endregion
}
