using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;

public partial class Company_ShopManager_RecommendGoodsList2 : AdminPageBase
{
    public int comPid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj2 = Request["KeyID"];
        if (obj2 != null)
        {
            comPid = Convert.ToInt32(obj2.ToString());
        }
        object obj = Request["action"];
        if (obj != null)
        {
            string action = obj.ToString();
            if (action == "edit")
            {
                string title = Common.NoHTML(Request["title"]);
                Response.Write(GetHtml(title));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            Bind2();
        }
    }
    /// <summary>
    ///左侧绑定
    /// </summary>
    private void Bind2()
    {
        if (comPid != 0)
        {
            StringBuilder html = new StringBuilder();
            List<Hi.Model.BD_ShopGoodsList> l = new Hi.BLL.BD_ShopGoodsList().GetList("", "isnull(dr,0)=0 and compId=" + comPid, "title");
            if (l.Count > 0)
            {
                string title = "";
                for (int i = 0; i < l.Count; i++)
                {
                    if (l[i].Title != title)
                    {
                        title = l[i].Title;
                        if (i != 0)
                        {
                            html.Append("</ul><div class=\"line\"></div>");
                        }
                        html.Append("<div class=\"bt\" style=\"cursor: pointer;\">" + l[i].Title + "<i title=\"编辑\" tip=\"" + l[i].Title + "\" class=\"edit_\">编辑</i></div><ul class=\"list\">");
                        html.Append("<li><a target=\"blank\" href=\"../../e" + l[i].GoodsID + "_" + l[i].CompID + ".html\">" + l[i].ShowName + "</a></li>");
                    }
                    else
                    {
                        html.Append("<li><a target=\"blank\" href=\"../../e" + l[i].GoodsID + "_" + l[i].CompID + ".html\">" + l[i].ShowName + "</a></li>");
                        continue;
                    }
                }
            }
            this.lblHtml.Text = html.ToString();
        }
        else
        {
            // JScript.AlertMsgOne(this, "厂商Id有误", JScript.IconOption.错误);
            JScript.AlertMethod(this, "厂商Id有误", JScript.IconOption.错误, "function(){  window.location.href='../../admin/systems/CompList.aspx'; }");
            return;
        }
    }
    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (comPid != 0)
        {
            SqlTransaction Tran = null;
            try
            {

                object goodsId =Common.NoHTML( Request["hidGoodsId"]);
                object goodsName = Common.NoHTML(Request["txtGoodsName"]);
                object showName = Common.NoHTML(Request["txtShowName"]);
                if (goodsId != null)
                {
                    Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
                    if (!Util.IsEmpty(this.hidtitle.Value.Trim()))
                    {
                        string title = string.Empty;
                        if (this.hidtitle.Value.Trim() != Request["txtTitle"].Trim())
                        {
                            title = Common.NoHTML(this.hidtitle.Value.Trim());
                        }
                        else
                        {
                            title = Common.NoHTML(Request["txtTitle"].Trim());

                        }
                        List<Hi.Model.BD_ShopGoodsList> ll = new Hi.BLL.BD_ShopGoodsList().GetList("", "isnull(dr,0)=0 and compId=" + comPid + " and title='" + title + "'", "", Tran);
                        if (ll.Count > 0)
                        {
                            new Hi.BLL.BD_ShopGoodsList().Delete(comPid, title, Tran);
                        }
                    }
                    for (int i = 0; i < goodsId.ToString().Split(',').Length; i++)
                    {
                        if (!Util.IsEmpty(goodsId.ToString().Split(',')[i].Trim()))
                        {
                            Hi.Model.BD_ShopGoodsList model = null;

                            model = new Hi.Model.BD_ShopGoodsList();
                            model.CompID = comPid;
                            model.Title = Common.NoHTML(Request["txtTitle"].Trim());
                            model.GoodsID = Convert.ToInt32(goodsId.ToString().Split(',')[i].Trim());
                            if (!Util.IsEmpty(showName.ToString().Split(',')[i].Trim()))
                            {
                                model.ShowName = Common.NoHTML(showName.ToString().Split(',')[i].Trim());
                            }
                            else
                            {
                                model.ShowName = Common.NoHTML(goodsName.ToString().Split(',')[i].Trim());
                            }
                            model.CreateDate = DateTime.Now;
                            model.CreateUserID = this.UserID;
                            model.ts = DateTime.Now;
                            model.dr = 0;
                            model.modifyuser = this.UserID;
                            new Hi.BLL.BD_ShopGoodsList().Add(model, Tran);
                        }
                    }

                    Tran.Commit();
                    Response.Redirect("RecommendGoodsList2.aspx?KeyID=" + comPid);
                }

            }
            catch (Exception ex)
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                        Tran.Rollback();
                }
                JScript.AlertMsgOne(this, "操作失败！", JScript.IconOption.错误);
                return;
            }
            finally
            {
                DBUtility.SqlHelper.ConnectionClose();
            }
        }
        else
        {
            // JScript.AlertMsgOne(this, "厂商Id有误", JScript.IconOption.错误);
            JScript.AlertMethod(this, "厂商Id有误", JScript.IconOption.错误, "function(){  window.location.href='../../admin/systems/CompList.aspx'; }");
            return;
        }

    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        if (comPid != 0)
        {
            string title = this.hidtitle.Value.Trim();
            if (!Util.IsEmpty(title))
            {
                new Hi.BLL.BD_ShopGoodsList().Delete(comPid, title, null);
                Response.Redirect("RecommendGoodsList.aspx");
            }
            else
            {
                JScript.AlertMsgOne(this, "删除失败！", JScript.IconOption.错误);
                return;
            }
        }
        else
        {
            // JScript.AlertMsgOne(this, "厂商Id有误", JScript.IconOption.错误);
            JScript.AlertMethod(this, "厂商Id有误", JScript.IconOption.错误, "function(){  window.location.href='../../admin/systems/CompList.aspx'; }");
            return;
        }
    }
    /// <summary>
    /// 得到html数据
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    public string GetHtml(string title)
    {
        if (comPid != 0)
        {
            StringBuilder html = new StringBuilder();
            List<Hi.Model.BD_ShopGoodsList> l = new Hi.BLL.BD_ShopGoodsList().GetList("", "isnull(dr,0)=0 and compId=" + comPid + " and title='" + title + "'", "");
            if (l.Count > 0)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    if (i == 0)
                    {
                        html.Append("<li><i style=\"color: Red\">*</i>分类标题：<input type=\"text\" class=\"textBox txtTitle\" name=\"txtTitle\" value=\"" + l[i].Title + "\" /></li>");
                    }
                    string goodsName = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(l[i].GoodsID)).GoodsName;
                    html.Append("<li><input type=\"hidden\" class=\"hidGoodsId\" name=\"hidGoodsId\"  value=\"" + l[i].GoodsID + "\" /></li><li><i style=\"color: Red\">*</i>选择商品：<input type=\"text\" class=\"textBox txtGoodsName\"  name=\"txtGoodsName\" readonly=\"readonly\" value=\"" + goodsName + "\"  />");
                    if (i == l.Count - 1)
                    {
                        html.Append("<i class=\"add_\" title=\"添加\">添加</i>");
                    }
                    else
                    {
                        html.Append("<i class=\"del_\" title=\"删除\">删除</i>");
                    }
                    html.Append("</li><li><i>*</i>显示名称：<input type=\"text\" class=\"textBox txtShowName\" name=\"txtShowName\" value=\"" + l[i].ShowName + "\" /></li>");
                }
            }
            return html.ToString();
        }
        else
        {
            return "";
        }
    }
}