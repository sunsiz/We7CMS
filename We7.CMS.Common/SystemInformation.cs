using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

namespace We7.CMS.Common
{
    /// <summary>
    /// ϵͳ����
    /// </summary>
    [Obsolete]
    [Serializable]
    public class SystemInformation
    {
        string administratorID;


        string administratorKey;
        string productName;
        string prodcutVersion;
        string companyID;
        string companyName;
        string compangDescription;
        string wdUrl;
        string idUrl;
        string allowSignup;
        string siteID;
        string rootUrl;
        string articleAutoPublish;
        string defaultTemplateGroup;
        string defaultTemplateGroupFileName;
        string defaultChannelPageTitle;
        string defaultHomePageTitle;
        string defaultContentPageTitle;
        bool isAddLog;
        bool isAuditComment;
        string articleAutoShare;
        string systemMail;
        string sysMailUser;
        string sysMailPassword;
        string sysMailServer;
        string notifyMail;
        string genericUserManageType;

        bool isPasswordHashed;

        string templateGroupBasePath;
        string templateBasePath;
        string enableSiteSkins;
        string siteSkinsBasePath;

        string articleUrlGenerator;
        string enableLoginAuhenCode;

        string keywordPageMeta;
        string descriptionPageMeta;

        string ssoUrl;
        string ssoServername;     
        string ssoUsername;
        string ssoPassword;
        string ssoDomain;
        string ssoWdID;

        string cMSTheme;
        string defaultCompanyRole;
        string defaultPersonRole;
        string allCutCheckBox;



        public string AdministratorKey
        {
            get { return administratorKey; }
            set { administratorKey = value; }
        }

        public string DefaultTemplateGroupFileName
        {
            get { return defaultTemplateGroupFileName; }
            set { defaultTemplateGroupFileName = value; }
        }

        public string DefaultTemplateGroup
        {
            get { return defaultTemplateGroup; }
            set { defaultTemplateGroup = value; }
        }

        public string DefaultHomePageTitle
        {
            get { return defaultHomePageTitle; }
            set { defaultHomePageTitle = value; }
        }

        public string DefaultChannelPageTitle
        {
            get { return defaultChannelPageTitle; }
            set { defaultChannelPageTitle = value; }
        }

        public string DefaultContentPageTitle
        {
            get { return defaultContentPageTitle; }
            set { defaultContentPageTitle = value; }
        }

        public string RootUrl
        {
            get { return rootUrl; }
            set { rootUrl = value; }
        }

        public string ArticleAutoPublish
        {
            get { return articleAutoPublish; }
            set { articleAutoPublish = value; }
        }
	
