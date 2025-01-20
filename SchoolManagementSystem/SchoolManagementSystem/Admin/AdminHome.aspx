<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="SchoolManagementSystem.Admin.AdminHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblStatus" runat="server" ForeColor="Green"></asp:Label>
            </div>

            <h2 class="text-center">Admin Home Page</h2>

            <div class="row">
                <div class="col-md-3">
                    <asp:Label ID="lblClassCount" runat="server" CssClass="h4"></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblStudentCount" runat="server" CssClass="h4"></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblSubjectCount" runat="server" CssClass="h4"></asp:Label>
                </div>
                <div class="col-md-3">
                    <asp:Label ID="lblTeacherCount" runat="server" CssClass="h4"></asp:Label>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-md-6">
                    <asp:Label ID="lblTotalFees" runat="server" CssClass="h4"></asp:Label>
                </div>
            </div>

            <div class="mt-5">
                <h3>Latest Student Attendance</h3>
                <asp:GridView ID="GridViewAttendance" runat="server" AutoGenerateColumns="false" CssClass="table table-striped">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="Name" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>
</asp:Content>
