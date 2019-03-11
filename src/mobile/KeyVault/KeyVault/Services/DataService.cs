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
    public class DataService : IDataService
    {
        public async Task<List<TopSecret>> GetSecretData()
        {
            var creds = new StorageCredentials("kvmobilestorage", "CCUiQLEzes7E9CG8y4odJsipjn4SWqBBMGSfczbe32ds75l5R6wDAESa09a9rtANpSVOKkJtcWMbZB549SfLxg==");
            var cloudStorAcct = new CloudStorageAccount(creds, "core.windows.net", true);

            var tableClient = cloudStorAcct.CreateCloudTableClient();

            var table = tableClient.GetTableReference("SecretTable");

            var tableQuery = new TableQuery<TopSecret>();

            var tableContents = await table.ExecuteQuerySegmentedAsync(tableQuery, null);

            return tableContents.Results;
        }
    }
}
