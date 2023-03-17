<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EmployeesView.aspx.cs" Inherits="EmployeesView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .iconfile {
            border: 1px solid #777;
            background-color: #fdfdfd;
            -webkit-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            -moz-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            padding-left:0!important;
            padding-top:4px !important;
            padding-bottom:4px !important;
            margin-bottom:12px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script><script type="text/javascript" src="js/common.js"></script>
    <link rel="stylesheet" href="/Document-Viewer/style.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/yepnope.1.5.3-min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/ttw-document-viewer.min.js"></script>
    <script type="text/javascript">
        var employeeId = <%=this.EmployeeId %>;
        var employee = <%=this.Employee.Json %>;
        var employeeSkills = <%=this.EmployeeSkills %>;
        var departmentsEmployee = <%= this.DepartmentsEmployeeJson %>;
        var jobPositionEmployee = <%= this.JobPositionEmployeeJson %>
        var ddData = [<%=this.CountryData %>];
        var EmployeeUserId = <%=this.EmployeeUserId %>;
        var EmployeeUserName = "<%=this.UserName %>";
        var SecurityGroups = <%=this.GroupsJson %>;
        var CompanyUserNames = [<%=this.CompanyUserNames %>];
        var newDepartmentDescription = "";
        var DepartmentSelected;
        var JobPositionSelected;
        var typeItemId = 5;
        var itemId = employeeId;
        var JobPositionCompany = <%=JobsPositionCompany %>;
        var SkillWorkExperienceValid = <%=this.SkillWorkExperienceValid %>;
        var SkillAcademicValid = <%=this.SkillAcademicValid %>;
        var SkillSpecificValid = <%=this.SkillSpecificValid %>;
        var SkillHabilityValid = <%=this.SkillHabilityValid %>;
        var HasActions = <%=this.Employee.HasActions ? "true" : "false" %>;
        var pageType = "form";
        var aprovedByActual = <%=this.AprovedByText %>;
        var active = <%=this.Active ? "true" : "false" %>;
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
                                                <div id="home" class="tab-pane<%=this.SelectedTab =="home" ? " active" : String.Empty %>">       
                                                    <div class="row">
                                                        <form class="form-horizontal" role="form">
                                                            <div class="form-group">
                                                                <label id="TxtNombreLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Name"] %><span style="color:#f00">*</span></label>
                                                                <div class="col-sm-3">
                                                                    <input type="text" id="TxtNombre" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Name"] %>" value="<%=this.Employee.Name %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtNombreErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtNombreErrorDuplicated"><%=this.Dictionary["Item_Employee_ErrorMessage_AmployeeAlreadyExists"] %></span>
                                                                </div>
                                                                <label id="TxtApellido1Label" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_LastName"] %><span style="color:#f00">*</span></label>
                                                                <div class="col-sm-4">
                                                                    <input type="text" id="TxtApellido1" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_LastName"] %>" value="<%=this.Employee.LastName %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtApellido1ErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtApellido1ErrorDuplicated"><%=this.Dictionary["Item_Employee_ErrorMessage_AmployeeAlreadyExists"] %></span>
                                                                </div>
                                                                <label id="TxtNifLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_NIF"]%></label>
                                                                <div class="col-sm-2">
                                                                    <input type="text" id="TxtNif" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_NIF"] %>" value="<%=this.Employee.Nif %>" class="col-xs-12 col-sm-12" maxlength="15" onblur="this.value=$.trim(this.value.toUpperCase());" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtNifErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtNifErrorMalformed"><%=this.Dictionary["Common_Error_IncorrectNIF"] %></span>
                                                                    <span class="ErrorMessage" id="TxtNifErrorDuplicated"><%=this.Dictionary["Item_Employee_ErrorMessage_NifAlreadyExists"]%></span>
                                                                </div>
                                                            </div>                                                            
                                                            <div class="form-group">
                                                                <label id="TxtTelefonoLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Phone"]%></label>
                                                                <div class="col-sm-3">
                                                                    <input type="text" id="TxtTelefono" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Phone"] %>" value="<%=this.Employee.Phone %>" class="col-xs-12 col-sm-12" onkeypress="validate(event)" maxlength="15" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtTelefonoErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                                <label id="TxtEmailLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Email"]%><span style="color:#f00">*</span></label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" id="TxtEmail" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Email"] %>" value="<%=this.Employee.Email %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtEmailErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    <span class="ErrorMessage" id="TxtEmailErrorMalformed"><%=this.Dictionary["Item_Employee_ErrorMessage_MailMalformed"] %></span>
                                                                    <span class="ErrorMessage" id="TxtEmailErrorDuplicated"><%=this.Dictionary["Item_Employee_ErrorMessage_AmployeeAlreadyExists"] %></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label id="TxtDireccionLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Address"]%></label>
                                                                <div class="col-sm-11">
                                                                    <input type="text" id="TxtDireccion" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Address"] %>" value="<%=this.Employee.Address.Address %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtDireccionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label id="TxtCpLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_PostalCode"]%></label>
                                                                <div class="col-sm-3">
                                                                    <input type="text" id="TxtCp" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_PostalCode"] %>" value="<%=this.Employee.Address.PostalCode %>" class="col-xs-12 col-sm-12" maxlength="10" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtCpErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                                <label id="TxtPoblacionLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_City"]%></label>
                                                                <div class="col-sm-7">
                                                                    <input type="text" id="TxtPoblacion" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_City"] %>" value="<%=this.Employee.Address.City %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtPoblacionErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label id="TxtProvinciaLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Province"]%></label>
                                                                <div class="col-sm-6">
                                                                    <input type="text" id="TxtProvincia" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Province"] %>" value="<%=this.Employee.Address.Province %>" class="col-xs-12 col-sm-12" maxlength="50" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtProvinciaErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                                <label id="TxtPaisLabel" class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Country"]%></label>
                                                                <div class="col-sm-3" id="DivCmbPais" style="height:35px !important;">
                                                                    <%if(this.Active ) { %>
                                                                    <select id="CmbPais" onchange="document.getElementById('TxtPais').value = this.value;" class="col-xs-12 col-sm-12">
                                                                        <asp:Literal runat="server" ID="LtCountries"></asp:Literal>
                                                                    </select>
                                                                    <% } %>
                                                                    <input <%if (this.Active) { %>style="display:none;"<% } %> type="text" id="TxtPais" placeholder="<%=this.Dictionary["Item_Employee_FieldLabel_Country"] %>" value="<%=this.Employee.Address.Country %>" class="col-xs-12 col-sm-12" maxlength="15" onblur="this.value=$.trim(this.value);" <%if(!this.Active) { %> readonly="readonly" <% } %> />
                                                                    <span class="ErrorMessage" id="TxtPaisErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-sm-1 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Notes"] %></label>
                                                                <div class="col-sm-11"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtNotas" <%if(!this.Active) { %> readonly="readonly" <% } %>><%=this.Employee.Notes %></textarea></div>
                                                            </div>                                                            
                                                        </form>
                                                    </div>  
                                                    <% if (this.EmployeeId > 0)
                                                       { %>
                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="table-responsive">
                                                                <h4><%=this.Dictionary["Item_Employee_SectionTitle_ProffesionalInfo"]%></h4>
                                                                <table class="table table-bordered table-striped">
                                                                    <thead class="thin-border-bottom">
                                                                        <tr>
                                                                            <th onclick="Sort(this,'WorkExperienceDataTable');" id="th0" class="sort" style="cursor:pointer;"><%=this.Dictionary["Item_Employee_ListJobPosition_JobPosition"]%></th>
                                                                            <th onclick="Sort(this,'WorkExperienceDataTable');" id="th1" class="sort hidden-480" style="cursor:pointer;"><%=this.Dictionary["Item_Employee_ListJobPosition_Department"]%></th>
                                                                            <th onclick="Sort(this,'WorkExperienceDataTable');" id="th2" class="sort hidden-480" style="cursor:pointer;"><%=this.Dictionary["Item_Employee_ListJobPosition_Responsible"]%></th>
                                                                            <th onclick="Sort(this,'WorkExperienceDataTable');" id="th3" class="sort hidden-480" style="width:110px;cursor:pointer;"><%=this.Dictionary["Item_Employee_ListJobPosition_StartDate"]%></th>
                                                                            <th onclick="Sort(this,'WorkExperienceDataTable');" id="th4" class="sort hidden-480" style="width:110px;cursor:pointer;"><%=this.Dictionary["Item_Employee_ListJobPosition_EndDate"]%></th>
                                                                            <th style="width:60px;">&nbsp;</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <%if (this.Active)
                                                                      { %>
                                                                    <tfoot>
                                                                        <tr>
                                                                            <td colspan="6">
                                                                                <button class="btn btn-info" type="button" id="BtnNewJobPosition" style="padding:0 !important">
                                                                                    <i class="icon-ok bigger-110"></i>
                                                                                    <%=this.Dictionary["Item_Employee_Button_LinkJobPosition"]%>
                                                                                </button>
                                                                            </td>
                                                                        </tr>
                                                                    </tfoot>
                                                                    <% } %>
                                                                    <tbody id="WorkExperienceDataTable"><asp:Literal runat="server" ID="TableExperiencia"></asp:Literal></tbody>
                                                                </table>
                                                            </div><!-- /.table-responsive -->
                                                        </div><!-- /span -->
                                                    </div><!-- /row -->	
                                                    <% }
                                                       else
                                                       {%>
                                                    <div class="row">
                                                        <div class="col-xs-12">&nbsp;</div>
                                                    </div>
                                                    <% } %>
                                                </div>
                                                <%if (this.EmployeeId > 0)
                                                  { %>
                                                <div id="formacion" class="tab-pane">
                                                    <h4><%=this.Dictionary["Item_Employee_SectionTitle_Skills"] %></h4>
                                                    <form class="form-horizontal" role="form">
                                                        <div class="for-group">
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_FieldLabel_Academic_Desired"]%></label>
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_FieldLabel_Academic_Real"]%>
                                                                <table style="float:right;">
                                                                    <tr>
                                                                        <td colspan="2"><label><%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"] %></label>:&nbsp;</td>
                                                                    <%if(this.Active) { %>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_Yes"]%>&nbsp;</td>
                                                                        <td><input type="radio" id="AcademicValidYes" name="AcademicValid" /></td>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_No"]%></td>
                                                                        <td><input type="radio" id="AcademicValidNo" name ="AcademicValid" /></td>
                                                                    <% }
                                                                      else
                                                                      { %>
                                                                       <td align="center"><%= this.JobPositionAcademicValid %></td>
                                                                    <%} %>
                                                                    </tr>
                                                                </table>
                                                            </label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" readonly="readonly" maxlength="250" id="TxtJobPositionAcademic"><%=this.JobPositionAcademic %></textarea></div>
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtAcademic" <%if(!this.Active) { %> readonly="readonly" <% } %>><%=this.Employee.EmployeeSkills.Academic %></textarea></div>
                                                        </div>
                                                        <div class="for-group">
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_FieldLabel_Especific_Desired"]%></label>
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_FieldLabel_Especific_Real"]%>
                                                                <table style="float:right;">
                                                                    <tr>
                                                                        <td colspan="2"><label><%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"] %></label>:&nbsp;</td>
                                                                    <%if(this.Active) { %>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_Yes"]%>&nbsp;</td>
                                                                        <td><input type="radio" id="SpecificValidYes" name="SpecificValid" /></td>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_No"]%></td>
                                                                        <td><input type="radio" id="SpecificValidNo" name="SpecificValid" /></td>
                                                                    <% }
                                                                      else
                                                                      { %>
                                                                       <td align="center"><%= this.JobPositionSpecificValid%></td>
                                                                    <%} %>
                                                                    </tr>
                                                                </table>
                                                            </label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" readonly="readonly" maxlength="250" id="TxtJobPositionSpecific"><%=this.JobPositionSpecific %></textarea></div>
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtSpecific" <%if(!this.Active) { %> readonly="readonly" <% } %>><%=this.Employee.EmployeeSkills.Specific %></textarea></div>
                                                        </div>
                                                        <div class="for-group">
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_Label_DesiredWorkExperience"]%></label>
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_Label_RealWorkExperience"]%>
                                                                <table style="float:right;">
                                                                    <tr>
                                                                        <td colspan="2"><label><%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"] %></label>:&nbsp;</td>
                                                                    <%if(this.Active) { %>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_Yes"]%>&nbsp;</td>
                                                                        <td><input type="radio" id="WorkExperienceValidYes" name="WorkExperienceValid" /></td>
                                                                        <td><%=this.Dictionary["Common_No"]%></td>
                                                                        <td>&nbsp;<input type="radio" id="WorkExperienceValidNo" name ="WorkExperienceValid" /></td>
                                                                    <% }
                                                                      else
                                                                      { %>
                                                                       <td align="center"><%= this.JobPositionWorkExperienceValid %></td>
                                                                    <%} %>
                                                                    </tr>
                                                                </table>
                                                            </label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" readonly="readonly" maxlength="250" id="TxtJobPositionWorkExperience"><%=this.JobPositionWorkExperience %></textarea></div>
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtWorkExperience" <%if(!this.Active) { %> readonly="readonly" <% } %>><%=this.Employee.EmployeeSkills.WorkExperience %></textarea></div>
                                                        </div> 
                                                        <div class="for-group">
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_Skills_FieldLabel_DesiredHabilities"]%></label>
                                                            <label class="col-sm-6"><%=this.Dictionary["Item_Employee_Skills_FieldLabel_RealHabilities"]%>
                                                                <table style="float:right;">
                                                                    <tr>
                                                                        <td colspan="2"><label><%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"] %></label>:&nbsp;</td>
                                                                    
                                                                    <%if (this.Active)
                                                                      { %>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_Yes"]%>&nbsp;</td>
                                                                        <td><input type="radio" id="HabilityValidYes" name="HabilityValid" /></td>
                                                                        <td>&nbsp;<%=this.Dictionary["Common_No"]%></td>
                                                                        <td><input type="radio" id="HabilityValidNo" name ="HabilityValid" /></td>
                                                                    <% }
                                                                      else
                                                                      { %>
                                                                       <td align="center"><%= this.JobPositionHabilityValid %></td>
                                                                    <%} %>
                                                                    </tr>
                                                                </table>
                                                            </label>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" readonly="readonly" maxlength="250" id="TxtJobPositionHability"><%=this.JobPositionHability %></textarea></div>
                                                            <div class="col-sm-6"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtHability" <%if(!this.Active) { %> readonly="readonly" <% } %>><%=this.Employee.EmployeeSkills.Ability %></textarea></div>                                                           
                                                        </div> 
                                                    

                                                        <div class="form-group">
                                                            <label class="col-sm-6">&nbsp;</label>
                                                            <label class="col-sm-1 control-label no-padding-right" style="padding-left: 0!important;" id="CmbAprovedByLabel" for="CmbAprovedBy"><%=this.Dictionary["Item_JobPosition_FieldLabel_AprovedBy"] %><span class="required" id="aprovedByRequired" style="display:none;">*</span></label>
                                                            <div class="col-sm-5">
                                                                <select id="CmbAprovedBy" class="form-control col-xs-12 col-sm-12" onchange="AprovedByChanged();"><asp:Literal runat="server" ID="LtCmbAprovedBy"></asp:Literal></select>
                                                                <input style="display:none;" type="text" readonly="readonly" id="CmbAprovedByValue" class="col-xs-12 col-sm-12" value="<%=this.AprovedByText == "0" ? string.Empty : this.AprovedByText %>" />
                                                                <span class="ErrorMessage" id="CmbAprovedByErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-6">&nbsp;</label>
                                                            <label class="col-sm-1 control-label no-padding-right" id="TxtAprovedOnLabel" for="TxtAprovedOn"><%=this.Dictionary["Item_JobPosition_FieldLabel_AprovedOn"] %><span class="required" id="aprovedOnRequired" style="display:none;">*</span></label>
                                                            <div class="col-sm-2">
                                                                <div class="input-group">
                                                                    <input style="width:90px;" class="form-control date-picker" id="TxtAprovedOn" onchange="AprovedOnChanged();" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.AprovedOnText) %>" />
                                                                    <span id="TxtAprovedOnBtn" class="input-group-addon" onclick="document.getElementById('TxtAprovedOn').focus();">
                                                                        <i class="icon-calendar bigger-110"></i>
                                                                    </span>
                                                                </div>
                                                                <span class="ErrorMessage" id="TxtAprovedOnErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-sm-5" id="NotesLasbel" for="TxtNotes"><%=this.Dictionary["Item_JobPosition_FieldLabel_Notes"] %></label>
                                                            <label class="col-sm-7">&nbsp;</label>
                                                        </div>
                                                        <div class="form-group">                                                        
                                                            <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="250" id="TxtNotes" <%if(!this.Active) { %> readonly="readonly" <% } %>><%=this.Employee.EmployeeSkills.Notes %></textarea></div>                                                            
                                                        </div>
                                                        <div style="display:none"><label class="col-sm-2 control-label no-padding-right" id="TxtLastChangeLabel" for="TxtAprovedOn"><br /><%=this.Dictionary["Item_JobPosition_FieldLabel_LastChange"] %></label>
                                                            <div class="col-sm-2">
                                                                <br />
                                                                <div class="input-group">
                                                                    <input style="width:90px;" class="form-control date-picker" id="LastChange" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="<%= string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.LastChangeText) %>" />
                                                                    <span id="TxtLastChangeBtn" class="input-group-addon" onclick="document.getElementById('TxtLastChange').focus();">
                                                                        <i class="icon-calendar bigger-110"></i>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </form>
                                                </div>
                                                <div id="formacionInterna" class="tab-pane<%=this.SelectedTab =="formacioninterna" ? " active" : String.Empty %>">		
                                                    <h4><%=this.Dictionary["Item_Employee_SectionTitle_InternalLearning"] %></h4>											
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th onclick="Sort(this,'InternalLearningDataTable', 'text', false);" id="th0" class="sort" style="cursor:pointer;"><%=this.Dictionary["Item_Learning_FieldLabel_Name"]%></th>
                                                                <th onclick="Sort(this,'InternalLearningDataTable', 'date', false);" id="th1" class="sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_Employee_ListLearningHeader_Date"]%></th>
                                                                <th onclick="Sort(this,'InternalLearningDataTable', 'text', false);" id="th2" class="sort" style="width:60px;cursor:pointer;"><%=this.Dictionary["Item_LearningAssistant_Status_Evaluated"]%></th>												
                                                            </tr>
                                                        </thead>
                                                        <tbody id="InternalLearningDataTable">
                                                            <asp:Literal runat="server" ID="TableLearningAssistance"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                
                                                <div id="uploadFiles" class="tab-pane">
                                                    <div class="col-sm-12">
                                                        <div class="col-sm-8">
                                                            <div class="btn-group btn-corner" style="display:inline;">
												                <button id="BtnModeList" class="btn" type="button" style="border-bottom-left-radius:8px!important;border-top-left-radius:8px!important;" onclick="documentsModeView(0);"><i class="icon-th-list"></i></button>
												                <button id="BtnModeGrid" class="btn btn-info" type="button" style="border-bottom-right-radius:8px!important;border-top-right-radius:8px!important;" onclick="documentsModeView(1);"><i class="icon-th"></i></button>
											                </div>
                                                            <h4 style="float:left;">&nbsp;<%=this.Dictionary["Item_Attachment_SectionTitle"] %></h4>
                                                        </div>
                                                        <div class="col-sm-4" style="text-align:right;">
                                                            
                                                            <h4 class="pink" style="right:0;">
                                                                <button class="btn btn-success" type="button" id="BtnNewUploadfile" onclick="UploadFile();">
                                                                    <i class="icon-plus-sign bigger-110"></i>
                                                                    <%=this.Dictionary["Item_Attachment_Btn_New"] %>
                                                                </button>
                                                            </h4>
                                                        </div>
                                                    </div>
                                                    <div style="clear:both">&nbsp;</div>
                                                    <div class="col-sm-12" id="UploadFilesContainer">
                                                        <asp:Literal runat="server" ID="LtDocuments"></asp:Literal>
                                                    </div>
                                                    <div class="col-sm-12" id="UploadFilesList" style="display:none;">
                                                        <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <!--<th style="width:150px;"><%=this.Dictionary["Item_Attachment_Header_FileName"] %></th>-->
                                                                <th><%=this.Dictionary["Item_Attachment_Header_Description"] %></th>
                                                                <th style="width:90px;"><%=this.Dictionary["Item_Attachment_Header_CreateDate"] %></th>
                                                                <th style="width:120px;"><%=this.Dictionary["Item_Attachment_Header_Size"] %></th>
                                                                <th style="width:160px;"></th>													
                                                            </tr>
                                                        </thead>
                                                        <tbody id="TBodyDocumentsList">
                                                            <asp:Literal runat="server" ID="LtDocumentsList"></asp:Literal>
                                                        </tbody>
                                                    </table>
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
                                                <% } %>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="PopupUploadFile" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <form action="/dummy.html" class="dropzone well dz-clickable" id="dropzone">
                                        <input type="file" id="fileName" name="fileName" multiple style="position:absolute;top:-100000px;"/>
                                        <div class="dz-default dz-message">
                                            <span id="UploadMessage">
                                                <span class="bigger-125 bolder">
                                                    <i class="ace-icon fa fa-caret-right red"></i>
                                                </span>
                                                <span class="bigger-125"><%=this.Dictionary["Item_DocumentAttachment_UpladTitle1"] %>
                                                <%=this.Dictionary["Item_DocumentAttachment_UpladTitle2"] %></span> &nbsp;&nbsp;&nbsp;
                                                <i class="upload-icon ace-icon fa fa-cloud-upload blue fa-2x"></i>
                                            </span>
                                            <span id="UploadMessageSelected" style="display:none;">
                                                <span class="bigger-150 bolder">
                                                    <i class="ace-icon fa  icon-file-text blue">&nbsp;</i>
                                                    <span id="UploadMessageSelectedFileName"></span>
                                                </span>&nbsp;
                                                <i style="cursor:pointer;" class="ace-icon icon-ok-sign green fa-2x" onclick="ShowPreview();"></i>
                                                &nbsp;
                                                <i class="ace-icon icon-remove-sign red fa-2x" onclick="RestoreUpload();"></i>
                                            </span>
                                        </div>
									</form>
                                        <div class="col-sm-12">
                                            <label class="input-append col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_DocumentAttachment_PopupUpload_Description_Label"] %></label>
                                            <label class="input-append col-sm-10"><input class="col-sm-11" id="UploadFileDescription" name="UploadFileDescription" /></label>
                                        </div>
                                        <!--<div class="col-sm-12">
                                            <p><input type="checkbox" /> Guardar como copia local</p>
                                        </div>-->
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->
                            <div id="DepartmentNewAsociationDialog" class="hide" style="width:600px;">
                                <div class="form-group">
                                    <label id="DepartmentNewAsociationTextLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Employee_FieldLabel_Name"] %></label>                                        
                                    <div class="col-sm-10">
                                        <input type="text" maxlength="50" size="50" id="DepartmentNewAsociationText" class="col-xs-12 col-sm-12" value="" />
                                        <span class="ErrorMessage" id="DepartmentNewAsociationTextErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        <span class="ErrorMessage" id="DepartmentNewAsociationTextErrorDuplicated"><%=this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
                            </div>
                            <div id="DepartmentDesassociationDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Employee_Popup_UnlinkDepartment_Message"]%><strong> <span id="DepartmentDesassociationText"></span></strong></p>
                            </div>
                            <div id="JobPositionDesassociationDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Employee_Popup_UnlinkJobPosition_Message"]%><strong> <span id="JobPositionDesassociationText"></span></strong></p>
                                <p><%=this.Dictionary["Item_Employee_Popup_UnlinkJobPosition_Message2"]%></p>
                                <input type="hidden" id="BtnStartCopyDate" />
                                <div class="col-sm-12">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 tooltip-info" id="DivFinishDate">
                                            <div class="input-group">
                                                <input class="form-control date-picker" id="TxtFinishDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                <span id="BtnFinishDate" class="input-group-addon" onclick="document.getElementById('TxtFinishDate').focus();">
                                                    <i class="icon-calendar bigger-110"></i>
                                                </span>                                                
                                            </div>
                                            <span class="ErrorMessage" id="TxtFinishDateErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                            <span class="ErrorMessage" id="TxtFinishDateErrorMaximumToday"><%=this.Dictionary["Common_Error_MaximumToday"] %></span>
                                            <span class="ErrorMessage" id="TxtFinishDateErrorBeforeStart"><%=this.Dictionary["Common_Error_BeforeStart"] %></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
    
                            <div id="JobPositionAssociationDateDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Employee_Popup_LinkJobPosition_Message"]%></p>
                                <div class="col-sm-12">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 tooltip-info" id="DivStartDate">
                                            <div class="input-group">
                                                <input class="form-control date-picker" id="TxtStartDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                <span id="BtnStartDate" class="input-group-addon" onclick="document.getElementById('TxtFinishDate').focus();">
                                                    <i class="icon-calendar bigger-110"></i>
                                                </span>                                                
                                            </div>
                                            <span class="ErrorMessage" id="TxtStartDateErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                            <span class="ErrorMessage" id="TxtStartDateErrorMaximumToday"><%=this.Dictionary["Common_Error_MaximumToday"] %></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="DepartmentAssociationDialog" class="hide" style="width:800px;">								
                                <table class="table table-bordered table-striped">
                                    <thead class="thin-border-bottom">
                                        <tr>
                                            <th><%=this.Dictionary["Item_Employee_ListDepartment_Header_Department"] %></th>
                                            <th style="width:150px;" class="hidden-480">&nbsp;</th>													
                                        </tr>
                                    </thead>
                                    <tbody id="DepartmentsPopupBody">
                                    </tbody>
                                </table>
                            </div><!-- #dialog-message -->
                            <div id="JobPositionAssociationDialog" class="hide" style="width:500px;">
                                <table class="table table-bordered table-striped">
                                    <thead class="thin-border-bottom">
                                        <tr>
                                            <th><%=this.Dictionary["Item_Employee_ListJobPosition_Header_JobPosition"] %></th>
                                            <th><%=this.Dictionary["Item_Employee_ListJobPosition_Header_Department"] %></th>
                                            <th style="width:40px;" class="hidden-480">&nbsp;</th>													
                                        </tr>
                                    </thead>
                                    <tbody id="JobPositionAssociationDialogTable"></tbody>
                                </table>
                            </div>
                            <div id="DepartmentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Department_PopupDelete_Message"] %>&nbsp;<strong><span id="DepartmentName"></span></strong>?</p>
                            </div>
                            <div id="JobPositionDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_JobPosition_PopupDelete_Message"] %>&nbsp;<strong><span id="JobPositionName"></span></strong>?</p>
                            </div>
                            <div id="DepartmentUpdateDialog" class="hide" style="width:600px;">
                                <p>
                                    <label class="col-sm-2 control-label no-padding-right" id="TxtDepartmentUpdateNameLabel"><%=this.Dictionary["Common_Name"] %>:</label> 
                                    &nbsp;&nbsp;
                                    <input type="text" id="TxtDepartmentUpdateName" size="50" placeholder="<%= this.Dictionary["Common_Name"] %>" maxlength="50" onblur="this.value=$.trim(this.value);" />
                                </p>
                                <span class="ErrorMessage" id="TxtDepartmentUpdateNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentUpdateNameErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>

                            <div id="dialogAnular" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnular">
                                    <div class="form-group">
                                        <label id="TxtEndDateLabel" class="col-sm-3 control-label no-padding-right" for="TxtRecordDate"><%=this.Dictionary["Item_IndicatorRecord_FieldLabel_Date"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="DivEndDate">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEndDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="BtnEndDate" class="input-group-addon" onclick="document.getElementById('TxtEndDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                    <span class="ErrorMessage" id="TxtEndDateErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                    <span class="ErrorMessage" id="TxtEndDateMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </form>
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
        <script type="text/javascript" src="/js/EmployeeDepartments.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EmployeeJobPosition.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EmployeesView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>

