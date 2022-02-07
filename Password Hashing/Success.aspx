<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="Password_Hashing.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <asp:Label ID="lbl_success" runat="server" Text="Label"></asp:Label>
    <form id="form1" runat="server">
    <div>
        <h2>User Profile</h2>

        <p>User ID : <asp:Label ID="lbl_userID" runat="server"></asp:Label>
        </p>

        <p>First Name. :&nbsp;
            <asp:Label ID="lbl_fname" runat="server"></asp:Label>
        </p>

        <p>Last Name :&nbsp;
            <asp:Label ID="lbl_lname" runat="server"></asp:Label>
        </p>

        <p>Credit Card No. :&nbsp;
            <asp:Label ID="lbl_cc" runat="server"></asp:Label>
        </p>

        <p>Date of Birth :&nbsp;
            <asp:Label ID="lbl_dob" runat="server"></asp:Label>
        </p>
            
        <p>Photo :&nbsp;
            <asp:Image ID="photo" runat="server" Height="200px" Width="200px" />
        </p>
    </div>
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick ="Logout"/>
    </form>

</body>

</html>
