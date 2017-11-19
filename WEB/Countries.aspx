<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="Countries.aspx.cs" Inherits="Countries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <h4><%=this.Dictionary["Item_CompanyCountries_Status_Selected"]%></h4>
								<div class="row">
									<div class="col-xs-12">
                                        <asp:Literal runat="server" ID="LtSelected"></asp:Literal>
                                    </div>
                                </div>
                                <br />
                                <br />
                                <h4><%=this.Dictionary["Item_CompanyCountries_Status_Availables"]%></h4>
                                <div class="form-group">
								    <label class="col-sm-1 control-label" id="TxtNameLabel"><%=this.Dictionary["Buscar"] %></label>                                        
								    <div class="col-sm-4">
                                        <input type="text" id="TxtName" placeholder="<%=this.Dictionary["Nombre"] %>" class="col-xs-12 col-sm-12" value="" maxlength="50" onblur="this.value=$.trim(this.value);" onkeyup="filter();" />                                        
                                    </div>
							    </div>                               
								<div class="row">
									<div class="col-xs-12" id="AvailablesDiv">
                                        <asp:Literal runat="server" ID="LtAvailables"></asp:Literal>
                                    </div>
                                </div>
                                <%=this.FormFooter %>
                                <input type="text" id="SelectedCountries" style="display:none;" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
		<script type="text/javascript" src="assets/js/jquery.ui.touch-punch.min.js"></script>
		<script type="text/javascript" src="assets/js/chosen.jquery.min.js"></script>
		<script type="text/javascript" src="assets/js/fuelux/fuelux.spinner.min.js"></script>
		<script type="text/javascript" src="assets/js/date-time/bootstrap-datepicker.min.js"></script>
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
            function filter() {
                var pattern = document.getElementById('TxtName').value.toLowerCase();
                for (var x = 0; x < document.getElementById('AvailablesDiv').childNodes.length; x++) {
                    var tag = document.getElementById('AvailablesDiv').childNodes[x];
                    if (tag.tagName === 'DIV') {
                        if (tag.childNodes[2].innerHTML.toLowerCase().indexOf(pattern) != -1) {
                            tag.style.display = '';
                        }
                        else {
                            tag.style.display = 'none';
                        }
                    }
                }
            }

            function SelectCountry(sender) {
                if (sender.checked === true) {
                    document.getElementById('SelectedCountries').value += sender.id + '|';
                }
                else {
                    document.getElementById('SelectedCountries').value = document.getElementById('SelectedCountries').value.split(sender.id + '|').join('');
                }
            }

            function SaveCountries() {
                var webMethod = "/Async/CompanyActions.asmx/SelectCountries";
                var data = {
                    'countries': document.getElementById('SelectedCountries').value,
                    'companyId': Company.Id
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
                        else {
                            document.location = document.location + '';
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
                        var $title = this.options.title || '&nbsp;'
                        if (("title_html" in this.options) && this.options.title_html == true)
                            title.html($title);
                        else title.text($title);
                    }
                }));

                $('#BtnSave').click(SaveCountries);
                $('#BtnCancel').click(function (e) { document.location = referrer; });
            });
        </script>
</asp:Content>

