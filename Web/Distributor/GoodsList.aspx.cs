using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_GoodsList : DisPageBase
{
    public int IsInve = 0;//是否启用库存
    public string cxtype = "no";//是否是桌面点最新促销进来
    protected void Page_Load(object sender, EventArgs e)
    {
        cxtype = Request["cx"];//地址栏是否有传值
        if (!IsPostBack)
        {
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            this.hidCompId.Value = this.ddrComp.Value;//厂商id
            this.hidDisId.Value = this.DisID.ToString();//代理商id
            this.hidsDigits.Value = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.DisID); //是否取整
            this.hidFlie.Value = Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/";//图片路径
            this.hidIsInve.Value = OrderInfoType.rdoOrderAudit("商品是否启用库存", this.CompID);//是否启用库存
            object sc = Request["sc"];//收藏商品
            if (sc != null)
            {
                if (sc.ToString().Trim() == "sc")
                {
                    this.hidShouc.Value = sc.ToString().Trim();
                }
            }

            //加入收藏
            if (!Common.HasRight(this.CompID, this.UserID, "2111", this.DisID))
            {
                this.iskeep.Value = "1";
            }
            //加入购物车
            if (!Common.HasRight(this.CompID, this.UserID, "2112", this.DisID))
            {
                this.isaddCart.Value = "1";
            }

        }
    }
}