using System;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using KeyVault;

//[assembly: Xamarin.Forms.Dependency(typeof(DataService))]
namespace KeyVault
{
    public class DataFromTableService : IDataService
    {
        public async Task<List<TopSecret>> GetSecretData()
        {
            var creds = new StorageCredentials("kvmobilestorage", "YOUR KEY HERE!");
            var cloudStorAcct = new CloudStorageAccount(creds, "core.windows.net", true);

            var tableClient = cloudStorAcct.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SecretTable");

            var tableQuery = new TableQuery<TopSecret>();

            var tableContents = await table.ExecuteQuerySegmentedAsync(tableQuery, null);

            return tableContents.Results;
        }
    }
}
