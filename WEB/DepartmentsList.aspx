﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="DepartmentsList.aspx.cs" Inherits="DepartmentsList" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th style="width: 220px;" onclick="Sort(this,'ListDataTable', 'text');" id="th0" class="sort search"><%=this.Dictionary["Item_Department_ListHeader_Name"] %></th>
                                                        <th style="width: 300px;" class="search"><%=this.Dictionary["Item_Department_ListHeaderJobPositions"] %></th>
                                                        <th class="search"><%=this.Dictionary["Item_Department_ListHeaderEmployees"] %></th>
                                                        <th style="width: 106px;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;">
                                                <table class="table table-bordered table-striped" style="border-top: none;">
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="DepartmentData"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin: 0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<asp:Literal runat="server" ID="DeparmentDataTotal"></asp:Literal></i></th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->                                        
                                    </div><!-- /span -->
                                </div><!-- /row -->
                            </div><!-- /.col -->

                            <!-- Popups -->
                            <div id="DepartmentDeleteDialog" class="hide" style="width: 500px;">
                                <p><%=this.Dictionary["Item_Department_PopupDelete_Message"] %>&nbsp;<strong><span id="DepartmentName"></span></strong>?</p>
                            </div>
                            <div id="DepartmentUpdateDialog" class="hide" style="width: 500px;">
                                <p><span id="TxtDepartmentNameLabel"><%=this.Dictionary["Common_Name"] %></span>&nbsp;<input type="text" id="TxtDepartmentName" /></p>
                                <span class="ErrorMessage" id="TxtDepartmentNameErrorRequired" style="display: none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentNameErrorDuplicated" style="display: none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
                            <div id="DepartmentInsertDialog" class="hide" style="width: 500px;">
                                <p><span id="TxtDepartmentNewNameLabel"><%=this.Dictionary["Common_Name"] %></span>&nbsp;<input type="text" id="TxtDepartmentNewName" /></p>
                                <span class="ErrorMessage" id="TxtDepartmentNewNameErrorRequired" style="display: none;"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtDepartmentNewNameErrorDuplicated" style="display: none;"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server"> 
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>  
        <script type="text/javascript" src="/js/common.js?ac=<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/DepartmentList.js?<%=this.AntiCache %>"></script>
</asp:Content>

