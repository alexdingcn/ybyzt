using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using LitJson;
using System.Data.SqlClient;
using DBUtility;
/// <summary>
///CheckCode 的摘要说明
/// </summary>
public class CheckCode
{
	public CheckCode()
	{
	}

    #region 验证码长度(默认4个验证码的长度)
    int length = 4;
    public int Length
    {
        get { return length; }
        set { length = value; }
    }
    #endregion

    #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
    int fontSize = 12;
    public int FontSize
    {
        get { return fontSize; }
        set { fontSize = value; }
    }
    #endregion

    #region 边框补(默认1像素)
    int padding = 1;
    public int Padding
    {
        get { return padding; }
        set { padding = value; }
    }
    #endregion

    #region 是否输出燥点(默认不输出)
    bool chaos = true;
    public bool Chaos
    {
        get { return chaos; }
        set { chaos = value; }
    }
    #endregion

    #region 输出燥点的颜色(默认灰色)
    Color chaosColor = Color.LightGray;
    public Color ChaosColor
    {
        get { return chaosColor; }
        set { chaosColor = value; }
    }
    #endregion

    #region 自定义背景色(默认白色)
    Color backgroundColor = Color.White;
    public Color BackgroundColor
    {
        get { return backgroundColor; }
        set { backgroundColor = value; }
    }
    #endregion

    #region 自定义随机颜色数组
    //Color[] colors = { Color.Black };
    Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
    public Color[] Colors
    {
        get { return colors; }
        set { colors = value; }
    }
    #endregion

    #region 自定义字体数组

    public static string[] GetFont()
    {

        List<string> list = new List<string>();
        InstalledFontCollection insFont = new InstalledFontCollection();
        FontFamily[] families = insFont.Families;
        foreach (FontFamily family in families)
        {
            list.Add(family.Name);

        }
        return list.ToArray();
    }
    string[] fonts = { "宋体", "Arial", "Georgia", "" };
    //string[] fonts = GetFont();
    public string[] Fonts
    {
        get { return fonts; }
        set { fonts = value; }
    }
    #endregion

    #region 自定义随机码字符串序列(使用逗号分隔)
    //string codeSerial = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,";//A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
    string codeSerial = "1,2,3,4,5,6,7,8,9,0";//A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
    public string CodeSerial
    {
        get { return codeSerial; }
        set { codeSerial = value; }
    }
    #endregion

    #region 产生波形滤镜效果

    private const double PI = 3.1415926535897932384626433832795;
    private const double PI2 = 6.283185307179586476925286766559;

    /// <summary>
    /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
    /// </summary>
    /// <param name="srcBmp">图片路径</param>
    /// <param name="bXDir">如果扭曲则选择为True</param>
    /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
    /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
    /// <returns></returns>
    public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
    {
        System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

        // 将位图背景填充为白色
        System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
        graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
        graph.Dispose();

        double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

        for (int i = 0; i < destBmp.Width; i++)
        {
            for (int j = 0; j < destBmp.Height; j++)
            {
                double dx = 0;
                dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                dx += dPhase;
                double dy = Math.Sin(dx);

                // 取得当前点的颜色
                int nOldX = 0, nOldY = 0;
                nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                System.Drawing.Color color = srcBmp.GetPixel(i, j);
                if (nOldX >= 0 && nOldX < destBmp.Width
                 && nOldY >= 0 && nOldY < destBmp.Height)
                {
                    destBmp.SetPixel(nOldX, nOldY, color);
                }
            }
        }

        return destBmp;
    }
    #endregion

    #region 生成校验码图片
    public Bitmap CreateImageCode(string code)
    {
        int fSize = FontSize;
        int fWidth = fSize + Padding;

        int imageWidth = (int)(code.Length * fWidth) + Padding * 2;
        int imageHeight = fSize * 2 + Padding;

        System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

        Graphics g = Graphics.FromImage(image);

        g.Clear(BackgroundColor);

        Random rand = new Random();

        //给背景添加随机生成的燥点
        if (this.Chaos)
        {

            Pen pen = new Pen(ChaosColor, 0);
            int c = Length * 10;

            for (int i = 0; i < c; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);

                g.DrawRectangle(pen, x, y, 1, 1);
            }
        }

        int left = 0, top = 0, top1 = 1, top2 = 1;

        int n1 = (imageHeight - FontSize - Padding * 2);
        int n2 = n1 / 4;
        top1 = n2;
        top2 = n2 * 2;

        Font f;
        Brush b;

        int cindex, findex;

