<%@ Page Title="" Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        function num_key(evt) {
            evt = evt || windows.event;
            var k = evt.keyCode || evt.which;
            return (k > 47 && k < 58 || k == 8 || k == 9)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_title" Runat="Server">
    ข้อมูลโครงการ
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_content" Runat="Server">
    <asp:UpdatePanel ID="upDefault" runat="server">
        <ContentTemplate>
            
            <asp:Panel ID="pnlMasterForm" runat="server" DefaultButton="btnSave">
                <table cellpadding="5px" width="100%">
                    <tr>
                        <td class="r" style="width:150px;">ชื่อโครงการ <span class="require">*</span> : </td>
                        <td>
                            <asp:TextBox ID="txtProjectName" runat="server" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="r">ชื่อเจ้าของโครงการ <span class="require">*</span> : </td>
                        <td><asp:TextBox ID="txtProjectOwner" runat="server" Width="100%"></asp:TextBox></td>
                    </tr>--%>
                    <tr>
                        <td class="r">ค่ามิเตอร์น้ำ : </td>
                        <td><asp:TextBox ID="txtMeterUnit" runat="server" Width="100px" onkeypress="return num_key(event);"></asp:TextBox> บาท/หน่วย</td>
                    </tr>
                    <tr>
                        <td class="r">ค่าบริการ : </td>
                        <td><asp:TextBox ID="txtService" runat="server" Width="100px" onkeypress="return num_key(event);"></asp:TextBox> บาท</td>
                    </tr>
                    <tr>
                        <td colspan="2"><div class="line"></div></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="c">
                            <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_click" OnClientClick="return confirm('ต้องการบันทึก ???')" />
                            <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><div class="line"></div></td>
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
                        <asp:TemplateField HeaderText="ชื่อโครงการ">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnProjectName" runat="server" CommandName="bEdit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="ProjectOwner" HeaderText="ชื่อเจ้าของโครงการ" SortExpression="ProjectOwner" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="MeterRate" HeaderText="ค่ามิเตอร์น้ำ(บาท/หน่วย)" SortExpression="MeterRate" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ServiceRate" HeaderText="ค่าบริการ" SortExpression="ServiceRate" ItemStyle-HorizontalAlign="Center" />
                    </Columns>
                </asp:GridView>
                <div class="cb"></div>
            </asp:Panel>
            <asp:Literal ID="ltAlert" runat="server"></asp:Literal>
            <div class="line"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

