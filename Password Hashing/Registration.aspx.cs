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

using System.Text.RegularExpressions;
using System.Drawing;

//Install System.IO Nuget Package
using System.IO;

using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Net;


using BCrypt.Net;

namespace Password_Hashing
{
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        //static string isDebug = ConfigurationManager.AppSettings["isDebug"].ToString();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            //photo validator
            bool photo = false;

            //check if password matches
            string pwd = tb_pwd.Text.ToString().Trim();
            string cfpwd = tb_cfpwd.Text.ToString().Trim();

            // Check if all shit in filled in (input validation)
            // then send it to the database
            string email = tb_email.Text.ToString().Trim();
            string fname = tb_fname.Text.ToString().Trim();
            string lname = tb_lname.Text.ToString().Trim();
            string creditcard = tb_creditcard.Text.ToString().Trim();
            DateTime dob = Calendar1.SelectedDate;

            if (string.IsNullOrEmpty(email))
            {
                emailchecker.Text = "Email Required!";
                emailchecker.ForeColor = Color.Red;

            }
            else
            {
                emailchecker.Text = " ";
            }

            if (string.IsNullOrEmpty(fname))
            {

                fnamechecker.Text = "First Name Required!";
                fnamechecker.ForeColor = Color.Red;
            }
            else
            {
                fnamechecker.Text = " ";
            }

            if (string.IsNullOrEmpty(lname))
            {

                lnamechecker.Text = "Last Name Required";
                lnamechecker.ForeColor = Color.Red;
            }
            else
            {
                lnamechecker.Text = " ";
            }

            if (string.IsNullOrEmpty(pwd))
            {

                passchecker.Text = "Password Required!";
                passchecker.ForeColor = Color.Red;
            }
            else
            {
                passchecker.Text = " ";
            }

            if (string.IsNullOrEmpty(cfpwd))
            {
                cfpasschecker.Text = "Please Confirm your password";
                cfpasschecker.ForeColor= Color.Red;
            }
            else
            {
                cfpasschecker.Text = " ";
            }

            if (string.IsNullOrEmpty(creditcard))
            {
                creditcardchecker.Text = "Credit Card No. Required";
                creditcardchecker.ForeColor = Color.Red;
            }
            else
            {
                creditcardchecker.Text = " ";
            }

            if (dob == null)
            {

                Console.WriteLine(dob.ToString());

                calenderchecker.Text = "Date of Birth Required!";
                calenderchecker.ForeColor = Color.Red;
            }
            else
            {
                Console.WriteLine("-----------------------------------------------------------------");
                Console.WriteLine(dob.ToString());
                string dobstr = dob.ToString();
                calenderchecker.Text = dobstr;
                Console.WriteLine("-----------------------------------------------------------------");
                calenderchecker.Text = " ";
            }

