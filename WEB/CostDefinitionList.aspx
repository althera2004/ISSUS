<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="CostDefinitionList.aspx.cs" Inherits="CostDefinitionList" %>

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
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
		                                            <tr id="ListDataHeader">
			                                            <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_CostDefinition_ListHeader_Name"] %></th>
			                                            <th onclick="Sort(this,'ListDataTable','money',false);" id="th1" class="search sort" style="width:200px; text-align:right"><%=this.Dictionary["Item_CostDefinition_ListHeader_Amount"]%>&nbsp;&nbsp;</th>
                                                        <th style="width:106px;">&nbsp;</th>
                                                    </tr>
	                                            </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow:scroll;overflow-x:hidden;padding:0;">
                                                <table class="table table-bordered table-striped" style="border-top:none;">                                                        
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="CostDefinitionData"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="CostDefinitionDataTotal"></asp:Literal></i></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->			
                            </div><!-- /.col -->

                            <div id="CostDefinitionDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_CostDefinition_PopupDelete_Message"] %>&nbsp;<strong><span id="CostDefinitionName"></span></strong>?</p>
                            </div>
                            <div id="CostDefinitionUpdateDialog" class="hide" style="width:500px;">
                                <p><span id="TxtCostDefinitionNameLabel"><%=this.Dictionary["Common_Name"] %></span>&nbsp;<input type="text" id="TxtCostDefinitionName" /></p>
                                <span class="ErrorMessage" id="TxtCostDefinitionNameErrorRequired" style="display:none;"> <%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtCostDefinitionNameErrorDuplicated" style="display:none;"> <%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
                            <div id="CostDefinitionInsertDialog" class="hide" style="width:500px;">
                                <p><span id="TxtCostDefinitionNewNameLabel"><%=this.Dictionary["Common_Name"] %></span>&nbsp;<input type="text" id="TxtCostDefinitionNewName" /></p>
                                <span class="ErrorMessage" id="TxtCostDefinitionNewNameErrorRequired" style="display:none;"> <%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtCostDefinitionNewNameErrorDuplicated" style="display:none;"> <%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/CostDefinitionList.js?<%=this.AntiCache %>"></script>
</asp:Content>

