<%@ Page Title="" Language="C#" MasterPageFile="~/MP.master" AutoEventWireup="true" CodeFile="Customers.aspx.cs" Inherits="Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style>
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }
        .center
        {
            z-index: 1000;
            margin: 300px auto;
            padding: 5px;
            width: 50px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }
        .center img
        {
            height: 50px;
            width: 50px;
        }
    </style>

    <script src="js/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script>
        function num_key(evt) {
            evt = evt || windows.event;
            var k = evt.keyCode || evt.which;
            return (k > 47 && k < 58 || k == 8 || k == 9)
        }
        
        function setNextInput(evt, el) {
            evt = evt || windows.event;
            var k = evt.keyCode || evt.which;
            if (k == 13) {
                var $el = $(el);
                var $tr = $el.closest('tr');
                if ($tr.length) {

                    var $sp = $tr.find('.txtTotal')
                    if ($sp.length) {
                        var _mr = $('#<%=hdfMR.ClientID%>').val();
                        var _sr = $('#<%=hdfSR.ClientID%>').val();
                        var _m = $el.val();
                        var _t = (+_m * +_mr) + +_sr;
                        console.log(_t, $sp)
                        $sp.html(_t);
                    }

                    var $next = $tr.next();
                    if ($next.length) {
                        $next.find('.txtMN').focus();
                    }
                }
            }
        }

        function confirmPrint() {
            document.getElementById('pnlLoading').style.display = 'block';
            var _c = confirm('ต้องการบันทึกเลขมิเตอร์/Print ???');
            return _c;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_title" Runat="Server">
    ข้อมูลลูกค้า
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_content" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlMasterForm" runat="server" DefaultButton="btnSave">
                <table cellpadding="5px" width="100%">
                    <tr>
                        <td class="r" style="width:150px;">ชื่อโครงการ <span class="require">*</span> : </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlProjectName" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlProjectName_Changed"></asp:DropDownList>
                            <asp:HiddenField ID="hdfMR" runat="server" />
                            <asp:HiddenField ID="hdfSR" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="r">บ้านเลขที่  : </td>
                        <td><asp:TextBox ID="txtNo" runat="server" Width="100px"></asp:TextBox></td>
                        <td class="r">ชื่อลูกค้า <span class="require">*</span> : </td>
                        <td><asp:TextBox ID="txtCustName" runat="server" Width="100%"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="r">เลขมิเตอร์น้ำปัจจุบัน <span class="require">*</span>  : </td>
                        <td><asp:TextBox ID="txtMeterNo" runat="server" Width="60px" CssClass="r" onkeypress="return num_key(event);"></asp:TextBox> หน่วย</td>
                        <td class="r"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4"><div class="line"></div></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="c">
                            <asp:Button ID="btnSave" runat="server" Text="บันทึก" OnClick="btnSave_click" OnClientClick="return confirm('ต้องการบันทึก ???')" />
                            <asp:Button ID="btnCancel" runat="server" Text="Clear" OnClick="btnCancel_click" />
                            <div style="float:right;">
                                <asp:Button ID="btnPrint" runat="server" Text="Save / Print" OnClick="btnPrint_click" OnClientClick="confirmPrint();" />
                            </div>
                            <div style="clear:both;"></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"><div class="line"></div></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlGVListProject" runat="server">
                <div class="r">
                    <label>
                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkAll_Changed" />
                        Select All
                    </label>
                </div>
                
                <asp:Literal ID="ltAlert" runat="server"></asp:Literal>
                
                <asp:GridView ID="gvListProject" runat="server" AutoGenerateColumns="false"
                    CssClass="mGrid" PagerStyle-CssClass="pgr"  AlternatingRowStyle-CssClass="alt" 
                    AllowPaging="False" PageSize="20" ForeColor="#333333" GridLines="None"
                    OnRowCommand="gvListProject_RowCommand" OnRowDataBound="gvListProject_DataBound"
                    OnPageIndexChanging="gvListProject_PageIndexChanging" >
                    <Columns>
                        <asp:BoundField DataField="CID" HeaderText="-" SortExpression="CID" Visible="false"/>
                        <%--<asp:TemplateField HeaderText="บ้านเลขที่ ">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnNo" runat="server" CommandName="bEdit"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="ชื่อลูกค้า ">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnCName" runat="server" CommandName="bEdit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="CName" HeaderText="ชื่อลูกค้า" SortExpression="CName" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="MeterDate" HeaderText="วันที่จดล่าสุด" SortExpression="MeterDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px"/>
                        <asp:TemplateField HeaderText="เลขมิเตอร์น้ำปัจจุบัน">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMeterCurrent" runat="server" Width="70px" CssClass="r" Enabled="false"></asp:TextBox>
                                <asp:LinkButton ID="MRate" runat="server" Visible="false"></asp:LinkButton>
                                <asp:LinkButton ID="SRate" runat="server" Visible="false"></asp:LinkButton>
                                <asp:LinkButton ID="MDate" runat="server" Visible="false"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="120px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="เลขมิเตอร์น้ำ">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMeterNow" runat="server" Width="70px" CssClass="r txtMN" onkeypress="return num_key(event);" onkeydown="setNextInput(event, this);"></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="รวม">
                            <ItemTemplate>
                                <span class="txtTotal"></span>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="90px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Print">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkPrint" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="cb"></div>
            </asp:Panel>
            
            <div class="line"></div>
            
            <asp:Literal ID="ltScript" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

