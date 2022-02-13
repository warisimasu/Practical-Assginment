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

using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Net;
using System.IO;

using System.Text.RegularExpressions;
using System.Drawing;

//using Scrypt;

namespace Password_Hashing
{
    public partial class Login : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";

        int dbAttempts = 0;
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>window.alert('before getDBHash.')</script>");         
            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();


            string dbHash = getDBHash(userid).Trim();
            string dbSalt = getDBSalt(userid).Trim();
            dbAttempts = getAttempts(userid);


            //if (ValidateCaptcha())
            //{
            if (dbAttempts > 0)
            {
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;

                        //string userHash = BCrypt.Net.BCrypt.HashPassword(pwd);
                        //string userHash = Convert.ToBase64String(hashWithSalt);

                        SHA512Managed hashing = new SHA512Managed();
                        string userHash = Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt)));

                        if (userHash.Equals(dbHash))
                        {

                            logLogin();

                            Session["UserID"] = userid;
                            //lb_error.Text = dbHash + "   " + userHash + "\n" + dbSalt + "\nSuccessful";

                            // Generate the AuthToken
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));


                            Response.Redirect("Success.aspx", false);
                        }
                        else
                        {
                            reduceAttempts(userid);
                            errorMsg = "Userid or password is not valid. Please try again.";
                            lb_error.Text = errorMsg;
                            //lb_error.Text =  dbHash + "   " + userHash + "\n" + dbSalt;
                            //Response.Redirect("Login.aspx", false);
                        }
                    }
                    else
                    {
                        errorMsg = "Userid or password is not valid. Please try again.";
                        lb_error.Text = errorMsg;
                        //Response.Redirect("Login.aspx", false);
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
                lb_error.Text = "Maximum login attmepts reached";
            }

            //}
        }

        //private bool validateEmail(string email)
        //{

        //    string pattern = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        //    if (Regex.IsMatch(email, "[\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*]"))
        //    {

        //    }
        //}


        protected string getDBSalt(string email)
        {

            string s = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {

                    string sqlCmd = "SELECT pass_salt FROM AppUsers WHERE email=@email";
                    SqlCommand cmd = new SqlCommand(sqlCmd, conn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    s = (string)cmd.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return s;

            //SqlConnection connection = new SqlConnection(MYDBConnectionString);
            //string sql = "select pass_salt FROM AppUsers WHERE email=@USERID";
            //SqlCommand command = new SqlCommand(sql, connection);
            //command.Parameters.AddWithValue("@USERID", userid);

            //try
            //{
            //    connection.Open();

            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            if (reader["pass_salt"] != null)
            //            {
            //                if (reader["pass_salt"] != DBNull.Value)
            //                {
            //                    s = reader["pass_salt"].ToString();
            //                }
            //            }
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.ToString());
            //}

            //finally { connection.Close(); }

        }

        protected string getDBHash(string email)
        {

            string h = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {

                    string sqlCmd = "SELECT passhash FROM AppUsers WHERE email=@email";
                    SqlCommand cmd = new SqlCommand(sqlCmd, conn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    h = (string)cmd.ExecuteScalar();

                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
            return h;

          

        }

        protected int getAttempts(string email)
        {

            string h = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {

                    string sqlCmd = "SELECT loginAttempts FROM AppUsers WHERE email=@email";
                    SqlCommand cmd = new SqlCommand(sqlCmd, conn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@email", email);

                    conn.Open();
                    h = (string)cmd.ExecuteScalar();

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            int g = Int32.Parse(h);

            return g;



        }


        protected void reduceAttempts(string email)
        {
            dbAttempts = dbAttempts - 1;

            try
            {
                using(SqlConnection conn = new SqlConnection(MYDBConnectionString))
                {
                    string sqlCmd = "UPDATE AppUsers SET loginAttempts = @dbAttempts WHERE email=@email";
                    SqlCommand cmd = new SqlCommand(sqlCmd, conn);

                    cmd.CommandType=CommandType.Text;
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@dbAttempts", dbAttempts);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }




        protected string decryptData(byte[] cipherText)
        {

            string decryptedString = null;
            //byte[] cipherText = Convert.FromBase64String(cipherString);

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                //Decrypt
                //byte[] decryptedText = decryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                //decryptedString = Encoding.UTF8.GetString(decryptedText);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return decryptedString;
        }



        // public class to validate Captcha
        public class MyObject
        {
            public string success { get; set; }

            public List<string> ErrorMessage { get; set; }
        }

        // validate captcha
        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            //string captchaResponse = 

            //lb_error2.Text = captchaResponse;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                (" https://www.google.com/recaptcha/api/siteverifiy?secret=6Ld-01oeAAAAAFBlX8-W3QfuX3cYU651TK8cAhrP &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {

                        string jsonResponse = readStream.ReadToEnd();

                        lb_error2.Text = jsonResponse.ToString();

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


        protected void logLogin()
        {

            try
            {
                // declare an instance of the database
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Logger VALUES(@email,@datetime,@action )"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;

                            DateTime datetime = DateTime.Now;


                            cmd.Parameters.AddWithValue("@email", tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@datetime", datetime);
                            cmd.Parameters.AddWithValue("@action", "Login");


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


        protected void logFail()
        {

            try
            {
                // declare an instance of the database
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Logger VALUES(@email,@datetime,@action )"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;

                            DateTime datetime = DateTime.Now;


                            cmd.Parameters.AddWithValue("@email", tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@datetime", datetime);
                            cmd.Parameters.AddWithValue("@action", "LoginFailed");


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

    }
}