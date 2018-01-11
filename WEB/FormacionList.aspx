﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="FormacionList.aspx.cs" Inherits="FormacionList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .tags
        {
            display:block !important;
            width:100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td>
                                            <%=this.Dictionary["Item_Learning_Filter_FromYear"] %>:
                                            <select onchange="Go(0, this.value);" id="CmbYearFrom">
                                                <asp:Literal runat="server" ID="LtYearFrom"></asp:Literal>
                                            </select>
                                        </td>
                                        <td>
                                            <%=this.Dictionary["Item_Learning_Filter_ToYear"] %>:
                                            <select onchange="Go(1, this.value);" id="CmbYearTo">
                                                <asp:Literal runat="server" ID="LtYearTo"></asp:Literal>
                                            </select>
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
                                        <div class="table-responsive">
                                            <table class="table table-bordered table-striped">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeader">
                                                                <th onclick="Sort(this,'ListDataTable');" id="th0" class="sort search" style="width:200px;cursor:pointer;"><%=this.Dictionary["Item_Learning_ListHeader_Course"] %></th>
                                                                <th><%=this.Dictionary["Item_Learning_ListHeader_Assistants"] %></th>
                                                                <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_EstimatedDate"] %></th>
                                                                <th class="hidden-480" style="width:100px;"><%=this.Dictionary["Item_Learning_ListHeader_Cost"] %></th>
                                                                <th class="hidden-480" style="width:90px !important;">&nbsp;</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="ListDataTable">
                                                            <asp:Literal runat="server" ID="LtLearningTable"></asp:Literal>
                                                        </tbody>
                                                        <tfoot class="thin-border-bottom">
                                                            <tr id="ListDataFooter">
                                                                <td style="color:#333;" colspan="2"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="LtCount"></asp:Literal></i></td>
                                                                <td style="color:#333;" align="right"><strong><%=this.Dictionary["Common_Total"] %>:&nbsp</strong></td>
                                                                <td style="color:#333;" align="right"><strong><asp:Literal runat="server" ID="LtTotal"></asp:Literal></strong></td>
                                                                <td style="color:#333;"></td>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                            <br />
                                            <br />
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
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/FormacionList.js?ac=<%=this.AntiCache %>"></script>
</asp:Content>

