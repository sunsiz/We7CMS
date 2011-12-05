using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;

using We7.CMS.Config;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using System.Xml;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;
using Thinkment.Data;
using We7.CMS.ShopService;
using System.Text;

namespace We7.CMS
{
    /// <summary>
    /// ��̨ҳ�������
    /// </summary>
    public partial class BasePage : Page
    {
        #region helper reference
        /// <summary>
        /// Ȩ��ҵ�����
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// ҵ����󹤳�
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return HelperFactory.Instance; }
        }

        /// <summary>
        /// ģ��ҵ�����
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }

        /// <summary>
        /// �û��ؼ�ҵ�����
        /// </summary>
        protected DataControlHelper DataControlHelper
        {
            get { return HelperFactory.GetHelper<DataControlHelper>(); }
        }

        /// <summary>
        /// ��Ŀҵ�����
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// ͨ����Ϣҵ�����
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        /// <summary>
        /// �˵�ҵ�����
        /// </summary>
        protected MenuHelper MenuHelper
        {
            get { return HelperFactory.GetHelper<MenuHelper>(); }
        }


        /// <summary>
        /// �����Ϣҵ�����
        /// </summary>
        protected ProcessingHelper ProcessHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }

        /// <summary>
        /// �����ʷҵ�����
        /// </summary>
        protected ProcessHistoryHelper ProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }

        /// <summary>
        /// ����ͳ��ҵ�����
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// ҳ�����ͳ�ƶ���
        /// </summary>
        protected PageVisitorHelper PageVisitorHelper
        {
            get { return HelperFactory.GetHelper<PageVisitorHelper>(); }
        }

        /// <summary>
        /// ��־ҵ�����
        /// </summary>
        protected LogHelper LogHelper
        {
            get { return HelperFactory.GetHelper<LogHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
		//protected LinkHelper LinkHelper
		//{
		//    get { return HelperFactory.GetHelper<LinkHelper>(); }
		//}

        /// <summary>
        /// ��Ϣҵ�����
        /// </summary>
        protected MessageHelper MessageHelper
        {
            get { return HelperFactory.GetHelper<MessageHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }

        /// <summary>
        /// ��ǩҵ�����
        /// </summary>
        protected TagsHelper TagsHelper
        {
            get { return HelperFactory.GetHelper<TagsHelper>(); }
        }

        /// <summary>
        /// �汾��Ϣҵ�����
        /// </summary>
        protected TemplateVersionHelper TemplateVersionHelper
        {
            get { return HelperFactory.GetHelper<TemplateVersionHelper>(); }
        }

        /// <summary>
        /// ��������ҵ�����
        /// </summary>
        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        /// <summary>
        /// �����ظ�ҵ�����
        /// </summary>
        protected AdviceReplyHelper AdviceReplyHelper
        {
            get { return HelperFactory.GetHelper<AdviceReplyHelper>(); }
        }

        /// <summary>
        /// �����ҵ�����
        /// </summary>
        protected ClickRecordHelper ClickRecordHelper
        {
            get { return HelperFactory.GetHelper<ClickRecordHelper>(); }
        }

        /*
        /// <summary>
        /// �ʾ�������ҵ�����
        /// </summary>
        protected QuestionnaireTypeHelper QuestionnaireTypeHelper
        {
            get { return HelperFactory.GetHelper<QuestionnaireTypeHelper>(); }
        }

        /// <summary>
        /// �ʾ�ҵ�����
        /// </summary>
        protected QuestionnaireHelper QuestionnaireHelper
        {
            get { return HelperFactory.GetHelper<QuestionnaireHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected QuestionHelper QuestionHelper
        {
            get { return HelperFactory.GetHelper<QuestionHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected OptionHelper OptionHelper
        {
            get { return HelperFactory.GetHelper<OptionHelper>(); }
        }

        /// <summary>
        /// ���ҵ�����
        /// </summary>
        protected AnswerSheetHelper AnswerSheetHelper
        {
            get { return HelperFactory.GetHelper<AnswerSheetHelper>(); }
        }
        */
        #endregion

        #region ��������
        private string loginMeth = "";

        public string LoginMeth
        {
            get { return loginMeth; }
            set { loginMeth = value; }
        }

        /// <summary>
        /// ����Ӧ�õ�����·��
        /// </summary>
        public string AppPath
        {
            get
            {
                if (MasterPageIs == MasterPageMode.User)
                    return "";
                else
                    return "/admin";
            }
        }

        private string humanLoginType;
        /// <summary>
        /// ��½��ʽ����
        /// </summary>
        public string HumanLoginType
        {
            get { return humanLoginType; }
            set { humanLoginType = value; }
        }

        /// <summary>
        /// ȡ�õ�ǰCookie
        /// </summary>
        /// <returns></returns>
        protected HttpCookie GetCookie()
        {
            return Request.Cookies["wethepowerseven"];
        }

        OwnerRank MenuOwner
        {
            get
            {
                if (MasterPageIs == MasterPageMode.User)
                    return OwnerRank.Normal;
                else
                    return OwnerRank.Admin;
            }
        }

        #endregion

        #region ����������
        /// <summary>
        /// ��ҳ���ʼ��֮ǰ����masterpageֵ���ı�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (MasterPageIs != MasterPageMode.None)
            {
                string theme = GeneralConfigs.GetConfig().CMSTheme;
                if (theme == null || theme == "") theme = "classic";

                if (MasterPageIs == MasterPageMode.FullMenu)
                {
                    string url = "~/admin/" + Constants.ThemePath + "/" + theme + "/content.Master";
                    Page.MasterPageFile = url;
                }
                else if (MasterPageIs == MasterPageMode.NoMenu)
                {
                    string url = "~/admin/" + Constants.ThemePath + "/" + theme + "/ContentNoMenu.Master";
                    Page.MasterPageFile = url;
                }
                else if (MasterPageIs == MasterPageMode.User)
                {
                    string url = "~/User/DefaultMaster/content.Master";
                    string masterFile = Path.Combine(TemplateHelper.DefaultTemplateGroupPath, "content.Master");
                    if (File.Exists(masterFile))
                    {
                        url = string.Format("{0}/{1}", Constants.TemplateUrlPath, "content.Master");
                    }
                    Page.MasterPageFile = url;
                }
            }
        }

        /// <summary>
        /// ��ȡ������·��
        /// </summary>
        public string ThemePath
        {
            get
            {
                string theme = GeneralConfigs.GetConfig().CMSTheme;
                if (theme == null || theme == "") theme = "classic";
                return "/admin/" + Constants.ThemePath + "/" + theme;
            }
        }

        /// <summary>
        /// ��ȡվ������
        /// </summary>
        public string ThisSiteName
        {
            get
            {
                return SiteConfigs.GetConfig().SiteName;
            }
        }

        /// <summary>
        /// ��ǰ�û�ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        #endregion

        #region We7����̳����
        private ShopService.ShopService _ShopService;
        /// <summary>
        /// �̳�Service��ַ todo
        /// </summary>
        public ShopService.ShopService ShopService
        {
            get
            {
                if (_ShopService == null)
                {
                    _ShopService = new ShopService.ShopService();
                    _ShopService.Timeout = 10000;
                    _ShopService.Url = GeneralConfigs.GetConfig().ShopService.TrimEnd('/').TrimEnd('\\') + "/Plugins/ShopPlugin/ShopService.asmx";
                }
                return _ShopService;
            }
        }

        /// <summary>
        /// ͨ��Web Service Ping�ӿڲ��Բ���̳ǽӿ��Ƿ����
        /// </summary>
        /// <returns>true������</returns>
        public virtual bool IsShopServicesCanWork()
        {
            try
            {
                string result = ShopService.Ping();                     
                return result.Equals("Pong");
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return false;
            }
        }


        /// <summary>
        /// ��ȡ�Ƽ�����
        /// </summary>
        /// <param name="count">����</param>
        /// <returns></returns>
        public List<StoreModel> GetRecommendStore(int count)
        {
            try
            {
                StoreModel[] stores = ShopService.GetRecommendStore(count);
                List<StoreModel> listStores = null;
                if (stores.Length > 0)
                {
                    listStores = new List<StoreModel>(stores);
                }
                return listStores;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return null;
            }
        }

        /// <summary>
        /// ��ȡ�Ƽ���Ʒ
        /// </summary>
        /// <param name="count">����</param>
        /// <returns></returns>
        public List<ProductInfo> GetRecommendProduct(int count)
        {
            try
            {
                List<ProductInfo> listProducts = null;
                ProductInfo[] products = ShopService.GetRecommendProduct(count);
                if (products.Length > 0)
                {
                    listProducts = new List<ProductInfo>(products);
                }
                return listProducts;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return null;
            }
        }

        /// <summary>
        /// ��ȡ������Ӧ���Ǻ��ִ�
        /// </summary>
        /// <param name="str">0-6,����</param>
        /// <returns>3��,����������</returns>
        public virtual string GetLevelString(string str)
        {
            int stars = 0;
            int.TryParse(str, out stars);

            int max = 5;
            int nostar = max - stars;
            StringBuilder sb = new StringBuilder();
            sb.Append(new string('��', stars));
            sb.Append(new string('��', nostar));

            return sb.ToString();
        }

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="input">�����ִ�</param>
        /// <param name="count">�������</param>
        /// <param name="omit">ʡ�Է�</param>
        /// <returns></returns>
        public virtual string GetChopString(string input, int count, string omit)
        {
            string result = input;
            if (input.Length > count)
            {
                result = We7.Framework.Util.Utils.CutString(input, 0, count - omit.Length);
                result += omit;
            }
            return result;
        }

        /// <summary>
        /// ��ȡû��html���ŵ��ִ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual string GetClearHtml(string input)
        {
            return We7Helper.RemoveHtml(input);
        }

        /// <summary>
        /// ��ȡ��ѵ�ģ��
        /// </summary>
        /// <param name="count">����</param>
        /// <returns></returns>
        public List<ProductInfo> GetFreeTemplates(int count)
        {
            try
            {
                List<ProductInfo> listProducts = null;
                ProductInfo[] products = ShopService.GetRecommendProductByType(count,"mb",-1);
                if (products.Length > 0)
                {
                    listProducts = new List<ProductInfo>(products);
                }
                return listProducts;
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
                return null;
            }
        }

        /// <summary>
        /// ��ȡ�ļ��ߴ��Ӧ��M��
        /// </summary>
        /// <param name="productSize">�ֽ���</param>
        /// <returns></returns>
        public string GetProductFileSize(string productSize)
        {
            string result=string.Empty;
            int sizes;
            int.TryParse(productSize, out sizes);
            if (sizes == 0)
                result = "0";
            if (sizes > 0)
                result = ((double)sizes / (double)1048576).ToString("f2");
            return result + "M";
        }

        /// <summary>
        /// ���ݼ۸��ֶβ�ѯ��Ʒ�Ƿ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsFree(object input)
        {
            int price = 0;
            int.TryParse(input.ToString(), out price);

            return price.Equals(0);
        }

        /// <summary>
        /// վ���Ƿ���̳�
        /// </summary>
        /// <returns></returns>
        public bool IsSiteBindShop()
        {
            string sln = SiteConfigs.GetConfig().ShopLoginName.Trim();
            if (string.IsNullOrEmpty(sln))
                return false;

            try
            {
                //�ʺż���
                SiteConfigInfo si = SiteConfigs.GetConfig();
                string[] states = ShopService.CheckSite(si.ShopLoginName, si.ShopPassword, si.SiteUrl);
                if (states != null && states.Length > 0 && states[0] == "1")
                    return true;
                return false;
            }
            catch(Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage),ex);
                return false;
            }
        }
        #endregion

        /// <summary>
        /// ҳ�����
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Response.Expires = -1;

                if (IsCheckInstallation)
                {
                    CheckInstallation();
                }
                if (!IsPostBack)
                {
                    if (NeedAnAccount)
                    {
                        CheckSignin();
                    }
                    if (NeedAnPermission)
                    {
                        CheckPermission();
                    }
                    Initialize();
                }
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(BasePage), ex);
            }
        }
         
        /// <summary>
        /// �Ƿ����Ѱ�װ
        /// </summary>
        protected virtual bool IsCheckInstallation
        {
            get { return true; }
        }

        /// <summary>
        /// �Ƿ���Ҫ��¼
        /// </summary>
        protected virtual bool NeedAnAccount
        {
            get { return true; }
        }

        /// <summary>
        /// �Ƿ��ж��û�Ȩ��
        /// </summary>
        protected virtual bool NeedAnPermission
        {
            get { return true; }
        }

        /// <summary>
        /// ������һ��ĸ��-masterpage
        /// </summary>
        protected virtual MasterPageMode MasterPageIs
        {
            get { return MasterPageMode.FullMenu; }
        }

        /// <summary>
        /// ����û��Ƿ��Ѿ���¼
        /// </summary>
        protected virtual void CheckSignin()
        {
            if (!Security.IsAuthenticated())
            {
                Account a = null;
                if (SiteConfigs.GetConfig().SiteGroupEnabled)
                    a = AccountHelper.GetAuthenticatedAccount();
                if (a == null)
                {
                    Response.Redirect(AppPath + "/Signin.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl), false);
                }
            }
        }


        /// <summary>
        /// ����û������ݿ������ļ��Ƿ��Ѿ�����
        /// </summary>
        protected virtual void CheckInstallation()
        {
            if (!BaseConfigs.ConfigFileExist())
            {
                Response.Write("�������ݿ������ļ���δ���ɣ����������ݿ���δ����������Ҫ�������ݿ������ļ����������ݿ⡣���ڿ�ʼ��<a href='/install/index.aspx'><u>�����������ݿ�</u></a>");
                Response.End();
            }
        }


        /// <summary>
        /// ����û��Ƿ���Ȩ�޷��ʱ�ҳ
        /// </summary>
        protected virtual void CheckPermission()
        {
            if (!NeedAnPermission || AccountID == We7Helper.EmptyGUID)
            {
                return;
            }
            string errorPage = Request.Url.Host + ":" + Request.Url.Port.ToString() + AppPath + "/Errors.aspx";
            if (HttpContext.Current.Session["ALLMENUURL"]!=null && HttpContext.Current.Session["ALLMENUURL"].ToString() == errorPage)
                return;

            // ���Ȩ��
           if(!MenuHelper.URLHavePermission(HttpContext.Current,MenuOwner))
           {
                HanldeNoPermission();
            }
        }

        /// <summary>
        /// ���һ���û�û�κ�Ȩ�޾���ת������ҳ
        /// </summary>
        protected virtual void HanldeNoPermission()
        {
            HttpContext context = HttpContext.Current;
            if (context.Request["iframe"] != null)
                Response.Redirect("/Errors.aspx?t=permission&iframe=1",false);
               // Server.Transfer("/Errors.aspx?t=permission&iframe=1");
            else
                Response.Redirect("/Errors.aspx?t=permission&iframe=1", false);
                //Server.Transfer("/Errors.aspx?t=permission");
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="e"></param>
        protected virtual void HandleException(Exception e)
        {
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// �ǳ�
        /// </summary>
        protected string SignOut()
        {
            return AccountHelper.SignOut();
        }

        /// <summary>
        /// �����־
        /// </summary>
        /// <param name="pages">ҳ��</param>
        /// <param name="content">��־����</param>
        protected void AddLog(string pages, string content)
        {
            if (CDHelper.Config.IsAddLog)
            {
                LogHelper.WriteLog(AccountID, pages, content, CDHelper.Config.DefaultHomePageTitle);
            }
        }

        /// <summary>
        /// ��ʾվ����ʾ��Ϣ
        /// </summary>
        protected bool DemoSiteMessage
        {
            get
            {
                if (GeneralConfigs.GetConfig().IsDemoSite)
                {
                    ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "<script>alert('�Բ��𣬴���ʾվ����û�иò���Ȩ�ޣ�')</script>");
                    return true;
                }
                return false;
            }
        } 

    }
}
