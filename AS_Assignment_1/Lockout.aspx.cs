using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment_1
{
    public partial class Lockout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            System.Web.UI.HtmlControls.HtmlGenericControl login =
                   (System.Web.UI.HtmlControls.HtmlGenericControl)Master?.FindControl("login");
            if (login != null) login.Visible = false;
            System.Web.UI.HtmlControls.HtmlGenericControl reg =
                   (System.Web.UI.HtmlControls.HtmlGenericControl)Master?.FindControl("reg");
            if (reg != null) reg.Visible = false;
            System.Web.UI.HtmlControls.HtmlGenericControl home =
                   (System.Web.UI.HtmlControls.HtmlGenericControl)Master?.FindControl("home");
            if (home != null) home.Visible = false;
        }

        protected void Timer_Tick(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["LoginCount"] != null)
            {
                Response.Cookies["LoginCount"].Value = string.Empty;
                Response.Cookies["LoginCount"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}