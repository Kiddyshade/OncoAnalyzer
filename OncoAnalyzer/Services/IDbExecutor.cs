using System;
using System.Data;

namespace OncoAnalyzer.Services
{
    public interface IDbExecutor
    {
        int ExecuteNonQuery(string query, Action<IDbCommand> parameterize);
        IDataReader ExecuteReader(string query, Action<IDbCommand> parameterize);
    }
}
