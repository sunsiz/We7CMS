using System;
using System.IO;
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
using System.Text;

using Thinkment.Data;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.CMS.Accounts;
using We7.Framework.Util;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace We7.CMS.Web
{
    /// <summary>
    /// ģ��·���ࣺ�ۺϴ����ѹ���-2009-12-17��
    /// </summary>
    public partial class Go : FrontBasePage
    {

        string ColumnID;

        string ColumnMode;

        string SearchWord
        {
            get { return Request["keyword"]; }
        }
        string SeSearchWord
        {
            get { return Request["sekeyword"]; }
        }

        string ColumnAlias
        {
            get { return Request["alias"]; }
        }

        string ArticleID;

        protected override string[] Permissions
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

        protected override string PermissionObjectID
        {
            get
            {
                if (ThisChannel != null)
                {
                    if (ThisChannel.SecurityLevel > 0)
                    {
                        if (AccountID == null)
                        {
                            Response.Redirect("/go/login.aspx?ReturnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
                        }
                        else
                            return ColumnID;
                    }
                }
                return base.PermissionObjectID;
            }
        }

        protected IPSecurityHelper IPSecurityHelper
        {
            get { return HelperFactory.GetHelper<IPSecurityHelper>(); }
        }

        protected override string TemplatePath { get; set; }        

        Channel ThisChannel;


        void Page_Init(object sender, System.EventArgs e)
        {
            if (!BaseConfigs.ConfigFileExist())
            {
                Response.Write("�������ݿ������ļ���δ���ɣ����������ݿ���δ����������Ҫ�������ݿ������ļ����������ݿ⡣���ڿ�ʼ��<a href='/install/index.aspx'><u>�����������ݿ�</u></a>");
                Response.End();
            }

            if (UnLoginCheck())
            {
                Response.Redirect("/go/login.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
            }

            Initialize();

            if (!CheckIPStrategy())
            {
                Response.Write("IP���ޣ�����IPû�����ܷ÷�Χ�ڣ�");
                Response.End();
                return;
            }

            if (TemplatePath != null)
            {
                if (File.Exists(Context.Server.MapPath(TemplatePath)))
                {
                    #region ҳ�洦�ӻ���Ҳ��Ҫ��ҳ�滺�������������
                    //����������ͨCache�ķ�����ȥ����ҳ�滺�棬����Ҳ��Ҫ
                    //�����������ȥ�滮��Ȼ���ҳ�ģ���Զ��ӻ���Ļ����뷽����
                    #endregion


                    Control ctl = LoadControl(TemplatePath);

                    this.Controls.Add(ctl);

                    try
                    {
                        if (this.Title != null)
                        {
                            this.Title = GetCurrentPageTitle(ColumnID, ArticleID);
                        }
                        //meta���
                        HtmlGenericControl KeywordsMeta = new HtmlGenericControl("meta");
                        KeywordsMeta.Attributes["name"] = "keywords";
                        string keyWordString = "";
                        keyWordString = CDHelper.Config.KeywordPageMeta;

                        HtmlGenericControl DescriptionMeta = new HtmlGenericControl("meta");
                        DescriptionMeta.Attributes["name"] = "description";
                        string descriptionWordString = "";
                        descriptionWordString = CDHelper.Config.DescriptionPageMeta;

                        if (ColumnID != "" && ArticleID != "")
                        {
                            Channel channel = ChannelHelper.GetChannel(ColumnID, null);
                            Article article = ArticleHelper.GetArticle(ArticleID);
                            if ((channel.KeyWord + article.KeyWord).Length > 0)
                                keyWordString = channel.KeyWord + article.KeyWord;
                            if ((channel.DescriptionKey + article.DescriptionKey).Length > 0)
                                descriptionWordString = channel.DescriptionKey + article.DescriptionKey;
                        }
                        else if (ColumnID == "" && ArticleID != "")
                        {
                            Article article = ArticleHelper.GetArticle(ArticleID);
                            if (article.KeyWord != null && article.KeyWord.Length > 0)
                                keyWordString = article.KeyWord;
                            if (article.DescriptionKey != null && article.DescriptionKey.Length > 0)
                                descriptionWordString = article.DescriptionKey;
                        }
                        else if (ColumnID != "" && ArticleID == "")
                        {
                            Channel channel = ChannelHelper.GetChannel(ColumnID, null);
                            if (channel.KeyWord != null && channel.KeyWord.Length > 0)
                                keyWordString = channel.KeyWord;
                            if (channel.DescriptionKey != null && channel.DescriptionKey.Length > 0)
                                descriptionWordString = channel.DescriptionKey;
                        }

                        KeywordsMeta.Attributes["content"] = keyWordString.ToString();
                        this.Header.Controls.Add(KeywordsMeta);

                        DescriptionMeta.Attributes["content"] = descriptionWordString.ToString();
                        this.Header.Controls.Add(DescriptionMeta);

                    }
                    catch (Exception ex)
                    {
                        We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    }

                    //�����ͳ��
                    AddStatistic();
                }
                else
                {
                    Response.Write("���棺����Ŀָ����ģ�岻���ڣ�������ָ������ģ���λ���Ƿ���ȷ��");
                }
            }
        }

        protected void Initialize()
        {
            bool enableCache = (CDHelper.Config.EnableCache == "true");
            string result = "";
            string curUrl = Context.Request.Path.Replace('/', '_');
            result = ChannelHelper.GetChannelIDFromURL();
            ColumnID = result;

            //��ʼ��ArticleID
            result = ArticleHelper.GetArticleIDFromURL();
            ArticleID = result;

            //��ʼ��ColumnMode
            result = GetColumnMode();
            ColumnMode = result;

            //��ʼ��ThisChannel
            Channel ch = null;
            if (ColumnID != null && ColumnID != "")
            {
                ch = ChannelHelper.GetChannel(ColumnID, null);
            }
            else if (ColumnAlias != null)
            {
                ch = ChannelHelper.GetChannelByAlias(ColumnAlias);
            }
            else
            {
                ch = null;
            }

            ThisChannel = ch;
            //��ʼ��TemplatePath
            result = TemplateHelper.GetThisPageTemplate(ColumnMode, ColumnID, SearchWord, SeSearchWord);
            if (result != null)
            {
                if (!result.StartsWith("/"))
                {
                    TemplatePath = "/" + result;
                }
                else
                {
                    TemplatePath = result;
                }
            }
        }

        void AddStatistic()
        {
            PageVisitor pv;
            if (Session[PageVisitorHelper.PageVisitorSessionKey] == null)
            {
                pv = PageVisitorHelper.AddPageVisitor(AccountID);
                Session[PageVisitorHelper.PageVisitorSessionKey] = pv;
            }
            else
                pv = (PageVisitor)Session[PageVisitorHelper.PageVisitorSessionKey];

            if (pv != null)
            {
                StatisticsHelper.AddStatistics(pv, ArticleID, ColumnID);
                TimeSpan ts = DateTime.Now - pv.OnlineTime;
                pv.Clicks += 1;
                if (ts.TotalSeconds > 10)//��Ϊ10�� ˢ������ʱ��
                {
                    pv.OnlineTime = DateTime.Now;
                    pv.LogoutDate = DateTime.Now;
                    PageVisitorHelper.UpdatePageVisitor(pv, new string[] { "OnlineTime", "LogoutDate","Clicks" });
                }
            }
            else
                Session[PageVisitorHelper.PageVisitorSessionKey] = null;
        }

        /// <summary>
        /// ��ȡǰ̨��ǰҳ����Ŀ��������
        /// </summary>
        /// <returns></returns>
        string GetColumnMode()
        {
            if (Request["mode"] != null)
                return Request["mode"];
            else
            {
                if (ArticleID != null && ArticleID != "")
                {
                    Article a = ArticleHelper.GetArticle(ArticleID);
                    string channelID = "";
                    if (a != null) channelID = a.OwnerID;
                    if (channelID != "")
                    {
                        Channel ch = this.ChannelHelper.GetChannel(channelID, new string[] { "EnumState" });
                        //string type = StateManagement.GetStateName(ch.EnumState, UserEnumLibrary.Business.ArticleType).ToString();
                        EnumLibrary.ArticleType type = (EnumLibrary.ArticleType)StateMgr.GetStateValueEnum(ch.EnumState, EnumLibrary.Business.ArticleType);
                        if (type == EnumLibrary.ArticleType.Product)
                        {
                            return "productDetail";
                        }
                        else if (type == EnumLibrary.ArticleType.Article)
                        {
                            return "detail";
                        }
                        else
                        {
                            return "contentMode";
                        }
                    }
                    else if (AdviceHelper.Exist(ArticleID))
                    {
                        return "adviceMode";
                    }
                    else
                    { return string.Empty; }
                }

                else
                    return string.Empty;
            }
        }


        private bool UnLoginCheck()
        {
            GeneralConfigInfo ci = GeneralConfigs.GetConfig();
            return ci.OnlyLoginUserCanVisit && !Security.IsAuthenticated();
        }

        bool CheckIPStrategy()
        {
            string ip = Context.Request.ServerVariables["REMOTE_ADDR"];
            return IPSecurityHelper.CheckIPStrategy(ip,ColumnID,ArticleID);
        }

        #region ��̬�����

        /// <summary>
        /// ��дRender����
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Request["Createhtml"]) && Request["Createhtml"] == "1")
            {
                StringWriter strWriter = new StringWriter();
                HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
                try
                {
                    base.Render(tempWriter);
                }
                catch (Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    strWriter.Write("");
                };

                //��ȡԭʼģ������
                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.OptionCheckSyntax = true;
                doc.OptionOutputOriginalCase = true;
                try
                {
                    doc.Load(Server.MapPath(TemplatePath), Encoding.UTF8);
                }
                catch(Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    throw new Exception("��ʽ��HTML����");
                }
                string strContent = doc.DocumentNode.InnerText;
                //��ȡ�ؼ�ע����Ϣ
                string pat = @"<%@[^>]*>";
                Regex reg = new Regex(pat);
                MatchCollection m = reg.Matches(strContent);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < m.Count; i++)
                {
                    string temp = m[i].Value;
                    if (!m[i].ToString().Contains("Src=\"/"))
                    {
                        temp = m[i].Value.Replace("Src=\"", "Src=\"" + TemplatePath.Remove(TemplatePath.LastIndexOf("/")) + "/");
                    }
                    sb.Append(temp + "\r\n");
                }
                //�����ɺ�ģ����ӿؼ�ע����Ϣ  
                string content = strWriter.ToString();
                string pat1 = @"<html[^>]*>";
                string RegAndHtml = sb.ToString() + "\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">";
                content = Regex.Replace(content, pat1, RegAndHtml, RegexOptions.IgnoreCase);
                content = content.Replace("<head>", "<head runat=\"server\">");

                writer.Write(content);
                string channelUrl = ChannelHelper.GetChannelUrlFromUrl(Context.Request.RawUrl, Context.Request.ApplicationPath);
                string fileName = "";
                if (string.IsNullOrEmpty(ColumnMode))
                {
                    fileName = "index.ascx";
                }
                else
                {
                    fileName = ColumnMode + ".ascx";
                }
                string resultPath = Server.MapPath(TemplatePath.Remove(TemplatePath.LastIndexOf("/")) + "/HtmlTemplate/" + channelUrl + fileName);
                FileHelper.WriteFile(resultPath, content);
            }
            else
            {
                StringWriter strWriter = new StringWriter();
                HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
                try
                {
                    base.Render(tempWriter);
                }
                catch (Exception ex)
                {
                    We7.Framework.LogHelper.WriteLog(typeof(Go), ex);
                    strWriter.Write("");
                };
                string content = strWriter.ToString();
                writer.Write(content);
            }
        }

        #endregion
    }
}