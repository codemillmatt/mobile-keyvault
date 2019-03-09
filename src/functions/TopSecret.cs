using Microsoft.WindowsAzure.Storage.Table;

namespace com.microsoft.usekeyvault
{
    public class TopSecret : TableEntity
    {
        public string SecretValue { get; set; }
    }
}