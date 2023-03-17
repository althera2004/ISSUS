<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="UserView.aspx.cs" Inherits="UserView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .employeeProfile{display:none;}
        .emailed{visibility:hidden;}
        #ItemStatus{display:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var ItemUserId = <%=this.UserItemId %>;
        var CompanyUserNames = [<%=this.CompanyUserNames %>];
        var CompanyId = <%=this.CompanyId %>;
        var itemUser = <%=this.UserItem.Json %>;
        var ddData = [<%=this.CountryData %>];
        var userEmails = <%=this.Emails %>;
        var debug = "<%=this.Debug %>";
        var pageType = "form";
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div>                             
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active" id="TabAcceso">
                                                    <a data-toggle="tab" href="#principal"><%=this.Dictionary["Item_User_Tab_Principal"]%></a>
                                                </li>
                                                <li class="" id="TabPermisos" style="display:none;">
                                                    <a data-toggle="tab" href="#permisos"><%=this.Dictionary["Item_User_Tab_Grants"]%></a>
                                                </li>
                                                <!--<li class="" id="TabTrazas" runat="server">
                                                    <a data-toggle="tab"href="#trazas"><%=this.Dictionary["Item_Traces"] %></a>
                                                </li>-->
                                            </ul>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="principal" class="tab-pane active">
                                                    <h4><%=this.Dictionary["Item_User_Title_Account"]%></h4>
                                                    <div class="form-group col-sm-12">
                                                        <label id="TxtUserNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Common_Name"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-5">
                                                            <input type="text" id="TxtUserName" placeholder="<%=this.Dictionary["Common_Name"] %>" value="<%=this.UserItem.UserName %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                                            <span class="ErrorMessage" id="TxtUserNameErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                                            <span class="ErrorMessage" id="TxtUserNameErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"]%></span>
                                                        </div>
                                                        <label id="TxtUserEmailLabel" class="col-sm-1 control-label no-padding-right _emailed"><%=this.Dictionary["Email"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-5 _emailed">
                                                            <input type="text" id="TxtUserEmail" placeholder="<%=this.Dictionary["Email"] %>" value="<%=this.UserItem.Email %>" class="col-xs-12 col-sm-12" onblur="this.value=$.trim(this.value);" />
                                                            <span class="ErrorMessage" id="TxtUserEmailErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                                            <span class="ErrorMessage" id="TxtUserEmailErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"]%></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-sm-12">
                                                        <label class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Profile_FieldLabel_Language"] %></label>
                                                        <div class="col-xs-3" id="DivCmbIdioma" style="height:35px !important;">
                                                            <select id="CmbIdioma" class="col-xs-12">
                                                                <asp:Literal runat="server" ID="LtIdiomas"></asp:Literal>
                                                            </select>
                                                        </div>
                                                    <!--/div>
                                                    <div class="form-group col-sm-12"-->
                                                        <label id="" class="col-sm-2 control-label no-padding-right">&nbsp;</label>
                                                        <label id="TxtPrimaryUserLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_User_Rol"] %></label>
                                                        <div class="col-sm-2">
                                                            <input type="checkbox" id="ChkPrimaryUser" disabled="disabled" />&nbsp;<%=this.Dictionary["User_PrimaryUser"] %>
                                                        </div>
                                                        <div class="col-sm-2 _emailed">
                                                            <input type="checkbox" id="ChkAdmin" />&nbsp;<%=this.Dictionary["User_Admin"] %>
                                                        </div>
                                                    </div>
                                                    <h4 style="clear:both;"><%=this.Dictionary["Item_Employee"]%></h4>
                                                    <div class="form-group col-sm-12">
                                                        <label id="CmbEmployeeLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee"] %></label>
                                                        <div class="col-sm-5">
                                                            <select id="CmbEmployee" onchange="CmbEmployeeChanged()" class="col-sm-12">
                                                                <asp:Literal runat="server" ID="CmbEmployeeData"></asp:Literal>
                                                            </select>
                                                            <span class="ErrorMessage" id="CmbEmployeeRequired"><%=this.Dictionary["Common_Required"] %></span>                                                            
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-sm-12 employeeProfile">
                                                        <label id="TxtNombreLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Name"] %></label>
                                                        <div class="col-sm-3">
                                                            <input type="text" id="TxtNombre" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Name"] %>" value="<%=this.UserItem.Employee.Name %>" class="col-xs-12 col-sm-12" maxlength="50" readonly="readonly" /></div>
                                                        <label id="TxtApellido1Label" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_LastName"] %></label>
                                                        <div class="col-sm-4">
                                                            <input type="text" id="TxtApellido1" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_LastName"] %>" value="<%=this.UserItem.Employee.LastName %>" class="col-xs-12 col-sm-12" maxlength="50" readonly="readonly" />
                                                        </div>
                                                        <label id="TxtNifLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_NIF"]%></label>
                                                        <div class="col-sm-2">
                                                            <input type="text" id="TxtNif" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_NIF"] %>" value="<%=this.UserItem.Employee.Nif %>" class="col-xs-12 col-sm-12" maxlength="15" readonly="readonly" />
                                                        </div>
                                                    </div>                                                            
                                                    <div class="form-group col-sm-12 employeeProfile">
                                                        <label id="TxtTelefonoLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Phone"]%></label>
                                                        <div class="col-sm-3">
                                                            <input type="text" id="TxtTelefono" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Phone"] %>" value="<%=this.UserItem.Employee.Phone %>" class="col-xs-12 col-sm-12" onkeypress="validate(event)" maxlength="15" readonly="readonly" />
                                                            <span class="ErrorMessage" id="TxtTelefonoErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label id="TxtEmailLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Email"]%></label>
                                                        <div class="col-sm-7">
                                                            <input type="text" id="TxtEmail" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Email"] %>" value="<%=this.UserItem.Employee.Email %>" class="col-xs-12 col-sm-12" maxlength="50" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-sm-12 employeeProfile">
                                                        <label id="TxtDireccionLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Address"]%></label>
                                                        <div class="col-sm-11">
                                                            <input type="text" id="TxtDireccion" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Address"] %>" value="<%=this.UserItem.Employee.Address.Address %>" class="col-xs-12 col-sm-12" maxlength="50" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-sm-12 employeeProfile">
                                                        <label id="TxtCpLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_PostalCode"]%></label>
                                                        <div class="col-sm-2">
                                                            <input type="text" id="TxtCp" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_PostalCode"] %>" value="<%=this.UserItem.Employee.Address.PostalCode %>" class="col-xs-12 col-sm-12" maxlength="10" readonly="readonly" />                                                            
                                                        </div>
                                                        <label id="TxtPoblacionLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_City"]%></label>
                                                        <div class="col-sm-8">
                                                            <input type="text" id="TxtPoblacion" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_City"] %>" value="<%=this.UserItem.Employee.Address.City %>" class="col-xs-12 col-sm-12" maxlength="50" readonly="readonly" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group col-sm-12 employeeProfile">
                                                        <label id="TxtProvinciaLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Province"]%></label>
                                                        <div class="col-sm-6">
                                                            <input type="text" id="TxtProvincia" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Province"] %>" value="<%=this.UserItem.Employee.Address.Province %>" class="col-xs-12 col-sm-12" maxlength="50" readonly="readonly" />                                                            
                                                        </div>
                                                        <label id="TxtPaisLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Country"]%></label>
                                                        <div class="col-sm-4" id="DivCmbPais" style="height:35px !important;">
                                                            <input type="text" id="TxtPais" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Country"] %>" value="<%=this.UserItem.Employee.Address.Country %>" class="col-xs-12 col-sm-12" maxlength="15" readonly="readonly" />                                                            
                                                        </div>
                                                    </div>
                                                    <% if(this.UserItemId > 0) { %>
                                                    <div style="clear:both;">&nbsp;</div>
                                                    <h4 id="ResetH4"><%=this.Dictionary["Item_User_Title_ResetPassword"]%></h4>
                                                    <button class="btn btn-success" type="button" id="Button1" onclick="ResetPassword();">
                                                        <i class="icon-repeat bigger-110"></i>
                                                        <%=this.Dictionary["Item_User_Btn_ResetPassword"]%>
                                                    </button>
                                                    <div style="clear:both;">&nbsp;</div>
                                                    <div id="ResetAlert" class="alert alert-info">
                                                        <i class="icon-info-sign fa-2x"></i>
                                                        <h3 style="display: inline;"><%=this.Dictionary["Common_Warning"] %></h3><br />
                                                        <p style="margin-left: 50px;">
                                                        <%=this.Dictionary["Item_User_Help_ResetPassword"] %>
                                                        </p>
                                                    </div>
                                                    <% } else { %>                                                    
                                                    <div style="clear:both;">&nbsp;</div>
                                                    <% } %>
                                                </div>
                                                <div id="permisos" class="tab-pane">                                                    
                                                    <div class="alert alert-info" style="display:none;" id="DivPrimaryUser">
                                                        <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                        <h3 style="display:inline;"><%=this.Dictionary["Item_User_Help_PrimaryUser"] %></h3>
                                                    </div>
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th><%=this.Dictionary["Item_User_List_Header_Grant"]%></th>
                                                                <th style="width:200px;text-align:center"><%=this.Dictionary["Item_User_List_Header_Read"] %>&nbsp;<input type="checkbox" id="RAll" onclick="ReadAll();" /></th>
                                                                <th style="width:200px;text-align:center"><%=this.Dictionary["Item_User_List_Header_Write"] %>&nbsp;<input type="checkbox" id="WAll" onclick="WriteAll();" /></th>											
                                                            </tr>
                                                        </thead>
                                                        <tbody id="PermisosDataTable"><%=this.GrantsList %></tbody>
                                                    </table>
                                                </div>
                                                <div id="trazas" class="tab-pane">													
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th style="width:150px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"]%></th>
                                                                <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"]%></th>
                                                                <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"]%></th>
                                                                <th style="width:250px;"><%= this.Dictionary["Item_Tace_ListHeader_User"]%></th>													
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:Literal runat="server" ID="LtTrazas"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <textarea runat="server" id="Grants" style="display:none;"></textarea>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="/assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="/assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="/assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UserView.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>