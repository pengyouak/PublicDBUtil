using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using DBManager;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;

namespace PublicDBUtil
{
    internal enum ExecuteType
    {
        Table,
        Object,
        None
    }

    public static class DatabaseBasic
    {
        private static Dictionary<string, Database> _dbDatabases = new Dictionary<string, Database>();
        private static System.Text.RegularExpressions.Regex _providerRex = new System.Text.RegularExpressions.Regex("Provider[^;]+;|provider[^;]+;|PROVIDER[^;]+;");

        private static Database CreateDB(string connKey)
        {
            if (string.IsNullOrEmpty(connKey))
            {
                LogHelper.Logger.Error($"没有找到配置文件中的[{connKey}]节点.");
                return null;
            }
            if (_dbDatabases.ContainsKey(connKey) == true)
                return _dbDatabases[connKey];

            try
            {
                Database dbTmp = null;
                dbTmp = DatabaseFactory.CreateDatabase(connKey);
                try
                {
                    string strConnstring = dbTmp.ConnectionString;
                    string noProviderStr = strConnstring;

                    if (_providerRex.Match(strConnstring).Value.Length > 0)
                    {
                        noProviderStr = strConnstring.Replace(_providerRex.Match(strConnstring).Value, "");
                    }
                    if (dbTmp.DbProviderFactory is System.Data.OleDb.OleDbFactory)
                    {
                        var dbOledb = new OledbDatabase(strConnstring);
                        LogHelper.Logger.Info("数据库对象创建成功（OLEDB）");
                        _dbDatabases.Add(connKey, dbOledb);
                        return dbOledb;
                    }
                    else if (dbTmp.DbProviderFactory is System.Data.OracleClient.OracleClientFactory)
                    {
                        var dbOracle = new OracleDatabase(noProviderStr);
                        LogHelper.Logger.Info("数据库对象创建成功（Oracle）");
                        _dbDatabases.Add(connKey, dbOracle);
                        return dbOracle;
                    }
                    else if (dbTmp.DbProviderFactory is MySql.Data.MySqlClient.MySqlClientFactory)
                    {
                        var dbMySql = new MySqlDatabase(noProviderStr);
                        LogHelper.Logger.Info("数据库对象创建成功（MySQL）");
                        _dbDatabases.Add(connKey, dbMySql);
                        return dbMySql;
                    }
                    else
                    {
                        var dbLocal = new SqlDatabase(noProviderStr);
                        LogHelper.Logger.Info("数据库对象创建成功（默认SQLServer）");
                        _dbDatabases.Add(connKey, dbLocal);
                        return dbLocal;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex, "创建数据库失败.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "创建数据库失败.");
            }

            return null;
        }

        private static DBResult Execute(string connKey, string strSQL, ExecuteType type)
        {
            int iCommandTimeout = 1800;
            if (ConfigurationManager.AppSettings["COMMAND_TIMEOUT"] != null)
                if (!int.TryParse(ConfigurationManager.AppSettings["COMMAND_TIMEOUT"], out iCommandTimeout))
                    iCommandTimeout = 1800;

            try
            {
                var result = new DBResult();
                Database db = null;
                db = CreateDB(connKey);
                if (db == null)
                {
                    result.IsSuccessed = false;
                    result.Message = "创建数据库连接失败";
                    return result;
                }

                using (var conn = db.CreateConnection())
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = iCommandTimeout;
                    cmd.CommandText = strSQL;
                    switch (type)
                    {
                        case ExecuteType.Table:
                        {
                            var ds = db.ExecuteDataSet(cmd);
                            result.DataSet = ds;
                            result.Table = ds.Tables[0];
                            break;
                        }
                        case ExecuteType.Object:
                        {
                            result.DataObject = db.ExecuteReader(cmd);
                            break;
                        }
                        case ExecuteType.None:
                        {
                            result.AffectRowCount = db.ExecuteNonQuery(cmd);
                            break;
                        }
                    }
                    conn.Close();
                }

                result.IsSuccessed = true;
                return result;
            }
            catch (Exception ex)
            {
                return new DBResult()
                {
                    IsSuccessed = false,
                    Message = $"执行查询语句失败，原因：{ex.Message} (SQL：{strSQL})"
                };
            }
        }

        public static DBResult GetTableResult(string connKey, string strSQL)
        {
            return Execute(connKey, strSQL, ExecuteType.Table);
        }

        public static DBResult GetObjectResult(string connKey, string strSQL)
        {
            return Execute(connKey, strSQL, ExecuteType.Object);
        }

        public static DBResult GetNoneResult(string connKey, string strSQL)
        {
            return Execute(connKey, strSQL, ExecuteType.None);
        }
    }
}
