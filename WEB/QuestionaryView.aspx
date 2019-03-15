<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="QuestionaryView.aspx.cs" Inherits="QuestionaryView" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
    <script type="text/javascript">
        var Questionary = <%=this.Questionary.Json %>;
        var Questions = <%=this.Questions %>;
        var ApartadosNorma = <%=this.ApartadosNormasList %>;
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                            <div>
                                <div id="user-profile-2" class="user-profile">
                                    <div class="tabbable">
                                        <% if (!this.NewQuestionary)
                                           { %>
                                        <ul class="nav nav-tabs padding-18">
                                            <li class="active">
                                                <a data-toggle="tab" href="#home"><%=this.Dictionary["Item_JobPosition_Tab_Principal"]%></a>
                                            </li>
                                            <!--<% if (this.GrantTraces) { %>
                                            <li class="" id="TabTrazas">
                                                <a data-toggle="tab" href="#trazas"><%=this.Dictionary["Item_JobPosition_Tab_Traces"]%></a>
                                            </li>
                                            <% } %>-->
                                        </ul>
                                        <% } %>
                                        <div class="tab-content no-border padding-24">
                                            <div id="home" class="tab-pane active">                                                
                                                <div class="form-horizontal" role="form">
                                                    <div class="form-group">
                                                        <label class="col-sm-1 control-label no-padding-right" style="margin-right:15px;" id="TxtNameLabel"><%=this.Dictionary["Item_Questionary"] %><span style="color:#f00">*</span></label>                                        
                                                        <%=this.TxtName %>					                    
                                                    </div>
                                                    <div class="form-group">
                                                        <label id="TxtProcessNameLabel" class="col-sm-1 control-label no-padding-right" style="margin-right:15px;"><%=this.Dictionary["Item_Questionary_FieldLabel_Process"]%><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-3" id="DivCmbProcess" style="height:35px !important;">
                                                            <select id="CmbProcess" class="col-xs-12 col-sm-12 tooltip-info" onchange="CmbProcessChanged();"><asp:Literal runat="server" ID="LtProcessList"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="TxtProcessName" placeholder="<%=this.Dictionary["Item_Questionary_FieldLabel_Process"] %>" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="TxtProcessNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label id="TxtRuleNameLabel" class="col-sm-1 control-label no-padding-right" style="margin-right:15px;"><%=this.Dictionary["Item_Questionary_FieldLabel_Rule"]%><span style="color:#f00">*</span></label>
                                                        <div class="col-sm-3" id="DivCmbRule" style="height:35px !important;">
                                                            <select id="CmbRule" class="col-xs-12 col-sm-12 tooltip-info" onchange="FillApartadoNorma();"><asp:Literal runat="server" ID="LtRulesList"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="TxtRuleName" placeholder="<%=this.Dictionary["Item_Questionary_FieldLabel_Rule"] %>" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="TxtRuleNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                        <label id="TxtApartadoNormaNameLabel" class="col-sm-1 control-label no-padding-right" style="margin-right:15px;"><%=this.Dictionary["Item_Questionary_FieldLabel_ApartadoNorma"]%></label>
                                                        <div class="col-sm-2" id="DivCmbApartadoNorma" style="height:35px !important;">
                                                            <select id="CmbApartadoNorma" class="col-xs-12 col-sm-12 tooltip-info" onchange="CmbApartadoNormaChanged();"><asp:Literal runat="server" ID="LtApartadosNormaList"></asp:Literal></select>
                                                            <input style="display:none;" type="text" readonly="readonly" id="TxtApartadoNormaName" placeholder="<%=this.Dictionary["Item_Questionary_FieldLabel_ApartadoNorma"] %>" class="col-xs-12 col-sm-12" />
                                                            <span class="ErrorMessage" id="TxtApartadoNormaNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-sm-6"><%=this.Dictionary["Item_Questionary_FieldLabel_Notes"] %></label>
                                                        <div class="col-sm-12"><textarea rows="5" class="form-control col-xs-12 col-sm-12" maxlength="2000" id="TxtNotes"><%=this.Questionary.Notes %></textarea></div>
                                                    </div> 
                                                    <div class="row" id="TableQuestionsHeader" style="display:none;">
                                                        <div class="col-xs-6"><h4><%=this.Dictionary["Item_QuestionaryQuestion_Title"]%></h4></div>
                                                        <div class="col-xs-4">
                                                            <div class="nav-search" id="nav-search-question" style="display: block;">                            
                                                                <span class="input-icon">
                                                                    <input type="text" placeholder="Buscar" ..."="" class="nav-search-input" id="nav-search-input-question" autocomplete="off" />
                                                                    <i class="icon-search nav-search-icon"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <div class="col-xs-2"><button class="btn btn-success" type="button" id="BtnNewItem" onclick="" style="margin-top:6px;height:28px;padding-top:0;"><i class="icon-plus bigger-110"></i>Añadir pregunta</button></div>
                                                    </div>
                                                    
                                                    <div class="table-responsive" id="scrollTableDiv" style="display:none;">
                                                        <table class="table table-bordered table-striped" style="margin: 0">
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataHeader">
			                                                        <th onclick="Sort(this,'ListDataTable');" id="th0" class="search sort" style="cursor:pointer;"><%=this.Dictionary["Item_QuestionaryQuestion_ListHeader_Name"] %></th>
			                                                        <th style="width:107px;">&nbsp;</th>
		                                                        </tr>
                                                            </thead>
                                                        </table>
                                                        <div id="ListDataDiv" style="display:none;overflow: scroll; overflow-x: hidden; padding: 0;min-height:120px;">
                                                            <table class="table table-bordered table-striped" style="border-top: none;">
                                                                <tbody id="ListDataTable"></tbody>
                                                            </table>
                                                        </div>
                                                        <div id="NoData" style="display:none;width:100%;height:99%;min-height:120px;background-color:#eef;text-align:center;font-size:large;color:#aaf;">&nbsp;<div style="height:40%;"></div><i class="icon-info-sign"></i>&nbsp;<%=this.Dictionary["Common_VoidSearchResult"] %></div>                                                        
                                                        <table class="table table-bordered table-striped" style="margin: 0" >
                                                            <thead class="thin-border-bottom">
                                                                <tr id="ListDataFooter">
                                                                    <th style="color:#aaa;"><i><%=this.Dictionary["Common_RegisterCount"] %>:&nbsp;<span id="QuestionsTotal"></span></i></th>
                                                                </tr>
                                                            </thead>
                                                        </table>
                                                    </div> <!-- /.table-responsive -->
                                                    <div class="alert alert-info" style="display:none;" id="DivNewQuestionary">
                                                        <strong><i class="icon-info-sign fa-2x"></i></strong>
                                                        <h3 style="display:inline;"><%=this.Dictionary["Item_Questionary_NewQuestionaryWarning"] %></h3>
                                                    </div>
                                                    <%=this.FormFooter %>
                                                </div>
                                            </div>
                                            <div id="trazas" class="tab-pane">													
                                                <table class="table table-bordered table-striped">
                                                    <thead class="thin-border-bottom">
                                                        <tr>
                                                            <th style="width:150px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"]%></th>
                                                            <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"]%></th>
                                                            <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"]%></th>
                                                            <th style="width:250px;"><%= this.Dictionary["Item_Tace_ListHeader_User"]%></th>													
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Literal runat="server" ID="LtTrazas"></asp:Literal>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>                                            
                                    </div>
                                </div>
                            </div>
                            <div id="QuestionaryQuestionDeleteDialog" class="hide" style="width:500px;">
                                <p><%=this.Dictionary["Item_QuestionaryQuestion_PopupDelete_Message"] %>&nbsp;<strong><span id="QuestionaryQuestionName"></span></strong>?</p>
                            </div>
                            <div id="QuestionaryQuestionUpdateDialog" class="hide" style="width: 600px;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right" id="TxtQuestionaryQuestionUpdateNameLabel"><%=this.Dictionary["Item_Questionary_FieldLabel_QuestionaryQuestionName"] %>:</label>
                                        <div class="col-sm-10">
                                            <textarea id="TxtQuestionaryQuestionUpdateName" class="col-sm-12" rows="5" placeholder="<%= this.Dictionary["Item_Questionary_FieldLabel_QuestionaryQuestionName"] %>" maxlength="2000" onblur="this.value=$.trim(this.value);"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <span class="ErrorMessage" id="TxtQuestionaryQuestionUpdateNameErrorRequired"><%= this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtQuestionaryQuestionUpdateNameErrorDuplicated"><%= this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
                            <div id="QuestionaryQuestionInsertDialog" class="hide" style="width:600px;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label class="col-sm-2 control-label no-padding-right" id="TxtQuestionaryQuestionNewNameLabel"><%=this.Dictionary["Item_Questionary_FieldLabel_QuestionaryQuestionName"] %>:</label> 
                                        <div class="col-sm-10">
                                            <textarea id="TxtQuestionaryQuestionNewName" class="col-sm-12" rows="5" placeholder="<%= this.Dictionary["Item_Questionary_FieldLabel_QuestionaryQuestionName"] %>" maxlength="2000" onblur="this.value=$.trim(this.value);"></textarea>
                                        </div>
                                    </div>
                                </div>
                                <span class="ErrorMessage" id="TxtQuestionaryQuestionNewNameErrorRequired"><%=this.Dictionary["Common_Required"] %></span>
                                <span class="ErrorMessage" id="TxtQuestionaryQuestionNewNameErrorDuplicated"><%=this.Dictionary["Common_Error_NameAlreadyExists"] %></span>
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
        <script type="text/javascript" src="/assets/js/jquery-ui-1.10.3.full.min.js"></script>
        <script type="text/javascript" src="/assets/js/jquery.ui.touch-punch.min.js"></script>
        <script type="text/javascript" src="/js/common.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/QuestionaryView.js?<%=this.AntiCache %>"></script>
        <script type="text/javascript" src="/js/UploadFile.js?ac<%= this.AntiCache %>"></script>
</asp:Content>

