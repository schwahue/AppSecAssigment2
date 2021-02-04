using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AppSecAssigment2
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {


        }

        /*
         * todo:
         * passSalt + hashing
         * creditInfo encryption + VI + key
         * password compare against table?
         * antibot
         * addition to db
         */

        /*
         * Password age store a date. Compare current time to that stored value for result.
         */

        protected void btn_Register_Click(object sender, EventArgs e)
        {
            try
            {
                if (fieldChecker() == 0)
                {
                    //input sanitation
                    var fName = HttpUtility.HtmlEncode(tb_fName.Text.ToString());
                    var lName = HttpUtility.HtmlEncode(tb_lName.Text.ToString());
                    DateTime dob = Convert.ToDateTime(HttpUtility.HtmlEncode(tb_DOB.Text));
                    var pwd = HttpUtility.HtmlEncode(tb_pwd.Text.ToString().Trim());
                    var email = HttpUtility.HtmlEncode(tb_email.Text.ToString());
                    var creditCard = HttpUtility.HtmlEncode(tb_creditCard.Text.ToString());


                    if (pwdChecker(pwd) == 0)
                    {
                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;

                        var pwdDict = HashPwd(pwd);
                        insertAccountDB(fName, lName, creditCard, email, pwdDict["pwdHash"], pwdDict["pwdSalt"], dob);
                        Session["successMsg"] = $"Your account has been created successfully, {fName} {lName}!";
                        Response.Redirect("Login", false);
                    }
                }
            }
            catch (Exception ex)
            {
                lb_genError.Visible = true;
                lb_genError.Text = "Something went wrong with your registration processm </br> Please contact our admin.";
                throw new Exception(ex.ToString());
            }


;
        }

        //return dict of [pwdHash, pwdSalt]
        public Dictionary<String, String> HashPwd(string pwd)
        {
            Dictionary<String, String> hashedPwd = new Dictionary<string, string>();

            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            string salt = Convert.ToBase64String(saltByte);

            //init hashing algo
            SHA512Managed hashing = new SHA512Managed();

            //process
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd)); // usage?
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            string finalHash = Convert.ToBase64String(hashWithSalt);

            hashedPwd.Add("pwdHash", finalHash);
            hashedPwd.Add("pwdSalt", salt);

            return hashedPwd;
        }

        //return cipherText in readable format
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

        public void insertAccountDB(
            string fName, string lName, string Credit_Card, string Email, string pwdHash, string pwdSalt, DateTime DOB)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@fName, @lName, @creditCard, @Email, @pwdHash, @pwdSalt, @DOB, @IV, @Key, @lockout,@pwdAge)"))
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@fName", fName);
                            cmd.Parameters.AddWithValue("@lName", lName);
                            cmd.Parameters.AddWithValue("@creditCard", encryptData(Credit_Card));
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@pwdHash", pwdHash);
                            cmd.Parameters.AddWithValue("@pwdSalt", pwdSalt);
                            cmd.Parameters.AddWithValue("@DOB", DOB);
                            cmd.Parameters.AddWithValue("@IV", IV);
                            cmd.Parameters.AddWithValue("@Key", Key);
                            cmd.Parameters.AddWithValue("@lockout", 0);
                            cmd.Parameters.AddWithValue("@pwdAge", DateTime.Now);

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

        //return true if there's an existing record else false
        protected bool dbEmailChecker(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select COUNT(*) FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", email);
            try
            {
                connection.Open();
                int userCount = (int)command.ExecuteScalar();
                if(userCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
        }

        //Validations
        protected int fieldChecker()
        {
            int error = 0;
            if (string.IsNullOrEmpty(tb_fName.Text.ToString())) {
                error += 1;
                lb_fNameError.Visible = true;
                lb_fNameError.ForeColor = Color.Red;
                lb_fNameError.Text = "Please enter a First Name";
            }
            else
            {
                lb_fNameError.Visible = false;
            }

            if (string.IsNullOrEmpty(tb_lName.Text.ToString())) {
                error += 1;
                lb_lNameError.Visible = true;
                lb_lNameError.ForeColor = Color.Red;
                lb_lNameError.Text = "Please enter a Last Name";
            }
            else
            {
                lb_lNameError.Visible = false;
            }

            if (string.IsNullOrEmpty(tb_DOB.Text.ToString())) {
                error += 1;
                lb_DOBError.Visible = true;
                lb_DOBError.ForeColor = Color.Red;
                lb_DOBError.Text = "Please enter a Date of Birth";
            }
            else
            {
                lb_DOBError.Visible = false;
            }

            if (!string.IsNullOrEmpty(tb_pwd.Text.ToString())) {
                if (pwdChecker(tb_pwd.Text.ToString()) == 0)
                {
                    lb_pwdError.Visible = false;
                }
            }
            else
            {
                error += 1;
                lb_pwdError.Visible = true;
                lb_pwdError.ForeColor = Color.Red;
                lb_pwdError.Text = "Please enter a Password";
            }

            var emailChecker = HttpUtility.HtmlEncode(tb_email.Text.ToString());
            if (string.IsNullOrEmpty(tb_email.Text.ToString())) {
                error += 1;
                lb_emailError.Visible = true;
                lb_emailError.ForeColor = Color.Red;
                lb_emailError.Text = "Please enter an Email";
            }
            else if(dbEmailChecker(emailChecker)){
                error += 1;
                lb_emailError.Visible = true;
                lb_emailError.ForeColor = Color.Red;
                lb_emailError.Text = "Email is already in use, please try another email";
            }
            else
            {
                lb_emailError.Visible = false;
            }

            if (string.IsNullOrEmpty(tb_creditCard.Text.ToString())) {
                error += 1;
                lb_creditCardInfoError.Visible = true;
                lb_creditCardInfoError.ForeColor = Color.Red;
                lb_creditCardInfoError.Text = "Please enter a Credit Card Information";
            }
            else
            {
                lb_creditCardInfoError.Visible = false;
            }

            return error;
        }

        protected int pwdChecker(string pwd)
        {
            int error = 0;
            string errorMsg = "";
            if (pwd.Length < 8)
            {
                error += 1;
                errorMsg += "Password is too short </br>";
            }
            if (!Regex.IsMatch(pwd, "[a-z]"))
            {
                error += 1;
                errorMsg += "Password needs at least one smaller case character </br>";
            }
            if (!Regex.IsMatch(pwd, "[A-Z]"))
            {
                error += 1;
                errorMsg += "Password needs at least one upper case character </br>";
            }
            if (!Regex.IsMatch(pwd, "[/d]"))
            {
                error += 1;
                errorMsg += "Password needs at least one number </br>";
            }
            if (!Regex.IsMatch(pwd, "[!@#$%^&*(),.?]"))
            {
                error += 1;
                errorMsg += "Password needs at least a special character </br>";
            }

            lb_pwdError.Visible = true;
            lb_pwdError.Text = errorMsg;
            lb_pwdError.Text = errorMsg;
            lb_pwdError.ForeColor = Color.Red;

            return error;
        }
    }
}