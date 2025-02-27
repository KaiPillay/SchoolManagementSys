<%@ Page Title="" Language="C#" MasterPageFile="~/Parents/ParentMst.Master" AutoEventWireup="true" CodeBehind="ParentHome.aspx.cs" Inherits="SchoolManagementSystem.Parents.ParentHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Student Home</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">Welcome, <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label>!</h1>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
        <asp:Label ID="lblStatus" runat="server" CssClass="text-success"></asp:Label>

        <div class="mt-3">
            <h3>Your Recent Attendance Records</h3>
            <asp:GridView ID="GridViewAttendance" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" EmptyDataText="No attendance records found">
                <Columns>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:yyyy-MM-dd}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
