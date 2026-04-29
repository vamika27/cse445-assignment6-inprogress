<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffPage.aspx.cs" Inherits="Assignment6.StaffPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Swim Club - Assignment 6</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 30px;
            background-color: #f0f8ff;
        }

        h1 {
            color: #003366;
        }

        h2 {
            color: #005599;
        }

        table {
            border-collapse: collapse;
            width: 100%;
            margin-top: 10px;
        }

        th {
            background-color: #003366;
            color: white;
            padding: 8px;
        }

        td {
            border: 1px solid #ccc;
            padding: 8px;
            vertical-align: top;
        }

        tr:nth-child(even) {
            background-color: #e8f0fe;
        }

        .btn {
            background-color: #003366;
            color: white;
            padding: 8px 16px;
            border: none;
            cursor: pointer;
            margin: 5px;
            border-radius: 4px;
            min-width: 200px;
            max-width: 300px;
        }

        .btn:hover {
            background-color: #005599;
        }

        .nav {
            display: flex;
            justify-content: space-between;
            margin: 20px 0;
        }

        .deployment {
            background-color: #ffffcc;
            padding: 10px;
            border: 1px solid #cccc00;
            margin-bottom: 20px;
        }

        .result-box {
            background-color: #e8f0fe;
            border: 1px solid #003366;
            padding: 15px;
            margin-top: 10px;
            border-radius: 4px;
        }

        .scheduleRow {
            background-color: #5599cc;
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 20px;
            padding: 10px;
            border: 1px solid #005599;
            border-radius: 6px;
            margin-bottom: 10px;
        }

        .currentEventRow {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 20px;
            padding: 10px;
            border: 1px solid #005599;
            border-radius: 6px;
            margin-bottom: 10px;
        }

        .formGroup {
            display: flex;
            align-items: center;
            gap: 10px;
        }

        #eventDropDown {
            padding: 6px;
            border-radius: 4px;
            border: 1px solid #005599;
            min-width: 180px;
        }

        .buttonGroup {
            display: flex;
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
            <asp:Button ID="btnHome" runat="server" Text="Home Page" CssClass="btn" OnClick="btnHome_Click" />
            <asp:Button ID="btnLogOut" runat="server" Text="Log Out" CssClass="btn" OnClick="btnLogOut_Click" />
        </div>

        <hr />
        <!-- Schedule new items-->
        <h2>Scheduling:</h2>
        <div class="scheduleRow">
            <div class="formGroup">
                <label>Date:</label>
                <asp:TextBox ID="txtDateComp" runat="server" TextMode="Date"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Time:</label>
                <asp:TextBox ID="txtTimeComp" runat="server" TextMode="Time"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Location:</label>
                <asp:TextBox ID="txtLocationComp" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="btnScheduleCompetition" runat="server" Text="Schedule Competition" CssClass="btn" OnClick="btnScheduleCompetition_Click" />
        </div>
        <div class="scheduleRow">
            <div class="formGroup">
                <label>Date:</label>
                <asp:TextBox ID="txtDatePractice" runat="server" TextMode="Date"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Time:</label>
                <asp:TextBox ID="txtTimePractice" runat="server" TextMode="Time"></asp:TextBox>
                <asp:TextBox ID="txtTimePracticeEnd" runat="server" TextMode="Time"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Location:</label>
                <asp:TextBox ID="txtLocationPractice" runat="server"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Purpose:</label>
                <asp:TextBox ID="txtPurposePractice" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="btnSchedulePractice" runat="server" Text="Schedule Practice" CssClass="btn" OnClick="btnSchedulePractice_Click" />
        </div>
        <div class="scheduleRow">
            <div class="formGroup">
                <label>Date:</label>
                <asp:TextBox ID="txtDateFund" runat="server" TextMode="Date"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Time:</label>
                <asp:TextBox ID="txtTimeFund" runat="server" TextMode="Time"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Location:</label>
                <asp:TextBox ID="txtLocationFund" runat="server"></asp:TextBox>
            </div>
            <div class="formGroup">
                <label>Purpose:</label>
                <asp:TextBox ID="txtPurposeFund" runat="server"></asp:TextBox>
            </div>
            <asp:Button ID="btnScheduleFundraiser" runat="server" Text="Schedule Fundraiser" CssClass="btn" OnClick="btnScheduleFundraiser_Click" />
        </div>
        <hr />
        <!-- Competition -->
        <h2>Competition:</h2>
        <div>
            <label>Requesting Event</label> <br />
            <asp:DropDownList ID="ddListEventsReq" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnAcceptEvent" runat="server" CssClass="btn" Text="Accept" />
            <asp:Button ID="btnDenyEvent" runat="server" CssClass="btn" Text="Deny" />
        </div>
        <div>
            <label>Assign Event</label> <br />
            <asp:DropDownList ID="ddListEvents" runat="server"> </asp:DropDownList>
            <asp:DropDownList ID="ddListDates" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnAssignCompetitor" runat="server" CssClass="btn" Text="Assign Competitor" />
            <asp:Button ID="btnAssignBackup" runat="server" CssClass="btn"  Text="Assign Backup" />
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

        <div class="currentEventRow">
            <div class="formGroup">
                <label>Current Event: </label>
            </div>
            <div class="formGroup">
                <asp:DropDownList ID="eventDropDown" runat="server"> </asp:DropDownList>
                <asp:Button ID="btnSetEvent" runat="server" Text="Set Current Event" CssClass="btn" OnClick="btnSetEvent_Click" />
            </div>
        </div>
        <hr />
        <div>
            Name:
            <asp:TextBox ID="txtName" runat="server" /><br />
            Password:
            <asp:TextBox ID="txtPassword" runat="server" /><br />
            <asp:Button ID="btnCreateDefaultAccount" runat="server" CssClass="btn" OnClick="btnCreateDefaultAccount_Click" Text="Create Default Account" />
            <asp:Button ID="btnCreateStaffAccount" runat="server" CssClass="btn" Text="Create Staff Account" />
        </div>
        <div>
            Name:
            <asp:TextBox ID="txtRemoveName" runat="server" /><br />
            <asp:Button ID="btnRemoveMembers" runat="server" CssClass="btn" OnClick="btnRemoveMember_Click" Text="Remove Member" />
        </div>
        <hr />
        <h2>Fundraising:</h2>
        <div>
            <asp:TextBox ID="txtFundAmount" runat="server"/>
            <asp:DropDownList ID="eventList" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnAddFund" runat="server" Text="Add Money to Fundraiser" CssClass="btn" OnClick="btnAddFund_Click" />
        </div> 
        <div>
            <label>Pledging Donation</label><br />
            <asp:DropDownList ID="ddListPledges" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnAcceptPledge" runat="server" CssClass="btn" Text="Accept" />
            <asp:Button ID="btDenyPledge" runat="server" CssClass="btn" Text="Deny" />
        </div>
        <div>
            <asp:DropDownList ID="ddListFundraisers" runat="server"> </asp:DropDownList>
            <asp:Button ID="btnRemoveFundraiser" runat="server" Text="Fundraiser is Over" CssClass="btn" />
        </div> 

        <hr />
        <!-- Fundraisers -->
        <h2>Highlighted Fundraiser:</h2>
        <div class="fundraiser">
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
        <hr />
        <div>
            <label>Total Funds:</label>
            <asp:Label ID="txtTotalFunds" runat="server" Text="TempFunds"></asp:Label>
        </div>
        <div>
            <asp:TextBox ID="txtRemoveAmount" runat="server"/>
            <asp:Button ID="btnAddExpenditure" runat="server" Text="Mark Expenditure and Remove" CssClass="btn" OnClick="btnAddExpenditure_Click" />
        </div>
    </form>
</body>
</html>

