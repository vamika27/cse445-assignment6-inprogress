using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Web.Security;
using System.Xml.Schema;
using System.IO;

namespace Assignment6
{
    public partial class Default : System.Web.UI.Page
    {
        DateTime currentDate;
        string dateString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check login cookie and update Login/Logout button
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (loginCookie != null && !string.IsNullOrEmpty(loginCookie["Username"]))
            {
                string username = loginCookie["Username"];
                string role = loginCookie["Role"];
                lblMessage.Text = string.Format(
                    "✅ Logged in as <strong>{0}</strong> ({1})",
                    username, role);
                btnLogIn.Text = "Log Out";
            }
            else
            {
                lblMessage.Text = "";
                btnLogIn.Text = "Log In";
            }

            // Display application state values set by Global.asax
            lblAppName.Text = Application["AppName"] != null ?
                Application["AppName"].ToString() : "N/A";
            lblStartTime.Text = Application["AppStartTime"] != null ?
                Application["AppStartTime"].ToString() : "N/A";
            lblVisitors.Text = Application["TotalVisitors"] != null ?
                Application["TotalVisitors"].ToString() : "0";

            btnFullEventList.Visible = false;
            btnCloseEventList.Visible = false;

            LoadEvents();
            TableFundraiser();

            string currEvent = "200 Freestyle";
            string nxtEvent = "200 Individual Medley";
            getCompFilteredEvents(currEvent, nxtEvent);

            DateTime currentDate = new DateTime(2045, 01, 23);
            DateTime compDate = new DateTime(2045, 01, 23);
            if (currentDate == compDate)
            {
                btnFullEventList.Visible = true;
            }
            string dateString = currentDate.ToString("MM:dd:yyyy");

            if (!this.IsPostBack)
            {
                string xmlPath = Server.MapPath("~/App_Data/Schedule.xml");
                XDocument xmlDoc = XDocument.Load(xmlPath);

                var schedule = xmlDoc.Descendants("Schedule").FirstOrDefault(s =>
                    (string)s.Attribute("date") == dateString);
                if (schedule != null)
                {
                    var fullEvent = schedule.Descendants("Event").Select(fe => new {
                        EventName = (string)fe.Element("EventName"),
                        Competitor = string.Join(", ", fe.Elements("Competitor").Select(t => t.Value)),
                        Backup = string.Join(", ", fe.Elements("Backup").Select(t => t.Value))
                    }).ToList();
                    FullEventList.DataSource = fullEvent;
                    FullEventList.DataBind();

                    var practice = schedule.Descendants("Practice").Select(p => new
                    {
                        Day = (string)p.Element("Day"),
                        Time = $"{p.Element("StartTime")?.Value} - {p.Element("EndTime")?.Value}",
                        Activities = (string)p.Element("Activities"),
                        Coach = (string)p.Element("Coach"),
                        Pool = (string)p.Element("Pool")
                    }).ToList();
                    Practice.DataSource = practice;
                    Practice.DataBind();
                }
            }
        }

        void getCompFilteredEvents(string curr, string nxt)
        {
            string xmlPath = Server.MapPath("~/App_Data/Schedule.xml");
            XDocument xmlDoc = XDocument.Load(xmlPath);

            var schedule = xmlDoc.Descendants("Schedule").FirstOrDefault(s =>
                (string)s.Attribute("date") == dateString);

            if (schedule != null)
            {
                var currentEvent = schedule.Descendants("Event").Where(e =>
                    (string)e.Element("EventName") == curr).Select(ce => new
                    {
                        EventName = (string)ce.Element("EventName"),
                        Competitor = string.Join(", ", ce.Elements("Competitor").Select(t => t.Value)),
                        Backup = string.Join(", ", ce.Elements("Backup").Select(t => t.Value))
                    }).ToList();
                CurrentEvent.DataSource = currentEvent;
                CurrentEvent.DataBind();

                var nextEvent = schedule.Descendants("Event").Where(es =>
                    (string)es.Element("EventName") == nxt).Select(ne => new
                    {
                        EventName = (string)ne.Element("EventName"),
                        Competitor = string.Join(", ", ne.Elements("Competitor").Select(t => t.Value)),
                        Backup = string.Join(", ", ne.Elements("Backup").Select(t => t.Value))
                    }).ToList();
                NextEvent.DataSource = nextEvent;
                NextEvent.DataBind();
            }
        }

        public void LoadEvents()
        {
            ddListFundraisers.Items.Clear();
            CreateDrop("~/App_Data/Fundraisers.xml", "Fundraiser", ddListFundraisers);
        }

