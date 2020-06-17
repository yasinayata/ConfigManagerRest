using ConfigManagerRest.Database;
using ConfigManagerRest.General;
using ConfigManagerRest.Models;
using ConfigManagerRest.Encryption;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using Newtonsoft.Json;using System.Xml;
using System.Xml.Serialization;

namespace ConfigManagerRest.Serialization
{
    [Serializable]
    public static class Serialization
    {
        #region Serialize Json
        public static string Serialize(this object Object)
        {
            OperationResult op = new OperationResult();
            try
            {
                op.Message = JsonConvert.SerializeObject(Object); //, Formatting.Indented); js.Serialize(Object);
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op.Message;
        }
        public static T Deserialize<T>(this string Json)
        {
            OperationResult<T> op = new OperationResult<T>();
            try
            {
                op.Data = JsonConvert.DeserializeObject<T>(Json);// js.Deserialize<T>(Json);
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }
            return op.Data;
        }
        #endregion

        #region BinarySerialize
        public static byte[] BinarySerialize(this object graph)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                formatter.Serialize(stream, graph);

                return stream.ToArray();
            }
        }

        public static object BinaryDeserialize(this byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                var formatter = new BinaryFormatter();

                return formatter.Deserialize(stream);
            }
        }
        #endregion

        #region SerializeObjectXml
        private static OperationResult SerializeObjectXml(this object Object)
        {
            OperationResult op = new OperationResult();
            try
            {
                XmlSerializer MySerializer = new XmlSerializer(typeof(Object));
                TextWriter TW = new StringWriter();
                MySerializer.Serialize(TW, Object);
                op.Message = TW.ToString();

            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }

        private static T DeserializeXml2<T>(string XmlData) where T : new()
        {
            T t = new T();

            XmlSerializer MyDeserializer = new XmlSerializer(typeof(T));
            StringReader SR = new StringReader(XmlData);
            XmlReader XR = new XmlTextReader(SR);
            if (MyDeserializer.CanDeserialize(XR))
            {
                t = (T)MyDeserializer.Deserialize(XR);
            }

            return t;
        }

        private static OperationResult<T> DeserializeXml<T>(string XmlData)
        {
            OperationResult<T> op = new OperationResult<T>();

            try
            {
                XmlSerializer MyDeserializer = new XmlSerializer(typeof(T));
                StringReader SR = new StringReader(XmlData);
                XmlReader XR = new XmlTextReader(SR);
                if (MyDeserializer.CanDeserialize(XR))
                {
                    op.Data = (T)MyDeserializer.Deserialize(XR);
                }
                else
                {
                    op.Result = false;
                    op.Message = "Non Deserialize Object";
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }

        #endregion

        public static object ChangeType(object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;
            return Convert.ChangeType(value, type);
        }

        #region ConvertDataTableToEnumerable
        public static List<T> ConvertDataTableToEnumerable<T>(this DataTable table) where T : new()
        {
            List<T> returnListT = new List<T>();
            foreach (DataRow item in table.Rows)
            {
                T model = new T();
                foreach (var field in model.GetType().GetProperties())
                {
                    try
                    {
                        object fieldValue = item[field.Name];
                        Type tp = field.PropertyType;
                        if (Nullable.GetUnderlyingType(tp) != null)
                            field.SetValue(model, Convert.ChangeType(fieldValue, Type.GetType(Nullable.GetUnderlyingType(tp).ToString())), null);
                        else
                            //field.SetValue(model, Convert.ChangeType(fieldValue, tp), null);
                            field.SetValue(model, ChangeType(fieldValue, tp), null);        //for Guid convert
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($"Exception : {exception.Message}");
                    }

                }

                returnListT.Add(model);
            }
            return returnListT;
        }
        #endregion

        #region ConvertObject
        public static T ConvertObject<T>(this object Object) where T : new()
        {
            T t = new T();
            foreach (var field in t.GetType().GetProperties())
            {
                try
                {
                    object fieldValue = Object.GetType().GetProperty(field.Name).GetValue(Object, null);

                    Type tp = field.PropertyType;

                    if (Nullable.GetUnderlyingType(tp) != null)
                        field.SetValue(t, Convert.ChangeType(fieldValue, Type.GetType(Nullable.GetUnderlyingType(tp).ToString())), null);
                    else
                        field.SetValue(t, Convert.ChangeType(fieldValue, tp), null);
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception : {exception.Message}");
                }
            }

            return t;
        }
        #endregion

        #region DESEncrypted
        public static OperationResult DESEncrypted(this string This)
        {
            return SymmetricalEncryption.DESEncrypted(This);
        }

        public static OperationResult DESDecrypted(this string This)
        {
            return SymmetricalEncryption.DESDecrypted(This);
        }
        #endregion

        #region Delete
        public static OperationResult<object> Delete(this object Object)
        {
            return Common.dbProvider.Delete(Object);

        }
        #endregion

        #region Put
        public static OperationResult<object> Put(this object Object)
        {
            return Common.dbProvider.Put(Object);
        }
        #endregion

        #region Get
        public static OperationResult<object> Get(this object Object)
        {
            return Common.dbProvider.Get(Object);
        }
        #endregion

        #region List
        public static OperationResult<object> List(this object Object, SqlClause sqlClause)
        {
            return Common.dbProvider.List(sqlClause);
        }
        #endregion
    }
}