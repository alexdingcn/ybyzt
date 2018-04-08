using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using DBUtility;
using Jayrock.Json.Conversion;
using LitJson;

/// <summary>
///BD_CompNews 的摘要说明
/// </summary>
public class BD_CompNews
{
	public BD_CompNews()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
    }

    public ResultNewsList CompNewsList(string JSon)
    {
        try
        {
            StringBuilder str = new StringBuilder();

            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
            string getType = string.Empty; //方向
            string rows = string.Empty;
            string sortType = string.Empty;
            string sort = string.Empty;
            string orderType = string.Empty;
            string DisID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != ""
                && JInfo["Search"].ToString() != "" && JInfo["CriticalOrderID"].ToString() != ""
                && JInfo["GetType"].ToString() != "" && JInfo["Rows"].ToString() != ""
                && JInfo["SortType"].ToString() != "" && JInfo["Sort"].ToString() != ""
                && JInfo["OrderType"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString().Trim();
                compID = JInfo["CompanyID"].ToString().Trim();
                str.Append(" AND CompID='" + compID + "' AND IsEnabled=1 and dr=0");
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
                orderType = JInfo["OrderType"].ToString();//0：只按时间 1：置顶优先，再按时间排序
                //DisID = JInfo["ResellerID"].ToString();//经销商登录时需要传入经销商ID用于对登录信息进行判断
            }
            else
            {
                
                return new ResultNewsList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            //if (DisID == "")
            //{
            //    if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(compID)))
            //        return new ResultNewsList() { Result = "F", Description = "登录信息异常" };
            //    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(compID));
            //    if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
            //        return new ResultNewsList() { Result = "F", Description = "核心企业异常" };
            //}
            //else
            //{
            //    if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(DisID)))
            //        return new ResultNewsList() { Result = "F", Description = "登录信息异常" };
            //    //判断经销商信息是否异常
            //    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(DisID));
            //    if (dis == null || dis.dr == 1 || dis.IsEnabled == 0 || dis.AuditState == 0)
            //        return new ResultNewsList() { Result = "F", Description = "经销商信息异常" };
            //    //判断经销商对应的核心企业信息是否异常
            //    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(dis.CompID);
            //    if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
            //        return new ResultNewsList() { Result = "F", Description = "核心企业异常" };
            //}

            JsonData JMsg = JInfo["Search"];
            if (JMsg.Count > 0)
            {
                string NewsID = JMsg["NewsID"].ToString().Trim();
                string Title = JMsg["Title"].ToString().Trim();
                string IsEnabled = JMsg["IsEnable"].ToString().Trim();
                string IsTop = JMsg["IsTop"].ToString().Trim();
                string NewsType = JMsg["NewsType"].ToString().Trim();

                string ShowType = JMsg["ShowType"].ToString().Trim();

                if (NewsID != "-1")
                    str.Append(" and ID ='" + NewsID + "' ");
                if (Title != "-1")
                    str.Append(" and NewsTitle like '%" + Title + "%' ");
                if (IsEnabled != "-1")
                    str.Append(" and IsEnable = '" + IsEnabled + "' ");
                if (IsTop != "-1")
                    str.Append(" and IsTop = '" + IsTop + "' ");
                if (NewsType != "-1")
                    str.Append(" and NewsType = '" + NewsType + "' ");
                if (ShowType != "-1")
                    str.Append(ShowType == "3" ? " and ShowType = '1,2' " : " and ShowType = '" + ShowType + "' ");
            }

            #endregion

            List<NewsInfo> NewsList = new List<NewsInfo>();

            if (orderType == "1")
            {
                #region 取当前置顶的新闻公告

                List<Hi.Model.BD_CompNews> newsList = new Hi.BLL.BD_CompNews().GetList("",
                    " IsEnabled =1 and IsTop =1 and CompID='" + compID + "' and dr=0", "createDate desc");
                if (newsList != null && newsList.Count != 0)
                {
                    str.Append(" and ID not in (-1");  //通用列表中去除置顶公告
                    if (criticalOrderID.Trim() == "-1")
                    {
                    //    List<NewsInfo> NewsList_select = newsList.Select(newsInfo => new NewsInfo()
                    //       {
                    //           NewsID = newsInfo.ID.ToString(),
                    //           Title = newsInfo.NewsTitle,
                    //           IsEnabled = newsInfo.IsEnabled.ToString(),
                    //           IsTop = newsInfo.IsTop.ToString(),
                    //           NewsType = newsInfo.NewsType.ToString(),

                    //           ShowType =
                    //               newsInfo.ShowType != "1" && newsInfo.ShowType != "2" && ClsSystem.gnvl(newsInfo.ShowType,"").Trim() != ""
                    //               ? "3" : newsInfo.ShowType,
                    //           CreateTime = newsInfo.CreateDate.ToString()
                    //       }
                    //   ).ToList();
                        foreach (
                            NewsInfo news in newsList.Select(newsInfo => new NewsInfo()
                            {
                                NewsID = newsInfo.ID.ToString(),
                                Title = newsInfo.NewsTitle,
                                IsEnabled = newsInfo.IsEnabled.ToString(),
                                IsTop = newsInfo.IsTop.ToString(),
                                NewsType = newsInfo.NewsType.ToString(),

                                ShowType =
                                    newsInfo.ShowType != "1" && newsInfo.ShowType != "2" && ClsSystem.gnvl(newsInfo.ShowType,"").Trim() != ""
                                    ? "3" : newsInfo.ShowType,
                                CreateTime = newsInfo.CreateDate.ToString()
                            }
                        ))
                        //foreach (NewsInfo news in NewsList_select)
                        {
                            NewsList.Add(news);
                            str.Append("," + news.NewsID);
                        }
                    }
                    str.Append(")");
                }

                #endregion

                int dtRow = Convert.ToInt32(rows) - NewsList.Count;
                if (dtRow > 1)
                    rows = dtRow.ToString();

            }

            #region 模拟分页

            string tabName = " [dbo].[BD_CompNews]"; //表名
            string strsql = string.Empty; //搜索sql
            sortType = "CreateDate" ;
            sort = "0";
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType, sort, str.ToString(), getType, rows);
            if (strsql == "")
                return new ResultNewsList() { Result = "F", Description = "基础数据异常" };

            #endregion

            #region 赋值

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
            {
                if (NewsList.Count==0)
                    return new ResultNewsList() { Result = "T", Description = "没有更多数据" };
                else
                {
                    return new ResultNewsList() { Result = "T", Description = "", NewsList = NewsList };
                }
            }
            DataTable dtList = ds.Tables[0];
            if (dtList != null)
            {
                if (dtList.Rows.Count == 0)
                {
                    if (NewsList.Count != 0)
                        return new ResultNewsList() {Result = "T", Description = "", NewsList = NewsList};
                    else
                    {
                        return new ResultNewsList() { Result = "F", Description = "没有更多数据" };
                    }
                }
                foreach (DataRow newsInfo in dtList.Rows)
                {
                    NewsInfo news = new NewsInfo();

                    news.NewsID = newsInfo["ID"].ToString();
                    news.Title = newsInfo["NewsTitle"].ToString();
                    news.IsOverdue = "0";
                    if (newsInfo["NewsType"].ToString() == "4")
                    {
                        if (newsInfo["PmID"]!=null && Convert.ToInt32(newsInfo["PmID"]) !=0)
                        {
                            Hi.Model.BD_Promotion pro = new Hi.BLL.BD_Promotion().GetModel(Convert.ToInt32(newsInfo["PmID"].ToString()));
                            if (pro.ProEndTime < DateTime.Now)
                            {
                                news.IsOverdue = "1";
                            }
                        }
                    }
                    news.IsEnabled = newsInfo["IsEnabled"].ToString();
                    news.IsTop = newsInfo["IsTop"].ToString();
                    news.NewsType = newsInfo["NewsType"].ToString();
                    news.ShowType = newsInfo["ShowType"].ToString() != "1" && newsInfo["ShowType"].ToString() != "2" &&
                                    newsInfo["ShowType"].ToString().Trim() != "" ? "3" : newsInfo["ShowType"].ToString();
                    news.CreateTime = newsInfo["CreateDate"].ToString();
                    
                    NewsList.Add(news);
                }
            }

            #endregion

            return new ResultNewsList() { Result = "T", Description = "获取成功", NewsList = NewsList };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompNewsList ：" + JSon);
            return new ResultNewsList() { Result = "F", Description = "异常" };
        }
    }

    public ResultNewsInfo GetNewsInfo(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string newsID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["NewsID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString().Trim();
                compID = JInfo["CompanyID"].ToString().Trim();
                newsID = JInfo["NewsID"].ToString().Trim();
            }
            else
            {
                return new ResultNewsInfo() { Result = "F", Description = "参数异常" };
            }

            //Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            //if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(compID)))
            //    return new ResultNewsInfo() { Result = "F", Description = "登录信息异常" };

            #endregion

            Hi.Model.BD_CompNews newsInfo = new Hi.BLL.BD_CompNews().GetModel(Convert.ToInt32(newsID));
            if (newsInfo == null)
                return new ResultNewsInfo() { Result = "F", Description = "未找到信息" };
            NewsInfo news = new NewsInfo()
            {
                NewsID = newsInfo.ID.ToString(),
                Title = newsInfo.NewsTitle,
                Contents = newsInfo.NewsContents,
                IsEnabled = newsInfo.IsEnabled.ToString(),
                IsTop = newsInfo.IsTop.ToString(),
                NewsType = newsInfo.NewsType.ToString(),
                ShowType = newsInfo.ShowType != "1" && newsInfo.ShowType != "2" && newsInfo.ShowType.Trim() != "" ? "3" : newsInfo.ShowType,
                CreateTime = newsInfo.CreateDate.ToString()
            };
            return new ResultNewsInfo() { Result = "T", Description = "返回成功", NewsInfo = news };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetNewsInfo ：" + JSon);
            return new ResultNewsInfo() { Result = "F", Description = "异常" };
        }
    }

    public ResultNewsAdd CompNewsAdd(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string title = string.Empty;
            string contents = string.Empty;
            string IsEnabled = string.Empty;
            string IsTop = string.Empty;
            string NewsType = string.Empty;
            string ShowType = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompanyID"].ToString() != "" &&
                JInfo["Title"].ToString() != ""  &&
                JInfo["IsEnable"].ToString() != "" && JInfo["IsTop"].ToString() != "" &&
                JInfo["NewsType"].ToString() != "" )
            {
                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
                title = Common.NoHTML(JInfo["Title"].ToString());
                contents = Common.NoHTML(JInfo["Contents"].ToString());
                IsEnabled = JInfo["IsEnable"].ToString();
                IsTop = JInfo["IsTop"].ToString();
                NewsType = JInfo["NewsType"].ToString();
                ShowType = JInfo["ShowType"].ToString();
            }
            else
            {
                return new ResultNewsAdd() {Result = "F", Description = "参数异常"};
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(compID)))
                return new ResultNewsAdd() { Result = "F", Description = "登录信息异常" };

            #endregion

            Hi.Model.BD_CompNews NewsNotice = new Hi.Model.BD_CompNews();
            NewsNotice.CompID = Convert.ToInt32(compID);
            NewsNotice.NewsTitle = title;
            NewsNotice.NewsContents = contents;
            NewsNotice.IsEnabled = IsEnabled=="1" ? 1 : 0;//是否启用
            NewsNotice.IsTop = IsTop=="1" ? 1 : 0;//是否置顶
            if (NewsType.Trim() != "1" && NewsType.Trim() != "2" && NewsType.Trim() != "3" &&
                NewsType.Trim() != "4" && NewsType.Trim() != "5")
                return new ResultNewsAdd() {Result = "F", Description = "类别异常"};
            NewsNotice.NewsType = Convert.ToInt32(NewsType);//类别
            if (ShowType.Trim() != "1" && ShowType.Trim() != "2" && ShowType.Trim() != "3" && ShowType.Trim() != "-1")
                return new ResultNewsAdd() {Result = "F", Description = "展示效果异常"};
            if (ShowType != "-1")
                NewsNotice.ShowType = ShowType =="3"?"1,2":ShowType;//标红、加new
            //标准参数
            NewsNotice.CreateDate = DateTime.Now;
            NewsNotice.CreateUserID = Convert.ToInt32(userID);
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = Convert.ToInt32(userID);
            int newsrid = 0;
            newsrid = new Hi.BLL.BD_CompNews().Add(NewsNotice);

            if (newsrid <= 0)
                return new ResultNewsAdd() {Result = "F", Description = "添加失败", NewsID = newsrid.ToString()};
            if (IsTop=="1")//置顶数量不能超过3个
            {
                List<Hi.Model.BD_CompNews> CompNew = new Hi.BLL.BD_CompNews().GetList("", " isnull(dr,0)=0 and Compid=" + compID + " and IsEnabled=1 and istop=1 and PMID=0", " createdate desc");
                if (CompNew.Count > 3)
                {
                    for (int i = 0; i < CompNew.Count; i++)
                    {
                        if (i > 1 && CompNew[i].ID != newsrid)
                        {
                            CompNew[i].IsTop = 0;
                            NewsNotice.ts = DateTime.Now;
                            NewsNotice.modifyuser = Convert.ToInt32(userID);
                            new Hi.BLL.BD_CompNews().Update(CompNew[i]);
                        }
                    }
                }
            }
            
            new MsgSend().GetMsgService(newsrid.ToString(),"1");
            return new ResultNewsAdd() { Result = "T", Description = "添加成功", NewsID = newsrid.ToString() };
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "CompNewsAdd ：" + JSon);
            return new ResultNewsAdd() {Result = "F", Description = "异常"};
        }
    }

    public ResultNoRead GetAnnouncement(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != ""
                && JInfo["MessageIDList"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString().Trim();
                disID = JInfo["ResellerID"].ToString().Trim();
            }
            else
            {
                return new ResultNoRead() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultNoRead() { Result = "F", Description = "用户异常" };
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled==0)
                return new ResultNoRead() { Result = "F", Description = "经销商异常" };

            List<Message> messageList = new List<Message>();
            JsonData rList = JInfo["MessageIDList"];
            if (rList.Count == 0)
                return new ResultNoRead() { Result = "F", Description = "参数异常" };
            foreach (JsonData JMsg in rList)
            {
                if (JMsg.Count > 0)
                {
                    string NewsID = JMsg["MessageID"].ToString().Trim();
                    string Type = JMsg["MessageType"].ToString().Trim();
                    string strWhere = " and Compid='" + dis.CompID + "' and IsEnabled=1 and dr=0 and ID >" + NewsID;
                    if (Type != "6")
                        strWhere += " and newstype='" + Type + "'";

                    List<Hi.Model.BD_CompNews> list = Common.GetDataSource<Hi.Model.BD_CompNews>("*", strWhere);
                    if (list != null && list.Count > 0)
                    {
                        Message message = new Message();
                        message.MessageType = Type;
                        message.MessageIDList = list.Select(item => item.ID.ToString()).ToList();
                        messageList.Add(message);
                    }
                    else
                    {
                        Message message = new Message()
                        {
                            MessageType = Type,
                            MessageIDList = default(List<string>)
                        };
                        messageList.Add(message);
                    }
                }
            }

            #endregion

            return new ResultNoRead() { Result = "T", Description = "获取成功", MessageList = messageList };
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetAnnouncement ：" + JSon);
            return new ResultNoRead() { Result = "F", Description = "异常" };
        }
    }

    public ResultNoTime GetMessageDeleteId(string JSon)
    {
        try
        {
            string userID = string.Empty;
            string disID = string.Empty;
            string IDlist = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != ""
                && JInfo["UnreadMessageID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString().Trim();
                disID = JInfo["ResellerID"].ToString().Trim();
                IDlist = JInfo["UnreadMessageID"].ToString().Trim();
            }
            else
            {
                return new ResultNoTime() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultNoTime() { Result = "F", Description = "用户异常" };
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultNoTime() { Result = "F", Description = "经销商异常" };

            string newlist = string.Empty;
            List<int> intList = new List<int>();
            List<Hi.Model.BD_CompNews> list = new Hi.BLL.BD_CompNews().GetList("", "ID in (" + IDlist + ") and (dr=1 or IsEnabled=0)", "");
            if (list != null && list.Count > 0)
            {
                newlist = string.Join(",", list.Select(p=>p.ID));
            }
            return new ResultNoTime() { Result = "T", Description = "获取成功", DeleteMessageID = newlist };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetMessageDeleteId ：" + JSon);
            return new ResultNoTime() { Result = "F", Description = "参数异常" };
        }
    }

    #region 返回

    public class ResultNoTime
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string DeleteMessageID { get; set; }
    }

    public class ResultNoRead
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<Message> MessageList { get; set; }
    }

    public class Message
    {
        public List<string> MessageIDList { get; set; }
        public string MessageType { get; set; }
    }

    public class ResultNewsList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<NewsInfo> NewsList { get; set; }
    }

    public class ResultNewsInfo
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public NewsInfo NewsInfo { get; set; }
    }

    public class NewsInfo
    {
        public string NewsID { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public string IsEnabled { get; set; }// 0:不启用 1:启用
        public string IsTop { get; set; }    // 0:不置顶 1:置顶
        public string NewsType { get; set; } // 1:新闻 2:通知 3:公告 4:促销 5:企业动态 
        public string ShowType { get; set; } // 1:new 2:标红 3:new + 标红
        public string CreateTime { get; set; }
        public string IsOverdue { get; set; } 
    }

    public class ResultNewsAdd
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string NewsID { get; set; }
    }

    #endregion
}