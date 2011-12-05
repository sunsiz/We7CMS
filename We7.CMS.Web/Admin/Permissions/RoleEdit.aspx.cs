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
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class RoleEdit : BasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }

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
                tabString += string.Format(strActive, 1, "������Ϣ", dispay);
                Control ctl = this.LoadControl("../Permissions/Role_Basic.ascx");
                ContentHolder.Controls.Add(ctl);
            }
            else
                tabString += string.Format(strLink, 1, "������Ϣ", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

            if (RoleID != null)
            {
                if (r != null)
                    dispay = "";
                else
                    dispay = "none";

                if (tab == 2)
                {
                    tabString += string.Format(strActive, 2, "�����û�", dispay);
                    Control ctl = this.LoadControl("../Permissions/Role_Accounts.ascx");
                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 2, "�����û�", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));

                if (tab == 3)
                {
                    tabString += string.Format(strActive, 3, "ģ��Ȩ��", dispay);

                    //��̬�ؼ�����
                    Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
                    ctl.OwnerType = "role";
                    ctl.OwnerID = r.ID;

                    if ((OwnerRank)r.RoleType == OwnerRank.Admin)
                    {
                        ctl.ObjectID = "System.Administration";
                        ctl.EntityID = "System.Administration";
                    }
                    else
                    {
                        ctl.ObjectID = "System.User";
                        ctl.EntityID = "System.User";
                    }

                    ContentHolder.Controls.Add(ctl);
                }
                else
                    tabString += string.Format(strLink, 3, "ģ��Ȩ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));


            //    dispay = "";

                //if (tab == 4)
                //{
                //    tabString += string.Format(strActive, 4, "����Ȩ��", dispay);

                //    //��̬�ؼ�����
                //    Permission_Func ctl = this.LoadControl("../Permissions/Permission_Func.ascx") as Permission_Func;
                //    ctl.OwnerType = "role";
                //    ctl.OwnerID = r.ID;
                //    ctl.ObjectID = Helper.EmptyGUID;
                //    ctl.EntityID = "System.Function";
                //    ContentHolder.Controls.Add(ctl);
                //}
                //else
                //    tabString += string.Format(strLink, 4, "����Ȩ��", dispay, Helper.AddParamToUrl(rawurl, "tab", "4"));

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

            if (RoleID != null)
            {
                Role r = AccountHelper.GetRole(RoleID);

                if (r != null)
                {
                    pos = "��ʼ > <a >վ�����</a> >  <a href=\"../Roles.aspx\" >��ɫ����</a> >  <a>�༭��ɫ��"
                        + r.Name + "��</a>";
                }
            }
            else
            {
                pos = "��ʼ > <a >վ�����</a> >  <a href=\"../Roles.aspx\" >��ɫ����</a> >  <a>�����½�ɫ</a>";
            }

            return pos;

        }
    }
}