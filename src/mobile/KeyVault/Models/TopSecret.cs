using System;
using Microsoft.Azure.Cosmos.Table;

namespace KeyVault
{
    public class TopSecret : TableEntity
    {
        public string SecretValue { get; set; }
    }
}
