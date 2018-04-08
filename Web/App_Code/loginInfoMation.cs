using System;
using System.Web;
using Hi.Model;
using System.Web.SessionState;
public class loginInfoMation : IReadOnlySessionState
{
    public static int UserID { get; set; }
    public static string UserName { get; set; }
    public static int DisID { get; set; }
    public static int CompID { get; set; }
    public static int TypeId { get; set; }
    public static int Ctype { get; set; }
    public static LoginModel UserModel { get; set; }
    public static string CompName { get; set; }
    public  loginInfoMation()
    {

    }
    public  void LoadData()
    {
        if (HttpContext.Current.Session["UserModel"] is LoginModel)
        {
            UserModel = HttpContext.Current.Session["UserModel"] as LoginModel;
            if (UserModel != null)
            {
                UserID = UserModel.UserID;
                UserName = UserModel.UserName;
                DisID = UserModel.DisID;
                CompID = UserModel.CompID;
                TypeId = UserModel.TypeID;
                Ctype = UserModel.Ctype;
                if (UserModel.Ctype==2)
                {
                    CompName = UserModel.DisName;
                }
                else
                {
                    CompName = UserModel.CompName;
                }
            }
            else
            {
                UserModel = null;
                UserID = 0;
                UserName = null;
                DisID = 0;
                CompID = 0;
                TypeId = 0;
                Ctype = 0;
                CompName = "";
            }
        }
        else
        {
            UserModel = null;
            UserID = 0;
            UserName = null;
            DisID = 0;
            CompID = 0;
            TypeId = 0;
            Ctype = 0;
            CompName = "";
        }
    }
}