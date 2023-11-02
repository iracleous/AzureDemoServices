using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/***
 * packages
 * 
 * Azure.Security.KeyVault.Secrets
 * Azure.Identity
 * 
 * 
 * ***/


namespace AzureExtended.MyDemos;

internal class VaultDemo
{

    // AzureConstansts.VaultUriString
    // AzureConstansts.MySecretName

    public static async Task ReadFromVaultAsync()
    {
        var client = new SecretClient(new Uri(AzureConstants.VaultUriString),
            new DefaultAzureCredential());

          await client.SetSecretAsync( new KeyVaultSecret(AzureConstants.MySecretName,"xxxxiiii"));

        KeyVaultSecret secret = await client.GetSecretAsync(AzureConstants.MySecretName);
        string secretValue = secret.Value;
        Console.WriteLine(secretValue);
    }
}


 
