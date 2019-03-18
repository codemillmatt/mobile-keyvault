using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

using KeyVault;
using Newtonsoft.Json;

//[assembly: Xamarin.Forms.Dependency(typeof(DataFromFunctionService))]
namespace KeyVault
{
    public class DataFromFunctionService : IDataService
    {
        static HttpClient httpClient = new HttpClient();

        public async Task<List<TopSecret>> GetSecretData()
        {
            var secretJson = await httpClient.GetStringAsync("https://mobiledont.azurewebsites.net/api/UseKeyVault");

            return JsonConvert.DeserializeObject<List<TopSecret>>(secretJson);
        }
    }
}