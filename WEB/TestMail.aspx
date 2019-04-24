<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestMail.aspx.cs" Inherits="TestMail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button runat="server" ID="btnMail" OnClick="btnMail_Click" />
        <asp:Literal runat="server" ID="LtMail"></asp:Literal>
    </div>
    </form>
</body>
</html>
