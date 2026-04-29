<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Assignment6.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Swim Club - Assignment 6</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; background-color: #f0f8ff; }
        h1 { color: #003366; }
        h2 { color: #005599; }
        table { border-collapse: collapse; width: 100%; margin-top: 10px; }
        th { background-color: #003366; color: white; padding: 8px; }
        td { border: 1px solid #ccc; padding: 8px; vertical-align: top; }
        tr:nth-child(even) { background-color: #e8f0fe; }
        .btn { background-color: #003366; color: white; padding: 8px 16px;
               border: none; cursor: pointer; margin: 5px; border-radius: 4px; width: 200px;}
        .btn:hover { background-color: #005599; }
        .nav { 
               display:flex;
               justify-content: space-between;
               margin: 20px 0; 
        }
        .deployment { background-color: #ffffcc; padding: 10px;
                      border: 1px solid #cccc00; margin-bottom: 20px; }
        .result-box { background-color: #e8f0fe; border: 1px solid #003366;
                      padding: 15px; margin-top: 10px; border-radius: 4px; }
        .fullEvent {
            margin: 20px 0; 
            display:flex;
            justify-content: center;
            align-content: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <h1>🏊 ASU Swim Club Web Application</h1>
        <p>This application manages membership, events, scheduling, and finances for the ASU Swim Club.</p>

        <!-- Deployment URL -->
        <div class="deployment">
            <strong>Deployment URL:</strong>
            http://webstrarportal-env.eba-uzcvm8rb.us-west-2.elasticbeanstalk.com/sites/website147/Page0/Default.aspx
        </div>

        <!-- Navigation Buttons -->
        <div class="nav">
            <asp:Button ID="btnMember" runat="server" Text="Member Page" CssClass="btn" OnClick="btnMember_Click" />
            <asp:Button ID="btnStaff" runat="server" Text="Staff Page" CssClass="btn" OnClick="btnStaff_Click" />
            <asp:Button ID="btnLogIn" runat="server" Text="Log In" CssClass="btn" OnClick="btnLogIn_Click" />
        </div>

        <hr />
        <!-- Today's Schedule -->
        <h2>Today's Schedule Is:</h2>
        <div class="schedule">
            <!-- These elements may need to be hosted seperately so that the schedule can change from competition to practice 
                and what competition event it is based on a button press in staff-->
              <asp:GridView ID="CurrentEvent" runat="server" AutoGenerateColumns="false">
                 <Columns>
                     <asp:BoundField DataField="EventName" HeaderText="Current Event" />
                     <asp:BoundField DataField="Competitor" HeaderText="Comptetior" />
                     <asp:BoundField DataField="Backup" HeaderText="Backup" />
                 </Columns>
             </asp:GridView>

             <asp:GridView ID="NextEvent" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="EventName" HeaderText="Next Event" />
                    <asp:BoundField DataField="Competitor" HeaderText="Comptetior" />
                    <asp:BoundField DataField="Backup" HeaderText="Backup" />
                </Columns>
            </asp:GridView>

             <div class="fullEvent">
                <asp:Button ID="btnFullEventList" runat="server" Text="All Events" CssClass="btn" OnClick="btnAllEvents_Click" Visbile="false"/>
             </div>
            <asp:Panel ID="fulleEventListPanel" runat="server" Visible="false">
                <asp:GridView ID="FullEventList" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="EventName" HeaderText="Event" />
                        <asp:BoundField DataField="Competitor" HeaderText="Comptetior" />
                        <asp:BoundField DataField="Backup" HeaderText="Backup" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
             
            <div class="fullEvent">
               <asp:Button ID="btnCloseEventList" runat="server" Text="Close Events" CssClass="btn" OnClick="btnCloseEvents_Click" Visbile="false"/>
            </div>

             <asp:GridView ID="Practice" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Day" HeaderText="Today" />
                    <asp:BoundField DataField="Time" HeaderText="Time" />
                    <asp:BoundField DataField="Activities" HeaderText="Activities" />
                    <asp:BoundField DataField="Coach" HeaderText="Coach" />
                    <asp:BoundField DataField="Pool" HeaderText="Pool" />
                </Columns>
            </asp:GridView>
             
        </div>

        <!-- Fundraisers -->
        <h2>Current Fundraisers:</h2>
        <div class="fundraiser">
            <!-- These elements may need to be hosted seperately so that the fundraiser can have items added and removed -->
            <asp:Table ID="fundraiserTable" runat="server" CssClass="directory-table">
                <asp:TableRow>
                    <asp:TableHeaderCell>Date</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Time</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Purpose</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Money Collected</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>  
        <div>
            <asp:TextBox ID="txtFundAmount" runat="server"/>
            <asp:DropDownList ID="ddListFundraisers" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnPledgeFund" runat="server" Text="Pledge Money" CssClass="btn" OnClick="btnPledgeFund_Click" />
        </div> 

        <hr />

        <!-- Application Description -->
        <h2>About This Application</h2>
        <p>
            The ASU Swim Club app allows swimmers to sign up for events, propose fundraisers,
            and track earnings. Staff (coaches/captains) can manage events, view rosters, and
            oversee club finances. The public can view swim results, event times, and schedules.
        </p>

        <h2>How to Test This Application</h2>
        <ul>
            <li>Use the TryIt buttons in the Service Directory below to test each component.</li>
            <li>Sign up as a member using the Sign Up control on the Member page.</li>
            <li>Log in as Staff using: <strong>Username: TA / Password: Cse445!</strong></li>
        </ul>

        <hr />

        <!-- Service Directory Table -->
        <h2>Service Directory</h2>
        <asp:Table ID="tblDirectory" runat="server" CssClass="directory-table">
            <asp:TableRow>
                <asp:TableHeaderCell>Provider</asp:TableHeaderCell>
                <asp:TableHeaderCell>Component Type</asp:TableHeaderCell>
                <asp:TableHeaderCell>Description</asp:TableHeaderCell>
                <asp:TableHeaderCell>Inputs / Output</asp:TableHeaderCell>
                <asp:TableHeaderCell>Used In</asp:TableHeaderCell>
                <asp:TableHeaderCell>TryIt</asp:TableHeaderCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Vamika</asp:TableCell>
                <asp:TableCell>Cookie</asp:TableCell>
                <asp:TableCell>Stores user profile (name, email, preferred event) in a browser cookie across sessions.</asp:TableCell>
                <asp:TableCell>Input: Name, Email, Event preference / Output: Cookie stored and retrieved</asp:TableCell>
                <asp:TableCell>CookieDemo.aspx</asp:TableCell>
                <asp:TableCell><a href="CookieDemo.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Vamika</asp:TableCell>
                <asp:TableCell>User Control (.ascx)</asp:TableCell>
                <asp:TableCell>Sign Up control — allows new swimmers to register with name, email, password, and preferred swim event.</asp:TableCell>
                <asp:TableCell>Input: Name, Email, Password, Event / Output: Confirmation message</asp:TableCell>
                <asp:TableCell>SignUpDemo.aspx</asp:TableCell>
                <asp:TableCell><a href="SignUpDemo.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Vamika</asp:TableCell>
                <asp:TableCell>ASMX Web Service</asp:TableCell>
                <asp:TableCell>SwimEventService — returns swim event results and calculates points for a swimmer based on their time.</asp:TableCell>
                <asp:TableCell>Input: SwimmerName (string), EventName (string), Time (double) / Output: Points (int) or Result Summary (string)</asp:TableCell>
                <asp:TableCell>TryItServices.aspx</asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Vamika</asp:TableCell>
                <asp:TableCell>ASMX Web Service</asp:TableCell>
                <asp:TableCell>PracticeService — returns the practice schedule for a given day of the week.</asp:TableCell>
                <asp:TableCell>Input: Day (string) / Output: Schedule details (string)</asp:TableCell>
                <asp:TableCell>TryItServices.aspx</asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Vamika</asp:TableCell>
                <asp:TableCell>ASMX Web Service</asp:TableCell>
                <asp:TableCell>FundraisingService — tracks fundraising goal vs. amount raised and returns progress percentage and status.</asp:TableCell>
                <asp:TableCell>Input: Goal (double), Raised (double) / Output: Progress % and Status (string)</asp:TableCell>
                <asp:TableCell>TryItServices.aspx</asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Brandon</asp:TableCell>
                <asp:TableCell>Dynamic-link Libary (DLL)</asp:TableCell>
                <asp:TableCell>CryptoUtil — implements SHA-256 hashing on passwords before storing them in Staff.xml or Member.xml.</asp:TableCell>
                <asp:TableCell>Input: Password (string) <br /> Output: SHA-256 hash (string) </asp:TableCell>
                <asp:TableCell>StaffPage.aspx: Account Creation LoginPage.aspx: Login TryItServices.aspx: Hash Test
                </asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Brandon</asp:TableCell>
                <asp:TableCell>ASPX Web Service</asp:TableCell>
                <asp:TableCell>Account Management via XML — Create member and staff accounts, hash passwords, prevents duplicate usernames by appending numbers, and remove accounts.</asp:TableCell>
                <asp:TableCell>Input: Name, Password, Role (strings) <br /> Output: Staff.xml and Member.xml in App_Data</asp:TableCell>
                <asp:TableCell>StaffPage.aspx</asp:TableCell>
                <asp:TableCell><a href="StaffPage.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Brandon</asp:TableCell>
                <asp:TableCell>ASPX Web Service / XML</asp:TableCell>
                <asp:TableCell>Event Scheduling via XML  — Add and remove competitions, practices, and fundraisers.
                </asp:TableCell>
                <asp:TableCell>Input: Date, Time, Location, and Purpose applicable <br /> Output: Competitions.xml, Practices.xml, and Fundraisers.xml in App_Data</asp:TableCell>
                <asp:TableCell>StaffPage.aspx: <br /> Default.aspx: Display Fundraiser</asp:TableCell>
                <asp:TableCell><a href="StaffPage.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Brandon</asp:TableCell>
                <asp:TableCell>ASPX Web Service / XML</asp:TableCell>
                <asp:TableCell>Money Management via XML  —  Add money, mark expenditure, calculate total funds, display fundraisers, and accept or deny pledges.</asp:TableCell>
                <asp:TableCell>Input: fundraiser selection, amount, pledge selection <br /> Output: Fundraisers.xml and Pledges.xml updated; total funds displayed</asp:TableCell>
                <asp:TableCell>StaffPage.aspx: Handles funds, accept/deny Default.aspx: Displays Pledges  <br /> MemberPage: Sends Pledges to Staff</asp:TableCell>
                <asp:TableCell><a href="StaffPage.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Xandra</asp:TableCell>
                <asp:TableCell>WSDL Service / XML</asp:TableCell>
                <asp:TableCell>ConvertCompetitionTime: converts the time a user got from one type of pool to a different size. </asp:TableCell>
                <asp:TableCell>Input: Time (string), fromUnit (string), toUnit (string) / Output: Time (string) </asp:TableCell>
                <asp:TableCell>TryItServices.aspx</asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Xandra</asp:TableCell>
                <asp:TableCell>Global (.asax)</asp:TableCell>
                <asp:TableCell>CurrentViewers - counts how many people currently have the site open</asp:TableCell>
                <asp:TableCell>Input:  / Output:</asp:TableCell>
                <asp:TableCell>TryItServices.aspx</asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>Xandra</asp:TableCell>
                <asp:TableCell>Global (.asax)</asp:TableCell>
                <asp:TableCell>CurrentSwimmers - counts how many swimmers are currently logged in and on the site</asp:TableCell>
                <asp:TableCell>Input: Sign In/Sign Out / Output:Member Swimmers</asp:TableCell>
                <asp:TableCell>TryItServices.aspx</asp:TableCell>
                <asp:TableCell><a href="TryItServices.aspx">TryIt →</a></asp:TableCell>
            </asp:TableRow>


        </asp:Table>

        <br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

        <!-- Application State Display (from Global.asax) -->
        <hr />
        <h2>📊 Application State (Global.asax)</h2>
        <p><em>Note: Global.asax full implementation developed by Xandra (team member).</em></p>
        <div class="result-box" style="background-color:#ffffcc; border:1px solid #cccc00;
                                       padding:15px; margin-top:10px;">
            <strong>App Name:</strong>
            <asp:Label ID="lblAppName" runat="server" /><br />
            <strong>App Start Time:</strong>
            <asp:Label ID="lblStartTime" runat="server" /><br />
            <strong>Total Visitors This Session:</strong>
            <asp:Label ID="lblVisitors" runat="server" />
        </div>

    </form>
</body>
</html>
