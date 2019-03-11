using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyVault
{
    public interface IDataService
    {
        Task<List<TopSecret>> GetSecretData();
    }
}
