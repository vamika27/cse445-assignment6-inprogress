<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignUpControl.ascx.cs" Inherits="Assignment6.SignUpControl" %>
<style>
    .signup-container { background-color: #e8f0fe; border: 1px solid #003366;
                        padding: 20px; border-radius: 6px; max-width: 500px; }
    .signup-container h3 { color: #003366; margin-top: 0; }
    .form-group { margin: 10px 0; }
    .form-label { display: inline-block; width: 160px; font-weight: bold; }
    .signup-btn { background-color: #003366; color: white; padding: 8px 16px;
                  border: none; cursor: pointer; border-radius: 4px; margin-top: 10px; }
    .signup-btn:hover { background-color: #005599; }
    .validation-error { color: red; font-size: 0.85em; }
</style>

<div class="signup-container">
    <h3>🏊 Swim Club Member Sign Up</h3>

    <div class="form-group">
        <span class="form-label">Full Name:</span>
        <asp:TextBox ID="txtName" runat="server" Width="220px" />
        <br />
        <asp:RequiredFieldValidator ID="rfvName" runat="server"
            ControlToValidate="txtName"
            ErrorMessage="Name is required."
            CssClass="validation-error"
            Display="Dynamic" />
    </div>

    <div class="form-group">
        <span class="form-label">Email:</span>
        <asp:TextBox ID="txtEmail" runat="server" Width="220px" />
        <br />
        <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
            ControlToValidate="txtEmail"
            ErrorMessage="Email is required."
            CssClass="validation-error"
            Display="Dynamic" />
        <asp:RegularExpressionValidator ID="revEmail" runat="server"
            ControlToValidate="txtEmail"
            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
            ErrorMessage="Please enter a valid email address."
            CssClass="validation-error"
            Display="Dynamic" />
    </div>

    <div class="form-group">
        <span class="form-label">Password:</span>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="220px" />
        <br />
        <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
            ControlToValidate="txtPassword"
            ErrorMessage="Password is required."
            CssClass="validation-error"
            Display="Dynamic" />
        <asp:RegularExpressionValidator ID="revPassword" runat="server"
            ControlToValidate="txtPassword"
            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"
            ErrorMessage="Password must be 8+ characters with upper, lower, digit, and special character."
            CssClass="validation-error"
            Display="Dynamic" />
    </div>

    <div class="form-group">
        <span class="form-label">Confirm Password:</span>
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Width="220px" />
        <br />
        <asp:CompareValidator ID="cvPassword" runat="server"
            ControlToValidate="txtConfirmPassword"
            ControlToCompare="txtPassword"
            ErrorMessage="Passwords do not match."
            CssClass="validation-error"
            Display="Dynamic" />
    </div>

    <div class="form-group">
        <span class="form-label">Preferred Event:</span>
        <asp:DropDownList ID="ddlEvent" runat="server" Width="228px">
            <asp:ListItem Text="-- Select Event --" Value="" />
            <asp:ListItem Text="50m Freestyle" Value="50m Freestyle" />
            <asp:ListItem Text="100m Freestyle" Value="100m Freestyle" />
            <asp:ListItem Text="200m Freestyle" Value="200m Freestyle" />
            <asp:ListItem Text="100m Backstroke" Value="100m Backstroke" />
            <asp:ListItem Text="100m Breaststroke" Value="100m Breaststroke" />
            <asp:ListItem Text="100m Butterfly" Value="100m Butterfly" />
            <asp:ListItem Text="200m Individual Medley" Value="200m Individual Medley" />
        </asp:DropDownList>
        <br />
        <asp:RequiredFieldValidator ID="rfvEvent" runat="server"
            ControlToValidate="ddlEvent"
            InitialValue=""
            ErrorMessage="Please select a preferred event."
            CssClass="validation-error"
            Display="Dynamic" />
    </div>

    <div class="form-group">
        <span class="form-label">Phone (optional):</span>
        <asp:TextBox ID="txtPhone" runat="server" Width="220px" />
    </div>

    <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" 
                CssClass="signup-btn" OnClick="btnSignUp_Click" />
    <asp:Button ID="btnReset" runat="server" Text="Reset" 
                CssClass="signup-btn" OnClick="btnReset_Click" CausesValidation="false" />

    <br /><br />

    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <asp:Label ID="lblMessage" runat="server" />
    </asp:Panel>

</div>