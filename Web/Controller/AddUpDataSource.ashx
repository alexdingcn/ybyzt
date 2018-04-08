<%@ WebHandler Language="C#" Class="AddUpDataSource" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;
using System.Configuration;

public class AddUpDataSource : loginInfoMation, IHttpHandler
{

    string compid = "";
    public void ProcessRequest(HttpContext context)
    {
        string Requst_REFERER = context.Request.ServerVariables["HTTP_REFERER"];
        if (string.IsNullOrWhiteSpace(Requst_REFERER))
        {
            context.Response.Write("禁止地址栏访问处理程序");
            context.Response.End();
        }
        context.Response.ContentType = "text/plain";
        string action = context.Request["action"];
        string GetAction = context.Request["GetAction"] + "";
        string Value = context.Request["Value"] + "";
        string ReturnMsg = "";
        compid = context.Request["CompKey"];
        string Phone = context.Request["phone"] + "";
        switch (action)
        {
            case "AddCart": ReturnMsg = AddCart(context); break;
            case "UpCompBanner": ReturnMsg = UpCompBanner(context, Value); break;
            case "DelCompBanner": ReturnMsg = DelCompBanner(context, Value); break;
            case "DelFiveImg": ReturnMsg = DelFiveImg(context, Value); break;
            case "UpRecommend": ReturnMsg = UpRecommend(context, Value); break;
            case "UpFiveImg": ReturnMsg = UpFiveImg(context, Value); break;
            case "UpCompContact": ReturnMsg = UpCompContact(context, Value); break;
        }
        if (GetAction == "SendPhoneCode")
        {
            ReturnMsg = SendPhoneCode(Phone, context);
        }
        context.Response.Write(ReturnMsg);
        return;
    }

