using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureExtended.MyDemos
{
    internal static class AzureConstansts
    {
        public static readonly string VaultName = "az204vault-16978";
        public static readonly string MySecretName = "ExamplePassword";
        public static readonly string VaultUriString = "https://" + VaultName + ".vault.azure.net/";


        public static readonly string _storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=potatochips;AccountKey=SyOl8W8rfXwpiMp/TxsSxG6rpRU+mKCEGSn4dfMNjiBR59TzThaTI7gQTpihUhtxg+ss1Ls6s0Ma+ASttwrnbw==;EndpointSuffix=core.windows.net";
        public static readonly string _containerName = "images";
        public static readonly string _fileName = "txtFile";
        // https://potatochips.blob.core.windows.net/images/340751895_3479940802290276_7283390250138615953_n.jpg


        public static readonly string TenantId = "b1732512-60e5-48fb-92e8-8d6902ac1349";

        public static readonly string _fileNameImage = "340751895_3479940802290276_7283390250138615953_n.jpg";
        public static readonly string _accountName = "potatochips";
        public static readonly string _accountKey = "SyOl8W8rfXwpiMp/TxsSxG6rpRU+mKCEGSn4dfMNjiBR59TzThaTI7gQTpihUhtxg+ss1Ls6s0Ma+ASttwrnbw==";



        /// <summary>
        /// Cosmos db
        /// </summary>
        // Replace <documentEndpoint> with the information created earlier
        public static readonly string EndpointUri = "https://dimitris.documents.azure.com:443/";

        // Set variable to the Primary Key from earlier.
        public static readonly string PrimaryKey = "tVautrowoR4U6ungLAGkHVA8rO7WXMaokzILFKqkmqxqg8Hz581PnOiVisiuwf3n4YGNHRKI7IH8ACDb0AFBZw==";

        // The names of the database and container we will create
        public static readonly string databaseId = "Az204Database";
        public static readonly string containerId = "Az204Container";




    }
}
