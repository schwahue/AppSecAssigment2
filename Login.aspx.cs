using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppSecAssigment2
{
    public class myObject
    {
        public string success { get; set; }
        public List<String> ErrorMessage { get; set; }
    }
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;



        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["successMsg"] != null)
            {
                var successMsg = Session["successMsg"].ToString();
                lb_Msg.Visible = true;
                lb_Msg.ForeColor = Color.Green;
                lb_Msg.Text = successMsg;
            }
        }

        /*
         * todo
         * avoid fixation attack
         * account lockout after 3 fail
         * antibot
         */

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration");
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            var email = HttpUtility.HtmlEncode(tb_Email.Text.ToString().Trim());
            var pwd = HttpUtility.HtmlEncode(tb_pwd.Text.ToString().Trim());
            int attempt = attemptFromDB();

            lb_Msg.Visible = true;
            if (ValidateCaptcha())
            {
                if (PwdValidate(pwd, email) && attempt < 3)
                {

                    Session["userid"] = email;

                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;
                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                    Response.Redirect("LandingPage", false);
                }
                else if (attempt == 3)
                {
                    lb_Msg.ForeColor = Color.Red;
                    lb_Msg.Text = "Your account has been locked out! </br> Please contact our admin to unlock your account!";
                }
                else
                {
                    attempt += 1;
                    lb_Msg.ForeColor = Color.Red;
                    lb_Msg.Text = $"Invalid login credential. Please try again! </br> You have {3 - attempt} attempts left.";
                }
            }
            
        }

        protected int attemptFromDB()
        {
            return 0;
        }

        protected int fieldValidator()
        {
            int Error = 0;
            return Error;
        }

        protected bool PwdValidate(string pwd, string email)
        {
            SHA512Managed hashing = new SHA512Managed();
            var dbHash = getDBHash(email);
            var dbSalt = getDBSalt(email);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return false;
        }

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Password_Hash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["Password_Hash"] != null)
                        {
                            if (reader["Password_Hash"] != DBNull.Value)
                            {
                                h = reader["Password_Hash"].ToString();
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

        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Password_Salt FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Password_Salt"] != null)
                        {
                            if (reader["Password_Salt"] != DBNull.Value)
                            {
                                s = reader["Password_Salt"].ToString();
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

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LdHgOUZAAAAALB4CShfEHq9hQGfyg2mMmzXzpRP &response=" + captchaResponse);
            try
            {
                using(WebResponse wResponse = req.GetResponse())
                {
                    using(StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        lb_Msg.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        myObject jsonObject = js.Deserialize<myObject>(jsonResponse);
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