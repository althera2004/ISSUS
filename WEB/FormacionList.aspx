<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="FormacionList.aspx.cs" Inherits="FormacionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" runat="server">
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
	<script>
		var userLanguage = "<%=this.UserLanguage %>";
		var dateFrom = "<%=this.DateFrom%>";
        var dateTo = "<%=this.DateTo%>";
        var learningData = [<%=this.LeargingData %>];
        var Filter = "<%=this.LearningFilterData %>";
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <table cellpadding="2" cellspacing="2" style="width:100%;">
                                    <tr>
                                        <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_Learning_Filter_Periode1"] %>:</strong></td>
										<td>
                                            <div class="col-xs-12 col-sm-12">
												<div class="input-group">
													<input class="form-control date-picker" style="width:100px;" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													<span class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();" id="BtnDateFrom">
														<i class="icon-calendar bigger-110"></i>
													</span>
												</div>
											    <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											    <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											    <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                            </div>
										</td>
                                        <td id="TxtDateToLabel"><%=this.Dictionary["Item_Learning_Filter_Periode2"] %></td>
										<td>
                                            <div class="col-xs-12 col-sm-12">
												<div class="input-group">
													<input class="form-control date-picker" style="width:100px;" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													<span class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();" id="BtnDateTo">
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
                                        <td><strong><%=this.Dictionary["Item_Learning_FieldLabel_Status"] %>:</strong></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="status0" onclick="Go();" /><%=this.Dictionary["Item_Learning_Status_InProgress"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="status1" onclick="Go();" /><%=this.Dictionary["Item_Learning_Status_Started"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="status2" onclick="Go();" /><%=this.Dictionary["Item_Learning_Status_Done"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="status3" onclick="Go();" /><%=this.Dictionary["Item_Learning_Status_Evaluated"] %></td>
                                        <!--<td>&nbsp;&nbsp;&nbsp;<input runat="server" type="checkbox" id="status4" name="status" value="-1" onclick="Go();" /><%=this.Dictionary["Common_All_Female_Plural"] %></td>-->
                                        
                                        <td style="text-align:right;">
                                            <button class="btn btn-success btn-filter" type="button" id="BtnRestoreFilter" onclick="RestoreFilter();" title="<%= this.Dictionary["Common_All_Male_Plural"] %>"><i class="icon-refresh"></i></button>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div style="height:12px;clear:both;"></div>
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th onclick="Sort(this,'ListDataTable','text',false);" id="th0" class="sort search"><%=this.Dictionary["Item_Learning_ListHeader_Course"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','date',false);" id="th1" class="hidden-480 sort search" style="width:100px; text-align:center;"><%=this.Dictionary["Item_Learning_ListHeader_EstimatedDate"] %></th>
														<th onclick="Sort(this,'ListDataTable','date',false);" id="th2" class="hidden-480 sort search" style="width:100px; text-align:center;"><%=this.Dictionary["Item_Learning_ListHeader_DateComplete"] %></th>
														<th onclick="Sort(this,'ListDataTable','text',false);" id="th3" class="hidden-480 sort" style="width:100px; text-align:center;"><%=this.Dictionary["Item_Learning_ListHeader_Status"] %></th>
														<th onclick="Sort(this,'ListDataTable','money',false);" id="th4" class="hidden-480 sort totalizable" style="width:150px; text-align:center;"><%=this.Dictionary["Item_Learning_ListHeader_Cost"] %></th>
                                                        <th class="hidden-480" style="width:106px !important;">&nbsp;</th>
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
                            </div>
                            
                            <div id="LearningDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Learning_PopupDelete_Message"] %>&nbsp;<strong><span id="LearningName"></span></strong>?</p>
                                <!--<span id="TxtNewReasonLabel"><%=this.Dictionary["Item_Document_PopupDelete_Message"]%><br /></span>
                                <textarea id="TxtNewReason" cols="40" rows="3"></textarea>
                                <span class="ErrorMessage" id="TxtNewReasonErrorRequired" style="display:none;"> <%=this.Dictionary["Item_Learning_Error_DeleteReasonRequired"] %></span>-->
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
        <script type="text/javascript" src="/assets/js/autoNumeric.js"></script>
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/FormacionList.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>