            if (!FileUpload1.HasFile)
            {
                photochecker.Text = "Photo Required";
                photochecker.ForeColor = Color.Red;
                photo = false;
            }
            else
            {
                photochecker.Text = " ";
                HttpPostedFile postedFile = FileUpload1.PostedFile;
                string fileName = Path.GetFileName(postedFile.FileName);
                string fileExtension = Path.GetExtension(fileName);
                int fileSize = postedFile.ContentLength;

                if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".bmp" ||
                                fileExtension.ToLower() == ".gif" || fileExtension.ToLower() == ".png")
                {

                    photo = true;

                }
                else
                {
                    photochecker.Text = "Please Upload a .jpg, .png, .bmp or .gif file";
                    photochecker.ForeColor = Color.Red;
                    photo = false;


                }

             }

            // Implement codes for the button event
            // Extract data from textbox
            if (!string.IsNullOrEmpty(pwd))
            {
                int scores = checkPassword(tb_pwd.Text);
                string status = "";

                // different cases with then scores with switch cases
                switch (scores)
                {
                    // Score 1
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
                        status = "Very Strong";
                        break;


                    default:
                        break;

                }

                passchecker.Text = "Status: " + status;


                //Change color to red if score is below 4
                if (scores < 4)
                {
                    passchecker.ForeColor = Color.Red;
                    return;
                }

                else
                {
                    // default color of green
                    passchecker.ForeColor = Color.Green;



                    // if confirm password matches password
                    if (pwd == cfpwd)
                    {

                        //if (ValidateCaptcha())
                        //{
                            // if all blanks are filled in
                            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(lname) && !string.IsNullOrEmpty(pwd) && !string.IsNullOrEmpty(cfpwd) && !string.IsNullOrEmpty(creditcard) && !(dob == null) && photo)
                            {
                                lb_error2.Text = "Successful";
                                lb_error2.ForeColor = Color.Green;



                                // throw all information into the database

                                // generate random salt
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] saltByte = new byte[8];
                                rng.GetBytes(saltByte);
                                salt = Convert.ToBase64String(saltByte);

                                SHA512Managed hashing = new SHA512Managed();

                                // combine the password with the salt
                                string pwdAndSalt = pwd + salt;
                                //finalHash = hashing.Encode(pwdAndSalt);
                                //finalHash = BCrypt.Net.BCrypt.(pwdAndSalt);
                                finalHash = Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdAndSalt)));




                                // Encryption thingys
                                RijndaelManaged cipher = new RijndaelManaged();
                                cipher.GenerateKey();
                                Key = cipher.Key;
                                IV = cipher.IV;




                                // the method that pushes items into the database
                                createAccount();

                                // the method that logs the regsitration
                                logRegister();


                            }




                        //}
                    

                    }
                    else
                    {
                        //error message time
                        lb_error1.Text = "Passwords Do Not Match!";
                        lb_error1.ForeColor = Color.Red;
                    }

                }



            }




        }

        protected void logRegister()
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


                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@datetime", datetime);
                            cmd.Parameters.AddWithValue("@action", "Registration");
                            

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


        protected void createAccount()
        {

            try
            {
                // declare an instance of the database
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO AppUsers VALUES(@email,@fname,@lname," +
                        "@ccencrypt,@passhash,@pass_salt,@dob,@photo,@IV,@Key,@loginAttempts )"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;

                            //ScryptEncoder hashing = new ScryptEncoder();


                            cmd.Parameters.AddWithValue("@email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@fname", tb_fname.Text.Trim());   
                            cmd.Parameters.AddWithValue("lname", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@ccencrypt", Convert.ToBase64String(encryptData(tb_creditcard.Text.Trim())));
                            cmd.Parameters.AddWithValue("@passhash", finalHash);
                            cmd.Parameters.AddWithValue("@pass_salt", salt);
                            cmd.Parameters.AddWithValue("@dob", Calendar1.SelectedDate);

                            // seting image path
                            //string str = FileUpload1.FileName;
                            //FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Upload/" + str));
                            //string Image = "~/Upload/" + str.ToString();
                            //cmd.Parameters.AddWithValue("@photo", Image);

                            // save photo data
                            HttpPostedFile postedFile = FileUpload1.PostedFile;
                            string fileName = Path.GetFileName(postedFile.FileName);
                            string fileExtension = Path.GetExtension(fileName);
                            int fileSize = postedFile.ContentLength;

                            Stream stream = postedFile.InputStream;
                            BinaryReader binaryReader = new BinaryReader(stream);
                            byte[] PhotoDatabytes = binaryReader.ReadBytes((int)stream.Length);

                            cmd.Parameters.AddWithValue("@photo", PhotoDatabytes);

                            // encryption keys and iv
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));

                            cmd.Parameters.AddWithValue("@loginAttempts", "3");

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
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

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

            // Score 0 very weak
            // If Length of password is less than 8 chars
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            //Score 2 weak
            // If the password has at least 1 lowercase character
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            else
            {
                return score;
            }

            //Score 3 medium
            // If the password has at least 1 uppercase character
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            else
            {
                return score;
            }

            //Score 4 Strong
            // If the password has at least 1 numeral
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            else
            {
                return score;
            }

            //Score 5 Excellent
            // If the password contans at least 1 special character
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }
            else
            {
                return score;
            }

            return score;
        }

        // public class to validate Captcha
        public class MyObject{
            public string success {  get; set; } 

            public List<string> ErrorMessage { get; set; }
        }

        // validate captcha
        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
                ("https://www.google.com/recaptcha/api/siteverifiy?secret=6Ld-01oeAAAAAFBlX8-W3QfuX3cYU651TK8cAhrP &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {

                    using(StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
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


    }
}