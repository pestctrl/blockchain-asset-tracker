using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAPI
{
    public interface IBlockchainService
    {
        Task<String> InvokeGet(String url);
        Task<String> InvokePost(String url, String jsonObject);
        Task<bool> InvokeHead(String url, String request);
        Task<String> InvokePostFlask(String url, String jsonObject);
        Task<String> InvokeGetFlask(String url);
    }
}
