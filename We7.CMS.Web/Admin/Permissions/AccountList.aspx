<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountList.aspx.cs" Inherits="We7.CMS.Web.Admin.AccountList"
    MasterPageFile="~/admin/theme/classic/content.Master" ValidateRequest="false" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function DeleteConfirm(id, name, state) {
            if (state == "Account") {
                var messages = "ɾ���û�����" + name + "��\r\n" +
                "�˲�����ɾ���û���Ϣ���Լ�����û���ص���Ȩ��Ϣ��\r\n" +
                "�û�һ����ɾ���������ܹ��ָ������Ƿ�ȷ��ɾ����";
                var ifConfirm = window.confirm(messages);
                if (ifConfirm) {
                    var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                    IDTextBox.value = id;
                    var NameTextBox = document.getElementById("<%=NameTextBox.ClientID %>");
                    NameTextBox.value = name;
                    var DeleteButton = document.getElementById("<%=DeleteAccountButton.ClientID %>");
                    DeleteButton.click();
                }
            }
            else {
                var messages = "ɾ�����ţ���" + name + "��\r\n" +
                "ɾ�����Ų���������ɾ���ò����Լ����������Ӳ��š��û�����Ϣ��\r\n" +
                "����һ����ɾ���������ܹ��ָ������Ƿ�ȷ��ɾ����";
                var ifConfirm = window.confirm(messages);
                if (ifConfirm) {
                    var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                    IDTextBox.value = id;
                    var NameTextBox = document.getElementById("<%=NameTextBox.ClientID %>");
                    NameTextBox.value = name;
                    var DeleteButton = document.getElementById("<%=DeleteDepartmentButton.ClientID %>");
                    DeleteButton.click();
                }
            }

        }
    </script>
    <div>
        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_user.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="�û�����">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
            </span>
        </h2>
        <div id="position">
            &nbsp;<asp:Label ID="FullPathLabel" runat="server" Text=""> </asp:Label>
        </div>
        <div class="toolbar">
            <asp:HyperLink ID="NewUserHyperLink" runat="server">
            �½��û�</asp:HyperLink>
            <span></span>
            <asp:HyperLink ID="RefreshHyperLink" runat="server">
            ˢ��</asp:HyperLink>
        </div>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
    </div>
    <!--
    <div style="display: table; width: 100%">
        <ul class="subsubsub">
            <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
        </ul>
        <p class="search-box">
            <label class="hidden" for="user-search-input">
                �����û�:</label>
            <input type="text" class="search-input" id="KeyWord" name="KeyWord" value="" />
            <input type="button" value="�����û�" class="button" id="SearchButton" />
        </p>
    </div>
   
    <div style="min-height: 35px;">
        <asp:GridView ID="AccountsGridView" runat="server" AutoGenerateColumns="False" CssClass="List"
            GridLines="Horizontal" ShowFooter="True">
            <AlternatingRowStyle CssClass="alter" />
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                <asp:BoundField DataField="Mode" HeaderText="ģʽ" Visible="False" />
                <asp:ImageField DataImageUrlField="Mode" DataImageUrlFormatString="/admin/images/icon_{0}.gif">
                    <HeaderStyle Width="20px" />
                </asp:ImageField>
                <asp:HyperLinkField DataNavigateUrlFields="Url" DataNavigateUrlFormatString="{0}"
                    DataTextField="Text" DataTextFormatString="{0}" HeaderText="����">
                    <HeaderStyle Width="200px" />
                </asp:HyperLinkField>
                <asp:BoundField HeaderText="��������" DataField="Summary"></asp:BoundField>
                <asp:BoundField HeaderText="����" DataField="State"></asp:BoundField>
                <asp:BoundField HeaderText="ע������" DataField="RegisterDate"></asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <%# Eval("ManageLinks")%>
                        <a href="<%# Eval("EditUrl") %>">�༭</a> <a href="<%# Eval("DeleteUrl") %>">ɾ��</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="pagination">
        <WEC:URLPager ID="UPager" runat="server" UseSpacer="False" UseFirstLast="true" PageSize="10"
            FirstText="<< ��ҳ" LastText="βҳ >>" LinkFormatActive='<span class=Current>{1}</span>'
            UrlFormat="Departments.aspx?pg={0}" CssClass="Pager" />
    </div>
    <div style="display: none">
        <input type="submit" id="DoNothingButton" onclick="return false" />
        <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="DeleteDepartmentButton" runat="server" OnClick="DeleteDepartmentButton_Click" />
        <asp:Button ID="DeleteAccountButton" runat="server" OnClick="DeleteAccountButton_Click" />
    </div>-->
    <script type="text/javascript" src="/scripts/we7/we7.loader.js">
	
		// ��������
		var FromSiteID = new we7.BindCondition("FromSiteID", we7.bindVerb.equals, "<%=SiteID%>");
       
		var CurrentState=null;
         <%if (CurrentState!=OwnerRank.All)
       {%>
         CurrentState= new we7.BindCondition("UserType", we7.bindVerb.equals, "<%=(int)CurrentState%>");
       <%}%>
		// ����Ҫ�󶨵���Դ��Ŀ��
		var bindDestination = new we7.BindOption({
			tableName: "Account"
		    , fields: { "ID":{},"LoginName":{},"Department":{},"ModelState":{},"Created":{},"UserType":{value:{"1":"��ͨ�û�","0":"�����û�"}}}
			, sortField: "Created"
			, sortOrder: "desc"
            , rows:10
		});
		bindDestination.conditions.push(FromSiteID);
        bindDestination.conditions.push(CurrentState);
		
		//�󶨹���
		function bindData(){
			var options = {
				caption: "�û��б�",
				height: 220,
                rowNum:10,
                autowidth:true,
                editColumnHeader:"����",
                deletableRow:true
			};
			
			we7("#ModelList").bind(bindDestination, options);
		}
		
		$(document).ready(function () {
			
			bindData();
			
		});
    </script>
    <table id="ModelList" style="display:none">
    <tr><td header="����"><img src="/admin/images/icon_User.gif" style="border-width:0px;"><a href="AccountEdit.aspx?id=${ID}">${LoginName}</a></td><td header="��������">${Department}</td><td editable="select" header="����" editkey="UserType">{{if UserType==0}}�����û�{{else}}��ͨ�û� {{/if}}</td><td header="ע������">{{html Created.substr(0,10)}}</td><td header="����"><a href="AccountEdit.aspx?id=${ID}&tab=2">��ɫ����</a>  <a href="AccountEdit.aspx?id=${ID}&tab=6">ģ��Ȩ��</a></td>
    </tr>
    </table>
    <script type="text/javascript">
        $(function () {
            $('#KeyWord').bind('keyup', function (event) {
                if (event.keyCode == 13) {
                    window.location = "AccountList.aspx?keyword=" + encodeURIComponent(this.value);
                }
            });
            $('#SearchButton').click(function () {
                window.location = "AccountList.aspx?keyword=" + encodeURIComponent($('#KeyWord').val());
            });
            if (QueryString('keyword'))
                $('#KeyWord').val(QueryString('keyword'));
        });
    </script>
</asp:Content>
