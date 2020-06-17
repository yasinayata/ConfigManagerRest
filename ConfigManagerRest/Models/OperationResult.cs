using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.Models
{
    #region OperationResult
    public class OperationResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }

        public OperationResult()
        {
            this.Result = true;
            this.Message = "Successful";
        }
    }
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; }
    }
    #endregion OperationResult
}
