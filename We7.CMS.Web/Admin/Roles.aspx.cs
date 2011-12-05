using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class Roles : BasePage
    {
        /// <summary>
        /// ��ǰ��������
        /// </summary>
        protected OwnerRank CurrentState
        {
            get
            {
                OwnerRank s = OwnerRank.All;
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                        s =(OwnerRank)int.Parse(Request["state"].ToString());
                }
                return s;
            }
        }

        string Keyword
        {
            get
            {
                return Request["keyword"];
            }
        }

        protected override void Initialize()
        {
            DataBinds(CurrentState);
            StateLiteral.Text = BuildStateLinks();
        }

        private string siteID;
        protected string SiteID
        {
            get
            {
                if (string.IsNullOrEmpty(siteID))
                {
                   siteID= SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
                }
                return siteID;
            }
        }

        protected void DataBinds(OwnerRank state)
        {
            string siteID = SiteID;
            List<Role> roles = AccountHelper.GetRoles(siteID, state,Keyword);
            DataGridView.DataSource = roles;
            DataGridView.DataBind();
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//�Ƿ�����ʾվ��

            string name = NameTextBox.Text;
            string id = IDTextBox.Text;

            if (We7Helper.IsNumber(id))
            {
                Messages.ShowError(name + "Ϊϵͳ��ɫ��������ɾ����");
            }
            else
            {
                try
                {
                    AccountHelper.DeleteRole(id);

                    //��¼��־
                    string content = string.Format("ɾ���˽�ɫ:��{0}��", name);
                    AddLog("��ɫ����", content);
                    Initialize();
                }
                catch (Exception ex)
                {
                    string messages = "ɾ����ɫ��" + name + "����������ԭ��" + ex.Message;
                    Messages.ShowError(messages);
                    Initialize();
                }
            }
        }

        /// <summary>
        /// ����������/״̬���˵ĳ��������ַ���
        /// </summary>
        /// <returns></returns>
        string BuildStateLinks()
        {
            string links = @"<li> <a href='Roles.aspx'   {0} >ȫ��<span class=""count"">({1})</span></a> |</li>
            <li><a href='Roles.aspx?state=0'  {2} >����Ա��ɫ<span class=""count"">({3})</span></a> |</li>
            <li><a href='Roles.aspx?state=1'  {4} >��ͨ�û���ɫ<span class=""count"">({5})</span></a> </li>";

            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            string css100, css0, css1, css2;
            css100 = css0 = css1 = css2 = "";
            if (CurrentState == OwnerRank.All) css100 = "class=\"current\"";
            if (CurrentState == OwnerRank.Admin) css0 = "class=\"current\"";
            if (CurrentState == OwnerRank.Normal) css1 = "class=\"current\"";
            links = string.Format(links, css100, AccountHelper.GetRoleCount(siteID,OwnerRank.All),
                css0, AccountHelper.GetRoleCount(siteID, OwnerRank.Admin), css1, AccountHelper.GetRoleCount(siteID,OwnerRank.Normal));

            return links;
        }

        /// <summary>
        /// ������ǰλ�õ���
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos ="��ʼ > <a >վ�����</a> >  <a href=\"Roles.aspx\" >��ɫ����</a>";
            return pos;
        }
    }
}
