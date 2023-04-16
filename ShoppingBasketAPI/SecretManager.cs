using Azure.Security.KeyVault.Secrets;
using Azure;
using Azure.Identity;

public class SecretManager
{
    private readonly SecretClient _secretClient;
    private readonly IConfiguration _configuration;
    public SecretManager(SecretClient secretClient, IConfiguration configuration)
    {
        _secretClient = secretClient;
        _configuration = configuration;
        if (secretClient == null)
        {
            var credential = new ClientSecretCredential(_configuration["AzureAd:TenantId"], _configuration["AzureAd:ClientId"], _configuration["AzureAd:ClientSecret"]);
            _secretClient = new SecretClient(new Uri(_configuration["KeyVault:VaultUri"]), credential);

        }
      
    }
    public async Task<string> GetSecretAsync(string secretName)
    {
        string secretValue = string.Empty;
        secretValue = _configuration[secretName];

        try
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            secretValue = secret.Value;
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
            {
                return secretValue;
            }
            else
            {
                throw;
            }
        }

        return secretValue;
    }
}