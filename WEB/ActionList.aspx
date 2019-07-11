<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ActionList.aspx.cs" Inherits="ActionList" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="col-sm-12">
                                    <table cellpadding="2" cellspacing="2" style="width:100%;">
                                        <tr>
                                            <td id="TxtDateFromLabel"><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Periode1"] %>:</strong></td>
										    <td>
                                                <div class="col-xs-12 col-sm-12">
												    <div class="input-group">
													    <input class="form-control date-picker" style="width:100px;" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													    <span class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();" id="TxtDateFromBtn">
														    <i class="icon-calendar bigger-110"></i>
													    </span>
												    </div>
											        <span class="ErrorMessage" id="TxtDateFromErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											        <span class="ErrorMessage" id="TxtDateFromErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											        <span class="ErrorMessage" id="TxtDateFromDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                </div>
										    </td>
                                            <td id="TxtDateToLabel"><%=this.Dictionary["Item_IncidentAction_List_Filter_Periode2"] %></td>
										    <td>
                                                <div class="col-xs-12 col-sm-12">
												    <div class="input-group">
													    <input class="form-control date-picker" style="width:100px;" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
													    <span class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();" id="TxtDateToBtn">
														    <i class="icon-calendar bigger-110"></i>
													    </span>
												    </div>
											        <span class="ErrorMessage" id="TxtDateToErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
											        <span class="ErrorMessage" id="TxtDateToErrorDateRange"><%=this.Dictionary["Item_Learning_ErrorMessage_UntemporalyDates"] %></span>
											        <span class="ErrorMessage" id="TxtDateToDateMalformed"><%=this.Dictionary["Common_Error_DateMalformed"] %></span>
                                                </div>
										    </td>
                                            <td><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Source"] %>:</strong></td>
                                            <td>
                                                <div class="col-xs-12 col-sm-12">
                                                    <select id="CmbOrigin" style="width:200px;">
                                                        <option value="-1"><%=this.Dictionary["Common_All_Female_Plural"] %></option>
                                                        <option value="1"><%=this.Dictionary["Item_IncidentAction_Origin1"] %></option>
                                                        <option value="2"><%=this.Dictionary["Item_IncidentAction_Origin2"] %></option>
                                                        <option value="3"><%=this.Dictionary["Item_IncidentAction_Origin3"] %></option>
                                                        <option value="4"><%=this.Dictionary["Item_IncidentAction_Origin46"] %></option>
                                                        <option value="5"><%=this.Dictionary["Item_IncidentAction_Origin5"] %></option>
                                                    </select>
                                                </div>
                                            </td>

                                            <td><strong><%=this.Dictionary["Item_IncidentAction_Label_Reporter"] %>:</strong></td>
                                            <td>
                                                <div class="col-xs-12 col-sm-12">
                                                    <select id="CmbReporter">
                                                        <option value="0"><%=this.Dictionary["Common_All_Male_Plural"] %></option>
                                                        <option value="1"><%=this.Dictionary["Item_IncidentAction_ReporterType1"] %></option>
                                                        <option value="2"><%=this.Dictionary["Item_IncidentAction_ReporterType2"] %></option>
                                                        <option value="3"><%=this.Dictionary["Item_IncidentAction_ReporterType3"] %></option>
                                                    </select>
                                                </div>
                                            </td>
                                            
                                        </tr>
                                        <tr>

                                            <td><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Type"] %>:</strong></td>
                                            <td colspan="3">
                                                <input type="checkbox" id="RType1" name="Rtype" checked="checked" /><%=this.Dictionary["Item_IncidentAction_Type1"]%>
                                                &nbsp;&nbsp;&nbsp;
                                                <input type="checkbox" id="RType2" name="Rtype" checked="checked" /><%=this.Dictionary["Item_IncidentAction_Type2"]%>
                                                &nbsp;&nbsp;&nbsp;
                                                <input type="checkbox" id="RType3" name="Rtype" checked="checked" /><%=this.Dictionary["Item_IncidentAction_Type3"]%>
                                            </td>
                                            <td><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Status"] %>:</strong></td>
                                            <td colspan="2">
                                                <input type="checkbox" id="chkStatus1" checked="checked" />&nbsp;<%=this.Dictionary["Item_IndicentAction_Status1"] %>
                                                &nbsp;&nbsp;&nbsp;
                                                <input type="checkbox" id="chkStatus2" checked="checked" />&nbsp;<%=this.Dictionary["Item_IndicentAction_Status2"] %>
                                                &nbsp;&nbsp;&nbsp;
                                                <input type="checkbox" id="chkStatus3" checked="checked" />&nbsp;<%=this.Dictionary["Item_IndicentAction_Status3"] %>
                                                &nbsp;&nbsp;&nbsp;
                                                <input type="checkbox" id="chkStatus4"/>&nbsp;<%=this.Dictionary["Item_IndicentAction_Status4"] %>
                                            </td>
                                            <td style="text-align:right;">
                                                <!--div class="col-xs-12 col-sm-12"-->                                      
                                                    <!--<button class="btn btn-success" style="width:100px;display:none;" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%= this.Dictionary["Common_Filter"] %></button>-->
                                                    <button class="btn btn-filter" type="button" id="BtnRecordShowAll" title="<%= this.Dictionary["Common_All_Male_Plural"] %>"><i class="icon-list"></i></button>
                                                    <!--<button class="btn btn-success" type="button" id="BtnRecordShowNone" title="<%= this.Dictionary["Common_None_Male"] %>"><i class="icon-remove-circle"></i></button>-->
                                                <!--/div-->
                                            </td>
                                        </tr>
                                    </table>
                                </div> 
                                <div style="height:8px;clear:both;"></div>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th id="th1" style="width:65px;"><%=this.Dictionary["Item_IncidentAction_Header_Status"] %></th>
														<th onclick="Sort(this,'ListDataTable','date',false);" id="th1" class="sort" style="width:100px; text-align:center;"><%=this.Dictionary["Item_IncidentAction_Header_Open"] %></th>
														<th onclick="Sort(this,'ListDataTable','text',false);" id="th2" class="sort search"><%=this.Dictionary["Item_IncidentAction_Header_Description"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','text',false);" id="th3" class="sort" style="width:250px;"><%=this.Dictionary["Item_IncidentAction_Header_Origin"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','text',false);" id="th4" class="sort" style="width:100px;"><%=this.Dictionary["Item_IncidentAction_Header_Type"] %></th>
														<th onclick="Sort(this,'ListDataTable','date',false);" id="th5" class="sort hidden-480" style="width:100px;"><%=this.Dictionary["Item_IncidentAction_Header_ImplementDate"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','money',false);" id="th6" class="sort hidden-480 totalizable" style="width:100px; text-align:center"><%=this.Dictionary["Item_IncidentAction_Header_Cost"] %></th>
                                                        <th class="hidden-480" style="width:107px !important;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable"></tbody>
                                                </table>
                                                <table id="ItemTableError" style="display:none;width:100%;">
                                                    <tr>
                                                        <td colspan="10" style="color:#aa0000;text-align:center;">
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
                                                <div id="NoData" style="display:none;width:100%;height:99%;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>                                            
                                            </div>                                                                                       
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <td class="thfooter">
                                                            <%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalList" style="font-weight:bold;"></span>
                                                            <span style="float:right"><%=this.Dictionary["Common_Total"] %></span>
														</td>
                                                        <th class="thfooter" style="width:100px;"><div id="TotalAmount" style="width:100%;text-align:right;font-weight:bold;"></div></th>
                                                        <th style="width:107px;"></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->						
                            </div><!-- /.col -->
                            <br /><br />
                            <div id="IncidentActionDeleteDialog" class="hide" style="width:500px;">
                                <p>&nbsp;<strong><span id="IncidentActionDeleteName"></span></strong>?</p>
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
        <script type="text/javascript" src="/js/ActionsList.js?ac=<%=this.AntiCache %>"></script>
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

            $("#BtnSearch").on("click", function (e) {
                e.preventDefault();
                IncidentActionGetFilter(); 
            });

            $("#BtnRecordShowAll").on("click", function (e) {
                e.preventDefault();
                IncidentActionGetAll();
            });

            $("#BtnRecordShowNone").on("click", function (e){
                e.preventDefault();
                IncidentActionGetNone();
            });
        });

        function IncidentActionGetNone() {
            $("#BtnRecordShowAll").show();
            $("#BtnRecordShowNone").hide();

            $("TxtDateFrom").val("");
            $("TxtDateTo").val("");
            document.getElementById("chkStatus1").checked = false;
            document.getElementById("chkStatus2").checked = false;
            document.getElementById("chkStatus3").checked = false;
            document.getElementById("chkStatus4").checked = false;
            document.getElementById("RType1").checked = false;
            document.getElementById("RType2").checked = false;
            document.getElementById("RType3").checked = false;
            $("CmbOrigin").val(-1);
            $("CmbReporter").val(0);
            VoidTable("ListDataTable");
        }

        function IncidentActionGetAll() {
            $("TxtDateFrom").val("");
            $("TxtDateTo").val("");
            document.getElementById("chkStatus1").checked = true;
            document.getElementById("chkStatus2").checked = true;
            document.getElementById("chkStatus3").checked = true;
            document.getElementById("chkStatus4").checked = true;
            document.getElementById("RType1").checked = true;
            document.getElementById("RType2").checked = true;
            document.getElementById("RType3").checked = true;
            $("CmbOrigin").val(-1);
            $("CmbReporter").val(0);
            IncidentActionGetFilter();
        }

        if (Filter != null) {
            $("#TxtDateFrom").val(GetDateYYYYMMDDText(Filter.from, "/", false));
            $("#TxtDateTo").val(GetDateYYYYMMDDText(Filter.to, "/", false));
            document.getElementById("chkStatus1").checked = Filter.statusIdnetified;
            document.getElementById("chkStatus2").checked = Filter.statusAnalyzed;
            document.getElementById("chkStatus3").checked = Filter.statusInProgress;
            document.getElementById("chkStatus4").checked = Filter.statusClose;
            document.getElementById("RType1").checked = Filter.typeImprovement;
            document.getElementById("RType2").checked = Filter.typeFix;
            document.getElementById("RType3").checked = Filter.typePrevent;
            $("#CmbOrigin").val(Filter.origin);
            IncidentActionGetFilter();
        }

        IncidentActionGetFilter();
    </script>
</asp:Content>