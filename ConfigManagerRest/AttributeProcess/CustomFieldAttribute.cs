using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.AttributeProcess
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomFieldAttribute : Attribute
    {
        public int MaxLength { get; set; }
        public bool IsEncrypted { get; set; }
        public bool IsSearchKey { get; set; }

        public CustomFieldAttribute()
        {
            this.IsEncrypted = false;
            this.IsSearchKey = false;
        }
    }
}
