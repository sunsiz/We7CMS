using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// ������Ŀ
    /// </summary>
    public class RssChannel:IXml
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public RssChannel() { }

        private string title;

        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string link;

        /// <summary>
        /// ���ӵ�ַ
        /// </summary>
        public string Link
        {
            get { return link; }
            set { link = value; }
        }

        private string description;

        /// <summary>
        /// ��ע��Ϣ
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string language;

        /// <summary>
        /// ����
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        private string lastBuildDate;

        /// <summary>
        /// �����ʱ��
        /// </summary>
        public string LastBuildDate
        {
            get { return lastBuildDate; }
            set { lastBuildDate = value; }
        }
        private string pubDate;

        public string PubDate
        {
            get { return pubDate; }
            set { pubDate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private string ttl;

        public string Ttl
        {
            get { return ttl; }
            set { ttl = value; }
        }
        private List<RssItem> rssitem;

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public List<RssItem> Rssitem
        {
            get { return rssitem; }
            set { rssitem = value; }
        }

        /// <summary>
        /// ��ȡXML��Ϣ
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(xd);

            //XmlProcessingInstruction newPI;
            //String PItext = "type='text/css'   href='/cgi-bin/templates/controls/styles/allrss.css'";
            //newPI = doc.CreateProcessingInstruction("xml-stylesheet", PItext);

            //doc.InsertBefore(newPI, doc.DocumentElement);   

            XmlElement xe = doc.CreateElement("rss");

            xe.SetAttribute("version", "2.0");
            //xe.SetAttribute("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            //xe.SetAttribute("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");
            //xe.SetAttribute("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
            //xe.SetAttribute("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
            xe.AppendChild(ToXml(doc));
            doc.AppendChild(xe);
            return doc;
        }

        /// <summary>
        /// װʵ��ת��Ϊxml
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("channel");
            CreateElement(doc, xe, "title", Title);
            CreateElement(doc, xe, "link", Link);
            CreateElement(doc, xe, "description", Description);
            CreateElement(doc, xe, "language", Language);
            CreateElement(doc, xe, "lastBuildDate", LastBuildDate);
            CreateElement(doc, xe, "pubDate", PubDate);
            CreateElement(doc, xe, "ttl", Ttl);
            CreateItemElement(doc, xe);
            return xe;
        }

        /// <summary>
        /// װxmlת��Ϊʵ����
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public IXml FromXml(XmlElement xe)
        {
            Title = getValue(xe, "title");
            link = getValue(xe, "link");
            description = getValue(xe, "description");
            language = getValue(xe, "language");
            lastBuildDate = getValue(xe, "lastBuildDate");
            pubDate = getValue(xe, "pubDate");
            ttl = getValue(xe, "ttl");
            getItemValues(xe);

            return this;
        }


        /// <summary>
        /// ����Item
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xe"></param>
        private void CreateItemElement(XmlDocument doc, XmlElement xe)
        {
            if (rssitem != null)
                foreach (RssItem ri in rssitem)
                {
                    xe.AppendChild(ri.ToXml(doc));
                }
        }


        /// <summary>
        /// ����Channel�е��ļ�
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xe"></param>
        /// <param name="element"></param>
        /// <param name="value"></param>
        private void CreateElement(XmlDocument doc, XmlElement xe, string element, string value)
        {
            XmlElement t = doc.CreateElement(element);
            t.InnerText = value;
            xe.AppendChild(t);
        }

        /// <summary>
        /// ��ȡ�ڵ�ֵ
        /// </summary>
        /// <param name="xe"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        private string getValue(XmlElement xe, string filed)
        {
            return xe.SelectSingleNode(filed).InnerText;
        }


        /// <summary>
        /// ����Ƿ���RssItem�ļ���
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private IList<RssItem> getItemValues(XmlElement doc)
        {
            List<RssItem> list = new List<RssItem>();
            XmlNodeList xns = doc.SelectNodes("item");
            foreach (XmlNode node in xns)
            {
                XmlElement xe = node as XmlElement;
                RssItem rss = new RssItem();
                rss.FromXml(xe);
                list.Add(rss);
            }
            return list;
        }

        #region IX ��Ա

        XmlElement IXml.ToXml(XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        IXml IXml.FromXml(XmlElement xe)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
