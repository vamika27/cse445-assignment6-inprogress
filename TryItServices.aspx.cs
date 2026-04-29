using Assignment6.Models;
using Assignment6.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CryptoUtil;

namespace Assignment6
{
    public partial class TryItServices : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Nothing needed on load
        }

        // ===================== SWIM EVENT SERVICE HANDLERS =====================

        // Calls GetSwimmerPoints and displays the result
        protected void btnGetPoints_Click(object sender, EventArgs e)
        {
            string swimmerName = txtSwimmerName.Text.Trim();
            string eventName = ddlSwimEvent.SelectedValue;
            double time;

            if (string.IsNullOrEmpty(swimmerName))
            {
                ShowSwimResult("⚠️ Please enter a swimmer name.", false);
                return;
            }

            if (!double.TryParse(txtTime.Text.Trim(), out time) || time <= 0)
            {
                ShowSwimResult("⚠️ Please enter a valid time in seconds (e.g. 45.5).", false);
                return;
            }

            // Instantiate and call the service directly
            SwimEventService service = new SwimEventService();
            int points = service.GetSwimmerPoints(swimmerName, eventName, time);

            if (points == -1)
                ShowSwimResult("⚠️ Service returned an error. Check your inputs.", false);
            else
                ShowSwimResult(string.Format(
                    "✅ <strong>{0}</strong> earned <strong>{1} points</strong> in the {2}.",
                    swimmerName, points, eventName), true);
        }

        // Calls GetEventResult and displays the full result summary
        protected void btnGetResult_Click(object sender, EventArgs e)
        {
            string swimmerName = txtSwimmerName.Text.Trim();
            string eventName = ddlSwimEvent.SelectedValue;
            double time;

            if (string.IsNullOrEmpty(swimmerName))
            {
                ShowSwimResult("⚠️ Please enter a swimmer name.", false);
                return;
            }

            if (!double.TryParse(txtTime.Text.Trim(), out time) || time <= 0)
            {
                ShowSwimResult("⚠️ Please enter a valid time in seconds (e.g. 45.5).", false);
                return;
            }

            SwimEventService service = new SwimEventService();
            string result = service.GetEventResult(swimmerName, eventName, time);
            ShowSwimResult("✅ " + result, true);
        }

        // ===================== PRACTICE SERVICE HANDLERS =====================

        // Calls GetPracticeSchedule for the selected day
        protected void btnGetSchedule_Click(object sender, EventArgs e)
        {
            string day = ddlDay.SelectedValue;
            PracticeService service = new PracticeService();
            string result = service.GetPracticeSchedule(day);
            ShowPracticeResult("✅ " + result);
        }

        // Calls GetAllPracticeDays
        protected void btnGetAllDays_Click(object sender, EventArgs e)
        {
            PracticeService service = new PracticeService();
            string result = service.GetAllPracticeDays();
            ShowPracticeResult("✅ " + result);
        }

        // ===================== FUNDRAISING SERVICE HANDLERS =====================

        // Calls GetFundraisingProgress
        protected void btnGetProgress_Click(object sender, EventArgs e)
        {
            double goal, raised;

            if (!double.TryParse(txtGoal.Text.Trim(), out goal) || goal <= 0)
            {
                ShowFundResult("⚠️ Please enter a valid fundraising goal.", false);
                return;
            }

            if (!double.TryParse(txtRaised.Text.Trim(), out raised) || raised < 0)
            {
                ShowFundResult("⚠️ Please enter a valid amount raised.", false);
                return;
            }

            FundraisingService service = new FundraisingService();
            string result = service.GetFundraisingProgress(goal, raised);
            ShowFundResult("✅ " + result, true);
        }

        // Calls GetFundraisingStatus
        protected void btnGetStatus_Click(object sender, EventArgs e)
        {
            double goal, raised;

            if (!double.TryParse(txtGoal.Text.Trim(), out goal) || goal <= 0)
            {
                ShowFundResult("⚠️ Please enter a valid fundraising goal.", false);
                return;
            }

            if (!double.TryParse(txtRaised.Text.Trim(), out raised) || raised < 0)
            {
                ShowFundResult("⚠️ Please enter a valid amount raised.", false);
                return;
            }

            FundraisingService service = new FundraisingService();
            string result = service.GetFundraisingStatus(goal, raised);
            ShowFundResult("✅ " + result, true);
        }


        protected void btnHashMe_Click(object sender, EventArgs e)
        {
            string hash = txtHashMe.Text.Trim();
            if (string.IsNullOrEmpty(hash))
            {
                lblHashed.Text = "Enter a string to hash.";
                pnlHashed.Visible = true;
                return;
            }         
            lblHashed.Text = CryptoUtil.hashMe(hash);
            pnlHashed.Visible = true;
        }

        // ===================== HELPERS =====================

        private void ShowSwimResult(string message, bool success)
        {
            pnlSwimResult.Visible = true;
            lblSwimResult.Text = message;
            lblSwimResult.ForeColor = success ?
                System.Drawing.Color.DarkGreen : System.Drawing.Color.Red;
        }

        private void ShowPracticeResult(string message)
        {
            pnlPracticeResult.Visible = true;
            lblPracticeResult.Text = message;
            lblPracticeResult.ForeColor = System.Drawing.Color.DarkGreen;
        }

        private void ShowFundResult(string message, bool success)
        {
            pnlFundResult.Visible = true;
            lblFundResult.Text = message;
            lblFundResult.ForeColor = success ?
                System.Drawing.Color.DarkGreen : System.Drawing.Color.Red;
        }
    }
}