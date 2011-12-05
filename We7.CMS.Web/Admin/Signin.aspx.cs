using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7;
using System.Text.RegularExpressions;
using We7.CMS.Controls;
using System.Net;
using System.IO;
using System.Text;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Framework.Config;
using We7.CMS.Accounts;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin
{
    public partial class Signin : BasePage
    {
        protected override bool NeedAnAccount
        {
            get { return false; }
        }
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ReturnURL
        {
            get
            {
                if (Request["ReturnURL"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Server.UrlDecode(Request["ReturnURL"].ToString());
                }
            }
        }
        protected string ProductBrand
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    return si.ProductName;
                }
                else
                    return "We7";
            }
        }
        int LoginCount
        {
            get
            {
                int count = 0;
                if (ViewState[this.ID + "_LoginCount"] != null)
                {
                    count = (int)ViewState[this.ID + "_LoginCount"];
                }
                return count;
            }
            set
            {
                ViewState[this.ID + "_LoginCount"] = value;
            }
        }
        void ShowMessage(string m)
        {
            MessageLabel.Text = m;
        }
        private void GenerateRandomCode()
        {
            if (CDHelper.Config.EnableLoginAuhenCode == "true")
            {
                tbAuthenCode2.Visible = true;
                Response.Cookies["AreYouHuman"].Value = CaptchaImage.GenerateRandomCode();
            }
        }


        /// <summary>
        /// ԭʼ��¼�ķ���
        /// </summary>
        /// <param name="loginName">�����û���</param>
        /// <param name="password">�����û�������</param>
        /// <param name="checkPassword">�Ƿ�У������</param>
        void LoginAction(string loginName, string password)
        {
            if (String.IsNullOrEmpty(loginName) || String.IsNullOrEmpty(loginName.Trim()))
            {
                ShowMessage("�����û�������Ϊ�գ�");
                return;
            }

            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(password.Trim()))
            {
                ShowMessage("�������벻��Ϊ�գ�");
                return;
            }

            if (GeneralConfigs.GetConfig().EnableLoginAuhenCode == "true" && this.CodeNumberTextBox.Text != Request.Cookies["AreYouHuman"].Value)
            {
                ShowMessage("�������������֤�벻��ȷ�����������룡");
                this.CodeNumberTextBox.Text = "";
                Response.Cookies["AreYouHuman"].Value = CaptchaImage.GenerateRandomCode();
                return;
            }

            bool loginSuccess = false;
            if (CheckLocalAdministrator(loginName))
            {
                if (CDHelper.AdminPasswordIsValid(password))
                {
                    Security.SetAccountID(We7Helper.EmptyGUID);
                    loginSuccess = true;
                    SSOLogin(loginName, password);
                }
                else
                {
                    ShowMessage("�޷���¼��ԭ���������");
                    return;
                }
            }
            else
            {
                string[] result = AccountHelper.Login(loginName, password);
                if (result[0] == "false")
                {
                    ShowMessage("�޷���¼��ԭ��" + result[1]);
                    return;
                }
                else
                {
                    SSOLogin(loginName, password);
                }
            }

            GoWhere();
        }



        private void GoWhere()
        {
            NewSiteConfig();
            if (ReturnURL == null || ReturnURL == string.Empty)
            {
                Response.Redirect("default.aspx");
            }
            else
            {
                Response.Redirect(ReturnURL);
            }
        }

        /// <summary>
        /// �Ƿ񳬼��û�
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        bool CheckLocalAdministrator(string loginName)
        {
            if (String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0  )
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// ��ʼ��վ��
        /// </summary>
        /// <returns></returns>
        private void NewSiteConfig()
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (string.IsNullOrEmpty(si.SiteTitle) || string.IsNullOrEmpty(si.Copyright) || string.IsNullOrEmpty(si.SiteFullName) || string.IsNullOrEmpty(si.IcpInfo) || string.IsNullOrEmpty(si.SiteLogo))
            {
                Response.Redirect(AppPath + "/NewSiteWizard.aspx?nomenu=1");
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null)
                {
                    if (si.IsOEM)
                        CopyrightLiteral.Text = si.Copyright;
                    else
                        CopyrightLiteral.Text = si.CopyrightOfWe7;
                }

                SiteConfigInfo sci = SiteConfigs.GetConfig();
                if (sci == null)
                {
                    Response.Write("�Բ���,����ϵͳ���������������ļ���δ����������Ҫ���������ݽ�������������������<a href='../install/upgradeconfig.aspx'><u>��������</u></a>");
                    Response.End();
                }
                else
                {
#if DEBUG
                    LoginNameTextBox.Text = sci.AdministratorName;
#endif
                    GenerateRandomCode();
                    if (Request["action"] != null && Request["action"].ToString() == "logout" && Request["Authenticator"] == null)
                    {
                        //��¼��־
                        string content = string.Format("�˳�վ��");
                        AddLog("վ���¼", content);
                        string result = SignOut();
                        if (!string.IsNullOrEmpty(result))
                            ShowMessage("��¼�˳�û�гɹ���ԭ��" + result);
                        else
                            SSOLogout();
                    }
                }
            }

            if (Request["user"] != null && Request["pass"] != null)
            {
                LoginAction(Request["user"].ToString(), Request["pass"].ToString());
            }

            if (Request["Authenticator"] != null && Request["accountID"] != null)
            {
                SSORequest ssoRequest = SSORequest.GetRequest(HttpContext.Current);
                string actID = ssoRequest.AccountID;
                if (Authentication.ValidateEACToken(ssoRequest) && !string.IsNullOrEmpty(actID) && We7Helper.IsGUID(actID))
                {
                    Security.SetAccountID(actID);
                    SSOLogin(ssoRequest.UserName, ssoRequest.Password);
                    GoWhere();
                }
                else if (Request["message"] != null)
                {
                    ShowMessage("��¼ʧ�ܣ�ԭ��" + Request["message"]);
                    return;
                }
            }
        }

        void AddUserLoginStatistics()
        {
            PageVisitorHelper.AddPageVisitor(AccountID);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            LoginAction(LoginNameTextBox.Text.Trim(), PasswordTextBox.Text);
        }

        private void SSOLogin(string userName, string password)
        {
            if (!String.IsNullOrEmpty(GeneralConfigs.GetConfig().SSOSiteUrls))
            {
                SSORequest ssoRequest = new SSORequest();
                ssoRequest.ToUrls = GeneralConfigs.GetConfig().SSOSiteUrls;
                ssoRequest.AppUrl = String.Format("{0}/{1}", Utils.GetRootUrl(), String.IsNullOrEmpty(ReturnURL) ? "Admin/theme/classic/main.aspx" : ReturnURL.TrimStart('/'));
                ssoRequest.Action = "signin";
                ssoRequest.UserName = userName;
                ssoRequest.Password = password;
                Authentication.PostChains(ssoRequest);
            }
        }

        private void SSOLogout()
        {
            if (!String.IsNullOrEmpty(GeneralConfigs.GetConfig().SSOSiteUrls))
            {
                SSORequest ssoRequest = new SSORequest();
                ssoRequest.ToUrls = GeneralConfigs.GetConfig().SSOSiteUrls;
                ssoRequest.AppUrl = String.Format("{0}/{1}", Utils.GetRootUrl(),"Admin/Signin.aspx");
                ssoRequest.Action = "logout";
                Authentication.PostChains(ssoRequest);
            }
        }
    }
}
