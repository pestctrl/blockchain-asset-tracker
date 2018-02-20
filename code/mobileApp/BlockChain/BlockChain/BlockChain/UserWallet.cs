using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain
{
    public class UserWallet
    {
        public List<String> assets = new List<String>()
        {
            "Asset A",
            "Asset B",
            "Asset C"
        };

        public String SelectedAsset(String _asset)
        {
            String result = "";
            foreach (String asset in assets)
            {
                if (asset == _asset)
                {
                    result = _asset;
                }
            }
            return result;
        }

        public String GetReceiverAddress(String _receiverAddress)
        {
            
            return _receiverAddress;
        }

        public String Send(String _receiverAddress, String _asset)
        {
            return "Send " + _asset + " to " + _receiverAddress;
        }

        public void ReceivedAsset(String _asset)
        {
            assets.Add(_asset);
        }
    }
}
