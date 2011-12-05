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
using We7.CMS.Config;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.CMS.Accounts;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class UpdatePassword : BasePage
    {

        protected override bool NeedAnAccount
        {
            get { return false; }
        }

        
        void ShowMessage(string m)
        {
            MessageLabel.Text = m;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!We7Helper.IsEmptyID(AccountID))
                {
                    LoginNameTextBox.Text = AccountHelper.GetAccount(AccountID, new string[] { "LoginName" }).LoginName;
                }
                else
                    LoginNameTextBox.Text = SiteConfigs.GetConfig().AdministratorName;
            }
        }

        void changePassword(string loginName, string password,string newPassword)
        {

            if (AccountID==We7Helper.EmptyGUID && String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0)
            {
                if (CDHelper.AdminPasswordIsValid(password))
                {
                   SiteConfigInfo si = SiteConfigs.GetConfig();
                    bool isHashed =si.IsPasswordHashed ;
                    if (isHashed != IsHashedPasswordCheckBox.Checked)
                        si.IsPasswordHashed = IsHashedPasswordCheckBox.Checked;
                    if(IsHashedPasswordCheckBox.Checked)
                        si.AdministratorKey=Security.Encrypt(newPassword);
                    else
                         si.AdministratorKey=newPassword;

                    SiteConfigs.SaveConfig(si);
                     //CDHelper.UpdateSystemInformation(si);

                    ShowMessage("�����������޸ĳɹ���");
                }
                else
                {
                    ShowMessage("�Բ���������ľ����벻��ȷ��");
                }
            }
            else
            {
                Account act = AccountHelper.GetAccountByLoginName(loginName);
                if (act == null )
                {
                    ShowMessage("ָ�����û������ڡ�");
                }
                else if (!AccountHelper.IsValidPassword(act,password))
                {
                    ShowMessage("�Բ���������ľ����벻��ȷ��");
                }
                else if (act.State != 1)
                {
                    ShowMessage("���ʻ������á�");
                }
                else
                {
                    act.IsPasswordHashed = IsHashedPasswordCheckBox.Checked;
                    AccountHelper.UpdatePassword(act, newPassword);

                    //��¼��־
                    string content = string.Format("�޸��ˡ�{0}��������", act.LoginName);
                    AddLog("�޸�����", content);

                    ShowMessage("�����������޸ĳɹ���");
                }
            }
        }


        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//�Ƿ�����ʾվ��
            changePassword(LoginNameTextBox.Text, PasswordTextBox.Text, NewPasswordTextBox.Text);
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            LoginNameTextBox.Text="";
            PasswordTextBox.Text="";
            NewPasswordTextBox.Text="";
            AgainPasswordTextBox.Text="";
        }
    }
}
