<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="Assignment6.CaptchaControl" %>

 <style>
     body {
         background-color: #f0f0f0; 
         align-content: center;
     }
     .captcha {
         background-color: #e0e0ff; 
         border: 2px solid #000; 
         width: 500px; 
         height: 350px;
         padding: 5px;
         margin: 10px;
     }
     .btn-info {
         width: 120px;
         height: 40px;
     }
     .btnCenter {
         display: flex;
         justify-content: center;
         align-content: center;
     }
     .verifcation {
         display:flex;
         justify-content: space-between;
         vertical-align: middle;
     }
     #txtInput {
         width: 370px;
         height: 30px;
         margin-top: 5px;
     }

 </style>

 <div class="captcha"">
     <h2>Swimmer Verification</h2>
     <h4>Type the code below to prove you are human:</h4>
     <asp:Image ID="imgCaptcha" runat="server" />
     <br />
     <div class="btnCenter">
         <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" class="btn-info" />
     </div>
     <br />
     <div class="verifcation">
         <asp:TextBox ID="txtInput" runat="server" placeholder="Captcha here"></asp:TextBox>
         <asp:Button ID="btnCheck" runat="server" Text="Verify" OnClick="btnCheck_Click" class="btn-info" />
     </div>
     <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
 </div>