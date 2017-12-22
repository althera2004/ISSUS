<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="Documents.aspx.cs" Inherits="Documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #scrollTableDiv, #scrollTableDiv2{
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
        var documents = <%=this.DocumentsJson %>;
        var Filter = "<%=this.Filter %>";
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row" style="padding-bottom:8px;" id="SelectRow">
                                    <div class="col-xs-12">
                                        <div class="col-xs-3">
                                            <input type="checkbox" id="Chk1" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["Common_Active_Plural"] %>
                                        </div>
                                        <div class="col-xs-3">
                                            <input type="checkbox" id="Chk2" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["Common_Inactive_Plural"] %>
                                        </div>
                                    </div>
                                </div>
                                <div class="tabbable">
                                    <!--<ul class="nav nav-tabs padding-18">
                                        <li class="active">
                                            <a data-toggle="tab" href="#active" onclick="document.getElementById('BtnNewItem').parentNode.style.visibility='visible';"><%=this.Dictionary["Item_Document_Status_ActivePlural"] %></a>
                                        </li>
                                        <li class="">
                                            <a data-toggle="tab" href="#inactive" onclick="document.getElementById('BtnNewItem').parentNode.style.visibility='hidden';"><%=this.Dictionary["Item_Document_Status_InactivePlural"] %></a>
                                        </li>
                                    </ul>-->
                                    <div class="tab-content no-border padding-24" style="height:500px;">
                                        <div id="active" class="tab-pane active">  
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead>
                                                                <tr id="ListDataHeader">
			                                                        <th id="th0" onclick="Sort(this,'ListDataTable','link');" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_Document_ListHeader_Name"] %></th>
			                                                        <th id="th1" onclick="Sort(this,'ListDataTable','text');" class="search sort" style="width:110px;"><%=this.Dictionary["Item_Document_ListHeader_Code"] %></th>
			                                                        <th id="th2" onclick="Sort(this,'ListDataTable','money');" class="sort" style="width:110px;"><%=this.Dictionary["Item_Document_ListHeader_Revision"] %></th>
			                                                        <th style="width:106px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTable">
                                                                    <asp:Literal runat="server" ID="LtDocumentsActive"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="TotalRecords"></span></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div><!-- /.table-responsive -->
                                                </div><!-- /span -->
                                            </div><!-- /row -->	
                                        </div>
                                    </div>
                                </div>                                                                            
                            </div><!-- /.col -->
                            
                            <div id="DocumentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Document_PopupDelete_Message"] %>&nbsp;<strong><span id="DocumentName"></span></strong>?</p>
                                <span id="TxtNewReasonLabel"><%=this.Dictionary["Item_Document_PopupDelete_Message"]%><br /></span>
                                <textarea id="TxtNewReason" cols="40" rows="3"></textarea>
                                <span class="ErrorMessage" id="TxtNewReasonErrorRequired" style="display:none;"> <%=this.Dictionary["Item_Document_Error_DeleteReasonRequired"] %></span>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server"> 
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/js/common.js?ac<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/DocumentList.js?ac<%=this.AntiCache %>"></script>
</asp:Content>