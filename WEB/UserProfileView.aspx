<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="UserProfileView.aspx.cs" Inherits="UserProfileView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .sidebar1{width:190px;float:left;position:relative;border:1px solid #ccc;border-width:0 1px 0 0;background-color:#f2f2f2}
        .sidebar1:before{content:"";display:block;width:190px;position:fixed;z-index:-1;background-color:#f2f2f2;border:1px solid #ccc;border-width:0 1px 0 0}
        .avatar{float:left;padding:4px;background-color:#fff;border:1px solid #ccc;}
        .avatarSelected{float:left;padding:4px;background-color:#0f0;border:1px solid #0f0;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var CompanyCode = "<%=this.CompanyCode %>";
        var User = <%=this.UserJson %>;
        var Employee = {};
        var colorBlue = Dictionary.Common_Color_Blue;
        var colorRed = Dictionary.Common_Color_Red;
        var colorGreen = Dictionary.Common_Color_Green;
        var colorYellow = Dictionary.Common_Color_Yellow;
        var shorcuts = <%=this.ShortcutsJson %>;
        var userShortCuts = <%=this.UserShortcuts %>;
        var ColorSelected="";
        var HomePage = "<%=this.HomePage%>";

        function SaveProfile()
        {
            var showHelp = document.getElementById("chkShowHelp").checked;
            var language = document.getElementById("CmbIdioma").value;

            var blue = null;
            var green = null;
            var red = null;
            var yellow = null;

            for(var x=0; x<userShortCuts.length;x++)
            {
                if(userShortCuts[x].Color == "Blue")
                {
                    blue = userShortCuts[x].Id == "" ? null : userShortCuts[x].Id;
                    break;
                }
            }

            for(var x=0; x<userShortCuts.length;x++)
            {
                if(userShortCuts[x].Color == "Green")
                {
                    green = userShortCuts[x].Id == "" ? null : userShortCuts[x].Id;
                    break;
                }
            }

            for(var x=0; x<userShortCuts.length;x++)
            {
                if(userShortCuts[x].Color == "Yellow")
                {
                    yellow = userShortCuts[x].Id == "" ? null : userShortCuts[x].Id;
                    break;
                }
            }

            for(var x=0; x<userShortCuts.length;x++)
            {
                if(userShortCuts[x].Color == "Red")
                {
                    red = userShortCuts[x].Id == "" ? null : userShortCuts[x].Id;
                    break;
                }
            }

            var data = {
                "userId": User.Id,
                "language": language,
                "showHelp": showHelp,
                "blue": blue,
                "green": green,
                "yellow": yellow,
                "red": red,
                "companyId": Company.Id
            };

            LoadingShow("");
            $.ajax({
                type: "POST",
                url: "/Async/LoginActions.asmx/SaveProfile",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data, null, 2),
                success: function (response) {
                    LoadingHide();
                    if (response.d.Success === true) {
                        document.location = HomePage;
                    }
                    if (response.d.Success !== true) {
                        alertUI(response.d.MessageError);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    LoadingHide();
                    alertUI(jqXHR.responseText);
                }
            });
        }

        function CmbShortcutsChanged(id)
        {
            var shortcut = GetShortcutById(id);
            if(id==0)
            {        
                document.getElementById("Icon" + ColorSelected).className = "";
                document.getElementById("Button" + ColorSelected).title = "";
                for(var x=0; x<userShortCuts.length;x++)
                {
                    if(userShortCuts[x].Color == ColorSelected)
                    {
                        userShortCuts[x].Id = "";
                    }
                }
                return false;
            }

            if(shortcut!=null)
            {
                document.getElementById("Icon" + ColorSelected).className = shortcut.Icon;
                document.getElementById("Button" + ColorSelected).title = shortcut.Label;
                for(var x=0; x<userShortCuts.length;x++)
                {
                    if(userShortCuts[x].Color == ColorSelected)
                    {
                        userShortCuts[x].Id = shortcut.Id;
                    }
                }
            }
        }

        function GetShortcutById(id)
        {
            for(var x=0; x<shorcuts.length;x++)
            {
                if(shorcuts[x].Id == id)
                {
                    return shorcuts[x];
                }
            }

            return null;
        }

        function GetShortcutByName(name)
        {
            for(var x=0; x<shorcuts.length;x++)
            {
                if(shorcuts[x].Label == name)
                {
                    return shorcuts[x];
                }
            }

            return null;
        }

        function SetCmbShortcutsValue(value)
        {    
            var comboItems = document.getElementById("Contentholder1_CmbShorcuts").childNodes
            for (var x = 0; x < comboItems.length; x++) {
                var item = comboItems[x];
                if (item.tagName == "OPTION") {
                    if (item.value == value) {
                        item.selected = true;
                    }
                    else {
                        item.selected = false;
                    }
                }
            }
        }

        function select(id, name)
        {
            ColorSelected = id;
            var shortcut = GetShortcutByName(name);
            eval("document.getElementById('ColorSelectedSpan').innerHTML = color" + id + ";");
            if(shortcut!= null && shortcut.Id != null)
            {
                SetCmbShortcutsValue(shortcut.Id);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <ul class="nav nav-tabs padding-18">
                                                <li class="active">
                                                    <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_User_Tab_Principal"] %></a>
                                                </li>
                                                <li class="" id="TabDepartments">
                                                    <a data-toggle="tab" href="#password"><%=this.Dictionary["Item_User_Tab_Password"] %></a>
                                                </li>
                                            </ul>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="home" class="tab-pane active"> 
                                                    <h4><%=this.Dictionary["Item_Profile_Title_PersonalData"]%></h4>
                                                    <div class="profile-user-info profile-user-info-striped">
                                                        <div class="profile-info-row">
                                                            <div class="profile-info-name"><%=this.Dictionary["Item_Profile_FieldLabel_Name"] %></div>
                                                            <div class="profile-info-value"><span class="control-label" id="username"><%=this.Description %></span></div>
                                                        </div>

                                                        <div class="profile-info-row">
                                                            <div class="profile-info-name">Email</div>
                                                            <div class="profile-info-value"><span class="control-label"><%=this.ApplicationUser.Email %>&nbsp;</span></div>
                                                        </div>
                                                    </div>     
                                                    <hr />                                                    
                                                    <div>
                                                        <h4><%=this.Dictionary["Item_User_Profile_Title_Settings"]%></h4>
                                                        <div class="form-group">
                                                            <label class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Profile_FieldLabel_Language"] %></label>
                                                            <div class="col-xs-3" id="DivCmbIdioma" style="height:35px !important;">
                                                                <select id="CmbIdioma" class="col-xs-12">
                                                                    <asp:Literal runat="server" ID="LtIdiomas"></asp:Literal>
                                                                </select>
                                                            </div>
                                                            <div class="col-xs-1">&nbsp;</div>
                                                            <label class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Profile_ShowHelp"] %></label>
                                                            <div class="col-xs-3">
                                                                <label>
                                                                    <input name="chkShowHelp" id="chkShowHelp" class="ace ace-switch ace-switch-4" type="checkbox" <%=this.ShowHelpChecked %> />
                                                                    <span class="lbl"></span>
                                                                </label>
                                                            </div>
                                                            <div class="col-xs-2">&nbsp;</div>
                                                        </div>
                                                    </div>
                                                    <br /><hr />
                                                    <div>
                                                        <h4><%=this.Dictionary["Item_Profile_ShortCuts"]%></h4>
                                                        <div class="form-group">
                                                            <div class="col-sm-2">
                                                                <div class="sidebar1" id="sidebar1" style="width:250px;">
                                                                    <!-- #sidebar-shortcuts -->
                                                                    <asp:Literal runat="server" ID="LtMenuShortCuts"></asp:Literal>
                                                                </div>
                                                            </div>
                                                            <label class="col-sm-1 control-label no-padding-right" for="form-field-1" id="ColorSelectedSpan"></label>
                                                            <div class="col-sm-4"><asp:DropDownList runat="server" ID="CmbShorcuts"></asp:DropDownList></div>
                                                        </div>
                                                    </div> 
                                                    <div class="row">
                                                        <div class="col-xs-12"></div>
                                                    </div> 
                                                    <div >
                                                        <h4><%=this.Dictionary["Avatar"]%></h4>
                                                        <div class="col-xs-12">
                                                            <div id="TableAvatars"><asp:Literal runat="server" ID="LtAvatar"></asp:Literal></div>                                                           
                                                        </div>   
                                                    </div>   
                                                    <div class="row">
                                                        <div class="col-xs-12"></div>
                                                    </div>
                                                    <%=this.FormFooter %>
                                                </div>
                                                <div id="password" class="tab-pane">	                                            
                                                    <form class="form-horizontal" role="form">
                                                        <h4><%=this.Dictionary["Item_User_ChangePassword"] %></h4>
                                                        <div class="form-group">
                                                            <label id="TxtPassActualLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_User_ActualPassword"]%></label>
                                                            <div class="col-sm-4">
                                                                <input type="password" placeholder="<%=this.Dictionary["Item_User_ActualPassword"] %>" id="TxtPassActual" value="" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtPassActualErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtPassNew1Label" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_User_NewPassword"]%></label>
                                                            <div class="col-sm-4">
                                                                <input type="password" placeholder="<%=this.Dictionary["Item_User_NewPassword"] %>" id="TxtPassNew1" value="" class="col-xs-12 col-sm-12" />
                                                                <span class="ErrorMessage" id="TxtPassNew1ErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                                <span class="ErrorMessage" id="TxtPassNew1ErrorMatch" style="display:none;"><%=this.Dictionary["Item_User_ErrorMessage_PaswordsNoMatch"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtPassNew2Label" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_User_FieldLabel_PasswordConfirm"]%></label>
                                                            <div class="col-sm-4">
                                                                <input type="password" placeholder="<%=this.Dictionary["Item_User_FieldLabel_PasswordConfirm"] %>" id="TxtPassNew2" value="" class="col-xs-12 col-sm-12"/>
                                                                <span class="ErrorMessage" id="TxtPassNew2ErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                                <span class="ErrorMessage" id="TxtPassNew2ErrorMatch" style="display:none;"><%=this.Dictionary["Item_User_ErrorMessage_PaswordsNoMatch"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="alert alert-danger">
                                                            <strong><%=this.Dictionary["Common_Warning"] %></strong>
                                                            <%=this.Dictionary["Item_User_Message_AfterChangePassword"] %><br />
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-12 control-label no-padding-right">
                                                                <button class="btn btn-success" type="button" id="BtnPasswordOk">
                                                                    <i class="icon-lock bigger-110"></i>
                                                                    <%=this.Dictionary["Item_User_ChangePassword"] %>
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </form> 
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
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
        <script type="text/javascript">
            var selectedAvatar = "";
            function SelectAvatar(name) {
                selectedAvatar = name;
                var data = {
                    "avatar": name,
                    "userId": user.Id,
                    "companyId": Company.Id
                };

                LoadingShow("");
                $.ajax({
                    type: "POST",
                    url: "/Async/LoginActions.asmx/Changeavatar",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (response) {
                        LoadingHide();
                        if (response.d.Success === true) {
                            for (var x = 0; x < document.getElementById("TableAvatars").childNodes.length; x++) {
                                document.getElementById("TableAvatars").childNodes[x].className = "avatar";
                            }

                            document.getElementById(selectedAvatar).className = "avatarSelected";
                        }
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        LoadingHide();
                        alertUI(jqXHR.responseText);
                    }
                });
            }

            function ChangePassword() {
                var ok = true;                
                if(!RequiredFieldText("TxtPassActual")) { ok = false; }
                if(!RequiredFieldText("TxtPassNew1")) { ok = false; }
                if(!RequiredFieldText("TxtPassNew2")) { ok = false; }
                if(!MatchRequiredBothFieldText("TxtPassNew1","TxtPassNew2")) { ok = false; }

                if(ok===false) {
                    window.scrollTo(0, 0); 
                    return false;
                }

                var data = {
                    "oldPassword": $("#TxtPassActual").val(),
                    "newPassword": $("#TxtPassNew1").val(),
                    "userId": user.Id,
                    "companyId": Company.Id
                };

                LoadingShow('');
                $.ajax({
                    type: "POST",
                    url: "/Async/LoginActions.asmx/ChangePassword",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (response) {
                        LoadingHide();
                        if (response.d.Success === true) {
                            document.location = "LogOut.aspx?company=" + CompanyCode;
                        }
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        LoadingHide();
                        alertUI(jqXHR.responseText);
                    }
                });
            }

            jQuery(function ($) {
                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || "&nbsp;"
                        if (("title_html" in this.options) && this.options.title_html == true) {
                            title.html($title);
                        }
                        else {
                            title.text($title);
                        }
                    }
                }));
                
                <%if(this.ShowHelp) { %>
                SetToolTip("DivCmbIdioma","<%=this.Dictionary["Item_Employee_Help_Language"] %>");
                SetToolTip("chkShowHelp","<%=this.Dictionary["Item_Employee_Help_OnlineHelp"] %>");
                SetToolTip("ButtonBlue","<%=this.Dictionary["Item_Employee_Help_IconCommon_Color_Blue"] %>");
                SetToolTip("ButtonGreen","<%=this.Dictionary["Item_Employee_Help_IconCommon_Color_Green"] %>");
                SetToolTip("ButtonRed","<%=this.Dictionary["Item_Employee_Help_IconCommon_Color_Red"] %>");
                SetToolTip("ButtonYellow","<%=this.Dictionary["Item_Employee_Help_IconCommon_Color_Yellow"] %>");
                SetToolTip("Contentholder1_CmbShorcuts","<%=this.Dictionary["Item_Employee_Help_Shortcuts"] %>");
                SetToolTip("TxtPassActual","<%=this.Dictionary["Item_Employee_Help_PasswordActual"] %>");
                SetToolTip("TxtPassNew1","<%=this.Dictionary["Item_Employee_Help_NewPassword"] %>");
                SetToolTip("TxtPassNew2","<%=this.Dictionary["Item_Employee_Help_ConfirmarNuevoPassword"] %>");

                $("[data-rel=tooltip]").tooltip();
                $("[data-rel=popover]").popover({ container: "body" });
                <% } %>

                $("#BtnPasswordOk").click(ChangePassword);
                $("#BtnSave").click(SaveProfile);
                $("#BtnCancel").click(function (e) { document.location = "DashBoard.aspx"; });
            });
        </script>
</asp:Content>