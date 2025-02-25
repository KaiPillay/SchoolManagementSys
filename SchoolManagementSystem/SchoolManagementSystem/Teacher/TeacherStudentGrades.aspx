<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="TeacherStudentGrades.aspx.cs" Inherits="SchoolManagementSystem.Teacher.TeacherStudentGrades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>Student Marks with Graph</h2>


        <div class="form-group">
            <label for="ddlStudents">Select Student:</label>
            <asp:DropDownList ID="ddlStudents" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged">
            </asp:DropDownList>
        </div>

        <br />


        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>

        <hr />

        <asp:GridView ID="gvExamResults" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="ExamId" HeaderText="Exam ID" SortExpression="ExamId" />
                <asp:BoundField DataField="SubjectName" HeaderText="Subject" SortExpression="SubjectName" />
                <asp:BoundField DataField="TotalMarks" HeaderText="Total Marks" SortExpression="TotalMarks" />
                <asp:BoundField DataField="OutOfMarks" HeaderText="Out of Marks" SortExpression="OutOfMarks" />
            </Columns>
            <EmptyDataTemplate>
                <p>No exam records found for this student.</p>
            </EmptyDataTemplate>
        </asp:GridView>

        <br />


        <canvas id="marksChart" width="400" height="200"></canvas>

    </div>


    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script type="text/javascript">
        var chart;

        // Function to create the chart with the fetched exam data
        function createChart(labels, data) {
            var ctx = document.getElementById('marksChart').getContext('2d');
            if (chart) {
                chart.destroy();  // Destroy existing chart if any
            }
            chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Marks Obtained',
                        data: data,
                        backgroundColor: 'rgba(54, 162, 235, 0.2)',
                        borderColor: 'rgba(54, 162, 235, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }
    </script>

</asp:Content>
