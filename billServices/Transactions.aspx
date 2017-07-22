<%@ Page Title="" Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Transactions.aspx.cs" Inherits="Transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_title" Runat="Server">
    รายการ มิเตอร์น้ำ
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_content" Runat="Server">
    <asp:UpdatePanel ID="upDefault" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMasterForm" runat="server">
                <table cellpadding="5px" width="100%">
                    <tr>
                        <td class="r" style="width:150px;">ชื่อโครงการ <span class="require">*</span> : </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlProjectName" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlProjectName_Changed"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"><div class="line"></div></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlGVListProject" runat="server">
                <asp:GridView ID="gvListProject" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid" PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt" 
                    AllowPaging="True" PageSize="20" ForeColor="#333333" GridLines="None"
                    OnRowCommand="gvListProject_RowCommand" OnRowDataBound="gvListProject_DataBound"
                    OnPageIndexChanging="gvListProject_PageIndexChanging" >
                    <Columns>
                        <asp:TemplateField HeaderText="บ้านเลขที่ ">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnNo" runat="server" CommandName="bEdit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CName" HeaderText="ชื่อลูกค้า" SortExpression="CName" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ProjectName" HeaderText="ชื่อโครงการ" SortExpression="ProjectName" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                </asp:GridView>
                <div class="cb"></div>
            </asp:Panel>
            <asp:Literal ID="ltAlert" runat="server"></asp:Literal>
            <div class="line"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