        //随机字体和颜色的验证码字符
        for (int i = 0; i < code.Length; i++)
        {
            cindex = rand.Next(Colors.Length - 1);
            findex = rand.Next(Fonts.Length - 1);

            f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
            b = new System.Drawing.SolidBrush(Colors[cindex]);

            if (i % 2 == 1)
            {
                top = top2;
            }
            else
            {
                top = top1;
            }

            left = i * fWidth;

            g.DrawString(code.Substring(i, 1), f, b, left, top);
        }

        //画一个边框 边框颜色为Color.Gainsboro
        g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
        g.Dispose();

        //产生波形
        //image = TwistImage(image, false, 3, 2);
        image = TwistImage(image, false, 0, 0);
        return image;
    }
    #endregion

    #region 生成随机字符码
    public string CreateVerifyCode(int codeLen)
    {
        if (codeLen == 0)
        {
            codeLen = Length;
        }

        string[] arr = CodeSerial.Split(',');

        string code = "";

        int randValue = -1;

        Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

        for (int i = 0; i < codeLen; i++)
        {
            randValue = rand.Next(0, arr.Length - 1);

            code += arr[randValue];
        }
        //Session["CheckCode"] = code.ToLower();
        return code;
    }
    public string CreateVerifyCode()
    {
        return CreateVerifyCode(0);
    }
    #endregion

    //手机获得验证图片
    #region
    public ResultCaptchaPhoto GetCaptchaPhoto()
    {
        try
        {
            //随机生成字符串，作为验证图片中的字符串
            string code = CreateVerifyCode();
            //生成验证图片
            Bitmap photo = CreateImageCode(code);
            byte[] b_photo = null;
            //将验证图片转成流（byte数组）
            MemoryStream stream = new MemoryStream();
            using (photo)
            {
                photo.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            photo.Dispose();
            using (stream)
            {
               b_photo = stream.GetBuffer();
            }
            //stream.Dispose();
            stream.Close();
            //将验证图片的流，转化成字符串输出
            string PhotoUrl = Convert.ToBase64String(b_photo, 0, b_photo.Length);
            //返回参数
            ResultCaptchaPhoto resultphoto = new ResultCaptchaPhoto();
            resultphoto.Result = "T";
            resultphoto.Description = "返回成功";
            resultphoto.PhotoString = code;
            resultphoto.PhotoUrl = PhotoUrl;
            return resultphoto;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCaptchaPhoto" );
            return new ResultCaptchaPhoto() { Result = "F", Description = "参数异常" };
        } 
    }
    #endregion

    //public string GetPhoto()
    //{
    //    try
    //    {
    //        string code = CreateVerifyCode();
    //        Bitmap photo = CreateImageCode(code);
    //        byte[] b_photo = null;
    //        MemoryStream stream = new MemoryStream();
    //        using (photo)
    //        {
    //            photo.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
    //        }
    //        photo.Dispose();
    //        using (stream)
    //        {
    //            b_photo = stream.GetBuffer();
    //        }
    //        //stream.Dispose();
    //        stream.Close();
    //        string PhotoUrl = Convert.ToBase64String(b_photo, 0, b_photo.Length);
    //        return PhotoUrl;

    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.ToString();
    //    }
    //}

    //public void write()
    //{
    //    string PhotoUrl = GetPhoto();
    //    byte[] b = Convert.FromBase64String(PhotoUrl);
    //    FileStream fs = new FileStream(@"F:/QQ文件/IMG_14.JPG",FileMode.Create,FileAccess.Write);
    //    fs.Write(b,0,b.Length);
    //    fs.Flush();
    //    fs.Close();
    //}


    //核心企业提交入驻申请
    #region
    public ResultCompEnter SendEnterRequest(string JSon, string version)
    {
        string PhoneNumb = string.Empty;
        string LoginName = string.Empty;
        string PassWord = string.Empty;
        string CompanyName = string.Empty;
        string Captcha = string.Empty;
        string SendId = string.Empty;
        string Type = string.Empty;
        int compid = 0;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["PhoneNumb"].ToString().Trim() != "" && JInfo["LoginName"].ToString().Trim() != "" && JInfo["Captcha"].ToString().Trim() != "" &&
                JInfo["PassWord"].ToString().Trim() != "" && JInfo["CompanyName"].ToString().Trim() != "" && JInfo["SendId"].ToString().Trim() != "" &&
                JInfo["Type"].ToString().Trim() != "")
            {
                PhoneNumb = Common.NoHTML(JInfo["PhoneNumb"].ToString());
                LoginName = Common.NoHTML(JInfo["LoginName"].ToString());
                if (LoginName != JInfo["LoginName"].ToString())
                    return new ResultCompEnter() { Result = "F", Description = "用户名存在非法字符串" };
                PassWord = JInfo["PassWord"].ToString();
                CompanyName = Common.NoHTML(JInfo["CompanyName"].ToString());
                Captcha = JInfo["Captcha"].ToString();
                SendId = JInfo["SendId"].ToString();
                Type = JInfo["Type"].ToString();
            }
            else
            {
                return new ResultCompEnter() { Result ="F",Description = "参数异常"};
            }
            #endregion
            #region//验证验证码是否有效
            Hi.Model.SYS_PhoneCode code = new Hi.BLL.SYS_PhoneCode().GetModel(int.Parse(SendId));
            if (code != null && code.dr == 0)
            {
                if (code.ts.AddMinutes(30) < DateTime.Now || code.IsPast == 1)
                    return new ResultCompEnter() { Result = "F", Description = "验证码过期" };
               
                if (code.PhoneCode != Captcha)
                    return new ResultCompEnter() { Result = "F", Description = "验证码错误" };
            }
            else
            {
                return new ResultCompEnter() { Result = "F", Description = "验证码不可用" };
            }
            code.IsPast = 1;
            code.ts = DateTime.Now;
            code.modifyuser = 0;
            SqlConnection conn = new SqlConnection(SqlHelper.LocalSqlServer);
            if (conn.State.ToString().ToLower() != "open")
                conn.Open();
            SqlTransaction mytran = conn.BeginTransaction();

            #endregion
            //如果验证码正确的话，修改验证码状态
            try
            {
                if (new Hi.BLL.SYS_PhoneCode().Update(code, mytran))//验证码状态修改成功的话，开始进行注册流程
                {
                    if (Type == "distributor")
                    {
                        Boolean result = RegisterDistributor(CompanyName, PhoneNumb, PassWord, mytran);
                        if (result)
                        {
                            return new ResultCompEnter() { Result = "T", Description = "注册成功" };
                        }
                        else
                        {
                            return new ResultCompEnter() { Result = "F", Description = "注册用户失败" };
                        }
                    }
                    else
                    {
                        //首先在bd_company表中新增一条数据
                        Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
                        comp.CompName = CompanyName;
                        comp.LegalTel = PhoneNumb;
                        comp.Phone = PhoneNumb;
                        comp.AuditState = 0;
                        comp.IsEnabled = 1;
                        comp.FirstShow = 1;
                        comp.Erptype = 0;
                        comp.SortIndex = "001";
                        comp.HotShow = 1;
                        comp.CreateDate = DateTime.Now;
                        comp.CreateUserID = 0;
                        comp.ts = DateTime.Now;
                        comp.modifyuser = 0;
                        compid = new Hi.BLL.BD_Company().Add(comp, mytran);
                        //bd_company表中数据新增成功后,在sys_users表中新增一条数据
                        if (compid <= 0)
                        {
                            mytran.Rollback();
                            conn.Close();
                            return new ResultCompEnter() { Result = "F", Description = "注册核心企业失败" };
                        }
                        //在表sys_users表中新增一条数据
                        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                        user.UserName = LoginName;
                        user.TrueName = "";
                        user.UserPwd = new GetPhoneCode().md5(PassWord);
                        user.Phone = PhoneNumb;
                        user.CreateDate = DateTime.Now;
                        user.CreateUserID = 0;
                        user.ts = DateTime.Now;
                        user.modifyuser = 0;
                        user.AuditState = 2;
                        user.IsEnabled = 1;
                        int userid = new Hi.BLL.SYS_Users().Add(user, mytran);
                        if (userid <= 0)
                        {
                            mytran.Rollback();
                            conn.Close();
                            return new ResultCompEnter() { Result = "F", Description = "注册用户失败" };
                        }

                        //sys_users新增成功的话，在sys_compuser表中新增一条数据
                        Hi.Model.SYS_CompUser compuser = new Hi.Model.SYS_CompUser();
                        compuser.CompID = compid;
                        compuser.DisID = 0;
                        compuser.CreateDate = DateTime.Now;
                        compuser.CreateUserID = 0;
                        compuser.ts = DateTime.Now;
                        compuser.modifyuser = 0;
                        compuser.CType = 1;
                        compuser.UType = 4;
                        compuser.dr = 0;
                        compuser.IsAudit = 0;
                        compuser.IsEnabled = 1;
                        compuser.UserID = userid;
                        int compuserid = new Hi.BLL.SYS_CompUser().Add(compuser, mytran);
                        if (compuserid <= 0)
                        {
                            mytran.Rollback();
                            conn.Close();
                            return new ResultCompEnter() { Result = "F", Description = "用户与核心企业关联失败" };
                        }
                        else
                        {
                            // 通知运营
                            string SendRegiPhone = System.Configuration.ConfigurationManager.AppSettings["SendTels"].ToString();
                            string[] Phones = SendRegiPhone.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string tel in Phones)
                            {
                                GetPhoneCode phoneCode = new GetPhoneCode();
                                phoneCode.GetUser(
                                        System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(),
                                        System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
                                phoneCode.ReturnComp(tel, comp.CompName);
                            }
                        }
                    }
                }
                else
                {
                    mytran.Rollback();
                    conn.Close();
                    return new ResultCompEnter() { Result = "F", Description = "验证码异常" };
                }
            }
            catch
            {
                mytran.Rollback();
                conn.Close();
            }
            mytran.Commit();
            conn.Close();


            return new ResultCompEnter() { Result = "T", Description = "注册成功", CompID = compid.ToString() };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "SendEnterRequest" + JSon);
            return new ResultCompEnter() { Result = "F", Description = "参数异常" };
        }
    }

    private bool RegisterDistributor(string distributorName, string phone, string password, SqlTransaction Tran)
    {
        try
        {
            int Compid = 0;
            int UserID = 0;

            Hi.Model.BD_Distributor Distributor = new Hi.Model.BD_Distributor();
            Distributor.CompID = Compid;
            Distributor.DisName = distributorName;
            Distributor.IsEnabled = 1;
            Distributor.Paypwd = new GetPhoneCode().md5(password);
            Distributor.Phone = phone;
            Distributor.AuditState = 2;
            Distributor.CreateDate = DateTime.Now;
            Distributor.CreateUserID = UserID;
            Distributor.ts = DateTime.Now;
            Distributor.modifyuser = UserID;
            Distributor.IsCheck = 0;
            Distributor.CreditType = 0;

            int DistributorID = 0;

            if ((DistributorID = new Hi.BLL.BD_Distributor().Add(Distributor, Tran)) > 0)
            {
                int Roid = 0;

                //新增角色（企业管理员）
                Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                role.CompID = Compid;
                role.DisID = DistributorID;
                role.RoleName = "企业管理员";
                role.IsEnabled = 1;
                role.SortIndex = "1";
                role.CreateDate = DateTime.Now;
                role.CreateUserID = UserID;
                role.ts = DateTime.Now;
                role.modifyuser = UserID;
                role.dr = 0;
                Roid = new Hi.BLL.SYS_Role().Add(role, Tran);

                //新增角色权限表
                Hi.Model.SYS_RoleSysFun rolesys = null;
                List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("FunCode,FunName", " Type=2", "");
                foreach (Hi.Model.SYS_SysFun sys in funList)
                {
                    rolesys = new Hi.Model.SYS_RoleSysFun();
                    rolesys.CompID = Compid;
                    rolesys.DisID = DistributorID;
                    rolesys.RoleID = Roid;
                    rolesys.FunCode = sys.FunCode;
                    rolesys.FunName = sys.FunName;
                    rolesys.IsEnabled = 1;
                    rolesys.CreateUserID = UserID;
                    rolesys.CreateDate = DateTime.Now;
                    rolesys.ts = DateTime.Now;
                    rolesys.modifyuser = UserID;
                    new Hi.BLL.SYS_RoleSysFun().Add(rolesys, Tran);
                }

                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                user.UserName = phone;
                user.TrueName = phone;
                user.DisID = DistributorID;
                user.TrueName = "";
                user.UserPwd = new GetPhoneCode().md5(password);
                user.IsEnabled = 1;
                user.Phone = phone;
                user.CreateDate = DateTime.Now;
                user.CreateUserID = UserID;
                user.ts = DateTime.Now;
                user.modifyuser = UserID;
                user.AuditState = 2;
                int userid = 0;
                userid = new Hi.BLL.SYS_Users().Add(user, Tran);

                ///用户明细表
                Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                CompUser.CompID = Compid;
                CompUser.DisID = DistributorID;
                CompUser.CreateDate = DateTime.Now;
                CompUser.CreateUserID = UserID;
                CompUser.modifyuser = UserID;
                CompUser.CType = 2;
                CompUser.UType = 5;
                //CompUser.IsEnabled = 1;
                CompUser.IsAudit = 2;
                CompUser.RoleID = Roid;
                CompUser.ts = DateTime.Now;
                CompUser.dr = 0;
                CompUser.UserID = userid;
                CompUser.IsEnabled = 1;
                new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
            }
            else
            {
                return false;
            }
            Tran.Commit();
            return true;
        }
        catch (Exception)
        {
            if (Tran != null && Tran.Connection != null)
            { 
                Tran.Rollback();
            }
            return false;
        }
        finally
        {
            if (Tran != null && Tran.Connection != null)
            {
                Tran.Rollback();
            }
        }

    }
    #endregion

    //核心企业提交入驻申请的返回参数
    public class ResultCompEnter
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String CompID { get; set; }
    }

    //手机获得验证图片的返回参数
    public class ResultCaptchaPhoto
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String PhotoString { get; set; }
        public String PhotoUrl { get; set; }
    }
}