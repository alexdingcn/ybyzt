using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Data.Common;
using System.Web;


//JSON转换类
public class ConvertJson
{
    #region DataTabel转 JSON格式
    /// <summary> 
    /// Datatable转换为Json 
    /// </summary> 
    /// <param name="table">Datatable对象</param> 
    /// <returns>Json字符串</returns> 
    public static string ToJson(DataTable dt)
    {
        StringBuilder jsonString = new StringBuilder();
        if (dt != null && dt.Rows.Count > 0)
        {
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    strValue = StringFormat(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(strValue + ",");
                    }
                    else
                    {
                        jsonString.Append(strValue);
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
        }
        return jsonString.ToString();
    }
    /// <summary> 
    /// Datatable转换为Json 
    /// </summary> 
    /// <param name="table">Datatable对象</param> 
    /// <returns>Json字符串</returns> 
    public static string ToJson2(DataTable dt)
    {
        StringBuilder jsonString = new StringBuilder();
        jsonString.Append("[");
        DataRowCollection drc = dt.Rows;
        for (int i = 0; i < drc.Count; i++)
        {
            jsonString.Append("{");
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                string strKey = dt.Columns[j].ColumnName;
                string strValue = drc[i][j].ToString();
                Type type = dt.Columns[j].DataType;
                jsonString.Append("\"" + strKey + "\":");
                strValue = StringFormat(strValue, type);
                if (j < dt.Columns.Count - 1)
                {
                    jsonString.Append(strValue + ",");
                }
                else
                {
                    jsonString.Append(strValue);
                }
                if (j == 24)
                {
                    if (!Util.IsEmpty(drc[i]["proTypes"].ToString()))
                    {
                        string html = IsCx(drc[i]["proTypes"].ToString(), drc[i]["ProType"].ToString(), drc[i]["proGoodsPrice"].ToString(), drc[i]["proDiscount"].ToString(), drc[i]["Unit"].ToString());
                        jsonString.Append(",\"Cux\":" + "\"" + html.Replace("\"", "'") + "\"");
                    }
                    else
                    {
                        jsonString.Append(",\"Cux\":" + "\"\"");
                    }
                }

            }
            jsonString.Append("},");
        }
        jsonString.Remove(jsonString.Length - 1, 1);
        jsonString.Append("]");
        return jsonString.ToString();
    }
    /// <summary>
    /// 格式化字符型、日期型、布尔型
    /// </summary>
    private static string StringFormat(string str, Type type)
    {
        if (type == typeof(string))
        {
            str = String2Json(str);
            str = "\"" + str + "\"";
        }
        else if (type == typeof(Guid))
        {
            str = "\"" + str + "\"";
        }
        else if (type == typeof(DateTime))
        {
            str = "\"" + str + "\"";
        }
        else if (type == typeof(bool))
        {
            str = str.ToLower();
        }
        else if (type != typeof(string) && string.IsNullOrEmpty(str))
        {
            str = "\"" + str + "\"";
        }
        return str;
    }
    /// <summary>
    /// 过滤特殊字符
    /// </summary>
    private static string String2Json(String s)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < s.Length; i++)
        {
            char c = s.ToCharArray()[i];
            switch (c)
            {
                case '\"':
                    sb.Append("\\\""); break;
                case '\\':
                    sb.Append("\\\\"); break;
                case '/':
                    sb.Append("\\/"); break;
                case '\b':
                    sb.Append("\\b"); break;
                case '\f':
                    sb.Append("\\f"); break;
                case '\n':
                    sb.Append("\\n"); break;
                case '\r':
                    sb.Append("\\r"); break;
                case '\t':
                    sb.Append("\\t"); break;
                default:
                    sb.Append(c); break;
            }
        }
        return sb.ToString();
    }
    /// <summary>
    /// 判断促销
    /// </summary>
    /// <returns></returns>
    public static string IsCx(string proTypes, string proType, string proGoodsPrice, string proDisCount, string unit)
    {
        LoginModel louser = HttpContext.Current.Session["UserModel"] as LoginModel;
        string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", louser.CompID);
        string str = string.Empty;
        if (!Util.IsEmpty(proTypes))
        {
            if (proTypes == "0")//特价促销
            {
                str = "特价商品";
            }
            else if (proTypes == "1")//商品促销
            {
                if (proType == "3")
                { //商品促销满送
                    str = "满" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(proDisCount).ToString("#,##" + Digits))) + unit + " ，获赠商品（" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(proGoodsPrice).ToString("#,##" + Digits))) + "）" + unit;
                }
                else if (proType == "4")//商品促销打折
                {
                    str = "在原订货价基础上打" + (Convert.ToDecimal(proDisCount) / 10).ToString("N") + "折";
                }
            }
            return "<div class=\"sale-box\"><i class=\"sale\">促销</i><div class=\"sale-txt\"><i class=\"arrow\"></i>" + str + "</div></div>";
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 判断促销
    /// </summary>
    /// <returns></returns>
    public static string IsCxComp(string proTypes, string proType, string proGoodsPrice, string proDisCount, string unit, int Compid)
    {
        string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", Compid);

        string str = string.Empty;
        if (!Util.IsEmpty(proTypes))
        {
            if (proTypes == "0")//特价促销
            {
                str = "特价商品";
            }
            else if (proTypes == "1")//商品促销
            {
                if (proType == "3")
                { //商品促销满送
                    str = "满" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(proDisCount).ToString("#,##" + Digits))) + unit + " ，获赠商品（" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(proGoodsPrice).ToString("#,##" + Digits))) + "）" + unit;
                }
                else if (proType == "4")//商品促销打折
                {
                    str = "在原订货价基础上打" + (Convert.ToDecimal(proDisCount) / 10).ToString("N") + "折";
                }
            }
            return "<div class=\"sale-box\"><i class=\"sale\">促销</i><div class=\"sale-txt\"><i class=\"arrow\"></i>" + str + "</div></div>";
        }
        else
        {
            return "";
        }
    }
    #endregion
}
