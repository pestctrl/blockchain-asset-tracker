using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BlockchainApp
{
    public interface BarcodeGenerator
    {
        Stream GenerateQRImage(string codeText);
    }
}
