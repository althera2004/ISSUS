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
                                    <table style="width:100%;">
                                        <tr>
                                            <td style="width:250px;">
                                                <div class="row">
                                                    <label id="TxtDateFromLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentAction_List_Filter_From"] %></label>										
                                                    <div class="col-xs-9 col-sm-9 tooltip-info" id="TxtDateFromDiv">
                                                        <div class="input-group">
                                                            <input class="form-control date-picker" id="TxtDateFrom" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                            <span id="TxtDateFromBtn" class="input-group-addon" onclick="document.getElementById('TxtDateFrom').focus();">
                                                                <i class="icon-calendar bigger-110"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Status"] %>:</strong></td>
                                            <td><input type="checkbox" id="chkStatus1" checked="checked" />&nbsp;<%=this.Dictionary["Item_IndicentAction_Status1"] %></td>
                                            <td><input type="checkbox" id="chkStatus2" checked="checked" />&nbsp;<%=this.Dictionary["Item_IndicentAction_Status2"] %></td>
                                            <td><input type="checkbox" id="chkStatus3" checked="checked" />&nbsp;<%=this.Dictionary["Item_IndicentAction_Status3"] %></td>
                                            <td><input type="checkbox" id="chkStatus4"/>&nbsp;<%=this.Dictionary["Item_IndicentAction_Status4"] %></td>                                        
                                            </tr>
                                        <tr>
                                            <td style="width:250px;">
                                                <div class="row">
                                                    <label id="TxtDateToLabel" class="col-sm-3 control-label no-padding-right"><%=this.Dictionary["Item_IncidentAction_List_Filter_To"] %></label>										
                                                    <div class="col-xs-9 col-sm-9 tooltip-info" id="TxtDateToDiv">
                                                        <div class="input-group">
                                                            <input class="form-control date-picker" id="TxtDateTo" type="text" data-date-format="dd/mm/yyyy" maxlength="10" />
                                                            <span id="TxtDateToBtn" class="input-group-addon" onclick="document.getElementById('TxtDateTo').focus();">
                                                                <i class="icon-calendar bigger-110"></i>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </td>
                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Type"] %>:</strong></td>
                                            <td><input type="checkbox" id="RType1" name="Rtype" checked="checked" /><%=this.Dictionary["Item_IncidentAction_Type1"]%></td>
                                            <td><input type="checkbox" id="RType2" name="Rtype" checked="checked" /><%=this.Dictionary["Item_IncidentAction_Type2"]%></td>
                                            <td><input type="checkbox" id="RType3" name="Rtype" checked="checked" /><%=this.Dictionary["Item_IncidentAction_Type3"]%></td>
                                            <td>&nbsp;</td>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td style="width:100px;padding-left:20px;"><strong><%=this.Dictionary["Item_IncidentAction_List_Filter_Source"] %>:</strong></td>
                                            <td colspan="4">
                                                <select id="CmbOrigin">
                                                    <option value="-1"><%=this.Dictionary["Common_All_Female_Plural"] %></option>
                                                    <option value="1"><%=this.Dictionary["Item_IncidentAction_Origin1"] %></option>
                                                    <option value="2"><%=this.Dictionary["Item_IncidentAction_Origin2"] %></option>
                                                    <option value="3"><%=this.Dictionary["Item_IncidentAction_Origin3"] %></option>
                                                    <option value="4"><%=this.Dictionary["Item_IncidentAction_Origin4"] %></option>
                                                </select>  
                                            </td>
                                            <td>
                                                <button class="btn btn-success" type="button" id="BtnSearch"><i class="icon-filter bigger-110"></i><%= this.Dictionary["Common_Filter"] %></button>
                                                <button class="btn btn-success" type="button" id="BtnRecordShowAll"><i class="icon-list bigger-110"></i><%= this.Dictionary["Common_All_Male_Plural"] %></button>
                                                <button class="btn btn-success" type="button" id="BtnRecordShowNone" style="display: none;"><i class="icon-remove-circle bigger-110"></i><%= this.Dictionary["Common_None_Male"] %></button>
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
                                                        <!-- <th id="th0" class="search"><%=this.Dictionary["Item_IncidentAction_Header_Description"] %></th> -->
														<th onclick="Sort(this,'ListDataTable','text',false);" id="th0" class="sort search"><%=this.Dictionary["Item_IncidentAction_Header_Description"] %></th>
														<th onclick="Sort(this,'ListDataTable','text',false);" id="th1" class="sort search" style="width:100px;"><%=this.Dictionary["Item_IncidentAction_Header_Type"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','date',false);" id="th2" class="sort search" style="width:100px; text-align:center;"><%=this.Dictionary["Item_IncidentAction_Header_Open"] %></th>
                                                        <th id="th3" style="width:60px;"><%=this.Dictionary["Item_IncidentAction_Header_Status"] %></th>
                                                        <!-- <th id="th4" class="search sort" style="width:150px;"><%=this.Dictionary["Item_IncidentAction_Header_Origin"] %></th> -->
														<th onclick="Sort(this,'ListDataTable','text',false);" id="th4" class="sort search" style="width:150px;"><%=this.Dictionary["Item_IncidentAction_Header_Origin"] %></th>
														<th onclick="Sort(this,'ListDataTable','date',false);" id="th5" class="sort hidden-480 search" style="width:100px;"><%=this.Dictionary["Item_IncidentAction_Header_ImplementDate"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable','date',false);" id="th6" class="sort hidden-480 search" style="width:100px;text-align:center;"><%=this.Dictionary["Item_IncidentAction_Header_Close"] %></th>
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
                                                        <th style="color:#aaa;">
															<i>
																<%=this.Dictionary["Common_RegisterCount"] %>:
																&nbsp;
																<span id="NumberCosts"></span>
															</i>
														</th>
                                                        <th style="width:100px; font-weight:bold; text-align:right"><%=this.Dictionary["Common_Total"] %>:</th>
														<th style="width:100px;"><div id="TotalCosts" style="width:100%;text-align:right;font-weight:bold;"></div></th>
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

            /*$('.date-picker').datepicker({
                autoclose: true,
                todayHighlight: true,
                monthNamesShort: [ "Jan", "Feb", "222", "Apr", "Maj", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dec" ]
            });*/
            var options = $.extend({}, $.datepicker.regional["<%=this.UserLanguage %>"], { autoclose: true, todayHighlight: true });
            $(".date-picker").datepicker(options);

            $("#BtnSearch").on('click', function (e) {
                e.preventDefault();
                IncidentActionGetFilter(); 
            });

            $('#BtnRecordShowAll').on('click', function (e) {
                e.preventDefault();
                EquipmentRecordGetAll();
            });

            $('#BtnRecordShowNone').on('click', function (e){
                e.preventDefault();
                EquipmentRecordGetNone();
            });
        });

        function EquipmentRecordGetNone() {
            document.getElementById('BtnRecordShowAll').style.display = '';
            document.getElementById('BtnRecordShowNone').style.display = 'none';
            
            document.getElementById('TxtDateFrom').value = '';
            document.getElementById('TxtDateTo').value = '';
            document.getElementById('chkStatus1').checked = false;
            document.getElementById('chkStatus2').checked = false;
            document.getElementById('chkStatus3').checked = false;
            document.getElementById('chkStatus4').checked = false;
            document.getElementById('RType1').checked = false;
            document.getElementById('RType2').checked = false;
            document.getElementById('RType3').checked = false;
            document.getElementById('CmbOrigin').value = -1;
            VoidTable('ListDataTable');
        }

        function EquipmentRecordGetAll() {
            document.getElementById('BtnRecordShowAll').style.display = 'none';
            document.getElementById('BtnRecordShowNone').style.display = '';

            document.getElementById('TxtDateFrom').value = '';
            document.getElementById('TxtDateTo').value = '';
            document.getElementById('chkStatus1').checked = true;
            document.getElementById('chkStatus2').checked = true;
            document.getElementById('chkStatus3').checked = true;
            document.getElementById('chkStatus4').checked = true;
            document.getElementById('RType1').checked = true;
            document.getElementById('RType2').checked = true;
            document.getElementById('RType3').checked = true;
            document.getElementById('CmbOrigin').value = -1;
            IncidentActionGetFilter();
        }

        if(Filter!=null)
        {
            document.getElementById('TxtDateFrom').value = GetDateYYYYMMDDText(Filter.from,'/',false);
            document.getElementById('TxtDateTo').value = GetDateYYYYMMDDText(Filter.to,'/',false);
            document.getElementById('chkStatus1').checked = Filter.statusIdnetified;
            document.getElementById('chkStatus2').checked = Filter.statusAnalyzed;
            document.getElementById('chkStatus3').checked = Filter.statusInProgress;
            document.getElementById('chkStatus4').checked = Filter.statusClose;
            document.getElementById('RType1').checked = Filter.typeImprovement;
            document.getElementById('RType2').checked = Filter.typeFix;
            document.getElementById('RType3').checked = Filter.typePrevent;
            document.getElementById('CmbOrigin').value = Filter.origin;
            IncidentActionGetFilter();
        }

        IncidentActionGetFilter();
    </script>
</asp:Content>

