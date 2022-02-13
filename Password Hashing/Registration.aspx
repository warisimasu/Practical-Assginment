<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Password_Hashing.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>


        <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID%>').value;
            console.log(str);

            var pattern = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[-+_!@#$%^&*.,?]).+$");

            var capsTest = new RegExp("^(?=.*[A-Z])");

            var specialTest = new RegExp("^(?=.*[-+_!@#$%^&*.,?])");

            if (str.length == 0) {
                document.getElementById("passchecker").innerHTML = "Required Field!";
                document.getElementById("passchecker").style.color = "Red";
                return ("no_password");
            }

            else if (str.length < 12) {
                document.getElementById("passchecker").innerHTML = "Password Length Must be at Least 12 Characters Long";
                document.getElementById("passchecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("passchecker").innerHTML = "Password requires at least 1 number";
                document.getElementById("passchecker").style.color = "red";
                return ("no_number");
            }
            else if (capsTest.test(str) == false) {
                document.getElementById("passchecker").innerHTML = "Password requires at least 1 capital character";
                document.getElementById("passchecker").style.color = "red";
                return ("no_caps");
            }
            else if (specialTest.test(str) == false) {
                document.getElementById("passchecker").innerHTML = "Password requires at least 1 special character";
                document.getElementById("passchecker").style.color = "red";
                return ("no_special");
            }


            document.getElementById("passchecker").innerHTML = "Excellent!";
            document.getElementById("passchecker").style.color = "Blue";
        }

        //document.getElementById("but_passchecker").addEventListener("dblclick");


            function validateEmail() {

                var str = document.getElementById('<%=tb_email.ClientID%>').value;

                console.log(str)

                var pattern = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])")

                if (str.length == 0) {
                    document.getElementById("emailchecker").innerHTML = "Required Field!";
                    document.getElementById("emailchecker"), style.color = "Red";
                    return ("no_email");
                }

                else if (pattern.test(str) == false) {

                    document.getElementById("emailchecker").innerHTML = "Invalid Email";
                    document.getElementById("emailchecker").style.color = "Red";
                    return ("invalid_email");
                }

                document.getElementById("emailchecker").innerHTML = " ";
            }


            function validateCC() {

                var str = document.getElementById('<%=tb_creditcard%>').value;

                console.log(str)

                var pattern = new RegExp("[0-9/s]{13,19}")

                if (str.length == 0) {
                    document.getElementById("creditcardchecker").innerHTML = "Required Field!";
                    documnet.getElementById("creditcardchecker").style.color = "Red";
                    return ("no_cc")
                }
                else if (pattern.test(str) == false) {

                    document.getElementById("creditcardchecker").innerHTML = "Invalid Credit Card No."
                    document.getElementById("creditcardchecker").style.color = "Red";
                    return ("invalid_cc")
                }

                document.getElementById("creditcardchecker").innerHTML = " ";
            }





            function samePass() {

                var pass = document.getElementById('<%=tb_pwd.ClientID%>').value;
                var cfpass = document.getElementById('<%=tb_cfpwd.ClientID%>').value;

                console.log(pass)
                console.log(cfpass)

                if (pass !== cfpass) {

                    document.getElementById("cfpasschecker").innerHTML = "Passwords do not match";
                    document.getElementById("cfpasschecker").style.color = "Red";
                    return ("cf_pass_not_matching")

                }
                else {
                    document.getElementById("cfpasschecker").innerHTML = "";
                }
            }
        </script>

    <!-- Google Recaptcha V3-->
        <script src="https://www.google.com/recaptcha/api.js?render=6Ld-01oeAAAAAAJyN2zmZcbh4zJ_0yJRCF9sh6mn"></script>
        
    

</head>
<body>
        <form id="form1" runat="server">
    <div>
    
    <h2>
        
        <br />
        <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
        <br />
        <br />
   </h2>
        <table class="style1">

            <!----------------------------- Email ----------------------------->
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="Email (UserID)"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_email" runat="server" Height="36px" Width="280px" onKeyup="javascript:validateEmail()" pattern="^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$"></asp:TextBox>
                    <asp:Label ID="emailchecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>


            <!----------------------------- Fisrt Name ----------------------------->
            <tr>
                <td class="style3">
        <asp:Label ID="Label7" runat="server" Text="First Name"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_fname" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    <asp:Label ID="fnamechecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>


            <!----------------------------- Last Name ----------------------------->
            <tr>
                <td class="style3">
        <asp:Label ID="Label8" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_lname" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    <asp:Label ID="lnamechecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            

            <!----------------------------- Password ----------------------------->
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px" onKeyup="javascript:validate(); javascript:samePass()" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="passchecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>


            <!----------------------------- Confirm Password ----------------------------->
            <tr>
                <td class="style3">
        <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_cfpwd" runat="server" Height="32px" Width="281px" onKeyup="javascript:samePass()" TextMode="Password"></asp:TextBox>
                    <asp:Label ID="cfpasschecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>

            <!----------------------------- Credit Card ----------------------------->            
                        <tr>
                <td class="style6">
        <asp:Label ID="Label5" runat="server" Text="Credit Card No."></asp:Label>
                </td>
                <td class="style7">
                    <asp:TextBox ID="tb_creditcard" runat="server" Height="32px" Width="281px" type="tel" inputmode="numeric" pattern="[0-9/s]{13,19}" maxlength="19" placeholder="xxxx xxxx xxxx xxxx" onKeyup="javascript:validateCC()"></asp:TextBox>
                    <asp:Label ID="creditcardchecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>

            <!----------------------------- BOD ----------------------------->
            <!--
            <tr>
                <td class="style3">
                    <asp:Label ID="Label9" runat="server" Text="Date of Birth"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_dateofbirth" runat="server" Height="32px" Width="281px"></asp:TextBox>
                </td>
            </tr>
            -->

            <tr>
                <td class="style3">
                    <asp:Label ID="Label12" runat="server" Text="Date of Birth"></asp:Label>
                </td>
                <td class="style2">
                    <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                    <asp:Label ID="calenderchecker" runat="server" Text=""></asp:Label>
                </td>
            </tr>


            
            <!--            <tr>
                <td class="style3">
        <asp:Label ID="Label6" runat="server" Text="Mobile"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(+65)
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_mobile" runat="server" Height="32px" Width="281px">81888188</asp:TextBox>
                </td>
            </tr>
            -->

            
            <!----------------- Photo Upload ----------------------->
            <tr>
                <td class="style3">
                    <asp:Label ID="Label10" runat="server" Text="Photo"></asp:Label>
                </td>
                <td class="style2">
                    <asp:FileUpload ID="FileUpload1" runat="server" Height="32px" Width="281px"/>
                    <asp:Label ID="photochecker" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
                        <tr>
                <td class="style4">
       
                </td>




                <td class="style5">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" data-action="Registration"/>
                </td>
            </tr>
    </table>

        <script>
            grecaptcha.ready(function () {
                grecaptcha.execute('6Ld-01oeAAAAAAJyN2zmZcbh4zJ_0yJRCF9sh6mn', { action: 'Registration' }).then(function (token) {
                    document.getElementById("g-recaptcha-response").value = token;

                    console.log(token);
                });
            });
        </script>


        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
&nbsp;<br />
        <asp:Label ID="lb_error1" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lb_error2" runat="server"></asp:Label>
    <br />
        <br />
    
    </div>
           
    </form>
</body>
</html>
