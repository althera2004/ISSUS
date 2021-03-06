﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ObjetivoList.aspx.cs" Inherits="ObjetivoList" %>

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
        var Filter = <%=this.Filter %>;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">

                                <div class="col-sm-11">
                                    <table>
                                        <tr>
                                            <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_ObjetivoAction_List_Filter_Periode1"] %>:</strong></td>
										    <td style="width:120px;">
                                                <div class="col-xs-12 col-sm-12">
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
										    <td style="width:120px;">
                                                <div class="col-xs-12 col-sm-12">
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

                                            <td><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Status"] %>:</strong></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" name="RBStatus" id="RBStatus1" /><%= this.Dictionary["Item_ObjetivoAction_List_Filter_ShowActive"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" name="RBStatus" id="RBStatus2" /><%= this.Dictionary["Item_ObjetivoAction_List_Filter_ShowClosed"] %></td>
											<!--<td>&nbsp;&nbsp;&nbsp;<input type="radio" name="RBStatus" id="RBStatus0" /><%= this.Dictionary["Common_All"] %></td>-->                                            
                                        </tr>
                                    </table>
                                </div> 
                                <div class="col-xs-1">
                                    <button class="btn btn-success btn-filter" type="button" id="BtnRecordShowAll" title="<%= this.Dictionary["Common_All_Male_Plural"] %>"><i class="icon-refresh"></i></button>
                                </div>
                                <div style="height:8px;clear:both;"></div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
		                                            <tr id="ListDataHeader">
			                                            <th onclick="Sort(this,'ListDataTable','text',false);" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_Objetivo_Header_Name"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable','date',false);" id="th1" class="search sort" style="cursor:pointer;width:100px;"><%=this.Dictionary["Item_Objetivo_Header_StartDate"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable','date',false);" id="th2" class="search sort" style="cursor:pointer;width:100px;"><%=this.Dictionary["Item_Objetivo_Header_PreviewEndDate"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','text',false);" id="th3" class="search sort" style="cursor:pointer;width:200px;"><%=this.Dictionary["Item_Objetivo_Header_Responsible"] %></th>
			                                            <th style="width:107px;">&nbsp;</th>
		                                            </tr>
	                                            </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow:scroll;overflow-x:hidden;padding:0;">
                                                <table class="table table-bordered table-striped" style="border-top:none;">                                                        
                                                    <tbody id="ListDataTable"></tbody>
                                                </table>
                                                <table id="ItemTableError" style="display:none;width:100%;">
                                                    <tr>
                                                        <td colspan="10" align="center" style="color:#aa0000;">
                                                            <table style="border:none;width:100%;">
                                                                <tr>
                                                                    <td rowspan="2" style="border:none;text-align:right;"><i class="icon-warning-sign" style="font-size:48px;"></i></td>        
                                                                    <td style="border:none;">
                                                                        <h4><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorRequired"] %></h4>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="border:none;">
                                                                        <ul>
                                                                            <li id="ErrorDate"><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorDate"] %></li>
                                                                            <li id="ErrorStatus"><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorStatus"] %></li>
                                                                            <li id="ErrorType"><%=this.Dictionary["Item_IncidentAction_List_Filter_ErrorType"] %></li>
                                                                        </ul>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="ItemTableVoid" style="display:none;width:100%">
                                                    <tr>
                                                        <td colspan="10" align="center" style="color:#0000aa;">
                                                            <table style="border:none;width:100%">
                                                                <tr>
                                                                    <td rowspan="2" style="border:none;text-align:right;"><i class="icon-info-sign" style="font-size:48px;"></i></td>        
                                                                    <td style="border:none;">
                                                                        <h4><%=this.Dictionary["Common_VoidSearchResult"] %></h4>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>                                                                                       
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <td class="thfooter">
                                                            <%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalList" style="font-weight:bold;"></span>
														</td>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->	
                            <div id="ObjetivoDeleteDialog" class="hide" style="width:500px;">
                                <p>&nbsp;<strong><span id="ObjetivoDeleteName"></span></strong>?</p>
                                <div class="alert alert-danger"><%=this.Dictionary["Item_Incident_PopupDelete_Message_Actions"] %></div>
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
        <script type="text/javascript" src="/js/ObjetivoList.js?ac=<%=this.AntiCache %>"></script>
    <script type="text/javascript">

        jQuery(function ($) {

            $.widget("ui.dialog", $.extend({}, $.ui.dialog.prototype, {
                _title: function (title) {
                    var $title = this.options.title || '&nbsp;'
                    if (("title_html" in this.options) && this.options.title_html == true)
                        title.html($title);
                    else title.text($title);
                }
            }));

            var options = $.extend({}, $.datepicker.regional["<%=this.UserLanguage %>"], { autoclose: true, todayHighlight: true });
            $(".date-picker").datepicker(options);
            $(".date-picker").on("blur", function () { DatePickerChanged(this); });

            $("#BtnSearch").on("click", function (e) {
                e.preventDefault();
                ObjetivoGetFilter(); 
            });

            $("#BtnRecordShowAll").on("click", function (e) {
                e.preventDefault();
                ObjetivoGetAll();
            });

            $("#BtnRecordShowNone").on("click", function (e){
                e.preventDefault();
                ObjetivoGetNone();
            });
        });
    </script>
</asp:Content>

