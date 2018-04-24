    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.OleDb;
    using System.Web.UI.HtmlControls;
    using System.Net;
    using System.Configuration;
    using System.Text;

    public partial class Company_SysManager_FCMaterialEdit : CompPageBase
    {
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {
            bind();
            DataBindLink();
        }
      
    }


    /// <summary>
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        string id = Request.QueryString["id"];
        if (!string.IsNullOrWhiteSpace(id))
        {


            List<Hi.Model.YZT_Annex> annexList = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + id + " and dr=0 and fileAlias='4'", "");
            foreach (Hi.Model.YZT_Annex item in annexList)
            {
                    if (!string.IsNullOrEmpty(item.fileName))
                    {
                        LinkButton linkFile = new LinkButton();
                        linkFile.Click += new EventHandler(Download_Click);
                        if (item.fileName.LastIndexOf("_") != -1)
                        {
                            string text = item.fileName.Substring(0, item.fileName.LastIndexOf("_")) + Path.GetExtension(item.fileName);
                            if (text.Length < 15)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0, 15) + "...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        else
                        {
                            string text = item.fileName.Substring(0, item.fileName.LastIndexOf("-")) + Path.GetExtension(item.fileName);
                            if (text.Length < 15)
                                linkFile.Text = text;
                            else
                            {
                                linkFile.Text = text.Substring(0, 15) + "...";
                            }
                            linkFile.Attributes.Add("title", text);
                        }
                        linkFile.Style.Add("margin-right", "5px");
                        linkFile.Style.Add("text-decoration", "underline");
                        linkFile.Attributes.Add("fileName", item.fileName);
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Controls.Add(linkFile);

                    }
                }
           
        }
    }

    /// <summary>
    /// 页面数据绑定
    /// </summary>
    public void bind()
    {
        string id = Request.QueryString["id"];
        if (!string.IsNullOrWhiteSpace(id))
        {
            //修改
            Hi.Model.YZT_FCmaterials fCmaterialsModel = new Hi.BLL.YZT_FCmaterials().GetModel(Convert.ToInt32(id));
            if (fCmaterialsModel != null) {
                this.txtRise.Value = fCmaterialsModel.Rise;
                this.txtContent.Value = fCmaterialsModel.Content;
                this.txtOBank.Value = fCmaterialsModel.OBank;
                this.txtOAccount.Value = fCmaterialsModel.OAccount;
                this.txtTRNumber.Value = fCmaterialsModel.TRNumber;
                List<Hi.Model.YZT_Annex> annexList = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + id + " and dr=0 and fileAlias='4'", "");
                foreach (Hi.Model.YZT_Annex item in annexList)
                {
                    if (item.type == 5)
                    {
                        //营业执照绑定
                         this.HidFfileName.Value= item.fileName;
                         this.validDate.Value= item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 6)
                    {
                        //生产许可证绑定
                        this.HidFfileName2.Value = item.fileName;
                        this.validDate2.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 12)
                    {
                        this.HidFfileName3.Value = item.fileName;
                        this.validDate3.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 13)
                    {
                        this.HidFfileName4.Value = item.fileName;
                        this.validDate4.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 14)
                    {
                        this.HidFfileName5.Value = item.fileName;
                        this.validDate5.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 15)
                    {
                        this.HidFfileName6.Value = item.fileName;
                        this.validDate6.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 16)
                    {
                        this.HidFfileName7.Value = item.fileName;
                        this.validDate7.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 17)
                    {
                        this.HidFfileName8.Value = item.fileName;
                        this.validDate8.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 18)
                    {
                        this.HidFfileName9.Value = item.fileName;
                        this.validDate9.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                    else if (item.type == 19)
                    {
                        this.HidFfileName10.Value = item.fileName;
                        this.validDate10.Value = item.validDate.ToString("yyyy/MM/dd");
                    }
                }

            }
        }
    }


    /// <summary>
    /// 获取页面数据Model
    /// </summary>
    /// <returns></returns>
    private Hi.Model.YZT_FCmaterials getfCmaterialsModel(Hi.Model.YZT_FCmaterials fCmaterialsModel) {
        fCmaterialsModel.CompID = CompID;
        fCmaterialsModel.DisID = 0;
        fCmaterialsModel.type = 1;
        fCmaterialsModel.Rise = this.txtRise.Value.Trim();
        fCmaterialsModel.Content = this.txtContent.Value.Trim();
        fCmaterialsModel.OBank = this.txtOBank.Value.Trim();
        fCmaterialsModel.OAccount = this.txtOAccount.Value.Trim();
        fCmaterialsModel.TRNumber = this.txtTRNumber.Value.Trim();
        fCmaterialsModel.ts = DateTime.Now;
        fCmaterialsModel.modifyuser = UserID;

        return fCmaterialsModel;
    }

    /// <summary>
    /// 确认按钮单击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string HidFfileNameNew = this.HidFfileName.Value.Trim();//营业执照 附件名称
        string HidFfileNameOld = "";//营业执照 附件旧名称(更新后 需要删除的旧附件) 

        string HidFfileNameNew2 = this.HidFfileName2.Value.Trim();//生产许可证 附件名称
        string HidFfileNameOld2 = "";//生产许可证 附件旧名称(更新后 需要删除的旧附件) 

        string HidFfileNameNew3 = this.HidFfileName3.Value.Trim();// 附件名称
        string HidFfileNameOld3 = "";

        string HidFfileNameNew4 = this.HidFfileName4.Value.Trim();// 附件名称
        string HidFfileNameOld4 = "";

        string HidFfileNameNew5 = this.HidFfileName5.Value.Trim();// 附件名称
        string HidFfileNameOld5 = "";

        string HidFfileNameNew6 = this.HidFfileName6.Value.Trim();// 附件名称
        string HidFfileNameOld6 = "";

        string HidFfileNameNew7 = this.HidFfileName7.Value.Trim();// 附件名称
        string HidFfileNameOld7 = "";

        string HidFfileNameNew8 = this.HidFfileName8.Value.Trim();// 附件名称
        string HidFfileNameOld8 = "";

        string HidFfileNameNew9 = this.HidFfileName9.Value.Trim();// 附件名称
        string HidFfileNameOld9 = "";

        string HidFfileNameNew10 = this.HidFfileName10.Value.Trim();// 附件名称
        string HidFfileNameOld10 = "";


        Hi.Model.YZT_FCmaterials fCmaterialsModel =null;
        string id = Request.QueryString["id"];
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            int fid = 0;
            //新增
            if (string.IsNullOrWhiteSpace(id))
            {
                fCmaterialsModel = new Hi.Model.YZT_FCmaterials();
                fCmaterialsModel = getfCmaterialsModel(fCmaterialsModel);
                fCmaterialsModel.CreateUserID = UserID;
                fCmaterialsModel.CreateDate = DateTime.Now;
                fCmaterialsModel.dr = 0;
                fid = new Hi.BLL.YZT_FCmaterials().Add(fCmaterialsModel, Tran);
                if (fid > 0)
                {
                    DateTime time = DateTime.Now;
                    int count = 0;
                    try
                    {
                        count = 1;
                        //新增营业执照附件表
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName.Value.Trim()))
                        {
                            Hi.Model.YZT_Annex annexModel = insertAnnex(Convert.ToInt32(fid), 5, this.HidFfileName.Value.Trim(), this.validDate.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName2.Value.Trim()))
                        {
                            //新增生产许可证表
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 6, this.HidFfileName2.Value.Trim(), this.validDate2.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName3.Value.Trim()))
                        {
                            //新增税务登记证
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 12, this.HidFfileName3.Value.Trim(), this.validDate3.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName4.Value.Trim()))
                        {
                            //新增开户银行许可证
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 13, this.HidFfileName4.Value.Trim(), this.validDate4.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName5.Value.Trim()))
                        {
                            //新增质量管理体系调查表
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 14, this.HidFfileName5.Value.Trim(), this.validDate5.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName6.Value.Trim()))
                        {
                            //新增GSP/GMP证书 
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 15, this.HidFfileName6.Value.Trim(), this.validDate6.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }

                        if (!string.IsNullOrWhiteSpace(this.HidFfileName7.Value.Trim()))
                        {
                            //新增开票信息
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 16, this.HidFfileName7.Value.Trim(), this.validDate7.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName8.Value.Trim()))
                        {
                            //新增企业年报
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 17, this.HidFfileName8.Value.Trim(), this.validDate8.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName9.Value.Trim()))
                        {
                            //新增银行收付款帐号资料
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 18, this.HidFfileName9.Value.Trim(), this.validDate9.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                        if (!string.IsNullOrWhiteSpace(this.HidFfileName10.Value.Trim()))
                        {
                            //新增购销合同
                            Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(fid), 19, this.HidFfileName10.Value.Trim(), this.validDate10.Value.Trim());
                            count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                        }
                    }
                    catch (Exception)
                    {
                        count = 0;
                    }
                    if (count > 0)
                    {
                        Tran.Commit();
                        Response.Redirect("FCmaterialsInfo.aspx?id=" + fid, true);
                    }
                    else
                    {
                        throw new Exception("新增异常！");
                    }
                }
                else
                {
                    throw new Exception("新增异常！");
                }

            }
            else {
                //修改
                fCmaterialsModel = new Hi.BLL.YZT_FCmaterials().GetModel(Convert.ToInt32(id));
                fCmaterialsModel= getfCmaterialsModel(fCmaterialsModel);
                if (new Hi.BLL.YZT_FCmaterials().Update(fCmaterialsModel,Tran))
                {
                    DateTime time = DateTime.Now;
                    List<Hi.Model.YZT_Annex> annexList = new Hi.BLL.YZT_Annex().GetList("", " fcID="+id+" and dr=0", "");
                    foreach (Hi.Model.YZT_Annex item in annexList)
                    {
                        item.ts = time;
                        item.modifyuser = UserID;
                        if (item.type == 5)
                        {
                            //营业执照修改
                            HidFfileNameOld = item.fileName;
                            item.fileName = HidFfileNameNew;
                            if(!string.IsNullOrWhiteSpace(this.validDate.Value.Trim()))
                            item.validDate = Convert.ToDateTime(this.validDate.Value.Trim());
                        }
                        else if (item.type == 6)
                        {
                            //生产许可证修改
                            HidFfileNameOld2 = item.fileName;
                            item.fileName = HidFfileNameNew2;
                            if (!string.IsNullOrWhiteSpace(this.validDate2.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate2.Value.Trim());
                        }
                        else if (item.type == 12)
                        {
                            //税务登记证修改
                            HidFfileNameOld3 = item.fileName;
                            item.fileName = HidFfileNameNew3;
                            if (!string.IsNullOrWhiteSpace(this.validDate3.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate3.Value.Trim());
                        }
                        else if (item.type == 13)
                        {
                            //开户银行许可证修改
                            HidFfileNameOld4 = item.fileName;
                            item.fileName = HidFfileNameNew4;
                            if (!string.IsNullOrWhiteSpace(this.validDate4.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate4.Value.Trim());
                        }
                        else if (item.type == 14)
                        {
                            //质量管理体系调查表修改
                            HidFfileNameOld5 = item.fileName;
                            item.fileName = HidFfileNameNew5;
                            if (!string.IsNullOrWhiteSpace(this.validDate5.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate5.Value.Trim());
                        }
                        else if (item.type == 15)
                        {
                            //GSP/GMP证书修改
                            HidFfileNameOld6 = item.fileName;
                            item.fileName = HidFfileNameNew6;
                            if (!string.IsNullOrWhiteSpace(this.validDate6.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate6.Value.Trim());
                        }
                        else if (item.type == 16)
                        {
                            //开票信息修改
                            HidFfileNameOld7 = item.fileName;
                            item.fileName = HidFfileNameNew7;
                            if (!string.IsNullOrWhiteSpace(this.validDate7.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate7.Value.Trim());
                        }
                        else if (item.type ==17)
                        {
                            //企业年报修改
                            HidFfileNameOld8 = item.fileName;
                            item.fileName = HidFfileNameNew8;
                            if (!string.IsNullOrWhiteSpace(this.validDate8.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate8.Value.Trim());
                        }
                        else if (item.type == 18)
                        {
                            //银行收付款帐号资料修改
                            HidFfileNameOld9 = item.fileName;
                            item.fileName = HidFfileNameNew9;
                            if (!string.IsNullOrWhiteSpace(this.validDate9.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate9.Value.Trim());
                        }
                        else if (item.type == 19)
                        {
                            //购销合同修改
                            HidFfileNameOld10 = item.fileName;
                            item.fileName = HidFfileNameNew10;
                            if (!string.IsNullOrWhiteSpace(this.validDate10.Value.Trim()))
                                item.validDate = Convert.ToDateTime(this.validDate10.Value.Trim());
                        }

                        if (!new Hi.BLL.YZT_Annex().Update(item,Tran)) {
                            break;
                            throw new Exception("修改异常！");
                        }
                    }


                    List<Hi.Model.YZT_Annex> annex1 = annexList.Where(a => a.type == 5).ToList();
                    if (annex1.Count <= 0&&!string.IsNullOrWhiteSpace(this.HidFfileName.Value.Trim()))
                    {
                        //新增营业执照附件表
                        Hi.Model.YZT_Annex annexModel = insertAnnex(Convert.ToInt32(id), 5, this.HidFfileName.Value.Trim(), this.validDate.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex2 = annexList.Where(a => a.type == 6).ToList();
                    if (annex2.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName2.Value.Trim()))
                    {
                        //新增生产许可证表
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 6, this.HidFfileName2.Value.Trim(), this.validDate2.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex3 = annexList.Where(a => a.type == 12).ToList();
                    if (annex3.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName3.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 12, this.HidFfileName3.Value.Trim(), this.validDate3.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex4 = annexList.Where(a => a.type == 13).ToList();
                    if (annex4.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName4.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 13, this.HidFfileName4.Value.Trim(), this.validDate4.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex5 = annexList.Where(a => a.type == 14).ToList();
                    if (annex5.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName5.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 14, this.HidFfileName5.Value.Trim(), this.validDate5.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex6 = annexList.Where(a => a.type == 15).ToList();
                    if (annex6.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName6.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 15, this.HidFfileName6.Value.Trim(), this.validDate6.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex7 = annexList.Where(a => a.type == 16).ToList();
                    if (annex7.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName7.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 16, this.HidFfileName7.Value.Trim(), this.validDate7.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex8 = annexList.Where(a => a.type == 17).ToList();
                    if (annex8.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName8.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 17, this.HidFfileName8.Value.Trim(), this.validDate8.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex9 = annexList.Where(a => a.type == 18).ToList();
                    if (annex9.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName9.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 18, this.HidFfileName9.Value.Trim(), this.validDate9.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    List<Hi.Model.YZT_Annex> annex10= annexList.Where(a => a.type == 19).ToList();
                    if (annex10.Count <= 0 && !string.IsNullOrWhiteSpace(this.HidFfileName10.Value.Trim()))
                    {
                        Hi.Model.YZT_Annex annexModel2 = insertAnnex(Convert.ToInt32(id), 19, this.HidFfileName10.Value.Trim(), this.validDate10.Value.Trim());
                        int count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);
                    }

                    //删除旧的营业执照附件
                    if (HidFfileNameOld != HidFfileNameNew) {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    //删除旧的生产许可证 附加
                    if (HidFfileNameOld2 != HidFfileNameNew2)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld2);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (HidFfileNameOld3 != HidFfileNameNew3)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld3);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (HidFfileNameOld4 != HidFfileNameNew4)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld4);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (HidFfileNameOld5 != HidFfileNameNew5)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld5);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (HidFfileNameOld6 != HidFfileNameNew6)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld6);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (HidFfileNameOld7 != HidFfileNameNew7)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld7);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    if (HidFfileNameOld8 != HidFfileNameNew8)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld8);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    } 
                    if (HidFfileNameOld9 != HidFfileNameNew9)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld9);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    } 
                    if (HidFfileNameOld10 != HidFfileNameNew10)
                    {
                        FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../../UploadFile/") + HidFfileNameOld10);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }

                    Tran.Commit();
                    Response.Redirect("FCmaterialsInfo.aspx?id=" + id,true);
                }
                else {
                    throw new Exception("修改异常！");
                }

            }


        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "function (){ window.location.href=window.location.href; }");
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }

    public void Download_Click(object sender, EventArgs e)
    {
        LinkButton bt = sender as LinkButton;
        string fileName = bt.Attributes["fileName"];
        string filePath = Server.MapPath("../../UploadFile/") + fileName;
        if (File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name.Substring(0, file.Name.LastIndexOf("_")) + Path.GetExtension(file.Name))); //解决中文文件名乱码    
            Response.AddHeader("Content-length", file.Length.ToString());
            Response.ContentType = "appliction/octet-stream";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        else
        {
            JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.错误);
        }
    }


    public Hi.Model.YZT_Annex insertAnnex(int id, int type, string name, string date)
    {
        DateTime time = DateTime.Now;
        Hi.Model.YZT_Annex annexModel = new Hi.Model.YZT_Annex();
        annexModel.fcID = id;
        annexModel.type = type;
        annexModel.fileName = name;
        annexModel.fileAlias = "4";
        annexModel.validDate = Convert.ToDateTime(date);
        annexModel.CreateDate = time;
        annexModel.dr = 0;
        annexModel.ts = time;
        annexModel.modifyuser = UserID;
        annexModel.CreateUserID = UserID;
        return annexModel;
    }


}