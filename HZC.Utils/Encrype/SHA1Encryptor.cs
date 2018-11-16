using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class SHA1Encryptor
{
    public static string SHA1(string content)
    {
        return SHA1(content, Encoding.UTF8);
    }

    public static string SHA1(string content, Encoding encode)
    {
        try
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_in = encode.GetBytes(content);
            byte[] bytes_out = sha1.ComputeHash(bytes_in);
            sha1.Dispose();

            string result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception("SHA1加密失败：" + ex.Message);
        }
    }
}
