using QRCoder;
using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.ZKWeb;

namespace ShortURL
{
    /// <summary>
    /// 二维码和条形码
    /// https://blog.csdn.net/rex0y/article/details/81098637
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="message"></param>Margin 
        /// <param name="gifFileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static string CreateCodeEwm(string message, int width = 600, int height = 600)
        {          
            using (Bitmap map = CreateCodeBitmap(message, width, height))
            {
                var filePath = Directory.GetCurrentDirectory() + "/File/QrCode/" + Guid.NewGuid() + ".png";
                string dir = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                map.Save(filePath, ImageFormat.Png);
                map.Dispose();
                return filePath;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CreateCodeBitmap(string message, int width = 600, int height = 600)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions();
            options.DisableECI = true;
            //设置内容编码
            options.CharacterSet = "UTF-8";
            //设置二维码的宽度和高度
            options.Width = width;
            options.Height = height;
            //设置二维码的边距,单位不是固定像素
            options.Margin = 1;
            writer.Options = options;
            using (Bitmap map = writer.Write(message))
            {
                return map;
            }
        }

        /// <summary>
        /// https://www.codetd.com/article/1715157
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static Bitmap GetQRCode(string url, int pixel)
        {
            using (QRCodeGenerator generator = new QRCodeGenerator())
            {
                using (QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.L, true))
                {
                    using (QRCode qrcode = new QRCode(codeData))
                    {
                        Bitmap qrImage = qrcode.GetGraphic(pixel, Color.Black, Color.White, false);
                        return qrImage;
                    }
                }
            }
        }

        /// <summary>
        /// 读取二维码或者条形码从图片
        /// </summary>
        /// <param name="imgFile"></param>
        /// <returns></returns>
        public static string ReadFromImage(string imgFile)
        {
            if (string.IsNullOrWhiteSpace(imgFile))
            {
                return "";
            }
            Image img = Image.FromFile(imgFile);
            Bitmap b = new Bitmap(img);

            //该类名称为BarcodeReader,可以读二维码和条形码
            var zzb = new ZXing.ZKWeb.BarcodeReader();
            zzb.Options = new DecodingOptions
            {
                CharacterSet = "UTF-8"
            };
            Result r = zzb.Decode(b);
            string resultText = r.Text;
            b.Dispose();
            img.Dispose();

            return resultText;

        }

        /// <summary>
        /// 将Bitmap  写为byte[]的方法
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static byte[] BitmapToArray(Bitmap bmp)
        {
            byte[] byteArray = null;

            using (MemoryStream stream = new MemoryStream())
            {

                bmp.Save(stream, ImageFormat.Png);
                byteArray = stream.GetBuffer();
            }

            return byteArray;
        }

        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="message"></param>
        /// <param name="gifFileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void CreateCodeTxm(string message, string gifFileName, int width, int height)
        {

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var w = new ZXing.OneD.CodaBarWriter();
            BitMatrix b = w.encode(message, BarcodeFormat.ITF, width, height);
            var zzb = new ZXing.ZKWeb.BarcodeWriter();
            zzb.Options = new EncodingOptions()
            {
                Margin = 3,
                PureBarcode = true
            };
            string dir = Path.GetDirectoryName(gifFileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            Bitmap b2 = zzb.Write(b);
            b2.Save(gifFileName, ImageFormat.Gif);
            b2.Dispose();
        }        
    }
}
