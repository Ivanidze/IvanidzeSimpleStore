
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using FluentNHibernate.Cfg;
using Infrasturcture.DataBindingInterception;
using NHibernate;
using NHibernate.Cfg;
namespace Infrasturcture.Bootstrapping
{
    public class BootStrapper
    {
        public static ISessionFactory SessionFactory { get; private set; }
        private const string SerializedConfiguration = "configuration.serialized";
        private const string ConfigFile = "hibernate.cfg.xml";
        private static Configuration Configuration { get; set; }
        public static void Initialize()
        {
            
            Configuration = LoadConfigurationFromFile();

            if (Configuration == null)
            {
                var cfg = Fluently.Configure().Database(
                    FluentNHibernate.Cfg.Db.MsSqlCeConfiguration.Standard.ConnectionString(
                        "Data Source = SimpleStore.sdf"));
                //cfg.Mappings(m => m.FluentMappings.AddFromAssemblyOf<DataModel.Domain.Client>());
                Configuration = cfg.BuildConfiguration();
                
                //SaveConfigurationToFile(Configuration);
            }

            var intercepter = new DataBindingIntercepter();
            SessionFactory = Configuration
                .SetInterceptor(intercepter)
                .BuildSessionFactory();
            intercepter.SessionFactory = SessionFactory;
        }

        private static bool IsConfigurationFileValid
        {
            get
            {
                var ass = Assembly.GetCallingAssembly();
                if (ass.Location == null)
                    return false;
                var configInfo = new FileInfo(SerializedConfiguration);
                var assInfo = new FileInfo(ass.Location);
                var configFileInfo = new FileInfo(ConfigFile);
                if (configInfo.LastWriteTime < assInfo.LastWriteTime)
                    return false;
                if (configInfo.LastWriteTime < configFileInfo.LastWriteTime)
                    return false;
                return true;
            }
        }

        private static void SaveConfigurationToFile(Configuration configuration)
        {
            using (var file = File.Open(SerializedConfiguration, FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
        }

        private static Configuration LoadConfigurationFromFile()
        {
            if (IsConfigurationFileValid == false)
                return null;
            try
            {
                using (var file = File.Open(SerializedConfiguration, FileMode.Open))
                {
                    var bf = new BinaryFormatter();
                    return bf.Deserialize(file) as Configuration;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
