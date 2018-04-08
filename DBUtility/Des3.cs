using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBUtility
{
    public class Des3
    {
        public const string IV = "jdjrivps";
        /// <summary>  
        /// DES3 CBC模式加密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="data">明文的byte数组</param>  
        /// <returns>密文的byte数组</returns>  
        public static string EncodeCBC(string key, string data)
        {
            //复制于MSDN  
            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值  
                tdsp.Padding = PaddingMode.PKCS7;       //默认值  
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(Convert.FromBase64String(key), System.Text.Encoding.UTF8.GetBytes(IV)),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it. 
                byte[] dataByte = System.Text.Encoding.UTF8.GetBytes(data);
                cStream.Write(dataByte, 0, dataByte.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the   
                // MemoryStream that holds the   
                // encrypted data.  
                byte[] ret = mStream.ToArray();
                // Close the streams.  
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.  
                return Convert.ToBase64String(ret);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        /// <summary>  
        /// DES3 CBC模式解密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV</param>  
        /// <param name="data">密文的byte数组</param>  
        /// <returns>明文的byte数组</returns>  
        public static string DecodeCBC(string key, string data)
        {
            try
            {
                // Create a new MemoryStream using the passed   
                // array of encrypted data.  
                string rdata = Encoding.UTF8.GetString(Convert.FromBase64String(data));
                byte[] dataByte = Convert.FromBase64String(rdata);
                MemoryStream msDecrypt = new MemoryStream(dataByte);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(Convert.FromBase64String(key), System.Text.Encoding.UTF8.GetBytes(IV)),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.  
                byte[] fromEncrypt = new byte[dataByte.Length];
                // Read the decrypted data out of the crypto stream  
                // and place it into the temporary buffer.  
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.  
                return System.Text.Encoding.UTF8.GetString(fromEncrypt);  

                //byte[] dataByte = Convert.FromBase64String(data);
                ////byte[] keys = Convert.FromBase64String(key);

                //var des = new TripleDESCryptoServiceProvider
                //{
                //    Key = Convert.FromBase64String(key),
                //    Mode = CipherMode.CBC,
                //    Padding = PaddingMode.PKCS7
                //};
                //des.IV = System.Text.Encoding.UTF8.GetBytes(IV);

                //var desDecrypt = des.CreateDecryptor();
                //var result = "";
                //byte[] buffer = Convert.FromBase64String(data);
                //result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
                //return result;

            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        //public static void Main()
        //{
        //    //key的Base64编码  
        //    string key = "testtesttesttesttesttesttesttest";
        //    //加密字符串
        //    string data = "{\"name\":\"\",\"cardNum\":\"\"}";
        //    System.Console.WriteLine("加密字符串:");
        //    string encodeStr = Des3.EncodeCBC(key, data);
        //    System.Console.WriteLine(encodeStr);

        //    System.Console.WriteLine("解密字符串:");
        //    string decodeStr = Des3.DecodeCBC(key, encodeStr);
        //    System.Console.WriteLine(decodeStr);
        //    System.Console.WriteLine();
        //}
    }
}