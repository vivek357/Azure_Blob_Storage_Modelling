using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorageModelling.Logic
{
    class Login_Azure
    {
        public static CloudBlobClient AzureConnection()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
        CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            return blobClient;

        }

    }
}
