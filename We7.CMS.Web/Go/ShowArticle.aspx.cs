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
using We7.CMS.Common;
using We7.Framework.Util;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace We7.CMS.Web
{
    public partial class ShowArticle : FrontBasePage
    {
        
        /// <summary>
        /// ������
        /// </summary>
        protected override string GoHandler { get { return "channel"; } }

        protected override string ColumnMode
        {
            get
            {
                return "detail";
            }
        }

        string columnID;
        string articleID;

        /// <summary>
        /// ģ��·��
        /// </summary>
        protected override string TemplatePath { get; set; }

        protected override void Initialize()
        {

            columnID = ChannelHelper.GetChannelIDFromURL();
            articleID = ArticleHelper.GetArticleIDFromURL();

            AddClicks(articleID);

            //�����ͳ��
            AddStatistic(articleID, columnID);

            //��ʼ��TemplatePath
            string templatePath =IsHtmlTemplate? 
                 TemplateHelper.GetHtmlTemplateByHandlers("detail", columnID, null, null)
                :TemplateHelper.GetTemplateByHandlers("detail", columnID, null, null);

            TemplatePath = templatePath;
            if (!string.IsNullOrEmpty(templatePath))
            {
                if (!templatePath.StartsWith("/"))
                    templatePath = "/" + templatePath;

                if (File.Exists(Context.Server.MapPath(templatePath)))
                {
                    Control ctl = CheckControlByBuilder();

                    this.Controls.Add(ctl);
                    if (ctl != null)
                    {
                        if (this.Page.Header != null && this.Title != null)
                        {
                            this.Title = GetCurrentPageTitle(columnID, articleID);


                            Article thisArticle = ArticleHelper.GetArticle(articleID);
                            //meta���
                            HtmlGenericControl KeywordsMeta = new HtmlGenericControl("meta");
                            KeywordsMeta.Attributes["name"] = "keywords";
                            KeywordsMeta.Attributes["content"] = (thisArticle!=null&& !String.IsNullOrEmpty(thisArticle.KeyWord) && thisArticle.KeyWord.Length > 0) ?
                                thisArticle.KeyWord : CDHelper.Config.KeywordPageMeta;
                            this.Header.Controls.Add(KeywordsMeta);

                            HtmlGenericControl DescriptionMeta = new HtmlGenericControl("meta");
                            DescriptionMeta.Attributes["name"] = "description";
                            if (thisArticle!=null&&string.IsNullOrEmpty(thisArticle.DescriptionKey))
                                thisArticle.DescriptionKey = thisArticle.Summary;
                            DescriptionMeta.Attributes["content"] = (thisArticle!=null&&!String.IsNullOrEmpty(thisArticle.DescriptionKey) && thisArticle.DescriptionKey.Length > 0) ?
                                thisArticle.DescriptionKey : CDHelper.Config.DescriptionPageMeta;
                            this.Header.Controls.Add(DescriptionMeta);

                            //���ص����ͳ��js�ļ�
           
                            AddJavascriptFile2Header("/Scripts/jQuery/jquery-1.4.2.js");
                            AddJavascriptFile2Header("/admin/ajax/ClickRecord.js");
                        }
                    }
                }
                else
                {
                    Server.Transfer(TemplateGuideUrl, true);
                }
            }
            else
                Server.Transfer(TemplateGuideUrl, true);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void AddClicks(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Article a = ArticleHelper.GetArticle(id);
                if (a != null)
                {
                    a.Clicks = a.Clicks + 1;
                    ArticleHelper.UpdateArticle(a, new string[] { "Clicks" });
                }
            }
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
                catch
                {
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
                    if(!m[i].ToString().Contains("Src=\"/"))
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
                FileHelper.WriteFileEx(resultPath, strWriter.ToString(), false);
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
                    strWriter.Write("");
                };
                string content = strWriter.ToString();
                writer.Write(content);
            }
        }

        #endregion
    }
}