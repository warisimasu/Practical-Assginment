<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Password_Hashing.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="https://www.google.com/recaptcha/api.js?render=6Ld-01oeAAAAAAJyN2zmZcbh4zJ_0yJRCF9sh6mn"></script>

</head>
<body>
       <form id="form1" runat="server">
    <h2>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_userid" runat="server" Height="16px" Width="280px" pattern="^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$">john@gmail.com</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="16px" Width="281px" TextMode="Password">Notion@8pmSucks</asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
       
                </td>
                <td class="style2">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Login" Width="288px" data-action="Login"/>
                </td>
            </tr>
    </table>
&nbsp;&nbsp;&nbsp;



        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" /> 

    <br />
           <br />
        <asp:Label ID="lb_error" runat="server"></asp:Label>
           <br />
           <asp:Label ID="lb_error2" runat="server"></asp:Label>
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>

               <script>
                   grecaptcha.ready(function () {
                       grecaptcha.execute('6Ld-01oeAAAAAAJyN2zmZcbh4zJ_0yJRCF9sh6mn', { action: 'Login' }).then(function (token) {
                           document.getElementById("g-recaptcha-response").value = token;
                           //document.getElementById("lb_error2").innerHTML = token;

                           console.log(token);
                            
                       });
                   });
               </script>
</body>
</html>
