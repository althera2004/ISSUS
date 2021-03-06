﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="ProcesosList.aspx.cs" Inherits="ProcesosList" %>

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
            var filter = "<%=this.Filter%>";
        </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!--<div class="row" style="padding-bottom:8px;" id="SelectRow">
                                    <div class="col-xs-12">
                                        <div class="col-xs-2">
                                            <input type="checkbox" id="Chk1" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["Common_Status_ActivePlural"] %>
                                        </div>
                                        <div class="col-xs-3">
                                            <input type="checkbox" id="Chk2" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["Common_Status_InactivePlural"] %>
                                        </div>
                                    </div>
                                </div>-->
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="sort search"><%=this.Dictionary["Item_Process_ListHeader_Name"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th1" class="sort search" style="width:300px;" ><%=this.Dictionary["Item_Process_ListHeader_Responsible"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th2" class="sort" style="width:200px; text-align:center;"><%=this.Dictionary["Item_Process_ListHeader_ProcessType"] %></th>
                                                        <th style="width:107px;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow:scroll;overflow-x:hidden;padding:0;">
                                                <div id="NoData" style="display:none;width:100%;height:99%;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>                                            
                                                <table id="TableData" class="table table-bordered table-striped" style="border-top:none;">                                                        
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="ProcesosData"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <td class="thfooter">
                                                            <%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalList" style="font-weight:bold;"><asp:Literal runat="server" ID="ProcesosDataTotal"></asp:Literal></span>
                                                        </td>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->								
                            </div><!-- /.col -->                            
                            <div id="ProcessDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Process_PopupDelete_Message"] %>&nbsp;<strong><span id="ProcessName"></span></strong>?</p>
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
        <script type="text/javascript" src="/js/ProcesosList.js?<%=this.AntiCache %>"></script>
</asp:Content>