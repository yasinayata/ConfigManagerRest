using ConfigManagerRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.Database
{
    [Serializable]
    public class DatabaseProcess : IDatabaseProcess
    {
        private static IDatabaseProcess databaseProcess;
        public OperationResult<object> Delete(object Object)
        {
            return databaseProcess.Delete(Object);
        }

        public OperationResult<object> Get(object Object)
        {
            return databaseProcess.Get(Object);
        }

        public OperationResult<object> Put(object Object)
        {
            return databaseProcess.Put(Object);
        }
        public OperationResult<object> List(SqlClause sqlClause)
        {
            return databaseProcess.List(sqlClause);
        }

        public DatabaseProcess(IDatabaseProcess dbProcess)
        {
            databaseProcess = dbProcess;
        }
    }
}
