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
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin.Permissions
{
    public partial class AccountEdit : BasePage
    {

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        public string TabID
        {
            get { return Request["tab"]; }
        }

        public string ActID
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

            if (tab == 1)
            {
                tabString += string.Format(strActive, 1, "������Ϣ", dispay);
                Control ctl = this.LoadControl("../Permissions/Account_Basic.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "������Ϣ", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (ActID != null)
            {
                Account acc = AccountHelper.GetAccount(ActID, new string[] { "LoginName", "UserType" });
                string actName = acc.LoginName;

                if (acc != null)
                    dispay = "";
                else
                    dispay = "none";

                if (tab == 4)
                {
                    tabString += string.Format(strActive, 4, "ѡ��", dispay);
                    Control ctl = this.LoadControl("../Permissions/Account_Extend.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 4, "ѡ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));

                if (tab == 2)
                {
                    tabString += string.Format(strActive, 2, "��ɫ����", dispay);
                    Account_Roles ctl = this.LoadControl("../Permissions/Account_Roles.ascx") as Account_Roles ;
                    if (acc.UserType == (int)OwnerRank.Admin)
                        ctl.RoleType = OwnerRank.All;
                    else
                        ctl.RoleType = (OwnerRank)acc.UserType;
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 2, "��ɫ����", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));

                if (tab == 3)
                {
                    tabString += string.Format(strActive, 3, "��Ա�˵�", dispay);
                    //��̬�ؼ�����
                    Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
                    ctl.OwnerType = "account";
                    ctl.OwnerID = ActID;
                    ctl.ObjectID = "System.User";
                    ctl.EntityID = "System.User";
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 3, "��Ա�˵�", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));

                //if (acc.UserType == (int)OwnerRank.Admin) 
                if (Security.CurrentAccountID.Equals(We7Helper.EmptyGUID))//����޸ĺ����˼��ֻ�г�������Ա�������ù���Ա
                {
                    if (tab == 6)
                    {
                        tabString += string.Format(strActive, 6, "��̨�˵�", dispay);
                        //��̬�ؼ�����
                        Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
                        ctl.OwnerType = "account";
                        ctl.OwnerID = ActID;
                        ctl.ObjectID = "System.Administration";
                        ctl.EntityID = "System.Administration";
                        ContentHolder.Controls.Add(ctl);
                    }
                    else
                        tabString += string.Format(strLink, 6, "��̨�˵�", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "6"));
                }

                if (tab == 5)
                {
                    tabString += string.Format(strActive, 5, "����", dispay);
                    Account_Points ctl = this.LoadControl("../Permissions/Account_Points.ascx") as Account_Points;
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 5, "����", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "5"));
            }

            return tabString;
        }

        /// <summary>
        /// ������ǰλ�õ���
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "";

            if (ActID != null)
            {
                string actName = AccountHelper.GetAccount(ActID, new string[] { "LoginName" }).LoginName;

                if (actName != string.Empty)
                {
                    pos = "<a href='/admin/'>����̨</a> > <a >�û�</a> >  <a href='AccountList.aspx'>�û�����</a> >  <a>�༭�û�<b>"
                        + actName + "</b> </a>";
                }
            }
            else
            {
                pos = "<a href='/admin/'>����̨</a> > <a >�û�</a> >  <a href='AccountList.aspx'>�û�����</a> >  <a>�������û�</a>";
            }

            return pos;

        }
    }
}