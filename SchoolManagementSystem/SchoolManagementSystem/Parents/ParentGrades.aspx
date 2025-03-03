<%@ Page Title="Parent Grades" Language="C#" MasterPageFile="~/Parents/ParentMst.Master" AutoEventWireup="true" CodeBehind="ParentGrades.aspx.cs" Inherits="SchoolManagementSystem.Parents.ParentGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {
            fetch('ParentGrades.aspx?data=true')
                .then(response => response.json())
                .then(jsonData => {
                    var data = new google.visualization.DataTable();
                    data.addColumn('string', 'Subject');
                    data.addColumn('number', 'Total Marks');

                    jsonData.forEach(row => {
                        data.addRow([row.SubjectName, row.TotalMarks]);
                    });

                    var options = {
                        title: 'Grades Overview',
                        hAxis: { title: 'Subjects' },
                        vAxis: { title: 'Marks' },
                        legend: { position: 'top' }
                    };

                    var chart = new google.visualization.ColumnChart(document.getElementById('grades_chart_div'));
                    chart.draw(data, options);
                })
                .catch(error => console.error('Error loading data:', error));
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">My Child's Grades - <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label></h1>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>

        <div class="mt-3">
            <h3>All Grade Records</h3>
            <asp:GridView ID="gvGrades" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                EmptyDataText="No grade records found">
                <Columns>
                    <asp:BoundField DataField="SubjectName" HeaderText="Subject" SortExpression="SubjectName" />
                    <asp:BoundField DataField="TotalMarks" HeaderText="Total Marks" SortExpression="TotalMarks" />
                    <asp:BoundField DataField="OutOfMarks" HeaderText="Out of Marks" SortExpression="OutOfMarks" />
                </Columns>
            </asp:GridView>
        </div>

        <div id="grades_chart_div" style="width: 100%; height: 500px;"></div>
    </div>
</asp:Content>