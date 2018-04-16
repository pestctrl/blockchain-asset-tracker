using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BlockchainApp
{
    public interface QRServices
    {
        Stream GenerateQRImage(string codeText);
        void SaveQRImage(Stream imageStream, string fileName);
    }
}
