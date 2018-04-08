using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using System.Web.Script.Serialization;
using System.Collections;
using System.Data;

/// <summary>
///Transfer 的摘要说明
/// </summary>
public class Transfer
{
	public Transfer()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public string GetTransfer(string Json) {
        Common.CatchInfo(Json, "GetTransfer", "1");

        int UserID = 0;
        int DisID = 0;

        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["UserID"].ToString() == "" || Params["DisID"].ToString() == "")
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());
                DisID = Convert.ToInt32(Params["DisID"].ToString());
            }

        }
        catch
        {
            return "{\"result\":\"F\",\"Description\":\"参数有误！\"}";
        }
       // Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);
       // Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(UserID);
        Hi.Model.SYS_CompUser CompUser_model = Common.Get_CompUser(UserID);
        if (CompUser_model == null)
        {
            return "{\"result\":\"F\",\"Description\":\"参数有误,数据不存在！\"}";
        }
        string strsql = "select dis.DisName DisName,pre.ID TransferNo ,pre.price Price,pre.CreatDate TransferDate,[user].TrueName TransferUser,pre.vdef1 Description from Pay_Prepayment pre left join BD_Distributor dis on pre.DisID=dis.ID left join SYS_Users [user] on pre.CrateUser = [user].ID where pre.Start=1 and pre.PreType=6 and pre.CompID=" + CompUser_model.CompID + " order by pre.CreatDate desc";
        DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strsql).Tables[0];

        ArrayList arrayList = new ArrayList();
        foreach (DataRow dataRow in dt.Rows)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
            foreach (DataColumn dataColumn in dt.Columns)
            {
                dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
            }
            arrayList.Add(dictionary); //ArrayList集合中添加键值
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        string TransferList = js.Serialize(arrayList);

        return "{\"Result\":\"T\",\"Description\":\"成功！\",\"TransferList\":" + TransferList + "}";
    }
    public string GetReceivables(string Json) {
        Common.CatchInfo(Json, "GetReceivables", "1");

        int UserID = 0;

        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["UserID"].ToString() == "")
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                UserID = Convert.ToInt32(Params["UserID"].ToString());
            }

        }
        catch
        {
            return "{\"result\":\"F\",\"Description\":\"参数有误！\"}";
        }
         Hi.Model.SYS_CompUser  CompUser_model=Common.Get_CompUser(UserID);
        //Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(UserID);
         if (CompUser_model == null)
         {
             return "{\"result\":\"F\",\"Description\":\"参数有误,数据不存在！\"}";
         }
        //Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(UserID);
         string strSql = "select dis.ID DisID,DisName,area.AreaName DisDress,DisLevel DisGrade, [Address] DetailedDre, Principal Contact,'' Balance,CompID from BD_Distributor dis left join BD_DisArea area on dis.AreaID=area.ID where isnull(dis.dr,0)=0 and dis.AuditState=2 and dis.CompID =" + CompUser_model.CompID;
        DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strSql).Tables[0];

        ArrayList arrayList = new ArrayList();
        foreach (DataRow dataRow in dt.Rows)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
            foreach (DataColumn dataColumn in dt.Columns)
            {
                if (dataColumn.ColumnName == "Balance")
                {
                    dictionary.Add(dataColumn.ColumnName, new Hi.BLL.PAY_PrePayment().sums(Convert.ToInt32(dataRow["DisID"].ToString()), Convert.ToInt32(dataRow["CompID"].ToString())).ToString("0.00"));
                }
                else
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
            }
            arrayList.Add(dictionary); //ArrayList集合中添加键值
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        string ReceivablesList = js.Serialize(arrayList);
        return "{\"Result\":\"T\",\"Description\":\"成功！\",\"ReceivablesList\":" + ReceivablesList + "}";
    }
    public string GetReDetailed(string Json)
    {
        Common.CatchInfo(Json, "GetReDetailed", "1");

        int DisID = 0;

        JsonData Params = JsonMapper.ToObject(Json);
        try
        {
            if (Params["DisID"].ToString() == "")
            {
                return "{\"result\":\"F\",\"Description\":\"参数错误！\"}";
            }
            else
            {
                DisID = Convert.ToInt32(Params["DisID"].ToString());
            }

        }
        catch
        {
            return "{\"result\":\"F\",\"Description\":\"参数有误！\"}";
        }

        string strSql = "select pre.ID PreNo,price Price,CreatDate InDate,u.TrueName Operator,Start PayType,PreType,vdef1 Description from Pay_Prepayment pre left join SYS_Users u on pre.CrateUser=u.ID where isnull(pre.dr,0)=0 and pre.Start=1 and pre.PreType <>6 and pre.PreType <> 7  and pre.DisID=" + DisID + "order by CreatDate desc";
        DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, strSql).Tables[0];

        ArrayList arrayList = new ArrayList();
        foreach (DataRow dataRow in dt.Rows)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
            foreach (DataColumn dataColumn in dt.Columns)
            {
                if (dataColumn.ColumnName == "PayType")
                {
                    dictionary.Add(dataColumn.ColumnName, Common.GetNameBYPrePayMentStart(Convert.ToInt32(dataRow[dataColumn.ColumnName].ToString())));
                }
                else if (dataColumn.ColumnName == "PreType")
                {
                    dictionary.Add(dataColumn.ColumnName, Common.GetPrePayStartName(Convert.ToInt32(dataRow[dataColumn.ColumnName].ToString())));
                }
                else
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
            }
            arrayList.Add(dictionary); //ArrayList集合中添加键值
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        string ReDetailedList = js.Serialize(arrayList);
        return "{\"Result\":\"T\",\"Description\":\"成功！\",\"ReDetailedList\":" + ReDetailedList + "}";
    }
}