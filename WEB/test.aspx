﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button runat="server" ID="BtnMail" Text="Test email" OnClick="BtnMail_Click" />
        <asp:Literal runat="server" ID="ltMail"></asp:Literal>
        <asp:Literal runat="server" ID="ltCns"></asp:Literal>
    <asp:Literal runat="server" ID="ltCnn"></asp:Literal>
    </div>
    </form>
</body>
</html>
