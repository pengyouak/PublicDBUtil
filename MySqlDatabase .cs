using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using MySql.Data.MySqlClient;

namespace DBManager
{
    [ConfigurationElementType(typeof (MySqlDatabaseData))]
    public class MySqlDatabase : Database
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.  
        /// </summary>  
        /// <param name="connectionString">The connection string.</param>  
        public MySqlDatabase(string connectionString)
            : base(connectionString, MySqlClientFactory.Instance)
        {
        }

        /// <summary>  
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a  
        /// connection string and instrumentation provider.  
        /// </summary>  
        /// <param name="connectionString">The connection string.</param>  
        /// <param name="instrumentationProvider">The instrumentation provider.</param>  
        public MySqlDatabase(string connectionString, IDataInstrumentationProvider instrumentationProvider)
            : base(connectionString, MySqlClientFactory.Instance, instrumentationProvider)
        {
        }

        /// <summary>  
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object.   
        /// </summary>  
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>  
        /// <remarks>The <see cref="DbCommand"/> must be a <see cref="SqlCommand"/> instance.</remarks>  
        protected override void DeriveParameters(System.Data.Common.DbCommand discoveryCommand)
        {
            MySqlCommandBuilder.DeriveParameters((MySqlCommand) discoveryCommand);
        }
    }

}
