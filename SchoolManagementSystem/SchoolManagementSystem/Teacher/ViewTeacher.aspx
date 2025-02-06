<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="ViewTeacher.aspx.cs" Inherits="SchoolManagementSystem.Teacher.ViewTeacher" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="text-center">View Teachers</h3>
    <asp:Label ID="lblStatus" runat="server" ForeColor="Black"></asp:Label>

    <asp:GridView ID="gvTeachers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">
        <Columns>
            <asp:BoundField DataField="TeacherId" HeaderText="Teacher ID" SortExpression="TeacherId" />
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="DOB" HeaderText="Date of Birth" SortExpression="DOB" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
            <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
        </Columns>
    </asp:GridView>
</asp:Content>
