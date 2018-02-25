using System;
using System.Collections.Generic;
using System.Text;
using BlockChain.Models;

namespace BlockChain
{
    public class UserWallet
    {
        private IBlockChainService _blockChainService;
        public List<String> assets = new List<String>()
        {
            "Asset A",
            "Asset B",
            "Asset C"
        };

        public UserWallet()
        {

        }

        public UserWallet(IBlockChainService blockChainService)
        {
            _blockChainService = blockChainService;
        }


        //public String SelectedAsset(String _asset)
        //{
        //    String result = "";
        //    foreach (String asset in assets)
        //    {
        //        if (asset == _asset)
        //        {
        //            result = _asset;
        //        }
        //    }
        //    return result;
        //}

        //public String GetReceiverAddress(String _receiverAddress)
        //{

        //    return _receiverAddress;
        //}

        public Tuple<String, List<String>, bool> GetTransactionVerify(String _receiverAddress, List<String> _asset)
        {
            bool verify;
            try
            {
                verify = _blockChainService.VerifiedTransaction(_receiverAddress, _asset);
            }
            catch (Exception ex)
            {
                verify = false;
            }

            var transactionVerify = new Tuple<String, List<String>, bool>(_receiverAddress, _asset, verify);
            return transactionVerify;
        }

        public void ReceivedAsset(List<String> _assets)
        {
            foreach (String asset in _assets)
                assets.Add(asset);
        }

        public void SendVerifiedTransaction(Tuple<String, List<String>, bool> verifiedTrasaction)
        {
            foreach (var asset in verifiedTrasaction.Item2)
            {
                if (verifiedTrasaction.Item3 == true)
                {
                    assets.Remove(asset);
                }
            }
        }
    }
}
