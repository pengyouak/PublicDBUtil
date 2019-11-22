using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;


namespace DBManager
{
    public class OledbDatabaseData : DatabaseData
    {
        #region Public Methods  

        public OledbDatabaseData(ConnectionStringSettings connectionStringSettings,
            IConfigurationSource configurationSource)
            : base(connectionStringSettings, configurationSource)
        {
        }

        #endregion

        /// <summary>  
        /// Creates a <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.TypeRegistration"/> instance describing the <see cref="SqlDatabase"/> represented by   
        /// this configuration object.  
        /// </summary>  
        /// <returns>A <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.TypeRegistration"/> instance describing a database.</returns>  
        public override
            System.Collections.Generic.IEnumerable
                <Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.TypeRegistration>
            GetRegistrations()
        {
            yield return new TypeRegistration<Database>(
                () => new OledbDatabase(
                    ConnectionString,
                    Container.Resolved<IDataInstrumentationProvider>(Name)))
            {
                Name = Name,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }
    }
}
