using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;

/// <summary>
///Common 的摘要说明
/// </summary>
public partial class Common
{
    

    public static string md5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
    }

    /// <summary>
    /// 查看guid是否重复
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string Number_repeat(string guid)
    {
        System.Threading.Thread.Sleep(1); //保证yyyyMMddHHmmssffff唯一  
        Random d = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999);
        return WebConfigurationManager.AppSettings["OrgCode"] + strUnique;
    }

    /// <summary>
    /// 得到企业钱包表的支付状态
    /// </summary>
    /// <param name="ProductLine"></param>
    /// <returns></returns>
    public static string GetNameBYPrePayMentStart(int paystartid)
    {
        string StartName = "";

        if (Enum.IsDefined(typeof(Enums.PrePayMentState), paystartid))
        {
            StartName = ((Enums.PrePayMentState)paystartid).ToString();
        }
        return StartName;
    }

    /// <summary>
    /// 得到企业钱包表的款项类型
    /// </summary>
    /// <param name="ProductLine"></param>
    /// <returns></returns>
    public static string GetPrePayStartName(int PayTypeId)
    {
        string TypeName = "";

        if (Enum.IsDefined(typeof(Enums.PrePayType), PayTypeId))
        {
            TypeName = ((Enums.PrePayType)PayTypeId).ToString();
        }
        return TypeName;
    }
}