    public string DelFiveImg(HttpContext context, string Value)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        try
        {
            new loginInfoMation().LoadData();
            if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    string CompKey = Common.DesDecrypt(compid, Common.EncryptKey);
                    if (CompKey.ToInt(0) == CompID)
                    {
                        string PathValue = ConfigurationManager.AppSettings["ImgPath"] + "CompFiveImg/" + Value;
                        if (File.Exists(PathValue))
                        {
                            File.Delete(PathValue);
                            msg.Result = true;
                        };
                    }
                    else
                    {
                        msg.Msg = "检测到登陆用户已发生变化，请重新刷新页面。";
                    }
                }
                else
                {
                    msg.Msg = "当前用户非企业，无法操作。";
                }
            }
            else
            {
                msg.Msg = "用户未登陆。";
            }
        }
        catch (Exception ex)
        {

        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string DelCompBanner(HttpContext context, string Value)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        try
        {
            new loginInfoMation().LoadData();
            if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    string CompKey = Common.DesDecrypt(compid, Common.EncryptKey);
                    if (CompKey.ToInt(0) == CompID)
                    {
                        Regex ImgRg = new Regex(@"^\S{0,}_" + CompID + "$");
                        string Extension = Path.GetExtension(Value);
                        if (!string.IsNullOrEmpty(Extension))
                        {
                            Regex ReplaceRg = new Regex(@"" + Extension + "$");
                            Value = ReplaceRg.Replace(Value, "");
                            if (ImgRg.IsMatch(Value))
                            {
                                string PathValue = ConfigurationManager.AppSettings["ImgPath"] + "CompImage/" + Value + Extension;
                                if (File.Exists(PathValue))
                                {
                                    File.Delete(PathValue);
                                    msg.Result = true;
                                };
                            }
                            else
                            {
                                msg.Msg = "删除的图片不存在。";
                            }
                        }
                        else
                        {
                            msg.Msg = "错误的图片名字。";
                        }
                    }
                    else
                    {
                        msg.Msg = "检测到登陆用户已发生变化，请重新刷新页面。";
                    }
                }
                else
                {
                    msg.Msg = "当前用户非企业，无法操作。";
                }
            }
            else
            {
                msg.Msg = "用户未登陆。";
            }
        }
        catch (Exception ex)
        {
            msg.Msg = "删除图片失败";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }


    public string UpCompContact(HttpContext context, string Value)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        try
        {
            new loginInfoMation().LoadData();
            if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    string CompKey = Common.DesDecrypt(compid, Common.EncryptKey);
                    if (CompKey.ToInt(0) == CompID)
                    {
                        List<Hi.Model.BD_Company> ComList = new Hi.BLL.BD_Company().GetList("*", " id='" + CompID + "' and dr=0 and AuditState=2 ", "");
                        if (ComList.Count > 0)
                        {
                            JsonData JsonData = JsonMapper.ToObject(Value);
                            ComList[0].Principal =Common.NoHTML( JsonData["Principal"].ToString());
                            ComList[0].Phone =Common.NoHTML( JsonData["Phone"].ToString());
                            ComList[0].Address =Common.NoHTML( JsonData["Address"].ToString());
                            ComList[0].ts = DateTime.Now;
                            if (UserModel != null)
                            {
                                ComList[0].modifyuser = UserID;
                            }
                            else
                            {
                                ComList[0].modifyuser = (context.Session["AdminUser"] as Hi.Model.SYS_AdminUser).ID;
                            }
                            new Hi.BLL.BD_Company().Update(ComList[0]);
                            msg.Result = true;
                        }
                        else
                        {
                            msg.Msg = "企业异常，无法提交数据。";
                        }
                    }
                    else
                    {
                        msg.Msg = "检测到登陆用户已发生变化，请重新刷新页面。";
                    }
                }
                else
                {
                    msg.Msg = "当前用户非企业，无法操作。";
                }
            }
            else
            {
                msg.Msg = "用户未登陆。";
            }
        }
        catch (Exception ex)
        {
            msg.Msg = "服务器异常，请稍候再试";
            if (ex is System.InvalidOperationException)
            {
                msg.Msg = "请求参数异常，请重试或刷新页面";
            }
            else if (ex is ApplicationException)
            {
                msg.Msg = ex.Message;
            }
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string UpFiveImg(HttpContext context, string Value)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        System.Data.SqlClient.SqlTransaction Tran = null;
        try
        {
            new loginInfoMation().LoadData();
            if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    string CompKey = Common.DesDecrypt(compid, Common.EncryptKey);
                    if (CompKey.ToInt(0) == CompID)
                    {
                        Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                        JsonData JsonArry = JsonMapper.ToObject(Value);
                        //if (JsonArry.Count != 5)
                        //{
                        //    throw new ApplicationException("请求数据超出限制，请重试。");
                        //}
                        var ShopBLL = new Hi.BLL.BD_ShopImageList();
                        string Key = "";
                        string GoodsID = "";
                        string GoodsUrl = "";
                        string GooodsName = "";
                        string ImgUrl = "";
                        List<Hi.Model.BD_ShopImageList> RemoveList;
                        List<string> DelImgList = new List<string>();
                        List<Hi.Model.BD_ShopImageList> FindList = new List<Hi.Model.BD_ShopImageList>();
                        List<Hi.Model.BD_ShopImageList> ImgList = RemoveList = ShopBLL.GetList("*", "isnull(dr,0)=0 and compId=" + CompID, "id");
                        int ListCount = ImgList.Count;
                        foreach (JsonData Jdata in JsonArry)
                        {
                            Key = Jdata["Key"].ToString();
                            GoodsUrl =Common.NoHTML( Jdata["GoodsUrl"].ToString());
                            GoodsID = Jdata["GoodsID"].ToString();
                            ImgUrl = Jdata["ImgUrl"].ToString();
                            GooodsName = Jdata["GoodsName"].ToString();
                            if ((FindList = ImgList.Where(T => T.ID == Key.ToInt(0)).ToList()).Count > 0)
                            {
                                FindList[0].GoodsID = GoodsID.ToInt(0);
                                FindList[0].GoodsUrl = GoodsUrl;
                                if (FindList[0].ImageUrl != ImgUrl)
                                {
                                    DelImgList.Add(FindList[0].ImageUrl);
                                }
                                FindList[0].ImageUrl = ImgUrl;
                                FindList[0].ImageName = GooodsName;
                                FindList[0].ts = DateTime.Now;
                                if (UserModel != null)
                                {
                                    FindList[0].modifyuser = UserID;
                                }
                                else
                                {
                                    FindList[0].modifyuser = (context.Session["AdminUser"] as Hi.Model.SYS_AdminUser).ID;
                                }
                                ShopBLL.Update(FindList[0], Tran);
                                RemoveList.Remove(FindList[0]);
                            }
                            else
                            {
                                Hi.Model.BD_ShopImageList model = new Hi.Model.BD_ShopImageList();
                                model.GoodsID = GoodsID.ToInt(0);
                                model.GoodsUrl = GoodsUrl;
                                model.ImageUrl = ImgUrl;
                                model.ImageName = GooodsName;
                                model.CompID = CompID;
                                model.Type = 2;
                                model.ImageTitle = "";
                                model.ts = DateTime.Now;
                                if (UserModel != null)
                                {
                                    model.modifyuser = UserID;
                                    model.CreateUserID = UserID;
                                }
                                else
                                {
                                    model.CreateUserID = model.modifyuser = (context.Session["AdminUser"] as Hi.Model.SYS_AdminUser).ID;
                                }
                                model.CreateDate = DateTime.Now;
                                ShopBLL.Add(model, Tran);
                            }
                        }
                        //if (ListCount > 0 && RemoveList.Count != 0)
                        //{
                        //    throw new ApplicationException("请求数据异常请刷新页面或重试");
                        //}
                        Tran.Commit();
                        foreach (string Name in DelImgList)
                        {
                            string PathValue = ConfigurationManager.AppSettings["ImgPath"] + "CompFiveImg/" + Name;
                            if (File.Exists(PathValue))
                            {
                                File.Delete(PathValue);
                            };
                        }
                        msg.Result = true;
                    }
                    else
                    {
                        msg.Msg = "检测到登陆用户已发生变化，请重新刷新页面。";
                    }
                }
                else
                {
                    msg.Msg = "当前用户非企业用户";
                }
            }
            else
            {
                msg.Msg = "用户未登陆。";
            }
        }
        catch (Exception ex)
        {
            if (Tran.Connection != null)
            {
                Tran.Rollback();
            }
            msg.Msg = "服务器异常，请稍候再试";
            if (ex is System.InvalidOperationException)
            {
                msg.Msg = "请求参数异常，请重试或刷新页面";
            }
            else if (ex is ApplicationException)
            {
                msg.Msg = ex.Message;
            }
        }
        finally
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string UpRecommend(HttpContext context, string value)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        new loginInfoMation().LoadData();
        System.Data.SqlClient.SqlTransaction Tran = null;
        if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
        {
            try
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    string CompKey = Common.DesDecrypt(compid, Common.EncryptKey);
                    if (CompKey.ToInt(0) == CompID)
                    {
                        Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                        JsonData JsonArry = JsonMapper.ToObject(value);
                        var ShopBLL = new Hi.BLL.BD_ShopGoodsList();
                        int Keyid = 0;
                        JsonData ItemData;
                        List<Hi.Model.BD_ShopGoodsList> FindList = new List<Hi.Model.BD_ShopGoodsList>();
                        List<Hi.Model.BD_ShopGoodsList> ShopList = ShopBLL.GetList("*", "isnull(dr,0)=0 and compId=" + CompID, "title");
                        List<Hi.Model.BD_ShopGoodsList> RemoveShopList = ShopList;
                        foreach (JsonData Jdata in JsonArry)
                        {
                            string Title =Common.NoHTML( Jdata["Title"].ToString());
                            ItemData = Jdata["Data"];
                            foreach (JsonData Jdata2 in ItemData)
                            {
                                string Key = Jdata2["Key"].ToString();
                                string Goodsid = Jdata2["GoodsID"].ToString();
                                string ShowName =Common.NoHTML( Jdata2["ShoaName"].ToString());
                                if (int.TryParse(Key, out Keyid))
                                {
                                    if (int.TryParse(Goodsid, out Keyid))
                                    {
                                        FindList = ShopList.Where(T => T.ID == Key.ToInt(0)).ToList();
                                        if (FindList.Count > 0)
                                        {
                                            FindList[0].Title = Title;
                                            FindList[0].GoodsID = Goodsid.ToInt(0);
                                            FindList[0].ShowName = ShowName;
                                            FindList[0].ts = DateTime.Now;
                                            if (UserModel != null)
                                            {
                                                FindList[0].modifyuser = UserID;
                                            }
                                            else
                                            {
                                                FindList[0].modifyuser = (context.Session["AdminUser"] as Hi.Model.SYS_AdminUser).ID;
                                            }
                                            ShopBLL.Update(FindList[0], Tran);
                                            RemoveShopList.Remove(FindList[0]);
                                        }
                                        else
                                        {
                                            throw new ApplicationException("请求数据异常，请刷新页面重新获取数据");
                                        }
                                    }
                                    else
                                    {
                                        throw new ApplicationException("请求数据异常，请刷新页面重新获取数据");
                                    }
                                }
                                else if (Key == "")
                                {
                                    if (int.TryParse(Goodsid, out Keyid))
                                    {
                                        Hi.Model.BD_ShopGoodsList ShopModel = new Hi.Model.BD_ShopGoodsList();
                                        ShopModel.CreateDate = DateTime.Now;
                                        ShopModel.modifyuser = UserID;
                                        ShopModel.ts = DateTime.Now;
                                        if (UserModel != null)
                                        {
                                            ShopModel.CreateUserID = UserID;
                                            ShopModel.modifyuser = UserID;
                                        }
                                        else
                                        {
                                            ShopModel.CreateUserID = ShopModel.modifyuser = (context.Session["AdminUser"] as Hi.Model.SYS_AdminUser).ID;
                                        }
                                        ShopModel.CompID = CompID;
                                        ShopModel.Title = Title;
                                        ShopModel.GoodsID = Goodsid.ToInt(0);
                                        ShopModel.ShowName = ShowName;
                                        ShopBLL.Add(ShopModel, Tran);
                                    }
                                }
                            }
                        }
                        foreach (Hi.Model.BD_ShopGoodsList model in RemoveShopList)
                        {
                            model.dr = 1;
                            model.modifyuser = UserID;
                            model.ts = DateTime.Now;
                            ShopBLL.Update(model, Tran);
                        }
                        Tran.Commit();
                        msg.Result = true;
                    }
                    else
                    {
                        msg.Msg = "检测到登陆用户已发生变化，请重新刷新页面。";
                    }
                }
                else
                {
                    msg.Msg = "当前用户非企业，无法操作。";
                }
            }
            catch (Exception ex)
            {
                if (Tran.Connection != null) {
                    Tran.Rollback();
                }
                msg.Msg = "服务器异常，请稍候再试";
                if (ex is System.InvalidOperationException)
                {
                    msg.Msg = "请求参数异常，请重试或刷新页面";
                }
                else if (ex is ApplicationException)
                {
                    msg.Msg = ex.Message;
                }
            }
            finally
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                    {
                        Tran.Rollback();
                    }
                }
            }
        }
        else
        {
            msg.Msg = "用户未登陆。";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string UpCompBanner(HttpContext context, string Value)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        try
        {
            new loginInfoMation().LoadData();
            if (UserModel != null || context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
            {
                if (CompID <= 0 && context.Session["AdminUser"] is Hi.Model.SYS_AdminUser)
                {
                    CompID = Common.DesDecrypt(compid, Common.EncryptKey).ToInt(0);
                    Ctype = 1;
                }
                if (Ctype == 1)
                {
                    string CompKey = Common.DesDecrypt(compid, Common.EncryptKey);
                    if (CompKey.ToInt(0) == CompID)
                    {
                        string[] Values = Value.Split(new char[] { ',' });
                        List<Hi.Model.BD_Company> ComList = new Hi.BLL.BD_Company().GetList("*", " id='" + CompID + "' and dr=0 and AuditState=2 ", "");
                        if (ComList.Count > 0)
                        {

                            string[] FirstBanners = ComList[0].FirstBanerImg.Split(new char[] { ',' });
                            if (FirstBanners.Length > 0)
                            {
                                foreach (string FirstBanner in FirstBanners)
                                {
                                    if (!Values.Contains(FirstBanner))
                                    {
                                        string Path = ConfigurationManager.AppSettings["ImgPath"] + "CompImage/" + FirstBanner;
                                        if (File.Exists(Path))
                                        {
                                            File.Delete(Path);
                                        };
                                    }
                                }
                            }
                            ComList[0].FirstBanerImg = Value;
                            ComList[0].ts = DateTime.Now;
                            if (UserModel != null)
                            {
                                ComList[0].modifyuser = UserID;
                            }
                            else
                            {
                                ComList[0].modifyuser = (context.Session["AdminUser"] as Hi.Model.SYS_AdminUser).ID;
                            }
                            new Hi.BLL.BD_Company().Update(ComList[0]);
                            msg.Result = true;
                        }
                        else
                        {
                            msg.Msg = "企业异常，无法修改数据。";
                        }
                    }
                    else
                    {
                        msg.Msg = "检测到登陆用户已发生变化，请重新刷新页面。";
                    }
                }
                else
                {
                    msg.Msg = "当前用户非企业用户";
                }
            }
            else
            {
                msg.Msg = "用户未登陆。";
            }
        }
        catch (Exception ex)
        {
            msg.Msg = "服务器异常，请稍候再试";
        }
        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string AddCart(HttpContext context)
    {
        Information.ResultMsg msg = new Information.ResultMsg();
        string Goodsid = context.Request["Goodsid"];
        new loginInfoMation().LoadData();
        if (!string.IsNullOrWhiteSpace(Goodsid))
        {
            if (UserModel != null)
            {
                List<Hi.Model.BD_Goods> LGoods = new Hi.BLL.BD_Goods().GetList(" top 1 *", "  id='" + Goodsid + "' and isnull(dr,0)=0  ", "");
                if (LGoods.Count == 0)
                {
                    msg.Msg = "收藏的商品不存在，请确认。";
                    return (new JavaScriptSerializer().Serialize(msg));
                }
                //if ((TypeId == 1 || TypeId == 5) && DisID != 0)
                //{
                    Hi.BLL.BD_DisCollect CollectBLL = new Hi.BLL.BD_DisCollect();
                    if (LGoods.Count > 0)
                    {
                        //if (LGoods[0].CompID == CompID)
                        //{
                            if (LGoods[0].IsOffline == 1 && LGoods[0].IsEnabled == 1)
                            {
                                List<Hi.Model.BD_DisCollect> LCollect = CollectBLL.GetList("ID", " Compid=" + LGoods[0].CompID + " and Disid=" + DisID + " and goodsid='" + Goodsid + "' ", "");
                                if (LCollect.Count > 0)
                                {
                                    if (CollectBLL.Delete(LCollect[0].ID))
                                    {
                                        msg.Result = true;
                                        msg.Msg = "取消收藏成功";
                                        msg.Code = "收藏";
                                    }
                                }
                                else
                                {

                                    List<Hi.Model.BD_GoodsAreas> Lares = new Hi.BLL.BD_GoodsAreas().GetList("id", " dr=0 and GoodsID=" + Goodsid + " and disid=" + DisID + " ", "");
                                    if (Lares.Count == 0)
                                    {
                                        Hi.Model.BD_DisCollect model = new Hi.Model.BD_DisCollect();
                                        model.GoodsID = LGoods[0].ID;
                                        model.CompID = LGoods[0].CompID;
                                        model.DisID = DisID;
                                        model.DisUserID = UserID;
                                        model.IsEnabled = 1;
                                        model.CreateUserID = UserID;
                                        model.CreateDate = DateTime.Now;
                                        model.ts = DateTime.Now;
                                        model.dr = 0;
                                        model.modifyuser = UserID;
                                        if (CollectBLL.Add(model) > 0)
                                        {
                                            msg.Result = true;
                                            msg.Msg = "添加收藏成功";
                                            msg.Code = "取消收藏";
                                        }
                                    }
                                    else
                                    {
                                        msg.Msg = "当前用户不在该商品的可售区域内，无法加入收藏。";
                                    }
                                }
                            }
                            else
                            {
                                msg.Msg = "该商品已下架或已被禁用，无法加入收藏。";
                            }
                        //}
                        //else
                        //{
                        //    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id,IsAudit", " isnull(dr,0)=0 and ctype=2  and Compid=" + LGoods[0].CompID + " and Userid=" + UserID + "  ", "");
                        //    if (ListCompUser.Count > 0)
                        //    {
                        //        if (ListCompUser[0].IsAudit == 0)
                        //        {
                        //            msg.Msg = "请等待审核通过后，切换至该企业的代理商后操作。";
                        //        }
                        //        else
                        //        {
                        //            msg.Msg = "请切换至该企业的代理商后操作。";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        msg.Msg = "未加盟该企业，无法加入收藏。";
                        //    }
                        //}
                    }
                    else
                    {
                        msg.Msg = "收藏的商品不存在，请确认。";
                    }
                //}
                //else
                //{
                //    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("id,IsAudit", " isnull(dr,0)=0 and ctype=2 and Compid=" + LGoods[0].CompID + " and Userid=" + UserID + " ", "");
                //    if (ListCompUser.Count > 0)
                //    {
                //        if (ListCompUser[0].IsAudit == 0)
                //        {
                //            msg.Msg = "请等待审核通过后，切换至该企业的代理商后操作。";
                //        }
                //        else
                //        {
                //            msg.Msg = "请切换至该企业的代理商后操作。";
                //        }
                //    }
                //    else
                //    {
                //        msg.Msg = "当前用户不是该企业代理商，请加盟后操作！";
                //    }
                //}
            }
            else
            {
                msg.Msg = "用户未登陆，请登录后操作。";
                msg.Code = "Login";
            }
        }
        else {
            msg.Msg = "请求参数错误，请重试。";
        }

        return (new JavaScriptSerializer().Serialize(msg));
    }

    public string SendPhoneCode(string Phone, HttpContext context)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        try
        {
            List<Hi.Model.SYS_PhoneCode> Lphones = new Hi.BLL.SYS_PhoneCode().GetList("CreateDate", " dr=0 and isPast=0 and  DateDiff(dd,createdate,getdate())=0 and phone='" + Phone + "' ", " CreateDate desc");
            int QuickPaycard = Convert.ToInt32(ConfigCommon.GetNodeValue("Version.xml", "quickcardnum"));
            if (Lphones.Count==QuickPaycard)
            {
                Msg.Msg = "今日加盟，已达到最大次数，请明日再试！";
                Msg.Code ="今日加盟，已达到最大次数，请明日再试！";
                return new JavaScriptSerializer().Serialize(Msg);
            }
            System.Text.RegularExpressions.Regex Phonereg = new System.Text.RegularExpressions.Regex("^0?1[0-9]{10}$");
            if (Phonereg.IsMatch(Phone))
            {
                List<Hi.Model.SYS_PhoneCode> Lphone = new Hi.BLL.SYS_PhoneCode().GetList("CreateDate", " dr=0 and isPast=0 and '" + DateTime.Now + "' between  createdate and DATEADD(MI,30,CreateDate) and Module='企业注册' and phone='" + Phone + "' ", " CreateDate desc");
                if (Lphone.Count > 0)
                {
                    if (Lphone[0].CreateDate.AddMinutes(2) >= DateTime.Now)
                    {
                        Msg.Msg = "" + (int)((Lphone[0].CreateDate.AddMinutes(2) - DateTime.Now).TotalSeconds) + "s后重新获取验证码";
                    }
                    else
                    {
                        string PhoneCode = CreateRandomCode(6);
                        bool IsSend = SendPhone(Phone, context, PhoneCode);
                        Msg.Result = IsSend;
                        Msg.Msg = IsSend ? "" : "短信验证码发送失败，请重试。";
                    }
                }
                else
                {
                    string PhoneCode = CreateRandomCode(6);
                    bool IsSend = SendPhone(Phone, context, PhoneCode);
                    Msg.Result = IsSend;
                    Msg.Msg = IsSend ? "" : "短信验证码发送失败，请重试。";
                }
            }
            else
            {
                Msg.Msg = "手机号码格式错误，请重试。";
            }
        }
        catch (Exception ex)
        {
            Msg.Msg = "短信验证码发送失败，请重试。";
            Msg.Code = ex.Message;
        }

        return new JavaScriptSerializer().Serialize(Msg);
    }

    public bool SendPhone(string Phone, HttpContext context, string Msg)
    {
        try
        {
            string PhoneCodeAccount = System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString();
            string PhoneCodePwd = System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString();
            DBUtility.GetPhoneCode getphonecode = new DBUtility.GetPhoneCode();
            getphonecode.GetUser(PhoneCodeAccount, PhoneCodePwd);
            string str = getphonecode.ReturnSTR(Phone, Msg);
            if (str == "Success")
            {
                Hi.Model.SYS_PhoneCode Phonecode = new Hi.Model.SYS_PhoneCode();
                Phonecode.CreateDate = DateTime.Now;
                Phonecode.dr = 0;
                Phonecode.IsPast = 0;
                Phonecode.modifyuser = 0;
                Phonecode.ts = DateTime.Now;
                Phonecode.Phone = Phone;
                Phonecode.PhoneCode = Msg;
                Phonecode.Module = "企业注册";
                Phonecode.UserName = "";
                Phonecode.Type = 0;
                new Hi.BLL.SYS_PhoneCode().Add(Phonecode);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string CreateRandomCode(int n)
    {
        string code = "";
        Random rand = new Random(unchecked(((int)DateTime.Now.Ticks)));
        for (int i = 0; i < n; i++)
        {
            code += rand.Next(0, 9).ToString();
        }
        return code;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}