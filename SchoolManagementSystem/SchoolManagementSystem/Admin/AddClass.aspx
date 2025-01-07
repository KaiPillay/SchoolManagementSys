<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="AddClass.aspx.cs" Inherits="SchoolManagementSystem.Admin.AddClass" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:720px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
        <div class="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
            </div>
            <h3 class="text-center">New Class</h3>

            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-6">
                    <label for="txtClass">Class Name</label>
                    <asp:TextBox ID="txtClass" runat="server" CssClass="form-control" placeholder="Enter Class Name" required></asp:TextBox>
                </div>
            </div>
            <asp:Label ID="lblStatus" runat="server" ForeColor="Black" />
            <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                <div class="col-md-3 col-md-offset-2 mb-3">
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block" BackColour="#5558C9" Text="Add Class" OnClick="btnAdd_Click" />
                </div>
            </div>
                <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
                    <div class="col-md-6">
                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" DataKeyNames="ClassID" AutoGenerateColumns="False"
                        EmptyDataText="No Record To Display" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCancelingEdit="GridView1_RowCancelingEdit"
                        OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting" Width="1786px" AllowPaging="true" PageSize="4">
                        <Columns>
                            <asp:BoundField DataField="ClassID" HeaderText="Class ID" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Class">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtClassEdit" runat="server" Text='<%# Eval("ClassName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
        </div>
    </div>

</asp:Content>
