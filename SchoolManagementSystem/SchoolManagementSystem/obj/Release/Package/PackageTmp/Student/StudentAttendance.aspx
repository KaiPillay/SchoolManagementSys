<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMst.Master" AutoEventWireup="true" CodeBehind="StudentAttendance.aspx.cs" Inherits="SchoolManagementSystem.Student.StudentAttendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>My Attendance</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">My Attendance - <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label></h1>

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
    </div>
</asp:Content>
