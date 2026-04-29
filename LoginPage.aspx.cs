using Assignment6.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
            // If already logged in redirect away from login page
            if (!IsPostBack && Request.IsAuthenticated)
            {
                string role = Session["UserRole"] as string;
                if (role == "Staff")
                    Response.Redirect("StaffPage.aspx");
                else if (role == "Member")
                    Response.Redirect("MemberPage.aspx");
                else
                    Response.Redirect("Default.aspx");
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
                // Hash password using Brandon's DLL
                string hashedPassword = CryptoUtil.hashMe(password);

                // DEBUG — shows hash and role being used
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

                    // Set Forms Auth cookie
                    FormsAuthentication.SetAuthCookie(username, false);

                    // Redirect based on role
                    if (role == "Staff")
                        Response.Redirect("StaffPage.aspx", false);
                    else
                        Response.Redirect("MemberPage.aspx", false);
                }
                else
                {
                    lblError.Text = "⚠️ Invalid username or password.";
                }
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