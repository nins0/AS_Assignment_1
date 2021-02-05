using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Windows.Forms;

namespace AS_Assignment_1
{ 
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string Email = HttpUtility.HtmlEncode(tb_login_email.Text);
                tb_login_email.Attributes.Add("value", Email);
            }


        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
           
            if (ValidateCaptcha())
            {   
                string pwd = HttpUtility.HtmlEncode(tb_login_pwd.Text.ToString().Trim());
                string userid = HttpUtility.HtmlEncode(tb_login_email.Text.ToString().Trim());
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = GetDBHash(userid);
                string dbSalt = GetDBSalt(userid);
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (userHash.Equals(dbHash))
                        {
                            // account lock out
                            Session["LoginCount"] = 0;

                            Session["LoggedIn"] = userid;

                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;

                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                            Response.Redirect("Home.aspx",false);
                            
                        }
                        else
                        {
                            
                            Session["LoginCount"] = Convert.ToInt32(Session["LoginCount"]) + 1;

                            if (Convert.ToInt32(Session["LoginCount"]) > 3)
                            {
                                string message = "Your account is locked. Exceeded login attempts.";
                                DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);
                                if (result == DialogResult.OK)
                                {
                                    Response.Redirect("Lockout.aspx",false);
                                }
                                Response.Redirect("Lockout.aspx",false);


                            }
                            else
                            {
                                lblerrorMsg.Text = "Email or password is not valid. Please try again.";
                            }
                            //Response.Redirect("Login.aspx", false);
                        }
                    }
                    else
                    {
                        lblerrorMsg.Text = "Email or password is not valid. Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally { }

            }
            else
            {
                
            }
        }

        protected string GetDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string GetDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        public class My_Object
        {
            public string success { get; set; }
            public List<String> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET reponse to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                (" &response=" + captchaResponse);
            

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        // The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        My_Object jsonObject = js.Deserialize<My_Object>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

    }

}
