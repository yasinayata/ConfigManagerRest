using ConfigManagerRest.Encryption;
using ConfigManagerRest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ConfigManagerRest.Database
{
    [Serializable]
    public class MsSqlProcess : IDatabaseProcess
    {
        #region User Variables
        //private static MsSqlProcess _instance;          //Class object
        //private static object _locker = new object();   //for thread safety

        public string SqlConnectionString { get; set; }
        public static Int32 CommandTimeOut { get; set; }
        private static Dictionary<string, SqlConnection> SQLConnectionList = new Dictionary<string, SqlConnection>();
        #endregion

        #region MsSqlProcess - Constructor

        public MsSqlProcess()
        {
            SqlConnectionString = "";
            CommandTimeOut = 30;
        }

        //public static MsSqlProcess GetInstance()
        //{
        //    //Thread safety.
        //    lock (_locker)
        //    {
        //        return _instance ?? (_instance = new MsSqlProcess());
        //    }
        //}

        internal void InitialProcess()
        {
        }
        #endregion

        #region Database Connection Area
        #region CheckConnection
        private OperationResult CheckConnection()
        {
            //string ProcessName = "MSSQLProcess.CheckConnection";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult op = new OperationResult();

            try
            {
                if (SQLConnectionList.ContainsKey(SqlConnectionString))
                {
                    op.Message = SQLConnectionList[SqlConnectionString].State.ToString();
                    switch (SQLConnectionList[SqlConnectionString].State)
                    {
                        case System.Data.ConnectionState.Closed:
                            op.Result = false;
                            SQLConnectionList.Remove(SqlConnectionString);
                            break;
                        case System.Data.ConnectionState.Open:
                            break;
                        case System.Data.ConnectionState.Connecting:
                            break;
                        case System.Data.ConnectionState.Executing:
                            break;
                        case System.Data.ConnectionState.Fetching:
                            break;
                        case System.Data.ConnectionState.Broken:
                            op.Result = false;
                            SQLConnectionList.Remove(SqlConnectionString);
                            break;
                        default:
                            op.Result = false;
                            SQLConnectionList.Remove(SqlConnectionString);
                            break;
                    }
                }
                else
                {
                    op.Result = false;
                    op.Message = "There is no connection";
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;

                if (SQLConnectionList.ContainsKey(SqlConnectionString))
                {
                    SQLConnectionList.Remove(SqlConnectionString);
                }
            }

            return op;
        }
        #endregion

        #region OpenConnection
        private OperationResult OpenConnection()
        {
            //string ProcessName = "MSSQLProcess.OpenConnection";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult op = new OperationResult();

            try
            {
                op = CheckConnection();

                if (!op.Result)
                {
                    op.Result = true;
                    op.Message = "Successful";

                    SqlConnection connection = new SqlConnection(SqlConnectionString);
                    SQLConnectionList[SqlConnectionString] = connection;
                    SQLConnectionList[SqlConnectionString].Open();
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;

                SQLConnectionList.Remove(SqlConnectionString);
            }

            return op;
        }
        #endregion
        #endregion

        #region CreateCommand
        private OperationResult<SqlCommand> CreateCommand(string CommandText)
        {
            //string ProcessName = "MSSQLProcess.CreateCommand";
            string CurrentThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

            OperationResult opc = new OperationResult();                        //Connection
            OperationResult<SqlCommand> op = new OperationResult<SqlCommand>(); //Command

            try
            {
                #region SQL connection crate and open
                opc = OpenConnection();
                op.Result = op.Result;
                op.Message = op.Message;
                if (!op.Result)
                    return op;
                #endregion

                SqlCommand command = new SqlCommand
                {
                    Connection = SQLConnectionList[SqlConnectionString],
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = CommandText,
                    CommandTimeout = CommandTimeOut
                };

                op.Data = command;
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion

        #region DeleteRecord
        internal OperationResult<object> DeleteRecord(SqlClause sqlClause)
        {
            OperationResult<object> op = new OperationResult<object>();
            OperationResult opc = new OperationResult();

            string command = sqlClause.DeleteClause;

            try
            {
                opc = OpenConnection();
                op.Result = opc.Result;
                op.Message = opc.Message;
                op.Data = null;
                if (op.Result)
                {
                    using (SqlConnection connection = SQLConnectionList[SqlConnectionString])
                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
                op.Data = null;
            }

            return op;
        }
        #endregion

        #region FindRecord
        internal OperationResult<object> FindRecord(SqlClause sqlClause)
        {
            OperationResult<object> op = new OperationResult<object>();
            OperationResult opc = new OperationResult();

            string command = sqlClause.FindClause;

            try
            {
                opc = OpenConnection();
                op.Result = opc.Result;
                op.Message = opc.Message;
                if (op.Result)
                {
                    DataTable dataTable = new DataTable();

                    using (SqlConnection connection = SQLConnectionList[SqlConnectionString])
                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        //Data has been read from DB and Encrypted fields is preparing
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count <= 0)
                        {
                            op.Result = false;
                            op.Message = "No record";
                            op.Data = null;
                        }
                        else
                        {
                            for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
                            {
                                for (int j = 0; j <= dataTable.Columns.Count - 1; j++)
                                {
                                    dataTable.Rows[i][j] = SymmetricalEncryption.DESDecrypted(dataTable.Rows[i][j].ToString()).Message;
                                    //Console.WriteLine($"Value : {dataTable.Rows[i][j]}");
                                }
                            }

                            op.Data = dataTable;
                        }
                    }
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

        #region ListRecord
        internal OperationResult<object> ListRecord(SqlClause sqlClause)
        {
            OperationResult<object> op = new OperationResult<object>();
            OperationResult opc = new OperationResult();

            string command = sqlClause.CustomClause;

            try
            {
                opc = OpenConnection();
                op.Result = opc.Result;
                op.Message = opc.Message;
                if (op.Result)
                {
                    DataTable table = new DataTable();

                    using (SqlConnection connection = SQLConnectionList[SqlConnectionString])
                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            //var dataModel = table.ConvertDataTableToEnumerable<object or T >();
                            var dataModel = table;
                            op.Data = dataModel;
                        }
                        else
                        {
                            op.Result = false;
                            op.Message = "No record";
                            op.Data = null;
                        }
                    }
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

        #region Insert
        internal OperationResult<object> Insert(SqlClause sqlClause)
        {
            OperationResult<object> op = new OperationResult<object>();
            OperationResult opc = new OperationResult();

            string command = sqlClause.InsertClause;

            try
            {
                opc = OpenConnection();
                op.Result = opc.Result;
                op.Message = opc.Message;
                if (op.Result)
                {
                    using (SqlConnection connection = SQLConnectionList[SqlConnectionString])
                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
                op.Data = null;
            }

            return op;
        }
        #endregion

        #region Update
        internal OperationResult<object> Update(SqlClause sqlClause)
        {
            OperationResult<object> op = new OperationResult<object>();
            OperationResult opc = new OperationResult();

            string command = sqlClause.UpdateClause;

            try
            {
                opc = OpenConnection();
                op.Result = opc.Result;
                op.Message = opc.Message;
                if (op.Result)
                {
                    using (SqlConnection connection = SQLConnectionList[SqlConnectionString])
                    using (SqlCommand cmd = new SqlCommand(command, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
                op.Data = null;
            }

            return op;
        }
        #endregion

        public OperationResult<object> Delete(object Object)
        {
            OperationResult<object> op = new OperationResult<object>();
            try
            {
                SqlClause sqlClause = new SqlClause();
                sqlClause = PrepareSqlClause.Prepare(Object, sqlClause);

                op = FindRecord(sqlClause);

                if (op.Result)
                    op = DeleteRecord(sqlClause);

                return op;
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }
            return op;
        }

        public OperationResult<object> Get(object Object)
        {
            OperationResult<object> opl = new OperationResult<object>();
            try
            {
                SqlClause sqlClause = new SqlClause();
                sqlClause = PrepareSqlClause.Prepare(Object, sqlClause);

                return FindRecord(sqlClause);
            }
            catch (Exception exception)
            {
                opl.Result = false;
                opl.Message = exception.Message;
            }
            return opl;
        }

        public OperationResult<object> Put(object Object)
        {
            OperationResult<object> op = new OperationResult<object>();
            try
            {
                SqlClause sqlClause = new SqlClause();
                sqlClause = PrepareSqlClause.Prepare(Object, sqlClause);

                op = FindRecord(sqlClause);

                if (!op.Result)
                    op = Insert(sqlClause);
                else
                    op = Update(sqlClause);

                if (op.Result)
                    op = FindRecord(sqlClause);
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }
            return op;
        }

        public OperationResult<object> List(SqlClause sqlClause)
        {
            OperationResult<object> opl = new OperationResult<object>();
            try
            {
                return ListRecord(sqlClause);
            }
            catch (Exception exception)
            {
                opl.Result = false;
                opl.Message = exception.Message;
            }
            return opl;
        }
    }
}
