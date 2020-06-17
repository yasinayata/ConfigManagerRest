using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConfigManagerRest.Database;
using ConfigManagerRest.Encryption;

namespace ConfigManagerRest.AttributeProcess
{
    #region ObjectAttribute
    public static class ObjectAttribute
    {
        #region PrepareAllFields
        public static SqlClause PrepareAllFields(this object Object, SqlClause sqlClause)
        {
            List<string> UpdateFields = new List<string>();
            List<string> WhereClause = new List<string>();

            string AttributeIsSearchKey = "IsSearchKey";
            string AttributeIsEncrypted = "IsEncrypted";
            bool IsSearchKey = false;
            bool IsEncrypted = false;

            object obj = new object();
            CustomFieldAttribute cfa = new CustomFieldAttribute();
            try
            {
                Type oType = Object.GetType();
                Type aType = cfa.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(oType.GetProperties());

                foreach (var property in Object.GetType().GetProperties())
                {
                    var propertyInfo = oType.GetProperty(property.Name);
                    if (propertyInfo != null)
                    {
                        //var x = ObjectProperties.GetAttributeValue(typeof(Object), property.Name, typeof(CustomAttributeData), "IsSearchKey");
                        //x = ObjectProperties.GetAttributeValue(typeof(Object), property.Name, typeof(CustomAttributeData), "IsEncrypted");
                        IsSearchKey = false;
                        IsEncrypted = false;

                        string KeyName = propertyInfo.Name;
                        string KeyValue = property.GetValue(Object, null).ToString();

                        if (Attribute.IsDefined(propertyInfo, aType))
                        {
                            var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, aType);
                            if (attributeInstance != null)
                            {
                                foreach (PropertyInfo info in aType.GetProperties())
                                {
                                    //Console.WriteLine($"KeyName : {KeyName} - KeyValue : {KeyValue} - Attribute : {info.Name} - Value : {info.GetValue(attributeInstance, null)}");

                                    var Keyvalue = propertyInfo.GetValue(Object, null);
                                    if (info.CanRead && String.Compare(info.Name, AttributeIsSearchKey, StringComparison.InvariantCultureIgnoreCase) == 0 && info.GetValue(attributeInstance, null).ToString().ToLower() == "true")
                                        IsSearchKey = true;

                                    if (info.CanRead && String.Compare(info.Name, AttributeIsEncrypted, StringComparison.InvariantCultureIgnoreCase) == 0 && info.GetValue(attributeInstance, null).ToString().ToLower() == "true")
                                        IsEncrypted = true;
                                }
                            }
                        }
                        if (KeyValue.Trim() != "" && KeyValue != "00000000-0000-0000-0000-000000000000")
                        {
                            if (IsEncrypted)
                                KeyValue = SymmetricalEncryption.DESEncrypted(KeyValue).Message;

                            if (IsSearchKey)
                                WhereClause.Add(KeyName + " = '" + KeyValue + "'");

                            //Console.WriteLine("Name: {0} - Value: {1}", KeyName, KeyValue);
                            UpdateFields.Add(KeyName + " ┼ '" + KeyValue + "'");
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }

            sqlClause.UpdateFields = UpdateFields;
            sqlClause.FindFields = WhereClause;

            return sqlClause;
        }
        #endregion

        #region GetAttributeIsSearchKey
        public static List<string> GetAttributeIsSearchKey(this object Object)
        {
            List<string> WhereClause = new List<string>();

            object obj = new object();
            CustomFieldAttribute cfa = new CustomFieldAttribute();
            string AttributeName = "IsSearchKey";
            try
            {
                Type oType = Object.GetType();
                Type aType = cfa.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(oType.GetProperties());

                foreach (var property in Object.GetType().GetProperties())
                {
                    var propertyInfo = oType.GetProperty(property.Name);
                    if (propertyInfo != null)
                    {
                        //Console.WriteLine("Name: {0} - Value: {1}", propertyInfo.Name, propertyInfo.GetValue(Object, null));

                        if (Attribute.IsDefined(propertyInfo, aType))
                        {
                            var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, aType);
                            if (attributeInstance != null)
                            {
                                foreach (PropertyInfo info in aType.GetProperties())
                                {
                                    if (info.CanRead && String.Compare(info.Name, AttributeName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                    {
                                        var IsSearchKey = info.GetValue(attributeInstance, null);
                                        var value = propertyInfo.GetValue(Object, null);
                                        //Console.WriteLine("Name: {0} - Value: {1} - IsSearchKey: {2}", property.Name, property.GetValue(Object, null), IsSearchKey);
                                        if (Convert.ToBoolean(IsSearchKey) == true && (value.ToString() != "0" && value.ToString() != "00000000-0000-0000-0000-000000000000" && value.ToString().Trim() != ""))
                                        {
                                            WhereClause.Add(property.Name + " = '" + property.GetValue(Object, null) + "'");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }

            return WhereClause;
        }
        #endregion

        #region GetUpdateFields
        public static List<string> GetUpdateFields(this object Object)
        {
            List<string> UpdateFields = new List<string>();

            object obj = new object();
            CustomFieldAttribute cfa = new CustomFieldAttribute();
            try
            {
                Type oType = Object.GetType();
                Type aType = cfa.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(oType.GetProperties());

                foreach (var property in Object.GetType().GetProperties())
                {
                    var propertyInfo = oType.GetProperty(property.Name);
                    if (propertyInfo != null)
                    {
                        //Console.WriteLine("Name: {0} - Value: {1}", propertyInfo.Name, propertyInfo.GetValue(Object, null));
                        UpdateFields.Add(property.Name + " = '" + property.GetValue(Object, null) + "'");
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }

            return UpdateFields;
        }
        #endregion

        #region GetAttributeValue
        public static object GetAttributeValue(Type objectType, string propertyName, Type attributeType, string attributePropertyName)
        {
            var propertyInfo = objectType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                if (Attribute.IsDefined(propertyInfo, attributeType))
                {
                    var attributeInstance = Attribute.GetCustomAttribute(propertyInfo, attributeType);
                    if (attributeInstance != null)
                    {
                        foreach (PropertyInfo info in attributeType.GetProperties())
                        {
                            if (info.CanRead && String.Compare(info.Name, attributePropertyName, StringComparison.InvariantCultureIgnoreCase) == 0)
                            {
                                return info.GetValue(attributeInstance, null);
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
    #endregion
}
