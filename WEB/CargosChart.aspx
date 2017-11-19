<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CargosChart.aspx.cs" Inherits="CargosChart" %>

<!DOCTYPE html>

<html>
  <head>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        function Go(id)
        {
            top.location = '/CargosView.aspx?id=' + id;
        }
      google.load("visualization", "1", {packages:["orgchart"]});
      google.setOnLoadCallback(drawChart);
      function drawChart() {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Name');
        data.addColumn('string', 'Manager');
        data.addColumn('string', 'ToolTip');

        data.addRows(
            [
                [{ v: '36', f: '<h4>Dirección&nbsp;general</h4><br /><span style="cursor:pointer;" onclick="Go(36);">Editar</span>' }, '', 'The President'],
                [{ v: '35', f: '<h4>Promoción&nbsp;internacional</h4><br /><span style="cursor:pointer;" onclick="Go(35);">Editar</span>' }, '36', 'The President'],
                [{ v: '17', f: '<h4>Ventas&nbsp;a&nbsp;Europa</h4><br /><span style="cursor:pointer;" onclick="Go(17);">Editar</span>' }, '35', 'The President'],
                [{ v: '16', f: '<h4>Gestión&nbsp;de&nbsp;comunicación</h4><br /><span style="cursor:pointer;" onclick="Go(16);">Editar</span>' }, '36', 'The President'],
                [{ v: '54', f: '<h4>Gestión&nbsp;IT</h4><br /><span style="cursor:pointer;" onclick="Go(54);">Editar</span>' }, '36', 'The President'],
                [{ v: '55', f: '<h4>Contratación&nbsp;y&nbsp;compras&nbsp;&nbsp;IT</h4><br /><span style="cursor:pointer;" onclick="Go(55);">Editar</span>' }, '54', 'The President'],
                [{ v: '51', f: '<h4>Cargo&nbsp;sin&nbsp;responsable</h4><br /><span style="cursor:pointer;" onclick="Go(51);">Editar</span>' }, '', 'The President']
            ]);

        var chart = new google.visualization.OrgChart(document.getElementById('chart_div'));
        chart.draw(data, { allowHtml: true });
      }
   </script>
    </head>
  <body>
    <div id="chart_div"></div>
  </body>
</html>
