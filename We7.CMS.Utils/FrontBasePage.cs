using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;
using We7.CMS.Accounts;
using System.IO;
using System.Web.UI.HtmlControls;
using Thinkment.Data;
using We7.Framework.Util;
using System.Text.RegularExpressions;
using System.Reflection;

namespace We7.CMS
{
    /// <summary>
    /// ǰ̨ҳ�������
    /// </summary>
    public class FrontBasePage : Page,IDataAccessPage
    {

        /// <summary>
        /// ģ��·��
        /// </summary>
        protected virtual string TemplatePath
        {
            get;
            set;
        }

        #region load time
        /// <summary>
        /// ������
        /// </summary>
        public FrontBasePage()
        {
            processStartTime = DateTime.Now;
        }

        /// <summary>
        /// ��ǰҳ��ִ��ʱ��(����)
        /// </summary>
        private double processTimeSpan;

        /// <summary>
        /// �õ���ǰҳ�������ʱ�乩ģ���е���(��λ:����)
        /// </summary>
        /// <returns>��ǰҳ�������ʱ��</returns>
        public double ProcessTimeSpan
        {
            get { return processTimeSpan; }
        }

        /// <summary>
        /// ��ǰҳ�濪ʼ����ʱ��(����)
        /// </summary>
        private DateTime processStartTime;

