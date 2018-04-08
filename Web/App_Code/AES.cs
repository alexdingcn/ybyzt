using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using LitJson;
using Microsoft.SqlServer.Server;
using System.Reflection;

/// <summary>
///AES 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class AES : System.Web.Services.WebService {

    public AES () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "解密，返回的加密编码")]
    public string KeyTransfer(string sign,string valid,string user)
    {
        return new AESHelper().KeyDecrypt(sign,valid, user);
    }

    [WebMethod(Description = "加密，自助定义加密编码")]
    public string KeyMaker(string inKey, string userInfo)
    {
        try
        {
            string outKey = string.Empty;
            string validity = string.Empty;
            string result = new AESHelper(inKey).KeyEncrypt(userInfo, out outKey, out validity);

            if ( result=="-1" || outKey =="-1" || validity =="-1")
                return "{\"sign\":\"-1\",\"valid\":\"-1\",\"user\":\"加密失败\"}";

            return "{\"sign\":\"" + outKey + "\",\"valid\":\"" + validity + "\",\"user\":\"" + result + "\"}";
        }
        catch
        {
            return "{\"sign\":\"-1\",\"valid\":\"-1\",\"user\":\"加密失败\"}";
        }        
    }
}
