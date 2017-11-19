<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="NoGrants.aspx.cs" Inherits="NoGrants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <p><%=this.Dictionary["Common_NoGrants_Explanation"] %>&nbsp;<strong><%=this.Company.DefaultAddress.Email %></strong></p>
</asp:Content>

