using Assignment6.Models;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Assignment6
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If already logged in via cookie redirect to home
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (!IsPostBack && loginCookie != null &&
                !string.IsNullOrEmpty(loginCookie["Username"]))
            {
                Response.Redirect("Default.aspx");
                return;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            // Validate inputs
            if (string.IsNullOrEmpty(username))
            {
                lblError.Text = "⚠️ Please enter a username.";
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                lblError.Text = "⚠️ Please enter a password.";
                return;
            }

            try
            {
                // Hash password using Brandon's CryptoUtil
                string hashedPassword = CryptoUtil.hashMe(password);

                // DEBUG — remove before final submission
                lblDebug.Text = string.Format(
                    "DEBUG: Role={0} | Hash={1}...",
                    role, hashedPassword.Substring(0, 15));

                // Authenticate against correct XML file
                string xmlFile = (role == "Staff") ? "Staff.xml" : "Member.xml";
                bool isAuthenticated = AuthenticateUser(username, hashedPassword, xmlFile);

                if (isAuthenticated)
                {
                    // Store in session
                    Session["UserRole"] = role;
                    Session["Username"] = username;

                    // Save to persistent cookie (7 days)
                    HttpCookie loginCookie = new HttpCookie("SwimClubLogin");
                    loginCookie["Username"] = username;
                    loginCookie["Role"] = role;
                    loginCookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(loginCookie);

                    // Set Forms Auth ticket
                    FormsAuthentication.SetAuthCookie(username, false);

                    // Always redirect to home page after login
                    Response.Redirect("Default.aspx", true);
                }
                else
                {
                    lblError.Text = "⚠️ Invalid username or password.";
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Expected when Response.Redirect called with true — ignore
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }
        }

        private bool AuthenticateUser(string username, string hashedPassword, string xmlFile)
        {
            string xmlPath = Server.MapPath("~/App_Data/" + xmlFile);

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);

            XmlNodeList members = doc.SelectNodes("//Member");
            foreach (XmlNode member in members)
            {
                string storedUsername = member["Username"] != null ?
                    member["Username"].InnerText.Trim() : "";
                string storedPassword = member["Password"] != null ?
                    member["Password"].InnerText.Trim() : "";

                lblDebug.Text += string.Format(
                    " | Checking: [{0}] vs [{1}]",
                    storedUsername, username);

                if (storedUsername == username && storedPassword == hashedPassword)
                    return true;
            }
            return false;
        }
    }
}