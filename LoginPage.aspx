<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="Assignment6.LoginPage" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - ASU Swim Club</title>
    <style>
        body { font-family: Arial, sans-serif; background-color: #f0f8ff;
               display: flex; justify-content: center; align-items: center;
               min-height: 100vh; margin: 0; }
        .login-box { background: white; padding: 40px; border-radius: 8px;
                     box-shadow: 0 2px 10px rgba(0,0,0,0.1); width: 380px; }
        h1 { color: #003366; text-align: center; margin-bottom: 5px; }
        .subtitle { text-align: center; color: #666; margin-bottom: 25px; }
        .form-group { margin: 15px 0; }
        label { display: block; font-weight: bold; color: #003366; margin-bottom: 5px; }
        .input-field { width: 100%; padding: 10px; border: 1px solid #ccc;
                       border-radius: 4px; box-sizing: border-box; font-size: 14px; }
        .btn { background-color: #003366; color: white; padding: 12px;
               border: none; cursor: pointer; border-radius: 4px;
               width: 100%; font-size: 16px; margin-top: 10px; }
        .btn:hover { background-color: #005599; }
        .error-msg { color: red; text-align: center; margin-top: 10px;
                     font-weight: bold; display: block; }
        .debug-msg { color: blue; text-align: center; margin-top: 5px;
                     font-size: 0.85em; display: block; }
        .signup-link { text-align: center; margin-top: 15px; }
        .signup-link a { color: #005599; }
        .back-link { text-align: center; margin-top: 10px; display: block; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-box">
            <h1>🏊 Swim Club</h1>
            <p class="subtitle">Please log in to continue</p>

            <div class="form-group">
                <label>Login As:</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="input-field">
                    <asp:ListItem Text="Member" Value="Member" />
                    <asp:ListItem Text="Staff" Value="Staff" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label>Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" />
            </div>

            <div class="form-group">
                <label>Password:</label>
                <asp:TextBox ID="txtPassword" runat="server"
                             TextMode="Password" CssClass="input-field" />
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Login"
                        CssClass="btn" OnClick="btnLogin_Click"
                        CausesValidation="false" />

            <asp:Label ID="lblError" runat="server" CssClass="error-msg" />
            <asp:Label ID="lblDebug" runat="server" CssClass="debug-msg" />

            <div class="signup-link">
                Not a member yet? <a href="SignUpDemo.aspx">Sign Up here</a>
            </div>
            <a href="Default.aspx" class="back-link">← Back to Home</a>
        </div>
    </form>
</body>
</html>