<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="UserView.aspx.cs" Inherits="UserView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .employeeProfile{display:none;}
        .emailed{visibility:hidden;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var CompanyId = <%=this.CompanyId %>;
        var itemUser = <%=this.UserItem.Json %>;
        var ddData = [<%=this.CountryData %>];
        var userEmails = <%=this.Emails %>;
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
                                                            <span class="ErrorMessage" id="TxtUserNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                                            <span class="ErrorMessage" id="TxtUserNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"]%></span>
                                                        </div>
                                                        <label id="TxtUserEmailLabel" class="col-sm-1 control-label no-padding-right _emailed"><%=this.Dictionary["Email"] %><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-5 _emailed">
                                                            <input type="text" id="TxtUserEmail" placeholder="<%=this.Dictionary["Email"] %>" value="<%=this.UserItem.Email %>" class="col-xs-12 col-sm-12" onblur="this.value=$.trim(this.value);" />
                                                            <span class="ErrorMessage" id="TxtUserEmailErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                                            <span class="ErrorMessage" id="TxtUserEmailErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"]%></span>
                                                        </div>
                                                    </div>
                                                    <h4 style="clear:both;"><%=this.Dictionary["Item_Employee"]%></h4>
                                                    <div class="form-group col-sm-12">
                                                        <label id="CmbEmployeeLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Name"] %></label>
                                                        <div class="col-sm-5">
                                                            <select id="CmbEmployee" onchange="CmbEmployeeChanged()" class="col-sm-12">
                                                                <asp:Literal runat="server" ID="CmbEmployeeData"></asp:Literal>
                                                            </select>
                                                            <span class="ErrorMessage" id="CmbEmployeeRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>                                                            
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
                                                            <span class="ErrorMessage" id="TxtTelefonoErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
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
                                                        <i class="icon-info-sign fa-2x"/></i>
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
                                                        <tbody id="PermisosDataTable">
                                                            <asp:Literal runat="server" ID="LtGrantList"></asp:Literal>
                                                        </tbody>
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
                                                <%=this.FormFooter %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <textarea runat="server" id="Grants" style="display:none;"></textarea>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="assets/js/chosen.jquery.min.js"></script>
        <script type="text/javascript" src="assets/js/fuelux/fuelux.spinner.min.js"></script>
        <script type="text/javascript" src="assets/js/date-time/bootstrap-timepicker.min.js"></script>
        <script type="text/javascript" src="assets/js/date-time/moment.min.js"></script>
        <script type="text/javascript" src="assets/js/date-time/daterangepicker.min.js"></script>
        <script type="text/javascript" src="assets/js/bootstrap-colorpicker.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.knob.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.autosize.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.inputlimiter.1.3.1.min.js"></script>
        <script type="text/javascript" src="assets/js/jquery.maskedinput.min.js"></script>
        <script type="text/javascript" src="assets/js/bootstrap-tag.min.js"></script>
        <script type="text/javascript" src="js/common.js"></script>
        <script type="text/javascript">
            var ItemUserId = <%=this.UserItemId %>;
            var CompanyUserNames = [<%=this.CompanyUserNames %>];
            function GrantChanged(mode, id, sender) {
                var grants = document.getElementById('Contentholder1_Grants').value;

                var tag = mode + id + '|';

                if (sender.checked === true) {
                    grants += tag;
                }
                else {
                    grants = grants.split(tag).join('');
                }

                if (mode === 'R' && !sender.checked) {
                    document.getElementById('CheckboxWrite' + id).checked = false;
                    grants = grants.split('W' + id + '|').join('');
                }

                if (mode === 'W' && sender.checked) {
                    document.getElementById('CheckboxRead' + id).checked = true;
                    grants += 'R' + id + '|';
                }

                document.getElementById('Contentholder1_Grants').value = grants;
                TestCBAll();
            }

            function SaveGrants() {
                var webMethod = "/Async/LoginActions.asmx/Grants";

                var calculatedGrants = "|";
                for(var x=0;x<CBR.length;x++){
                    if(CBR[x].checked === true){
                        calculatedGrants +=  "R"+CBR[x].id.substring(12)+"|"
                    }
                }
                for(var x=0;x<CBW.length;x++){
                    if(CBW[x].checked === true){
                        calculatedGrants +=  "W"+CBW[x].id.substring(13)+"|"
                    }
                }

                var data = { grants: calculatedGrants, itemUserId: itemUser.Id, userId: ApplicationUser.Id };
                console.log(calculatedGrants);

                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (msg) {                           
                        document.location = referrer;
                    },
                    error: function (msg) {
                        alertUI(msg.responseText);
                    }
                });
            }

            var newUserId = null;
            function ChangeUserName()
            {
                var ok = true;
                document.getElementById('TxtUserNameErrorDuplicated').style.display = 'none';
                if(!RequiredFieldText('TxtUserName')) { ok = false; }
                else
                {
                    var duplicated = false;
                    for(var x=0; x< CompanyUserNames.length; x++)
                    {
                        if(CompanyUserNames[x].UserName.toLowerCase() == document.getElementById('TxtUserName').value.toLowerCase() &&
                           CompanyUserNames[x].UserId != ItemUserId)
                           {
                            duplicated = true;
                            ok = false;
                            break;
                           }
                    }

                    if(duplicated === true)
                    {
                        ok = false;
                        document.getElementById('TxtUserNameLabel').style.color = '#f00';
                        document.getElementById('TxtUserNameErrorDuplicated').style.display = 'block';
                    }

                    var duplicatedEmail = false;
                    for(var x=0; x< userEmails.length; x++)
                    {
                        if(userEmails[x].Email.toLowerCase() == document.getElementById('TxtUserEmail').value.toLowerCase() &&
                           userEmails[x].UserId != ItemUserId)
                        {
                            duplicatedEmail = true;
                            ok = false;
                            break;
                        }
                    }

                    if(duplicatedEmail === true)
                    {
                        ok = false;
                        document.getElementById('TxtUserEmailLabel').style.color = '#f00';
                        document.getElementById('TxtUserEmailErrorDuplicated').style.display = 'block';
                    }
                }

                if(ok===false)
                {
                    return false;
                }

                var webMethod = "/Async/LoginActions.asmx/ChangeUserName";
                if(itemUser.Id < 1)
                {
                    webMethod = "/Async/LoginActions.asmx/InsertUser";
                }

                var email = $('#TxtUserEmail').val();
                var data = 
                {
                    "itemUser":
                    {
                        "Id":itemUser.Id,
                        "UserName": $('#TxtUserName').val(),
                        "Email": email,
                        "CompanyId": CompanyId 
                    },
                    "employeeId": $('#CmbEmployee').val()*1,
                    "userId": user.Id
                };

                console.log("SAVE",data);

                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (msg) {
                        if(itemUser.Id<1)
                        {
                            // ISSUS-259
                            console.log("SAVE OK",msg.d );
                            itemUser.Id = msg.d.ReturnValue * 1;
                            newUserId = itemUser.Id;
                            alertInfoUI(Dictionary.Item_User_Message_WelcommeMailSent, grantsAvaiable);
                        }
                        else
                        {
                            SaveGrants();
                        }
                    },
                    error: function (msg) {
                        alertUI(msg.responseText);
                    }
                });
            }

            function ResetPassword()
            { 
                var webMethod = "/Async/LoginActions.asmx/ResetPassword";
                var data = { userId: ItemUserId, companyId: Company.Id };
                LoadingShow(Dictionary.Common_Message_Saving);
                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (msg) {
                        LoadingHide();
                        if(msg.d.Success!==false)
                        {
                            alertInfoUI(Dictionary.Item_User_Message_ResetPassword_Ok);
                        }
                        else
                        {
                            alertUI(msg.d.MessageError);
                        }
                    },
                    error: function (msg) {
                        LoadingHide();
                        alertUI(msg.responseText);
                    }
                });
            }
            
            jQuery(function ($) {
                $('#BtnSave').click(ChangeUserName);
                $('#BtnCancel').click(function (e) { document.location = referrer; });

                var i = 0;
                for(var x=0; x<ddData.length;x++)
                {
                    if(ddData[x].value == ApplicationUser.Employee.Address.Country) { i=x; break; }
                }

                $('#CmbPais').ddslick({data: ddData});
                $('#CmbPais').ddslick('select', {index: i });
            });

            // ISSUS-190
            //document.getElementById('TxtUserName').focus();

            // ISSUS-259
            function grantsAvaiable()
            {
                document.getElementById('TabPermisos').style.display='';
                alertInfoUI(Dictionary.Item_User_Message_GrantsAvaiable, GoNew);                
            }

            function GoNew(){document.location = "UserView.aspx?id="+newUserId;}

            function CmbEmployeeChanged()
            {
                EmployeeLayout();

                var id = $('#CmbEmployee').val()*1;
                var employeeSelected = null;
                for(var x=0; x<Company.Employees.length;x++)
                {
                    if(Company.Employees[x].Id === id)
                    {
                        employeeSelected = Company.Employees[x];
                    }
                }

                if(employeeSelected!==null)
                {
                    $('#TxtNombre').val(employeeSelected.Name);
                    $('#TxtApellido1').val(employeeSelected.LastName);
                    $('#TxtNif').val(employeeSelected.Nif);
                    $('#TxtTelfono').val(employeeSelected.Phone);
                    $('#TxtEmail').val(employeeSelected.Email);
                    $('#TxtDireccion').val(employeeSelected.Address.Address);
                    $('#TxtCp').val(employeeSelected.Address.PostalCode);
                    $('#TxtPoblacion').val(employeeSelected.Address.City);
                    $('#TxtProvincia').val(employeeSelected.Address.Province);
                    $('#TxtPais').val(CountryById(employeeSelected.Address.Country));
                    $('.employeeProfile').css('visibility','visible');
                    $('.emailed').css('display','none');
                }
                else{
                    $('.employeeProfile').css('display','none');
                    $('.employeeProfile').css('visibility','hidden');
                }
            }
            
            if($('#TxtPais').val()!=='')
            {
                $('#TxtPais').val(CountryById($('#TxtPais').val()*1));
            }

            function CountryById(id)
            {
                for(var x=0;x<Company.Countries.length;x++)
                {
                    if(Company.Countries[x].Id === id)
                    {
                        return Company.Countries[x].Name;
                    }
                }
                return '';
            }

            function EmployeeLayout()
            {
                if($('#CmbEmployee').val()*1 > 0)
                {
                    $('.employeeProfile').css('display','block');
                    //$('.emailed').css('visibility','hidden');
                }
                else{
                    $('.employeeProfile').css('display','none');
                    //$('.emailed').css('visibility','visible');
                }
            }

            window.onload = function()
            {
                EmployeeLayout();
                if(itemUser.Id > 0 && ApplicationUser.Grants.User.Write !== false)
                {
                    document.getElementById('TabPermisos').style.display="";
                }
            }

            function ReadAll(){
                var grant = document.getElementById("RAll").checked;
                if(grant==true)
                {
                    var grants = "";
                    for(var x=0;x<CBR.length;x++){
                        CBR[x].checked = true;
                        GrantChanged('R',CBR[x].id.substring(12)*1,CBR[x]);
                    }

                    document.getElementById('Contentholder1_Grants').value = grants;
                }
                else
                {
                    for(var x=0;x<CBR.length;x++){
                        CBR[x].checked = false;
                        GrantChanged('R',CBR[x].id.substring(12)*1,CBR[x]);
                    }

                    for(var x=0;x<CBW.length;x++){
                        CBW[x].checked = false;
                        GrantChanged('W',CBW[x].id.substring(13)*1,CBR[x]);
                    }

                    document.getElementById('WAll').checked = false;
                    document.getElementById('Contentholder1_Grants').value = '';
                }
            }

            function WriteAll(){
                var grant = document.getElementById('WAll').checked;
                if(grant==true)
                {
                    for(var x=0;x<CBR.length;x++){
                        CBR[x].checked = true;
                        GrantChanged('R',CBR[x].id.substring(13)*1,CBR[x]);
                    }
                    for(var x=0;x<CBW.length;x++){
                        CBW[x].checked = true;
                        GrantChanged('W',CBW[x].id.substring(13)*1,CBR[x]);
                    }

                    document.getElementById('RAll').checked = true;
                }
                else{
                    for(var x=0;x<CBW.length;x++){
                        CBW[x].checked = false;
                        GrantChanged('R',CBW[x].id.substring(13)*1,CBR[x]);
                    }
                }
            }

            function TestCBAll()
            {
                for(var x=0;x<CBR.length;x++){
                    if( CBR[x].checked ==false)
                    {
                        document.getElementById('RAll').checked = false;
                        document.getElementById('WAll').checked = false;
                        return;
                    }
                }
                
                document.getElementById('RAll').checked = true;
                
                for(var x=0;x<CBW.length;x++){
                    if( CBW[x].checked ==false)
                    {
                        document.getElementById('WAll').checked = false;
                        return;
                    }
                }
                document.getElementById('WAll').checked = true;
            }
            
            var CBR = document.getElementsByClassName('CBR');
            var CBW = document.getElementsByClassName('CBW');
            TestCBAll();

            if(itemUser.PrimaryUser === true) {
                document.getElementById("DivPrimaryUser").style.display = "block";
                for(var x=0;x<CBR.length;x++){ CBR[x].disabled = true; }
                for(var x=0;x<CBW.length;x++){ CBW[x].disabled = true; }
                document.getElementById('RAll').disabled = true;
                document.getElementById('WAll').disabled = true;
            }
            console.log(itemUser);

            if(ApplicationUser.Grants.User.Write===false){
                document.getElementById("TxtUserName").disabled = true;
                document.getElementById("TxtUserEmail").disabled = true;
                document.getElementById("CmbEmployee").disabled = true;
                document.getElementById("CmbEmployee").style.backgroundColor = "#f5f5f5";

                $("#BtnSave").hide();
                $("#Button1").hide();
                $("#ResetH4").hide();
                $("#ResetAlert").hide();
            }

            if (user.Id === itemUser.Id) {
                $("#Button1").attr("disabled", "disabled");
                $("#ResetAlert").html(Dictionary.Item_User_Help_ResetPasswordOwner);
            }
        </script>
</asp:Content>

