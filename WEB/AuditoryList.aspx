<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="AuditoryList.aspx.cs" Inherits="AuditoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv{
            background-color:#fafaff;
            border:1px solid #e0e0e0;
            border-top:none;
            display:block;
        }
        .truncate {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            padding:0;
            margin:0;
        }

        TR:first-child{border-left:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var userLanguage = "<%=this.ApplicationUser.Language %>";
        var Filter = <%=this.Filter %>;
        var AuditoryList = [];
        var AuditorySelectedId = null;
        var AuditorySelected = null;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-11">
                                <table>
                                    <tr>
                                        <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Periode1"] %>:</strong></td>
										<td>
                                            <div class="col-xs-12 col-sm-12" style="margin:0;padding:0;">
												<div class="input-group">
													<input class="form-control date-picker" style="width:85px;" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													<span class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();" id="TxtDateFromBtn">
														<i class="icon-calendar bigger-110"></i>
													</span>
												</div>
											    <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											    <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											    <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                            </div>
										</td>
                                        <td>&nbsp;-&nbsp;</td>
                                        <td>
                                            <div class="col-xs-12 col-sm-12" style="margin:0;padding:0;">
												<div class="input-group">
													<input class="form-control date-picker" style="width:85px;" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													<span class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();" id="TxtDateToBtn">
														<i class="icon-calendar bigger-110"></i>
													</span>
												</div>
											    <span class="ErrorMessage" id="TxtDateToErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											    <span class="ErrorMessage" id="TxtDateToErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											    <span class="ErrorMessage" id="TxtDateToDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                            </div>
										</td>
                                        <td rowspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td><strong><%=this.Dictionary["Item_Auditory_Filter_Type"] %>:</strong></td>
                                        <td>
                                            <input type="checkbox" id="ChkType0" onclick="ChkTypeChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Type_Label_0"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkType1" onclick="ChkTypeChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Type_Label_1"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkType2" onclick="ChkTypeChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Type_Label_2"] %>
                                        </td>
                                    </tr>
                                    <tr style="height:30px;">
										<td colspan="4"></td>
                                        <td><strong><%=this.Dictionary["Item_Auditory_Filter_Status"] %>&nbsp;<i class="icon-question-sign" style="color:#77f;cursor:help;" onclick="ShowStatusHelp();">:</strong></td>
                                        <td>
                                            <input type="checkbox" id="ChkStatus0" onclick="ChkStatusChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_0"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkStatus1" onclick="ChkStatusChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_1"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkStatus2" onclick="ChkStatusChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_2"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkStatus3" onclick="ChkStatusChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_3"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkStatus4" onclick="ChkStatusChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_4"] %>&nbsp;&nbsp;
                                            <input type="checkbox" id="ChkStatus5" onclick="ChkStatusChanged();" />&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_5"] %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-xs-1">
                                <button class="btn btn-success" style="width:100px;display:none;" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%= this.Dictionary["Common_Filter"] %></button>
                                <button class="btn btn-success btn-filter" type="button" id="BtnRecordShowAll" title="<%= this.Dictionary["Common_All_Male_Plural"] %>"><i class="icon-refresh"></i></button>
                            </div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="sort search" style="width:45px;"></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th1" class="sort search"><%=this.Dictionary["Item_Auditory_ListHeader_Name"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th2" class="sort search" style="width:100px;"><%=this.Dictionary["Item_Auditory_ListHeader_Planned"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th3" class="sort search" style="width:100px;"><%=this.Dictionary["Item_Auditory_ListHeader_Closed"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th4" class="sort search totalizable" style="width:150px;"><%=this.Dictionary["Item_Auditory_ListHeader_Ammount"] %></th>
                                                        <th style="width:107px;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow:scroll;overflow-x:hidden;padding:0;">
                                                <table class="table table-bordered table-striped" style="border-top:none;">  
                                                    <tbody id="ListDataTable"><tr><td><%=this.Dictionary["Common_Loading"] %>...</td></tr></tbody>
                                                </table>
                                            </div>
                                            <div id="NoData" style="display:none;width:100%;height:99%;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th class="thin-border-bottom">
                                                            <%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalList" style="font-weight:bold;"></span>
                                                            <span style="float:right"><%=this.Dictionary["Common_Total"] %></span>
                                                        </th>
                                                        <th style="width:150px;text-align:right;"><strong><span id="TotalAmount"></span></strong></th>
                                                        <th style="width:107px;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->	  
                            <div id="AuditoryDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_PopupDelete_Message"] %>&nbsp;<strong><span id="AuditoryDeleteName"></span></strong>?</p>
                            </div>                          
                            <div id="QuestionaryDuplicateDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Questionary_PopupDuplicate_Message"] %>&nbsp;<strong><span id="QuestionaryToDuplicateName"></span></strong>?</p>
                                <div class="row">
                                    <label class="col col-xs-2">Nombre<span style="color:#f00;">*</span>:</label>
                                    <div class="col col-xs-10">
                                        <input class="col col-xs-12" type="text" id="QuestionaryNewDescription" onblur="this.value = this.value.trim();" />
                                        <span id="QuestionaryNewDescriptionErrorRequired" class="ErrorMessage"><%= this.Dictionary["Common_Required"] %></span>
                                        <span id="QuestionaryNewDescriptionErrorDuplicated" class="ErrorMessage"><%= this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
                            </div>                        
                            <div id="StatusHelpDialog" class="hide" style="width:500px;">
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td style="font-weight:bold;vertical-align:top;"><i class="icon-circle bigger-110" id="StatusIcon0"></i>&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_0"] %>:</td>
                                        <td><%=this.Dictionary["Item_Adutory_Status_Explanation_0"] %></td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight:bold;vertical-align:top;"><i class="icon-circle bigger-110" id="StatusIcon1"></i>&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_1"] %>:</td>
                                        <td><%=this.Dictionary["Item_Adutory_Status_Explanation_1"] %></td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight:bold;vertical-align:top;"><i class="icon-circle bigger-110" id="StatusIcon2"></i>&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_2"] %>:</td>
                                        <td><%=this.Dictionary["Item_Adutory_Status_Explanation_2"] %></td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight:bold;vertical-align:top;"><i class="icon-circle bigger-110" id="StatusIcon3"></i>&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_3"] %>:</td>
                                        <td><%=this.Dictionary["Item_Adutory_Status_Explanation_3"] %></td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight:bold;vertical-align:top;"><i class="icon-circle bigger-110" id="StatusIcon4"></i>&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_4"] %>:</td>
                                        <td><%=this.Dictionary["Item_Adutory_Status_Explanation_4"] %></td>
                                    </tr>
                                    <tr>
                                        <td style="font-weight:bold;vertical-align:top;"><i class="icon-circle bigger-110" id="StatusIcon5"></i>&nbsp;<%=this.Dictionary["Item_Adutory_Status_Label_5"] %>:</td>
                                        <td><%=this.Dictionary["Item_Adutory_Status_Explanation_5"] %></td>
                                    </tr>
                                </table>
                            </div>                      
                            <div id="SelectDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_SelectType_Title"] %></p>
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td><input type="radio" name="AuditoryTypeSelect" id="AuditoryTypeSelect0" value="0" />&nbsp;<%=this.Dictionary["Item_Adutory_Type_Label_0"] %></td>
                                    </tr>
                                    <tr>
                                        <td><input type="radio" name="AuditoryTypeSelect" id="AuditoryTypeSelect1" value="1" />&nbsp;<%=this.Dictionary["Item_Adutory_Type_Label_1"] %></td>
                                    </tr>
                                    <tr>
                                        <td><input type="radio" name="AuditoryTypeSelect" id="AuditoryTypeSelect2" value="2" />&nbsp;<%=this.Dictionary["Item_Adutory_Type_Label_2"] %></td>
                                    </tr>
                                    <tr id="ErrorSelect" style="display:none;color:#f00;">
                                        <td><%=this.Dictionary["Item_Auditory_SelectType_Error"] %></td>
                                    </tr>
                                </table>
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
        <script type="text/javascript" src="/js/AuditoryList.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript">
            $(document).ready(function() {
                console.log("document is ready");
                var containerHeight = $(window).height();
                $("#ListDataDiv").height(containerHeight - 410);
                $("#NoData").height(containerHeight - 410);
            });
        </script>
</asp:Content>

