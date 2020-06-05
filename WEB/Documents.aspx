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
        var categories = <%=this.CategoriesJson %>;
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
                                    <table cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td><strong><%=this.Dictionary["Item_Document_FieldLabel_Status"] %>:</strong></td>
										    <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="Chk1" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["Common_Active_Plural"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;<input type="checkbox" id="Chk2" onchange="FilterChanged();" />&nbsp;<%=this.Dictionary["Common_Inactive_Plural"] %></td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td><strong><%=this.Dictionary["Item_Document_FieldLabel_Category"] %>:</strong></td>
										    <td>
                                                <select style="width:350px;" id="CmbCategory">
                                                    <option value="-1"><%=this.Dictionary["Common_All_Female_Plural"] %></option>
                                                </select>
										    </td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            <td><strong><%=this.Dictionary["Item_Document_FieldLabel_Origin"] %>:</strong></td>
                                            <td>
                                                <select style="width:200px" id="CmbOrigin">
                                                    <option value="-1"><%=this.Dictionary["Common_All"] %></option>
                                                    <option value="0"><%=this.Dictionary["Common_Internal"] %></option>
                                                    <option value="1"><%=this.Dictionary["Common_External"] %></option>
                                                </select>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br /><br />

                                <div class="tabbable">
                                    <div class="tab-content no-border " style="height:500px;">
                                        <div id="active" class="tab-pane active">  
                                            <div class="row">
                                                <div class="col-xs-12">
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead>
                                                                <tr id="ListDataHeader">
			                                                        <th id="th0" onclick="Sort(this,'ListDataTable','link');" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_Document_ListHeader_Name"] %></th>
			                                                        <th id="th1" onclick="Sort(this,'ListDataTable','text');" class="search sort" style="width:150px;"><%=this.Dictionary["Item_Document_ListHeader_Code"] %></th>
			                                                        <th id="th2" onclick="Sort(this,'ListDataTable','text');" class="sort" style="width:200px;"><%=this.Dictionary["Item_Document_ListHeader_Category"] %></th>
			                                                        <th id="th3" onclick="Sort(this,'ListDataTable','text');" class="sort" style="width:110px;"><%=this.Dictionary["Item_Document_ListHeader_Origin"] %></th>
			                                                        <th id="th4" onclick="Sort(this,'ListDataTable','text');" class="search sort" style="width:200px;"><%=this.Dictionary["Item_Document_ListHeader_Location"] %></th>
			                                                        <th id="th5" onclick="Sort(this,'ListDataTable','money');" class="sort" style="width:80px;"><%=this.Dictionary["Item_Document_ListHeader_Revision"] %></th>
			                                                        <th id="th6" onclick="Sort(this,'ListDataTable','date',false);"  class="hidden-480 sort" style="width:100px; text-align:center;"><%=this.Dictionary["Item_BusinessRisk_LabelField_DateStart"] %></th>
			                                                        <th style="width:107px;">&nbsp;</th>
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
                                                        <table class="table table-bordered table-striped" style="margin:0">
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
                                        </div>
                                    </div>
                                </div>                                                                            
                            </div><!-- /.col -->
                            
                            <div id="DocumentDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Document_PopupDelete_Message"] %>&nbsp;<strong><span id="DocumentName">&nbsp;</span></strong>?</p>
                                <p><i><%=this.Dictionary["Common_DeleteMessage_NoUndo"] %></i></p>
                                <div style="display:none;">
                                    <span id="TxtNewReasonLabel"><%=this.Dictionary["Item_Document_PopupDelete_Message"]%><br /></span>
                                    <textarea id="TxtNewReason" cols="40" rows="3"></textarea>
                                    <span class="ErrorMessage" id="TxtNewReasonErrorRequired"> <%=this.Dictionary["Item_Document_Error_DeleteReasonRequired"] %></span>
                                </div>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server"> 
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/js/common.js?ac<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/DocumentList.js?ac<%=this.AntiCache %>"></script>
</asp:Content>