<%@ Page Title="Teacher Grades Chart" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="TeacherGradesChart.aspx.cs" Inherits="SchoolManagementSystem.Teacher.TeacherGradesChart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Google Charts Library -->
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    
    <script type="text/javascript">
        var chartInstance = null; // Store chart instance globally

        // Load Google Charts
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        // Function to draw the chart
        function drawChart() {
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Exam/Subject');
            data.addColumn('number', 'Total Marks');

            var chartData = <%= ChartData %>;
            data.addRows(chartData);

            var options = {
                title: 'All Students Grades',
                width: 800,
                height: 500,
                hAxis: { title: 'Exam/Subjects' },
                vAxis: { title: 'Marks' },
                legend: 'none'
            };

            chartInstance = new google.visualization.ColumnChart(document.getElementById('chart_div'));
            chartInstance.draw(data, options);

            // Enable the export button after chart is drawn
            document.getElementById('exportImage').disabled = false;
        }

        // Function to export the chart as an image
        document.addEventListener("DOMContentLoaded", function () {
            document.getElementById('exportImage').addEventListener('click', function (event) {
                event.preventDefault(); // Prevent form submission

                if (!chartInstance) {
                    alert("Chart is not available yet. Please wait.");
                    return;
                }

                // Get the chart image
                const imgData = chartInstance.getImageURI();

                if (!imgData) {
                    alert("Failed to generate image. Please try again.");
                    return;
                }

                // Create a download link
                const link = document.createElement('a');
                link.href = imgData;
                link.download = 'all_students_grades_chart.png';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

                alert('Chart exported successfully. Check your downloads folder.');
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">All Students Grades Chart</h1>
        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
        
        <!-- Subject Dropdown -->
        <div class="form-group">
            <label for="ddlSubjects">Select Subject:</label>
            <asp:DropDownList ID="ddlSubjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
        </div>
        
        <!-- Chart Container -->
        <div id="chart_div" style="width: 100%; height: 500px;"></div>
        
        <!-- Export Button -->
        <button id="exportImage" class="btn btn-primary mt-3" disabled>Export as Image</button>
    </div>
</asp:Content>