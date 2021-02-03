using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace AS_Assignment_1
{
    public partial class Home : System.Web.UI.Page
    {
        string MYDBConnectionString =System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] card = null;
        string userID = null;
 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lblMessage.Text = " Congratulations !, you are logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    userID = (string)Session["LoggedIn"];
                    displayUserProfile(userID);

                    System.Web.UI.HtmlControls.HtmlGenericControl login =
                    (System.Web.UI.HtmlControls.HtmlGenericControl)Master?.FindControl("login");
                    if (login != null) login.Visible = false;

                    System.Web.UI.HtmlControls.HtmlGenericControl reg =
                   (System.Web.UI.HtmlControls.HtmlGenericControl)Master?.FindControl("reg");
                    if (reg != null) reg.Visible = false;

                  
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;

        }

        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM Account WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(reader["Email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["Email"].ToString();
                        }
                        if (reader["Card"] != DBNull.Value)
                        {
                            //convert base64 in db to byte[]
                            card = Convert.FromBase64String(reader["Card"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }

                    }
                    lbl_card.Text = decryptData(card);
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Logout.aspx", false);
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

    }
}
               