        public void CreateDrop(string path, string eve, DropDownList ddList)
        {
            string file = Server.MapPath(path);
            if (!File.Exists(file))
                return;

            XmlDocument document = new XmlDocument();
            document.Load(file);
            XmlNodeList nodelist = document.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodelist)
            {
                string d = node["Date"]?.InnerText ?? "";
                string t = node["Time"]?.InnerText ?? "";
                string l = node["Location"]?.InnerText ?? "";
                string p = node["Purpose"]?.InnerText ?? "";
                ddList.Items.Add(new ListItem(
                    $"{eve} on {d} at {t} at {l}",
                    $"{eve};{d};{t};{l};{p}"));
            }
        }

        protected void btnMember_Click(object sender, EventArgs e)
        {
            // Only allow if logged in as Member, otherwise go to login
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (loginCookie != null && loginCookie["Role"] == "Member")
                Response.Redirect("MemberPage.aspx");
            else
                Response.Redirect("LoginPage.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            // Only allow if logged in as Staff, otherwise go to login
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (loginCookie != null && loginCookie["Role"] == "Staff")
                Response.Redirect("StaffPage.aspx");
            else
                Response.Redirect("LoginPage.aspx");
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            // Toggle Login/Logout based on cookie
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (loginCookie != null && !string.IsNullOrEmpty(loginCookie["Username"]))
            {
                // User is logged in — log them out
                HttpCookie expiredCookie = new HttpCookie("SwimClubLogin");
                expiredCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(expiredCookie);

                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();

                // Redirect back to home as guest
                Response.Redirect("Default.aspx");
            }
            else
            {
                // Not logged in — go to login page
                Response.Redirect("LoginPage.aspx");
            }
        }

        protected void btnAllEvents_Click(object sender, EventArgs e)
        {
            btnFullEventList.Visible = false;
            btnCloseEventList.Visible = true;
            fulleEventListPanel.Visible = true;
        }

        protected void btnCloseEvents_Click(object sender, EventArgs e)
        {
            fulleEventListPanel.Visible = false;
        }

        protected void btnPledgeFund_Click(object sender, EventArgs e)
        {
            // Validate amount
            string amountText = txtFundAmount.Text.Trim();
            decimal amount;
            if (string.IsNullOrEmpty(amountText) ||
                !decimal.TryParse(amountText, out amount) || amount <= 0)
            {
                lblMessage.Text = "⚠️ Please enter a valid pledge amount.";
                return;
            }

            // Validate fundraiser selected
            if (ddListFundraisers.Items.Count == 0 ||
                string.IsNullOrEmpty(ddListFundraisers.SelectedValue))
            {
                lblMessage.Text = "⚠️ No fundraiser selected.";
                return;
            }

            // Parse fundraiser details from dropdown value
            string[] parts = ddListFundraisers.SelectedValue.Split(';');
            string fundraiserDate = parts.Length > 1 ? parts[1] : "";
            string fundraiserTime = parts.Length > 2 ? parts[2] : "";
            string fundraiserLocation = parts.Length > 3 ? parts[3] : "";
            string fundraiserPurpose = parts.Length > 4 ? parts[4] : "";

            // Get username from cookie or use Guest
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            string username = (loginCookie != null &&
                !string.IsNullOrEmpty(loginCookie["Username"]))
                ? loginCookie["Username"] : "Guest";

            // Save pledge to Pledges.xml
            string pledgeFile = Server.MapPath("~/App_Data/Pledges.xml");
            XmlDocument doc = new XmlDocument();

            if (File.Exists(pledgeFile))
            {
                doc.Load(pledgeFile);
            }
            else
            {
                XmlElement root = doc.CreateElement("Pledges");
                doc.AppendChild(root);
            }

            // Build pledge node
            XmlElement pledge = doc.CreateElement("Pledge");

            XmlElement userEl = doc.CreateElement("Username");
            userEl.InnerText = username;
            XmlElement dateEl = doc.CreateElement("FundraiserDate");
            dateEl.InnerText = fundraiserDate;
            XmlElement timeEl = doc.CreateElement("FundraiserTime");
            timeEl.InnerText = fundraiserTime;
            XmlElement locationEl = doc.CreateElement("FundraiserLocation");
            locationEl.InnerText = fundraiserLocation;
            XmlElement purposeEl = doc.CreateElement("FundraiserPurpose");
            purposeEl.InnerText = fundraiserPurpose;
            XmlElement amountEl = doc.CreateElement("Amount");
            amountEl.InnerText = amount.ToString("F2");
            XmlElement pledgeDateEl = doc.CreateElement("PledgeDate");
            pledgeDateEl.InnerText = DateTime.Now.ToString("yyyy-MM-dd");

            pledge.AppendChild(userEl);
            pledge.AppendChild(dateEl);
            pledge.AppendChild(timeEl);
            pledge.AppendChild(locationEl);
            pledge.AppendChild(purposeEl);
            pledge.AppendChild(amountEl);
            pledge.AppendChild(pledgeDateEl);

            doc.DocumentElement.AppendChild(pledge);
            doc.Save(pledgeFile);

            lblMessage.Text = string.Format(
                "✅ Thank you <strong>{0}</strong>! Your pledge of " +
                "<strong>${1:F2}</strong> for the fundraiser on " +
                "<strong>{2}</strong> has been recorded.",
                username, amount, fundraiserDate);

            txtFundAmount.Text = "";
        }

        public void TableFundraiser()
        {
            fundraiserTable.Rows.Clear();
            TableRow tab = new TableRow();
            tab.Cells.Add(new TableHeaderCell { Text = "Date" });
            tab.Cells.Add(new TableHeaderCell { Text = "Time" });
            tab.Cells.Add(new TableHeaderCell { Text = "Location" });
            tab.Cells.Add(new TableHeaderCell { Text = "Purpose" });
            tab.Cells.Add(new TableHeaderCell { Text = "Money Collected" });
            fundraiserTable.Rows.Add(tab);

            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            if (!File.Exists(file))
                return;

            XmlDocument document = new XmlDocument();
            document.Load(file);
            XmlNodeList nodelist = document.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodelist)
            {
                string d = node["Date"]?.InnerText ?? "";
                string t = node["Time"]?.InnerText ?? "";
                string l = node["Location"]?.InnerText ?? "";
                string p = node["Purpose"]?.InnerText ?? "";
                string m = node["MoneyCollected"]?.InnerText ?? "0";

                TableRow row = new TableRow();
                row.Cells.Add(new TableCell { Text = d });
                row.Cells.Add(new TableCell { Text = t });
                row.Cells.Add(new TableCell { Text = l });
                row.Cells.Add(new TableCell { Text = p });
                row.Cells.Add(new TableCell { Text = "$" + m });
                fundraiserTable.Rows.Add(row);
            }
        }
    }
}