        /// <summary>
        /// ��ҳ������Ŀ
        /// </summary>
        protected Channel ThisChannel
        {
            get
            {
                HttpContext Context = HttpContext.Current;
                string key = "thisChannel";
                Channel ch = Context.Items[key] as Channel;
                if (ch != null)
                    return ch;
                else
                {
                    string columnID = ChannelHelper.GetChannelIDFromURL();

                    //��ʼ��ThisChannel
                    if (!We7Helper.IsEmptyID(columnID))
                    {
                        ch = ChannelHelper.GetChannel(columnID, null);
                        Context.Items.Remove(key);
                        Context.Items.Add(key, ch);
                        return ch;
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public string ModelName
        {
            get
            {
                if (ThisChannel != null)
                    return ThisChannel.ModelName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// ģ��������URL
        /// </summary>
        public string TemplateGuideUrl
        {
            get
            {
                if (GeneralConfigs.GetConfig().StartTemplateMap)
                    return string.Format("/go/TemplateGuide.aspx?handler={0}&mode={1}&model={2}", GoHandler, ColumnMode, ModelName);
                else
                    return "/Errors.aspx?t=notemplate";
            }
        }

        #endregion

        #region helpers

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
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// ģ��ҵ�����
        /// </summary>
        protected TemplateHelper TemplateHelper
        {
            get { return HelperFactory.GetHelper<TemplateHelper>(); }
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
        /// ����ҵ�����
        /// </summary>
        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        /// <summary>
        /// ��վ������Ϣҵ�����
        /// </summary>
        protected SiteSettingHelper CDHelper
        {
            get { return HelperFactory.GetHelper<SiteSettingHelper>(); }
        }

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
        /// IP��ȫҵ�����
        /// </summary>
        protected IPSecurityHelper IPSecurityHelper
        {
            get { return HelperFactory.GetHelper<IPSecurityHelper>(); }
        }
        #endregion

        protected virtual string GoHandler { get { return ""; } }

        protected virtual string ColumnMode { get { return ""; } }

        protected bool IsHtmlTemplate
        {
            get
            {
                return GeneralConfigs.GetConfig().EnableHtmlTemplate && String.IsNullOrEmpty(Request["CreateHtml"]);
            }
        }

        /// <summary>
        /// ǰ̨ҳ������
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Response.Expires = -1;
                base.OnLoad(e);

                if (!BaseConfigs.ConfigFileExist())
                {
                    Response.Write("�������ݿ������ļ���δ���ɣ����������ݿ���δ����������Ҫ�������ݿ������ļ����������ݿ⡣���ڿ�ʼ��<a href='/install/index.aspx'><u>�����������ݿ�</u></a>");
                    return;
                }

                if (UnLoginCheck())
                {
                    Response.Redirect("/login.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl),false);
                    return;
                }

                if (!CheckIPStrategy())
                {
                    Response.Write("IP���ޣ�����IPû�����ܷ÷�Χ�ڣ�");                    
                    return;
                }

                if (!CheckPermission())
                {
                    Response.Write("��û��Ȩ�޷��ʴ���Ŀ��");                    
                    return;
                }

                Initialize();
            }
            catch (Exception ex)
            {
                We7.Framework.LogHelper.WriteLog(typeof(FrontBasePage), ex);
                DisplayError(ex.Message);
            }
        }

        /// <summary>
        /// ��ʼ����Ϣ
        /// </summary>
        protected virtual void Initialize()
        {
        }

        /// <summary>
        /// �������Ȩ�޴�
        /// </summary>
        protected virtual string[] Permissions
        {
            get
            {
                if (ThisChannel != null)
                {
                    if (ThisChannel.SecurityLevel > 0)
                    {
                        return new string[] { "Channel.Read" };
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// ��Ȩ����ID������Ŀ��ģ��ID
        /// </summary>
        protected virtual string PermissionObjectID
        {
            get
            {
                if (ThisChannel != null)
                {
                    if (ThisChannel.SecurityLevel > 0)
                    {
                        if (string.IsNullOrEmpty(AccountID))
                        {
                            Response.Redirect("/login.aspx?ReturnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl),false);
                        }
                        else
                            return ThisChannel.ID;
                    }
                }
                return We7Helper.EmptyGUID;
            }
        }

        /// <summary>
        /// ��ǰ��¼�û�ID
        /// </summary>
        protected virtual string AccountID
        {
            get
            {
                return Security.CurrentAccountID;
            }
        }

        /// <summary>
        /// ����û��Ƿ���Ȩ�޷��ʱ�ҳ
        /// </summary>
        protected virtual bool CheckPermission()
        {
            if (Permissions == null || AccountID == We7Helper.EmptyGUID)
            {
                return true;
            }

            // ���μ�����е�Ȩ��
            // TODO: ����һ��Ȩ�޻��棬����ÿ�ζ�ȥ�������ݿ�����ȡȨ����Ϣ
            List<string> ps = AccountHelper.GetPermissionContents(AccountID, PermissionObjectID);
            foreach (string r in Permissions)
            {
                foreach (string p in ps)
                {
                    if (p == r)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// �޷���Ȩ����ת
        /// </summary>
        protected virtual void HanldeNoPermission()
        {
            Server.Transfer("/Errors.aspx?t=permission");
        }

        /// <summary>
        /// ��ȡ��ǰҳ�������ò���
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public string GetCurrentPageTitle(string channelID, string articleID)
        {
            string titleFormart = CDHelper.GetDefaultHomePageTitle();

            if (articleID != "" && channelID != "")//����ҳ
            {
                titleFormart = CDHelper.GetDefaultContentPageTitle();
            }
            else if (channelID != "") //��Ŀҳ
            {
                titleFormart = CDHelper.GetDefaultChannelPageTitle();
            }
            return ParselFormatTitle(titleFormart, channelID, articleID);
        }

        /// <summary>
        /// ��ʽ������
        /// </summary>
        /// <param name="titleFormat">�����ʽ</param>
        /// <param name="channelID">��ĿID</param>
        /// <param name="articleID">����ID</param>
        /// <returns></returns>
        private string ParselFormatTitle(string titleFormat, string channelID, string articleID)
        {
            string channelParam = "{$ChannelName}";
            string articleParam = "{$ArticleTitle}";

            if (titleFormat.IndexOf(channelParam) > -1)
            {
                string chName = ChannelHelper.GetChannelName(channelID);
                titleFormat = titleFormat.Replace(channelParam, chName);
            }
            if (titleFormat.IndexOf(articleParam) > -1 && articleID != "")
            {
                string title = "";
                try
                {
                    Article ar = ArticleHelper.GetArticle(articleID, null);
                    if (ar != null)
                    {
                        if (ar.Title != null)
                        {
                            title = ar.Title;
                        }
                        else
                        {
                            title = "��ϸҳ";
                        }
                    }
                    else
                    {
                        title = "��ϸҳ";
                    }
                }
                catch (Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(FrontBasePage), ex);
                }

                titleFormat = titleFormat.Replace(articleParam, title);
            }

            return titleFormat;

        }

        /// <summary>
        /// ���IP����
        /// </summary>
        /// <returns></returns>
        bool CheckIPStrategy()
        {
            string ip = Context.Request.ServerVariables["REMOTE_ADDR"];
            string articleID = ArticleHelper.GetArticleIDFromURL();
            string channelID = string.Empty;
            if (ThisChannel != null) channelID = ThisChannel.ID;
            return IPSecurityHelper.CheckIPStrategy(ip, channelID, articleID);
        }

        /// <summary>
        /// δ��¼���
        /// </summary>
        /// <returns></returns>
        protected bool UnLoginCheck()
        {
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            return ci.OnlyLoginUserCanVisit && !Security.IsAuthenticated();

        }

        /// <summary>
        /// ����һ�����ͳ��
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="columnID"></param>
        protected void AddStatistic(string articleID, string columnID)
        {
            if (GeneralConfigs.GetConfig().StartPageViewModule)
            {
                PageVisitorHandler handler = new PageVisitorHandler();
                PageVisitor pv;
                if (Session[PageVisitorHelper.PageVisitorSessionKey] == null)
                {
                    pv = PageVisitorHelper.AddPageVisitor(AccountID);
                    Session[PageVisitorHelper.PageVisitorSessionKey] = pv;
                    handler.AddVisitor();
                }
                else
                    pv = (PageVisitor)Session[PageVisitorHelper.PageVisitorSessionKey];

                if (pv != null)
                {
                    StatisticsHelper.AddStatistics(pv, articleID, columnID);
                    handler.AddPageVisit();
                    TimeSpan ts = DateTime.Now - pv.OnlineTime;
                    if (ts.TotalSeconds > 10)//��10��ˢ������ʱ��
                    {
                        pv.OnlineTime = DateTime.Now;
                        PageVisitorHelper.UpdatePageVisitor(pv, new string[] { "OnlineTime" });
                    }
                }
                else
                    Session[PageVisitorHelper.PageVisitorSessionKey] = null;
            }
        }

        class PageVisitorHandler
        {
            VisiteCount vc = AppCtx.Cache.RetrieveObject<VisiteCount>(We7.CMS.PageVisitorHelper.VisiteCountCacheKey);
            public void AddVisitor()
            {
                if (vc != null)
                {
                    vc.DayVisitors = vc.DayVisitors + 1;
                    vc.MonthVisitors = vc.MonthVisitors + 1;
                    vc.OnlineVisitors = vc.OnlineVisitors + 1;
                    vc.TotalVisitors = vc.TotalVisitors + 1;
                    vc.YearVisitors = vc.YearVisitors + 1;
                    vc.AverageDayVisitors = vc.TotalVisitors / ((DateTime.Now - vc.StartDate).Days + 1);
                }
            }

            public void AddPageVisit()
            {
                if (vc != null)
                {
                    vc.DayPageview = vc.DayPageview + 1;
                    vc.MonthPageview = vc.MonthPageview + 1;
                    vc.TotalPageView = vc.TotalPageView + 1;
                    vc.YearPageview = vc.YearPageview + 1;
                    vc.AverageDayPageview = vc.TotalPageView / ((DateTime.Now - vc.StartDate).Days + 1);
                }
            }
        }

        /// <summary>
        /// ���һ��js�ļ����õ�Header
        /// </summary>
        /// <param name="src"></param>
        protected void AddJavascriptFile2Header(string src)
        {
            HtmlGenericControl scriptElement = new HtmlGenericControl("script");
            scriptElement.Attributes["src"] = src;
            scriptElement.Attributes["type"] = "text/javascript";
            this.Header.Controls.Add(scriptElement);
        }


        //protected override void Render(HtmlTextWriter writer)
        //{
        //    base.Render(writer);
        //    Response.Write("<br />" + DateTime.Now.Subtract(processStartTime).TotalMilliseconds / 1000);
        //}

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="error"></param>
        protected void DisplayError(string error)
        {
            string ErrMsg = error;
            Response.Write(ErrMsg);
            Response.End();
        }

        #region ���˴��󲿼�

        #region ����ƥ���ַ���
        string ALlRegisterPattetns = "<%@[\\s]*?Register[\\s]*?Src=\"(?<Src>[\\s|\\S]*?)\"[\\s|\\S]*?TagName=\"(?<TagName>[\\s|\\S]*?)\"[\\s|\\S]*?TagPrefix=\"(?<TagPrefix>[\\s|\\S]*?)\"[\\s]*?%>";   //���е�ע����Ϣ
        string ContentPattentn = "<{0}:{1}(?<paramet>[\\s|\\S]*?filename=\"{2}\"[\\s|\\S]*?)>[\\s]*?</{0}:{1}>"; //ָ��������Ϣʵ����Ϣ
        string RegisterPattetn = "<%@[\\s]*?Register[\\s]*?Src=\"{0}\"[\\s|\\S]*?TagName=\"{1}\"[\\s|\\S]*?TagPrefix=\"{2}\"[\\s]*?%>"; //ָ������ע����Ϣ 
        #endregion

        /// <summary>
        /// �������ֵ�
        /// </summary>
        private Dictionary<string, templetaInfo> ErrorDic = new Dictionary<string, templetaInfo>();
        public Control CheckControlByBuilder()
        {
            if (IsHtmlTemplate)
            {
                return LoadControl(TemplatePath);
            }
            Control ctl = null;

            string templateHtml = string.Empty;
            if (AppCtx.Cache.RetrieveObject(TemplatePath) != null)  //�߻���
            {
                templateHtml = AppCtx.Cache.RetrieveObject<string>(TemplatePath);
            }
            else
            {
                templateHtml = FileHelper.ReadFileWithLine(Context.Server.MapPath(TemplatePath), Encoding.UTF8);  //��ȡ�ļ�
                AppCtx.Cache.AddObjectWithFileChange(TemplatePath, templateHtml, new string[] { Context.Server.MapPath(TemplatePath) });
            }

            MatchCollection mc = Regex.Matches(templateHtml, ALlRegisterPattetns);//��ȡ����ע����Ϣ

            Dictionary<string, templetaInfo> dic = InstanceTempletaInfo(templateHtml, mc);

            ctl = InstanceControl(ctl, templateHtml, dic);

            return ctl;
        }

        /// <summary>
        /// ʵ�����ؼ�
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="templateHtml"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private Control InstanceControl(Control ctl, string templateHtml, Dictionary<string, templetaInfo> dic)
        {
            if (dic != null && dic.Count != 0)
            {
                foreach (var item in dic)
                {
                    //System.Diagnostics.Stopwatch swtime = new System.Diagnostics.Stopwatch();
                    //swtime.Start();

                    #region �������
                    try
                    {
                        //LoadControl(LoadControl(item.Key).GetType(), item.Value.Parameter);
                        Type t = LoadControl(item.Key).GetType();
                        object instance = t.Assembly.CreateInstance(t.FullName);
                        foreach (var field in item.Value.Parameter)  //�����ֶθ�ֵ
                        {
                            FieldInfo fieldinfo = t.GetField(field.Key, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.SetField);  //ȡ���ֶ���Ϣ
                            if (fieldinfo != null)
                            {
                                object c = Convert.ChangeType(field.Value, fieldinfo.FieldType); //��̬ת������
                                fieldinfo.SetValue(instance, c);    //��ֵ
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        item.Value.Error = ex.Message;//������Ϣ

                        ErrorDic.Add(item.Key, item.Value);  //��Ӵ��󲿼��ֵ�
                        We7.Framework.LogHelper.WriteLog(this.GetType(), ex);
                    }
                    #endregion

                    //swtime.Stop();
                    //We7.Framework.LogHelper.WriteFileLog("showpageTimeTest.txt", "��������ʱ�����", "����·����" + item.Key + "\n�������м���ʱ�䣺" + swtime.ElapsedMilliseconds.ToString() + "����");
                }
                if (ErrorDic == null || ErrorDic.Count == 0)  //���û�з�������,������������
                {
                    ctl = LoadControl(TemplatePath);
                }
                else  //����������
                {
                    string ErroAscx = string.Empty;
                    foreach (var item in ErrorDic)
                    {
                        ErroAscx = Regex.Replace(templateHtml, ContentPattentn.Replace("{0}", item.Value.Wew).Replace("{1}", item.Value.Tagname).Replace("{2}", item.Value.Src), "<span style='color:Red' title='" + item.Value.Error + "'>�˲�����������</span>");  //��ʾ������Ϣ
                        ErroAscx = Regex.Replace(ErroAscx, RegisterPattetn.Replace("{0}", item.Value.Src).Replace("{1}", item.Value.Tagname).Replace("{2}", item.Value.Wew), string.Empty);  //ȥ�����󲿼�ע����Ϣ
                    }

                    string errorPath = TemplatePath.Insert(TemplatePath.LastIndexOf('.'), ".error"); ;//����ģ�帱��·��
                    if (AppCtx.Cache.RetrieveObject<string>(errorPath) != null && AppCtx.Cache.RetrieveObject<string>(errorPath).Equals(ErroAscx)) //����л���
                    {
                        ctl = LoadControl(errorPath);
                    }
                    else
                    {
                        FileHelper.WriteFileEx(Context.Server.MapPath(errorPath), ErroAscx, false);  //д����ģ�帱��
                        AppCtx.Cache.AddObjectWithFileChange(errorPath, ErroAscx, new string[] { Context.Server.MapPath(errorPath) });  //��ӻ���
                        ctl = LoadControl(errorPath);
                    }

                }
            }
            return ctl;
        }

        #region ����ʵ����Ϣʵ����
        /// <summary>
        /// ʵ��������ʵ����Ϣ
        /// </summary>
        /// <param name="templateHtml"></param>
        /// <param name="mc"></param>
        /// <returns></returns>
        private Dictionary<string, templetaInfo> InstanceTempletaInfo(string templateHtml, MatchCollection mc)
        {
            Dictionary<string, templetaInfo> dic = new Dictionary<string, templetaInfo>();   //������Ϣ
            foreach (Match item in mc) //����ʵ��ʵ����
            {
                MatchCollection contentMc = Regex.Matches(templateHtml, ContentPattentn.Replace("{0}", item.Groups["TagPrefix"].Value).Replace("{1}", item.Groups["TagName"].Value).Replace("{2}", item.Groups["Src"].Value));   //�������ݼ���
                foreach (Match content in contentMc)
                {
                    Dictionary<string, object> Dicpara = getParameter(content.Groups["paramet"].Value); //��ȡ����

                    templetaInfo templeta = new templetaInfo() { Src = item.Groups["Src"].Value, Tagname = item.Groups["TagName"].Value, Wew = item.Groups["TagPrefix"].Value, Parameter = Dicpara };
                    if (dic != null && !dic.ContainsKey(templeta.Src))
                    {
                        dic.Add(templeta.Src, templeta);
                    }
                }
            }

            return dic;
        }
        #endregion

        /// <summary>
        /// ��ȡ�ֶ��ֵ�
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private Dictionary<string, object> getParameter(string value)
        {
            MatchCollection mc = Regex.Matches(value, "(?<key>[\\w]*?)=\"(?<value>[\\w\\W\\s]*?)\"");
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            foreach (Match item in mc)
            {
                if (!item.Groups["key"].Value.ToLower().Equals("control") && !item.Groups["key"].Value.ToLower().Equals("filename") && !item.Groups["key"].Value.ToLower().Equals("id") && !item.Groups["key"].Value.ToLower().Equals("runat")) //��������(��Щ�����ֶ�)
                {
                    dic.Add(item.Groups["key"].Value, item.Groups["value"].Value);
                }
            }
            return dic;
        }
        #endregion
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    class templetaInfo
    {
        private string src;

        public string Src
        {
            get { return src; }
            set { src = value; }
        }
        private string tagname;


        public string Tagname
        {
            get { return tagname; }
            set { tagname = value; }
        }

        private string wew;

        public string Wew
        {
            get { return wew; }
            set { wew = value; }
        }

        private Dictionary<string, Object> parameter;
        /// <summary>
        /// �ֶ�
        /// </summary>
        public Dictionary<string, Object> Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }
        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
    }
}
