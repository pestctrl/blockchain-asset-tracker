using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAPI.Models
{
    public interface IBlockchainService
    {
        String GetTraderURL(string userName);
        String GetPropertiesByUserURL(string userName);
        String GetTransactionsURL();
        String GetTradersURL();
        String GetPropertyURL();
        Task<String> InvokeGet(String url);
        Task<String> InvokeGet(String url, object parameter);
    }
}
