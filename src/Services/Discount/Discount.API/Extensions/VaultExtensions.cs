using Npgsql;
using VaultSharp;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;

namespace Discount.API.Extensions
{
    public class VaultExtensions
    {
        private readonly ILogger _logger;
        private readonly IVaultClient Client;
        private readonly IHostEnvironment _hostEnvironment;

        public VaultExtensions(ILoggerFactory loggerFactory, IVaultClient vaultClient, IHostEnvironment hostEnvironment)
        {
            _logger = loggerFactory.CreateLogger("Vault");
            Client = vaultClient;
            _hostEnvironment = hostEnvironment;
        }
         
        private IDictionary<string,object> ConfigDic { get; set; }

        public object GetConfig(string key)
        {
            if (ConfigDic != null && ConfigDic.TryGetValue(key, out object value))
                return value;

            Secret<SecretData> secret = Client.V1.Secrets.KeyValue.V2.ReadSecretAsync(
              path: $"/{_hostEnvironment.EnvironmentName}/dbConfing",
              mountPoint: "secret"
          ).Result;

            ConfigDic = secret.Data.Data;

            _logger.LogInformation("getting secret api key from vault: started");

            return ConfigDic[key];
        }


        public string GetDatabaseCredentials()
        {
            _logger.LogInformation("getting temporary database credentials from vault: started");

            //每一次呼叫都會產生一組tmp user，這邊應該要加入判斷是否已經建立或失效處理
            //避免產生過多使用者
            Secret<UsernamePasswordCredentials> dynamicDatabaseCredentials = Client.V1.Secrets.Database.GetCredentialsAsync(
                roleName: "admin-role"
            ).Result;

            _logger.LogInformation("getting temporary database credentials from vault: done");

            return $"Server=localhost;Database=DiscountDb;User Id={dynamicDatabaseCredentials.Data.Username};Password={dynamicDatabaseCredentials.Data.Password}; TrustServerCertificate=true;";
                
        }

    }
}