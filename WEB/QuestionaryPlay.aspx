<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="QuestionaryPlay.aspx.cs" Inherits="QuestionaryPlay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        .iconfile {
            border: 1px solid #777;
            background-color: #fdfdfd;
            -webkit-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            -moz-box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            box-shadow: 4px 4px 3px 0px rgba(166,159,166,1);
            padding-left:0!important;
            padding-top:4px !important;
            padding-bottom:4px !important;
            margin-bottom:12px !important;
        }        
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

        .page-header {
            background-color:#fff;
            z-index:999;
            position: fixed;
            margin-top:-36px;
            -webkit-box-shadow: 4px 4px 3px 0px rgba(166,159,166,.3);
            -moz-box-shadow: 4px 4px 3px 0px rgba(166,159,166,.3);
            box-shadow: 4px 4px 3px 0px rgba(166,159,166,.3);
        }

        #navbar, #menu-toggler, #sidebar, #logofooter, #breadcrumbs, #oldFormFooter  {
            display: none;
        }

        .main-content {
            margin: 0;
        }

        #main-container{
            padding:0;
        }

        #user-profile-2{
            margin-top:60px;
        }

        #content-page {
            left: 0;
            padding-top:0;
            margin-top:-36px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var AuditoryId = <%=this.Auditory.Id %>;
        var AuditoryName = "<%=this.Auditory.Description %>";
        var QuestionaryId = <%=this.Questionary.Id %>;
        var QuestionaryName = "<%=this.Questionary.Description %>";
        var Founds = <%=this.Founds %>;
        var Improvements = <%=this.Improvements %>;
        var Editable = <%=this.Editable %>;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div id="user-profile-2" class="user-profile">                                             
                                                <div class="form-horizontal" role="form">
                                                    
                                                    <div class="table-responsive" id="scrollTableDiv">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeader">
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_QuestionaryQuestion_ListHeader_Name"] %></th>
			                                                        <th style="width:173px;">Resultado</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="overflow: scroll; overflow-x: hidden; padding: 0;min-height:200px;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTable"><asp:Literal runat="server" ID="LtQuestions"></asp:Literal></tbody>
                                                            </table>
                                                        </div>
                                                    </div> <!-- /.table-responsive -->
                                                    <%=this.FormFooter %>
                                                </div>
                                                <br />
                                                <div class="row" id="TableFoundHeader">
                                                    <div class="col-xs-6"><h4><%=this.Dictionary["Item_Auditory_Title_Found"]%></h4></div>
                                                    <div class="col-xs-4">&nbsp;</div>
                                                    <div class="col-xs-2" style="text-align:right;"><button class="btn btn-success" type="button" id="BtnNewFound" onclick="ShowPopupFoundDialog(-1);" style="margin-top:6px;height:28px;padding-top:0;"><i class="icon-plus bigger-110"></i><%= this.Dictionary["Item_Auditory_Button_AddFound"] %></button></div>
                                                </div>  
                                                <div class="table-responsive" id="scrollTableDivHallazgos">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderHallazgos">
			                                                    <th><%=this.Dictionary["Item_Auditory_Header_Found"] %></th>
			                                                    <th style="width:200px;"><%=this.Dictionary["Item_Auditory_Header_Requirement"] %></th>
			                                                    <th style="width:200px;"><%=this.Dictionary["Item_Auditory_Header_Result"] %></th>
			                                                    <th style="width:107px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivHallazgos" style="display:none;overflow-y: scroll; overflow-x: hidden; padding: 0;min-height:100px;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="HallazgosDataTable"></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataHallazgos" style="width:100%;height:99%;min-height:100px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterHallazgos">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanHallazgosTotal"></span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->
                                                <br />
                                                <div class="row" id="TableImprovementHeader">
                                                    <div class="col-xs-6"><h4><%=this.Dictionary["Item_Auditory_Title_Improvements"]%></h4></div>
                                                    <div class="col-xs-4">&nbsp;</div>
                                                    <div class="col-xs-2" style="text-align:right;"><button class="btn btn-success" type="button" id="BtnNewImprovement" onclick="ShowPopupImprovementDialog(-1);" style="margin-top:6px;height:28px;padding-top:0;"><i class="icon-plus bigger-110"></i><%= this.Dictionary["Item_Auditory_Button_AddImprovement"] %></button></div>
                                                </div>
                                                <div class="table-responsive" id="scrollTableDivMejoras">
                                                    <table class="table table-bordered table-striped" style="margin: 0">
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataHeaderMejoras">
			                                                    <th><%=this.Dictionary["Item_Auditory_Header_Improvement"] %></th>
			                                                    <th style="width:107px;">&nbsp;</th>
		                                                    </tr>
                                                        </thead>
                                                    </table>
                                                    <div id="ListDataDivMejoras" style="display:none;overflow-y: scroll; overflow-x: hidden; padding: 0;min-height:100px;">
                                                        <table class="table table-bordered table-striped" style="border-left:none;border-top: none;">
                                                            <tbody id="MejorasDataTable"></tbody>
                                                        </table>
                                                    </div>
                                                    <div id="NoDataMejoras" style="width:100%;height:99%;min-height:100px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>
                                                    <table class="table table-bordered table-striped" style="margin: 0" >
                                                        <thead class="thin-border-bottom">
                                                            <tr id="ListDataFooterMejoras">
                                                                <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="SpanMejorasTotal"></span></i></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div> <!-- /.table-responsive -->
                            </div>

                            </div>
                            <div id="foundDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_PopupDelete_MessageFound"] %>&nbsp;<strong><span id="foundName"></span></strong>?</p>
                            </div>
                            <div id="improvementDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_Auditory_PopupDelete_MessageImprovement"] %>&nbsp;<strong><span id="imporvementName"></span></strong>?</p>
                            </div>

                            <div id="PopupFoundDialog" class="hide" style="width:600px;overflow:hidden;">
                                <div class="form-group" style="clear:both;">
                                    <label id ="TxtTextLabel" class="col-sm-4 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Found"] %></label>
                                    <div class="col-sm-8">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtText"></textarea>
                                        <span class="ErrorMessage" id="TxtTextErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="form-group" style="clear:both;">
                                    <label id ="TxtRequerimentLabel" class="col-sm-4 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Requeriment"] %></label>
                                    <div class="col-sm-8">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtRequeriment"></textarea>
                                        <span class="ErrorMessage" id="TxtRequerimentErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                                <div class="form-group" style="clear:both;">
                                    <label id ="TxUnconformityLabel" class="col-sm-4 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Unconformity"] %></label>
                                    <div class="col-sm-8">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtUnconformity"></textarea>
                                        <span class="ErrorMessage" id="TxtUnconformityErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                            </div>

                            <div id="PopupImprovementDialog" class="hide" style="width:600px;overflow:hidden;">
                                <div class="form-group" style="clear:both;">
                                    <label id ="TxtImprovementTextLabel" class="col-sm-4 control-label no-padding-right" for="TxtHour"><%=this.Dictionary["Item_Auditory_Label_Improvement"] %></label>
                                    <div class="col-sm-8">
                                        <textarea class="col-xs-12 col-sm-12" rows="5" id="TxtImprovement"></textarea>
                                        <span class="ErrorMessage" id="TxtImprovementErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                    </div>
                                </div>
                            </div>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/QuestionaryPlay.js?<%=this.AntiCache %>"></script>
</asp:Content>

