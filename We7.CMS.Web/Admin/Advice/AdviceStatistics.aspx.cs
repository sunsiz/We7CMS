using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.PF;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceStatistics : BasePage
    {

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string RoleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MenuTabLabel.Text = BuildNavString();
            PagePathLiteral.Text = BuildPagePath();
        }

        /// <summary>
        /// ������ǩ���ȫ����ͨ������ÿ�����display����
        /// ���й�����Ŀ��ƣ���������ȫ���Խ�����ͬģ��ʽ�Ĺ���
        /// ���ҳ�桢Tab���ơ�Tab��ʾ���ԡ�Tab�����ؿؼ�
        /// �����Կ�������Ӧ����ʾ
        /// </summary>
        /// <returns></returns>
        string BuildNavString()
        {
            string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
            string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
            int tab = 1;
            string tabString = "";
            string dispay = "";
            string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
            rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

            if (TabID != null && We7Helper.IsNumber(TabID))
                tab = int.Parse(TabID);

            Role r = AccountHelper.GetRole(RoleID);

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "�ظ�ͳ��", dispay);
                Control ctl = this.LoadControl("../Advice/controls/AdviceReplyStatisticsControl.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "�ظ�ͳ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));
            return tabString;
        }

        /// <summary>
        /// ������ǰλ�õ���
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "��ʼ > <a >����</a> >  <a href=\"../Advice/AdviceStatistics.aspx\" >������Ϣͳ��</a>";
            return pos;
        }
    }
}
