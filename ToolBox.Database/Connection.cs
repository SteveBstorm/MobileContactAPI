using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Database
{
    public sealed class Connection
    {
        private string _ConnectionString;
        private DbProviderFactory _Factory;

        public Connection(DbProviderFactory Factory, string ConnectionString)
        {
            if(string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }
            if(Factory == null)
            {
                throw new ArgumentNullException(nameof(Factory));
            }

            this._Factory = Factory;
            this._ConnectionString = ConnectionString;
        }

        private DbConnection CreateConnection()
        {
            DbConnection db = _Factory.CreateConnection();
            db.ConnectionString = _ConnectionString;
            return db;
        }

        private DbCommand CreateCommand(DbConnection Db, Command Command)
        {
            DbCommand cmd = Db.CreateCommand();

            cmd.CommandText = Command.Query;
            cmd.CommandType = Command.IsProcedure ? CommandType.StoredProcedure : CommandType.Text;

            foreach (KeyValuePair<string, object> parameter in Command.Parameters)
            {
                DbParameter dbParameter = _Factory.CreateParameter();
                dbParameter.ParameterName = parameter.Key;
                dbParameter.Value = parameter.Value ?? DBNull.Value;

                cmd.Parameters.Add(dbParameter);
            }

            return cmd;
        }

        #region Connection management
        private DbConnection _DbConnection;

        public void Open()
        {
            _DbConnection = CreateConnection();
            _DbConnection.Open();
        }

        public void Close()
        {
            if(_DbConnection != null)
            {
                _DbConnection.Close();
                _DbConnection = null;
            }
        }
        #endregion

        #region Execute query SQL
        public int ExecuteNonQuery(Command Command)
        {
            if(_DbConnection == null)
            {
                throw new Exception("DB Connection not open !");
            }

            using (DbCommand cmd = CreateCommand(_DbConnection, Command))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public TResult ExecuteScalar<TResult>(Command Command)
        {
            if (_DbConnection == null)
            {
                throw new Exception("DB Connection not open !");
            }

            using (DbCommand cmd = CreateCommand(_DbConnection, Command))
            {
                object result = cmd.ExecuteScalar();

                return (result is DBNull) ? default(TResult) : (TResult)result;
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command Command, Func<IDataReader, TResult> Convert)
        {
            if (_DbConnection == null)
            {
                throw new Exception("DB Connection not open !");
            }

            using(DbCommand cmd = CreateCommand(_DbConnection, Command))
            {
                using(IDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        yield return Convert(reader);
                    }    
                }
            }
        }
        #endregion
    }
}
