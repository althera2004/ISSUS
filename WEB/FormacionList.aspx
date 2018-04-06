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
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td id="TxtDateFromLabel"><%=this.Dictionary["Item_Learning_Filter_FromYear"] %>:</td>
										<td>
                                            <div class="col-xs-12 col-sm-12">
												<div class="input-group">
													<input class="form-control date-picker" style="width:100px;" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													<span class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();" id="BtnDateFrom">
														<i class="icon-calendar bigger-110"></i>
													</span>
												</div>
											</div>
											<span class="ErrorMessage" id="TxtDateFromErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
											<span class="ErrorMessage" id="TxtDateFromErrorDateRange" style="display:none;"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											<span class="ErrorMessage" id="TxtDateFromDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
										</td>
                                        <td id="TxtDateToLabel"><%=this.Dictionary["Item_Learning_Filter_ToYear"] %>:</td>
										<td>
                                            <div class="col-xs-12 col-sm-12">
												<div class="input-group">
													<input class="form-control date-picker" style="width:100px;" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													<span class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();" id="BtnDateTo">
														<i class="icon-calendar bigger-110"></i>
													</span>
												</div>
											</div>
											<span class="ErrorMessage" id="TxtDateToErrorRequired" style="display:none;"><%=this.Dictionary["Common_Required"] %></span>
											<span class="ErrorMessage" id="TxtDateToErrorDateRange" style="display:none;"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											<span class="ErrorMessage" id="TxtDateToDateMalformed" style="display:none;"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
										</td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status0" name="status" value="0" onclick="Go(2,0);" /><%=this.Dictionary["Item_Learning_Status_InProgress"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status1" name="status" value="1" onclick="Go(2,1);" /><%=this.Dictionary["Item_Learning_Status_Done"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status2" name="status" value="2" onclick="Go(2,2);" /><%=this.Dictionary["Item_Learning_Status_Evaluated"] %></td>
                                        <td>&nbsp;&nbsp;&nbsp;<input runat="server" type="radio" id="status3" name="status" value="3" onclick="Go(2,3);" /><%=this.Dictionary["Common_All_Female_Plural"] %></td>
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
                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="sort search" style="cursor:pointer;"><%=this.Dictionary["Item_Learning_ListHeader_Course"] %></th>
                                                        <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_DateComplete"] %></th>
                                                        <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_Status"] %></th>
                                                        <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_EstimatedDate"] %></th>
                                                        <th class="hidden-480" style="width:150px;"><%=this.Dictionary["Item_Learning_ListHeader_Cost"] %></th>
                                                        <th class="hidden-480" style="width:106px !important;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>                                            
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="LtLearningTable"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#333;" colspan="3"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="LtCount"></asp:Literal></i></th>
                                                        <th style="color:#333;width:100px;" align="right"><strong><%=this.Dictionary["Common_Total"] %>:&nbsp</strong></th>
                                                        <th style="color:#333;width:150px;" align="right"><strong><asp:Literal runat="server" ID="LtTotal"></asp:Literal></strong></th>
                                                        <th style="color:#333;width:107px !important;">&nbsp;</th>
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

