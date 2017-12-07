<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="CostDefinitionView.aspx.cs" Inherits="CostDefinitionView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var ItemData = <%=this.CostDefinitionItem.Json %>;
        var CostDefinitions = [<%= CostDefinitions %>];
        var CostDefinitionId = <%=this.CostDefinitionId %>;
        userId = <%=this.User.Id %>;
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
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Description"]%><span style="color:#f00">*</span></label>
                                                            <%=this.TxtName %>
                                                        </div>
                                                        <div class="form-group">
                                                            <label id="TxtAmountLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_CostDefinition_Field_Amount"]%></label>                                                            
                                                            <div class="col-sm-2">                                                                                                                            
                                                                <input type="text" id="TxtAmount" placeholder="<%=this.Dictionary["Item_CostDefinition_Field_Amount"]%>" class="col-xs-12 col-sm-12 tooltip-info money-bank" value="" onblur="this.value=$.trim(this.value);" />
                                                                <span class="ErrorMessage" id="TxtAmountErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                                            </div>	
                                                        </div>
                                                        <%=this.FormFooter %>
                                                    </form>
                                                </div>
                                                <div id="trazas" class="tab-pane">
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th style="width: 150px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"]%></th>
                                                                <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"]%></th>
                                                                <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"]%></th>
                                                                <th style="width: 250px;"><%= this.Dictionary["Item_Tace_ListHeader_User"]%></th>
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
        function Save() {
            var ok = true;     
            document.getElementById('TxtNameLabel').style.color = '#000';       
            document.getElementById('TxtNameErrorRequired').style.display = 'none';
            document.getElementById('TxtNameErrorDuplicated').style.display = 'none';
            document.getElementById('TxtAmountLabel').style.color = '#000';       
            document.getElementById('TxtAmountErrorRequired').style.display = 'none';

            if (!RequiredFieldText('TxtName')) { ok = false; }            
            else
            {
                var duplicated = false;
                for(var x=0; x<CostDefinitions.length; x++)
                {
                    var description = CostDefinitions[x].Description.toLowerCase();
                    if(description == document.getElementById('TxtName').value.toLowerCase() && CostDefinitions[x].Id != CostDefinitionId)
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

            if (!RequiredFieldText('TxtAmount')) { ok = false; } 

            if (ok === true) {
                var webMethod = CostDefinitionId === -1 ?"/Async/CostDefinitionActions.asmx/Insert": "/Async/CostDefinitionActions.asmx/Update";
                var data = {
                    costDefinition:
                    {
                        "Id": CostDefinitionId,
                        "Description": $('#TxtName').val(),
                        "Amount": ParseInputValueToNumber($('#TxtAmount').val()),
                        "CompanyId": Company.Id
                    },
                    'userId': user.Id
                };     

                LoadingShow(Dictionary.Common_Message_Saving);
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

            return false;
        }

        jQuery(function ($) {
            $('#BtnSave').click(Save);
            $('#BtnCancel').click(function (e) { document.location = referrer; });

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

        // ISSUS-190
        document.getElementById('TxtName').focus();

        if(ItemData.Amount !== null)
        {
            $('#TxtAmount').val(ToMoneyFormat(ItemData.Amount,2));
        }

        

        if (typeof ApplicationUser.Grants.Cost === "undefined" || ApplicationUser.Grants.Cost.Write === false) {
            $(".btn-danger").hide();
            $("input").attr("disabled", true);
            $("textarea").attr("disabled", true);
            $("select").attr("disabled", true);
            $("select").css("background-color", "#eee");
            $("#BtnSave").hide();
            $(".ui-slider-handle").hide();
        }
    </script>
</asp:Content>

