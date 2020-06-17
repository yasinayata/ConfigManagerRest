using ConfigManagerRest.Serialization;
using ConfigManagerRest.General;
using ConfigManagerRest.Models;
using ConfigManagerRest.Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigManagerRest.Models
{
    #region ConfigProperties
    internal static class ConfigProcess
    {
        #region Config Process - user defination area
        private static FileSystemWatcher configFileWatcher;
        private static ServiceConfig config = null;

        public static string SqlConnectionString = "";
        public static string DBProvider = "MsSql";
        public static string configFile = "ConfigManagerRest.Json";

        private static int DebugMode = 0;

        public static int TraceLevel = 0;
        public static string TraceProcess = "";
        public static string TracePath = "";
        public static string TraceFileName = "ConfigManagerRest";

        public static string RaiseEventToEveryone = "0";

        public static string ServiceId = "-1";
        public static string ServiceName = "ConfigManagerRest";

        private static string PublishType = "net.tcp";
        private static string NetTcpAddress = "";
        private static string HttpAddress = "";

        public static int MultiThreadOperation = 0;
        #endregion

        #region Start
        public static void Start()
        {
            //string ProcessName = "ConfigProcess - Start";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            configFileWatcher = new FileSystemWatcher
            {
                Path = AppDomain.CurrentDomain.BaseDirectory + "..\\Config",
                Filter = configFile,
            };

            //configFileWatcher.Changed += LoadConfigFileXml;
            configFileWatcher.Changed += LoadConfigFileJson;

            //LoadConfigFileXml(null, null);
            LoadConfigFileJson(null, null);
        }
        #endregion

        #region LoadConfigFileJson
        private static void LoadConfigFileJson(object sender, FileSystemEventArgs e)
        {
            string ProcessName = "LoadConfigFileJson";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            configFileWatcher.EnableRaisingEvents = false;

            Logger.Log.TraceLevel = 1;
            Logger.Log.TraceProcess = "";
            Logger.Log.TraceFileName = TraceFileName;

            try
            {
                Thread.Sleep(1000);

                configFile = configFile.Replace(".xml", ".json");
                var l_path = AppDomain.CurrentDomain.BaseDirectory + "..\\Config\\" + configFile;

                string JSonText = File.ReadAllText(l_path).DecodeJSString();
                
                JSonText = JObject.Parse(JSonText).ToString();
                //string JSonText = File.ReadAllText(l_path).ToString();
                ServiceConfigJson observiceConsfigJson = JSonText.Deserialize<ServiceConfigJson>();

                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "l_path : " + l_path + " File.Exist : " + File.Exists(l_path));

                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, " Json : " + observiceConsfigJson.Serialize());

                DebugMode = observiceConsfigJson.DebugMode; //Convert.ToInt16(ReadPrameter(config, "DebugMode", "0"));

                ServiceId = observiceConsfigJson.ServiceId; // ReadPrameter(config, "ServiceId", ServiceId);
                ServiceName = observiceConsfigJson.ServiceName; // ReadPrameter(config, "ServiceName", ServiceName);

                TraceLevel = observiceConsfigJson.TraceLevel;   //Convert.ToInt16(ReadPrameter(config, "TraceLevel", "0"));
                TraceProcess = observiceConsfigJson.TraceProcess;   //ReadPrameter(config, "TraceProcess");
                TracePath = observiceConsfigJson.TracePath; // ReadPrameter(config, "TracePath", AppDomain.CurrentDomain.BaseDirectory + "..\\Config\\");
                TraceFileName = observiceConsfigJson.TraceFileName; // ReadPrameter(config, "TraceFileName", ServiceName);
                SqlConnectionString = observiceConsfigJson.SqlConnectionString.DESDecrypted().Message; //ReadPrameter(config, "SqlConnectionString").DESDecrypted().Message;

                DBProvider = observiceConsfigJson.Provider == null ? DBProvider : observiceConsfigJson.Provider ; //eadPrameter(config, "DBProvider", "MsSql");
                MultiThreadOperation = observiceConsfigJson.MultiThreadOperation;   //Convert.ToInt16(ReadPrameter(config, "MultiThreadOperation", "0"));

                RaiseEventToEveryone = observiceConsfigJson.RaiseEventToEveryone.ToString();   //ReadPrameter(config, "RaiseEventToEveryone", "0");

                Logger.Log.TraceLevel = TraceLevel;
                Logger.Log.TraceProcess = TraceProcess;
                Logger.Log.TracePath = TracePath;
                Logger.Log.TraceFileName = TraceFileName;

                Logger.Log.WriteLog(Logger.Log.LogLevel.Level2, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "======================================================================");
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TraceLevel : " + TraceLevel);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TraceProcess : " + TraceProcess);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TracePath : " + TracePath);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TraceFileName : " + TraceFileName);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "RaiseEventToEveryone : " + RaiseEventToEveryone);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "SqlConnectionString : " + SqlConnectionString.DESEncrypted().Message);    //SymmetricalEncryption.DESEncrypted(SqlConnectionString).Message);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "Provider : " + DBProvider);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "ServiceId : " + ServiceId);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "ServiceName : " + ServiceName);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "MultiThreadOperation : " + MultiThreadOperation);
            }
            catch (Exception ex)
            {
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level0, Logger.Log.LogType.Exception, CurrentThreadId, ProcessName, "Exception : " + ex.ToString());
            }

            configFileWatcher.EnableRaisingEvents = true;
        }
        #endregion

        #region LoadConfigFile
        private static void LoadConfigFileXml(object sender, FileSystemEventArgs e)
        {
            string ProcessName = "LoadConfigFileXml";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            configFileWatcher.EnableRaisingEvents = false;

            Logger.Log.TraceLevel = 1;
            Logger.Log.TraceProcess = "";
            Logger.Log.TraceFileName = TraceFileName;

            try
            {
                Thread.Sleep(1000);

                XmlSerializer xs = new XmlSerializer(typeof(ServiceConfig));

                var l_document = new XmlDocument();
                var l_path = AppDomain.CurrentDomain.BaseDirectory + "..\\Config\\" + configFile;
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "l_path : " + l_path + " File.Exist : " + File.Exists(l_path));

                l_document.Load(l_path);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "l_document.InnerXml : " + l_document.InnerXml);

                StringReader sr = new StringReader(l_document.InnerXml);

                config = (ServiceConfig)xs.Deserialize(sr);

                DebugMode = Convert.ToInt16(ReadPrameter(config, "DebugMode", "0"));

                ServiceId = ReadPrameter(config, "ServiceId", ServiceId);
                ServiceName = ReadPrameter(config, "ServiceName", ServiceName);

                TraceLevel = Convert.ToInt16(ReadPrameter(config, "TraceLevel", "0"));
                TraceProcess = ReadPrameter(config, "TraceProcess");
                TracePath = ReadPrameter(config, "TracePath", AppDomain.CurrentDomain.BaseDirectory + "..\\Config\\");
                TraceFileName = ReadPrameter(config, "TraceFileName", ServiceName);
                SqlConnectionString = ReadPrameter(config, "SqlConnectionString").DESDecrypted().Message;

                DBProvider = ReadPrameter(config, "DBProvider","MsSql");
                MultiThreadOperation = Convert.ToInt16(ReadPrameter(config, "MultiThreadOperation", "0"));

                RaiseEventToEveryone = ReadPrameter(config, "RaiseEventToEveryone", "0");

                PublishType = ReadPrameter(config, "PublishType", "net.tcp");

                NetTcpAddress = ReadPrameter(config, "NetTcpAddress");
                HttpAddress = ReadPrameter(config, "HttpAddress");

                Logger.Log.TraceLevel = TraceLevel;
                Logger.Log.TraceProcess = TraceProcess;
                Logger.Log.TracePath = TracePath;
                Logger.Log.TraceFileName = TraceFileName;

                Logger.Log.WriteLog(Logger.Log.LogLevel.Level2, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "======================================================================");
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TraceLevel : " + TraceLevel);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TraceProcess : " + TraceProcess);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TracePath : " + TracePath);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "TraceFileName : " + TraceFileName);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "RaiseEventToEveryone : " + RaiseEventToEveryone);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "SqlConnectionString : " + SymmetricalEncryption.DESEncrypted(SqlConnectionString).Message);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "ServiceId : " + ServiceId);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "ServiceName : " + ServiceName);
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level1, Logger.Log.LogType.Information, CurrentThreadId, ProcessName, "MultiThreadOperation : " + MultiThreadOperation);
            }
            catch (Exception ex)
            {
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level0, Logger.Log.LogType.Exception, CurrentThreadId, ProcessName, "Exception : " + ex.ToString());
            }

            configFileWatcher.EnableRaisingEvents = true;
        }
        #endregion

        #region ReadPrameter
        private static string ReadPrameter(ServiceConfig config, String ParameterName, string DefaultValue = "")
        {
            string ProcessName = "ReadPrameter";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            string Data = "unknown key";
            try
            {
                Data = config.Settings.Where(p => p.name == ParameterName).ToList().Count > 0 ? config.Settings.Where(p => p.name == ParameterName).FirstOrDefault().value : DefaultValue;
                Data = SymmetricalEncryption.DESDecrypted(Data).Message;
            }
            catch (Exception exception)
            {
                Logger.Log.WriteLog(Logger.Log.LogLevel.Level0, Logger.Log.LogType.Exception, CurrentThreadId, ProcessName, "Exception : " + exception.ToString());
                Console.WriteLine(exception);
                Data = "";
            }
            return Data;
        }
        #endregion
    }
    #endregion


    public class ServiceConfigJson
    {
        public int TraceLevel { get; set; }
        public string TraceProcess { get; set; }
        public string TracePath { get; set; }
        public string TraceFileName { get; set; }
        public int RaiseEventToEveryone { get; set; }
        public int MultiThreadOperation { get; set; }
        public string SqlConnectionString { get; set; }
        public string Provider { get; set; }
        public int DebugMode { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ServiceConfig
    {

        private ServiceConfigParameter[] settingsField;

        [System.Xml.Serialization.XmlArrayItemAttribute("parameter", IsNullable = false)]
        public ServiceConfigParameter[] Settings
        {
            get
            {
                return this.settingsField;
            }
            set
            {
                this.settingsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ServiceConfigParameter
    {
        private string nameField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}