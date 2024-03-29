﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="EquipmentView.aspx.cs" Inherits="EquipmentView" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <link rel="stylesheet" href="/Document-Viewer/style.css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/yepnope.1.5.3-min.js"></script>
    <script type="text/javascript" src="/Document-Viewer/ttw-document-viewer.min.js"></script>
    <script type="text/javascript">
        var itemGrantId = 11;
        scaleImages = true;
        var limitInitialDate = "<%=this.LimitInitialDate %>";
        var GrantToWrite = <%=this.GrantToWrite %>;
        var Equipment = <%=this.Equipment.Json %>;
        <%=EquipmentScaleDivisionList %>
        var EquipmentCalibrationActList = <%=this.EquipmentCalibrationActList %>;
        var EquipmentVerificationActList = <%=this.EquipmentVerificationActList %>;
        var EquipmentMaintenanceDefinitionList = <%=this.EquipmentMaintenanceDefinitionList %>;
        var EquipmentMaintenanceActList = <%=this.EquipmentMaintenanceActList %>;
        var EquipmentRepairList = <%=this.EquipmentRepairList %>;
        var EquipmentScaleDivisionSelected = <%=this.EquipmentScaleDivisionSelected %>;
        var mantenimientoLaunchId = 0;
        var EmployeesGrant = <%=GisoFramework.Item.Employee.EmployeesGrant(11, this.Company.Id)%>;
        var Providers = <%=this.ProvidersJson %>;
        var Customers = <%=this.CustomersJson %>;
        var Employees = <%=this.EmployeesJson %>;
        var OperationId = '<%=this.OperationId %>';
        var ProviderCalibrationDefinition = <%=this.CalibrationProviderId %>;
        var ProviderVerificationDefinition = <%=this.VerificationProviderId %>;
        var typeItemId = 11;
        var itemId = Equipment.Id;
        var pageType = "form";
        var EquipmentForm =
        {
            "FieldsFormat":
                [
                    { "Id": "TxtMeasure", "Format": "decimal" },
                    { "Id": "TxtScaleDivision", "Format": "decimal" }
                ],
            "RequiredFields":
                [
                    "TxtCode",
                    "TxtDescription",
                    "TxtLocation"
                ],
            "DuplicatedFields":
            [
                {FieldName:'TxtCode',Id: Equipment.Id, Values:[{Id:1,Value:'aaa'},{Id:2,Value:'bbb'},{Id:3,Value:'ccc'}]}
            ],
            "MinimumOptions":
                [
                    {
                        "Minimum": 1,
                        "Options": ["Contentholder1_status0", "Contentholder1_status1", "Contentholder1_status2"],
                        "Message": 'MinimumOptionsError'
                    }
                ]
        };
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div>
                                    <div id="user-profile-2" class="user-profile">
                                        <div class="tabbable">
                                            <%=this.TabBar %>
                                            <div class="tab-content no-border " style="height:500px;">
                                                <div id="home" class="tab-pane<%=this.SelectedTab =="home" ? " active" : String.Empty %>">       
                                                    <div class="row">
                                                        <form class="form-horizontal" role="form">
                                                            <div class="col-sm-10">
                                                                <div class="form-group">
                                                                    <%=this.TxtCode.Render %>
                                                                    <%=this.TxtDescription.Render %>
                                                                </div>                                                            
                                                                <div class="form-group">
                                                                    <%=this.TxtTradeMark.Render %>
                                                                    <%=this.TxtModel.Render %>
                                                                </div>                                                        
                                                                <div class="form-group">
                                                                    <%=this.TxtSerialNumber.Render %>
                                                                    <%=this.TxtLocation.Render %>
                                                                </div>                                                     
                                                                <div class="form-group">
                                                                    <%=this.TxtMeasureRange.Render %>
                                                                    <%=this.TxtScaleDivision.Render.Replace("decimalFormated","decimalFormated4") %>
                                                                    <%=this.BarScaleDivisionType.Render %>
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-xs-6">&nbsp;</div>
                                                                    <%=this.TxtStartDate.Render %>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-2">
                                                                <%=this.RenderImage %>
                                                            </div>
                                                            <div class="form-group col-sm-12">

                                                            <label id="Label99" class="col-sm-1"><%=this.Dictionary["Item_Equipment_List_Filter_ShowByOperation"] %></label>
                                                                <div class="col-sm-1" style="text-align:left;" ><input runat="server" type="checkbox" id="status0" name="status" value="0" />&nbsp;<%=this.Dictionary["Item_Equipment_Field_IsCalibration_Label"] %></div>
                                                                <div class="col-sm-1" style="text-align:left;" ><input runat="server" type="checkbox" id="status1" name="status" value="1" />&nbsp;<%=this.Dictionary["Item_Equipment_Field_IsVerification_Label"] %></div>
                                                                <div class="col-sm-1" style="text-align:center; padding-left:5px; padding-right:5px;"><input runat="server" type="checkbox" id="status2" name="status" value="2" />&nbsp;<%=this.Dictionary["Item_Equipment_Field_IsMaintenance_Label"]%></div>
                                                                    <%=this.CmbResponsible.Render %>
                                                                 <div class="col-sm-3">&nbsp;</div>
                                                            </div>
                                                            <div class="form-group">
                                                            <div class="col-sm-2">&nbsp;</div>
                                                            <div class="col-sm-10 ErrorMessage" id="MinimumOptionsError" style="display:none;"><%=this.Dictionary["Common_Error_MinimumOneAction"] %></div>
                                                            </div>
                                                            <div style="display:none;">
                                                            <%=this.TxtNotes.Render %>
                                                            </div>
                                                            <div style="height:12px;clear:both;">&nbsp;</div>
                                                            <%=this.TxtObservations.Render %>
                                                        </form>
                                                    </div>
                                                </div>
                                                <div id="calibracion" class="tab-pane<%=this.SelectedTab =="calibracion" ? " active" : String.Empty %>">
                                                    <div class="row">
                                                        <form class="form-horizontal" role="form">
                                                            <div class="col-sm-6" style="border:1px solid #e0e0e0;">
                                                                <div class="form-group">
                                                                    <div class="col-sm-12" style="padding-top: 5px;">
                                                                        <input type="checkbox" id="CalibrationInternalActive" name="CalibrationInternalActive" value="0" onclick="LockInternalCalibrationForm(this.checked);" />
                                                                        <span style="font-size:16px;font-weight:bold;"><%=this.Dictionary["Item_CalibrationType_Internal"] %></span>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationInternalTxtOperation.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationInternalTxtPeriodicity.Render%>
                                                                    <label id="Label11" class="col-sm-2"><%=this.Dictionary["Common_Label_Days"] %></label>
                                                                    <%=this.CalibrationInternalTxtUncertainty.Render.Replace("decimalFormated","decimalFormated6") %>
                                                                </div>   
                                                                <div class="form-group">
                                                                    <%=this.CalibrationInternalTxtRange.Render%>
                                                                </div>  
                                                                <div class="form-group">
                                                                    <%=this.CalibrationInternalTxtPattern.Render%>
                                                                    <label id="Label111" class="col-sm-1">&nbsp;</label>
                                                                    <%=this.CalibrationInternalTxtCost.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationInternalTxtNotes.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationInternalCmbResponsible.Render%>
                                                                </div>
                                                                <div class="form-group" style="height:35px;">
                                                                </div>
                                                                <div class="form-group" id="TxtICDFirstDateRow" style="visibility:hidden;">
                                                                    <label id="TxtICDFirstDateLabel" class="col-sm-4"><%=this.Dictionary["Common_FirstDate"] %></label>
                                                                    <div class="col-sm-4">                                                                                                                            
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="TxtICDFirstDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                            <span id="TxtICDFirstDateBtn" class="input-group-addon" onclick="document.getElementById('TxtICDFirstDate').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>	
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-xs-9">&nbsp;</div>
                                                                    <div class="col-xs-3">
                                                                        <button class="btn btn-success" type="button" id="BtnCalibrationInternalSave">
                                                                            <i class="icon-save bigger-110"></i>
                                                                            <%=this.Dictionary["Common_Save"] %>
                                                                        </button>
                                                                    </div> 
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6" style="border:1px solid #e0e0e0;">
                                                                <div class="form-group">
                                                                    <div class="col-sm-12" style="padding-top: 5px;">
                                                                        <input type="checkbox" id="CalibrationExternalActive" name="CalibrationExternalActive" value="0" onclick="LockExternalCalibrationForm(this.checked);" />
                                                                        <span style="font-size:16px;font-weight:bold;"><%=this.Dictionary["Item_CalibrationType_External"] %></span>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationExternalTxtOperation.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationExternalTxtPeriodicity.Render%>
                                                                    <label id="Label1" class="col-sm-2"><%=this.Dictionary["Common_Label_Days"] %></label>
                                                                    <%=this.CalibrationExternalTxtUncertainty.Render.Replace("decimalFormated","decimalFormated6") %>
                                                                </div>   
                                                                <div class="form-group">
                                                                    <%=this.CalibrationExternalTxtRange.Render%>
                                                                </div>  
                                                                <div class="form-group">
                                                                    <%=this.CalibrationExternalTxtPattern.Render%>
                                                                    <label id="Label111" class="col-sm-1">&nbsp;</label>
                                                                    <%=this.CalibrationExternalTxtCost.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.CalibrationExternalTxtNotes.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.ExternalCalibrationResponsible%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label id="CmbCalibrationExternalProviderLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Equipment_Field_Calibration_Provider"] %><span class="required">*</span></label>
                                                                    <div class="col-sm-8" id="CmbCalibrationExternalProviderrDiv" style="height:35px !important;">
                                                                        <select id="CmbCalibrationExternalProvider" class="form-control col-xs-12 col-sm-12"></select>
                                                                        <input style="display:none;" type="text" readonly="readonly" id="CmbCalibrationExternalProviderValue" placeholder="Proveedor" class="col-xs-12 col-sm-12" />
                                                                        <span class="ErrorMessage" id="CmbCalibrationExternalProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    </div>
                                                                    <div class="col-sm-1" id="CmbCalibrationExternalProviderBar"><button class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnCalibrationExternalProviderBAR">...</button></div>
                                                                </div>
                                                                <div class="form-group" id="TxtECDFirstDateRow" style="visibility:hidden;">
                                                                    <label id="TxtECDFirstDateLabel" class="col-sm-4"><%=this.Dictionary["Common_FirstDate"] %></label>
                                                                    <div class="col-sm-4">                                                                                                                            
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="TxtECDFirstDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                            <span id="TxtECDFirstDateBtn" class="input-group-addon" onclick="document.getElementById('TxtECDFirstDate').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>	
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-xs-9">&nbsp;</div>
                                                                    <div class="col-xs-3">
                                                                        <button class="btn btn-success" type="button" id="BtnCalibrationExternalSave">
                                                                            <i class="icon-save bigger-110"></i>
                                                                            <%=this.Dictionary["Common_Save"] %>
                                                                        </button>
                                                                    </div> 
                                                                </div>
                                                            </div>
                                                        </form>
                                                    </div>    
                                                    <hr />      
                                                    <div class="row" id="CalibrationWarning">
                                                        <div class="alert alert-danger">
                                                            <strong><%=this.Dictionary["Common_Warning"]%></strong>
                                                            <%=this.Dictionary["Item_CalibrationAct_Warning_DefinitionRequired"] %>
                                                        </div>
                                                    </div>
                                                    <div class="row" id="CalibrationDivTable">
                                                    	<div class="form-group col-sm-12">
                                                    	<div class="col-sm-10"><h4><%=this.Dictionary["Item_EquipmentCalibration_Reg"] %></h4></div>
                                                               <div class="col-sm-2" style="text-align:right;">
                                                        <!--div class="col-sm-12" style="margin-bottom:8px;"-->
                                                            <button class="btn btn-success" type="button" id="BtnNewCalibration">
                                                                <i class="icon-plus bigger-110"></i>
                                                                <%=this.Dictionary["Item_CalibrationAct_Button_New"] %>
                                                            </button>
                                                        </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-xs-12">
                                                                <div class="table-responsive">
                                                                    <table class="table table-bordered table-striped">
                                                                        <thead class="thin-border-bottom">
                                                                            <tr>
                                                                                <th id="th0" class="sort" style="width:90px;" onclick="Sort(this,'TableEquipmentCalibrationAct','date');" ><%=this.Dictionary["Common_Date"] %></th>
                                                                                <th id="th1" class="sort hidden-480" style="width:90px;"" onclick="Sort(this,'TableEquipmentCalibrationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Type"] %></th>
                                                                                <!-- th class="hidden-480" style="width:130px;""><%=this.Dictionary["Item_Equipment_Field_Calibration_Uncertainty"] %></th -->
                                                                                <th id="th2" class="sort hidden-480" style="width:130px;" onclick="Sort(this,'TableEquipmentCalibrationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Uncertainty"] %></th>
																				<th id="th3" class="sort hidden-480" style="width:120px;" onclick="Sort(this,'TableEquipmentCalibrationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Result"] %></th>	
                                                                                <th class="hidden-480"><%=this.Dictionary["Item_Equipment_Field_Calibration_Responsible"] %></th>	
                                                                                <th id="th5" class="sort hidden-480" style="width:120px;" onclick="Sort(this,'TableEquipmentCalibrationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Cost"] %></th>	
                                                                                <th id="th6" class="sort hidden-480" style="width:120px;"  onclick="Sort(this,'TableEquipmentCalibrationAct','date');" ><%=this.Dictionary["Item_Equipment_Field_Calibration_Expiration"] %></th>	
                                                                                <th class="hidden-480" style="width:110px;">&nbsp;</th>												
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="8" style="padding:0;">
                                                                                    <div class="scrollTable" style="width:100%;height:300px;overflow-y:scroll;">
                                                                                        <table style="width:100%;">
                                                                                            <tbody id="TableEquipmentCalibrationAct"></tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="5"><div id="TableEquipmentCalibrationActTotalLabel" style="width:100%"></div></td>
                                                                                <td style="font-weight:bold;text-align:right;"><span id="TableEquipmentCalibrationActTotal"></span></td>
                                                                                <td colspan="2">&nbsp;</td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div><!-- /widget-main -->
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="verificacion" class="tab-pane<%=this.SelectedTab =="verificacion" ? " active" : String.Empty %>">
                                                    <div class="row">
                                                        <form class="form-horizontal" role="form">
                                                            <div class="col-sm-6" style="border:1px solid #e0e0e0;">
                                                                <div class="form-group">
                                                                    <div class="col-sm-12" style="padding-top: 5px;">
                                                                        <input type="checkbox" id="VerificationInternalActive" name="VerificationInternalActive" value="0" onclick="LockInternalVerificationForm(this.checked);" />
                                                                        <span style="font-size:16px;font-weight:bold;"><%=this.Dictionary["Item_Equipment_VerificationType_Internal"] %></span>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationInternalTxtOperation.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationInternalTxtPeriodicity.Render%>
                                                                    <label id="Label3" class="col-sm-2"><%=this.Dictionary["Common_Label_Days"] %></label>
                                                                    <%=this.VerificationInternalTxtUncertainty.Render.Replace("decimalFormated","decimalFormated6") %>
                                                                </div>   
                                                                <div class="form-group">
                                                                    <%=this.VerificationInternalTxtRange.Render%>
                                                                </div>  
                                                                <div class="form-group">
                                                                    <%=this.VerificationInternalTxtPattern.Render%>
                                                                    <label id="Label31" class="col-sm-1">&nbsp;</label>
                                                                    <%=this.VerificationInternalTxtCost.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationInternalTxtNotes.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationInternalCmbResponsible.Render%>
                                                                </div>
                                                                <div class="form-group" style="height:35px;">&nbsp;</div>
                                                                <div class="form-group" id="TxtIVDFirstDateRow" style="visibility:hidden;">
                                                                    <label id="TxtIVDFirstDateLabel" class="col-sm-4"><%=this.Dictionary["Common_FirstDate"] %></label>
                                                                    <div class="col-sm-4">                                                                                                                            
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="TxtIVDFirstDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                            <span id="TxtIVDFirstDateBtn" class="input-group-addon" onclick="document.getElementById('TxtIVDFirstDate').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>	
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-xs-9">&nbsp;</div>
                                                                    <div class="col-xs-3">
                                                                        <button class="btn btn-success" type="button" id="BtnVerificationInternalSave">
                                                                            <i class="icon-save bigger-110"></i>
                                                                            <%=this.Dictionary["Common_Save"] %>
                                                                        </button>
                                                                    </div> 
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6" style="border:1px solid #e0e0e0;">
                                                                <div class="form-group">
                                                                    <div class="col-sm-12" style="padding-top: 5px;">
                                                                        <input type="checkbox" id="VerificationExternalActive" name="VerificationExternalActive" value="0" onclick="LockExternalVerificationForm(this.checked);" />
                                                                        <span style="font-size:16px;font-weight:bold;"><%=this.Dictionary["Item_Equipment_VerificationType_External"] %></span>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationExternalTxtOperation.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationExternalTxtPeriodicity.Render%>
                                                                    <label id="Label4" class="col-sm-2"><%=this.Dictionary["Common_Label_Days"] %></label>
                                                                    <%=this.VerificationExternalTxtUncertainty.Render.Replace("decimalFormated","decimalFormated6") %>
                                                                </div>   
                                                                <div class="form-group">
                                                                    <%=this.VerificationExternalTxtRange.Render%>
                                                                </div>  
                                                                <div class="form-group">
                                                                    <%=this.VerificationExternalTxtPattern.Render%>
                                                                    <label id="Label311" class="col-sm-1">&nbsp;</label>
                                                                    <%=this.VerificationExternalTxtCost.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationExternalTxtNotes.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <%=this.VerificationExternalCmbResponsible.Render%>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label id="CmbVerificationExternalProviderLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Equipment_Field_Verification_Provider"] %><span class="required">*</span></label>
                                                                    <div class="col-sm-8" id="CmbVerificationExternalProviderrDiv" style="height:35px !important;">
                                                                        <select id="CmbVerificationExternalProvider" class="form-control col-xs-12 col-sm-12"></select>
                                                                        <input style="display:none;" type="text" readonly="readonly" id="CmbVerificationExternalProviderValue" placeholder="Proveedor" class="col-xs-12 col-sm-12" />
                                                                        <span class="ErrorMessage" id="CmbVerificationExternalProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                                    </div>
                                                                    <div class="col-sm-1" id="CmbVerificationExternalProviderBar"><button class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnVerificationExternalProviderBAR">...</button></div>
                                                                </div>
                                                                <div class="form-group" id="TxtEVDFirstDateRow" style="visibility:hidden;">
                                                                    <label id="TxtEVDFirstDateLabel" class="col-sm-4"><%=this.Dictionary["Common_FirstDate"] %></label>
                                                                    <div class="col-sm-4">                                                                                                                            
                                                                        <div class="input-group">
                                                                            <input class="form-control date-picker" id="TxtEVDFirstDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" value="" />
                                                                            <span id="TxtEVDFirstDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEVDFirstDate').focus();">
                                                                                <i class="icon-calendar bigger-110"></i>
                                                                            </span>
                                                                        </div>
                                                                    </div>	
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-xs-9">&nbsp;</div>
                                                                    <div class="col-xs-3">
                                                                        <button class="btn btn-success" type="button" id="BtnVerificationExternalSave">
                                                                            <i class="icon-save bigger-110"></i>
                                                                            <%=this.Dictionary["Common_Save"] %>
                                                                        </button>
                                                                    </div> 
                                                                </div>
                                                            </div>
                                                        </form>
                                                    </div>
                                                    <hr />
                                                    <div class="row" id="VerificationWarning">
                                                        <div class="alert alert-danger">
                                                            <strong><%=this.Dictionary["Common_Warning"]%></strong>
                                                            <%=this.Dictionary["Item_EquipmentVerification_Message_WarningNew"] %>                                                            
                                                        </div>
                                                    </div>
                                                    <div class="row" id="VerificationDivTable">
                                                        <div class="form-group col-sm-12">
                                                    	<div class="col-sm-10"><h4><%=this.Dictionary["Item_EquipmentVerification_Reg"] %></h4></div>
                                                               <div class="col-sm-2" style="text-align:right;">
                                                        <!--div class="col-sm-12" style="margin-bottom:8px;"-->
                                                            <button class="btn btn-success" type="button" id="BtnNewVerification">
                                                                <i class="icon-plus bigger-110"></i>
                                                                <%=this.Dictionary["Item_EquipmentVerification_New"] %>
                                                            </button>
                                                        </div>
                                                        </div>
                                                        
                                                        <div class="row">
                                                            <div class="col-xs-12">
                                                                <div class="table-responsive">
                                                                    <table class="table table-bordered table-striped">
                                                                        <thead class="thin-border-bottom">
                                                                            <tr>	
                                                                                <th id="th0" class="sort" style="width:90px;" onclick="Sort(this,'TableEquipmentVerificationAct','date');" ><%=this.Dictionary["Common_Date"] %></th>
                                                                                <th id="th1" class="sort hidden-480" style="width:90px;"" onclick="Sort(this,'TableEquipmentVerificationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Type"] %></th>
                                                                                <!-- th class="hidden-480" style="width:130px;""><%=this.Dictionary["Item_EquipmentVerification_Field_Uncertainty"] %></th -->
																				<th id="th2" class="sort hidden-480" style="width:130px;" onclick="Sort(this,'TableEquipmentVerificationAct','money');"><%=this.Dictionary["Item_EquipmentVerification_Field_Uncertainty"] %></th>
																				<th id="th3" class="sort hidden-480" style="width:120px;" onclick="Sort(this,'TableEquipmentVerificationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Result"] %></th>	
                                                                                <th class="hidden-480"><%=this.Dictionary["Common_Responsible"] %></th>	
                                                                                <th id="th5" class="sort hidden-480" style="width:120px;" onclick="Sort(this,'TableEquipmentVerificationAct','money');"><%=this.Dictionary["Item_Equipment_Field_Calibration_Cost"] %></th>	
                                                                                <th id="th6" class="sort hidden-480" style="width:120px;"  onclick="Sort(this,'TableEquipmentVerificationAct','date');" ><%=this.Dictionary["Item_Equipment_Field_Calibration_Expiration"] %></th>	
                                                                                <th class="hidden-480" style="width:110px;">&nbsp;</th>												
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td colspan="8" style="padding:0;">
                                                                                    <div class="scrollTable" style="width:100%;height:300px;overflow-y:scroll;">
                                                                                        <table style="width:100%;">
                                                                                            <tbody id="TableEquipmentVerificationAct"></tbody>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="5"><div id="TableEquipmentVerificationActTotalLabel" style="width:100%"></div></td>
                                                                                <td style="font-weight:bold;text-align:right;"><span id="TableEquipmentVerificationActTotal"></span></td>
                                                                                <td colspan="5">&nbsp;</td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div><!-- /.table-responsive -->
                                                            </div><!-- /span -->
                                                        </div>
                                                    </div>
                                                </div>

                                                <div id="mantenimiento" class="tab-pane<%=this.SelectedTab =="mantenimiento" ? " active" : String.Empty %>">
                                                    <div class="col-sm-12" style="margin-bottom:4px;">
                                                        <div class="col-sm-6"><h4><%=this.Dictionary["Item_EquipmentMaintenanceDefinition_Plurar"] %></h4></div>
                                                        <div class="col-sm-6" style="text-align:right"><%=this.MaintenanceNewConfiguration.Render  %></div>
                                                    </div>
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceDefinition','text',false);" id="th0" class="sort" style="width:400px;"><%=this.Dictionary["Item_EquipmentMaintenanceDefinition_Header_Operation"] %></th>
                                                                <th style="width:90px;cursor:pointer;"><%=this.Dictionary["Item_EquipmentMaintenanceDefinition_Header_Type"] %></th>	
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceDefinition','money',false);" id="th2" class="sort" style="width:150px;cursor:pointer;"><%=this.Dictionary["Item_EquipmentMaintenanceDefinition_Header_Periodicity"] %></th>	
                                                                <th><%=this.Dictionary["Item_EquipmentMaintenanceDefinition_Header_Accesories"] %></th>	
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceDefinition','money',false);" id="th4" class="sort" style="width:90px;cursor:pointer;"><%=this.Dictionary["Item_EquipmentMaintenanceDefinition_Header_Cost"] %></th>	
                                                                <th style="width:110px;">&nbsp;</th>											
                                                            </tr>
                                                        </thead>
                                                        <tbody id="TableEquipmentMaintenanceDefinitionContainer">
                                                            <tr>
                                                                <td colspan="6" style="padding:0;">
                                                                    <div class="scrollTable" style="width:100%;height:200px;overflow-y:scroll;">
                                                                        <table style="width:100%;">
                                                                            <tbody id="TableEquipmentMaintenanceDefinition"></tbody>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    <div class="col-sm-12" style="margin-bottom:4px;">
                                                        <div class="col-sm-6"><h4><%=this.Dictionary["Item_EquipmentMaintenanceAct_Plural"] %></h4></div>
                                                        <div class="col-sm-6" style="text-align:right"><%=this.MaintenanceNewAct.Render %></div>
                                                    </div>                                                                                                       
                                                    <table class="table table-bordered table-striped" id="EquipmentMaintenanceActTable">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceAct','date');" id="th0" class="sort" style="width:90px;"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Date"] %></th>
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceAct','text');" id="th1" class="sort" style="width:400px;"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Operation"] %></th>
                                                                <th id="th14" class=""><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Observations"] %></th>	
                                                                <th id="th15" class="" style="width:200px;"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Responsible"] %></th>	
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceAct','money');" id="th4" class="sort" style="width:70px;"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Cost"] %></th>	
                                                                <th onclick="Sort(this,'TableEquipmentMaintenanceAct','date');" id="th5" class="sort" style="width:120px;"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Expiration"] %></th>
                                                                <th style="width:110px;">&nbsp;</th>														
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="7" style="padding:0;">
                                                                    <div class="scrollTable" style="width:100%;height:200px;overflow-y:scroll;">
                                                                        <table style="width:100%;">
                                                                            <tbody id="TableEquipmentMaintenanceAct"></tbody>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4"><div id="TableEquipmentMaintenanceActTotalLabel" style="width:100%"></div></td>
                                                                <td align="right" style="font-weight: bold;"><span id="TableEquipmentMaintenanceActTotal"></span></td>
                                                                <td colspan="2">&nbsp;</td>
                                                            </tr>
                                                        </tbody>                                                        
                                                    </table>
                                                </div>
                                                <div id="reparaciones" class="tab-pane">                                                    
                                                    <div class="col-sm-12" style="margin-bottom:4px;">
                                                        <div class="col-sm-6"><h4><%=this.Dictionary["Item_EquipmentRepair_Title"] %></h4></div>
                                                        <div class="col-sm-6" style="text-align:right">                                                                 
                                                            <!--h4 class="pink"-->
                                                                <button class="btn btn-success" type="button" id="EquipmentRepairNewBtn">
                                                                    <i class="icon-plus bigger-110"></i>
                                                                    <%=this.Dictionary["Item_EquipmentRepair_ButtonNew"] %>
                                                                </button>
                                                            <!--/h4-->	
                                                        </div>
                                                    </div>										
                                                    <table class="table table-bordered table-striped" id="TableEquipmentRepairMain">
                                                        <thead class="thin-border-bottom">
                                                            <tr>
                                                                <th onclick="Sort(this,'TableEquipmentRepair', 'date');" id="th0" class="sort" style="cursor:pointer;width:90px;"><%=this.Dictionary["Item_EquipmentRepair_Header_Date"] %></th>
                                                                <th onclick="Sort(this,'TableEquipmentRepair', 'text');" id="th1" class="sort" style="cursor:pointer;"><%=this.Dictionary["Item_EquipmentRepair_Header_Operation"] %></th>
                                                                <!--<th><%=this.Dictionary["Item_EquipmentRepair_Header_Observations"] %></th>	-->
                                                                <th onclick="Sort(this,'TableEquipmentRepair', 'text');" id="th3" class="sort" style="width:200px;cursor:pointer;"><%=this.Dictionary["Item_EquipmentRepair_Header_Responsible"] %></th>	
                                                                <th onclick="Sort(this,'TableEquipmentRepair', 'money');" id="th4" class="sort" style="width:90px;cursor:pointer;"><%=this.Dictionary["Item_EquipmentRepair_Header_Cost"] %></th>	
                                                                <th style="width:110px;" align="center">&nbsp;</th>										
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="5" style="padding:0;">
                                                                    <div class="scrollTable" style="width:100%;height:380px;overflow-y:scroll;">
                                                                        <table style="width:100%;">
                                                                            <tbody id="TableEquipmentRepair"></tbody>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3"><div id="TableEquipmentRepairTotalLabel" style="width:100%"></div></td>
                                                                <td style="font-weight:bold;text-align:right;"><span id="TableEquipmentRepairTotal"></span></td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div id="registros" class="tab-pane">
                                                    <h4><%=this.Dictionary["Item_Equipment_Tab_Records"] %></h4> 
                                                    <table cellpadding="2" cellspacing="2">
                                                        <tr>
                                                            <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_EquipmentRecord_Filter_Periode1"] %>:</strong></td>
										                    <td>
                                                                <div class="col-xs-12 col-sm-12">
												                    <div class="input-group">
													                    <input class="form-control date-picker" style="width:100px;" id="TxtRecordsFromDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                    <span class="input-group-addon" onclick="document.getElementById('TxtRecordsFromDate').focus();" id="TxtRecordsFromDateBtn">
														                    <i class="icon-calendar bigger-110"></i>
													                    </span>
												                    </div>
											                        <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                        <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                        <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                </div>
										                    </td>
                                                            <td id="TxtDateToLabel"><%=this.Dictionary["Item_EquipmentRecord_Filter_Periode2"] %></td>
										                    <td>
                                                                <div class="col-xs-12 col-sm-12">
												                    <div class="input-group">
													                    <input class="form-control date-picker" style="width:100px;" id="TxtRecordsToDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													                    <span class="input-group-addon" onclick="document.getElementById('TxtRecordsToDate').focus();" id="TxtRecordsToDateBtn">
														                    <i class="icon-calendar bigger-110"></i>
													                    </span>
												                    </div>
											                        <span class="ErrorMessage" id="TxtDateToErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											                        <span class="ErrorMessage" id="TxtDateToErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											                        <span class="ErrorMessage" id="TxtDateToDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                                </div>
										                    </td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td><strong><%=this.Dictionary["Item_EquipmentRecord_Filter_Type"] %>:</strong></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="CalInt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_CalibrationInternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="VerInt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_VerificationInternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="ManInt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_MaintenanceInternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="RepInt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_RepairInternal"] %></td>

                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
										                    <td>
                                                                <div class="col-xs-12 col-sm-12">
												                    <button class="btn btn-success" type="button" id="BtnRecordShowAll" title="<%=this.Dictionary["Common_All_Male_Plural"] %>"><i class="icon-list"></i></button>
                                                                    <button class="btn btn-success" type="button" id="BtnRecordShowNone" title="<%=this.Dictionary["Common_None_Male"] %>"><i class="icon-remove-circle"></i></button>
                                                                </div>
										                    </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
										                    <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
										                    <td>&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="CalExt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_CalibrationExternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="VerExt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_VerificationExternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="ManExt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_MaintenanceExternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="RepExt" /><%=this.Dictionary["Item_EquipmentRecord_Filter_RepairExternal"] %></td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
										                    <td>
                                                                <div class="col-xs-12 col-sm-12">
                                                                    <!--<button class="btn btn-success" type="button" id="BtnRecordFilter"><i class="icon-filter bigger-110"></i><%=this.Dictionary["Item_EquipmentRecord_Filter_Button"] %></button>-->
                                                                    <!--button class="btn btn-success" type="button" id="BtnRecordShowNone"><i class="icon-remove-circle bigger-110"></i><%=this.Dictionary["Common_None_Male"] %></button-->
                                                                </div>
										                    </td>
                                                        </tr>
                                                    </table>

                                                    <div style="height:12px;clear:both"></div>
                                                    <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom" id="RegistrosTHead">
                                                            <tr>
                                                                <td colspan="5" align="right">
                                                                    <span title="<%=this.Dictionary["Common_PrintPdf"] %>" class="btn btn-xs btn-info" onclick="EquipmentRecordGetFromFilter('PDF');"><i class="icon-file-pdf bigger-120"></i>&nbsp;PDF</span>
                                                                    &nbsp;
                                                                    <span title="<%=this.Dictionary["Common_PrintExcel"] %>" class="btn btn-xs btn-info" onclick="EquipmentRecordGetFromFilter('Excel');"><i class="icon-file-excel bigger-120"></i>&nbsp;Excel</span>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th onclick="Sort(this,'EquipmentRecordTable','date',true);" id="th0" class="sort" style="width:90px;"><%=this.Dictionary["Item_EquipmentRepair_HeaderList_Date"]%></th>
                                                                <th onclick="Sort(this,'EquipmentRecordTable','text',true);" id="th1" class="sort" style="width:150px;"><%=this.Dictionary["Item_EquipmentRepair_HeaderList_Type"]%></th>
                                                                <th onclick="Sort(this,'EquipmentRecordTable','text',true);" id="th2" class="sort"><%=this.Dictionary["Item_EquipmentRepair_HeaderList_Operation"]%></th>
                                                                <th onclick="Sort(this,'EquipmentRecordTable','text',true);" id="th3" class="sort" style="width:150px;"><%=this.Dictionary["Item_EquipmentRepair_HeaderList_Responsible"]%></th>
                                                                <th onclick="Sort(this,'EquipmentRecordTable','money',true);" id="th4" class="sort" style="width:90px;"><%=this.Dictionary["Item_EquipmentRepair_HeaderList_Cost"]%></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="EquipmentRecordTable"></tbody>
                                                        <tfoot id="ItemTableError" style="display:none;">
                                                            <tr>
                                                                <td colspan="10" style="background-color:#ffd;color:#a00;text-align:center;">
                                                                    <table style="border:none;">
                                                                        <tr>
                                                                            <td rowspan="2" style="border:none;"><i class="icon-warning-sign" style="font-size:48px;"></i></td>        
                                                                            <td style="border:none;"><h4><%=this.Dictionary["Item_EquipmentRecords_FilterError_Title"] %></h4></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="border:none;">
                                                                                <ul>
                                                                                    <li id="ErrorItem"><%=this.Dictionary["Item_EquipmentRecords_FilterError_Item"]%></li>
                                                                                    <li id="ErrorDate"><%=this.Dictionary["Item_EquipmentRecords_FilterError_Dates"]%></li>
                                                                                    <li id="ErrorDateMalformedFrom"><%=this.Dictionary["Item_EquipmentRecord_Error_FilterDateFromMalformed"]%></li>
                                                                                    <li id="ErrorDateMalformedTo"><%=this.Dictionary["Item_EquipmentRecord_Error_FilterDateToMalformed"]%></li>
                                                                                </ul>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
                                                        <tfoot id="ItemTableVoid" style="display:none;">
                                                            <tr>
                                                                <td colspan="10" style="background-color:ddf;color:00a;text-align:center;">
                                                                    <table style="border:none;">
                                                                        <tr>
                                                                            <td rowspan="2" style="border:none;" class="NoData"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                            <td style="border:none;"><h4 class="NoData" style="font-size:24px;"><%=this.Dictionary["Common_VoidSearchResult"] %></h4></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
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
                                                                    <%=this.Dictionary["Item_DocumentAttachment_Button_New"] %>
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
                                            </!--div>                                            
                                        </div>
                                    </div>
                                </div>                                
                                <div style="height:30px;">&nbsp;</div>
                            </div>
                            <!-- Popup dialogs -->
                            
                            <div id="dialogNewMaintaiment" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="FormDialogNewMaintaiment">
                                    <div class="form-group">
                                        <div class="col-sm-6">
                                            <input type="radio" id="RMaintainmentTypeInternal" name="RMaintainmentType" onclick="dialogNewMaintaimentTypeChanged();" />Interna
                                        </div>
                                        <div class="col-sm-6">
                                            <input type="radio" id="RMaintainmentTypeExternal" name="RMaintainmentType" onclick="dialogNewMaintaimentTypeChanged();" />Externa
                                        </div>
                                    </div>  
                                    <div class="form-group">
                                        <span class="ErrorMessage" id="RMaintainmentTypeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                    </div>                              
                                    <div class="form-group">
                                        <label id ="TxtNewMaintainmentOperationLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewMaintainmentOperation"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Operation"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <!--<input type="text" class="col-xs-12 col-sm-12" id="TxtNewMaintainmentOperation" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Operation"] %>" value="" maxlength="200" onblur="this.value=$.trim(this.value);" />-->
                                            <textarea class="col-xs-12 col-sm-12" id="TxtNewMaintainmentOperation" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Operation"] %>" rows="4" maxlength="200" onblur="this.value=$.trim(this.value);"></textarea>
                                            <span class="ErrorMessage" id="TxtNewMaintainmentOperationErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtNewMaintainmentOperationErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtNewMaintainmentPeriodicityLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewMaintainmentPeriodicity"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Periodicity"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12 integerFormated" id="TxtNewMaintainmentPeriodicity" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Periodicity"] %>" value="" maxlength="8" />
                                            <span class="ErrorMessage" id="TxtNewMaintainmentPeriodicityErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>                             
                                    <div class="form-group">
                                        <label id ="TxtNewMaintainmentAccessoriesLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewMaintainmentAccesories"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Accesories"] %></label>
                                        <div class="col-sm-9">
                                            <!--<input type="text" class="col-xs-12 col-sm-12" id="TxtNewMaintainmentAccessories" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Accesories"] %>" value="" maxlength="100" onblur="this.value=$.trim(this.value);" />-->
                                            <textarea class="col-xs-12 col-sm-12" id="TxtNewMaintainmentAccessories" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Accesories"] %>" rows="3" maxlength="100" onblur="this.value=$.trim(this.value);"></textarea>
                                        </div>
                                    </div>                           
                                    <div class="form-group">
                                        <!-- ISSUS-18 -->
                                        <label id ="TxtNewMaintainmentCostLabel" class="col-sm-3 control-label no-padding-right" for="TxtNewMaintainmentCost"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Amount"] %></label>
                                        <div class="col-sm-9">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtNewMaintainmentCost" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Amount"] %>" value="" maxlength="8" />
                                        </div>
                                    </div>                                   
                                    <div class="form-group" id="dialogNewMaintaimentProviderRow" style="display:none;">
                                        <label id="CmbNewMaintainmentProviderLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Provider"] %><span class="required">*</span></label>
                                        <div class="col-sm-7" id="CmbNewMaintainmentProviderDiv" style="height:35px !important;">
                                            <select id="CmbNewMaintainmentProvider" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbNewMaintainmentProviderValue" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Provider"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbNewMaintainmentProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                        <div class="col-sm-1" id="MaintenanceDefinitionDivProviderBar"><span class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnNewMaintainmentProviderBAR">...</span></div>
                                    </div>
                                    <div class="form-group">
                                        <label id="CmbNewMaintainmentResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Responsible"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                                <select id="CmbNewMaintainmentResponsible" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbNewMaintainmentResponsibleValue" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_FieldLabel_Responsible"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbNewMaintainmentResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="NewMaintainmentFirstDateLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_FirstDate"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtNewMaintainmentFirstDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="NewMaintainmentFirstDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="NewMaintainmentFirstDateBtn" class="input-group-addon" onclick="document.getElementById('NewMaintainmentFirstDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            <span class="ErrorMessage" id="TxtNewMaintainmentFirstDateErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtNewMaintainmentFirstDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"]%></span>
                                            <span class="ErrorMessage" id="TxtNewMaintainmentFirstDateOverTime">No pot ser anterior al darrer manteniment</span>
                                            <span class="ErrorMessage" id="TxtNewMaintainmentFirstDateOverTimeEquipment">No pot ser anterior a l'alta del equip</span>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            
                            <div id="dialogEquipmentMaintananceDefinitionDelete" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_EquipmentMaintenance_PopupConfigurationDelete_Question"] %>&nbsp;<strong><span id="dialogEquipmentMaintananceDefinitionDeleteName"></span></strong>?</p>
                            </div>


                            <!-- EQUIPMENTMAINTENANCEACT POPUPS -->
                            <div id="dialogNewEquipmentMaintenanceAct" class="hide" style="width:500px;">
                                <form class="form-horizontal" role="form" id="Form1">  
                                    <!--
                                    <span style="font-size:24px;font-weight:bold;" id="dialogNewEquipmentMaintenanceActOperation"></span>
                                    <hr />
                                    -->
                                    <div class="form-group">
                                        <label id="CmbEquipmentMaintenanceTypeLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Type"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmbEquipmentMaintenanceType" class="col-xs-12 col-sm-12" onchange="CmbEquipmentMaintenanceTypeChanged(this);"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbEquipmentMaintenanceTypeValue" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Responsible"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbEquipmentMaintenanceTypeErrorRequired"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_Error_NoDefinitionSelected"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtEquipmentMaintenanceActDateLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Date"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtEquipmentMaintenanceActDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEquipmentMaintenanceActDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtEquipmentMaintenanceActDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEquipmentMaintenanceActDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            <span class="ErrorMessage" id="TxtEquipmentMaintenanceActDateErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtEquipmentMaintenanceActDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"]%></span>
                                            <span class="ErrorMessage" id="TxtEquipmentMaintenanceActDateOverTime">No pot ser anterior al darrer manteniment</span>
                                        
                                            </div>
                                        </div>
                                        <!-- ISSUS-18 -->
                                        <label id ="TxtEquipmentMaintenanceActCostLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentMaintenanceActCost"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Amount"] %></label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtEquipmentMaintenanceActCost" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Amount"] %>" value="" maxlength="8" />
                                        </div>
                                    </div>                               
                                    <div class="form-group">
                                        <label id ="TxtEquipmentMaintenanceActObservationsLabel" class="col-sm-12" for="TxtEquipmentMaintenanceActObservations"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Observations"] %></label>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <textarea class="form-control col-xs-12 col-sm-12" id="TxtEquipmentMaintenanceActObservations" maxlength="250" rows="5"></textarea>
                                        </div>
                                    </div>
                                    <div class="form-group" id="CmbEquipmentMaintenanceActProviderRow">
                                        <label id="CmbEquipmentMaintenanceActProviderLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Provider"] %><span class="required">*</span></label>
                                        <div class="col-sm-7" id="CmbEquipmentMaintenanceActProviderDiv" style="height:35px !important;">
                                            <select id="CmbEquipmentMaintenanceActProvider" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbEquipmentMaintenanceActProviderValue" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Provider"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbEquipmentMaintenanceActProviderErrorRequired""><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                        <div class="col-sm-1" id="CmbEquipmentMaintenanceActProviderBar"><span class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnCmbEquipmentMaintenanceActProviderBAR">...</span></div>
                                    </div>
                                    <div class="form-group">
                                        <label id="CmbEquipmentMaintenanceActResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Responsible"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmbEquipmentMaintenanceActResponsible" class="form-control col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbEquipmentMaintenanceActResponsibleValue" placeholder="<%=this.Dictionary["Item_EquipmentMaintenance_Popup_Register_FieldLabel_Responsible"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbEquipmentMaintenanceActResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>       
                            
                            <div id="dialogEquipmentMaintananceActDelete" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="dialogDeleteEquipmentMaintenanceActName"></span></strong>?</p>
                            </div>    
                            <!-- EQUIPMENTMAINTENANCEACT POPUPS END -->     
                            
                            <!-- EQUIPMENTREPAIR POPUPS -->
                            <div id="dialogEquipmentRepairForm" class="hide" style="width:600px;">
                                <form class="form-horizontal" role="form">  
                                    <div class="form-group">
                                        <div class="col-sm-6">
                                            <input type="radio" id="REquipmentRepairTypeInternal" name="REquipmentRepairType" onclick="dialogMaintaimentRepairTypeChanged();" /><%=this.Dictionary["Common_Internal"] %>
                                        </div>
                                        <div class="col-sm-6">
                                            <input type="radio" id="REquipmentRepairTypeExternal" name="REquipmentRepairType" onclick="dialogMaintaimentRepairTypeChanged();" /><%=this.Dictionary["Common_External"] %>
                                        </div>
                                    </div>  
                                    <div class="form-group">
                                        <span class="ErrorMessage" id="REquipmentRepairTypeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                    </div> 
                                    <div class="form-group">
                                        <label id="TxtEquipmentRepairDateLabel" class="col-sm-3 control-label no-padding-right">Data<span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtEquipmentRepairDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEquipmentRepairDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtEquipmentRepairDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEquipmentRepairDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <label id ="TxtEquipmentRepairCostLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentRepairCost">Coste</label>
                                        <div class="col-sm-3">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtEquipmentRepairCost" placeholder="Coste" value="" maxlength="8" />
                                            <span class="ErrorMessage" id="TxtEquipmentRepairCostErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id ="TxtEquipmentRepairDescriptionLabel" class="col-sm-12" for="TxtEquipmentRepairDescription">Descripción<span class="required">*</span></label>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtEquipmentRepairDescription" maxlength="250" rows="3"></textarea>
                                            <span class="ErrorMessage" id="TxtEquipmentRepairDescriptionErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id ="TxtEquipmentRepairToolsLabel" class="col-sm-12" for="TxtEquipmentRepairTools">Material utilizado</label>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtEquipmentRepairTools" maxlength="250" rows="3"></textarea>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id ="TxtEquipmentRepairObservationsLabel" class="col-sm-12" for="TxtEquipmentRepairObservations">Observaciones</label>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtEquipmentRepairObservations" maxlength="250" rows="3"></textarea>
                                        </div>
                                    </div>
                                    <div class="form-group" id="CmbEquipmentRepairProviderRow">
                                        <label id="CmbEquipmentRepairProviderLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_Equipment_Field_Calibration_Provider"] %><span class="required">*</span></label>
                                        <div class="col-sm-7" id="CmbEquipmentRepairProviderDiv" style="height:35px !important;">
                                            <select id="CmbEquipmentRepairProvider" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbEquipmentRepairProviderValue" placeholder="Proveedor" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbEquipmentRepairProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                        <div class="col-sm-1" id="CmbEquipmentRepairProviderBar"><span class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnCmbEquipmentRepairProviderBAR">...</span></div>
                                    </div>
                                    <div class="form-group">
                                        <label id="CmbEquipmentRepairResponsibleLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Common_Responsible"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <select id="CmbEquipmentRepairResponsible" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmbEquipmentRepairResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                </form>
                            </div>       
                            
                            <div id="dialogEquipmentRepairDelete" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="dialogDeleteEquipmentRepairName"></span></strong>?</p>
                            </div>   
                            <!-- EQUIPMENTREPAIR POPUPS END -->
                            
                            <div id="dialogEquipmentCalibrationForm" class="hide" style="width:550px;overflow:hidden;">
                                <form class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <div class="col-sm-6">
                                            <input type="radio" id="REquipmentCalibrationActTypeInternal" name="REquipmentCalibrationActType" onclick="REquipmentCalibrationActTypeChanged();" /><%=this.Dictionary["Common_Internal"] %>
                                        </div>
                                        <div class="col-sm-6">
                                            <input type="radio" id="REquipmentCalibrationActTypeExternal" name="REquipmentCalibrationActType" onclick="REquipmentCalibrationActTypeChanged();" /><%=this.Dictionary["Common_External"] %>
                                        </div>
                                    </div>      
                                    <div class="form-group">
                                        <span class="ErrorMessage" id="REquipmentCalibrationActTypeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                    </div> 
                                    <div class="form-group">
                                        <label id="TxtEquipmentCalibrationActDateLabel" class="col-sm-2 control-label no-padding-right">Data<span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtEquipmentCalibrationActDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEquipmentCalibrationActDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtEquipmentCalibrationActDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEquipmentCalibrationActDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <span class="ErrorMessage" id="TxtEquipmentCalibrationActDateErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtEquipmentCalibrationActDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"]%></span>
                                            <span class="ErrorMessage" id="TxtEquipmentCalibrationActDateOverTime">No pot ser anterior a la darrera calibració</span>
                                        </div>
                                    </div>    
                                    <div class="form-group">
                                        <label id ="TxtEquipmentCalibrationActResultLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentCalibrationActResult"><%=this.Dictionary["Item_Equipment_Field_Calibration_Result"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <input type="text" class="col-xs-12 col-sm-12 decimalFormated6" id="TxtEquipmentCalibrationActResult" placeholder="<%=this.Dictionary["Item_Equipment_Field_Calibration_Result"] %>" value="" maxlength="12" />
                                            <span class="ErrorMessage" id="TxtEquipmentCalibrationActResultErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
                                        <label id ="TxtEquipmentCalibrationActCostLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentCalibrationActCost"><%=this.Dictionary["Item_Equipment_Field_Calibration_Cost"] %></label>
                                        <div class="col-sm-4">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtEquipmentCalibrationActCost" placeholder="<%=this.Dictionary["Item_Equipment_Field_Calibration_Cost"] %>" value="" maxlength="12" />                                            
                                        </div>
                                    </div>                                    
                                    <div class="form-group" id="CmbEquipmentCalibrationActProviderRow">
                                        <label id="CmbEquipmentCalibrationActProviderLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Equipment_Field_Calibration_Provider"] %><span class="required">*</span></label>
                                        <div class="col-sm-8" id="CmbEquipmentCalibrationActProviderDiv" style="height:35px !important;">
                                            <select id="CmbEquipmentCalibrationActProvider" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbEquipmentCalibrationActProviderValue" placeholder="><%=this.Dictionary["Item_Equipment_Field_Calibration_Provider"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbEquipmentCalibrationActProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                        <div class="col-sm-1" id="CmbCalibrationActProviderBar"><span class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnCalibrationActProviderBAR">...</span></div>
                                    </div>
                                    <div class="form-group">
                                        <label id="CmbEquipmentCalibrationActResponsibleLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Common_Responsible"] %><span class="required">*</span></label>
                                        <div class="col-sm-10">
                                            <select id="CmbEquipmentCalibrationActResponsible" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmbEquipmentCalibrationActResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id ="TxtEquipmentCalibrationActObservationsLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentCalibrationActResult"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Observations"] %></label>
                                        <div class="col-sm-12">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtEquipmentCalibrationActObservations" rows="3"></textarea>
                                        </div>
                                    </div>    
                                </form>
                            </div>                       
                            
                            <div id="dialogEquipmentVerificacionForm" class="hide" style="width:580px;overflow:hidden;">
                                <form class="form-horizontal" role="form" id="Form3">
                                    <div class="form-group">
                                        <div class="col-sm-6">
                                            <input type="radio" id="REquipmentVerificationActTypeInternal" name="REquipmentVerificationActType" onclick="REquipmentVerificationActTypeChanged();" /><%=this.Dictionary["Common_Internal"] %>
                                        </div>
                                        <div class="col-sm-6">
                                            <input type="radio" id="REquipmentVerificationActTypeExternal" name="REquipmentVerificationActType" onclick="REquipmentVerificationActTypeChanged();" /><%=this.Dictionary["Common_External"] %>
                                        </div>
                                    </div>      
                                    <div class="form-group">
                                        <span class="ErrorMessage" id="REquipmentVerificationActTypeErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                    </div>  
                                    <div class="form-group">
                                        <label id="TxtEquipmentVerificationActDateLabel" class="col-sm-2 control-label no-padding-right">Data<span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtEquipmentVerificationActDateDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEquipmentVerificationActDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtEquipmentVerificationActDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEquipmentVerificationDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <span class="ErrorMessage" id="TxtEquipmentVerificationActDateErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                            <span class="ErrorMessage" id="TxtEquipmentVerificationActDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"]%></span>                                        
                                            <span class="ErrorMessage" id="TxtEquipmentVerificationActDateOverTime">No pot ser anterior a la darrera verificació</span>
                                        </div>
                                    </div>    
                                    <div class="form-group">
                                        <label id ="TxtEquipmentVerificationActResultLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentVerificationActResult"><%=this.Dictionary["Item_Equipment_Field_Calibration_Result"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <input type="text" class="col-xs-12 col-sm-12 decimalFormated4" id="TxtEquipmentVerificationActResult" placeholder="<%=this.Dictionary["Item_Equipment_Field_Calibration_Result"] %>" value="" maxlength="12" />
                                            <span class="ErrorMessage" id="TxtEquipmentVerificationActErrorRequired"><%=this.Dictionary["Common_Required"]%></span>
                                        </div>
	
	
                                        <label id ="TxtEquipmentVerificationActCostLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentVerificationActCost"><%=this.Dictionary["Item_Equipment_Field_Calibration_Cost"] %></label>
                                        <div class="col-sm-4">
                                            <input type="text" class="col-xs-12 col-sm-12 money-bank" id="TxtEquipmentVerificationActCost" placeholder="<%=this.Dictionary["Item_Equipment_Field_Calibration_Cost"] %>" value="" maxlength="12" />
                                            <span class="ErrorMessage" id="TxtEquipmentVerificationActCostMalformed"><%=this.Dictionary["Common_Error_MoneyMalformed"] %></span>
                                        </div>
                                    </div>                                     
                                    <div class="form-group" id="CmbEquipmentVerificationActProviderRow">
                                        <label id="CmbEquipmentVerificationActProviderLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Item_Equipment_Field_Verification_Provider"] %><span class="required">*</span></label>
                                        <div class="col-sm-8" id="CmbEquipmentVerificationActProviderDiv" style="height:35px !important;">
                                            <select id="CmbEquipmentVerificationActProvider" class="col-xs-12 col-sm-12"></select>
                                            <input style="display:none;" type="text" readonly="readonly" id="CmbEquipmentVerificationActProviderValue" placeholder="><%=this.Dictionary["Item_Equipment_Field_Verification_Provider"] %>" class="col-xs-12 col-sm-12" />
                                            <span class="ErrorMessage" id="CmbEquipmentVerificationActProviderErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                        <div class="col-sm-1" id="CmbVerificationActProviderBar"><span class="btn btn-light" style="height:30px;" title="<%=this.Dictionary["Item_Providers"] %>" id="BtnVerificationActProviderBAR">...</span></div>
                                    </div>
                                    <div class="form-group">
                                        <label id="CmbEquipmentVerificationActResponsibleLabel" class="col-sm-2 control-label no-padding-right"><%=this.Dictionary["Common_Responsible"] %><span class="required">*</span></label>
                                        <div class="col-sm-10">
                                            <select id="CmbEquipmentVerificationActResponsible" class="col-xs-12 col-sm-12"></select>
                                            <span class="ErrorMessage" id="CmbEquipmentVerificationActResponsibleErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id ="TxtEquipmentVerificationActObservationsLabel" class="col-sm-2 control-label no-padding-right" for="TxtEquipmentVerificationActResult"><%=this.Dictionary["Item_EquipmentMaintenanceAct_Header_Observations"] %></label>
                                        <div class="col-sm-12">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtEquipmentVerificationActObservations" rows="3"></textarea>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            <!-- Fin popup dialogs -->

                            
                            
                            <div id="dialogEquipmentCalibrationActDelete" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_EquipmentCalibrationAct_PopupDelete_Question"] %>&nbsp;<strong><span id="dialogEquipmentCalibrationActDeleteName"></span></strong>?</p>
                            </div>                            
                            
                            <div id="dialogEquipmentVerificationActDelete" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="dialogEquipmentVerificationActDeleteName"></span></strong>?</p>
                            </div>

                            <div id="ChangeImageDialog" class="hide" style="width:850px;">
                                <div class="form-group">
                                    <label class="col-sm-6 control-label"><%=this.Dictionary["Item_Equipment_Field_Image_Actual_Label"] %></label>
                                    <label class="col-sm-6 control-label"><%=this.Dictionary["Item_Equipment_Field_Image_New_Label"] %></label>
                                    <div class="col-sm-6" style="border:1px solid #aaa;vertical-align:middle;text-align:center;height:200px;background-color:#fefefe;padding:4px;">
                                        <img id="actual" src="/images/equipments/<%=this.Equipment.Image %>" alt="<%=this.Dictionary["Item_Equipment_Field_Image_Actual_Label"] %>" style="max-width:192px;max-height:192px;" />
                                    </div>
                                    <div class="col-sm-6" style="border:1px solid #aaa;border-left:none;vertical-align:middle;text-align:center;height:200px;background-color:#fefefe;padding:4px;">
                                        <img id="blah" src="/images/noimage.jpg" alt="<%=this.Dictionary["Item_Equipment_Field_Image_New_Label"] %>" style="max-width:192px;max-height:192px;" />                                                                    
                                    </div>
                                    <div class="col-sm-12" style="margin-top:12px;">
                                        <input type='file' id="imgInp" />
                                    </div>
                                </div>                                
                            </div>
                            
                            <div id="DeleteImageDialog" class="hide" style="width:500px;">
                                <p>¿Seguro&nbsp;<strong><span id="Span1"></span></strong>?</p>
                            </div>

    
                            <div id="PopupUploadFile" class="hide" style="width:800px;">
                                <div class="table-responsive">
                                    <form action="/dummy.html" class="dropzone well dz-clickable" id="dropzone">
                                        <input type="file" id="fileName" name="fileName" multiple style="position:absolute;top:-100000px;"/>
                                        <div class="dz-default dz-message">
                                            <span id="UploadMessage">
                                                <span class="bigger-150 bolder">
                                                    <i class="ace-icon fa fa-caret-right red"></i>
                                                    <%=this.Dictionary["Item_DocumentAttachment_UpladTitle1"] %>
                                                </span>
                                                <%=this.Dictionary["Item_DocumentAttachment_UpladTitle2"] %>
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
                                            <label class="input-append col-sm-2"><%=this.Dictionary["Item_DocumentAttachment_PopupUpload_Description_Label"] %></label>
                                            <label class="input-append col-sm-10"><input class="col-sm-11" id="UploadFileDescription" name="UploadFileDescription" /></label>
                                        </div>
                                        <!--<div class="col-sm-12">
                                            <p><input type="checkbox" /> Guardar como copia local</p>
                                        </div>-->
                                </div><!-- /.table-responsive -->
                            </div><!-- #dialog-message -->

                            <div id="dialogAnular" class="hide" style="width: 500px;">
                                <form class="form-horizontal" role="form" id="FormDialogAnular">
                                    <div class="form-group">
                                        <label id="TxtEndReasonLabel" class="col-sm-3 control-label no-padding-right" for="TxtRegistroComments"><%=this.Dictionary["Item_Equipment_FieldLabel_EndReason"] %><span class="required">*</span></label>
                                        <div class="col-sm-9">
                                            <textarea class="col-xs-12 col-sm-12" id="TxtEndReason" rows="5"></textarea>
                                            <span class="ErrorMessage" id="TxtEndReasonErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="TxtEndDateLabel" class="col-sm-3 control-label no-padding-right" for="TxtRecordDate"><%=this.Dictionary["Item_Equipment_FieldLabel_EndDate"] %><span class="required">*</span></label>
                                        <div class="col-sm-4">
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 tooltip-info" id="TxtFechaCierreRealDiv">
                                                    <div class="input-group">
                                                        <input class="form-control date-picker" id="TxtEndDate" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                        <span id="TxtTxtEndDateBtn" class="input-group-addon" onclick="document.getElementById('TxtEndDate').focus();">
                                                            <i class="icon-calendar bigger-110"></i>
                                                        </span>
                                                    </div>
                                                    <span class="ErrorMessage" id="TxtEndDateErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                                    <span class="ErrorMessage" id="TxtEndDateMalformed"><%= this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <%=this.CmbResponsibleClose.Render %>
                                    </div>
                                </form>
                            </div>

                            <!----------- BAR ----------------->
                            <%=this.EquipmentScaleDivisionBarPopups.Render%>
                            <%=this.ProviderBarPopups.Render %>
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
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>""></script>
        <script type="text/javascript" src="/js/Equipment.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/Provider.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentMaintenanceDefinition.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentMaintenanceAct.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentCalibrationAct.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentVerificationAct.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentRepair.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentRecord.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentScaleDivision.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/EquipmentImage.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript">
                jQuery(function ($) {

                    $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                        "_title": function (title) {
                            var $title = this.options.title || "&nbsp;"
                            if (("title_html" in this.options) && this.options.title_html == true) {
                                title.html($title);
                            }
                            else {
                                title.text($title);
                            }
                        }
                    }));

                    var options = $.extend({}, $.datepicker.regional[user.Language], { "dateFormat": "dd/mm/yy", "autoclose": true, "todayHighlight": true });
                    $(".date-picker").datepicker(options);
                    $(".hasDatepicker").on("blur", function () { DatePickerChanged(this); });

                    $("#BtnRecordFilter").on("click", function (e) {
                        e.preventDefault();
                        EquipmentRecordGetFromFilter();
                    });

                    $("#BtnRecordShowAll").on("click", function (e) {
                        e.preventDefault();
                        EquipmentRecordGetAll();
                    });

                    $("#BtnRecordShowNone").on("click", function (e){
                        e.preventDefault();
                        EquipmentRecordGetNone();
                    });

                    $("#BtnEquipmentChangeImage").on("click", function (e) {
                        e.preventDefault();
                        EquipmentChangeImage();
                    });

                    $("#BtnEquipmentDeleteImage").on("click", function (e) {
                        e.preventDefault();
                        EquipmentDeleteImage();
                    });

                    $("#BtnSave").on("click", function (e) { e.preventDefault(); SaveEquipment(); });
                    $("#BtnCalibrationSave").on("click", function (e) { e.preventDefault(); SaveEquipment(); });
                    $("#BtnVerificationSave").on("click", function (e) { e.preventDefault(); SaveEquipment(); });
                    $("#BtnMaintenanceSave").on("click", function (e) { e.preventDefault(); SaveEquipment(); });
                    $("#BtnRepairSave").on("click", function (e) { e.preventDefault(); SaveEquipment(); });
                    $("#BtnRecordsSave").on("click", function (e) { e.preventDefault(); SaveEquipment(); });

                    $("#BtnCancel").on("click", function (e) { document.location = referrer; });
                    $("#BtnCalibrationCancel").on("click", function (e) { document.location = referrer; });
                    $("#BtnVerificationCancel").on("click", function (e) { document.location = referrer; });
                    $("#BtnMaintenanceCancel").on("click", function (e) { document.location = referrer; });
                    $("#BtnRepairCancel").on("click", function (e) { document.location = referrer; });
                    $("#BtnRecordsCancel").on("click", function (e) { document.location = referrer; });
                    
                    $("#BtnPrint").on("click", PrintData);
                    $("#BtnCalibrationPrint").on("click", PrintData);
                    $("#BtnVerificationPrint").on("click", PrintData);
                    $("#BtnMaintenancePrint").on("click", PrintData);
                    $("#BtnRepairPrint").on("click", PrintData);
                    $("#BtnRecordsPrint").on("click", PrintData);

                    $("#BtnNewMaintainment").on("click", function (e) {
                        e.preventDefault();
                        ShowMaintaimentPopup(0);
                    });
                    
                    $("#BtnNewMaintainmentAct").on("click", function (e){
                        e.preventDefault();
                        EquipmentMaintenanceDefinitionRegister(null);
                    });

                    $("#BtnNewCalibration").on("click", function (e) {
                        e.preventDefault();
                        ShowDialogNewCalibrationPopup(0);
                    });

                    $("#BtnNewVerification").on("click", function (e) {
                        e.preventDefault();
                        ShowDialogEquipmentVerificacionPopup(0);
                    });

                    $("#BtnEquipmentScaleDivisionBAR").on("click", function (e) {
                        e.preventDefault();
                        ShowEquipmentScaleDivisionBarPopup();
                    });

                    $("#BtnCalibrationActProviderBAR").on("click", function (e) {
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentCalibrationAct");
                    });

                    $("#BtnVerificationActProviderBAR").on("click", function (e) {
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentVerificationAct");
                    });

                    $("#BtnNewMaintainmentProviderBAR").on("click", function (e) {
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentMaintenanceDefinition");
                    });

                    $("#BtnCmbEquipmentMaintenanceActProviderBAR").on("click", function (e) {
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentMaintenanceAct");
                    });

                    $("#BtnCalibrationExternalProviderBAR").on("click", function (e){
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentCalibrationDefinition");
                    });

                    $("#BtnVerificationExternalProviderBAR").on("click", function (e){
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentVerificationDefinition");
                    });

                    $("#BtnCmbEquipmentRepairProviderBAR").on("click", function (e){
                        e.preventDefault();
                        ShowProviderBarPopup("EquipmentEquipmentRepair");
                    });

                    $("#EquipmentRepairNewBtn").on("click", function (e) {
                        e.preventDefault();
                        EquipmentRepairNew();
                    });
                    
                    $("#Contentholder1_status0").on("click", function(e)
                    {
                        CalibrationCheckChanged();
                    });

                    $("#Contentholder1_status1").on("click", function(e)
                    {
                        VerificationCheckChanged();
                    });

                    $("#Contentholder1_status2").on("click", function(e)
                    {
                        MaintenanceCheckChanged();
                    });
                    
                    $("#BtnCalibrationInternalSave").on("click", function(e)
                    {
                        e.preventDefault();
                        CalibrationInternalSave();
                    });
                    
                    $("#BtnCalibrationExternalSave").on("click", function(e)
                    {
                        e.preventDefault();
                        CalibrationExternalSave();
                    });
                    
                    $("#BtnVerificationInternalSave").on("click", function(e)
                    {
                        e.preventDefault();
                        VerificationInternalSave();
                    });
                    
                    $("#BtnVerificationExternalSave").on("click", function(e)
                    {
                        e.preventDefault();
                        VerificationExternalSave();
                    });
                    
                    if(ApplicationUser.ShowHelp===true){
                        SetToolTip("TxtCode",Dictionary.Item_Equipment_Help_Code);
                        SetToolTip("TxtDescription",Dictionary.Item_Equipment_Help_Description);
                        SetToolTip("TxtTradeMark",Dictionary.Item_Equipment_Help_TradeMark);
                        SetToolTip("TxtModel",Dictionary.Item_Equipment_Help_Model);
                        SetToolTip("TxtSerialNumber",Dictionary.Item_Equipment_Help_SerialNumber);
                        SetToolTip("TxtLocation",Dictionary.Item_Equipment_Help_Location);
                        SetToolTip("TxtMeasureRange",Dictionary.Item_Equipment_Help_MeasureRange);
                        SetToolTip("TxtScaleDivision",Dictionary.Item_Equipment_Help_ScaleDivision);
                        SetToolTip("DivCmbEquipmentScaleDivision",Dictionary.Item_Equipment_Help_EquipmentScaleDivision);
                        SetToolTip("BtnEquipmentScaleDivisionBAR",Dictionary.Item_Equipment_Help_EquipmentScaleDivisionBAR);
                        SetToolTip("DivCmbResponsible",Dictionary.Item_Equipment_Help_Responsible);
                        SetToolTip("Contentholder1_status0",Dictionary.Item_Equipment_Help_CalibrationCheck);
                        SetToolTip("Contentholder1_status1",Dictionary.Item_Equipment_Help_VerificationCheck);
                        SetToolTip("Contentholder1_status2",Dictionary.Item_Equipment_Help_MaintenanceCheck);
                        SetToolTip("TxtObservations",Dictionary.Item_Equipment_Help_Observations);
                        $("[data-rel=tooltip]").tooltip();
                    }

                    $("#BtnAnular").on("click", AnularPopup);
                    $("#BtnRestaurar").on("click", Restore);
                    
                    <%=this.Launch%>
                });

            function readURL(input) {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("#blah").attr("src", e.target.result);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
            }

            $("#imgInp").change(function () {
                console.log("image changed")
                readURL(this);
                return false;
            });

            function dialogNewMaintaimentTypeChanged() {
                if (document.getElementById("RMaintainmentTypeExternal").checked === true) {
                    $("#dialogNewMaintaimentProviderRow").show();
                }
                else {
                    $("#dialogNewMaintaimentProviderRow").hide();
                }
            }

            if (Equipment.Id > 0) {
                SetCalibrationForm();
                SetVerificationForm();
                EquipmentMaintenanceDefinitionRenderTable("TableEquipmentMaintenanceDefinition");
                EquipmentMaintenanceActRenderTable("TableEquipmentMaintenanceAct");
                EquipmentRepairRenderTable("TableEquipmentRepair");
                EquipmentCalibrationActRenderTable("TableEquipmentCalibrationAct");
                EquipmentVerificationActRenderTable("TableEquipmentVerificationAct");
            }

            function GetEmployeeById(id) {
                for (var x = 0; x < Employees.length; x++) {
                    if (Employees[x].Id === id) {
                        return Employees[x];
                    }
                }
                return null;
            }

            function CmbEquipmentScaleDivisionChanged() {
                $("#TxtEquipmentScaleDivision").val($("#CmbEquipmentScaleDivision").val());
            }

            function CmbResponsibleChanged() { /* NOOP */ }

            function EquipmentCalibrationActEdit(sender){
                SelectedEquipmentCalibrationActId = sender.parentNode.parentNode.id.substring(23) * 1;
                SelectedEquipmentCalibrationAct = EquipmentCalibrationActgetById(SelectedEquipmentCalibrationActId);
                if (SelectedEquipmentCalibrationAct == null) { return false; }
                FillCmbEquipmentCalibrationActResponsible();
                FillCmbEquipmentCalibrationActProvider();
                EquipmentCalibrationActEditFormFill(SelectedEquipmentCalibrationAct);
                $("#dialogEquipmentCalibrationForm").removeClass("hide").dialog({
                    "resizable": false,
                    "modal": true,
                    "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentCalibrationAct_PopupUpdate_Title + "</h4>",
                    "title_html": true,
                    "width": 550,
                    "buttons": [
                        {
                            "id": 'BtnNewEquipmentCalibrationActSave',
                            "html": "<i class='icon-refresh bigger-110'></i>&nbsp;" + Dictionary.Common_Update,
                            "class": "btn btn-success btn-xs",
                            "click": function () {
                                EquipmentCalibrationSave();
                            }
                        },
                        {
                            "id": 'BtnNewEquipmentCalibrationActCancel',
                            "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                            "class": "btn btn-xs",
                            "click": function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });

                if (Equipment.EndDate !== null) {
                    $("#BtnNewEquipmentCalibrationActSave").hide();
                }
            }

            function EquipmentCalibrationActDelete(sender){
                SelectedEquipmentCalibrationActId = sender.parentNode.parentNode.id.substring(23) * 1;
                SelectedEquipmentCalibrationAct = EquipmentCalibrationActgetById(SelectedEquipmentCalibrationActId);
                if (SelectedEquipmentCalibrationAct == null) { return false; }
                $("#dialogEquipmentCalibrationActDeleteName").html(" la calibración?");
                $("#dialogEquipmentCalibrationActDelete").removeClass("hide").dialog({
                    "resizable": false,
                    "modal": true,
                    "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentCalibrationAct_PopupDelete_Title + "</h4>",
                    "title_html": true,
                    "width": 500,
                    "buttons": [
                        {
                            "id": "BtnNewCalibrationActSave",
                            "html": "<i class=\"icon-trash bigger-110\"></i>&nbsp;" + Dictionary.Common_Delete,
                            "class": "btn btn-danger btn-xs",
                            "click": function () {
                                EquipmentCalibrationDeleteConfirmed();
                            }
                        },
                        {
                            "id": "BtnNewCalibrationActCancel",
                            "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                            "class": "btn btn-xs",
                            "click": function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            }

            function EquipmentVerificationActEdit(sender) {
                SelectedEquipmentVerificationActId = sender.parentNode.parentNode.id.substring(24) * 1;
                SelectedEquipmentVerificationAct = EquipmentVerificationActgetById(SelectedEquipmentVerificationActId);
                if (SelectedEquipmentVerificationAct == null) { return false; }
                FillCmbEquipmentVerificationActResponsible();
                FillCmbEquipmentVerificationActProvider();
                EquipmentVerificationActEditFormFill(SelectedEquipmentVerificationAct);
                $("#dialogEquipmentVerificacionForm").removeClass("hide").dialog({
                    "resizable": false,
                    "modal": true,
                    "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentVerification_PopupUpdate_Title + "</h4>",
                    "title_html": true,
                    "width": 550,
                    "buttons": [
                        {
                            "id": "BtnEquipmentVerificationActEditSave",
                            "html": "<i class=\"icon-refresh bigger-110\"></i>&nbsp;" + Dictionary.Common_Update,
                            "class": "btn btn-success btn-xs",
                            "click": function () {
                                EquipmentVerificationSave();
                            }
                        },
                        {
                            "id": "BtnEquipmentVerificationActEditCancel",
                            "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                            "class": "btn btn-xs",
                            "click": function () {
                                $(this).dialog("close");
                            }
                        }
                    ]

                });

                if (Equipment.EndDate !== null) {
                    $("#BtnEquipmentVerificationActEditSave").hide();
                }
            }

            function EquipmentVerificationActDelete(sender) {
                SelectedEquipmentVerificationActId = sender.parentNode.parentNode.id.substring(24) * 1;
                SelectedEquipmentVerificationAct = EquipmentVerificationActgetById(SelectedEquipmentVerificationActId);
                if (SelectedEquipmentVerificationAct == null) { return false; }

                $("#dialogEquipmentVerificationActDeleteName").html(" la verificación?");
                $("#dialogEquipmentVerificationActDelete").removeClass("hide").dialog({
                    "resizable": false,
                    "modal": true,
                    "title": "<h4 class=\"smaller\">" + Dictionary.Item_EquipmentVerification_Popup_Delete_Title + "</h4>",
                    "title_html": true,
                    "width": 500,
                    "buttons": [
                        {
                            "id": "EquipmentVerificationActDeleteBtnOk",
                            "html": "<i class='icon-trash bigger-110'></i>&nbsp;" + Dictionary.Common_Delete,
                            "class": "btn btn-danger btn-xs",
                            "click": function () {
                                EquipmentVerificationDeleteConfirmed();
                            }
                        },
                        {
                            "id": "EquipmentVerificationActDeleteBtnCancel",
                            "html": "<i class=\"icon-remove bigger-110\"></i>&nbsp;" + Dictionary.Common_Cancel,
                            "class": "btn btn-xs",
                            "click": function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            }

            function EquipmentCalibrationDeleteConfirmed() {
                var data = {
                    "equipmentCalibrationActId": SelectedEquipmentCalibrationActId,
                    "companyId": Company.Id,
                    "userId": user.Id
                };
                $.ajax({
                    "type": "POST",
                    "url": "/Async/EquipmentCalibrationActActions.asmx/Delete",
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": JSON.stringify(data, null, 2),
                    "success": function (msg) {
                        EquipmentCalibrationActRemoveFromList(SelectedEquipmentCalibrationActId);
                        EquipmentCalibrationActRenderTable("TableEquipmentCalibrationAct");
                        $("#dialogEquipmentCalibrationActDelete").dialog("close");

                    },
                    "error": function (msg) {
                        alertUI(msg.responseText);
                    }
                });
            }

            function EquipmentVerificationDeleteConfirmed(){
                var data = {
                    "equipmentVerificationActId": SelectedEquipmentVerificationActId,
                    "companyId": Company.Id,
                    "userId": user.Id
                };
                $.ajax({
                    "type": "POST",
                    "url": "/Async/EquipmentVerificationActActions.asmx/Delete",
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": JSON.stringify(data, null, 2),
                    "success": function (msg) {
                        EquipmentVerificationActRemoveFromList(SelectedEquipmentVerificationActId);
                        EquipmentVerificationActRenderTable('TableEquipmentVerificationAct');
                        $("#dialogEquipmentVerificationActDelete").dialog("close");

                    },
                    error: function (msg) {
                        alertUI(msg.responseText);
                    }
                });
            }
            
            document.getElementById("Tabcalibracion").style.display = Equipment.IsCalibration ? "" : "none";
            document.getElementById("Tabverificacion").style.display = Equipment.IsVerification ? "" : "none";
            document.getElementById("Tabmantenimiento").style.display = Equipment.IsMaintenance ? "" : "none";
            
            document.getElementById("CalibrationDivTable").style.display = Equipment.Id == 0 ? "none" : "";
            document.getElementById("VerificationDivTable").style.display = Equipment.Id == 0 ? "none" : "";

            // Style rectifications
            document.getElementById("TxtCalibrationInternalNotes").style.width = "90%";
            document.getElementById("TxtCalibrationInternalNotes").style.marginLeft = "5%";
            document.getElementById("TxtCalibrationExternalNotes").style.width = "90%";
            document.getElementById("TxtCalibrationExternalNotes").style.marginLeft = "5%";
            document.getElementById("TxtVerificationInternalNotes").style.width = "90%";
            document.getElementById("TxtVerificationInternalNotes").style.marginLeft = "5%";
            document.getElementById("TxtVerificationExternalNotes").style.width = "90%";
            document.getElementById("TxtVerificationExternalNotes").style.marginLeft = "5%";

            var emptyCalibration =
            {
                "Id": 0,
                "EquipmentId": 0,
                "CompanyId": 0,
                "CalibrationType": 0,
                "Description": "",
                "Periodicity": 0,
                "Uncertainty": 0,
                "Range": "",
                "FirstDate": null,
                "Pattern": "",
                "Cost": 0,
                "Notes": "",
                "Provider": { "Id": 0, "Value": "" },
                "Responsible":
                {
                    "Id": 0,
                    "CompanyId": 0,
                    "Name": "",
                    "LastName": ""
                }
            };

            var emptyVerification =
            {
                "Id": 0,
                "EquipmentId": 0,
                "CompanyId": 0,
                "CalibrationType": 0,
                "Description": "",
                "Periodicity": 0,
                "Uncertainty": 0,
                "Range": "",
                "FirstDate": null,
                "Pattern": "",
                "Cost": 0,
                "Notes": "",
                "Responsible":
                {
                    "Id": 0,
                    "CompanyId": 0,
                    "Name": "",
                    "LastName": ""
                }
            };

            if(Equipment.InternalCalibration==null)
            {
                Equipment.InternalCalibration = emptyCalibration;
            }

            if(Equipment.ExternalCalibration==null)
            {
                Equipment.ExternalCalibration = emptyCalibration;
            }

            if(Equipment.InternalVerification==null)
            {
                Equipment.InternalVerification = emptyVerification;
            }

            if(Equipment.ExternalVerification == null)
            {
                Equipment.ExternalVerification = emptyVerification;
            }

            if(user.Grants.Provider!=null)
            {
                if(user.Grants.Provider.Write === false)
                {
                    $("#MaintenanceDefinitionDivProviderBar").hide();
                    document.getElementById("CmbNewMaintainmentProviderDiv").className = "col-sm-9";
                }
            }
            else{                
                $("#MaintenanceDefinitionDivProviderBar").hide();
                document.getElementById("CmbNewMaintainmentProviderDiv").className = "col-sm-9";
            }

            for (var x = 0; x < EquipmentScaleDivision.length; x++) {
                if (EquipmentScaleDivision[x].Id === EquipmentScaleDivisionSelected) {
                    if(document.getElementById("TxtEquipmentScaleDivision")!==null){
                        document.getElementById("TxtEquipmentScaleDivision").value = EquipmentScaleDivision[x].Description;
                    }
                    break;
                }
            }

            FillCmbEquipmentScaleDivision();

            // Rellenar combos responsable
            $("#CmbResponsible").val(Equipment.Responsible.Id);
            $("#CmbCalibrationInternalResponsible").val(Equipment.InternalCalibration.Responsible.Id);
            $("#CmbCalibrationExternalResponsible").val(Equipment.ExternalCalibration.Responsible.Id);
            $("#CmbVerificationInternalResponsible").val(Equipment.InternalVerification.Responsible.Id);
            $("#CmbVerificationExternalResponsible").val(Equipment.ExternalVerification.Responsible.Id);
            
            if($("#CmbResponsible").val()===null) { $("#CmbResponsible").val(ApplicationUser.Employee.Id); }
            if($("#CmbCalibrationInternalResponsible").val()===null) { $("#CmbCalibrationInternalResponsible").val(ApplicationUser.Employee.Id); }
            if($("#CmbCalibrarionExternalResponsible").val()===null) { $("#CmbCalibrarionExternalResponsible").val(ApplicationUser.Employee.Id); }
            if($("#CmbVerificationInternalResponsible").val()===null) { $("#CmbVerificationInternalResponsible").val(ApplicationUser.Employee.Id); }
            if($("#CmbVerificationExternalResponsible").val()===null) { $("#CmbVerificationExternalResponsible").val(ApplicationUser.Employee.Id); }
            
            if(Equipment.ExternalCalibration!=null && typeof Equipment.ExternalCalibration !== "undefined" && Equipment.ExternalCalibration.Provider!==null && typeof Equipment.ExternalCalibration.Provider!=="undefined")
            {
                $("#CmbCalibrationExternalProvider").val(Equipment.ExternalCalibration.Provider.Id);
            }
            if(Equipment.ExternalVerification!=null && typeof Equipment.ExternalVerification !== "undefined" && Equipment.ExternalVerification.Provider!==null && typeof Equipment.ExternalVerification.Provider!=="undefined")
            {
                $("#CmbVerificationExternalProvider").val(Equipment.ExternalVerification.Provider.Id);
            }

            // Control de permisos
            if(typeof ApplicationUser.Grants.Equipment === "undefined" || ApplicationUser.Grants.Equipment.Write === false){
                document.getElementById("Contentholder1_status0").disabled = true;
                document.getElementById("Contentholder1_status1").disabled = true;
                document.getElementById("Contentholder1_status2").disabled = true;
                $("#BtnEquipmentChangeImage").hide();
                $("#BtnSave").hide();
                $("#BtnCalibrationInternalSave").hide();
                $("#BtnCalibrationExternalSave").hide();
                $("#BtnVerificationInternalSave").hide();
                $("#BtnVerificationExternalSave").hide();
                $("#BtnNewCalibration").hide();
                $("#BtnNewVerification").hide();
                $("#BtnNewMaintainment").hide();
                $("#BtnNewMaintainmentAct").hide();
                $("#EquipmentRepairNewBtn").hide();
                $("#BtnNewUploadfile").hide();
                $(".btn-danger").hide();
                $("#BtnCalibrationExternalProviderBAR").hide();
                $("#BtnVerificationExternalProviderBAR").hide();
                $("input").attr("disabled", true);
                $("textarea").attr("disabled", true);
                $("select").attr("disabled", true);
                
                $("#registros input").attr("disabled", false);
                $("#registros select").attr("disabled", false);
            }
        </script>
</asp:Content>