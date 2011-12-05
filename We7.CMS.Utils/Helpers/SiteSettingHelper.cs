using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

using Thinkment.Data;

using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.PF;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Accounts;

namespace We7.CMS
{
    /// <summary>
    /// ��վ��������������
    /// </summary>
    [Helper("We7.CMS.Helper")]
    public class SiteSettingHelper : BaseHelper
    {
        #region ������վ������������
        static string SIAdministratorID = "{00000000-0000-0000-0000-000000000000}";
         static string SIAdministratorKey = "CD.AdministratorKey";
         static string SICompanyName = "WD.CompanyName";
         static string SICompanyDescription = "WD.CompanyDescription";
         static string SICompanyID = "WD.CompanyID";
         static string SIProduct = "CD.Product";
         static string SIVersion = "CD.Version";
         static string SIWDUrl = "WD.Url";
         static string SIIDUrl = "ID.Url";
         static string SISiteID = "CD.SiteID";
         static string SIAllowSignup = "CD.AllowSignup";
         static string SIRootUrl = "CD.RootUrl";
         static string SIArticleAutoPublish = "CD.ArticleAutoPublish";
         static string SIArticleAutoShare = "CD.ArticleAutoShare";
         static string SIAllCutCheckBox = "CD.AllCutCheckBox";
         static string SIDefaultTemplateGroup = "CD.DefaultTemplateGroup";
         static string SIDefaultTemplateGroupFileName = "CD.DefaultTemplateGroupFileName";
         static string SIDefaultHomePageTitle = "CD.DefaultHomePageTitle";
         static string SIDefaultChannelPageTitle = "CD.DefaultChannelPageTitle";
         static string SIDefaultContentPageTitle = "CD.DefaultContentPageTitle";
         static string CDMenuDefault = "{00000000-0000-0001-0000-000000000000}";
         static string CDMenuAdministration = "{00000000-0000-0002-0000-000000000000}";
         static string CDMenuWebGroup = "{00000000-0000-0003-0000-000000000000}";

         static string SISystemMail = "CD.SystemMail";
         static string SISysMailUser = "CD.SysMailUser";
         static string SISysMailPassword = "CD.SysMailPassword";
         static string SISysMailServer = "CD.SysMailServer";
         static string SINotifyMail = "CD.NotifyMail";

         static string SIIsPasswordHashed = "CD.IsPasswordHashed";

         static string SIGenericUserManage = "CD.GenericUserManage";
         static string SIIsAddLog = "CD.IsAddLog";
         static string SIIsAuditComment = "CD.IsAuditComment";

         static string SITemplateGroupBasePath = "TemplateGroupBasePath";
         static string SITemplateBasePath = "TemplateBasePath";
         static string SIEnableSiteSkins = "EnableSiteSkins";
         static string SISiteSkinsBasePath = "SiteSkinsBasePath";

         static string SIArticleUrlGenerator = "CD.ArticleUrlGenerator";
         static string SIEnableLoginAuhenCode = "EnableLoginAuhenCode";

         static string SIDescriptionPageMeta = "DescriptionPageMeta";
         static string SIKeywordPageMeta = "KeywordPageMeta";

         static string SICMSTheme = "CMSTheme";

         static string SISSOUrl = "SSO.Url";
         static string SISSOServername = "SSO.Servername";
         static string SISSOUsername = "SSO.Username";
         static string SISSOPassword = "SSO.Password";
         static string SISSODomain = "SSO.Domain";
         static string SISSOWdID = "SSO.WdID";
         static string SIDefaultCompanyRole = "CD.DefaultCompanyRole";
         static string SIDefaultPersonalRole = "CD.DefaultPersonalRole";
         static string SIArticleSourceDefault = "CD.ArticleSourceDefault";

         static string SIEnableCache = "CD.EnableCache";

         public static string SIADUrl = "AD.Url";

        #endregion

         public SiteSettingHelper()
        {
        }

        /// <summary>
        /// ��ǰ��ʵ������
        /// </summary>
        public static SiteSettingHelper Instance
        {
            get
            {
                HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                SiteSettingHelper cdHelper = helperFactory.GetHelper<SiteSettingHelper>();
                return cdHelper;
            }
        }

