<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMst.Master" AutoEventWireup="true" CodeBehind="StudentGrades.aspx.cs" Inherits="SchoolManagementSystem.Student.StudentGrades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>My Grades</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">My Grades - <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label></h1>

        <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>

        <div class="mt-3">
            <h3>All Grade Records</h3>
            <asp:GridView ID="gvExamResults" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                EmptyDataText="No grade records found">
                <Columns>
                    <asp:BoundField DataField="SubjectName" HeaderText="Subject" SortExpression="SubjectName" />
                    <asp:BoundField DataField="TotalMarks" HeaderText="Total Marks" SortExpression="TotalMarks" />
                    <asp:BoundField DataField="OutOfMarks" HeaderText="Out of Marks" SortExpression="OutOfMarks" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
