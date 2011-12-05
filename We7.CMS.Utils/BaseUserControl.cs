using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.Caching;


using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;
using We7.Framework.Config;
using We7.CMS.ShopService;

namespace We7.CMS
{
    /// <summary>
    /// ��̨�ؼ�������
    /// </summary>
    [Serializable]
    public class BaseUserControl : UserControl
    {
        #region ��������

        /// <summary>
        /// ҵ����󹤳�
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// վ�������Ϣҵ�����
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

        /// <summary>
        /// Ȩ��ҵ�����
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
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
        /// ģ��ҵ�����
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
		//protected LinkHelper LinkHelper
		//{
		//    get { return HelperFactory.GetHelper<LinkHelper>(); }
		//}

        /// <summary>
        /// ͳ��ҵ�����
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// ҳ�����ҵ�����
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
        protected AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.GetHelper<AttachmentHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected TagsHelper TagsHelper
        {
            get { return HelperFactory.GetHelper<TagsHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        /// <summary>
        /// ���ҵ�����
        /// </summary>
        protected ProcessingHelper ArticleProcessHelper
        {
            get { return HelperFactory.GetHelper<ProcessingHelper>(); }
        }

        /// <summary>
        /// �����ʷҵ�����
        /// </summary>
        protected ProcessHistoryHelper ArticleProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }

        /// <summary>
        /// ��������ҵ�����
        /// </summary>
        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }

        /// <summary>
        /// ��������ҵ�����
        /// </summary>
        protected ArticleIndexHelper ArticleIndexHelper
        {
            get { return HelperFactory.GetHelper<ArticleIndexHelper>(); }
        }

        /// <summary>
        /// �����ظ�ҵ�����
        /// </summary>
        protected AdviceReplyHelper AdviceReplyHelper
        {
            get { return HelperFactory.GetHelper<AdviceReplyHelper>(); }
        }

        /*
        /// <summary>
        /// �ʾ����ҵ�����
        /// </summary>
        protected QuestionnaireTypeHelper QuestionnaireTypeHelper
        {
            get { return HelperFactory.GetHelper<QuestionnaireTypeHelper>(); }
        }
        */


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
        /// ��ȡû��html���ŵ��ִ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual string GetClearHtml(string input)
        {
            return We7Helper.RemoveHtml(input);
        }

        /// <summary>
        /// ��ȡ�ַ���
        /// </summary>
        /// <param name="input">�����ִ�</param>
        /// <param name="count">�������</param>
        /// <param name="omit">ʡ�Է�</param>
        /// <returns></returns>
        public virtual string GetChopString(string input,int count,string omit)
        {
            string result = input;
            if(input.Length>count){
                result = We7.Framework.Util.Utils.CutString(input, 0, count-omit.Length);
                result += omit;
            }
            return result;
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
        /// ��ȡ�û�ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        /// <summary>
        /// ����û��Ƿ��¼
        /// </summary>
        protected bool IsSignin
        {
            get { return CurrentAccount != null; }
        }

        /// <summary>
        /// �˳�ʱ���Session
        /// </summary>
        protected void SignOut()
        {
            string result = AccountHelper.SignOut();
        }

        /// <summary>
        /// ����Ӧ�õ�����·��
        /// </summary>
        public string AppPath
        {
            get
            {
                return "/admin";
            }
        }

        /// <summary>
        /// ������·��
        /// </summary>
        public string ThemePath
        {
            get
            {
                string theme = SiteSettingHelper.Instance.Config.CMSTheme;
                if (theme == null || theme == "") theme = "classic";
                return "/admin/" + Constants.ThemePath + "/" + theme;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ��¼�û�
        /// </summary>
        protected string CurrentAccount
        {
            get { return Security.CurrentAccountID; }
        }

        /// <summary>
        /// ͨ���û�ID��ȡ�û�����
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        protected string GetAccountName(string accountID)
        {
            if (accountID != null && accountID !="")
            {
                if (accountID == We7Helper.EmptyGUID)
                {
                    return "ϵͳ����Ա";
                }
                else
                {
                    Account act = AccountHelper.GetAccount(accountID, new string[] { "LoginName", "LastName" });
                    if (act != null)
                    {
                        return !string.IsNullOrEmpty(act.LastName) ? act.LastName : act.LoginName;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// �����־
        /// </summary>
        /// <param name="pages">��־����ҳ��</param>
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
                    Page.ClientScript.RegisterStartupScript(GetType(), Guid.NewGuid().ToString(), "<script>alert('�Բ�����ʾվ���ֹ���棡')</script>");
                    return true;
                }
                return false;
            }
        } 
    }
}
