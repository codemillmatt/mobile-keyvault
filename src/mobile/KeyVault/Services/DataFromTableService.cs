using System;
using System.Collections.Generic;

using Microsoft.Azure.Cosmos.Table;

using System.Threading.Tasks;
using System.Linq;
using KeyVault;

[assembly: Xamarin.Forms.Dependency(typeof(DataFromTableService))]
namespace KeyVault
{
    public class DataFromTableService : IDataService
    {
        public async Task<List<TopSecret>> GetSecretData()
        {
            var creds = new StorageCredentials("kvmobilestorage", "YOUR KEY HERE");
            var cloudStorAcct = new CloudStorageAccount(creds, "core.windows.net", true);

            var tableClient = cloudStorAcct.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SecretTable");

            var tableQuery = new TableQuery<TopSecret>();

            var tc = await table.ExecuteQuerySegmentedAsync(tableQuery, null);

            return tc.Results;
        }

        //public async Task<List<TopSecret>> GetSecretData()
        //{
        //    var creds = new Microsoft.Azure.Cosmos.Table.StorageCredentials("kvmobilestorage", "");
        //    var storageAcct = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(
        //}
    }
}
