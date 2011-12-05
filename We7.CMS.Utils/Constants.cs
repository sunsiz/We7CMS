using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.IO;

using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// ���ݴ��Ŀ¼��Ĭ��/_Data
        /// </summary>
        public static string DataUrlPath = "/_Data";
        /// <summary>
        /// 2.5����ǰ�ؼ���ŵ�Ŀ¼
        /// </summary>
        [Obsolete]
        public static string ControlBasePath = "cgi-bin\\templates\\controls";

       /// <summary>
       /// 2.5����ǰ�ؼ���Url·��
       /// </summary>
        [Obsolete]
        public static string ControlUrlPath
        {
            get
            {
                string temp = ControlBasePath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;
                return temp;
            }
        }

        /// <summary>
        /// We7�ؼ���Ŀ¼��Ŀ¼��
        /// </summary>
        public const string We7ControlsBasePath = "We7Controls";

        /// <summary>
        /// We7����ĸ�Ŀ¼Ŀ¼��
        /// </summary>
        public const string We7PluginBasePath = "Plugins";

        /// <summary>
        /// ģ���ļ���Ŀ¼Ŀ¼��
        /// </summary>
        public const string We7ModelBasePath = "Models";

        /// <summary>
        /// ���������ĸ�Ŀ¼
        /// </summary>
        public const string We7WidgetsFolder = "Widgets";

        /// <summary>
        /// �����ĸ�Ŀ¼
        /// </summary>
        public const string We7WidgetCollectionFolder = "Widgets\\WidgetCollection";

        /// <summary>
        /// ��������ĸ�Ŀ¼
        /// </summary>
        public const string We7ThemeFolder = "Widgets\\Themes";

        /// <summary>
        /// ��̬������Ŀ¼
        /// </summary>
        public const string We7HtmlWidgetFolder = "Widgets\\WidgetCollection\\��̬��";

        /// <summary>
        /// We7�ؼ��������ļ���,����չ��
        /// </summary>
        public const string We7ControlConfigFile = "DataControl.xml";

        /// <summary>
        /// We7Widget�������ļ���,����չ��
        /// </summary>
        public const string We7Widget = "Widget.xml";

         /// <summary>
        /// We7�ؼ���Ŀ¼������·��
        /// </summary>
        public readonly static string We7ControlPhysicalPath;
        /// <summary>
        /// ���������Ŀ¼
        /// </summary>
        public readonly static string We7PluginPhysicalPath;




        /// <summary>
        /// �����ģ������Ŀ¼
        /// </summary>
        public readonly static string We7ModelPhysicalPath;

        public readonly static string We7WidgetsPhysicalFolder;
        /// <summary>
        /// �ؼ��ļ�λ��
        /// </summary>
        public readonly static string We7WidgetsFileFolder;

        /// <summary>
        /// ��̬�����ļ�λ��
        /// </summary>
        public readonly static string We7HtmlWidgetsFileFolder;

        /// <summary>
        /// ���������ļ�λ��
        /// </summary>
        public readonly static string We7ThemeFileFolder;

        static Constants()
        {
            We7ControlPhysicalPath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory,We7ControlsBasePath);
            We7PluginPhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7PluginBasePath);
            We7ModelPhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7ModelBasePath);
            We7WidgetsPhysicalFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7WidgetsFolder);
            We7WidgetsFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7WidgetCollectionFolder);
            We7HtmlWidgetsFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7HtmlWidgetFolder);
            We7ThemeFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, We7ThemeFolder);
        }

        /// <summary>
        ///  ���õ�վ��Ƥ��
        /// </summary>
        public static bool EnableSiteSkins
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                string _default = si.EnableSiteSkins;
                if (_default != null && _default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        /// <summary>
        /// վ��Ƥ�Ż���·��
        /// </summary>
        public static string SiteSkinsBasePath
        {
            get
            {
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                string _default = si.SiteSkinsBasePath;
                if (_default==null || _default == string.Empty)
                    _default = "_skins";
                return _default;
            }
        }

        /// <summary>
        /// ģ�������·��
        /// </summary>
        public static string TemplateGroupBasePath 
        {
            get
            {
                    return SiteSkinsBasePath;
            }
        }

        /// <summary>
        /// ģ��·��
        /// </summary>
        public static string TemplateBasePath
        {
            get
            {
                if (EnableSiteSkins)
                {
                    return SiteSkinsBasePath;
                }
                else
                {
                    GeneralConfigInfo si = GeneralConfigs.GetConfig();
                    string _default = si.TemplateBasePath;
                    if (_default == null || _default == string.Empty)
                        _default = "cgi-bin\\templates";
                    return _default;
                }
            }
        }

        /// <summary>
        /// ģ���Url·��
        /// </summary>
        public static string TemplateUrlPath
        {
            get
            {
                string temp = TemplateBasePath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;

                if (EnableSiteSkins)
                {
                    //HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                    //CDHelper CDHelper = helperFactory.GetHelper<CDHelper>();
                    GeneralConfigInfo si = GeneralConfigs.GetConfig();
                    string _default = Path.GetFileNameWithoutExtension(si.DefaultTemplateGroupFileName);
                    temp = String.Format("{0}/{1}", temp, _default);
                }
                
                return temp;
            }
        }

        /// <summary>
        /// ����ģ��·��
        /// </summary>
        public static string TemplateLocalPath
        {
            get
            {
                string temp = TemplateBasePath;
                if (!temp.StartsWith("\\")) temp = "\\" + temp;
                return temp;
            }
        }

        /// <summary>
        /// ��ʱ�ļ�Ŀ¼
        /// </summary>
        public static string TempBasePath = "\\_temp";
        //public static string TemporaryPath = "Temp";
       
        //�˴������Ƶ�We7.CMS.Common��Channel.cs��
        public static string ChannelPath = "_data\\Channels";
        //�˴������Ƶ�We7.CMS.Common��Channel.cs��
        public static string ChannelUrlPath
        {
            get
            {
                string temp = ChannelPath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;
                //temp = "~" + temp;
                return temp;
            }
        }

        /// <summary>
        /// ����·������ /_data/2010/02/25/
        /// </summary>
        public static string AttachmentUrlPath
        {
            get
            {
                string year=DateTime.Today.ToString("yyyy");
                string month=DateTime.Today.ToString("MM");
                string day=DateTime.Today.ToString("dd");
                return string.Format("/_data/{0}/{1}/{2}/", year, month, day);
            }
        }

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public static string AliasPath = "Config\\Dictionary";

        /// <summary>
        /// ��ǩĿ¼
        /// </summary>
        public static string TagsPath = "Config\\Dictionary";
        
        /// <summary>
        /// �ؼ���Ŀ¼
        /// </summary>
        public static string KeywordsPath = "Config\\Dictionary";

        /// <summary>
        /// �ؼ������ļ����
        /// </summary>
        public static string ControlFileExtension = ".ascx.xml";

        /// <summary>
        /// ��ģ�������ļ����
        /// </summary>
        public static string TemplateFileExtension = ".xml";

        /// <summary>
        /// ģ���������ļ����
        /// </summary>
        public static string TemplateGroupFileExtension = ".xml";

        /// <summary>
        /// Ĭ�Ϸ�����
        /// </summary>
        public static int OwnerAccount = 0;

        /// <summary>
        /// Ĭ�Ͻ�ɫ
        /// </summary>
        public static int OwnerRole = 1;

        /// <summary>
        /// ������󳤶�
        /// </summary>
        public static int TitleMaxWord =20;

        /// <summary>
        /// ģ��汾��Ϣ�ļ�·��
        /// </summary>
        public static string TemplateVersionFileName = "TemplateVersion.config";

        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        public static string ThemePath = "theme";

        /// <summary>
        /// ��ҵ��ϢĿ¼
        /// </summary>
        public static string IndustryAttrXmlPath = "_data\\IndustrAttrXml";

        /// <summary>
        /// ����ģ��Ŀ¼
        /// </summary>
        public static string ContentModelXmlPath = "Config\\c-model";

        /// <summary>
        /// ��������Ŀ¼
        /// </summary>
        public static string Import3rdDataPath = "_data\\Import3rdData";

        /// <summary>
        /// ���ӻ����ģ��Ŀ¼����·��
        /// </summary>
        public static string VisualTemplateTemplateVirtualDirectory = "~/Admin/VisualTemplate/Templates/";
        /// <summary>
        /// ���ӻ����ģ��·������Ŀ¼·��
        /// </summary>
        public static string VisualTemplatePhysicalTemplateDirectory
        {
            get
            {

                return HttpContext.Current.Server.MapPath(VisualTemplateTemplateVirtualDirectory);
            }
        }

        /// <summary>
        /// ��װ��·��
        /// </summary>
        public static string WidgetsWrapperFolder = "~/Widgets/Wrapper/";
        /// <summary>
        /// ��װ������Ŀ¼·��
        /// </summary>
        public static string WidgetsWrapperFolderPhysicalDirectory
        {
            get
            {
                return HttpContext.Current.Server.MapPath(WidgetsWrapperFolder);
            }
        }
        /// <summary>
        /// ����������ģ������
        /// </summary>
        public const string ArticleModelName = "System.Article";
        /// <summary>
        /// ��������ģ��
        /// </summary>
        public const string AllInfomationModelName = "We7_Article_AllInfomation";

    }

    /// <summary>
    /// �ؼ���
    /// </summary>
    //public sealed class Keys
    //{
    //    private Keys() { }
    //    /// <summary>
    //    /// ҳ��ؼ���
    //    /// </summary>
    //    public const string QRYSTR_PAGEINDEX = "pg";

    //    /// <summary>
    //    /// Session�ؼ���
    //    /// </summary>
    //    internal const string SESSION_COOKIETEST = "CookieTest";
    //}
}
