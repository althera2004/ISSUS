<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="DepartmentView.aspx.cs" Inherits="DepartmentView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        departmentId = <%=this.DepartmentId %>;
        companyId = <%=this.Company.Id %>;
        userId = <%=this.User.Id %>;
        departments = [<%=this.Departments %>];
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
                                                            <label id="TxtNameLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Common_Name"]%><span style="color:#f00">*</span></label>
                                                            <%=this.TxtName %>
                                                        </div>

                                                        <% if (this.DepartmentId != -1)
                                                           { %>
                                                        <h4><%=this.Dictionary["Item_Department_ListEmployees"]%></h4>											
                                                        <table class="table table-bordered table-striped">
                                                            <thead class="thin-border-bottom">
                                                                <tr>
                                                                    <th style="cursor:pointer;" onclick="Sort(this,'InternalLearningDataTable');" id="th0" class=""><%=this.Dictionary["Item_Department_ListEmployees_Header_Name"]%></th>
                                                                    <th class="hidden-480" style="width:90px;"><%=this.Dictionary["Item_Department_ListEmployees_Header_Nif"]%></th>
                                                                    <th class="hidden-480"><%=this.Dictionary["Item_Department_ListEmployees_Header_Email"]%></th>
                                                                    <th class="hidden-480" style="width:120px;"><%=this.Dictionary["Item_Department_ListEmployees_Header_Phone"]%></th>
                                                                    <th style="width:45px;">&nbsp;</th>						
                                                                </tr>
                                                            </thead>
                                                            <tbody id="InternalLearningDataTable">
                                                                <asp:Literal runat="server" ID="TableEmployees"></asp:Literal>
                                                            </tbody>
                                                        </table>

                                                        <h4><%=this.Dictionary["Item_Department_ListJobPositions"]%></h4>											
                                                        <table class="table table-bordered table-striped">
                                                            <thead class="thin-border-bottom">
                                                                <tr>
                                                                    <th style="cursor:pointer;" onclick="Sort(this,'TableJobPositionDataTable');" id="th0" class=""><%=this.Dictionary["Item_Department_ListJobPositions_Header_Name"]%></th>
                                                                    <th style="width:40px;">&nbsp;</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody id="TableJobPositionDataTable">
                                                                <asp:Literal runat="server" ID="TableJobPosition"></asp:Literal>
                                                            </tbody>
                                                        </table>
                                                        <% } %>
                                                        <%=this.FormFooter %>
                                                    </form>
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
                            <div id="DepartmentDesassociationDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Employee_Popup_UnlinkDepartment_Message"]%><strong>&nbsp;<span id="DepartmentDesassociationText"></span></strong></p>
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
    <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
    <script type="text/javascript" src="/js/Departments.js?<%=this.AntiCache %>"></script>
</asp:Content>

