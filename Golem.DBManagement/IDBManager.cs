using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Golem.DBManagement
{
    public interface IDBManager : IDisposable
    {
        DataProvider ProviderType { get; set; }
        string ConnectionString { get; set; }
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IDbCommand Command { get; }
        IDataReader DataReader { get; }
        DataSet DataSet { get; set; }

        void Open();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        void CreateParameters(int paramsCount);
        void AddParameters(int index, string paramName, object objValue);
        IDataReader ExecuteReader(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        object ExecuteScalar(CommandType commandType, string commandText);
        int ExecuteNonQuery(CommandType commandType, string commandText);
        void CloseReader();
        void Close();    
    }
}
