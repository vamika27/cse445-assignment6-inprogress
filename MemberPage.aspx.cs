using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using System.Web.Security;
using System.Xml.Schema;
using System.IO;

namespace Assignment6
{
    public partial class MemberPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                TableFundraiser();
                string xmlPath = Server.MapPath("~/App_Data/MemberData.xml");
                XDocument xmlDoc = XDocument.Load(xmlPath);

                var swimmer = xmlDoc.Descendants("Swimmer").FirstOrDefault(s => (string)s.Element("Name") == "Xandra");
                if (swimmer != null)
                {
                    var personalEventsData = swimmer.Descendants("Event").Select(ed => new {
                        EventName = (string)ed.Element("EventName"),
                        PersonalTime = string.Join(", ", ed.Elements("PersonalTime").Select(t => t.Value)),
                        BestTime = ed.Elements("PersonalTime").Select(t => t.Value).OrderBy(t => t).FirstOrDefault() ?? "N/A"
                    }).ToList();
                    MyEventTable.DataSource = personalEventsData;
                    MyEventTable.DataBind();

                    var teamEventsData = swimmer.Descendants("TeamEvent").Select(te => new
                    {
                        EventName = (string)te.Element("EventName"),
                        Role = (string)te.Element("Role"),
                        PersonalTime = string.Join(", ", te.Elements("PersonalTime").Select(t => t.Value)),
                        BestTime = te.Elements("PersonalTime").Select(t => t.Value).OrderBy(t => t).FirstOrDefault(),
                        TeamTime = string.Join(", ", te.Elements("TeamTime").Select(t => t.Value)),
                        TeamBestTime = te.Elements("TeamTime").Select(t => t.Value).OrderBy(t => t).FirstOrDefault()
                    }).ToList();
                    TeamEventTable.DataSource = teamEventsData;
                    TeamEventTable.DataBind();
                }
            }

        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            // Redirects to Default page (relative path — no hardcoded URLs)
            Response.Redirect("Default.aspx");
        }


        protected void btnLogOut_Click(object sender, EventArgs e)
        {

        }

        protected void btnEventSignUp_Click(object sender, EventArgs e)
        {

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