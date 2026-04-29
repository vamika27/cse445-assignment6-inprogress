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
            // Check cookie for Staff role — redirect to login if not Staff
            // Session["Role"] = "Staff"; // DEBUG. STAFF WONT WORK UNTIL LOGIN EXISTS
            HttpCookie loginCookie = Request.Cookies["SwimClubLogin"];
            if (loginCookie == null || loginCookie["Role"] != "Staff")
            {
                Response.Redirect("LoginPage.aspx");
                return;
            }
             
            if (!IsPostBack)
            {
                // on first load load in our comp/prac/fund. 
                LoadEvents();
                TableFundraiser();
                LoadTotal();
            }
            /* if (Session["AccountType"] != null) // if not logged in
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
                        Response.Redirect("Login.aspx");
                        return;
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
                return;
            } */
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
            LoadPledges();
            //CreateDropEvents("~App_Data/Schedule.xml", eventDropDown);

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
                string fundTime = pledge["FundraiserTime"]?.InnerText ?? "";
                string fundLocation = pledge["FundraiserLocation"]?.InnerText ?? "";

                // Text shown in dropdown
                string displayText = string.Format(
                    "{0} pledged ${1} for '{2}' on {3}",
                    username, amount, purpose, pledgeDate);

                // Value for accept/deny processing
                string value = $"{fundDate};{fundTime};{fundLocation};{purpose};{amount};{username};{pledgeDate}";

                ddListPledges.Items.Add(new ListItem(displayText, value));
            }
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

            HttpCookie loginCookie = new HttpCookie("SwimClubLogin");
            loginCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(loginCookie);

            FormsAuthentication.SignOut(); // clears the cookie
            Session.Abandon(); // probably not needed since FormsAuthetication exist
            Session.Clear(); // also probably not needed
            Response.Redirect("Default.aspx");
        }

        protected void btnScheduleCompetition_Click(object sender, EventArgs e)
        {
            AddCompetition(txtDateComp.Text, txtTimeComp.Text, txtLocationComp.Text);
            txtDateComp.Text = "";
            txtTimeComp.Text = "";
            txtLocationComp.Text = "";
            LoadEvents(); // update dropdown
        }

        protected void btnSchedulePractice_Click(object sender, EventArgs e) // date time location
        {
            AddPractice(txtDatePractice.Text, txtTimePractice.Text, txtLocationPractice.Text, txtPurposePractice.Text);
            txtDatePractice.Text = "";
            txtTimePractice.Text = "";
            txtTimePracticeEnd.Text = "";
            txtLocationPractice.Text = "";
            txtPurposePractice.Text = "";
            LoadEvents(); // update dropdown
        }

        protected void btnScheduleFundraiser_Click(object sender, EventArgs e)
        {
            AddFundraiser(txtDateFund.Text, txtTimeFund.Text, txtLocationFund.Text, txtPurposeFund.Text);
            txtDateFund.Text = "";
            txtTimeFund.Text = "";
            txtLocationFund.Text = "";
            txtPurposeFund.Text = "";
            LoadEvents(); // update dropdown
            TableFundraiser();
            LoadTotal();
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

        protected void btnAcceptEvent_Click(object sender, EventArgs e) // accept event (remove)
        {
            if (string.IsNullOrEmpty(ddListEventsReq.SelectedValue)) // dont do anything if there's no dropdowns yet
            {
                return;
            }
            string tmp = ddListEventsReq.SelectedValue;
        }
        protected void btnDenyEvent_Click(object sender, EventArgs e) // deny event (remove)
        {
            if (string.IsNullOrEmpty(ddListEventsReq.SelectedValue)) // dont do anything if there's no dropdowns yet
            {
                return;
            }
            string tmp = ddListEventsReq.SelectedValue;
            
        }

        protected void btnAcceptPledge_Click(object sender, EventArgs e) // accept pledge (remove)
        {
            if (string.IsNullOrEmpty(ddListPledges.SelectedValue)) // dont do anything if there's no dropdowns yet
            {
                return;
            }
            string[] choose = ddListPledges.SelectedValue.Split(';');
            // concats based on ; and figures out the inputs for the add/removes
            string ev = choose[0];
            AddFunds(choose[0], choose[1], choose[2], choose[3], choose[4]);
            RemovePledge(choose[5], choose[6], choose[3]); // accepts based off name/date.
            LoadPledges();
            TableFundraiser();
            LoadTotal();
        }
            
        protected void btnDenyPledge_Click(object sender, EventArgs e) // deny pledge (remove)
        {

        if (string.IsNullOrEmpty(ddListPledges.SelectedValue)) // dont do anything if there's no dropdowns yet
        {
            return;
        }
            string[] choose = ddListPledges.SelectedValue.Split(';');
            RemovePledge(choose[5], choose[6], choose[3]);
            LoadPledges();
        }

        public void RemovePledge(string un, string date, string purpose) // DOES face an issue of overlapping pledges with the same date/time
        {
            string file = Server.MapPath("~/App_Data/Pledges.xml");
            XmlDocument document = new XmlDocument();
            if (!File.Exists(file)) // if there is a file already, we can just load it
            {
                return;
            }
            document.Load(file);
            string nodes = $"/Pledges/Pledge[Username='{un}' and PledgeDate='{date}' and FundraiserPurpose='{purpose}']";
            XmlNode remove = document.SelectSingleNode(nodes);
            // removes that node

            if (remove != null) // remove and save
            {
                remove.ParentNode.RemoveChild(remove);
                document.Save(file);
            }

        }

        protected void btnCreateDefaultAccount_Click(object sender, EventArgs e)
        {
            CreateDefaultAccount(txtName.Text, txtPassword.Text, "Member");
            txtName.Text = "";
            txtPassword.Text = ""; // rest fields
        }

        protected void btnCreateStaffAccount_Click(object sender, EventArgs e)
        {
            CreateDefaultAccount(txtName.Text, txtPassword.Text, "Staff");
            txtName.Text = "";
            txtPassword.Text = ""; // rest fields
        }

        public void CreateDefaultAccount(string un, string pw, string ac)
        {

            if (string.IsNullOrWhiteSpace(un) || string.IsNullOrWhiteSpace(pw)) // dont accept blank fields
            {
                return;
            }
            string file;
            string typ;
            if (ac == "Staff")
            {
                file = Server.MapPath("~/App_Data/Staff.xml");
                typ = "Staff";
            } else
            {
                file = Server.MapPath("~/App_Data/Member.xml");
                typ = "Member";
            }
            XmlDocument document = new XmlDocument();
            if (File.Exists(file)) // if there is a file already, we can just load it
            {
                document.Load(file);
            }
            else
            {
                XmlElement us = document.CreateElement(typ); // start building the xml, the root
                document.AppendChild(us); // put the root into the xml
            }
            XmlElement user = document.CreateElement("Member"); // copied the other xml code
            XmlElement username = document.CreateElement("Username");
            string oldname = un;
            string dupedname = oldname;
            int count = 1;
            while (document.SelectSingleNode($"/{typ}/Member[Username='{dupedname}']") != null) // while dupes exist. add 1 to the acct name until not duped
            {
                dupedname = oldname + count;
                count++;
            }
            XmlElement password = document.CreateElement("Password");
          //  XmlElement accounttype = document.CreateElement("AccountType");

            // populate them with the given info
            username.InnerText = dupedname;
            password.InnerText = CryptoUtil.hashMe(pw);
         //   accounttype.InnerText = ac;
            // append them into the node
            user.AppendChild(username);
            user.AppendChild(password);
        //    user.AppendChild(accounttype);
            // append the node to the root
            document.DocumentElement.AppendChild(user);
            // save it
            document.Save(file);
        }

        protected void btnRemoveMember_Click(object sender, EventArgs e) // type in the username and click remove
        {
            RemoveMember(txtRemoveName.Text);
            txtRemoveName.Text = "";
        }

        public void RemoveMember(string un)
        {
            string file = Server.MapPath("~/App_Data/Users.xml");
            XmlDocument document = new XmlDocument();
            if (!File.Exists(file))
            {
                return;
            }
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
                LoadTotal();
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
            decimal temp; // current value
            decimal add;

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
               
                if (!decimal.TryParse(amount.Replace("$", "").Trim(), out add)) // grabs the decimal value if you put in a $ when adding
                {
                    return;
                }
                if (!decimal.TryParse(m.InnerText.Replace("$", "").Trim(), out temp)) // same thing but for the existing value
                {
                    return;
                }
                m.InnerText = (temp + add).ToString("0.00");
                // save it
                document.Save(file);
            }
        }
        public void RemoveFunds(string date, string time, string location, string purpose, string amount)
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
            decimal temp; // current value
            decimal sub;

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

                if (!decimal.TryParse(amount.Replace("$", "").Trim(), out sub)) // grabs the decimal value if you put in a $ when adding
                {
                    return;
                }
                if (!decimal.TryParse(m.InnerText.Replace("$", "").Trim(), out temp)) // same thing but for the existing value
                {
                    return;
                }
                //  to protect against negative values in the fundraiser
                decimal check = temp - sub;
                if (check < 0)
                {
                    check = 0;
                }
                m.InnerText = check.ToString("0.00");
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
                    RemoveFunds(choose[1], choose[2], choose[3], choose[4], txtRemoveAmount.Text);
                    txtRemoveAmount.Text = "";
                    break;

            }
            LoadEvents(); // update dropdown
            TableFundraiser();
            LoadTotal();
        }

        public void LoadTotal()
        {
            string file = Server.MapPath("~/App_Data/Fundraisers.xml");
            XmlDocument document = new XmlDocument();
            if (!File.Exists(file))
            {
                txtTotalFunds.Text = "$0.00";
                return;
            }
            document.Load(file);
            decimal total = 0; // reset current value then parse it

            XmlNodeList nodelist = document.DocumentElement.ChildNodes;
            foreach (XmlNode node in nodelist)
            {
                string m = node["MoneyCollected"]?.InnerText ?? "0"; // for each, if they're null just put in "0"
                m = m.Replace("$", "").Trim(); // same as in add fund to fix decimal
                decimal amt;
                if (decimal.TryParse(m, out amt))
                {
                    total += amt;
                }
            }
            txtTotalFunds.Text = "$" + total.ToString("0.00");
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
            if (!File.Exists(file))
            {
                return; // if the file doesnt exist, skip this
            }
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
            if (!File.Exists(file))
            {
                return; // if the file doesnt exist, skip this
            }
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
            if (!File.Exists(file))
            {
                return; // if the file doesnt exist, skip this
            }
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

        protected void btnRemoveFundraiser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddListFundraisers.SelectedValue)) // dont do anything if there's no fundraisers yet
            {
                return;
            }
            string[] choose = ddListFundraisers.SelectedValue.Split(';');
            // concats based on ; and figures out the inputs for the add/removes
            string ev = choose[0];
            if (ev == "Fundraiser")
            {
                RemoveFundraiser(choose[1], choose[2], choose[3], choose[4]);

            }
            LoadEvents(); // update everything
            TableFundraiser();
            LoadTotal();

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