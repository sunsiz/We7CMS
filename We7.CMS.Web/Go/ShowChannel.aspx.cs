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
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.Framework.Util;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace We7.CMS.Web
{
    public partial class ShowChannel : FrontBasePage
    {
        /// <summary>
        /// ������
        /// </summary>
        protected override string GoHandler { get { return "channel"; } }

        string ColumnID;

        /// <summary>
        /// ģ���ӷ��࣬���¼��ע�ᡢ������
        /// </summary>
        protected override string ColumnMode
        {
            get
            {
                if (Request["mode"] != null)
                    return Request["mode"].ToString();
                else
                    return "";
            }
        }

        string SearchWord
        {
            get { return Request["keyword"]; }
        }


        protected override string TemplatePath { get; set; }
        protected override void Initialize()
        {
            ColumnID = ChannelHelper.GetChannelIDFromURL();

            //��ʼ��ThisChannel
            if (ThisChannel != null)
            {
                if (ThisChannel.Type == ((int)TypeOfChannel.ReturnChannel).ToString() && !string.IsNullOrEmpty(ThisChannel.ReturnUrl))
                {
                    //��ת���ͽ�����ת
                    Response.Redirect(ThisChannel.ReturnUrl, true);
                }
            }

            //��ʼ��TemplatePath
            string result = IsHtmlTemplate ? TemplateHelper.GetHtmlTemplateByHandlers(ColumnMode, ColumnID, SearchWord, "") :
                TemplateHelper.GetTemplateByHandlers(ColumnMode, ColumnID, SearchWord, null);
            if (!string.IsNullOrEmpty(result))
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

            if (!string.IsNullOrEmpty(TemplatePath))
            {
                if (File.Exists(Context.Server.MapPath(TemplatePath)))
                {
                   // Control ctl = LoadControl(TemplatePath);
                    Control ctl = CheckControlByBuilder();
                    if (ctl != null)
                    {
                        this.Controls.Add(ctl);
                        if (this.Page.Header != null && this.Title != null)
                        {
                            this.Title = GetCurrentPageTitle(ColumnID, "");

                            //meta���
                            HtmlGenericControl KeywordsMeta = new HtmlGenericControl("meta");
                            KeywordsMeta.Attributes["name"] = "keywords";
                            string strContent = "";
                            if (ThisChannel != null && ThisChannel.KeyWord != null && ThisChannel.KeyWord.Length > 0)
                            {
                                strContent = ThisChannel.KeyWord;
                            }
                            else
                            {
                                strContent = GeneralConfigs.GetConfig().KeywordPageMeta;
                            }
                            KeywordsMeta.Attributes["content"] = strContent;
                            this.Header.Controls.Add(KeywordsMeta);

                            HtmlGenericControl DescriptionMeta = new HtmlGenericControl("meta");
                            DescriptionMeta.Attributes["name"] = "description";
                            string strDescriptionMetaContent = "";
                            if (ThisChannel != null && ThisChannel.DescriptionKey != null && ThisChannel.DescriptionKey.Length > 0)
                            {
                                strDescriptionMetaContent = ThisChannel.DescriptionKey;
                            }
                            else
                            {
                                strDescriptionMetaContent = GeneralConfigs.GetConfig().DescriptionPageMeta;
                            }
                            DescriptionMeta.Attributes["content"] = strDescriptionMetaContent;
                            this.Header.Controls.Add(DescriptionMeta);
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
            //�����ͳ��
            AddStatistic("", ColumnID);
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
                FileHelper.WriteFileEx(resultPath, content, false);           
            }
            else
            {
                base.Render(writer);
            }
        }

        #endregion
    }
}