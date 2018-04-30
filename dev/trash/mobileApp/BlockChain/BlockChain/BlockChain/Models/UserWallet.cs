using System;
using System.Collections.Generic;
using System.Text;
using BlockChain.Models;

namespace BlockChain
{
    public class UserWallet
    {
        private IBlockChainService _blockChainService;

        public List<String> Assets { get; private set; }

        public UserWallet()
        {
            Assets = new List<string>();
        }

        public UserWallet(IBlockChainService blockChainService)
        {
            Assets = new List<string>();
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

        public Tuple<String, List<String>, bool> VerifyTransaction(String receiverAddress, List<String> assets)
        {
            bool verified;

            try
            {
                verified = _blockChainService.VerifiedTransaction(receiverAddress, assets);
            }
            catch (Exception ex)
            {
                verified = false;
            }
            return new Tuple<String, List<String>, bool>(receiverAddress, assets, verified);
        }

        public void ReceiveAssets(List<String> assets)
        {
            Assets.AddRange(assets.ToArray());
        }

        public void SendVerifiedTransaction(Tuple<String, List<String>, bool> verifiedTrasaction)
        {
            foreach (var asset in verifiedTrasaction.Item2)
            {
                if (verifiedTrasaction.Item3 == true)
                    Assets.Remove(asset);
            }
        }
    }
}
