using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace com.microsoft.usekeyvault
{
    public static class UseKeyVault
    {
        [FunctionName("UseKeyVault")]
        [StorageAccount("KeyvaultedStorageConnection")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Table("SecretTable", Connection="KeyvaultedStorageConnection")] CloudTable secretStorageTable,
            ILogger log)
        {
            log.LogInformation("Starting to grab the records from the Top Secret table!");

            TableQuery<TopSecret> topSecretQuery = new TableQuery<TopSecret>();

            var allTopSecretResults = await secretStorageTable.ExecuteQuerySegmentedAsync(topSecretQuery, null);

            return new OkObjectResult(allTopSecretResults.Results);
        }
    }
}
