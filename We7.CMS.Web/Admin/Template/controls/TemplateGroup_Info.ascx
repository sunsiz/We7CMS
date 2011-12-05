<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateGroup_Info.ascx.cs"
    Inherits="We7.CMS.Web.Admin.TemplateGroup_Info" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
    
    <script type="text/javascript">
        function SaveButtonClick() {
            document.getElementById("<%=SaveButton.ClientID %>").click();
        }

        function Register() {
            parent.UxEvents.reload = 1;
        }
    </script>
    
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <table>
        <tr>
            <th style="width: 20%">
                ��Ŀ
            </th>
            <th style="width: 443px">
                ����
            </th>
        </tr>
        <tr>
            <td>
                ���ƣ�
            </td>
            <td style="width: 443px">
                <asp:TextBox ID="NameTextBox" runat="server" Text="" Columns="50" MaxLength="64" CssClass="required"></asp:TextBox>
                <asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ControlToValidate="NameTextBox"
                    ErrorMessage="����ָ�����ƣ�" ValidationGroup="Detail" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="NameRegularExpressionValidator" runat="server"
                    Display="Dynamic" ErrorMessage="RegularExpressionValidator" ValidationExpression="^[a-zA-Z0-9_-\u4e00-\u9fa5]+$"
                    ControlToValidate="NameTextBox" ValidationGroup="Detail">��������ƷǷ�</asp:RegularExpressionValidator><br />
                &nbsp;<asp:Image ID="TNameImage" runat="server" ImageUrl="~/admin/Images/icon_warning.gif"
                    Visible="false" />
                <asp:Label ID="TNameLabel" runat="server" Text="" Style="position: relative" Width="162px"
                    Visible="False" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <span class="note">ģ��������ֻ��ΪӢ����ĸ���ֺ��»��ߣ�һ�����潫�����ٸ��ġ� </span>
            </td>
        </tr>
        <tr>
            <tr>
                <td>
                    ������
                </td>
                <td style="width: 443px">
                    <asp:TextBox ID="DescriptionTextBox" runat="server" Text="" TextMode="MultiLine"
                        Columns="40" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    ���ڣ�
                </td>
                <td style="width: 443px">
                    <asp:Label ID="CreatedLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    �ϴ�Ԥ��ͼ��
                </td>
                <td style="width: 443px">
                    <asp:FileUpload ID="PreviewFileUploador" runat="server" Style="position: relative" />
                    <asp:Panel ID="MessagePanel" runat="server" Style="position: relative" Visible="false">
                        <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_warning.gif" />
                        <asp:Label ID="MessageLabel" runat="server" Text="Ԥ��Ч��ͼƬ�ĸ�ʽΪjpg�ļ�."></asp:Label>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    �汾�ţ�
                </td>
                <td style="width: 443px">
                    V2.7
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="toolbar">
                    <br />
                    <li class="smallButton4">
                        <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:SaveButtonClick();" runat="server">��  ��</asp:HyperLink></li>
                </td>
            </tr>
    </table>
</div>
<div style="display: none">
    <asp:TextBox ID="FileTextBox" runat="server"></asp:TextBox>
    <asp:TextBox ID="TemplateTextBox" runat="server"></asp:TextBox>
    <asp:TextBox ID="DetailTemplateTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
</div>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" />

