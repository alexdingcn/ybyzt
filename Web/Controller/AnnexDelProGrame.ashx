<%@ WebHandler Language="C#" Class="AnnexDelProGrame" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Web.SessionState;

/// <summary>
/// 附件删除处理文件
/// </summary>
public class AnnexDelProGrame : IHttpHandler,IReadOnlySessionState {
    System.Web.Script.Serialization.JavaScriptSerializer js = new JavaScriptSerializer();
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string Dtype = context.Request["Dtype"] + "";  //type参数  Dis：代理商 Comp:企业
        string Id = context.Request["Id"] + "";   //企业或者代理商ID
        string AnnexDelName = context.Request["AnnexDelName"] + "";  //附件全名
        if (context.Session["UserModel"] is LoginModel && context.Session["AdminUser"] == null)
        {
            LoginModel Lmodel = context.Session["UserModel"] as LoginModel;
            switch (Lmodel.TypeID)
            {
                case 1:
                case 5: Id = Lmodel.DisID.ToString(); break;
                case 3:
                case 4: Id = Lmodel.CompID.ToString(); break;
                default: Id = Lmodel.DisID.ToString(); break;
            }
        }
        if (Id == "" || AnnexDelName == "" || Dtype == "")
        {
            context.Response.Write(js.Serialize(new ResultMessage { result = false, code = "" }));
            return;
        }
        try
        {
            //判断是企业还是代理商
            if (Dtype == "Dis")
            {
                Hi.Model.BD_Distributor Dis = new Hi.BLL.BD_Distributor().GetModel(Id.ToInt(0));
                if (Dis != null)
                {
                    if (Dis.pic.IndexOf(AnnexDelName) > -1)
                    {
                        if (Dis.pic == AnnexDelName)
                        {
                            Dis.pic = "";
                        }
                        else if (Dis.pic.IndexOf(AnnexDelName) == 0)
                        {
                            Dis.pic = Dis.pic.Replace(AnnexDelName + ",", "");
                        }
                        else
                        {
                            Dis.pic = Dis.pic.Replace("," + AnnexDelName, "");
                        }
                        Dis.ts = DateTime.Now;
                        if (new Hi.BLL.BD_Distributor().Update(Dis))
                        {
                            try
                            {
                                FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../UploadFile/") + AnnexDelName);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }
                            }
                            catch
                            {

                            }
                            context.Response.Write(js.Serialize(new ResultMessage { result = true, code = "删除成功" }));
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Write(js.Serialize(new ResultMessage { result = true, code = "附件不存在" }));
                        return;
                    }
                }
                else
                {
                    context.Response.Write(js.Serialize(new ResultMessage { result = false, code = "代理商不存在" }));
                    return;
                }
            }
            else if (Dtype == "Comp")
            {
                Hi.Model.BD_Company Comp = new Hi.BLL.BD_Company().GetModel(Id.ToInt(0));
                if (Comp != null)
                {
                    if (Comp.Attachment.IndexOf(AnnexDelName) > -1)
                    {
                        if (Comp.Attachment == AnnexDelName)
                        {
                            Comp.Attachment = "";
                        }
                        else if (Comp.Attachment.IndexOf(AnnexDelName) == 0)
                        {
                            Comp.Attachment = Comp.Attachment.Replace(AnnexDelName + ",", "");
                        }
                        else
                        {
                            Comp.Attachment = Comp.Attachment.Replace("," + AnnexDelName, "");
                        }
                        Comp.ts = DateTime.Now;
                        if (new Hi.BLL.BD_Company().Update(Comp))
                        {
                            try
                            {
                                FileInfo file = new FileInfo (HttpContext.Current.Server.MapPath("../UploadFile/") + AnnexDelName);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }
                            }
                            catch
                            {

                            }
                            context.Response.Write(js.Serialize(new ResultMessage { result = true, code = "删除成功" }));
                            return;
                        }
                    }
                    else
                    {
                        context.Response.Write(js.Serialize(new ResultMessage { result = true, code = "附件不存在" }));
                        return;
                    }
                }
                else
                {
                    context.Response.Write(js.Serialize(new ResultMessage { result = false, code = "企业不存在" }));
                    return;
                }
            }
        }
        catch
        {
            context.Response.Write(js.Serialize(new ResultMessage { result = false, code = "删除附件失败,服务器异常" }));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
public class ResultMessage
{
    public bool result { get; set; }

    public string code { get; set; }
}