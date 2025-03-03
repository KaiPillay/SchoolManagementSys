<%@ Page Title="Parent Attendance" Language="C#" MasterPageFile="~/Parents/ParentMst.Master" AutoEventWireup="true" CodeBehind="ParentAttendance.aspx.cs" Inherits="SchoolManagementSystem.Parents.ParentAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            fetch('ParentAttendance.aspx?data=true')
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

                    var chart = new google.visualization.ColumnChart(document.getElementById('attendance_chart_div'));
                    chart.draw(data, options);
                })
                .catch(error => console.error('Error loading data:', error));
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">My Child's Attendance - <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label></h1>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>

        <div class="mt-3">
            <h3>All Attendance Records</h3>
            <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                EmptyDataText="No attendance records found">
                <Columns>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:yyyy-MM-dd}" />
                </Columns>
            </asp:GridView>
        </div>

        <div id="attendance_chart_div" style="width: 100%; height: 500px;"></div>
    </div>
</asp:Content>