        public string SiteID
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public SystemInformation()
        {
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public string ProdcutVersion
        {
            get { return prodcutVersion; }
            set { prodcutVersion = value; }
        }

        public string CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public string CompangDescription
        {
            get { return compangDescription; }
            set { compangDescription = value; }
        }

        public string WebGroupServiceUrl
        {
            get { return wdUrl; }
            set { wdUrl = value; }
        }

        public string InformationServiceUrl
        {
            get { return idUrl; }
            set { idUrl = value; }
        }

        public string AllowSignup
        {
            get { return allowSignup; }
            set { allowSignup = value; }
        }

        public string AllCutCheckBox
        {
            get { return allCutCheckBox; }
            set { allCutCheckBox = value; }
        }

        public string SystemMail
        {
            get { return systemMail; }
            set { systemMail = value; }
        }

        public string SysMailUser
        {
            get { return sysMailUser; }
            set { sysMailUser = value; }
        }
        public string SysMailPassword
        {
            get { return sysMailPassword; }
            set { sysMailPassword = value; }
        }
        public string SysMailServer
        {
            get { return sysMailServer; }
            set { sysMailServer = value; }
        }

        public string NotifyMail
        {
            get { return notifyMail; }
            set { notifyMail = value; }
        }

        public string GenericUserManageType
        {
            get { return genericUserManageType; }
            set { genericUserManageType = value; }
        }


        /// <summary>
        /// ����Ա�����Ƿ����
        /// </summary>
        public bool IsPasswordHashed
        {
            get { return isPasswordHashed; }
            set { isPasswordHashed = value; }
        }

        /// <summary>
        /// �Ƿ�����־��¼����
        /// </summary>
        public bool IsAddLog
        {
            get { return isAddLog; }
            set { isAddLog = value; }
        }
        /// <summary>
        /// �����Ƿ���˺󷢲�
        /// </summary>
        public bool IsAuditComment
        {
            get { return isAuditComment; }
            set { isAuditComment = value; }
        }
        public string ArticleAutoShare
        {
            get { return articleAutoShare; }
            set { articleAutoShare = value; }
        }
        /// <summary>
        /// ��¼�Ƿ���֤
        /// </summary>
        public string EnableLoginAuhenCode
        {
            get { return enableLoginAuhenCode; }
            set { enableLoginAuhenCode = value; }
        }

        /// <summary>
        /// ����URL��ʽ
        /// </summary>
        public string ArticleUrlGenerator
        {
            get { return articleUrlGenerator; }
            set { articleUrlGenerator = value; }
        }

        /// <summary>
        /// վ��Ƥ����Ŀ¼
        /// </summary>
        public string SiteSkinsBasePath
        {
            get { return siteSkinsBasePath; }
            set { siteSkinsBasePath = value; }
        }

        /// <summary>
        /// ����Ƥ�������ļ���
        /// </summary>
        public string EnableSiteSkins
        {
            get { return enableSiteSkins; }
            set { enableSiteSkins = value; }
        }

        /// <summary>
        /// ģ���Ŀ¼
        /// </summary>
        public string TemplateBasePath
        {
            get { return templateBasePath; }
            set { templateBasePath = value; }
        }

        /// <summary>
        /// ģ�����Ŀ¼
        /// </summary>
        public string TemplateGroupBasePath
        {
            get { return templateGroupBasePath; }
            set { templateGroupBasePath = value; }
        }
        /// <summary>
        ///  ҳ������(description)
        /// </summary>
        public string DescriptionPageMeta
        {
            get { return descriptionPageMeta; }
            set { descriptionPageMeta = value; }
        }

        /// <summary>
        /// ҳ��ؼ���(keyword)
        /// </summary>
        public string KeywordPageMeta
        {
            get { return keywordPageMeta; }
            set { keywordPageMeta = value; }
        }

        /// <summary>
        /// ��̨�������
        /// </summary>
        public string CMSTheme
        {
            get { return cMSTheme; }
            set { cMSTheme = value; }
        }
        private string articleSourceDefault;
        /// <summary>
        /// ������ԴĬ��ֵ
        /// </summary>
        public string ArticleSourceDefault
        {
            get { return articleSourceDefault; }
            set { articleSourceDefault = value; }
        }
        public void FromXml(XmlElement xe)
        {
            ProductName = xe.GetAttribute("product");
            ProdcutVersion = xe.GetAttribute("verion");
            CompanyID = xe.GetAttribute("companyID");
            CompanyName = xe.GetAttribute("companyName");
            CompangDescription = xe.GetAttribute("companyDescription");
            WebGroupServiceUrl = xe.GetAttribute("wdUrl");
            InformationServiceUrl = xe.GetAttribute("idUrl");
            AllowSignup = xe.GetAttribute("signup");
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("System");
            xe.SetAttribute("product", productName);
            xe.SetAttribute("verion", ProdcutVersion);
            xe.SetAttribute("companyID", CompanyID);
            xe.SetAttribute("companyName", companyName);
            xe.SetAttribute("companyDescription", CompangDescription);
            xe.SetAttribute("wdUrl", wdUrl);
            xe.SetAttribute("idUrl", idUrl);
            return xe;
        }

        /// <summary>
        /// �൱�ڲ���AccountID
        /// </summary>
        public string AdministratorID
        {
          get { return administratorID; }
          set { administratorID = value; }
        }

        public string SsoUrl
        {
            get { return ssoUrl; }
            set { ssoUrl = value; }
        }

        public string SsoServername
        {
            get { return ssoServername; }
            set { ssoServername = value; }
        }

        public string SsoUsername
        {
            get { return ssoUsername; }
            set { ssoUsername = value; }
        }

        public string SsoPassword
        {
            get { return ssoPassword; }
            set { ssoPassword = value; }
        }

        public string SsoDomain
        {
            get { return ssoDomain; }
            set { ssoDomain = value; }
        }

        public string SsoWdID
        {
            get { return ssoWdID; }
            set { ssoWdID = value; }
        }

        string adUrl;
        /// <summary>
        /// վȺ������վ���ַ
        /// </summary>
        public string ADUrl
        {
            get { return adUrl; }
            set { adUrl = value; }
        }
        /// <summary>
        /// ������ҵ��ԱĬ�Ͻ�ɫ
        /// </summary>
        public string DefaultCompanyRole
        {
            get { return defaultCompanyRole; }
            set { defaultCompanyRole = value; }
        }
        /// <summary>
        /// ���ø��˻�ԱĬ�Ͻ�ɫ
        /// </summary>
        public string DefaultPersonRole
        {
            get { return defaultPersonRole; }
            set { defaultPersonRole = value; }
        }

        private string enableCache;
        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public string EnableCache
        {
            get { return enableCache; }
            set { enableCache = value; }
        }

        private Int16 cacheTimeSpan = 60;
        /// <summary>
        /// �������ʱ�������룩
        /// </summary>
        public Int16 CacheTimeSpan
        {
            get
            {
                string filePath = HttpContext.Current.Server.MapPath("/Config/TimeSpanForRemove.xml");
                XmlDocument doc = new XmlDocument();
                if (File.Exists(filePath))
                {
                    doc.Load(filePath);
                    XmlNode node = doc.SelectSingleNode("/timespan");
                    Int16 tmpInt = 60;
                    if (Int16.TryParse(node.InnerText, out tmpInt))
                    {
                        cacheTimeSpan = tmpInt;
                    }
                }
                return cacheTimeSpan;
            }
            set
            {
                cacheTimeSpan = value;
                string filePath = HttpContext.Current.Server.MapPath("/Configuration/TimeSpanForRemove.xml");
                XmlDocument doc = new XmlDocument();
                string xmlStr = "<?xml version=\"1.0\" standalone=\"yes\"?><timespan>" 
                    + cacheTimeSpan.ToString() + "</timespan>";
                doc.LoadXml(xmlStr);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                doc.Save(filePath);
            }
        }
        
    }
}
