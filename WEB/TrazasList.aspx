<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="TrazasList.aspx.cs" Inherits="TrazasList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
    <link rel="stylesheet" href="assets/css/jquery-ui-1.10.3.full.min.css" />
    <style type="text/css">
        #FooterStatus{visibility:hidden;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="widget-box transparent" id="recent-box">
                                            <div class="widget-header">
                                                <h4 class="lighter smaller">
                                                    <i class="icon-rss orange"></i>
                                                    <%=this.Dictionary["Item_Trace_LastActions"] %> (24 h.)
                                                </h4>

                                                <div class="widget-toolbar no-border">
                                                    <ul class="nav nav-tabs" id="recent-tab">
                                                        <li class="active">
                                                            <a data-toggle="tab" href="#document-tab"><%=this.Dictionary["Item_Documents"] %></a>
                                                        </li>
                                                        <li class="">
                                                            <a data-toggle="tab" href="#process-tab"><%=this.Dictionary["Item_Processes"] %></a>
                                                        </li>
                                                        <li>
                                                            <a data-toggle="tab" href="#learning-tab"><%=this.Dictionary["Item_Learning"] %></a>
                                                        </li>
                                                        <li>
                                                            <a data-toggle="tab" href="#learningAssistant-tab"><%=this.Dictionary["Item_LearningAssistants"]%></a>
                                                        </li>
                                                        <li>
                                                            <a data-toggle="tab" href="#employee-tab"><%=this.Dictionary["Item_Employees"] %></a>
                                                        </li>
                                                        <li>
                                                            <a data-toggle="tab" href="#department-tab"><%=this.Dictionary["Item_Departments"] %></a>
                                                        </li>
                                                        <li>
                                                            <a data-toggle="tab" href="#jobposition-tab"><%=this.Dictionary["Item_JobPositions"] %></a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>

                                            <div class="widget-body">
                                                <div class="widget-main padding-4">
                                                    <div class="tab-content padding-8 overflow-visible">
                                                        <div id="document-tab" class="tab-pane active">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesDocument"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="process-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTraceProccess"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="learning-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesLearning"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="learningAssistant-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesLearningAssistant"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="employee-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesEmployee"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="department-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesDepartment"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="jobposition-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesJobPosition"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="user-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesUser"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div id="otros-tab" class="tab-pane">
                                                            <table class="table table-bordered table-striped">
                                                                <thead class="thin-border-bottom">
                                                                    <tr>
                                                                        <th class="hidden-480" style="width:140px;"><%=this.Dictionary["Item_Tace_ListHeader_Date"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Name"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Reason"] %></th>
                                                                        <th><%=this.Dictionary["Item_Tace_ListHeader_Trace"] %></th>
                                                                        <th class="hidden-480"><%=this.Dictionary["Item_Tace_ListHeader_User"] %></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:Literal runat="server" ID="LtTracesOtros"></asp:Literal>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div><!-- /widget-main -->
                                            </div><!-- /widget-body -->
                                        </div><!-- /widget-box -->
                                    </div><!-- /span -->
                                </div><!-- /row -->
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
</asp:Content>

