using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Configuration;
using System.Text.Json;

namespace AS_Week6
{
    public partial class login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverify?secret=apikey &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

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

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            
            
            tb_userid.Text = HttpUtility.HtmlEncode(tb_userid.Text);
            tb_pwd.Text = HttpUtility.HtmlEncode(tb_pwd.Text);
            

            string pwd = tb_pwd.Text.ToString().Trim();
            string JS_useremail = tb_userid.Text.ToString().Trim();
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
                    //System.Diagnostics.Debug.WriteLine("get number error");
                    int getnumberofcount = Convert.ToInt32(getattemptlogin(JS_useremail));
                    

                    if (ValidateCaptcha())
                    {
                        if (getnumberofcount == 2)
                        {
                            //System.Diagnostics.Debug.WriteLine("acc lock error");
                            if (getaccountlocked(JS_useremail))
                            {
                                lbl_lockoutmsg.Text = "Your account has been locked, due to too many number of attempts";
                            }
                            
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine("update count error");
                                int updatecurrenttime = Updatedatetime(JS_useremail);
                                Updatenoofcount(JS_useremail, 1);
                                if (userHash.Equals(dbHash))
                                {


                                    Session["JS_userEMAIL"] = JS_useremail;

                                    // create a new GUID and save into the session
                                    string JS_guid = Guid.NewGuid().ToString();
                                    Session["JS_AuthToken"] = JS_guid;

                                    //create a new cookie with this guid value
                                    Response.Cookies.Add(new HttpCookie("JS_AuthToken", JS_guid));

                                    Response.Redirect("Success.aspx?Comment=" + HttpUtility.UrlEncode(tb_userid.Text), false);
                                }
                                else if (getnumberofcount < 3)
                                {
                                    int updatenumberofcount = getnumberofcount + 1;
                                    Updatenoofcount(JS_useremail, updatenumberofcount);

                                    errorMsg.Text = "Wrong email address or password is entered. Please try again.";
                                }


                                else
                                {
                                    errorMsg.Text = "Wrong email address or password is entered. Please try again.";
                                }
                            }
                        }
                        else if (userHash.Equals(dbHash))
                        {
                            

                            Session["JS_userEMAIL"] = JS_useremail;

                            // create a new GUID and save into the session
                            string JS_guid = Guid.NewGuid().ToString();
                            Session["JS_AuthToken"] = JS_guid;

                            //create a new cookie with this guid value
                            Response.Cookies.Add(new HttpCookie("JS_AuthToken", JS_guid));

                            Response.Redirect("Success.aspx?Comment=" + HttpUtility.UrlEncode(tb_userid.Text), false);
                        }

                        else if(getnumberofcount < 3)
                        {
                            int updatenumberofcount = getnumberofcount + 1;
                            Updatenoofcount(JS_useremail, updatenumberofcount);

                            errorMsg.Text = "Email address or password is invalid. Please enter again.";
                        }


                        else
                        {
                            errorMsg.Text = "Wrong email address or password is entered. Please try again.";
                        }
                    }
                    else
                    {
                        errorMsg.Text = "Wrong email address or password is entered. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }

        protected string getattemptlogin(string attempt_email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select attemptcounter FROM JSDatabase WHERE EmailAddress=@ATTEMPT_EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ATTEMPT_EMAIL", attempt_email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["attemptcounter"] != null)
                        {
                            if (reader["attemptcounter"] != DBNull.Value)
                            {
                                h = reader["attemptcounter"].ToString();
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

        public int Updatenoofcount(string attempt_email, int counter)
        {

            string DBConnect = ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "UPDATE JSDatabase SET attemptcounter=@paracounter WHERE EmailAddress=@ATTEMPT_EMAIL";
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@paracounter", counter);
            sqlCmd.Parameters.AddWithValue("@ATTEMPT_EMAIL", attempt_email);

            myConn.Open();
            int result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;
        }

        public int Updatedatetime(string attempt_email)
        {
            string DBConnect = ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
            SqlConnection myConn = new SqlConnection(DBConnect);

            string sqlStmt = "UPDATE JSDatabase SET lockoutduration=@paradatetime WHERE EmailAddress=@ATTEMPT_EMAIL";
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, myConn);

            sqlCmd.Parameters.AddWithValue("@paradatetime", DateTime.Now);
            sqlCmd.Parameters.AddWithValue("@ATTEMPT_EMAIL", attempt_email);

            myConn.Open();
            int result = sqlCmd.ExecuteNonQuery();

            myConn.Close();

            return result;
        }

        protected bool getaccountlocked(string attempt_email)
        {
            long h = 0;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "SELECT DATEDIFF(MINUTE, lockoutduration, GETDATE()) AS lockoutMin FROM JSDatabase WHERE EmailAddress=@ATTEMPT_EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ATTEMPT_EMAIL", attempt_email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["lockoutMin"] != null)
                        {
                            if (reader["lockoutMin"] != DBNull.Value)
                            {
                                h = Convert.ToInt64(reader["lockoutMin"]); // min since lockout
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
            return h > 5 ? false : true;
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

        protected void JS_btnRegisterNow_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }
    }
}