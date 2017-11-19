<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreInitSession.aspx.cs" Inherits="PreInitSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        window.onload = function()
        {
            document.getElementById("LoginForm").submit();
        }
    </script>
</head>
<body>
    <form id="LoginForm" action="InitSession.aspx" method="post">                                                
        <div style="display:none;">
            <input type="text" name="UserId" id="UserId" value="<%=this.UserId %>" />
            <input type="text" name="CompanyId" id="CompanyId" value="<%=this.CompanyId %>" />
        </div>
    </form>
</body>
</html>
