<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="Marks.aspx.cs" Inherits="SchoolManagementSystem.Admin.Marks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <h2>Marks Management</h2>

        <div class="form-group">
            <label for="ddlStudents">Select Student:</label>
            <asp:DropDownList ID="ddlStudents" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <br />
        <div class="form-group">
            <button type="button" class="btn btn-primary" id="btnAdd" runat="server" OnClick="btnAdd_Click">Show Marks</button>
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
    </div>
</asp:Content>
