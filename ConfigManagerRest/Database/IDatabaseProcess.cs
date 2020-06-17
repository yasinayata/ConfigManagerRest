using ConfigManagerRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigManagerRest.Database
{
    public interface IDatabaseProcess
    {
        OperationResult<object> Get(object Object);
        OperationResult<object> Put(object Object);
        OperationResult<object> Delete(object Object);
        OperationResult<object> List(SqlClause sqlClause);
    }
}
