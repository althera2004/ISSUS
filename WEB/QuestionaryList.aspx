<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="QuestionaryList.aspx.cs" Inherits="QuestionaryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #FooterStatus{visibility:hidden;}
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
        var ProcessList = <%=this.ProcessList %>;
        var RulesList = <%=this.RulesList %>;
        var ApartadosNormasList = <%=this.ApartadosNormasList %>;
        var QuestionaryList = <%= this.QuestionaryJsonList %>;
        var QuestionarySelectedId = null;
        var QuestionarySelected = null;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div class="col-xs-12">
                                <table cellpadding="2" cellspacing="2" style="width:100%;margin-bottom:16px;">
                                    <tr>
                                        <td id="CmbProcessLabel"><strong><%=this.Dictionary["Item_Questionary_List_Filter_Process"] %>:</strong></td>
                                        <td>
                                            <select id="CmbProcess" style="width:100%;"></select>
                                        </td>
                                        <td id="CmbRuleLabel"><strong><%=this.Dictionary["Item_Questionary_List_Filter_Rule"] %>:</strong></td>
                                        <td>
                                            <select id="CmbRules" style="width:100%;"></select>
                                        </td>
                                        <td id="CmbApartadoNormaLabel"><strong><%=this.Dictionary["Item_Questionary_List_Filter_ApartadoNorma"] %>:</strong></td>
                                        <td>
                                            <select id="CmbApartadosNorma" style="width:100%;min-width:120px;"></select>
                                        </td>
                                        <td>
                                            <div style="float:right">
                                                <button class="btn btn-success btn-filter" type="button" id="BtnRecordShowAll" title="<%=this.Dictionary["Common_All_Female_Plural"] %>"><i class="icon-list" style="margin-top:-2px;"></i></button>
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="table-responsive" id="scrollTableDiv">
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataHeader">
                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="sort search"><%=this.Dictionary["Item_Questionary_ListHeader_Name"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th1" class="sort search" style="width:300px;"><%=this.Dictionary["Item_Questionary_ListHeader_Process"] %></th>
                                                        <th onclick="Sort(this,'ListDataTable');" id="th2" class="sort search" style="width:300px;"><%=this.Dictionary["Item_Questionary_ListHeader_Rule"] %></th>
                                                        <th style="width:147px;">&nbsp;</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                            <div id="ListDataDiv" style="overflow:scroll;overflow-x:hidden;padding:0;">
                                                <table class="table table-bordered table-striped" style="border-top:none;">                                                        
                                                    <tbody id="ListDataTable">
                                                        <asp:Literal runat="server" ID="ProcesosData"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <table class="table table-bordered table-striped" style="margin:0">
                                                <thead class="thin-border-bottom">
                                                    <tr id="ListDataFooter">
                                                        <td><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<strong id="TotalList"><span id="QuestionaryDataTotal"></span></strong></td>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div><!-- /.table-responsive -->
                                    </div><!-- /span -->
                                </div><!-- /row -->								
                            </div><!-- /.col -->                            
                            <div id="QuestionaryDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Questionary_PopupDelete_Message"] %>&nbsp;<strong><span id="QuestionaryToDeleteName"></span></strong>?</p>
                            </div>                          
                            <div id="QuestionaryDuplicateDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Questionary_PopupDuplicate_Message"] %>&nbsp;<strong><span id="QuestionaryToDuplicateName"></span></strong>?</p>
                                <div class="row">
                                    <label class="col col-xs-2">Nombre<span style="color:#f00;">*</span>:</label>
                                    <div class="col col-xs-10">
                                        <input class="col col-xs-12" type="text" id="QuestionaryNewDescription" onblur="this.value = this.value.trim();" />
                                        <span id="QuestionaryNewDescriptionErrorRequired" class="ErrorMessage"><%= this.Dictionary["Common_Required"] %></span>
                                        <span id="QuestionaryNewDescriptionErrorDuplicated" class="ErrorMessage"><%= this.Dictionary["Common_AlreadyExists"] %></span>
                                    </div>
                                </div>
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
        <script type="text/javascript" src="/js/QuestionaryList.js?<%=this.AntiCache %>"></script>
</asp:Content>