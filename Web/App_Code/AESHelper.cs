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

    private string inKey = string.Empty;

    public AESHelper(string key)
    {
        int length = key.Length;
        if (length > 16)
            key = key.Remove(16);
        else if (length < 16)
        {
            for (int i = 1; i <= 16 - length; i++)
            {
                key += "0";
            }
        }
        inKey = key;
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

            return "HaiYuSoftOrder18";    ////必须是16位
        }
    }

    #region  作废代码
    //默认密钥向量 
    // private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    /// <summary>
    /// AES加密算法
    /// </summary>
    /// <param name="plainText">明文字符串</param>
    /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
    //public static string AESEncrypt(string plainText)
    //{
    //    //分组加密算法
    //    SymmetricAlgorithm des = Rijndael.Create();
    //    byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组 
    //    //设置密钥及密钥向量
    //    des.Key = Encoding.UTF8.GetBytes(Key);
    //    des.IV = _key1;
    //    byte[] cipherBytes = null;
    //    using (MemoryStream ms = new MemoryStream())
    //    {
    //        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
    //        {
    //            cs.Write(inputByteArray, 0, inputByteArray.Length);
    //            cs.FlushFinalBlock();
    //            cipherBytes = ms.ToArray();//得到加密后的字节数组
    //            cs.Close();
    //            ms.Close();
    //        }
    //    }
    //    return Convert.ToBase64String(cipherBytes);
    //}

    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="cipherText">密文字符串</param>
    /// <returns>返回解密后的明文字符串</returns>
    //public static string AESDecrypt(string showText)
    //{
    //    byte[] cipherText = Convert.FromBase64String(showText);

    //    SymmetricAlgorithm des = Rijndael.Create();
    //    des.Key = Encoding.UTF8.GetBytes(Key);
    //    des.IV = _key1;
    //    byte[] decryptBytes = new byte[cipherText.Length];
    //    using (MemoryStream ms = new MemoryStream(cipherText))
    //    {
    //        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
    //        {
    //            cs.Read(decryptBytes, 0, decryptBytes.Length);
    //            cs.Close();
    //            ms.Close();
    //        }
    //    }
    //    return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   ///将字符串后尾的'\0'去掉
    //}

    #endregion

    /// <summary>
    /// 加密方法
    /// </summary>
    /// <param name="toEncrypt">加密字符串</param>
    /// <returns></returns>
    public static string Encrypt_php(string toEncrypt)
    {
        try
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);
            byte[] ivArray = Encoding.UTF8.GetBytes(Iv);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
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
            byte[] keyArray = Encoding.UTF8.GetBytes(Key);
            byte[] ivArray = Encoding.UTF8.GetBytes(Iv);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray).TrimEnd('\0');
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
    private static CipherMode _cipherMode = CipherMode.ECB;
    /// <summary>
    /// 填充模式
    /// </summary>
    private static PaddingMode _paddingMode = PaddingMode.PKCS7;
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

    /// <summary>
    /// 加密字符串(加密为16进制字符串)
    /// </summary>
    /// <param name="inputString">要加密的字符串</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static string Encrypt_android(string inputString)
    {
        if(inputString==null)
            return null;
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
    /// 字符串加密(加密为16进制字符串)
    /// </summary>
    /// <param name="inputString">需要加密的字符串</param>
    /// <returns>加密后的字符串</returns>
    public static string EncryptString(string inputString)
    {
        if (inputString == null)
            return null;
        return Encrypt_android(inputString);
    }

    /// <summary>
    /// 单点登录加密
    /// </summary>
    /// <param name="inputString"></param>
    /// <param name="outKey"></param>
    /// <param name="validity"></param>
    /// <returns></returns>
    public string KeyEncrypt(string inputString, out string outKey, out string validity)
    {
       
        try
        {
            byte[] toEncryptArray = _encoding.GetBytes(inputString);
            byte[] result = Encrypt(toEncryptArray, inKey != "" ? inKey : Key);
            outKey = Encrypt_android(inKey != "" ? inKey : Key);
            validity = Encrypt_android(DateTime.Now.ToString());
            return ConvertByteToString(result);
        }
        catch
        {
            outKey = "-1";
            validity = "-1";
            return "-1";
        }
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

    /// <summary>
    /// 解密16进制的字符串为字符串
    /// </summary>
    /// <param name="inputString">要解密的字符串</param>
    /// <param name="password">密码</param>
    /// <returns>字符串</returns>
    public static string Decrypt_android(string inputString)
    {
        if (inputString == null)
            return null;
        byte[] toDecryptArray = ConvertStringToByte(inputString);
        string decryptString = _encoding.GetString(Decrypt(toDecryptArray, Key));
        return decryptString;
    }

    /// <summary>
    /// 解密16进制的字符串为字符串
    /// </summary>
    /// <param name="inputString">需要解密的字符串</param>
    /// <returns>解密后的字符串</returns>
    public static string DecryptString(string inputString)
    {
        if (inputString == null)
            return null;
        return Decrypt_android(inputString);
    }

    /// <summary>
    /// 单点登录界面   返回: -1 过期  -2 异常
    /// </summary>
    /// <param name="outKey"></param>
    /// <param name="valid"></param>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public string KeyDecrypt(string outKey, string valid , string inputString)
    {
        try
        {
            valid = Decrypt_android(valid);
            if (Convert.ToDateTime(valid) > DateTime.Now.AddSeconds(3))
                return "-1";
            outKey = Decrypt_android(outKey);            
            byte[] toDecryptArray = ConvertStringToByte(inputString);
            string decryptString = _encoding.GetString(Decrypt(toDecryptArray, outKey));
            return decryptString;
        }
        catch
        {
            return "-2";
        }
    }

    #endregion
}