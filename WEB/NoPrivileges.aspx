<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="NoPrivileges.aspx.cs" Inherits="NoPrivileges" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <h1><%=this.Dictionary["Common_Error_UserCanNotAccess"] %></h1>
     <div class="form-group">
    <%=this.UIForm %>
    </div>
</asp:Content>

