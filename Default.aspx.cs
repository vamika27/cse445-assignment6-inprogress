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

            //string currEvent = Application["CurrentEvent"] as string ?? "None";
            //string nxtEvent = Application["NextEvent"] as string ?? "None";

            string currEvent = "200 Freestyle";
            string nxtEvent = "200 Individual Medley";

            getCompFilteredEvents(currEvent, nxtEvent);

            //DateTime currentDate = DateTime.Now;
            //DateTime currentDate = new DateTime(2020, 01, 23);
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

                var schedule = xmlDoc.Descendants("Schedule").FirstOrDefault(s => (string)s.Attribute("date") == dateString);
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
                    (string)es.Element("EventName") == curr).Select(ne => new
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
        protected void btnMember_Click(object sender, EventArgs e)
        {
            // Redirects to Member page (relative path — no hardcoded URLs)
            Response.Redirect("MemberPage.aspx");
        }

        protected void btnStaff_Click(object sender, EventArgs e)
        {
            // Redirects to Staff page (relative path — no hardcoded URLs)
            Response.Redirect("StaffPage.aspx");
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            // Redirects to Staff page (relative path — no hardcoded URLs)
            Response.Redirect("LoginPage.aspx");

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
    }
}
