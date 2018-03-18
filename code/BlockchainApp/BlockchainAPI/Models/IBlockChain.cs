using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainAPI.Models
{
    public interface IBlockChain
    {
        String GetTraderURL(string userName);
        String GetPropertiesByUserURL(string userName);
        String GetTransactionsURL();
        String GetTradersURL();
    }
}
