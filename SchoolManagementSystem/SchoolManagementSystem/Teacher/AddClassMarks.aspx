<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="AddClassMarks.aspx.cs" Inherits="SchoolManagementSystem.Teacher.AddClassMarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div class="container">
        <h2>Add Marks for Individual Students</h2>

        <div class="form-group">
            <label for="ddlClass">Select Class:</label>
            <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="ddlSubject">Select Subject:</label>
            <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="txtOutOfMarks">Out of Marks:</label>
            <asp:TextBox ID="txtOutOfMarks" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <asp:GridView ID="gvStudents" runat="server" CssClass="table" AutoGenerateColumns="False" DataKeyNames="StudentID">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="Name" />
                    <asp:TemplateField HeaderText="Total Marks">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMarks" runat="server" CssClass="form-control" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div class="form-group">
            <asp:Button ID="btnAddMarks" runat="server" Text="Add Marks" CssClass="btn btn-primary" OnClick="btnAddMarks_Click" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
        </div>

        <div class="form-group">
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
        </div>
    </div>
</asp:Content>
