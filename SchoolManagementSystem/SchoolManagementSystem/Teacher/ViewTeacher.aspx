<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="ViewTeacher.aspx.cs" Inherits="SchoolManagementSystem.Teacher.ViewTeacher" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="text-center">View Teachers</h3>
    <asp:Label ID="lblStatus" runat="server" ForeColor="Black"></asp:Label>

    <asp:GridView ID="gvTeachers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" OnRowCommand="gvTeachers_RowCommand">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
            <asp:TemplateField HeaderText="Photo">
                <ItemTemplate>
                    <asp:Button ID="btnViewPhoto" runat="server" Text="View Photo" CommandName="ViewPhoto" CommandArgument='<%# Eval("TeacherID") %>' CssClass="btn btn-primary" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
