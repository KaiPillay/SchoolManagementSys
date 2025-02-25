<%@ Page Title="" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="SchoolManagementSystem.Teacher.Students" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <h3 class="text-center">View Student</h3>

            <asp:GridView ID="gvStudents" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowCommand="gvStudents_RowCommand">
                <Columns>
                    <asp:BoundField DataField="StudentID" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="DOB" HeaderText="Date of Birth" ReadOnly="True" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                    <asp:BoundField DataField="Mobile" HeaderText="Mobile" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    <asp:BoundField DataField="ClassName" HeaderText="Class" />
                    <asp:ButtonField ButtonType="Button" Text="View" CommandName="ViewDetails" HeaderText="Action" />
                </Columns>
            </asp:GridView>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtName">Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtDOB">Date of Birth</label>
                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="ddlGender">Gender</label>
                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control" Disabled="True">
                        <asp:ListItem Text="Select Gender" Value="" />
                        <asp:ListItem Text="Male" Value="Male" />
                        <asp:ListItem Text="Female" Value="Female" />
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtMobile">Mobile</label>
                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtAddress">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="ddlClass">Class</label>
                    <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" Disabled="True"></asp:DropDownList>
                </div>
            </div>

        </div>
    </div>

</asp:Content>
