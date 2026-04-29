<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpDemo.aspx.cs" Inherits="Assignment6.SignUpDemo" %>
<%@ Register Src="~/Controls/SignUpControl.ascx" TagName="SignUpControl" TagPrefix="uc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up Demo - Swim Club</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 30px; background-color: #f0f8ff; }
        h1 { color: #003366; }
        p { max-width: 600px; }
        .back-link { margin-top: 20px; display: block; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>📋 Sign Up User Control - TryIt</h1>
        <p>
            This page demonstrates Vamika's Sign Up User Control. Fill in the form
            below to simulate a new swimmer registering for the ASU Swim Club.
            Upon successful signup, your profile will also be saved to a cookie.
        </p>

        <!-- The Sign Up User Control embedded here -->
        <uc:SignUpControl ID="SignUpControl1" runat="server" />

        <a href="Default.aspx" class="back-link">← Back to Home</a>
    </form>
</body>
</html>