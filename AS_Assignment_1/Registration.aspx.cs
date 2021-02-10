using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace AS_Assignment_1
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string Password = HttpUtility.HtmlEncode(tb_pwd.Text);
                tb_pwd.Attributes.Add("value", Password);
            }
        }

        protected void btn_reg_Click(object sender, EventArgs e)
        {
           if(checkinput() == "success") { 

                string checkEmail = CheckEmail(HttpUtility.HtmlEncode(tb_email.Text));

                if (checkEmail == "passed")
                {
                    if (ValidateEmail() == true && ValidateCard() == true)
                    {
                        string pwd_result = ScoringSystem();
                        if (pwd_result == "success")
                        {

                            string message = "Registration Successful";
                            DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);
                            if (result == DialogResult.OK)
                            {
                                string pwd = HttpUtility.HtmlEncode(tb_pwd.Text.ToString().Trim());

                                //Generate random "salt"
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] saltByte = new byte[8];

                                //Fills array of bytes with a cryptographically strong sequence of random values.
                                rng.GetBytes(saltByte);
                                salt = Convert.ToBase64String(saltByte);

                                SHA512Managed hashing = new SHA512Managed();

                                string pwdWithSalt = pwd + salt;
                                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                                finalHash = Convert.ToBase64String(hashWithSalt);
                                RijndaelManaged cipher = new RijndaelManaged();
                                cipher.GenerateKey();
                                Key = cipher.Key;
                                IV = cipher.IV;

                                createAccount();
                                Response.Redirect(HttpUtility.UrlEncode("Login.aspx"));
                            }
                        }
                        else
                        {
                            ScoringSystem();
                        }


                    }
                    else
                    {
                        ValidateEmail();
                        ValidateCard();
                    }

                }
                else
                {
                    string message = "Registration Unsuccessful please try again";
                    DialogResult result = (DialogResult)System.Windows.MessageBox.Show(message);

                }
            }
            else
            {
                checkinput();
            }

        }

        public string checkinput()
        {
            string f_name = HttpUtility.HtmlEncode(tb_f_name.Text);
            string l_name = HttpUtility.HtmlEncode(tb_l_name.Text);
            string dob = HttpUtility.HtmlEncode(tb_dob.Text);

            if(f_name == "" && l_name == "" && dob == "")
            {
                lbl_inputchecker.Text = "Input cannot be empty";
                lbl_inputchecker.ForeColor = Color.Red;
                return "fail";
            }
            else
            {
                lbl_inputchecker.Text = "";
                return "success";
            }
        }

        public bool ValidateEmail()
        {
            string email = HttpUtility.HtmlEncode(tb_email.Text);
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",RegexOptions.CultureInvariant | RegexOptions.Singleline);
           
            bool isValidEmail = regex.IsMatch(email);

            if (!isValidEmail)
            {
                lbl_emailchecker.Text = "Invalid Email";
                lbl_emailchecker.ForeColor = Color.Red;
            }
            else
            {
                lbl_emailchecker.Text = "";
                return true;
            }

            return false;
            
        }

        public bool ValidateCard()
        {
            string cardNo = HttpUtility.HtmlEncode(tb_card.Text);

            Regex regex = new Regex(@"^(3762|4119|4265|4524|5240|5400)([\-\s]?[0-9]{4}){3}$");
            bool isValidCard = regex.IsMatch(cardNo);

            if (!isValidCard)
            { // <1>check card number is valid
                lbl_cardchecker.Text = "Invalid Card";
                lbl_cardchecker.ForeColor = Color.Red;
            }
            else
            {
                lbl_cardchecker.Text = "";
                return true;
            }

            return false;


        }
        public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@F_name, @L_name, @Email, @Card, @Dob, @PasswordHash, @PasswordSalt, @EmailVerified, @CardVerified, @IV, @Key)"))
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                 
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@F_name", HttpUtility.HtmlEncode(tb_f_name.Text.Trim()));
                            cmd.Parameters.AddWithValue("@L_name", HttpUtility.HtmlEncode(tb_l_name.Text.Trim())); 
                            cmd.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(tb_email.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Card", Convert.ToBase64String(encryptData(HttpUtility.HtmlEncode(tb_card.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@Dob", HttpUtility.HtmlEncode(tb_dob.Text));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@CardVerified", DBNull.Value);                           
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));

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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
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
            int scores = checkPassword(HttpUtility.HtmlEncode(tb_pwd.Text));
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
            lbl_pwdchecker2.Text = "Status : " + status;
            if (scores < 4)
            {
                lbl_pwdchecker2.ForeColor = Color.Red;
                return "fail";
            }
            lbl_pwdchecker2.ForeColor = Color.Green;
            return "success";
        }

        public string CheckEmail( string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            connection.Open();
            SqlCommand check_User_Name = new SqlCommand("SELECT COUNT(*) FROM [Account] WHERE ([Email] = @Email)" , connection);
            check_User_Name.Parameters.AddWithValue("@Email", email);
            int UserExist = (int)check_User_Name.ExecuteScalar();

            if (UserExist > 0)
            {
                //Username exist
                return "failed";
            }
            else
            {
                //Username doesn't exist.
                return "passed";
            }
            connection.Close();
        }

    }
}
