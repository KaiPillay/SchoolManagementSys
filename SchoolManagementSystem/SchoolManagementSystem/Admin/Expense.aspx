<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="Expense.aspx.cs" Inherits="SchoolManagementSystem.Admin.Expense" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image:url('../Images/bg3.jpg'); width:100%; height:1200px; background-repeat: no-repeat; background-size:cover; background-attachment:fixed;">
    <div class="container p-md-4 p-sm-4">
        <div>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
        <h3 class="text-center">Manage Expenses</h3>

        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-6">
                <label for="ddlClass">Class</label>
                <asp:DropDownList ID="ddlClass" runat="server" CssClass="form-control" required>
                </asp:DropDownList>
            </div>
            <div class="col-md-6">
                <label for="ddlSubject">Subject</label>
                <asp:DropDownList ID="ddlSubject" runat="server" CssClass="form-control" required>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-6">
                <label for="txtChargeAmount">Charge Amount</label>
                <asp:TextBox ID="txtChargeAmount" runat="server" CssClass="form-control" placeholder="Enter Charge Amount" required></asp:TextBox>
            </div>
        </div>
        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-6">
                <label for="txtDetails">Details</label>
                <asp:TextBox ID="txtDetails" runat="server" CssClass="form-control" placeholder="Enter details" required></asp:TextBox>
            </div>
        </div>
        <asp:Label ID="lblStatus" runat="server" ForeColor="Black" />
        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-3 col-md-offset-2 mb-3">
                <asp:Button ID="btnAddExpense" runat="server" CssClass="btn btn-primary btn-block" Text="Add Expense" OnClick="btnAddExpense_Click" />
            </div>
        </div>
        <div class="row mb-3 mr-lg-5 ml-lg-5 mt-md-5">
            <div class="col-md-12">
                <asp:GridView ID="GridViewExpenses" runat="server" CssClass="table table-hover table-bordered" DataKeyNames="ExpenseId" AutoGenerateColumns="False"
                EmptyDataText="No Record To Display" OnPageIndexChanging="GridViewExpenses_PageIndexChanging" Width="1786px" AllowPaging="true" PageSize="4"
                OnRowDeleting="GridViewExpenses_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="ExpenseId" HeaderText="Expense ID" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Class">
                            <ItemTemplate>
                                <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ClassName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subject">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("SubjectName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ChargeAmount" HeaderText="Charge Amount" SortExpression="ChargeAmount" />
                        <asp:BoundField DataField="Details" HeaderText="Details" SortExpression="Details" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</div>

</asp:Content>
