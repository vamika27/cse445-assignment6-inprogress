<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberPage.aspx.cs" Inherits="Assignment6.MemberPage" %>

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
        .competitionRow {
            background-color: #5599cc;
            display:flex;
            flex-wrap:wrap;
            align-items: center;
            gap: 20px;
            padding: 10px;
            border: 1px solid #005599;
            border-radius: 6px;
            margin-bottom: 10px;
        }
        .formGroup {
            display:flex;
            align-items:center;
            gap: 10px;
        }
        #eventDropDown {
            padding: 6px;
            border-radius: 4px;
            border: 1px solid #005599;
            min-width: 180px;
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
        <!-- Today's Schedule -->
        <h2>My Statistics:</h2>
        <div class="statistics">
            <asp:GridView ID="MyEventTable" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="EventName" HeaderText="My Events" />
                    <asp:BoundField DataField="PersonalTime" HeaderText="My Times" />
                    <asp:BoundField DataField="BestTime" HeaderText="My Best Time">
                        <ItemStyle Font-Bold="true" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>

            <asp:GridView ID="TeamEventTable" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="EventName" HeaderText="Team Events" />
                    <asp:BoundField DataField="Role" HeaderText="My Role" />
                    <asp:BoundField DataField="PersonalTime" HeaderText="My Times" />
                    <asp:BoundField DataField="BestTime" HeaderText="My Best Time" >
                        <ItemStyle Font-Bold="true" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TeamTime" HeaderText="Team Times" />
                    <asp:BoundField DataField="TeamBestTime" HeaderText="Best Team Time" >
                        <ItemStyle Font-Bold="true" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
        <hr />
        <h2>Competitions:</h2>
        <div class="competitionRow">
            <div class="formGroup">
                <label>Date:</label>
                <asp:TextBox ID="compDatePicker" runat="server" TextMode="Date" CssClass="myDatePicker"></asp:TextBox>
                
            </div>
            <div class="formGroup">
                <asp:DropDownList ID="eventDropDown" runat="server" >
                    <asp:ListItem>200 Medley Relay - Backstroke</asp:ListItem>
                    <asp:ListItem>200 Medley Relay - Breaststroke </asp:ListItem>
                    <asp:ListItem>200 Medley Relay - Butterfly</asp:ListItem>
                    <asp:ListItem>200 Medley Relay - Freestyle</asp:ListItem>
                    <asp:ListItem>200 Freestyle</asp:ListItem>
                    <asp:ListItem>200 Individual Medley</asp:ListItem>
                    <asp:ListItem>50 Freestyle</asp:ListItem>
                    <asp:ListItem>100 Butterfly</asp:ListItem>
                    <asp:ListItem>100 Freestyle</asp:ListItem>
                    <asp:ListItem>500 Freestyle</asp:ListItem>
                    <asp:ListItem>200 Freestyle Relay - Start</asp:ListItem>
                    <asp:ListItem>200 Freestyle Relay - 2nd</asp:ListItem>
                    <asp:ListItem>200 Freestyle Relay - 3rd</asp:ListItem>
                    <asp:ListItem>200 Freestyle Relay - Finish</asp:ListItem>
                    <asp:ListItem>100 Backstroke</asp:ListItem>
                    <asp:ListItem>100 Breaststroke</asp:ListItem>
                    <asp:ListItem>400 Freestyle Relay - Start</asp:ListItem>
                    <asp:ListItem>400 Freestyle Relay - 2nd</asp:ListItem>
                    <asp:ListItem>400 Freestyle Relay - 3rd</asp:ListItem>
                    <asp:ListItem>400 Freestyle Relay - Finish</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnEventSignUp" runat="server" Text="Sign Up for Event" CssClass="btn" OnClick="btnEventSignUp_Click" />

        </div>
        <hr />
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
            <asp:Button ID="btnPledgeFund" runat="server" Text="Pledge Money" CssClass="btn"  />
        </div> 

        

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
