<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="ViewStudentAttendance.aspx.cs" Inherits="SchoolManagementSystem.Admin.ViewStudentAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <h3 class="text-center">View Student Attendance</h3>

            <asp:Label ID="lblStatus" runat="server" ForeColor="Red" CssClass="alert alert-danger" Visible="false"></asp:Label>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="ddlStudent">Select Student</label>
                    <asp:DropDownList ID="ddlStudent" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlStudent_SelectedIndexChanged" AutoPostBack="true" required>
                        <asp:ListItem Text="Select Student" Value="" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-12">
                    <asp:GridView ID="gvAttendance" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false" EmptyDataText="No records found">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Student Name" SortExpression="Name" />
                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
