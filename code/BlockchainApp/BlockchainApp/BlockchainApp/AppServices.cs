using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BlockchainApp
{
    public interface IQRServices
    {
        void SaveQRImage(Stream imageStream, string fileName);
    }
}
