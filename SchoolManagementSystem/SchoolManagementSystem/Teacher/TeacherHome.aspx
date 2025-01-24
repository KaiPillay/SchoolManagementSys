<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherHome.aspx.cs" Inherits="SchoolManagementSystem.Teacher.TeacherHome" MasterPageFile="~/Teacher/TeacherMst.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Teacher Home</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-4">
        <h1 class="text-center">Welcome, Teacher!</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label> 
        <asp:Label ID="lblStatus2" runat="server" CssClass="text-success"></asp:Label>
        <div class="mt-3">
            <asp:GridView ID="GridViewAttendance2" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                    <asp:BoundField DataField="Date" HeaderText="Date" />
                    <asp:BoundField DataField="Status" HeaderText="Attendance Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
