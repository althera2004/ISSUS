<%@ Page Title="" Language="C#" MasterPageFile="~/Giso.master" AutoEventWireup="true" CodeFile="Schedule.aspx.cs" Inherits="Schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageStyles" Runat="Server">
        <link rel="stylesheet" href="assets/css/fullcalendar.min.css" />
        <style>
            .fc-title {
                white-space:normal;
            }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageScripts" Runat="Server">
    <script type="text/javascript">
        var tasks = <%= this.Tasks %>;
        var MonthName =
        [
            Dictionary.Common_MonthName_January,
            Dictionary.Common_MonthName_February,
            Dictionary.Common_MonthName_March,
            Dictionary.Common_MonthName_April,
            Dictionary.Common_MonthName_May,
            Dictionary.Common_MonthName_June,
            Dictionary.Common_MonthName_July,
            Dictionary.Common_MonthName_August,
            Dictionary.Common_MonthName_September,
            Dictionary.Common_MonthName_October,
            Dictionary.Common_MonthName_November,
            Dictionary.Common_MonthName_December
        ];
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptHeadContentHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Contentholder1" Runat="Server">

<div class="col-xs-12">
                                <!-- PAGE CONTENT BEGINS -->
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="space"></div>
                                        <div id="calendar" class="fc fc-ltr fc-unthemed">
                                            <div class="fc-toolbar">
                                                <div class="fc-left">
                                                    <div class="fc-button-group">
                                                        <button type="button" class="fc-prev-button fc-button fc-state-default fc-corner-left" title="AA"><span class="fc-icon fc-icon-left-single-arrow" onclick="PreviousMonth();"></span></button>
                                                        <button type="button" class="fc-today-button fc-button fc-state-default fc-corner-left fc-corner-right" onclick="ActualMonth();"><%=this.Dictionary["Common_ActualMonth"]%></button>
                                                        <button type="button" class="fc-next-button fc-button fc-state-default fc-corner-right" title="BB"><span class="fc-icon fc-icon-right-single-arrow" onclick="NextMonth();"></span></button>
                                                    </div>                                                    
                                                </div>
                                                <div class="fc-right" style="display:none;">
                                                    <div class="fc-button-group">
                                                        <button type="button" class="fc-month-button fc-button fc-state-default fc-corner-left fc-state-active"><%=this.Dictionary["Common_Month"] %></button>
                                                        <button type="button" class="fc-agendaWeek-button fc-button fc-state-default"><%=this.Dictionary["Common_Week"] %></button>
                                                        <button type="button" class="fc-agendaDay-button fc-button fc-state-default fc-corner-right"><%=this.Dictionary["Common_Day"] %></button>
                                                    </div>
                                                </div>
                                                <div class="fc-center"><h2 id="ActualMonthLabel"></h2></div>
                                                <div class="fc-clear"></div>
                                            </div>
                                            <div class="fc-view-container" style="">
                                                <div class="fc-view fc-month-view fc-basic-view">
                                                    <table>
                                                        <thead>
                                                            <tr>
                                                                <td class="fc-widget-header">
                                                                    <div class="fc-row fc-widget-header">
                                                                        <table>
                                                                            <thead>
                                                                                <tr>
                                                                                    <th class="fc-day-header fc-widget-header fc-sun"><%=this.Dictionary["Common_WeekDayShortName_Monday"] %></th>
                                                                                    <th class="fc-day-header fc-widget-header fc-mon"><%=this.Dictionary["Common_WeekDayShortName_Tuesday"]%></th>
                                                                                    <th class="fc-day-header fc-widget-header fc-tue"><%=this.Dictionary["Common_WeekDayShortName_Wednesday"] %></th>
                                                                                    <th class="fc-day-header fc-widget-header fc-wed"><%=this.Dictionary["Common_WeekDayShortName_Thursday"] %></th>
                                                                                    <th class="fc-day-header fc-widget-header fc-thu"><%=this.Dictionary["Common_WeekDayShortName_Friday"] %></th>
                                                                                    <th class="fc-day-header fc-widget-header fc-fri"><%=this.Dictionary["Common_WeekDayShortName_Saturday"] %></th>
                                                                                    <th class="fc-day-header fc-widget-header fc-sat"><%=this.Dictionary["Common_WeekDayShortName_Sunday"] %></th>
                                                                                </tr>
                                                                            </thead>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td class="fc-widget-content">
                                                                    <div class="fc-day-grid-container">
                                                                        <div class="fc-day-grid">
                                                                            <div class="fc-row fc-week fc-widget-content" style="height: 91px;">
                                                                                <div class="fc-bg">
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w1d1b" class="fc-day fc-widget-content fc-sun"></td>
                                                                                                <td id="w1d2b" class="fc-day fc-widget-content fc-mon"></td>
                                                                                                <td id="w1d3b" class="fc-day fc-widget-content fc-tue"></td>
                                                                                                <td id="w1d4b" class="fc-day fc-widget-content fc-wed"></td>
                                                                                                <td id="w1d5b" class="fc-day fc-widget-content fc-thu"></td>
                                                                                                <td id="w1d6b" class="fc-day fc-widget-content fc-fri"></td>
                                                                                                <td id="w1d7b" class="fc-day fc-widget-content fc-sat"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div class="fc-content-skeleton">
                                                                                    <table>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <td id="w1d1n" class="fc-day-number fc-sun">26</td>
                                                                                                <td id="w1d2n" class="fc-day-number fc-mon">27</td>
                                                                                                <td id="w1d3n" class="fc-day-number fc-tue">28</td>
                                                                                                <td id="w1d4n" class="fc-day-number fc-wed">29</td>
                                                                                                <td id="w1d5n" class="fc-day-number fc-thu">30</td>
                                                                                                <td id="w1d6n" class="fc-day-number fc-fri">1</td>
                                                                                                <td id="w1d7n" class="fc-day-number fc-sat">2</td>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w1d1d"></td>
                                                                                                <td id="w1d2d"></td>
                                                                                                <td id="w1d3d"></td>
                                                                                                <td id="w1d4d"></td>
                                                                                                <td id="w1d5d"></td>
                                                                                                <td id="w1d6d" class="fc-event-container">
                                                                                                    
                                                                                                </td>
                                                                                                <td id="w1d7d"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                            <div class="fc-row fc-week fc-widget-content" style="height: 91px;">
                                                                                <div class="fc-bg">
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w2d1b" class="fc-day fc-widget-content fc-sun"></td>
                                                                                                <td id="w2d2b" class="fc-day fc-widget-content fc-mon"></td>
                                                                                                <td id="w2d3b" class="fc-day fc-widget-content fc-tue"></td>
                                                                                                <td id="w2d4b" class="fc-day fc-widget-content fc-wed"></td>
                                                                                                <td id="w2d5b" class="fc-day fc-widget-content fc-thu"></td>
                                                                                                <td id="w2d6b" class="fc-day fc-widget-content fc-fri"></td>
                                                                                                <td id="w2d7b" class="fc-day fc-widget-content fc-sat"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div class="fc-content-skeleton">
                                                                                    <table>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <td id="w2d1n" class="fc-day-number fc-sun">3</td>
                                                                                                <td id="w2d2n" class="fc-day-number fc-mon">4</td>
                                                                                                <td id="w2d3n" class="fc-day-number fc-tue">5</td>
                                                                                                <td id="w2d4n" class="fc-day-number fc-wed">6</td>
                                                                                                <td id="w2d5n" class="fc-day-number fc-thu">7</td>
                                                                                                <td id="w2d6n" class="fc-day-number fc-fri">8</td>
                                                                                                <td id="w2d7n" class="fc-day-number fc-sat">9</td>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w2d1d"></td>
                                                                                                <td id="w2d2d"></td>
                                                                                                <td id="w2d3d"></td>
                                                                                                <td id="w2d4d"></td>
                                                                                                <td id="w2d5d"></td>
                                                                                                <td id="w2d6d"></td>
                                                                                                <td id="w2d7d"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                            <div class="fc-row fc-week fc-widget-content" style="height: 91px;">
                                                                                <div class="fc-bg">
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w3d1b" class="fc-day fc-widget-content fc-sun"></td>
                                                                                                <td id="w3d2b" class="fc-day fc-widget-content fc-mon"></td>
                                                                                                <td id="w3d3b" class="fc-day fc-widget-content fc-tue"></td>
                                                                                                <td id="w3d4b" class="fc-day fc-widget-content fc-wed"></td>
                                                                                                <td id="w3d5b" class="fc-day fc-widget-content fc-thu"></td>
                                                                                                <td id="w3d6b" class="fc-day fc-widget-content fc-fri"></td>
                                                                                                <td id="w3d7b" class="fc-day fc-widget-content fc-sat"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div class="fc-content-skeleton">
                                                                                    <table>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <td id="w3d1n" class="fc-day-number fc-sun">10</td>
                                                                                                <td id="w3d2n" class="fc-day-number fc-mon">11</td>
                                                                                                <td id="w3d3n" class="fc-day-number fc-tue">12</td>
                                                                                                <td id="w3d4n" class="fc-day-number fc-wed">13</td>
                                                                                                <td id="w3d5n" class="fc-day-number fc-thu">14</td>
                                                                                                <td id="w3d6n" class="fc-day-number fc-fri">15</td>
                                                                                                <td id="w3d7n" class="fc-day-number fc-sat">16</td>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w3d1d"></td>
                                                                                                <td id="w3d2d"></td>
                                                                                                <td id="w3d3d"></td>
                                                                                                <td id="w3d4d"></td>
                                                                                                <td id="w3d5d" class="fc-event-container">
                                                                                                    <a class="fc-day-grid-event fc-event fc-start fc-not-end label-success fc-draggable">
                                                                                                        <div class="fc-content">
                                                                                                            <span class="fc-title">Long Event</span>
                                                                                                        </div>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td id="w3d6d"></td>
                                                                                                <td id="w3d7d"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                            <div class="fc-row fc-week fc-widget-content" style="height: 91px;">
                                                                                <div class="fc-bg">
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w4d1b" class="fc-day fc-widget-content fc-sun"></td>
                                                                                                <td id="w4d2b" class="fc-day fc-widget-content fc-mon"></td>
                                                                                                <td id="w4d3b" class="fc-day fc-widget-content fc-tue"></td>
                                                                                                <td id="w4d4b" class="fc-day fc-widget-content fc-wed"></td>
                                                                                                <td id="w4d5b" class="fc-day fc-widget-content fc-thu"></td>
                                                                                                <td id="w4d6b" class="fc-day fc-widget-content fc-fri"></td>
                                                                                                <td id="w4d7b" class="fc-day fc-widget-content fc-sat"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div class="fc-content-skeleton">
                                                                                    <table>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <td id="w4d1n" class="fc-day-number fc-sun">17</td>
                                                                                                <td id="w4d2n" class="fc-day-number fc-mon">18</td>
                                                                                                <td id="w4d3n" class="fc-day-number fc-tue">19</td>
                                                                                                <td id="w4d4n" class="fc-day-number fc-wed">20</td>
                                                                                                <td id="w4d5n" class="fc-day-number fc-thu">21</td>
                                                                                                <td id="w4d6n" class="fc-day-number fc-fri">22</td>
                                                                                                <td id="w4d7n" class="fc-day-number fc-sat">23</td>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w4d1d" class="fc-event-container">
                                                                                                    <a class="fc-day-grid-event fc-event fc-not-start fc-end label-success fc-draggable fc-resizable">
                                                                                                        <div class="fc-content">
                                                                                                            <span class="fc-title">Long Event</span>
                                                                                                        </div>
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td id="w4d2d"></td>
                                                                                                <td id="w4d3d"></td>
                                                                                                <td id="w4d4d"></td>
                                                                                                <td id="w4d5d"></td>
                                                                                                <td id="w4d6d"></td>
                                                                                                <td id="w4d7d"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                            <div class="fc-row fc-week fc-widget-content" style="height: 91px;">
                                                                                <div class="fc-bg">
                                                                                    <table>
                                                                                        <tbody>                                                                                        
                                                                                            <tr>
                                                                                                <td id="w5d1b" class="fc-day fc-widget-content fc-sun"></td>
                                                                                                <td id="w5d2b" class="fc-day fc-widget-content fc-mon"></td>
                                                                                                <td id="w5d3b" class="fc-day fc-widget-content fc-tue"></td>
                                                                                                <td id="w5d4b" class="fc-day fc-widget-content fc-wed"></td>
                                                                                                <td id="w5d5b" class="fc-day fc-widget-content fc-thu"></td>
                                                                                                <td id="w5d6b" class="fc-day fc-widget-content fc-fri"></td>
                                                                                                <td id="w5d7b" class="fc-day fc-widget-content fc-sat"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div class="fc-content-skeleton">
                                                                                    <table>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <td id="w5d1n" class="fc-day-number fc-sun">24</td>
                                                                                                <td id="w5d2n" class="fc-day-number fc-mon">25</td>
                                                                                                <td id="w5d3n" class="fc-day-number fc-tue">26</td>
                                                                                                <td id="w5d4n" class="fc-day-number fc-wed">27</td>
                                                                                                <td id="w5d5n" class="fc-day-number fc-thu">28</td>
                                                                                                <td id="w5d6n" class="fc-day-number fc-fri">29</td>
                                                                                                <td id="w5d7n" class="fc-day-number fc-sat">30</td>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w5d1d"></td>
                                                                                                <td id="w5d2d"></td>
                                                                                                <td id="w5d3d"></td>
                                                                                                <td id="w5d4d"></td>
                                                                                                <td id="w5d5d"></td>
                                                                                                <td id="w5d6d"></td>
                                                                                                <td id="w5d7d"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                            <div class="fc-row fc-week fc-widget-content" style="height: 93px;">
                                                                                <div class="fc-bg">
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w6d1b" class="fc-day fc-widget-content fc-sun"></td>
                                                                                                <td id="w6d2b" class="fc-day fc-widget-content fc-mon"></td>
                                                                                                <td id="w6d3b" class="fc-day fc-widget-content fc-tue"></td>
                                                                                                <td id="w6d4b" class="fc-day fc-widget-content fc-wed"></td>
                                                                                                <td id="w6d5b" class="fc-day fc-widget-content fc-thu"></td>
                                                                                                <td id="w6d6b" class="fc-day fc-widget-content fc-fri"></td>
                                                                                                <td id="w6d7b" class="fc-day fc-widget-content fc-sat"></td>                                                                                                
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                                <div class="fc-content-skeleton">
                                                                                    <table>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <td id="w6d1n" class="fc-day-number fc-sun">31</td>
                                                                                                <td id="w6d2n" class="fc-day-number fc-mon">1</td>
                                                                                                <td id="w6d3n" class="fc-day-number fc-tue">2</td>
                                                                                                <td id="w6d4n" class="fc-day-number fc-wed">3</td>
                                                                                                <td id="w6d5n" class="fc-day-number fc-thu">4</td>
                                                                                                <td id="w6d6n" class="fc-day-number fc-fri">5</td>
                                                                                                <td id="w6d7n" class="fc-day-number fc-sat">6</td>
                                                                                            </tr>
                                                                                        </thead>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td id="w6d1d"></td>
                                                                                                <td id="w6d2d"></td>
                                                                                                <td id="w6d3d"></td>
                                                                                                <td id="w6d4d"></td>
                                                                                                <td id="w6d5d"></td>
                                                                                                <td id="w6d6d"></td>
                                                                                                <td id="w6d7d"></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- PAGE CONTENT ENDS -->
                            </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptBodyContentHolder" Runat="Server">
    <script type="text/javascript" src="assets/js/jquery-ui-1.10.3.full.min.js"></script>
    <script type="text/javascript">
        var firstDay;
        var lastDay;
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth(); //January is 0!
        var yyyy = today.getFullYear();
        var referenceDate = new Date(yyyy, mm, dd);

        function ActualMonth() {
            referenceDate = new Date();
            FillCalendar();
        }

        function NextMonth() {
            referenceDate.setDate(lastDay.getDate() + 1);
            FillCalendar();
        }

        function PreviousMonth() {
            referenceDate.setDate(firstDay.getDate() - 1);
            FillCalendar();
        }

        function FirstDayOfMonth() {
            firstDay = new Date(referenceDate.getFullYear(), referenceDate.getMonth(), 1);
            lastDay = new Date(referenceDate.getFullYear(), referenceDate.getMonth() + 1, 0);
        }

        function DayOfTheWeek(day) {
            return day.getDay();
        }

        function FillCalendar() {
            $("#ActualMonthLabel").html(MonthName[referenceDate.getMonth()] + " " + referenceDate.getFullYear());

            FirstDayOfMonth();

            // Los dias del mes anterior
            var firstWeekDay = DayOfTheWeek(firstDay);
            if (firstWeekDay == 1) { firstWeekDay = 8; }

            for (var x = 1; x < firstWeekDay; x++) {
                var myDate = new Date();
                myDate.setTime(firstDay.getTime() - (firstWeekDay - x) * 86400000);
                document.getElementById('w1d' + x + 'n').className = document.getElementById('w1d' + x + 'n').className + ' fc-other-month fc-past';
                document.getElementById('w1d' + x + 'n').innerHTML = myDate.getDate();
                document.getElementById('w1d' + x + 'd').innerHTML = '';
            }

            var lastMonthDay = firstWeekDay + lastDay.getDate() - 1;

            for (var x = firstWeekDay; x <= lastMonthDay; x++) {
                var idRow = 1;

                if (x > 35) { idRow = 6; }
                else if (x > 28) { idRow = 5; }
                else if (x > 21) { idRow = 4; }
                else if (x > 14) { idRow = 3; }
                else if (x > 7) { idRow = 2; }


                var idCell = x - (idRow - 1) * 7;
                var id = ("w" + idRow + "d" + idCell);

                if (document.getElementById("w" + idRow + "d" + idCell + "n") != null) {
                    document.getElementById("w" + idRow + "d" + idCell + "n").innerHTML = x - firstWeekDay + 1;
                    document.getElementById("w" + idRow + "d" + idCell + "n").className = "fc-day-number";
                }

                if (document.getElementById("w" + idRow + "d" + idCell + "d") != null) {
                    var task = GetTask(referenceDate, (x - firstWeekDay + 1));
                    document.getElementById("w" + idRow + "d" + idCell + "d").innerHTML = "";
                    if (task != null) {
                        document.getElementById("w" + idRow + "d" + idCell + "d").appendChild(task);
                    }
                }
            }

            for (var y = x; y < 43; y++) {
                var idRow = 1;

                if (y > 35) { idRow = 6; }
                else if (y > 28) { idRow = 5; }
                else if (y > 21) { idRow = 4; }
                else if (y > 14) { idRow = 3; }
                else if (y > 7) { idRow = 2; }


                var idCell = y - (idRow - 1) * 7;
                var id = ('w' + idRow + 'd' + idCell);
                document.getElementById('w' + idRow + 'd' + idCell + 'n').innerHTML = y - x + 1;
                document.getElementById('w' + idRow + 'd' + idCell + 'n').className = document.getElementById('w' + idRow + 'd' + idCell + 'n').className + ' fc-other-month fc-future';
                document.getElementById('w' + idRow + 'd' + idCell + 'd').innerHTML = '';
            }
        }

        function GetTask(day, counter) {
            var dayCode = referenceDate.getFullYear().toString();
            var month = referenceDate.getMonth() + 1
            if (month < 10) {
                dayCode += "0";
            }

            dayCode += month.toString();

            if (counter < 10) {
                dayCode += "0";
            }

            dayCode += (counter).toString();

            for (var t = 0; t < tasks.length; t++) {
                if (tasks[t].Expiration == dayCode) {
                    return RenderTask(tasks[t]);
                }
            }

            return null;
        }

        function RenderTask(task) {
            var href = "EquipmentView";
            var tooltip = "";
            switch (task.Type)
            {
                case "M":
                    tooltip = Dictionary.Item_EquipmentMaintenance;
                    href +='.aspx?id=' + task.Equipment.Id + "&Tab=mantenimiento&OperationId="+ task.OperationId+"&Action="+task.ActionId+"&Type=" + task.Internal;
                    break;
                case "V":
                    tooltip = Dictionary.Item_EquipmentVerification;
                    href +='.aspx?id=' + task.Equipment.Id + "&Tab=verificacion&OperationId="+ task.OperationId+"&Action="+task.ActionId+"&Type=" + task.Internal;
                    break;
                case "C":
                    tooltip = Dictionary.Item_EquipmentCalibration;
                    href +='.aspx?id=' + task.Equipment.Id + "&Tab=calibracion&OperationId="+ task.OperationId+"&Action="+task.ActionId+"&Type=" + task.Internal;
                    break;
                case "I":
                    tooltip = Dictionary.Item_Incident;
                    href = "IncidentView.aspx?id=" + task.Equipment.Id;
                    break;
                case "A":
                    tooltip = Dictionary.Item_IncidentAction;
                    href = "ActionView.aspx?id=" + task.Equipment.Id;
                    break;
                case "O":
                    tooltip = Dictionary.Item_Objetivo;
                    href = "ObjetivoView.aspx?id=" + task.Equipment.Id;
                    break;
                case "X":
                    tooltip = Dictionary.Item_Indicador;
                    href = "IndicadorView.aspx?id=" + task.Equipment.Id;
                    break;
                default:
                    tooltip = task.Type;
                    href = "ActionView.aspx?id=" + task.Equipment.Id;
                    break;

            }

            var link = document.createElement("A");
            link.href = href;

            if (task.Type == "M") {
                link.className = "fc-day-grid-event fc-event";
            }
            if (task.Type == "R") {
                link.className = "fc-day-grid-event fc-event";
            }
            if (task.Type == "C" || task.Type == "V") {
                link.className = "fc-day-grid-event fc-event";
            }
            if (task.Type == "I" || task.Type == "A") {
                link.className = "fc-day-grid-event fc-event";
            }
            if (task.Type == "X" || task.Type == "O") {
                link.className = "fc-day-grid-event fc-event";
            }

            var div = document.createElement("DIV");
            div.className = "fc-content";
            
            var span1 = document.createElement("SPAN");
            span1.className = "fc-time";
            if (task.Type.charAt(0) == "M") {
                span1.innerHTML = "<i class=\"icon-laptop\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_EquipmentMaintenance));
            }
            else if (task.Type == "R") {
                span1.innerHTML = "<i class=\"icon-laptop\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_EquipmentRepair));
            }
            else if (task.Type.charAt(0) == "C") {
                span1.innerHTML = "<i class=\"icon-laptop\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_EquipmentCalibration));
            }
            else if (task.Type.charAt(0) == "V") {
                span1.innerHTML = "<i class=\"icon-laptop\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_EquipmentVerification));
            }
            else if (task.Type == "I") {
                span1.innerHTML = "<i class=\"icon-warning-sign\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_Incident));
            }
            else if (task.Type == "A") {
                span1.innerHTML = "<i class=\"icon-tags\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_IncidentAction));
            }
            else if (task.Type == "X") {
                span1.innerHTML = "<i class=\"icon-tags\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_Indicador));
            }
            else if (task.Type == "O") {
                span1.innerHTML = "<i class=\"icon-tags\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(Dictionary.Item_Objetivo));
            }
            else {
                span1.innerHTML = "<i class=\"icon-tags\"></i>&nbsp;";
                span1.appendChild(document.createTextNode(task.Type + "*" + Dictionary.Item_IncidentAction));
            }

            var span2 = document.createElement("SPAN");
            span2.className = "fc-title";
            span2.innerHTML = task.Equipment.Description.length > 50 ? (task.Equipment.Description.substr(0,49) + "...") : task.Equipment.Description;
            link.title = task.Equipment.Description;

            div.appendChild(span1);
            div.appendChild(document.createElement("BR"));
            div.appendChild(span2);
            link.appendChild(div);
            return link;
        }

        FillCalendar();
    </script>
</asp:Content>

