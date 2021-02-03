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
using System.Drawing;
using System.Data;
using System.Web.Security;
using LinqToDB;
using System.Text.RegularExpressions;

namespace AS_Assignment_1
{
    public partial class Update : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string nfinalHash;
        static string nsalt;
        byte[] Key;
        byte[] IV;
        string userID = null;
        string count = null;
        int active;
        DateTime time_stamp;
        string r_hash;
        string r_salt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Home.aspx", false);
                }
                else
                {
                    userID = (string)Session["LoggedIn"];

                    displayUserProfile(userID);

                    CheckOldPwd(HttpUtility.HtmlEncode(lbl_email.Text.Trim()));

                    if (passwordMax() == true)
                    {
                        string message = "Password Expired . Please change.";
                        DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);

                    }

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
                Response.Redirect("Home.aspx", false);
            }
        }

        protected void btn_change_Click(object sender, EventArgs e)
        {
            
            checkPassword();
            userID = (string)Session["LoggedIn"];
            string pwd = HttpUtility.HtmlEncode(tb_current_pwd.Text.ToString().Trim());
            string new_pwd = HttpUtility.HtmlEncode(tb_new_pwd.Text.ToString().Trim());
            string dbHash = GetDBHash(userID);
            string dbSalt = GetDBSalt(userID);
            SHA512Managed hashing = new SHA512Managed();
            if (ScoringSystem() == "success")
            {
                if (checkPassword() == "success")
                {
                    try
                    {
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = pwd + dbSalt;

                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                            string userHash = Convert.ToBase64String(hashWithSalt);

                            if (userHash.Equals(dbHash))
                            {

                                //Generate random "salt"
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] saltByte = new byte[8];

                                //Fills array of bytes with a cryptographically strong sequence of random values.
                                rng.GetBytes(saltByte);
                                nsalt = Convert.ToBase64String(saltByte);

                                string new_pwdWithSalt = new_pwd + nsalt;
                                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(new_pwd));
                                byte[] new_hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(new_pwdWithSalt));
                                nfinalHash = Convert.ToBase64String(new_hashWithSalt);

                                if (count == "valid")
                                {
                                    if (passwordMin() == true)
                                    {
                                        if (nfinalHash != r_hash)
                                        {
                                            updateAccountPassword();
                                            updatePasswordHistory();
                                        }
                                        else
                                        {
                                            string message = "Reused Password.";
                                            DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);

                                        }
                                    }
                                    else
                                    {
                                        string message = "Minimum Password Age Violated.";
                                        DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);
                                        Response.Redirect("Home.aspx", false);
                                    }

                                }
                                if (count == null)
                                {
                                    updateAccountPassword();
                                    updatePasswordHistory();
                                }

                            }
                            else
                            {
                                lblerrorMsg.Text = "Incorrect password";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { }

                }
            }
            else
            {
                ScoringSystem();
            }
       
        }

      
        public void updateAccountPassword()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("update Account Set PasswordHash=@new_hash, PasswordSalt=@new_salt where Email = @userid"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@userid", HttpUtility.HtmlEncode(lbl_email.Text.Trim()));
                            cmd.Parameters.AddWithValue("@new_hash", nfinalHash);
                            cmd.Parameters.AddWithValue("@new_salt", nsalt);

                            cmd.Connection = con;

                            string message = "Password Updated Successfully";
                            DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);
                            Response.Redirect("Home.aspx", false);

                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());

                            }
                            finally
                            {
                                con.Close();
                                
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        public void updatePasswordHistory()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO OldPwd VALUES(@Email, @PasswordHash, @PasswordSalt, @Active, @Timestamp)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(lbl_email.Text.Trim()));
                            cmd.Parameters.AddWithValue("@PasswordHash", nfinalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", nsalt);
                            cmd.Parameters.AddWithValue("@Active", active+1);
                            cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                            cmd.Connection = con;

                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());

                            }
                            finally
                            {
                                con.Close();

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        public void CheckOldPwd(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM OldPwd WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] == DBNull.Value)
                        {
                            break;
                            
                        }
                        else
                        {
                            count = "valid";

                            if (reader["Active"] != DBNull.Value)
                            {
                                active = (int)reader["Active"];
                            }
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                r_hash = reader["PasswordHash"].ToString();
                            }
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                r_salt = reader["PasswordSalt"].ToString();
                            }
                            if (reader["Timestamp"] != DBNull.Value)
                            {
                                time_stamp = (DateTime)reader["Timestamp"];
                            }
                           
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public bool passwordMin()
        {
            DateTime pwd_update_time = time_stamp.AddMinutes(5);
            DateTime currentTime = DateTime.Now;

            if (count == null)
            {
                return false;
            }
            else
            {
                if (pwd_update_time >= currentTime)
                {
                    return false;
                }
                if (pwd_update_time <= currentTime)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            
        }

        public bool passwordMax()
        {
            DateTime pwd_update_time = time_stamp.AddMinutes(15);
            DateTime currentTime = DateTime.Now;

            if (count == null)
            {
                return false;
            }
            else
            {
                if (pwd_update_time >= currentTime)
                {
                    return false;
                }
                if (pwd_update_time <= currentTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool passwordHistory()
        {
            if (count == null)
            {
                return false;
            }
            else
            {
                if (active == 1 | active == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public string checkPassword()
        {
            string cur_pwd = HttpUtility.HtmlEncode(tb_current_pwd.Text);
            string pwd = HttpUtility.HtmlEncode(tb_new_pwd.Text);
            string pwd_cfm = HttpUtility.HtmlEncode(tb_cfm_pwd.Text);

            if (pwd == null && pwd_cfm == null)
            {
                lblerrorMsg0.Text = "Input cannot be empty";
                lblerrorMsg0.ForeColor = Color.Red;
                return "fail";
            }
            if (pwd != pwd_cfm)
            {
                lblerrorMsg0.Text = "Password do not match";
                lblerrorMsg0.ForeColor = Color.Red;
                return "fail";
            }
            if (cur_pwd == pwd_cfm)
            {
                lblerrorMsg0.Text = "Cannot change to same password";
                lblerrorMsg0.ForeColor = Color.Red;
                return "fail";
            }
            else
            {
                return "success";
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
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["Email"].ToString();
                        }
                        //if (reader["Card"] != DBNull.Value)
                        //{
                            //convert base64 in db to byte[]
                            //card = Convert.FromBase64String(reader["Card"].ToString());
                        //}
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }

                    }
                    //lbl_card.Text = decryptData(card);
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


        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                score++;
            }

            return score;
        }

        public string ScoringSystem()
        {
            int scores = checkPassword(HttpUtility.HtmlEncode(tb_new_pwd.Text));
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lblerrorMsg1.Text = "Status : " + status;
            if (scores < 4)
            {
                lblerrorMsg1.ForeColor = Color.Red;
                return "fail";
            }
            lblerrorMsg1.ForeColor = Color.Green;
            return "success";
        }

    }

}