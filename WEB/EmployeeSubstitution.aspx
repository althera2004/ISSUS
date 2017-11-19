<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EmployeeSubstitution.aspx.cs" Inherits="EmployeeSubstitution" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript" src="js/common.js"></script><script type="text/javascript" src="js/common.js"></script>
    <script type="text/javascript">
        var companyId = <%=this.CompanyId %>;
        var employeeId = <%=this.EmployeeId %>;
        var asignations = <%=this.Employee.EmployeeActions %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
    
                            <div style="margin-bottom:30px;">
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <%=this.TabBar %>
                                            <div class="tab-content no-border padding-24" style="height:500px;">
                                                <div id="home" class="tab-pane active">    
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive">
                                                                <h4><%=this.Dictionary["Item_Employee_List_Delete"]%></h4>
                                                                <table class="table table-bordered table-striped">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr>
                                                                            <th style="width:300px;"><%=this.Dictionary["Item_Employee_List_Delete_Header_Type"]%></th>
                                                                            <th><%=this.Dictionary["Item_Employee_List_Delete_Header_Description"]%></th>
                                                                            <!-- ISSUS-101 <th style="width:200px;"><%=this.Dictionary["Item_Employee_List_Delete_Header_Substitution"]%></th> -->
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody id="TableEmployeeElements"></tbody>
                                                                </table>
                                                            </div><!-- /.table-responsive -->
                                                        </div><!-- /span -->
                                                        <div class="col-xs-12" style="border-top:1px solid #ccc;padding-top:6px;">
                                                            <label id="TxtNombreLabel" class="col-sm-4 control-label no-padding-right"><%=this.Dictionary["Item_Employee_List_Delete_FieldLabel_All"] %></label>
                                                            <div class="col-sm-3">
                                                                <select id="All" onchange="ChangeAll();"></select>
                                                            </div>
                                                        </div>
                                                    </div><!-- /row -->	
                                                    
                                                    <%=this.FormFooter %>
                                                    <div style="display:none;">
                                                        <textarea id="TxtData"></textarea>
                                                        <input type="text" id="TxtEndDate" value="<%=this.EndDate %>" />
                                                    </div>
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
        <script type="text/javascript" src="js/EmployeeSubstitution.js"></script>
</asp:Content>

