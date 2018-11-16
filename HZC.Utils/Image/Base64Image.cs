using System;
using System.Drawing;
using System.IO;

namespace HZC.Utils
{
    public class Base64ImageUtil
    {
        public static string Base64StringToImage(string inputStr)
        {
            try
            {
                string dummyData = inputStr.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
                if (dummyData.Length % 4 > 0)
                {
                    dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
                }
                byte[] arr = Convert.FromBase64String(dummyData);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                string newFileName = StringUtil.GetRamCode() + ".jpg"; //随机生成新的文件名
                                                                       //string newThumbnailFileName = "thumb_" + newFileName; //随机生成缩略图文件名
                string path = "/upload/pics/" + DateTime.Today.ToString("yyyyMM") + "/" + DateTime.Today.ToString("dd") + "/";
                string localPath = StringUtil.GetMapPath("~" + path);

                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }

                string fileName = localPath + newFileName;

                bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg); 
                ms.Close();

                return path + newFileName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ImageToBase64(string filePath)
        {
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            System.Drawing.Bitmap bp = new System.Drawing.Bitmap(filePath);
            bp.Save(m, System.Drawing.Imaging.ImageFormat.Gif);
            byte[] b = m.GetBuffer();
            string base64string = Convert.ToBase64String(b);
            return base64string;
        }

        public static string ImageToBase64(Bitmap img)
        {
            System.IO.MemoryStream m = new System.IO.MemoryStream();
            //System.Drawing.Bitmap bp = new System.Drawing.Bitmap(filePath);
            img.Save(m, System.Drawing.Imaging.ImageFormat.Png);
            byte[] b = m.GetBuffer();
            string base64string = Convert.ToBase64String(b);
            return base64string;
        }
    }
}
