using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_NavTopControl : System.Web.UI.UserControl
{

    public string ShowID
    {
        get { return this.showID.Value.Trim(); }
        set { this.showID.Value = value; }
    }
    public string gettxtClientID {
        get { return txtcontent.ClientID; }
    }
    public string getSelectValue
    {
        get { return txtcontent.Value; }
        set { txtcontent.Value = value; }
    }

    public string getSelectType
    {
        set { opt.InnerHtml = value;
            opt.Attributes["type"] = "1";
        }
    }

    public bool  IsIndex { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Hi.Model.SYS_SysName> SysNameList = new Hi.BLL.SYS_SysName().GetList("Value", " name='搜索热词' and  dr=0 ", "");
            if (SysNameList.Count > 0)
            {
                List<string> valuelist = SysNameList[0].Value.Split(',').ToList();
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (var item in valuelist)
                {
                    count++;
                    if (count == 1)
                        sb.Append(" <a href =\"goodslist_0_" + item + ".html\" style=\"color:red\">" + item + "</a>");
                    else
                        sb.Append(" <a href =\"goodslist_0_" + item + ".html\">" + item + "</a>");

                }
                SelectValue.InnerHtml = sb.ToString();
            }
            if (!IsIndex)
            {
                leftNav.Style.Add("display", "none");
            }

            List<Hi.Model.SYS_GType> gType = new Hi.BLL.SYS_GType().GetList("", " ParentId=0 and IsEnabled=1 and ISNULL(dr,0)=0", "");
            if (gType != null && gType.Count > 0)
            {
                this.rpt_Gtype.DataSource = gType;
                this.rpt_Gtype.DataBind();
            }

        }
    }
  
}