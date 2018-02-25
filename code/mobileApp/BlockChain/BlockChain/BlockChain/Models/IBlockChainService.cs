using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Models
{
    public interface IBlockChainService
    {
        bool VerifiedTransaction(String receiverAddress, List<String> Asset);
    }
}
