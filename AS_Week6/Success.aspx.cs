using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AS_Week6
{
    public partial class Success : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] nric = null;
        string JS_userEMAIL = null;
        static string finalHash;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["JS_userEMAIL"] != null && Session["JS_AuthToken"] != null && Request.Cookies["JS_AuthToken"] != null)
            {
                if (!Session["JS_AuthToken"].ToString().Equals(Request.Cookies["JS_AuthToken"].Value))
                {
                    Response.Redirect("login.aspx", false);
                }
                else
                {
                    lbl_display.Text = HttpUtility.HtmlEncode(Request.QueryString["Comment"]);
                    JS_tbEmail.Text = (string)Session["JS_userEMAIL"];
                    JS_userEMAIL = (string)Session["JS_userEMAIL"];

                    displayUserProfile(JS_userEMAIL);
                }
            }
            else
            {
                Response.Redirect("login.aspx", false);
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

                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
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

        protected void displayUserProfile(string JS_useremail)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT * FROM JSDatabase WHERE EmailAddress=@JS_userEmail";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@JS_userEmail", JS_useremail);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["EmailAddress"] != DBNull.Value)
                        {
                            lbl_userID.Text = reader["EmailAddress"].ToString();
                        }

                        if (reader["CreditCardInfo"] != DBNull.Value)
                        {
                            //Convert base64 in db to byte []
                            nric = Convert.FromBase64String(reader["CreditCardInfo"].ToString());
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
                    lbl_nric.Text = decryptData(nric);
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

        protected void JS_btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("login.aspx", false);

            if(Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["JS_AuthToken"] != null)
            {
                Response.Cookies["JS_AuthToken"].Value = string.Empty;
                Response.Cookies["JS_AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

        }



        protected string getDBHash(string JS_useremail)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM JSDatabase WHERE EmailAddress=@JS_USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@JS_USEREMAIL", JS_useremail);
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


        protected void JS_CheckPassword_Click(object sender, EventArgs e)
        {
            string pwd = JS_tbCurrentPassword.Text.ToString().Trim();
            string JS_useremail = JS_tbEmail.Text.ToString().Trim();
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(JS_useremail);
            string dbSalt = getDBSalt(JS_useremail);
            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        JS_lbCheckCurrentPassword.Text = "Current Password is correct";
                    }
                    else
                    {
                        JS_lbCheckCurrentPassword.Text = "Current Password is wrong";
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }

        }




        protected string getDBSalt(string JS_useremail)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM JSDatabase WHERE EmailAddress=@JS_USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@JS_USEREMAIL", JS_useremail);
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



        protected void JS_btnUpdatePassword_Click(object sender, EventArgs e)
        {
            
            String JS_userEMAIL = JS_tbEmail.Text.ToString().Trim();
            String pwd = JS_tbNewPassword.Text.ToString().Trim();
            String dbsalt = getDBSalt(JS_userEMAIL);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + dbsalt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            JS_passwordupdated();
        }

        public void JS_passwordupdated()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE JSDATABASE SET PasswordHash=@ParaPwdHash WHERE EmailAddress=@JS_USEREMAIL"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@paraPwdHash", finalHash);
                            cmd.Parameters.AddWithValue("@JS_USEREMAIL", JS_tbEmail.Text.ToString());

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
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
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

        
    }
}