<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="StudsentAttendanceGraph.aspx.cs" Inherits="SchoolManagementSystem.Teacher.StudsentAttendanceGraph" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });

        function drawChart() {
            var studentId = document.getElementById('<%= ddlStudents.ClientID %>').value;
            if (!studentId) {
                alert("Please select a student.");
                return;
            }

            fetch(`StudsentAttendanceGraph.aspx?data=true&studentId=${studentId}`)

                .then(response => response.json())
                .then(jsonData => {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Date');
                    data.addColumn('number', 'Present');
                    data.addColumn('number', 'Absent');

                    jsonData.forEach(row => {
                        data.addRow([row.Date, row.Present, row.Absent]);
                    });

                    var options = {
                        title: 'Attendance Overview',
                        hAxis: { title: 'Date' },
                        vAxis: { title: 'Count' },
                        legend: { position: 'top' },
                        colors: ['#28a745', '#dc3545']
                    };

                    var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
                    chart.draw(data, options);
                })
                .catch(error => console.error('Error loading data:', error));
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">Student Attendance Graph</h1>
        
        <asp:DropDownList ID="ddlStudents" runat="server" AutoPostBack="false" CssClass="form-control mb-3"></asp:DropDownList>
        
        <button type="button" class="btn btn-primary" onclick="drawChart()">Show Attendance</button>

        <div id="chart_div" style="width: 100%; height: 500px;"></div>
    </div>
</asp:Content>