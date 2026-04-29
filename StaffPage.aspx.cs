using Assignment6.Models;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;

namespace Assignment6
{
    public partial class StaffPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check cookie for Staff role — redirect to login if not Staff
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (loginCookie == null || loginCookie["Role"] != "Staff")
            {
                Response.Redirect("LoginPage.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadEvents();
                TableFundraiser();
            }
        }

        public void LoadEvents()
        {
            eventList.Items.Clear();
            ddListEvents.Items.Clear();
            ddListFundraisers.Items.Clear();
            CreateDrop("~/App_Data/Competitions.xml", "Competition", ddListEvents);
            CreateDrop("~/App_Data/Fundraisers.xml", "Fundraiser", eventList);
            CreateDrop("~/App_Data/Fundraisers.xml", "Fundraiser", ddListFundraisers);

            // Load pledges from Pledges.xml into ddListPledges
            LoadPledges();
        }

        public void LoadPledges()
        {
            ddListPledges.Items.Clear();
            ddListPledges.Items.Add(new ListItem("-- Select a Pledge --", ""));

            string file = Server.MapPath("~/App_Data/Pledges.xml");
            if (!File.Exists(file))
                return;

            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNodeList pledges = doc.SelectNodes("//Pledge");

            if (pledges.Count == 0)
            {
                ddListPledges.Items.Add(new ListItem("No pledges yet", ""));
                return;
            }

            foreach (XmlNode pledge in pledges)
            {
                string username = pledge["Username"]?.InnerText ?? "Guest";
                string purpose = pledge["FundraiserPurpose"]?.InnerText ?? "";
                string amount = pledge["Amount"]?.InnerText ?? "0";
                string pledgeDate = pledge["PledgeDate"]?.InnerText ?? "";
                string fundDate = pledge["FundraiserDate"]?.InnerText ?? "";

                // Text shown in dropdown
                string displayText = string.Format(
                    "{0} pledged ${1} for '{2}' on {3}",
                    username, amount, purpose, pledgeDate);

                // Value for accept/deny processing
                string value = string.Format(
                    "{0};{1};{2};{3}",
                    username, fundDate, amount, pledgeDate);

                ddListPledges.Items.Add(new ListItem(displayText, value));
            }
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

        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            // Expire the login cookie
            HttpCookie loginCookie = new HttpCookie("SwimClubLogin");
            loginCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(loginCookie);

            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();

            // Go back to home page as guest
            Response.Redirect("Default.aspx");
        }

        protected void btnScheduleCompetition_Click(object sender, EventArgs e)
        {
            AddCompetition(txtDateComp.Text, txtTimeComp.Text, txtLocationComp.Text);
            LoadEvents();
        }

        protected void btnSchedulePractice_Click(object sender, EventArgs e)
        {
            AddPractice(txtDatePractice.Text, txtTimePractice.Text,
                        txtLocationPractice.Text, txtPurposePractice.Text);
            LoadEvents();
        }

        protected void btnScheduleFundraiser_Click(object sender, EventArgs e)
        {
            AddFundraiser(txtDateFund.Text, txtTimeFund.Text,
                          txtLocationFund.Text, txtPurposeFund.Text);
            LoadEvents();
            TableFundraiser();
        }

        public void AddCompetition(string date, string time, string location)
        {
            string file = Server.MapPath("~/App_Data/Competitions.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file))
                document.Load(file);
            else
            {
                XmlElement day = document.CreateElement("Competitions");
                document.AppendChild(day);
            }
            XmlElement comp = document.CreateElement("Competition");
            XmlElement dat = document.CreateElement("Date");
            XmlElement tim = document.CreateElement("Time");
            XmlElement loc = document.CreateElement("Location");
            dat.InnerText = date;
            tim.InnerText = time;
            loc.InnerText = location;
            comp.AppendChild(dat);
            comp.AppendChild(tim);
            comp.AppendChild(loc);
            document.DocumentElement.AppendChild(comp);
            document.Save(file);
        }

        public void AddPractice(string date, string time, string location, string purpose)
        {
            string file = Server.MapPath("~/App_Data/Practices.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file))
                document.Load(file);
            else
            {
                XmlElement day = document.CreateElement("Practices");
                document.AppendChild(day);
            }
            XmlElement prac = document.CreateElement("Practice");
            XmlElement dat = document.CreateElement("Date");
            XmlElement tim = document.CreateElement("Time");
            XmlElement loc = document.CreateElement("Location");
            XmlElement pur = document.CreateElement("Purpose");
            dat.InnerText = date;
            tim.InnerText = time;
            loc.InnerText = location;
            pur.InnerText = purpose;
            prac.AppendChild(dat);
            prac.AppendChild(tim);
            prac.AppendChild(loc);
            prac.AppendChild(pur);
            document.DocumentElement.AppendChild(prac);
            document.Save(file);
        }

        public void AddFundraiser(string date, string time, string location, string purpose)
        {
            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file))
                document.Load(file);
            else
            {
                XmlElement day = document.CreateElement("Fundraisers");
                document.AppendChild(day);
            }
            XmlElement fund = document.CreateElement("Fundraiser");
            XmlElement dat = document.CreateElement("Date");
            XmlElement tim = document.CreateElement("Time");
            XmlElement loc = document.CreateElement("Location");
            XmlElement pur = document.CreateElement("Purpose");
            dat.InnerText = date;
            tim.InnerText = time;
            loc.InnerText = location;
            pur.InnerText = purpose;
            fund.AppendChild(dat);
            fund.AppendChild(tim);
            fund.AppendChild(loc);
            fund.AppendChild(pur);
            document.DocumentElement.AppendChild(fund);
            document.Save(file);
        }

