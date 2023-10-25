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
        var client = new SecretClient(new Uri(AzureConstansts.VaultUriString),
            new DefaultAzureCredential());

          await client.SetSecretAsync( new KeyVaultSecret(AzureConstansts.MySecretName,"xxxxiiii"));

        KeyVaultSecret secret = await client.GetSecretAsync(AzureConstansts.MySecretName);
        string secretValue = secret.Value;
        Console.WriteLine(secretValue);
    }
}


 
