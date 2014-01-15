using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Configuration;


namespace Golem.DBManagement
{
    public class DataAccess
    {
        private static string SqlScriptsPath
        {
            get
            {
                return ConfigurationManager.AppSettings["SqlScriptsPath"];
            }
        }

        #region Public Methods

        public static void ExecuteTransactions(string entity, string queryID)
        {
            using (IDBManager dbManager = GetSqlConnection())
            {

                try
                {
                    dbManager.BeginTransaction();
                    dbManager.ExecuteNonQuery(CommandType.Text, GetSqlQuery(entity, queryID));
                    dbManager.CommitTransaction();
                }
                catch
                {
                    dbManager.RollBackTransaction();
                }
                finally
                {

                }
            }
        }

        //Example to use dataset
        public static void ExecuteDataset(string commandText)
        {
            using (IDBManager dbManager = GetSqlConnection())
            {
                try
                {
                    dbManager.ExecuteDataSet(CommandType.Text, commandText);

                    DataTable dTable = dbManager.DataSet.Tables["Table"];
                    foreach (DataRow row in dTable.Rows)
                    {
                        Console.WriteLine("NewRow");
                        Console.WriteLine("===========");
                        foreach (DataColumn col in dTable.Columns)
                        {
                            Console.WriteLine(col.ColumnName.ToString() + ":" + row[col].ToString());
                        }
                    }

                }

                finally
                {

                }
            }
        }

        public static object ExecuteScalar(string entity, string queryID)
        {
            using (IDBManager dbManager = GetSqlConnection())
            {
                try
                {
                    object obj = dbManager.ExecuteScalar(CommandType.Text, GetSqlQuery(entity, queryID));
                    return obj;
                }
                finally
                {
                    dbManager.Dispose();
                }
            }

        }

        public static string GetSqlQuery(string fileName, string queryId)
        {
            //string s;
            XDocument doc = XDocument.Load(SqlScriptsPath + fileName);
            var queryStrings = from q in doc.Elements("Queries").Elements("Query")
                               where (string)q.Attribute("id") == queryId
                               select q.Value.ToString();
            foreach (var queryString in queryStrings)
            {
                return queryString.ToString().Replace("\n", " ");
            }

            return string.Empty;
        }

        public static IDBManager GetSqlConnection()
        {
            IDBManager dbManager =  new DBManager(DataProvider.SqlServer);
            dbManager.ConnectionString = ConfigurationManager.ConnectionStrings["GolemDB"].ConnectionString;

            try
            {
                dbManager.Open();
            }
            catch
            {
                dbManager.Dispose();
            }

            finally
            {
            }

            return dbManager;
        }



        #endregion

    }
}
