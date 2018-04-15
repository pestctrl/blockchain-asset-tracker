using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using ZXing.Mobile;
using ZXing.Net.Mobile.Android;

[assembly: Xamarin.Forms.Dependency(typeof(BlockchainApp.Droid.BarcodeGeneratorImplementation))]
namespace BlockchainApp.Droid
{
    public class BarcodeGeneratorImplementation :BarcodeGenerator
    {
        public Stream GenerateQRImage(string codeText)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 0
                }
            };

            barcodeWriter.Renderer = new ZXing.Mobile.BitmapRenderer();
            var bitmap = barcodeWriter.Write(codeText);
            var stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Position = 0;
            return stream;
        }
    }
}