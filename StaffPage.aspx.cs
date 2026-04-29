using Assignment6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace Assignment6
{
    public partial class StaffPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session["AccountType"] = "Staff"; // DEBUG. STAFF WONT WORK UNTIL LOGIN EXISTS
            if (!IsPostBack)
            {
                // on first load load in our comp/prac/fund. 
                LoadEvents();
                TableFundraiser();

            }
            if (Session["AccountType"] != null) // if not logged in
            {

                string acctype = Session["AccountType"].ToString(); // in login, we'll have to set the user into the role "Staff" or "Member" for that session

                switch (acctype)
                {
                    case "Staff":
                        // passes through but can have extra capabilities
                        break;
                    case "Member":
                        // redirect to member page
                        Response.Redirect("MemberPage.aspx");
                        break;
                    default:
                        // basic response is send them to login
                        Response.Redirect("LoginPage.aspx");
                        return;
                }
            }
            else
            {
                Response.Redirect("LoginPage.aspx");
                return;
            }
        }

        public void LoadEvents()
        {
            eventList.Items.Clear(); // clear the list
            ddListEvents.Items.Clear();
            ddListFundraisers.Items.Clear();
            CreateDrop("~/App_Data/Competitions.xml", "Competition", ddListEvents); // create each part of the dropdown
            //CreateDrop("~/App_Data/Practices.xml", "Practice");
            CreateDrop("~/App_Data/Fundraisers.xml", "Fundraiser", eventList);
            CreateDrop("~/App_Data/Fundraisers.xml", "Fundraiser", ddListFundraisers);
            //CreateDropEvents("~App_Data/Schedule.xml", eventDropDown);

        }

        public void CreateDrop(string path, string eve, DropDownList ddList)
        {
            string file = Server.MapPath(path);

            if (!File.Exists(file))
            {
                return; // if the file doesnt exist, skip this
            }
            XmlDocument document = new XmlDocument();
            document.Load(file); // load
            XmlNodeList nodelist = document.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodelist)
            {
                string d = node["Date"]?.InnerText ?? ""; // for each, if they're null just put in ""
                string t = node["Time"]?.InnerText ?? "";
                string l = node["Location"]?.InnerText ?? "";
                string p = node["Purpose"]?.InnerText ?? "";

                ddList.Items.Add(new ListItem($"{eve} on {d} at {t} at {l}", $"{eve};{d};{t};{l};{p}"));
            }
        }
        /*public void CreateDropEvents(string path, DropDownList ddList)
        {
            string xmlPath = Server.MapPath(path);
            XDocument xmlDoc = XDocument.Load(xmlPath);
            var eventList = xmlDoc.Descendants("Event").Select(ed => new {
                EventName = (string)ed.Element("EventName")}).ToList();
            ddList.DataSource = eventList;
            ddList.DataBind();

        }
        */
        protected void btnHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnLogOut_Click(object sender, EventArgs e)
        {

            FormsAuthentication.SignOut(); // clears the cookie
            Session.Abandon(); // probably not needed since FormsAuthetication exist
            Session.Clear(); // also probably not needed
            Response.Redirect("LoginPage.aspx");
        }

        protected void btnScheduleCompetition_Click(object sender, EventArgs e)
        {
            AddCompetition(txtDateComp.Text, txtTimeComp.Text, txtLocationComp.Text);
            LoadEvents(); // update dropdown
        }

        protected void btnSchedulePractice_Click(object sender, EventArgs e) // date time location
        {
            AddPractice(txtDatePractice.Text, txtTimePractice.Text, txtLocationPractice.Text, txtPurposePractice.Text);
            LoadEvents(); // update dropdown
        }

        protected void btnScheduleFundraiser_Click(object sender, EventArgs e)
        {
            AddFundraiser(txtDateFund.Text, txtTimeFund.Text, txtLocationFund.Text, txtPurposeFund.Text);
            LoadEvents(); // update dropdown
            TableFundraiser();
        }


        public void AddCompetition(string date, string time, string location)
        {
            string file = Server.MapPath("~/App_Data/Competitions.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file)) // if there is a file already, we can just load it
            {
                document.Load(file);
            }
            else
            {
                XmlElement day = document.CreateElement("Competitions"); // start building the xml, the root
                document.AppendChild(day); // put the root into the xml
            }
            XmlElement comp = document.CreateElement("Competition"); // create a practice date, the node
            // create date/time/loc/purpose in the practice node in the event tree
            XmlElement dat = document.CreateElement("Date");
            XmlElement tim = document.CreateElement("Time");
            XmlElement loc = document.CreateElement("Location");
            // populate them with the given info
            dat.InnerText = date;
            tim.InnerText = time;
            loc.InnerText = location;
            // append them into the node
            comp.AppendChild(dat);
            comp.AppendChild(tim);
            comp.AppendChild(loc);
            // append the node to the root
            document.DocumentElement.AppendChild(comp);
            // save it
            document.Save(file);
        }

        public void AddPractice(string date, string time, string location, string purpose)
        {
            string file = Server.MapPath("~/App_Data/Practices.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file)) // if there is a file already, we can just load it
            {
                document.Load(file);
            }
            else
            {
                XmlElement day = document.CreateElement("Practices"); // start building the xml, the root
                document.AppendChild(day); // put the root into the xml
            }
            XmlElement prac = document.CreateElement("Practice"); // create a practice date, the node
            // create date/time/loc/purpose in the practice node in the event tree
            XmlElement dat = document.CreateElement("Date");
            XmlElement tim = document.CreateElement("Time");
            XmlElement loc = document.CreateElement("Location");
            XmlElement pur = document.CreateElement("Purpose");
            // populate them with the given info
            dat.InnerText = date;
            tim.InnerText = time;
            loc.InnerText = location;
            pur.InnerText = purpose;
            // append them into the node
            prac.AppendChild(dat);
            prac.AppendChild(tim);
            prac.AppendChild(loc);
            prac.AppendChild(pur);
            // append the node to the root
            document.DocumentElement.AppendChild(prac);
            // save it
            document.Save(file);
        }

        public void AddFundraiser(string date, string time, string location, string purpose)
        {
            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file)) // if there is a file already, we can just load it
            {
                document.Load(file);
            }
            else
            {
                XmlElement day = document.CreateElement("Fundraisers"); // start building the xml, the root
                document.AppendChild(day); // put the root into the xml
            }
            XmlElement fund = document.CreateElement("Fundraiser"); // create a practice date, the node
            // create date/time/loc/purpose in the practice node in the event tree
            XmlElement dat = document.CreateElement("Date");
            XmlElement tim = document.CreateElement("Time");
            XmlElement loc = document.CreateElement("Location");
            XmlElement pur = document.CreateElement("Purpose");
            // populate them with the given info
            dat.InnerText = date;
            tim.InnerText = time;
            loc.InnerText = location;
            pur.InnerText = purpose;
            // append them into the node
            fund.AppendChild(dat);
            fund.AppendChild(tim);
            fund.AppendChild(loc);
            fund.AppendChild(pur);
            // append the node to the root
            document.DocumentElement.AppendChild(fund);
            // save it
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
            txtPassword.Text = ""; // rest fields
        }

        public void CreateDefaultAccount(string un, string pw, string ac)
        {
            string file = Server.MapPath("~/App_Data/Users.xml");
            XmlDocument document = new XmlDocument();
            if (File.Exists(file)) // if there is a file already, we can just load it
            {
                document.Load(file);
            }
            else
            {
                XmlElement us = document.CreateElement("Users"); // start building the xml, the root
                document.AppendChild(us); // put the root into the xml
            }
            XmlElement user = document.CreateElement("User"); // copied the other xml code
            XmlElement username = document.CreateElement("Username");
            XmlElement password = document.CreateElement("Password");
            XmlElement accounttype = document.CreateElement("AccountType");

            // populate them with the given info
            username.InnerText = un;
            password.InnerText = CryptoUtil.hashMe(pw);
            accounttype.InnerText = ac;
            // append them into the node
            user.AppendChild(username);
            user.AppendChild(password);
            user.AppendChild(accounttype);
            // append the node to the root
            document.DocumentElement.AppendChild(user);
            // save it
            document.Save(file);
        }

        protected void btnRemoveMember_Click(object sender, EventArgs e) // type in the username and click
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
            // removes that node

            if (remove != null) // remove and save
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }

        }

        protected void btnAddFund_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(eventList.SelectedValue)) // dont do anything if there's no dropdowns yet
            {
                return;
            }
            string[] choose = eventList.SelectedValue.Split(';');
            // concats based on ; and figures out the inputs for the add/removes
            string ev = choose[0];
            if (ev == "Fundraiser")
            {
                AddFunds(choose[1], choose[2], choose[3], choose[4], txtFundAmount.Text);
                txtFundAmount.Text = ""; // clear field
                TableFundraiser();
            }
        }

        public void AddFunds(string date, string time, string location, string purpose, string amount)
        {
            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            XmlDocument document = new XmlDocument();
            if (!File.Exists(file))
            {
                return;
            }
            document.Load(file);
            string nodes = $"/Fundraisers/Fundraiser[Date='{date}' and Time='{time}' and Location='{location}' and Purpose='{purpose}']";
            XmlNode fund = document.SelectSingleNode(nodes);

            if (fund != null) // if fund, continue
            {
                XmlNode m = fund["MoneyCollected"];
                if (m == null) // if no money, create a base value of 0
                {
                    m = document.CreateElement("MoneyCollected");
                    m.InnerText = "0";
                    fund.AppendChild(m);
                }
                // turn the string to decimal
                decimal current = decimal.Parse(m.InnerText);
                m.InnerText = (current + decimal.Parse(amount)).ToString();
                // save it
                document.Save(file);
            }
        }

        protected void btnAddExpenditure_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(eventList.SelectedValue)) // dont do anything if there's no dropdowns yet
            {
                return;
            }
            string[] choose = eventList.SelectedValue.Split(';');
            // concats based on ; and figures out the inputs for the add/removes
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
            LoadEvents(); // update dropdown
            TableFundraiser();
        }

        public void TableFundraiser()
        {
            // clear the existing table
            fundraiserTable.Rows.Clear();
            // create a table
            TableRow tab = new TableRow();
            tab.Cells.Add(new TableHeaderCell { Text = "Date" });
            tab.Cells.Add(new TableHeaderCell { Text = "Time" });
            tab.Cells.Add(new TableHeaderCell { Text = "Location" });
            tab.Cells.Add(new TableHeaderCell { Text = "Purpose" });
            tab.Cells.Add(new TableHeaderCell { Text = "Money Collected" });
            fundraiserTable.Rows.Add(tab);


            string file = Server.MapPath("~/App_Data/Fundraisers.xml");

            if (!File.Exists(file))
            {
                return; // if the file doesnt exist, skip this
            }
            XmlDocument document = new XmlDocument();
            document.Load(file); // load
            XmlNodeList nodelist = document.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodelist)
            {
                string d = node["Date"]?.InnerText ?? ""; // for each, if they're null just put in ""
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
            string nodes = $"/Competitions/Competition[Date='{date}' and Time='{time}' and Location='{location}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            // removes that node

            if (remove != null) // remove and save
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
            string nodes = $"/Practices/Practice[Date='{date}' and Time='{time}' and Location='{location}' and Purpose='{purpose}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            // removes that node

            if (remove != null) // remove and save
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
            string nodes = $"/Fundraisers/Fundraiser[Date='{date}' and Time='{time}' and Location='{location}' and Purpose='{purpose}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            // removes that node

            if (remove != null) // remove and save
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }

        }
        protected void btnSetEvent_Click(object sender, EventArgs e)
        {
            int currID = 0;
            string currEvent = "None";
            string nxtEvent = "None(Last Event)";
            currID = eventDropDown.SelectedIndex;
            currEvent = eventDropDown.SelectedItem.Value;

            if (currID < eventDropDown.Items.Count - 1)
            {
                nxtEvent = eventDropDown.Items[currID + 1].Value;
            }

            //Application["CurrentEvent"] = currEvent;
            //Application["NextEvent"] = nxtEvent;
            //Session["CurrentEvent"] = eventDropDown.SelectedValue;
        }
    }
}