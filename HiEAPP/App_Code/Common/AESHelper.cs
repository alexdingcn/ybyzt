using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

/// <summary>
///AESHelp 的摘要说明
/// </summary>
public class AESHelper
{
    public AESHelper()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 获取密钥
    /// </summary>
    private static string Key
    {
        get
        {

            return "HaiYuSoftOrder18";    ////必须是16位
        }
    }

    /// <summary>
    /// 获取密钥向量
    /// </summary>
    private static string Iv
    {
        get
        {

            //return "HaiYuSoftOrder18";    ////必须是16位
            return "1hj^5B6k7o8v&*fR";
           
        }
    }

    /// <summary>
    /// 加密方法
    /// </summary>
    /// <param name="toEncrypt">加密字符串</param>
    /// <returns></returns>
    public static string Encrypt_php(string toEncrypt)
    {
        try
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(Iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="toDecrypt">传入解密字符串</param>
    /// <returns></returns>
    public static string Decrypt_php(string toDecrypt)
    {
        try
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(Key);
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(Iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray).TrimEnd('\0');
        }
        catch
        {
            return null;
        }
    }

    #region 成员变量
    /// <summary>
    /// 密钥(32位,不足在后面补0)
    /// </summary>
    private const string _passwd = "ihlih*0037JOHT*)(PIJY*(()JI^)IO%";
    /// <summary>
    /// 运算模式
    /// </summary>
    private static CipherMode _cipherMode = CipherMode.ECB;//之前的加密解密
    //private static CipherMode _cipherMode = CipherMode.CBC;
    /// <summary>
    /// 填充模式
    /// </summary>
    private static PaddingMode _paddingMode = PaddingMode.PKCS7;//之前的加密解密
    //private static PaddingMode _paddingMode = PaddingMode.Zeros;
    /// <summary>
    /// 字符串采用的编码
    /// </summary>
    private static Encoding _encoding = Encoding.UTF8;
    #endregion

    #region 辅助方法
    /// <summary>
    /// 获取32byte密钥数据
    /// </summary>
    /// <param name="password">密码</param>
    /// <returns></returns>
    private static byte[] GetKeyArray(string password)
    {
        if (password == null)
        {
            password = string.Empty;
        }

        if (password.Length < 32)
        {
            password = password.PadRight(32, '0');
        }
        else if (password.Length > 32)
        {
            password = password.Substring(0, 32);
        }

        return _encoding.GetBytes(password);
    }

    /// <summary>
    /// 将字符数组转换成字符串
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    private static string ConvertByteToString(byte[] inputData)
    {
        StringBuilder sb = new StringBuilder(inputData.Length * 2);
        foreach (var b in inputData)
        {
            sb.Append(b.ToString("X2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 将字符串转换成字符数组
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    private static byte[] ConvertStringToByte(string inputString)
    {
        if (inputString == null || inputString.Length < 2)
        {
            throw new ArgumentException();
        }
        int l = inputString.Length / 2;
        byte[] result = new byte[l];
        for (int i = 0; i < l; ++i)
        {
            result[i] = Convert.ToByte(inputString.Substring(2 * i, 2), 16);
        }

        return result;
    }
    #endregion

    #region 加密
    /// <summary>
    /// 加密字节数据
    /// </summary>
    /// <param name="inputData">要加密的字节数据</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static byte[] Encrypt(byte[] inputData, string password)
    {
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.Key = GetKeyArray(password);
        aes.Mode = _cipherMode;
        aes.Padding = _paddingMode;
        ICryptoTransform transform = aes.CreateEncryptor();
        byte[] data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
        aes.Clear();
        return data;
    }
    //后来的加密
    public static byte[] Encrypt(byte[] inputData, string password,string iv)
    {
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.Key = GetKeyArray(password);
        aes.IV = GetKeyArray(iv);
        aes.Mode = _cipherMode;
        aes.Padding = _paddingMode;
        ICryptoTransform transform = aes.CreateEncryptor();
        byte[] data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
        aes.Clear();
        return data;
    }
    //直接复制的代码
    //解密
    public static string Decrypt(string toDecrypt, string key, string iv)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
        byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
        //byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toDecrypt);

        AesCryptoServiceProvider rDel = new AesCryptoServiceProvider();
        rDel.Key = keyArray;
        rDel.IV = ivArray;
        rDel.Mode = CipherMode.CBC;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
       string str_all =  UTF8Encoding.UTF8.GetString(resultArray);
       //int i = str_all.IndexOf("}");
       //string str_return = str_all.Substring(0, i+1);
       //return str_return;
       return str_all;
        //return resultArray;
    }
    //加密
    public static string Encrypt(string toEncrypt, string key, string iv)
    {
        byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
        byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        RijndaelManaged rDel = new RijndaelManaged();
        rDel.Key = keyArray;
        rDel.IV = ivArray;
        rDel.Mode = CipherMode.CBC;
        rDel.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = rDel.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    /// <summary>
    /// 加密字符串(加密为16进制字符串)
    /// </summary>
    /// <param name="inputString">要加密的字符串</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static string Encrypt_android(string inputString)
    {
        try
        {
            //byte[] toEncryptArray = _encoding.GetBytes(inputString);
           string  result = Encrypt(inputString, Key,Iv);
            return result;
        }
        catch
        {
            return null;
        }
    }


    /// <summary>
    /// 加密支付key值(加密为16进制字符串)
    /// </summary>
    /// <param name="inputString">要加密的key值</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static string Encrpt_string(string inputString)
    {
        try
        {
            string result = Encrypt(inputString, Key, Iv);
            return result;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 字符串加密(加密为16进制字符串)
    /// </summary>
    /// <param name="inputString">需要加密的字符串</param>
    /// <returns>加密后的字符串</returns>
    public static string EncryptString(string inputString)
    {
        return Encrypt_android(inputString);
    }
    #endregion

    #region 解密
    /// <summary>
    /// 解密字节数组
    /// </summary>
    /// <param name="inputData">要解密的字节数据</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static byte[] Decrypt(byte[] inputData, string password)
    {
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.Key = GetKeyArray(password);
        aes.Mode = _cipherMode;
        aes.Padding = _paddingMode;
        ICryptoTransform transform = aes.CreateDecryptor();
        byte[] data = null;
        try
        {
            data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
        }
        catch
        {
            return null;
        }
        aes.Clear();
        return data;
    }
    //后来的解密
    public static byte[] Decrypt(byte[] inputData, string password,string iv)
    {
        AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
        aes.Key = GetKeyArray(password);
        aes.IV = GetKeyArray(iv);
        aes.Mode = _cipherMode;
        aes.Padding = _paddingMode;
        ICryptoTransform transform = aes.CreateDecryptor();
        byte[] data = null;
        try
        {
            data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
        }
        catch
        {
            return null;
        }
        aes.Clear();
        return data;
    }
    /// <summary>
    /// 解密16进制的字符串为字符串
    /// </summary>
    /// <param name="inputString">要解密的字符串</param>
    /// <param name="password">密码</param>
    /// <returns>字符串</returns>
    public static string Decrypt_android(string inputString)
    {
        //byte[] toDecryptArray = ConvertStringToByte(inputString);
        //string decryptString = _encoding.GetString(Decrypt(toDecryptArray, Key, Iv));
        string decryptString = Decrypt(inputString, Key, Iv);
        return decryptString;
    }
    public static string Decrypt_android_old(string inputString)
    {
        byte[] toDecryptArray = ConvertStringToByte(inputString);
        string decryptString = _encoding.GetString(Decrypt(toDecryptArray, Key));
        return decryptString;
        //string decryptString = Decrypt(inputString, Key, Iv);
        //return decryptString;
    }
    public static string Encrypt_android_old(string inputString)
    {
        try
        {
            byte[] toEncryptArray = _encoding.GetBytes(inputString);
            byte[] result = Encrypt(toEncryptArray, Key);
            return ConvertByteToString(result);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 解密16进制的字符串为字符串
    /// </summary>
    /// <param name="inputString">需要解密的字符串</param>
    /// <returns>解密后的字符串</returns>
    public static string DecryptString(string inputString)
    {
        return Decrypt_android(inputString);
    }
    #endregion
    //MD5加密
    #region
    public static string Encrypt_MD5(string inputstring)
    {
        byte[] result = Encoding.Default.GetBytes(inputstring);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(result);
        return BitConverter.ToString(output).Replace("-", "").ToLower();
    }
    #endregion


}