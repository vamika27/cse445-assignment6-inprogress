using Assignment6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Assignment6.Models;

namespace Assignment6
{
    public partial class SignUpControl : System.Web.UI.UserControl
    {
        // Public property so the host page can read the result
        public string SignUpMessage { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Nothing needed on load
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            // Validate all inputs are present
            if (!Page.IsValid) return;

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string preferredEvent = ddlEvent.SelectedValue;
            string phone = txtPhone.Text.Trim();

            // Double-check required fields in code-behind as well
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(preferredEvent))
            {
                ShowMessage("⚠️ Please fill in all required fields.", "red");
                return;
            }

            // Build a UserProfile object from the inputs
            UserProfile newUser = new UserProfile(name, email, preferredEvent);

            // Also save profile to a cookie so the user stays recognized
            System.Web.HttpCookie signUpCookie = new System.Web.HttpCookie("SwimClubUserProfile");
            signUpCookie["FullName"] = newUser.FullName;
            signUpCookie["Email"] = newUser.Email;
            signUpCookie["PreferredEvent"] = newUser.PreferredEvent;
            signUpCookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(signUpCookie);

            // Show success message
            string successMsg = string.Format(
                "✅ <strong>Sign up successful!</strong><br/>" +
                "<strong>Name:</strong> {0}<br/>" +
                "<strong>Email:</strong> {1}<br/>" +
                "<strong>Preferred Event:</strong> {2}<br/>" +
                "{3}" +
                "<em>Your profile has been saved to a cookie for 7 days.</em>",
                newUser.FullName,
                newUser.Email,
                newUser.PreferredEvent,
                !string.IsNullOrEmpty(phone) ? "<strong>Phone:</strong> " + phone + "<br/>" : ""
            );

            SignUpMessage = successMsg;
            ShowMessage(successMsg, "green");

            // Clear the form after successful signup
            ClearForm();

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearForm();
            pnlMessage.Visible = false;
        }

        // Helper: displays a message in the result panel
        private void ShowMessage(string message, string color)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.ForeColor = color == "green" ?
                System.Drawing.Color.Green :
                System.Drawing.Color.Red;
        }

        // Helper: resets all form fields
        private void ClearForm()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtPhone.Text = "";
            ddlEvent.SelectedIndex = 0;
        }
    }
}