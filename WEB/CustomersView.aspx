<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="CustomersView.aspx.cs" Inherits="CustomersView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var itemId = <%=this.CustomerId %>;
        var companyId = <%=this.Company.Id %>;
        var userId = <%=this.User.Id %>;
        pageItems = [<%=this.Customers %>];
        var customer = <%=this.CustomerItem.Json %>;
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
                                            <%=this.TabBar %>
                                            <div class="tab-content no-border padding-24">
                                                <div id="home" class="tab-pane active">                                                
                                                    <form class="form-horizontal" role="form">
                                                        <div class="form-group">
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Customer"]%><span style="color:#f00">*</span></label>
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
            document.getElementById("TxtNameLabel").style.color = "#000";
            document.getElementById("TxtNameErrorRequired").style.display = "none";            
            document.getElementById("TxtNameErrorDuplicated").style.display = "none";
            if (!RequiredFieldText("TxtName")) { ok = false; }            
            else
            {
                var duplicated = false;
                for(var x=0; x<pageItems.length; x++)
                {
                    var description = pageItems[x].Description.toLowerCase();
                    if (description == document.getElementById('TxtName').value.toLowerCase() && pageItems[x].Id != itemId)
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
                var newCustomer = 
                {
                    "Id": customer.Id,
                    "Description": document.getElementById("TxtName").value,
                    "Active": customer.Active,
                    "Deletable": customer.Deletable,
                    "CompanyId": customer.CompanyId
                }
                var webMethod = "/Async/CustomerActions.asmx/Update";
                var data = {
                    "oldCustomer": customer,
                    "newCustomer": newCustomer,
                    "userId": user.Id
                };

                if(itemId === -1)
                {
                    webMethod = "/Async/CustomerActions.asmx/Insert"
                    var data = {
                        "description": document.getElementById("TxtName").value,
                        "companyId": Company.Id,
                        "userId": user.Id
                    };
                }                

                LoadingShow(Dictionary.Common_Message_Saving);
                $.ajax({
                    "type": "POST",
                    "url": webMethod,
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": JSON.stringify(data, null, 2),
                    "success": function (response) {
                        LoadingHide();
                        if (response.d.Success !== true) {
                            alertUI(response.d.MessageError);
                        }
                        else
                        {
                            document.location = referrer;
                        }
                    },
                    "error": function (jqXHR) {
                        LoadingHide();
                        alertUI(jqXHR.responseText);
                    }
                });
            }
        }

        jQuery(function ($) {

            $("#BtnSave").on("click", Save);
            $("#BtnCancel").on("click", function () { document.location = referrer; });

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
            SetToolTip("TxtName","<%=this.Dictionary["Item_Customer_Help_Name"] %>");
            $("[data-rel=tooltip]").tooltip();
            <% } %>
        });


        if (ApplicationUser.Grants.Customer.Write === false) {
            $("#BtnSave").hide();
        }
        else{            
            // ISSUS-190
            document.getElementById("TxtName").focus();
        }
    </script>
</asp:Content>