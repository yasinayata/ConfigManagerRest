using ConfigManagerRest.AttributeProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigManagerRest.Models
{
    #region Parameter
    [Serializable]
    public class Parameter
    {
        [CustomFieldAttribute(IsSearchKey = true)]
        public Guid Guid { get; set; }
        [CustomFieldAttribute(IsSearchKey = true)]
        public string Username { get; set; }
        [CustomFieldAttribute(IsSearchKey = true)]
        public string Application { get; set; }
        [CustomFieldAttribute(IsSearchKey = true)]
        public string KeyName { get; set; }

        [CustomFieldAttribute(IsEncrypted = true)]
        public string KeyValue { get; set; }
        public string Notes { get; set; }

        public Parameter()
        {
            Guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
            Username = "";
            Application = "";
            KeyName = "";
            KeyValue = "";
        }
    }
    #endregion
}