        protected void btnAllEvents_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnCreateDefaultAccount_Click(object sender, EventArgs e)
        {
            CreateDefaultAccount(txtName.Text, txtPassword.Text, "Member");
            txtName.Text = "";
            txtPassword.Text = "";
        }

        public void CreateDefaultAccount(string un, string pw, string ac)
        {
            string file = Server.MapPath("~/App_Data/Users.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file))
                document.Load(file);
            else
            {
                XmlElement us = document.CreateElement("Users");
                document.AppendChild(us);
            }
            XmlElement user = document.CreateElement("User");
            XmlElement username = document.CreateElement("Username");
            XmlElement password = document.CreateElement("Password");
            XmlElement accounttype = document.CreateElement("AccountType");
            username.InnerText = un;
            password.InnerText = CryptoUtil.hashMe(pw);
            accounttype.InnerText = ac;
            user.AppendChild(username);
            user.AppendChild(password);
            user.AppendChild(accounttype);
            document.DocumentElement.AppendChild(user);
            document.Save(file);
        }

        protected void btnRemoveMember_Click(object sender, EventArgs e)
        {
            RemoveMember(txtName.Text);
            txtName.Text = "";
            txtPassword.Text = "";
        }

        public void RemoveMember(string un)
        {
            string file = Server.MapPath("~/App_Data/Users.xml");
            XmlDocument document = new XmlDocument();
            document.Load(file);
            string nodes = $"/Users/User[Username='{un}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            if (remove != null)
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }
        }

        protected void btnAddFund_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eventList.SelectedValue))
                return;

            string[] choose = eventList.SelectedValue.Split(';');
            string ev = choose[0];
            if (ev == "Fundraiser")
            {
                AddFunds(choose[1], choose[2], choose[3], choose[4], txtFundAmount.Text);
                txtFundAmount.Text = "";
                TableFundraiser();
            }
        }

        public void AddFunds(string date, string time, string location,
                             string purpose, string amount)
        {
            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            XmlDocument document = new XmlDocument();
            if (!File.Exists(file))
                return;

            document.Load(file);
            string nodes = $"/Fundraisers/Fundraiser[Date='{date}' and Time='{time}' " +
                           $"and Location='{location}' and Purpose='{purpose}']";
            XmlNode fund = document.SelectSingleNode(nodes);

            if (fund != null)
            {
                XmlNode m = fund["MoneyCollected"];
                if (m == null)
                {
                    m = document.CreateElement("MoneyCollected");
                    m.InnerText = "0";
                    fund.AppendChild(m);
                }
                decimal current = decimal.Parse(m.InnerText);
                m.InnerText = (current + decimal.Parse(amount)).ToString();
                document.Save(file);
            }
        }

        protected void btnAddExpenditure_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eventList.SelectedValue))
                return;

            string[] choose = eventList.SelectedValue.Split(';');
            string ev = choose[0];
            switch (ev)
            {
                case "Competition":
                    RemoveCompetition(choose[1], choose[2], choose[3]);
                    break;
                case "Practice":
                    RemovePractice(choose[1], choose[2], choose[3], choose[4]);
                    break;
                case "Fundraiser":
                    RemoveFundraiser(choose[1], choose[2], choose[3], choose[4]);
                    break;
            }
            LoadEvents();
            TableFundraiser();
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

        public void RemoveCompetition(string date, string time, string location)
        {
            string file = Server.MapPath("~/App_Data/Competitions.xml");
            XmlDocument document = new XmlDocument();
            document.Load(file);
            string nodes = $"/Competitions/Competition[Date='{date}' " +
                           $"and Time='{time}' and Location='{location}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            if (remove != null)
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }
        }

        public void RemovePractice(string date, string time, string location, string purpose)
        {
            string file = Server.MapPath("~/App_Data/Practices.xml");
            XmlDocument document = new XmlDocument();
            document.Load(file);
            string nodes = $"/Practices/Practice[Date='{date}' and Time='{time}' " +
                           $"and Location='{location}' and Purpose='{purpose}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            if (remove != null)
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }
        }

        public void RemoveFundraiser(string date, string time, string location, string purpose)
        {
            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            XmlDocument document = new XmlDocument();
            document.Load(file);
            string nodes = $"/Fundraisers/Fundraiser[Date='{date}' and Time='{time}' " +
                           $"and Location='{location}' and Purpose='{purpose}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            if (remove != null)
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }
        }

        protected void btnSetEvent_Click(object sender, EventArgs e)
        {
            int currID = eventDropDown.SelectedIndex;
            string currEvent = eventDropDown.SelectedItem.Value;
            string nxtEvent = "None(Last Event)";
            if (currID < eventDropDown.Items.Count - 1)
                nxtEvent = eventDropDown.Items[currID + 1].Value;
        }
    }
}