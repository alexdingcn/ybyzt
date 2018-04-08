using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

public partial class Company_ImportGoods : CompPageBase
{
    int TitleIndex = 2;
    string categoryId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 批量导入datetable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddList_Click(object sender, EventArgs e)
    {
        string path = "";
        int count = 0;
        int count2 = 0;
        int index = 0;
        try
        {
            if (upload.HasFile == false)//HasFile用来检查FileUpload是否有指定文件
            {
                JScript.AlertMethod(this, "请您选择Excel文件", JScript.IconOption.错误);
                return;//当无文件时,返回
            }
            string IsXls = System.IO.Path.GetExtension(upload.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            if (IsXls != ".xls" && IsXls != ".xlsx")
            {
                JScript.AlertMethod(this, "只可以选择Excel文件", JScript.IconOption.错误);
                return;//当选择的不是Excel文件时,返回
            }
            if (!Directory.Exists(Server.MapPath("GoodsNew/TemplateFile")))
            {
                Directory.CreateDirectory(Server.MapPath("GoodsNew/TemplateFile"));
            }
            string filename = upload.FileName;
            string name = filename.Replace(IsXls, "");
            path = Server.MapPath("GoodsNew/TemplateFile/") + name + "-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + IsXls;
            upload.SaveAs(path);
            System.Data.DataTable dt = Common.ExcelToDataTable(path, TitleIndex);
            if (dt == null)
            {
                throw new Exception("Excel表中无数据");
            }
            if (dt.Rows.Count == 0)
            {
                throw new Exception("Excel表中无数据");
            }

            DataRow[] rows = dt.Select();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("category", typeof(string));     //分类
            dt2.Columns.Add("categoryid", typeof(string));     //分类id
            dt2.Columns.Add("barcode", typeof(string));   //商品编码
            dt2.Columns.Add("goodsname", typeof(string));    //商品名称
            dt2.Columns.Add("inventory", typeof(string));   //商品库存
            dt2.Columns.Add("price", typeof(string)); //商品价格
            dt2.Columns.Add("unit", typeof(string)); //商品计量单位
            dt2.Columns.Add("title", typeof(string)); //卖点
            dt2.Columns.Add("isrecommended", typeof(string)); //店铺显示
            dt2.Columns.Add("isoffline", typeof(string)); //上架
            dt2.Columns.Add("details", typeof(string)); //备注
            dt2.Columns.Add("spec", typeof(string[])); //规格
            dt2.Columns.Add("value1", typeof(string)); //规格属性1
            dt2.Columns.Add("value2", typeof(string)); //规格属性2
            dt2.Columns.Add("value3", typeof(string)); //规格属性3
            dt2.Columns.Add("chkstr", typeof(string)); //检查结果

            HttpContext.Current.Session["GoodsTable"] = null;
            foreach (DataRow row in rows)
            {
                List<string[]> al = new List<string[]>();
                string goodsAttrValue = string.Empty;
                string goodsAttr = string.Empty;
                try
                {
                    if (row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "" && row["商品名称*（必填项，不超过30个汉字）"].ToString().Trim() == "" && row["计量单位*（必填项，如：件、个）"].ToString().Trim() == "" && row["价格*（必填项，最多两位小数）"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    index++;
                    if (row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "示例：食品/面包" || row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "示例：食品/牛奶/伊利" || row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "示例：电器")
                    {
                        continue;
                    }
                    if (row["分类*（必填项，“/”号分割分类）"].ToString().Trim() == "" && row["商品编码（允许为空，如果填写，不超过15个字符）"].ToString().Trim() == "" && row["商品名称*（必填项，不超过30个汉字）"].ToString().Trim() == "" && row["库存（可设置是否启用）"].ToString().Trim() == "" && row["价格*（必填项，最多两位小数）"].ToString().Trim() == "" && row["计量单位*（必填项，如：件、个）"].ToString().Trim() == "" && row["卖点/关键词（允许为空，所填内容用来简单描述商品卖点信息）"].ToString().Trim() == "" && row["店铺显示（是/否）"].ToString().Trim() == "" && row["上架（是/否）"].ToString().Trim() == "" && row["商品详情描述（允许为空）"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    else
                    {
                        DataRow dr1 = dt2.NewRow();
                        //dr1["category"] = row["分类*（必填项，“/”号分割分类）"].ToString().Trim();
                        // dr1["barcode"] = row["商品编码（允许为空，如果填写，不能重复，不超过15个字符）"].ToString().Trim();
                        //dr1["goodsname"] = row["商品名称*（必填项，不能重复，不超过30个汉字）"].ToString().Trim();
                        //dr1["inventory"] = row["库存（可设置是否启用）"].ToString().Trim();
                        // dr1["price"] = row["价格*（必填项，最多两位小数）"].ToString().Trim();
                        //dr1["unit"] = row["计量单位*（必填项，如：件、个）"].ToString().Trim();
                        //dr1["title"] = row["卖点/关键词（允许为空，所填内容用来简单描述商品卖点信息）"].ToString().Trim();
                        dr1["isrecommended"] = row["店铺显示（是/否）"].ToString().Trim() == "" || row["店铺显示（是/否）"].ToString().Trim() == "是" ? "是" : "否";
                        dr1["isoffline"] = row["上架（是/否）"].ToString().Trim() == "" || row["上架（是/否）"].ToString().Trim() == "是" ? "是" : "否";
                        dr1["details"] = row["商品详情描述（允许为空）"].ToString().Trim();
                        dr1["inventory"] = CheckPrice(CheckVal(row["库存（可设置是否启用）"].ToString().Trim(), "商品库存", dr1), "商品库存", dr1);
                        //if (!Util.IsEmpty(kc))
                        //{
                        //    dr1["inventory"] = kc;
                        //}
                        dr1["price"] = CheckPrice(CheckVal(row["价格*（必填项，最多两位小数）"].ToString().Trim(), "价格", dr1), "价格", dr1);
                        dr1["unit"] = CheckVal(row["计量单位*（必填项，如：件、个）"].ToString().Trim(), "计量单位", dr1);
                        string str = string.Empty;//几级商品分类
                        dr1["category"] = CheckVal(row["分类*（必填项，“/”号分割分类）"].ToString().Trim(), "分类", dr1);
                        if (!Util.IsEmpty(row["分类*（必填项，“/”号分割分类）"].ToString().Trim()))
                        {
                            string goodsCate = CheckGoodsCate(CheckVal(row["分类*（必填项，“/”号分割分类）"].ToString().Trim(), "分类", dr1), "", dr1);
                            dr1["categoryid"] = goodsCate.Split('@').Length == 1 ? "" : goodsCate.Split('@')[1];
                        }
                        dr1["title"] = row["卖点/关键词（允许为空，所填内容用来简单描述商品卖点信息）"].ToString().Trim();
                        string[] sepc = CheckSpec(row["多规格字段设置（不同规格用“/”号分割）"].ToString(), "商品规格", dr1);
                        dr1["spec"] = sepc;
                        dr1["barcode"] = GoodsObjExists(CheckVal(row["商品编码（允许为空，如果填写，不超过15个字符）"].ToString().Trim(), "商品编码", dr1), "商品编码",sepc, dr1, dt2);
                        dr1["goodsname"] = GoodsObjExists(CheckVal(row["商品名称*（必填项，不超过30个汉字）"].ToString().Trim(), "商品名称", dr1), "商品名称", sepc, dr1, dt2);
                        dr1["value1"] = CheckSpecValue(row["规格1内容（不同规格内容用“/”号分割）"].ToString().Trim(), "规格1内容", sepc, 1, dr1);
                        dr1["value2"] = CheckSpecValue(row["规格2内容（不同规格内容用“/”号分割）"].ToString().Trim(), "规格2内容", sepc, 2, dr1);
                        dr1["value3"] = CheckSpecValue(row["规格3内容（不同规格内容用“/”号分割）"].ToString().Trim(), "规格3内容", sepc, 3, dr1);
                        if (string.IsNullOrWhiteSpace(dr1["chkstr"].ToString()))
                        {
                            dr1["chkstr"] = "数据正确！";
                        }
                        else
                        {
                            dr1["chkstr"] = dr1["chkstr"].ToString().Substring(4);
                        }
                        dt2.Rows.Add(dr1);
                    }
                }
                catch (Exception ex)
                {
                    dt2.Rows.Clear();
                    if (ex is ApplicationException)
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception("商品Excel模版格式错误，请重新下载模版填入数据后导入。");
                    }
                }
            }
            if (dt2.Rows.Count == 0)
            {
                throw new Exception("Excel表中无数据");
            }
            else
            {
                HttpContext.Current.Session["GoodsTable"] = dt2;
                Response.Redirect("ImportGoods2.aspx", false);
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Session.Remove("GoodsTable");
            JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "");
        }
        finally
        {
            if (!Util.IsEmpty(path))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
    /// <summary>
    /// 非空判断
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string CheckVal(string value, string str, DataRow dr)
    {
        int IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);//是否启用库存
        if (str == "商品库存")
        {
            if (IsInve == 0)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    dr["chkstr"] += "<br>" + str + "已启用不能为空！";
                }
            }
        }
        else if (str == "商品名称")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                dr["chkstr"] += "<br>" + str + "不能为空！";
            }
        }
        else if (str == "计量单位")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                dr["chkstr"] += "<br>" + str + "不能为空！";
            }
        }
        else if (str == "价格")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                dr["chkstr"] += "<br>" + str + "不能为空！";
            }
        }
        return value;
    }
    /// <summary>
    /// 验证商品大类下是否包含这个商品一级分类
    /// </summary>
    /// <returns></returns>
    public string CheckGoodsCate(string value, string str, DataRow dr)
    {
        string catestr = string.Empty;
        string[] catelist = value.Split('/');//根据/分隔商品分类
        for (int i = 0; i < catelist.Length; i++)
        {
            string cateValue = catelist[i].Replace("'", "''");
            if (i == 0)
            {//一级
                
                List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=0   and TypeName='" + cateValue + "'", "");
                if (l.Count == 0)
                {
                    dr["chkstr"] += "<br>" + "没有【" + catelist[i] + "】这个一级分类！";
                    return catestr;
                }
                else
                {
                    catestr = catelist[i] + "@" + l[0].ID;
                    if (catelist.Length == 1)//只有一级分类情况
                    {
                        List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=" + l[0].ID + "", "");
                        if (ll.Count > 0)
                        {

                            dr["chkstr"] += "<br>" + "【" + catelist[i] + "】一级分类下还有二级分类，必须输入最小的分类！";
                            return catestr;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (i == 1)
            {//二级
                string goodsCateId = string.Empty;
                if (catestr.IndexOf("@") != -1)
                {
                    goodsCateId = catestr.Split('@')[1];
                    catestr = catestr.Split('@')[0];
                }
                else
                {
                    List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=0   and TypeName='" + catestr + "'", "");
                    if (ll.Count > 0)
                    {
                        goodsCateId = ll[0].ID.ToString();
                    }
                }
                List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1  and TypeName='" + cateValue + "'  and parentid=" + goodsCateId, "");
                if (l.Count == 0)
                {

                    dr["chkstr"] += "<br>" + "【" + catestr + "】一级分类下没有【" + catelist[i] + "】这个二级分类！";
                    return catestr;
                }
                else
                {
                    catestr = catelist[i] + "@" + l[0].ID;
                    if (catelist.Length == 2)//只有二级分类情况
                    {
                        List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=" + l[0].ID + " ", "");
                        if (ll.Count > 0)
                        {

                            dr["chkstr"] += "<br>" + "【" + catelist[i] + "】二级分类下还有三级分类，必须输入最小的分类！";
                            return catestr;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else if (i == 2)
            { //三级
                string goodsCateId = string.Empty;
                if (catestr.IndexOf("@") != -1)
                {
                    goodsCateId = catestr.Split('@')[1];
                    catestr = catestr.Split('@')[0];
                }
                else
                {
                    List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid!=0   and TypeName='" + catestr + "' ", "");
                    if (ll.Count > 0)
                    {
                        goodsCateId = ll[0].ID.ToString();
                    }
                }

                List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "isnull(dr,0)=0 and isenabled=1 and parentid=" + goodsCateId + " and TypeName='" + cateValue + "' ", "");
                if (l.Count == 0)
                {

                    dr["chkstr"] += "<br>" + "【" + catestr + "】二级分类下没有【" + catelist[i] + "】这个三级分类！";
                    return catestr;
                }
                else
                {
                    catestr = catelist[i] + "@" + l[0].ID;
                    if (catelist.Length == 3)//只有二级分类情况
                    {
                        break;
                    }
                }
            }
        }
        return catestr;
    }
    /// <summary>
    /// 长度验证
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string GoodsObjExists(string value, string str, string[] spec, DataRow dr, DataTable dt)
    {
        if (str == "商品名称")
        {
            if (value.Trim().Length > 60)
            {
                dr["chkstr"] += "<br>" + str + "字符过长！";
            }
            else
            {
                string count = Yanz(value, str, spec, dt);
                //if (count == "1")
                //{
                //    dr["chkstr"] = value + str + "已存在";
                //}
                //else
                    if (count == "2")
                {
                    dr["chkstr"] += "<br>" + str + "不存在这个列！";
                }
                else if (count == "3")
                {
                    dr["chkstr"] += "<br>" + str + "为：" + value + " 在Excel中已存在！";
                }
                return value;
            }
        }
        else if (str == "商品编码")
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Trim().Length > 15)
                {
                    dr["chkstr"] += "<br>" + str + "不允许超过15字符！";
                }
                else
                {
                    return value;
                }
            }
        }
        return "";
    }
    /// <summary>
    /// 验证是否重复
    /// </summary>
    /// <param name="goodsname"></param>
    /// <returns></returns>
    public string Yanz(string goodsname, string str, string[] spec, DataTable dt)
    {
        if (str == "商品名称")
        {

            //List<Hi.Model.BD_Goods> count = new Hi.BLL.BD_Goods().GetList("", "goodsname='" + goodsname.Trim().Replace("'", "''") + "' and isnull(dr,0)=0  and compid=" + this.CompID, "");
            //if (count.Count > 0)
            //{
            //    return "1";
            //}
            //else
            //{
            var rows = from p in dt.AsEnumerable() where p.Field<String>("goodsname") == goodsname.Trim() select p;
            if (rows.Count() > 0)
            {
                return "3";
            }
            return "0";
            //}
        }
        else if (str == "商品编码" && spec.Length == 0)
        {

            //List<Hi.Model.BD_GoodsInfo> count = new Hi.BLL.BD_GoodsInfo().GetList("", "barcode='" + goodsname.Trim() + "' and isnull(dr,0)=0  and compid=" + this.CompID, "");
            //if (count.Count > 0)
            //{
            //    return "1";
            //}
            //else
            //{
            var rows = from p in dt.AsEnumerable() where p.Field<String>("barcode") == goodsname.Trim() select p;
            if (rows.Count() > 0)
            {
                return "3";
            }
            return "0";
            //}
        }
        return "2";
    }
    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string CheckPrice(string value, string str, DataRow dr)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            if (!Regex.IsMatch(value, "^(([0-9]+)|([0-9]+\\.[0-9]{1,5}))$"))
            {
                dr["chkstr"] += "<br>" + str + "输入不正确！";
            }
        }
        return value;
    }


    /// <summary>
    /// 验证规格
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string[] CheckSpec(string value, string str, DataRow dr)
    {
        string[] SpecArry = new string[] { };
        if (!string.IsNullOrWhiteSpace(value))
        {
            try
            {
                SpecArry = value.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string[] newArry = SpecArry.Distinct().ToArray().Select(T => T.Trim()).ToArray();
                if (newArry.Length < SpecArry.Length)
                {
                    dr["chkstr"] += "<br>" + str + " 存在重复的值！";
                }
            }
            catch
            {
                dr["chkstr"] += "<br>" + str + "填写的格式不正确！";
                SpecArry = new string[] { value };
            }
        }
        return SpecArry;
    }


    /// <summary>
    /// 验证规格值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string CheckSpecValue(string value, string str, string[] spec,int len, DataRow dr)
    {
        if (string.IsNullOrWhiteSpace(value) && spec.Length >= len)
        {

            dr["chkstr"] += "<br>【" + spec[len - 1] + "】：【" + str + "】不能为空！";
        }
        else if(!string.IsNullOrWhiteSpace(value) && spec.Length >= len)
        {
           string [] valueArry = value.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string[] newArry = valueArry.Distinct().ToArray().Select(T => T.Trim()).ToArray();
            if (newArry.Length < valueArry.Length)
            {
                dr["chkstr"] += "<br>【" + str + "】存在重复的值！";
            }
        }
        return value;
    }
}