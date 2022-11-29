using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mainLoginPageWeb
{
    public partial class register : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection("Data Source=TARLAN\\LOCALHOST;Initial Catalog=Login;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) CaptchaText();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text != "" && txtUsername.Text != "" && txtRptPassword.Text != "")
                {
                    if (txtPassword.Text == txtRptPassword.Text)
                    {
                        if (txtCaptcha.Text == lblCaptcha.Text)
                        {
                            conn.Open();
                            string query = "declare @res int; execute Rgstr @res output, @username, @password; select @res Returner\r\n";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                            cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 0)
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('Successful Registation!');window.location='login.aspx';", true);
                            }
                            else
                            {
                                string script = "alert(\"This user exists! Please try again.\");";
                                ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                            }
                            conn.Close();
                        }
                        else
                        {
                            string script = "alert(\"Captchas are not matching! Please try again.\");";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                        }
                    }
                    else
                    {
                        string script = "alert(\"Passwords are not matching! Try again.\");";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                    }

                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtRptPassword.Text = "";
                    txtEmail.Text = "";
                    txtCaptcha.Text = "";
                    lblCaptcha.Text = "";
                    txtUsername.Focus();
                }
                else
                {
                    string script = "alert(\"Empty field! Please fill.\");";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                }
                CaptchaText();
            }
            catch { }
            finally { }
        }

        protected void btnToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }

        public void CaptchaText()
        {
            Random rnd = new Random();

            const string src = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int size = 6;

            var randomStr = "";

            for (int i = 0; i < size; i++)
            {
                int x = rnd.Next(src.Length);
                randomStr += src[x];
            }

            lblCaptcha.Text = randomStr;
        }
    }
}