        /// <summary>
        /// ��ֹ�ϴ�����
        /// </summary>
        public static string[] forbidType
        {
            get
            {
                string fs = GeneralConfigs.GetConfig().Upload_Forbid;
                return (fs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
        /// <summary>
        /// ����ͼƬ����
        /// </summary>
        public static string[] allowImageType
        {
            get
            {
                string fs =GeneralConfigs.GetConfig().Upload_AllowImageType  ;
                return (fs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        /// <summary>
        /// �Ƿ����ָ��ģ��
        /// </summary>
        /// <param name="fn">ģ���ļ���</param>
        /// <returns></returns>
        public bool ExistTemplate(string fn)
        {
            return false;
        }

        /// <summary>
        /// ͨ�����ò���
        /// </summary>
        public GeneralConfigInfo Config
        {
            get
            {
                return GeneralConfigs.GetConfig();
            }
        }

        /// <summary>
        /// վ�����ò���
        /// </summary>
        public  SiteConfigInfo SiteConfig
        {
            get
            {
                return SiteConfigs.GetConfig();
            }
        }

        /// <summary>
        /// վ����������ݿ�Ǩ�Ƶ�config�ļ�
        /// </summary>
        public bool MigrateConfig()
        {
            SiteConfigInfo sconfig = new SiteConfigInfo();
            GeneralConfigInfo gconfig = GeneralConfigs.GetConfig();
            if (gconfig == null)
            {
                HttpContext Context = HttpContext.Current;
                string configFile = Context.Server.MapPath("~/Config/general.config");
                gconfig = new GeneralConfigInfo();
                GeneralConfigs.Serialiaze(gconfig, configFile);
            }
            SystemInformation si = GetSystemInformation();

            sconfig.AdministratorID = si.AdministratorID;
            if( si.AdministratorKey!=null && si.AdministratorKey!="")
                sconfig.AdministratorKey = si.AdministratorKey;
            if(si.CompanyName !=null && si.CompanyName !="")
                sconfig.SiteName = si.CompanyName;
            sconfig.SiteDescription = si.CompangDescription;
            sconfig.InformationServiceUrl = si.InformationServiceUrl;
            gconfig.ProductName = si.ProductName;
            //gconfig.ProductVersion = si.ProdcutVersion;
            sconfig.WebGroupServiceUrl = si.WebGroupServiceUrl;
            sconfig.SiteID = si.SiteID;
            sconfig.RootUrl = si.RootUrl;
            sconfig.SsoUrl = si.SsoUrl;
            sconfig.SsoServername = si.SsoServername;
            sconfig.SsoUsername = si.SsoUsername;
            sconfig.SsoPassword = si.SsoPassword;
            sconfig.SsoDomain = si.SsoDomain;
            sconfig.SsoWdID = si.SsoWdID;
            sconfig.ADUrl = si.ADUrl;
            sconfig.IsPasswordHashed = si.IsPasswordHashed;

            gconfig.ArticleAutoPublish = si.ArticleAutoPublish;
            gconfig.ArticleAutoShare = si.ArticleAutoShare;
            gconfig.AllCutCheckBox = si.AllCutCheckBox;
            gconfig.AllowSignup = si.AllowSignup;
            gconfig.DefaultTemplateGroup = si.DefaultTemplateGroup;
            gconfig.DefaultTemplateGroupFileName = si.DefaultTemplateGroupFileName;
            gconfig.DefaultHomePageTitle = si.DefaultHomePageTitle;
            gconfig.DefaultChannelPageTitle = si.DefaultChannelPageTitle;
            gconfig.DefaultContentPageTitle = si.DefaultContentPageTitle;

            gconfig.SystemMail = si.SystemMail;
            gconfig.SysMailUser = si.SysMailUser;
            gconfig.SysMailServer = si.SysMailServer;
            gconfig.SysMailPassword = si.SysMailPassword;
            gconfig.NotifyMail = si.NotifyMail;
            gconfig.GenericUserManageType = si.GenericUserManageType;

            gconfig.IsAddLog = si.IsAddLog;
            gconfig.IsAuditComment = si.IsAuditComment;
            gconfig.EnableLoginAuhenCode = si.EnableLoginAuhenCode;
            gconfig.EnableSiteSkins = si.EnableSiteSkins;
            gconfig.TemplateBasePath = si.TemplateBasePath;
            gconfig.TemplateGroupBasePath = si.TemplateGroupBasePath;
            gconfig.SiteSkinsBasePath = si.SiteSkinsBasePath;
            gconfig.ArticleUrlGenerator = si.ArticleUrlGenerator;
            gconfig.KeywordPageMeta = si.KeywordPageMeta;
            gconfig.DescriptionPageMeta = si.DescriptionPageMeta;
            gconfig.CMSTheme = si.CMSTheme;
            gconfig.DefaultPersonRole = si.DefaultPersonRole;
            gconfig.DefaultCompanyRole = si.DefaultCompanyRole;
            gconfig.ArticleSourceDefault = si.ArticleSourceDefault;
            gconfig.EnableCache = si.EnableCache;

            return SiteConfigs.SaveConfig(sconfig) && GeneralConfigs.SaveConfig(gconfig);
        }

        /// <summary>
        /// ͨ��key��ȡվ�����õ�Value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string gettempsys(string key)
        {
            We7Helper.AssertNotNull(Assistant, "BaseHelper.Assistant");
            We7Helper.AssertNotNull(key, "BaseHelper.GetSystemParameter.key");

            Criteria c = new Criteria(CriteriaType.Equals, "ID", key);
            List<SiteSetting> sets = Assistant.List<SiteSetting>(c, null, 0, 1, new string[] { "ID", "Title", "Value", "SequenceIndex" });
            if (sets.Count > 0)
                return sets[0].Value;
            else
                return "";
        }

        /// <summary>
        /// ��ȡվ�������ļ�
        /// </summary>
        /// <returns></returns>
        public SystemInformation GetSystemInformation()
        {
            HttpContext context = HttpContext.Current;
            if (context.Application["CD.SIVAVUES"]==null)
            {
                SystemInformation si = new SystemInformation();

                si.AdministratorID = SIAdministratorID;
                si.AdministratorKey = gettempsys(SIAdministratorKey);
                si.CompanyName = gettempsys(SICompanyName);
                si.CompanyID = gettempsys(SICompanyID);
                si.CompangDescription = gettempsys(SICompanyDescription);
                si.InformationServiceUrl = gettempsys(SIIDUrl);
                si.ProductName = gettempsys(SIProduct);
                si.ProdcutVersion = gettempsys(SIVersion);
                si.WebGroupServiceUrl = gettempsys(SIWDUrl);
                si.SiteID = gettempsys(SISiteID);
                si.RootUrl = gettempsys(SIRootUrl);
                si.ArticleAutoPublish = gettempsys(SIArticleAutoPublish);
                si.ArticleAutoShare = gettempsys(SIArticleAutoShare);
                si.AllCutCheckBox = gettempsys(SIAllCutCheckBox);
                si.AllowSignup = gettempsys(SIAllowSignup);
                si.DefaultTemplateGroup = gettempsys(SIDefaultTemplateGroup);
                si.DefaultTemplateGroupFileName = gettempsys(SIDefaultTemplateGroupFileName);

                si.DefaultHomePageTitle = gettempsys(SIDefaultHomePageTitle);
                si.DefaultChannelPageTitle = gettempsys(SIDefaultChannelPageTitle);
                si.DefaultContentPageTitle = gettempsys(SIDefaultContentPageTitle);

                si.SystemMail = gettempsys(SISystemMail);
                si.SysMailUser = gettempsys(SISysMailUser);
                si.SysMailServer = gettempsys(SISysMailServer);
                si.SysMailPassword = gettempsys(SISysMailPassword);
                si.NotifyMail = gettempsys(SINotifyMail);
                si.GenericUserManageType = gettempsys(SIGenericUserManage);

                si.IsPasswordHashed = gettempsys(SIIsPasswordHashed) == "1";
                si.IsAddLog = gettempsys(SIIsAddLog) == "1";
                si.IsAuditComment = gettempsys(SIIsAuditComment) == "1";

                si.EnableLoginAuhenCode = gettempsys(SIEnableLoginAuhenCode);
                si.EnableSiteSkins = gettempsys(SIEnableSiteSkins);
                si.TemplateBasePath = gettempsys(SITemplateBasePath);
                si.TemplateGroupBasePath = gettempsys(SITemplateGroupBasePath);
                si.SiteSkinsBasePath = gettempsys(SISiteSkinsBasePath);
                si.ArticleUrlGenerator = gettempsys(SIArticleUrlGenerator);
                si.KeywordPageMeta = gettempsys(SIKeywordPageMeta);
                si.DescriptionPageMeta = gettempsys(SIDescriptionPageMeta);
                si.CMSTheme = gettempsys(SICMSTheme);

                si.SsoUrl = gettempsys(SISSOUrl);
                si.SsoServername = gettempsys(SISSOServername);
                si.SsoUsername = gettempsys(SISSOUsername);
                si.SsoPassword = gettempsys(SISSOPassword);
                si.SsoDomain = gettempsys(SISSODomain);
                si.SsoWdID = gettempsys(SISSOWdID);

                si.ADUrl = gettempsys(SIADUrl);
                si.DefaultPersonRole = gettempsys(SIDefaultPersonalRole);
                si.DefaultCompanyRole = gettempsys(SIDefaultCompanyRole);
                si.ArticleSourceDefault = gettempsys(SIArticleSourceDefault);

                si.EnableCache = gettempsys(SIEnableCache);

                context.Application["CD.SIVAVUES"] = si;
            }
            return (SystemInformation)context.Application["CD.SIVAVUES"];
        }

        /// <summary>
        /// ȡ�ù�˾����
        /// </summary>
        /// <returns></returns>
        public string GetCompanyName()
        {
            return SiteConfig.SiteName;
        }

        /// <summary>
        /// ȡ��վ��ID
        /// </summary>
        /// <returns></returns>
        public string GetSiteID()
        {
            return SiteConfig.SiteID;
        }

        /// <summary>
        /// ȡ��Ĭ����ҳ����
        /// </summary>
        /// <returns></returns>
        public string GetDefaultHomePageTitle()
        {
            return Config.DefaultHomePageTitle;
        }

        /// <summary>
        /// ȡ��Ĭ������ҳ����
        /// </summary>
        /// <returns></returns>
        public string GetDefaultContentPageTitle()
        {
            return Config.DefaultContentPageTitle;
        }

        /// <summary>
        /// ȡ��Ĭ����Ŀҳ����
        /// </summary>
        /// <returns></returns>
        public string GetDefaultChannelPageTitle()
        {
            return Config.DefaultChannelPageTitle;
        }

        /// <summary>
        /// �����Ƿ�Ӧ��Hash����
        /// </summary>
        /// <returns></returns>
        public bool GetPasswordIsHashed()
        {
            return SiteConfig.IsPasswordHashed;
        }

        /// <summary>
        /// ȡ��ϵͳ�ʼ���Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetSystemMail()
        {
            return Config.NotifyMail;
        }

        /// <summary>
        /// ȡ����Ϣ�����ͷ�ʽ
        /// </summary>
        /// <returns></returns>
        public int GetGUMSParam()
        {
            int type = 0;
            try
            {
                type = int.Parse(Config.GenericUserManageType);
            }
            catch {}
            
            return(type);
        }

        /// <summary>
        /// �Ƿ������־
        /// </summary>
        /// <returns></returns>
        public bool GetIsAddLog()
        {
            return Config.IsAddLog;
        }

        /// <summary>
        /// ��֤��½����
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AdminPasswordIsValid(string password)
        {
            if (SiteConfig.IsPasswordHashed)
            {                                
                password = Security.Encrypt(password);
            }
            string adminPass = SiteConfig.AdministratorKey;// GetSystemParameter("CD.AdministratorKey");
            return string.Compare(password, adminPass, false) == 0;
        }


        /// <summary>
        /// ��ǰ�ļ��Ƿ��ܹ��ϴ�
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <returns></returns>
        public bool CanUpload(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            foreach (string f in allowImageType)
            {
                if (String.Compare(f, ext, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ǰ�����Ƿ����ϴ�
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool CanUploadAttachment(string fileName)
        {
            string ext = Path.GetExtension(fileName);

            foreach (string f in forbidType)
            {
                if (String.Compare(f, ext, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �һ��û�����
        /// </summary>
        /// <param name="loginName">�û���</param>
        /// <param name="Mail">Email</param>
        /// <param name="AccountHelper">Ȩ��ҵ�����</param>
        /// <returns></returns>
        public string GetMyPassword(string loginName, string Mail, IAccountHelper AccountHelper)
        {

            if (String.Compare(loginName, SiteConfigs.GetConfig().AdministratorName, true) == 0)
            {
                if (Mail == GetSystemMail())
                {
                    Account ad = new Account();
                    ad.LastName = "����Ա";
                    ad.Email = Mail;
                    ad.LoginName = SiteConfigs.GetConfig().AdministratorName;
                    ad.Password = SiteConfigs.GetConfig().AdministratorKey;                    
                    ad.IsPasswordHashed = GetPasswordIsHashed();
                    return SendPasswordByMail(ad,AccountHelper);
                }
                else
                {
                    return "�Բ�������������䲻�ǹ���Աָ����ϵͳ�ʼ���ַ��";
                }
            }
            else
            {
                Account act = AccountHelper.GetAccountByLoginName(loginName);
                if (act == null)
                {
                    return "ָ�����û������ڡ�";
                }

                else if (act.State != 1)
                {
                    return "���ʻ������á�";
                }
                else if (act.Email != Mail)
                {
                    return "�Բ�������������䲻����ע��ʱ��д����Ч�ʼ���ַ��";
                }
                else
                {
                    return SendPasswordByMail(act,AccountHelper);
                }
            }
        }

        /// <summary>
        /// ͨ��Email����������Ϣ
        /// </summary>
        /// <param name="account">Ȩ����Ϣ</param>
        /// <param name="AccountHelper">Ȩ��ҵ�����</param>
        /// <returns></returns>
        public string SendPasswordByMail(Account account, IAccountHelper AccountHelper)
        {
            GeneralConfigInfo si = Config;
            if (si.SystemMail == "" || si.SysMailUser == "" || si.SysMailServer == "")
                return "ϵͳ�ʼ���Ϣ��ȫ���뵽��վ�˵���ϵͳ���á���������ز�����" ;
            else
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.AdminEmail = si.SystemMail;
                mailHelper.UserName = si.SysMailUser;
                mailHelper.Password = si.SysMailPassword;
                mailHelper.SmtpServer = si.SysMailServer;

                string password = null;
                if (account.IsPasswordHashed)
                {
                    password = Security.ResetPassword(account);

                    string secuPassword = "";
                    if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                    {
                        secuPassword = Security.Encrypt(password);
                    }
                    else if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                    {
                        secuPassword = Security.BbsEncrypt(password);
                    }
                    if (String.Compare(account.LoginName, SiteConfigs.GetConfig().AdministratorName, true) == 0)
                    {
                        //UpdateSystemParameter("CD.AdministratorKey", secuPassword);
                        SiteConfig.AdministratorKey = secuPassword;
                        SiteConfigs.SaveConfig(SiteConfig);
                    }
                    else
                    {
                        AccountHelper.UpdatePassword(account, secuPassword);
                    }

                }
                else
                {
                    password = account.Password;
                }

                string message = "�װ���{0}�����ã�\r\n���ĵ�¼�ʻ�:\r\n�û���: {3}\r\n����: {1}\r\n\r\n{5}-{2}\r\n��վ����Ա\r\n{4}";

                string mainurl = SiteConfig.RootUrl;
                string To = account.Email;
                string From = string.Format("\"{1}ϵͳ�ʼ�\" <{0}>", si.SystemMail, SiteConfig.SiteName);
                string Subject = "������վ��¼��Ϣ";
                string Body = string.Format(message, account.LastName, password, mainurl, account.LoginName, DateTime.Now.ToLongDateString(), SiteConfig.SiteName);
                mailHelper.Send(To, From, Subject, Body,"");
                //Message.Text = "Login Credentials Sent<br>";
                return "�������ѷ����������";
            }
        }



        #region ��̬URL
        /// <summary>
        /// ��url��ȡ��id�͵�ֵ
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetSearcherKeyFromUrl(string path)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            string mathstr = @"/(\w|\s|(-)|(_))+\." + ext + "$";
            if (path.ToLower().EndsWith("default." + ext))
                path = path.Remove(path.Length - 12);
            if (path.ToLower().EndsWith("index." + ext))
                path = path.Remove(path.Length - 10);

            if (Regex.IsMatch(path, mathstr))
            {
                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                int lastDot = path.LastIndexOf(".");
                if (lastDot > -1)
                {
                    path = path.Remove(lastDot, path.Length - lastDot);
                }

                return path;
            }
            else
                return string.Empty;

        }
        #endregion
        /// <summary>
        /// ����һ��������¼
        /// </summary>
        /// <param name="pages"></param>
        /// <param name="content"></param>
        public void AddLog(string pages, string content)
        {
            HelperFactory HelperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            LogHelper LogHelper = HelperFactory.GetHelper<LogHelper>();

            if (Config.IsAddLog)
            {
                LogHelper.WriteLog(Security.CurrentAccountID, pages, content, Config.DefaultHomePageTitle);
            }
        }
    }
}
