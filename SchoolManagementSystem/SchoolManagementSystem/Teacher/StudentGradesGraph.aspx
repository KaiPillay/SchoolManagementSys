<%@ Page Title="Teacher Grades Chart" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="StudentGradesGraph.aspx.cs" Inherits="SchoolManagementSystem.Teacher.StudentGradesGraph" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Student/Subject');
            data.addColumn('number', 'Total Marks');

            var chartData = <%= ChartData %>;
            data.addRows(chartData);

            var options = {
                title: 'Student Grades by Subject',
                width: 800,
                height: 500,
                hAxis: { title: 'Student/Subject' },
                vAxis: { title: 'Marks' },
                legend: 'none'
            };

            var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">Grades Chart for My Students</h1>
        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
        
        <div class="form-group">
            <label for="ddlSubjects">Select Subject:</label>
            <asp:DropDownList ID="ddlSubjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged" CssClass="form-control">
                <asp:ListItem Text="All Subjects" Value="" />
            </asp:DropDownList>
        </div>
        
        <div id="chart_div" style="width: 100%; height: 500px;"></div>
    </div>
</asp:Content>
