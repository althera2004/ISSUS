<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ProvidersView.aspx.cs" Inherits="ProvidersView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        providerId = <%=this.ProviderId %>;
        companyId = <%=this.Company.Id %>;
        userId = <%=this.User.Id %>;
        providers = [<%=this.Providers %>];
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">

                            <div>
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <%=this.TabBar %>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Nombre"]%></label>
                                                            <%=this.TxtName %>
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
        <script type="text/javascript" src="/assets/js/date-time/bootstrap-datepicker.min.js"></script>
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
        function Save() {
            var ok = true;
            if (!RequiredFieldText("TxtName")) { ok = false; }            
            else
            {
                var duplicated = false;
                for(var x=0; x<providers.length; x++)
                {
                    var description = providers[x].Value.toLowerCase();
                    if(description == document.getElementById("TxtName").value.toLowerCase() && providers[x].Id != providerId)
                    {
                        duplicated = true;
                        break;
                    }
                }

                if(duplicated===true) 
                { 
                    document.getElementById('TxtNameLabel').style.color = '#f00';
                    document.getElementById('TxtNameErrorDuplicated').style.display = 'block';
                    ok = false; 
                }
                else
                {
                    document.getElementById('TxtNameLabel').style.color = '#000';
                    document.getElementById('TxtNameErrorDuplicated').style.display = 'none';
                }
            }

            if (ok === false) {
                window.scrollTo(0, 0); 
                return false;
            }
            else {
                var webMethod = "/Async/ProviderActions.asmx/Update";
                var data = {
                    'providerId': providerId,
                    'description': document.getElementById('TxtName').value,
                    'companyId': companyId,
                    'userId': userId
                };

                if(providerId === -1)
                {
                    webMethod = "/Async/ProviderActions.asmx/Insert"
                    var data = {
                    'description': document.getElementById('TxtName').value,
                    'companyId': company.Id,
                    'userId': user.Id
                    };
                }                

                LoadingShow(Dictionary.Saving);
                $.ajax({
                    type: "POST",
                    url: webMethod,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(data, null, 2),
                    success: function (response) {
                        LoadingHide();
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                        else
                        {
                            document.location = referrer;
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        LoadingHide();
                        alertUI(jqXHR.responseText);
                    }
                });
            }
        }

            jQuery(function ($) {

                $('#BtnSave').click(Save);
                $('#BtnCancel').click(function (e) { 
                            //document.location = document.referrer;
                            document.location = referrer; });

                //override dialog's title function to allow for HTML titles
                $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                    _title: function (title) {
                        var $title = this.options.title || '&nbsp;'
                        if (("title_html" in this.options) && this.options.title_html == true)
                            title.html($title);
                        else title.text($title);
                    }
                }));
                

                <%if(this.ShowHelp) { %>
                SetToolTip('TxtName',"<%=this.Dictionary["Item_Provider_Help_Name"] %>");

                $('[data-rel=tooltip]').tooltip();
                <% } %>
             });
    </script>
</asp:Content>

