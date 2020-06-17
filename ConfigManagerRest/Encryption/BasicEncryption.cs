using ConfigManagerRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.Encryption
{
    #region BasicEncryption
    public static class BasicEncryption
    {
        #region BasicEncryptDecrypt
        public static OperationResult BasicEncryptDecrypt(string Data, int EncryptionKey)
        {
            OperationResult opr = new OperationResult();

            try
            {
                StringBuilder szInputStringBuild = new StringBuilder(Data);
                StringBuilder szOutStringBuild = new StringBuilder(Data.Length);
                char Textch;
                for (int iCount = 0; iCount < Data.Length; iCount++)
                {
                    Textch = szInputStringBuild[iCount];

                    //Console.WriteLine("first : " + Textch + " - " + (int)Textch);

                    Textch = (char)(Textch ^ EncryptionKey);

                    //Console.WriteLine("last : " + Textch + " - " + (int)Textch);

                    szOutStringBuild.Append(Textch);

                    opr.Message = szOutStringBuild.ToString();
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
