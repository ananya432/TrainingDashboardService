using PowerBIService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PowerBIService.Services
{
    public interface IService
    {
        public Task<Transactions> GetTransactionRecord();
    }
}
