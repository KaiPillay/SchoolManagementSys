<%@ Page Title="Student Attendance Graph" Language="C#" MasterPageFile="~/Student/StudentMst.Master" AutoEventWireup="true" CodeBehind="StudentAttendanceGraph.aspx.cs" Inherits="SchoolManagementSystem.Student.StudentAttendanceGraph" %>

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
            fetch('StudentAttendanceGraph.aspx?data=true')
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

                    chartInstance = new google.visualization.ColumnChart(document.getElementById('chart_div'));
                    chartInstance.draw(data, options);

                    // Enable the export button after chart is drawn
                    document.getElementById('exportImage').disabled = false;
                })
                .catch(error => console.error('Error loading data:', error));
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
                link.download = 'attendance_graph.png';
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
        <h1 class="text-center">My Attendance Graph - <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label></h1>
        <div id="chart_div" style="width: 100%; height: 500px;"></div>
        
        <!-- Export Button -->
        <button id="exportImage" class="btn btn-primary mt-3" disabled>Export as Image</button>
    </div>
</asp:Content>
