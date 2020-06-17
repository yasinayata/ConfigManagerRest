using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConfigManagerRest.Database;
using ConfigManagerRest.General;
using ConfigManagerRest.Models;
using ConfigManagerRest.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConfigManagerRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        public static Token token = new Token();

        #region Get
        [Route("Get")]
        [HttpPost]
        public OperationResult<object> Get([FromBody] GenericClass generic)
        {
            //string ProcessName = "Get";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            
            OperationResult<object> opo = new OperationResult<object>();            

            try
            {
                opo = GenericMethod(generic.sessionUser, generic.parameter, Common.WebApiExecType.GET);
                return opo;
            }
            catch (Exception exception)
            {
                opo.Result = false;
                opo.Message = exception.Message;
            }

            return opo;
        }
        #endregion

        #region Put
        [Route("Put")]
        [HttpPost]
        public OperationResult<object> Put([FromBody] GenericClass generic)
        {
            //string ProcessName = "Put";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult<object> opo = new OperationResult<object>();

            try
            {
                opo = GenericMethod(generic.sessionUser, generic.parameter, Common.WebApiExecType.PUT);
                return opo;
            }
            catch (Exception exception)
            {
                opo.Result = false;
                opo.Message = exception.Message;
            }

            return opo;
        }
        #endregion

        #region List
        [Route("List")]
        [HttpPost]
        public OperationResult<object> List([FromBody] GenericClass generic)
        {
            //string ProcessName = "Post";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult<object> opo = new OperationResult<object>();

            try
            {
                opo = GenericMethod(generic.sessionUser, generic.parameter, Common.WebApiExecType.LIST);
                return opo;
            }
            catch (Exception exception)
            {
                opo.Result = false;
                opo.Message = exception.Message;
            }

            return opo;
        }
        #endregion

        #region Delete
        [Route("Delete")]
        [HttpPost]
        public OperationResult<object> Delete([FromBody] GenericClass generic)
        {
            //string ProcessName = "Delete";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult<object> opo = new OperationResult<object>();

            try
            {
                opo = GenericMethod(generic.sessionUser, generic.parameter, Common.WebApiExecType.DELETE);
                return opo;
            }
            catch (Exception exception)
            {
                opo.Result = false;
                opo.Message = exception.Message;
            }

            return opo;
        }
        #endregion

        #region PING
        [Route("Ping")]
        [HttpGet]
        public OperationResult Ping()
        {
            OperationResult op = new OperationResult();
            return op;
        }
        #endregion

        #region LOGIN
        [Route("Login")]
        [HttpPost]
        public OperationResult<SessionUser> Login([FromBody] User user)
        {
            OperationResult<SessionUser> opu = new OperationResult<SessionUser>();

            try
            {
                opu = token.UserLogin(user);
            }
            catch (Exception exception)
            {
                opu.Result = false;
                opu.Message = exception.Message;
            }

            return opu;
        }
        #endregion

        #region USERCHECK
        [Route("Usercheck")]
        [HttpPost]
        public OperationResult UserCheck(SessionUser sessionUser)
        {
            OperationResult op = new OperationResult();

            try
            {
                op = token.UserCheck(sessionUser);
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion

        #region GenericMethod
        private OperationResult<object> GenericMethod(SessionUser sessionUser, Parameter parameter, Common.WebApiExecType type)
        {
            //string ProcessName = "GenericMethod";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult<object> opo = new OperationResult<object>();

            try
            {
                opo = token.UserCheck(sessionUser).ConvertObject<OperationResult<object>>();

                //User has been accepted...
                if (opo.Result)
                {
                    switch (type)
                    {
                        case Common.WebApiExecType.GET:
                            opo = parameter.Get();
                            break;
                        case Common.WebApiExecType.PUT:
                            opo = parameter.Put();
                            break;
                        case Common.WebApiExecType.LIST:
                            SqlClause sqlClause = new SqlClause();
                            sqlClause.CustomClause = "SELECT * FROM Parameter WHERE Guid = '" + parameter.Guid + "'";   //Never mind SQL injection, for now

                            opo = parameter.List(sqlClause);
                            break;
                        case Common.WebApiExecType.DELETE:
                            opo = parameter.Delete();
                            break;
                        default:
                            break;
                    }

                    if (opo.Result && type != Common.WebApiExecType.LIST)
                        opo.Data = opo.Data != null ? ((DataTable)opo.Data).ConvertDataTableToEnumerable<Parameter>().ToList()[0].ConvertObject<Parameter>() : null;
                }
            }
            catch (Exception exception)
            {
                opo.Result = false;
                opo.Message = exception.Message;
            }

            return opo;

        }
        #endregion

    }
}
