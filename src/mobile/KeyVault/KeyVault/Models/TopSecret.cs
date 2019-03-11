using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace KeyVault
{
    public class TopSecret : TableEntity
    {
        public string SecretValue { get; set; }
    }
}
