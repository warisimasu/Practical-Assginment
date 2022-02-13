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
using System.IO;

namespace Password_Hashing
{
    public partial class Success : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;
        //static byte[] nric = null;
        static string userid = null;
        static byte[] CC = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["UserID"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {

                lbl_success.Text = "very gentle words";
                if(!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {

                    lbl_success.Text = "very gentle words";
                    // Removing all sessions and cookies
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();

                    Response.Redirect("Login.aspx", false);

                    if(Request.Cookies["ASP.NET_SessionId"] != null)
                    {
                        // emptying a string
                        Response.Cookies["ASP.NET_Sessionid"].Value = string.Empty;

                        // Setting the session pass data to the pass to disrupt any investication.
                        Response.Cookies["ASP.NET_Sessionid"].Expires = DateTime.Now.AddMonths(-10);
                    }

                    if (Request.Cookies["AuthToken"] != null)
                    {
                        Response.Cookies["AuthToken"].Value = string.Empty;
                        Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                    }
                }
                else
                {
                    lbl_success.Text = "You have Logged In!";
                    lbl_success.ForeColor = System.Drawing.Color.Green;
                    userid = Session["UserID"].ToString();
                    displayUserProfile(userid);
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
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
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
            string sql = "SELECT * FROM AppUsers WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["email"] != DBNull.Value)
                        {
                            lbl_userID.Text = reader["email"].ToString();

                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }

                        if (reader["fname"] != DBNull.Value)
                        {
                            lbl_fname.Text = reader["fname"].ToString();

                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }
                        if (reader["lname"] != DBNull.Value)
                        {
                            lbl_lname.Text = reader["lname"].ToString();

                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }
                        if (reader["ccencrypt"] != DBNull.Value)
                        {
                            CC = Convert.FromBase64String(reader["ccencrypt"].ToString());

                            
                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            lbl_dob.Text = reader["dob"].ToString();

                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }
                        if (reader["photo"] != DBNull.Value)
                        {
                            byte[] data = (byte[])reader["photo"];
                            string strdata = Convert.ToBase64String(data);
                            photo.ImageUrl = "data:Image/png;base64," + strdata;

                            //cipherTextNRIC = (byte[])reader["Nric"];
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
                    lbl_cc.Text = decryptData(CC);
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


        protected void Logout(object sender, EventArgs e)
        {

            logingOut();

            // Remove All Sessions
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();


            //Redirct back to login page
            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                // emptying a string
                Response.Cookies["ASP.NET_Sessionid"].Value = string.Empty;

                // Setting the session pass data to the pass to disrupt any investication.
                Response.Cookies["ASP.NET_Sessionid"].Expires = DateTime.Now.AddMonths(-10);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void logingOut()
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


                            cmd.Parameters.AddWithValue("@email", lbl_userID.Text.Trim());
                            cmd.Parameters.AddWithValue("@datetime", datetime);
                            cmd.Parameters.AddWithValue("@action", "Logged Out");


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