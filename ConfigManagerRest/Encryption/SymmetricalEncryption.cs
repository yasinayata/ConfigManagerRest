using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ConfigManagerRest.General;
using ConfigManagerRest.Models;

namespace ConfigManagerRest.Encryption
{
    #region SymmetricalEncryption
    public static class SymmetricalEncryption
    {
        #region DES
        #region DESEncrypted
        public static OperationResult DESEncrypted(string Data)      //RSA encrypted process
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    Common.Key = Common.Byte8("35795164");
                    Common.IV =  Common.Byte8("35795164");

                    DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(Common.Key, Common.IV), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(Data);
                    writer.Flush();
                    cs.FlushFinalBlock();
                    writer.Flush();
                    opr.Message = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

                    writer.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }

            }
            catch (Exception exception)
            {
                opr.Result = false;
                opr.Message = exception.Message.ToString();
            }
            return opr;
        }
        #endregion

        #region DESDecrypted
        public static OperationResult DESDecrypted(string Data)
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    Common.Key = Common.Byte8("35795164");
                    Common.IV =  Common.Byte8("35795164");

                    DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(Data));
                    CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(Common.Key, Common.IV), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cs);
                    opr.Message = reader.ReadToEnd();
                    reader.Dispose();
                    cs.Dispose();
                    ms.Dispose();
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
        #endregion

        #region DES LimitedTime
        #region DESEncryptedLimitedTime
        public static OperationResult DESEncryptedLimitedTime(string Data, DateTime ExpirationDateTime)      //RSA encrypted process
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {

                    Common.stringKey = "Config02";
                    Common.Key = Encoding.UTF8.GetBytes(Common.stringKey.Substring(0, 8));
                    Common.IV = Common.Byte8("35795164");

                    Byte[] arrayData = Encoding.UTF8.GetBytes(Data);

                    DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(Common.Key, Common.IV), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(Data);
                    writer.Flush();
                    cs.FlushFinalBlock();
                    writer.Flush();

                    string EncryptedText = Convert.ToBase64String(ms.ToArray());
                    //Console.WriteLine("Clear text : " + EncryptedText);
                    byte[] time = BitConverter.GetBytes(ExpirationDateTime.ToBinary());
                    Common.Key = System.Text.Encoding.ASCII.GetBytes(EncryptedText);
                    EncryptedText = Convert.ToBase64String(time.Concat(Common.Key).ToArray());
                    //Console.WriteLine("Clear text : " + EncryptedText);

                    opr.Message = EncryptedText; // Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

                    writer.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }

            }
            catch (Exception exception)
            {
                opr.Result = false;
                opr.Message = exception.Message.ToString();
            }
            return opr;
        }
        #endregion

        #region DESDecryptedLimitedTime
        public static OperationResult DESDecryptedLimitedTime(string Data)
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    byte[] arrayData = Convert.FromBase64String(Data);
                    string EncryptedText = Encoding.ASCII.GetString(arrayData, 8, arrayData.Length - 8);
                    EncryptedText = EncryptedText.Trim();

                    DateTime when = DateTime.FromBinary(BitConverter.ToInt64(arrayData, 0));
                    if (when < DateTime.Now)
                    {
                        opr.Result = false;
                        opr.Message = "Token is over...";
                        return opr;
                    }

                    Common.stringKey = "Config02";
                    Common.Key = Encoding.UTF8.GetBytes(Common.stringKey.Substring(0, 8));
                    Common.IV = Common.Byte8("35795164");

                    DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(EncryptedText));
                    CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(Common.Key, Common.IV), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cs);
                    opr.Message = reader.ReadToEnd();
                    reader.Dispose();
                    cs.Dispose();
                    ms.Dispose();
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
        #endregion

        #region TripleDES 
        #region TripleDESEncrypted
        public static OperationResult TripleDESEncrypted(string Data)      //RSA encrypted process
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    Common.Key = Common.Byte8("987654321098765432109876");
                    Common.IV =  Common.Byte8("35795164");

                    TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, csp.CreateEncryptor(Common.Key, Common.IV), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(Data);
                    writer.Flush();
                    cs.FlushFinalBlock();
                    writer.Flush();
                    opr.Message = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);

                    writer.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }

            }
            catch (Exception exception)
            {
                opr.Result = false;
                opr.Message = exception.Message.ToString();
            }
            return opr;
        }
        #endregion

        #region TripleDESDecrypted
        public static OperationResult TripleDESDecrypted(string Data)
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    Common.Key = Common.Byte8("987654321098765432109876");
                    Common.IV = Common.Byte8("35795164");

                    TripleDESCryptoServiceProvider csp = new TripleDESCryptoServiceProvider();
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(Data));
                    CryptoStream cs = new CryptoStream(ms, csp.CreateDecryptor(Common.Key, Common.IV), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cs);
                    opr.Message = reader.ReadToEnd();
                    reader.Dispose();
                    cs.Dispose();
                    ms.Dispose();
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
        #endregion

        #region Rijndael
        #region RijndaelEncrypted
        public static OperationResult RijndaelEncrypted(string Data)      //RSA encrypted process
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    Common.Key = Common.Byte8("35795164");
                    Common.IV =  Common.Byte8("9876543210987654");

                    RijndaelManaged dec = new RijndaelManaged { Mode = CipherMode.CBC };
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, dec.CreateEncryptor(Common.Key, Common.IV), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cs);
                    writer.Write(Data);
                    writer.Flush();
                    cs.FlushFinalBlock();
                    writer.Flush();
                    opr.Message = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                    writer.Dispose();
                    cs.Dispose();
                }

            }
            catch (Exception exception)
            {
                opr.Result = false;
                opr.Message = exception.Message.ToString();
            }
            return opr;
        }
        #endregion

        #region RijndaelDecrypted
        public static OperationResult RijndaelDecrypted(string Data)
        {
            OperationResult opr = new OperationResult();
            try
            {
                if (String.IsNullOrEmpty(Data))
                {
                    opr.Result = false;
                    opr.Message = Data;
                }
                else
                {
                    Common.Key = Common.Byte8("35795164");
                    Common.IV =  Common.Byte8("9876543210987654");

                    RijndaelManaged cp = new RijndaelManaged();
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(Data));
                    CryptoStream cs = new CryptoStream(ms, cp.CreateDecryptor(Common.Key, Common.IV), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cs);
                    opr.Message = reader.ReadToEnd();
                    reader.Dispose();
                    cs.Dispose();
                    ms.Dispose();
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
        #endregion
    }
    #endregion
}
