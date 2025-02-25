<%@ Page Title="Student Grades Chart" Language="C#" MasterPageFile="~/Student/StudentMst.Master" AutoEventWireup="true" CodeBehind="StudentGradesChart.aspx.cs" Inherits="SchoolManagementSystem.Student.StudentGradesChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Exam/Subject');
            data.addColumn('number', 'Total Marks');

            var chartData = <%= ChartData %>;
            data.addRows(chartData);

            var options = {
                title: 'Student Grades',
                width: 800,
                height: 500,
                hAxis: { title: 'Exam/Subjects' },
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
        <h1 class="text-center">My Grades Chart</h1>
        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
        
        <div class="form-group">
            <label for="ddlSubjects">Select Subject:</label>
            <asp:DropDownList ID="ddlSubjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
        </div>
        
        <div id="chart_div" style="width: 100%; height: 500px;"></div>
    </div>
</asp:Content>
