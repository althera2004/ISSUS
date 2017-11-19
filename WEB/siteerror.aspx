<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="SiteError.aspx.cs" Inherits="SiteError" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    <div class="col-sm-12" id="contentError">
            <div class="text-center error-box">
                <h1 class="error-text tada animated"><i class="fa fa-times-circle text-danger error-icon-shadow"></i> ¡Vaya!</h1>
                <h2 class="font-xl"><strong>Parece que algo no va como debería!</strong></h2>
                <br />
                <p class="lead semi-bold">
                    <strong>Ha ocurrido un error técnico. Lo sentimos.</strong>
                </p>
                <p class="lead semi-bold" style="color:#f33;">
                    <asp:Literal runat="server" ID="ErrorMessage"></asp:Literal>
                </p>
                <p class="lead semi-bold">
                    <small>Si cree que estaba trabajando correctamente. Espere unos instantes y vuelva a intentarlo.</small>
                </p>
            </div>

        </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
</asp:Content>

