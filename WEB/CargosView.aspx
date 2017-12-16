<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="CargosView.aspx.cs" Inherits="CargosView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <script type="text/javascript">
        var cargo = <%=this.CargoJson %>;
        var SelectedResponsible = cargo.Responsible.Id;
        var SelectedDepartment = cargo.Department.Id;
        var first = true;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div id="user-profile-2" class="user-profile">
                                    <div class="tabbable">
                                        <% if (!this.NewJobPosition)
                                           { %>
                                        <ul class="nav nav-tabs padding-18">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_JobPosition_Tab_Principal"]%></a>
                                            </li>
                                            <!--<% if (this.GrantTraces) { %>
                                            <li class="" id="TabTrazas">
                                                <a data-toggle="tab" href="#trazas"><%=this.Dictionary["Item_JobPosition_Tab_Traces"]%></a>
                                            </li>
                                            <% } %>-->
                                        </ul>
                                        <% } %>
                                        <div class="tab-content no-border padding-24">
                                            <div id="home" class="tab-pane active">                                                
                                                <div class="form-horizontal" role="form">
                                                    <div class="form-group">
                                                        <label class="col-sm-1 control-label no-padding-right" style="margin-right:15px;" id="TxtNameLabel"><%=this.Dictionary["Item_JobPosition_FieldLabel_Name"] %></label>                                        
                                                        <%=this.TxtName %>					                    
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="TxtDepartmentNameLabel" class="col-sm-1" style="margin-right:15px;"><%=this.Dictionary["Item_JobPosition_FieldLabel_Department"]%></label>
                                                        <!--<%=this.BarDepartment %>-->
                                                        <div class="col-sm-4" id="DivCmbDepartment" style="height:35px !important;">
                                                            <select id="CmbDepartment" class="col-xs-12 col-sm-12 tooltip-info" onchange="CmbDepartmentChanged();"></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="TxtDepartmentName" placeholder="<%=this.Dictionary["Item_JobPosition_FieldLabel_Department"] %>" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="TxtDepartmentNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <div class="col-sm-1"><span class="btn btn-light" style="height:30px;" id="BtnDepartment" title="<%=this.Dictionary["Item_JobPosition_Button_DepartmentsBAR"] %>">...</span></div>
                                                            
                                                    <!--/div-->
                                                    <div class="col-sm-5" style="display:none;">										
                                                            <select class="form-control col-xs-12 col-sm-12 tooltip-info" id="CmbDepartamentos" data-placeholder="<%=this.Dictionary["Item_Employees"] %>" onchange="GetEmployeesByDepartment(this.value);">
                                                                <asp:Literal runat="server" ID="LtDepartamentos"></asp:Literal>
                                                            </select>
                                                            <span class="ErrorMessage" id="CmbDepartamentosErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"]%></span>
                                                        </div>
                                                    <!--div class="form-group"-->
                                                        <label class="col-sm-1 control-label no-padding-right" style="margin-right:15px;" id="CmbEmpleadosLabel"><%=this.Dictionary["Common_Responsible"] %></label>                                        
                                                        <%=CmbResponsible %>
                                                    </div>
                                                    <div class="for-group">
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_JobPosition_FieldLabel_Responsabilities"] %></label>
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_JobPosition_FieldLabel_Notes"] %></label>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtResponsabilidades"><%=this.Cargo.Responsibilities %></textarea></div>
                                                        <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtNotas"><%=this.Cargo.Notes %></textarea></div>
                                                    </div>
                                                    <div class="for-group">
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_JobPosition_FieldLabel_Academic_Learning"] %></label>
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_JobPosition_FieldLabel_Specific_Learning"] %></label>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtFormacionAcademicaDeseada"><%=this.Cargo.AcademicSkills %></textarea></div>
                                                        <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtFormacionEspecificaDeseada"><%=this.Cargo.SpecificSkills %></textarea></div>
                                                    </div>
                                                    <div class="for-group">
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_JobPosition_FieldLabel_Work_Experience"] %></label>
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_JobPosition_FieldLabel_Habilities"] %></label>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtExperienciaLaboral"><%=this.Cargo.WorkExperience %></textarea></div>
                                                        <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtHabilidades"><%=this.Cargo.Habilities %></textarea></div>
                                                    </div> 
                                                    <% if (!this.NewJobPosition)
                                                        { %>
                                                    <h4><%=this.Dictionary["Item_JobPosition_ListEmployees_Title"]%></h4>											
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th><%=this.Dictionary["Item_JobPosition_ListEmployees_Header_Name"]%></th>
                                                                <th style="width:90px;"><%=this.Dictionary["Item_JobPosition_ListEmployees_Header_Nif"]%></th>
                                                                <th><%=this.Dictionary["Item_JobPosition_ListEmployees_Header_Email"]%></th>
                                                                <th style="width:120px;"><%=this.Dictionary["Item_JobPosition_ListEmployees_Header_Phone"]%></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="InternalLearningDataTable">
                                                            <asp:Literal runat="server" ID="TableEmployees"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                    <% } %>
                                                    <%=this.FormFooter %>
                                                </div>
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
                            <div id="dialogDepartment" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped">
                                        <thead class="thin-border-bottom">
                                            <tr>
                                                <th><i class="icon-caret-right blue"></i><%=this.Dictionary["Item_JobPosition_List_Departments_Department"] %></th>
                                                <th style="width:150px;white-space:nowrap;">&nbsp;</th>													
                                            </tr>
                                        </thead>
                                        <tbody id="DepartmentsTablePopup"></tbody>
                                    </table>
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->
                            <div id="DepartmentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Department_PopupDelete_Message"] %>&nbsp;<strong><span id="DepartmentName"></span></strong>?</p>
                            </div>
                            <div id="DepartmentUpdateDialog" class="hide" style="width:600px;">
                                <p>
                                    <label class="col-sm-2 control-label no-padding-right" id="TxtDepartmentUpdateNameLabel"><%=this.Dictionary["Item_JobPosition_FieldLabel_Name"] %>:</label> 
                                    &nbsp;&nbsp;
                                    <input type="text" id="TxtDepartmentUpdateName" size="50" placeholder="<%= this.Dictionary["Item_JobPosition_FieldLabel_Name"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                </p>
                                <span class="ErrorMessage" id="TxtDepartmentUpdateNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentUpdateNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
                            <div id="DepartmentInsertDialog" class="hide" style="width:600px;">
                                <p>
                                    <label class="col-sm-2 control-label no-padding-right" id="TxtDepartmentNewNameLabel"><%=this.Dictionary["Item_JobPosition_FieldLabel_DepartmentName"] %></label> 
                                    &nbsp;&nbsp;
                                    <input type="text" id="TxtDepartmentNewName" size="50" placeholder="<%= this.Dictionary["Item_JobPosition_FieldLabel_DepartmentName"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                </p>
                                <span class="ErrorMessage" id="TxtDepartmentNewNameErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentNewNameErrorDuplicated" style="display:none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/CargosView.js?<%=this.AntiCache %>"></script>
</asp:Content>