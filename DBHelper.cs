using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBManager;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NLog;

namespace PublicDBUtil
{
    public static class DBHelper
    {
        public static DBResult GetTable(string connKey, string sql)
        {
            return DatabaseBasic.GetTableResult(connKey, sql);
        }

        public static DBResult GetObject(string connKey, string sql)
        {
            return DatabaseBasic.GetObjectResult(connKey, sql);
        }

        public static DBResult GetNone(string connKey, string sql)
        {
            return DatabaseBasic.GetNoneResult(connKey, sql);
        }
    }
}
