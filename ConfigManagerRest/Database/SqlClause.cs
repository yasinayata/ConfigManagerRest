using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigManagerRest.AttributeProcess;
using ConfigManagerRest.Models;

namespace ConfigManagerRest.Database
{
    #region SqlClause
    public class SqlClause
    {
        public string TableName { get; set; }

        public List<string> FindFields { get; set; }
        public List<string> UpdateFields { get; set; }

        public string FindClause { get; set; }
        public string InsertClause { get; set; }
        public string UpdateClause { get; set; }
        public string DeleteClause { get; set; }

        public string CustomClause { get; set; }


        public SqlClause()
        {
            TableName = "";
            FindFields = new List<string>();
            UpdateFields = new List<string>();

            FindClause = "";
            InsertClause = "";
            UpdateClause = "";
            DeleteClause = "";
            CustomClause = "";
        }
    }
    #endregion

    #region PrepareSqlClause
    public static class PrepareSqlClause
    {
        public static SqlClause Prepare(object Object, SqlClause sqlClause)
        {
            sqlClause.TableName = FindTableName(Object);
            //sqlClause.FindFields = FindWhereFields(Object);
            //sqlClause.UpdateFields = FindUpdateFields(Object);

            sqlClause = PrepareAllFields(Object, sqlClause);

            sqlClause.FindClause = CreateFindCommand(sqlClause).Message;
            sqlClause.InsertClause = CreateInsertCommand(sqlClause).Message;
            sqlClause.UpdateClause = CreateUpdateCommand(sqlClause).Message;
            sqlClause.DeleteClause = CreateDeleteCommand(sqlClause).Message;

            return sqlClause;

        }


        #region FindTableName
        internal static string FindTableName(object Object)
        {
            string TableName = "";
            try
            {
                Type objtype = Object.GetType();
                //Console.WriteLine($"Table name: {objtype.Name}");
                return objtype.Name;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }
            return TableName;
        }
        #endregion

        #region PrepareAllFields
        internal static SqlClause PrepareAllFields(object Object, SqlClause sqlClause)
        {
            SqlClause sc = new SqlClause();
            try
            {
                sc = Object.PrepareAllFields(sqlClause);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }

            return sc;
        }
        #endregion


        #region FindWhereFields
        internal static List<string> FindWhereFields(object Object)
        {
            List<string> result = new List<string>();
            try
            {
                result = Object.GetAttributeIsSearchKey();

            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }

            return result;
        }
        #endregion

        #region FindUpdateFields
        internal static List<string> FindUpdateFields(object Object)
        {
            List<string> result = new List<string>();
            try
            {
                result = Object.GetUpdateFields();

            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
            }

            return result;
        }
        #endregion

        #region CreateDeleteCommand
        internal static OperationResult CreateDeleteCommand(SqlClause sqlClause)
        {
            OperationResult op = new OperationResult();
            try
            {
                #region Real Delete area
                string command = "";
                command = "DELETE " + sqlClause.TableName;

                if (sqlClause.FindFields.Count > 0)
                {
                    command = command + " WHERE ";
                    //Console.WriteLine($"Command : {command}");

                    foreach (var item in sqlClause.FindFields)
                    {
                        command = command + item + " AND ";
                        //Console.WriteLine($"Command : {command}");
                    }
                    //command = command.Substring(0, command.Length - 5);
                    command = command + " IsDeleted = 0";
                }
                #endregion

                command = "";
                command = "UPDATE " + sqlClause.TableName;
                command = command + " SET IsDeleted = 1";

                if (sqlClause.FindFields.Count > 0)
                {
                    command = command + " WHERE ";
                    //Console.WriteLine($"Command : {command}");

                    foreach (var item in sqlClause.FindFields)
                    {
                        command = command + item + " AND ";
                        //Console.WriteLine($"Command : {command}");
                    }
                    //command = command.Substring(0, command.Length - 5);
                    command = command + " IsDeleted = 0";
                }

                op.Message = command;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion

        #region CreateFindCommand
        internal static OperationResult CreateFindCommand(SqlClause sqlClause)
        {
            OperationResult op = new OperationResult();
            try
            {
                string command = "";
                command = "SELECT TOP 1 * FROM " + sqlClause.TableName + " WITH (NOLOCK)";
                command = command + " WHERE ";

                if (sqlClause.FindFields.Count > 0)
                {
                    //Console.WriteLine($"Command : {command}");
                    foreach (var item in sqlClause.FindFields)
                    {
                        command = command + item + " AND ";
                        //Console.WriteLine($"Command : {command}");
                    }
                    //command = command.Substring(0, command.Length - 5);
                }
                command = command + " IsDeleted = 0";

                //Console.WriteLine($"Command : {command}");

                op.Message = command;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion

        #region CreateUpdateCommand
        internal static OperationResult CreateUpdateCommand(SqlClause sqlClause)
        {
            OperationResult op = new OperationResult();
            try
            {
                string command = "";
                command = "UPDATE " + sqlClause.TableName;
                if (sqlClause.FindFields.Count > 0)
                    command = command + " SET ";

                foreach (var item in sqlClause.UpdateFields)
                {
                    
                    command = command + item.Replace('┼', '=') + ", ";
                    //Console.WriteLine($"Command : {command}");
                }
                command = command.Substring(0, command.Length - 2);
                //Console.WriteLine($"Command : {command}");

                if (sqlClause.FindFields.Count > 0)
                {
                    command = command + " WHERE ";
                    //Console.WriteLine($"Command : {command}");

                    foreach (var item in sqlClause.FindFields)
                    {
                        command = command + item + " AND ";
                        //Console.WriteLine($"Command : {command}");
                    }
                    //command = command.Substring(0, command.Length - 5);
                    command = command + " IsDeleted = 0";
                }

                //Console.WriteLine($"Command : {command}");
                op.Message = command;
            }
            catch (Exception exception)
            {
                //Console.WriteLine($"Exception : {exception.Message}");
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion

        #region CreateInsertCommand
        internal static OperationResult CreateInsertCommand(SqlClause sqlClause)
        {
            OperationResult op = new OperationResult();
            try
            {
                string command = "";
                command = "INSERT INTO " + sqlClause.TableName + "(";
                foreach (var item in sqlClause.UpdateFields)
                {
                    string[] words = item.Split('┼');
                    command = command + words[0].Trim() + ", ";
                    //Console.WriteLine($"Command : {command}");
                }
                command = command.Substring(0, command.Length - 2);
                command = command + ") VALUES (";
                foreach (var item in sqlClause.UpdateFields)
                {
                    string combine = "";
                    string[] words = item.Split('┼');
                    for (int i = 1; i < words.Count(); i++)
                    {
                        combine = combine + (words[i].Trim() == "" ? "=" : words[i]);
                        Console.WriteLine(combine);
                    }
                    command = command + combine.Trim() + ", ";
                    //Console.WriteLine($"Command : {command}");
                }
                command = command.Substring(0, command.Length - 2);
                command = command + ")";

                //Console.WriteLine($"Command : {command}");
                op.Message = command;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception : {exception.Message}");
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion

    }
    #endregion
}
