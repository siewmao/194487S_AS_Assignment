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
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;


namespace AS_Week6
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
            

            if (!IsPostBack)
            {
                JS_tbDOB.Visible = true;
                JS_lbYear.Visible = false;
                JS_lbMonth.Visible = false;
                JS_ddlYear.Visible = false;
                JS_ddlMonth.Visible = false;
                line.Visible = false;
                JS_Calendar.Visible = false;

                DataSet years = new DataSet();
                years.ReadXml(Server.MapPath("~/194487S_Year.xml"));

                JS_ddlYear.DataTextField = "Number";
                JS_ddlYear.DataValueField = "Number";

                JS_ddlYear.DataSource = years;
                JS_ddlYear.DataBind();

                DataSet months = new DataSet();
                months.ReadXml(Server.MapPath("~/194487S_Month.xml"));

                JS_ddlMonth.DataTextField = "Number";
                JS_ddlMonth.DataValueField = "Number";

                JS_ddlMonth.DataSource = months;
                JS_ddlMonth.DataBind();

            }
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


        private Boolean JS_checkemail()
        {
            Boolean JS_emailavailable = false;
            String mycon = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename =| DataDirectory |\MYDB.mdf; Initial Catalog = MYDB; Integrated Security = True";
            String myquery = "Select * from JSDatabase where EmailAddress='" + JS_tbEmailAddr.Text + "'";
            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO JSDatabase VALUES(@FirstName, @LastName, @CreditCardInfo, @EmailAddress, @PasswordHash, @PasswordSalt, @DOB, @DateTimeRegistered, @EmailVerified, @IV, @Key, @attemptcounter, @lockoutduration)"))
                {
                    cmd.CommandText = myquery;
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            JS_emailavailable = true;
                        }
                        con.Close();

                        return JS_emailavailable;
                    }
                }
            }
        }






            public void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO JSDatabase VALUES(@FirstName, @LastName, @CreditCardInfo, @EmailAddress, @PasswordHash, @PasswordSalt, @DOB, @DateTimeRegistered, @EmailVerified, @IV, @Key, @attemptcounter, @lockoutduration)"))
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", JS_tbFName.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", JS_tbLName.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCardInfo", Convert.ToBase64String(encryptData(JS_tbCCInfo.Text.Trim())));
                            cmd.Parameters.AddWithValue("@EmailAddress", JS_tbEmailAddr.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(JS_tbDOB.Text));
                            cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@attemptcounter", 0);
                            cmd.Parameters.AddWithValue("@lockoutduration", DateTime.Now);

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

        private bool JS_ValidateInput()
        {
            bool result;
            lbFName.Text = String.Empty;
            lbLName.Text = String.Empty;
            lbCCInfo.Text = String.Empty;
            lbEmailAddr.Text = String.Empty;
            lbPwd.Text = String.Empty;
            lbCfmPwd.Text = String.Empty;
            lbDOB.Text = String.Empty;

            if (String.IsNullOrEmpty(JS_tbFName.Text))
            {
                lbFName.Text += "Required!" + "<br/>";
            }
            if (String.IsNullOrEmpty(JS_tbLName.Text))
            {
                lbLName.Text += "Required!" + "<br/>";
            }
            if (String.IsNullOrEmpty(JS_tbCCInfo.Text))
            {
                lbCCInfo.Text += "Required!" + "<br/>";
            }
            if (String.IsNullOrEmpty(JS_tbEmailAddr.Text))
            {
                lbEmailAddr.Text += "Required!" + "<br/>";
            }
            if (String.IsNullOrEmpty(tb_pwd.Text))
            {
                lbPwd.Text += "Required!" + "<br/>";
            }
            if(String.IsNullOrEmpty(tb_cfpwd.Text))
            {
                lbCfmPwd.Text += "Required!" + "<br/>";
            }
            if (String.IsNullOrEmpty(JS_tbDOB.Text))
            {
                lbDOB.Text += "Required!" + "<br/>";
            }
            if (String.IsNullOrEmpty(lbFName.Text) && String.IsNullOrEmpty(lbLName.Text) && String.IsNullOrEmpty(lbCCInfo.Text) && String.IsNullOrEmpty(lbEmailAddr.Text) && String.IsNullOrEmpty(lbPwd.Text) && String.IsNullOrEmpty(tb_cfpwd.Text) && String.IsNullOrEmpty(lbDOB.Text))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        

        private int checkPasswordStrength(string password)
        {
            int score = 0;

            // include your implementation here

            // Score 1 Very Weak!
            // if length of password is less than 8 chars
            if (password.Length < 8)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            // Score 2 Weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            // Score 3 Medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            // Score 4 Strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            // Score 5 Excellent
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }

            return score;
        }

        protected void JS_btn_Submit_Click(object sender, EventArgs e)
        {


            //string simple = HttpUtility.HtmlEncode(JS_tbFName.Text);
            //Response.Write(simple);
            //HttpUtility.HtmlEncode(JS_tbEmailAddr.Text);
            //HttpUtility.HtmlEncode(tb_pwd.Text);
            //HttpUtility.HtmlEncode(tb_cfpwd.Text);


            
                if (JS_tbFName.Text == "" || JS_tbLName.Text == "" || JS_tbCCInfo.Text == "" || JS_tbEmailAddr.Text == "" || JS_tbDOB.Text == "" || tb_cfpwd.Text == "" || tb_pwd.Text == "")
                {
                    bool validInput = JS_ValidateInput();
                }
                else if (tb_cfpwd.Text != tb_pwd.Text)
                {
                    lbMismatchPwd.Text = "Password Mismatch!";
                }
                else if (tb_cfpwd.Text == tb_pwd.Text || JS_tbFName.Text != "" || JS_tbLName.Text != "" || JS_tbCCInfo.Text != "" || JS_tbEmailAddr.Text != "" || JS_tbDOB.Text != "" || tb_cfpwd.Text != "" || tb_pwd.Text != "")
                {
                    if (JS_checkemail() == true)
                    {
                        lbEmailExisted.Text = "Email already existed!";

                    }
                    else if (tb_cfpwd.Text == tb_pwd.Text || JS_tbFName.Text != "" || JS_tbLName.Text != "" || JS_tbCCInfo.Text != "" || JS_tbEmailAddr.Text != "" || JS_tbDOB.Text != "" || tb_cfpwd.Text != "" || tb_pwd.Text != "" || JS_checkemail() == false)
                    {
                    

                        //string pwd = get value from your Textbox

                        string pwd = tb_pwd.Text.ToString().Trim();
                    
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
                    
                        Response.Redirect("login.aspx");
                    
                }
            }
            
            
        }
        
                protected void JS_ChooseDOB_Click(object sender, EventArgs e)
                {
                    if (JS_Calendar.Visible)
                    {

                        JS_lbYear.Visible = false;
                        JS_lbMonth.Visible = false;
                        JS_ddlYear.Visible = false;
                        JS_ddlMonth.Visible = false;
                        line.Visible = false;
                        JS_Calendar.Visible = false;
                    }
                    else
                    {
                        JS_tbDOB.Visible = true;
                        JS_lbYear.Visible = true;
                        JS_lbMonth.Visible = true;
                        JS_ddlYear.Visible = true;
                        JS_ddlMonth.Visible = true;
                        line.Visible = true;
                        JS_Calendar.Visible = true;
                    }
                }

                protected void JS_ddlYear_SelectedIndexChanged(object sender, EventArgs e)
                {
                    int year = Convert.ToInt16(JS_ddlYear.SelectedValue);
                    int month = Convert.ToInt16(JS_ddlMonth.SelectedValue);

                    JS_Calendar.VisibleDate = new DateTime(year, month, 1);
                    JS_Calendar.SelectedDate = new DateTime(year, month, 1);
                }

                protected void JS_ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
                {
                    int year = Convert.ToInt16(JS_ddlYear.SelectedValue);
                    int month = Convert.ToInt16(JS_ddlMonth.SelectedValue);

                    JS_Calendar.VisibleDate = new DateTime(year, month, 1);
                    JS_Calendar.SelectedDate = new DateTime(year, month, 1);
                }

                protected void JS_Calendar_SelectionChanged(object sender, EventArgs e)
                {
                    JS_tbDOB.Text = JS_Calendar.SelectedDate.ToShortDateString();
                    JS_lbYear.Visible = false;
                    JS_lbMonth.Visible = false;
                    JS_ddlYear.Visible = false;
                    JS_ddlMonth.Visible = false;
                    line.Visible = false;
                    JS_Calendar.Visible = false;
                }
        

        protected void JS_btnCheck_Click(object sender, EventArgs e)
        {
            //implement codes for the button event
            //Extract data from textbox
            int scores = checkPasswordStrength(tb_pwd.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Password is Very Weak";
                    break;

                case 2:
                    status = "Password is Weak";
                    break;

                case 3:
                    status = "Password is Medium";
                    break;

                case 4:
                    status = "Password is Strong";
                    break;

                case 5:
                    status = "Password is very Strong";
                    break;

                default:
                    break;
            }
            lbMismatchPwd.Text = "Status: " + status;
            if (scores < 4)
            {
                lbMismatchPwd.ForeColor = Color.Red;
                return;
            }
            lbMismatchPwd.ForeColor = Color.Green;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}