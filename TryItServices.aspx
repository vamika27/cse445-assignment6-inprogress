<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TryItServices.aspx.cs" Inherits="Assignment6.TryItServices" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TryIt Services - Swim Club</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; background-color: #f0f8ff; }
        h1 { color: #003366; }
        h2 { color: #005599; margin-top: 40px; }
        .form-group { margin: 10px 0; }
        label { display: inline-block; width: 180px; font-weight: bold; }
        .btn { background-color: #003366; color: white; padding: 8px 16px;
               border: none; cursor: pointer; margin-top: 10px; border-radius: 4px; }
        .btn:hover { background-color: #005599; }
        .result-box { background-color: #e8f0fe; border: 1px solid #003366;
                      padding: 15px; margin-top: 15px; border-radius: 4px; max-width: 600px; }
        .service-section { border: 1px solid #ccc; padding: 20px; margin-top: 30px;
                           border-radius: 6px; background-color: #ffffff; max-width: 640px; }
        .back-link { margin-top: 30px; display: block; }
        hr { border: 1px solid #003366; margin-top: 40px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>⚙️ Web Services TryIt Page</h1>
        <p>
            This page demonstrates all three of Vamika's deployed ASMX web services.
            Enter inputs below and click the corresponding button to invoke each service.
        </p>

        <!-- ===================== SWIM EVENT SERVICE ===================== -->
        <div class="service-section">
            <h2>🏅 SwimEventService</h2>
            <p>Calculates points and returns a result summary for a swimmer based on their event time.</p>

            <div class="form-group">
                <label>Swimmer Name:</label>
                <asp:TextBox ID="txtSwimmerName" runat="server" Width="220px" />
            </div>
            <div class="form-group">
                <label>Event Name:</label>
                <asp:DropDownList ID="ddlSwimEvent" runat="server" Width="228px">
                    <asp:ListItem Text="50m Freestyle" Value="50m Freestyle" />
                    <asp:ListItem Text="100m Freestyle" Value="100m Freestyle" />
                    <asp:ListItem Text="200m Freestyle" Value="200m Freestyle" />
                    <asp:ListItem Text="100m Backstroke" Value="100m Backstroke" />
                    <asp:ListItem Text="100m Breaststroke" Value="100m Breaststroke" />
                    <asp:ListItem Text="100m Butterfly" Value="100m Butterfly" />
                    <asp:ListItem Text="200m Individual Medley" Value="200m Individual Medley" />
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>Time (seconds):</label>
                <asp:TextBox ID="txtTime" runat="server" Width="220px" placeholder="e.g. 45.5" />
            </div>

            <asp:Button ID="btnGetPoints" runat="server" Text="Get Points" 
                        CssClass="btn" OnClick="btnGetPoints_Click" />
            <asp:Button ID="btnGetResult" runat="server" Text="Get Full Result" 
                        CssClass="btn" OnClick="btnGetResult_Click" />

            <asp:Panel ID="pnlSwimResult" runat="server" CssClass="result-box" Visible="false">
                <asp:Label ID="lblSwimResult" runat="server" />
            </asp:Panel>
        </div>

        <!-- ===================== PRACTICE SERVICE ===================== -->
        <div class="service-section">
            <h2>📅 PracticeService</h2>
            <p>Returns the practice schedule for a given day, or lists all practice days.</p>

            <div class="form-group">
                <label>Day of the Week:</label>
                <asp:DropDownList ID="ddlDay" runat="server" Width="228px">
                    <asp:ListItem Text="Monday" Value="Monday" />
                    <asp:ListItem Text="Tuesday" Value="Tuesday" />
                    <asp:ListItem Text="Wednesday" Value="Wednesday" />
                    <asp:ListItem Text="Thursday" Value="Thursday" />
                    <asp:ListItem Text="Friday" Value="Friday" />
                    <asp:ListItem Text="Saturday" Value="Saturday" />
                    <asp:ListItem Text="Sunday" Value="Sunday" />
                </asp:DropDownList>
            </div>

            <asp:Button ID="btnGetSchedule" runat="server" Text="Get Schedule" 
                        CssClass="btn" OnClick="btnGetSchedule_Click" />
            <asp:Button ID="btnGetAllDays" runat="server" Text="Get All Practice Days" 
                        CssClass="btn" OnClick="btnGetAllDays_Click" />

            <asp:Panel ID="pnlPracticeResult" runat="server" CssClass="result-box" Visible="false">
                <asp:Label ID="lblPracticeResult" runat="server" />
            </asp:Panel>
        </div>

        <!-- ===================== FUNDRAISING SERVICE ===================== -->
        <div class="service-section">
            <h2>💰 FundraisingService</h2>
            <p>Tracks fundraising progress and returns a status message based on goal vs. amount raised.</p>

            <div class="form-group">
                <label>Fundraising Goal ($):</label>
                <asp:TextBox ID="txtGoal" runat="server" Width="220px" placeholder="e.g. 5000" />
            </div>
            <div class="form-group">
                <label>Amount Raised ($):</label>
                <asp:TextBox ID="txtRaised" runat="server" Width="220px" placeholder="e.g. 2500" />
            </div>

            <asp:Button ID="btnGetProgress" runat="server" Text="Get Progress" 
                        CssClass="btn" OnClick="btnGetProgress_Click" />
            <asp:Button ID="btnGetStatus" runat="server" Text="Get Status Message" 
                        CssClass="btn" OnClick="btnGetStatus_Click" />

            <asp:Panel ID="pnlFundResult" runat="server" CssClass="result-box" Visible="false">
                <asp:Label ID="lblFundResult" runat="server" />
            </asp:Panel>
        </div>

        <!-- ===================== HASHING SERVICE ===================== -->
        <div class="service-section">
            <h2>🔐 CryptoUtil Hashing Service</h2>
            <p>Hashes an input (passwords) upon creation of a member/staff account.</p>

            <div class="form-group">
                <label>Hashed Password:</label>
                <asp:TextBox ID="txtHashMe" runat="server" Width="220px" placeholder="e.g. test" />
            </div>

            <asp:Button ID="btnHashMe" runat="server" Text="Hash Password" 
                        CssClass="btn" OnClick="btnHashMe_Click" />

            <asp:Panel ID="pnlHashed" runat="server" CssClass="result-box" Visible="false">
                <asp:Label ID="lblHashed" runat="server" />
            </asp:Panel>
        </div>

        <a href="Default.aspx" class="back-link">← Back to Home</a>

    </form>
</body>
</html>