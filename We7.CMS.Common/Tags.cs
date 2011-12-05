using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ��ǩ��
    /// </summary>
    public class Tags
    {
        string id;
        string identifier;
        int frequency;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ��ʶ��
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Ƶ��
        /// </summary>
        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        public Tags()
        {
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("ChannelTags");
            xe.SetAttribute("id", ID);
            xe.SetAttribute("identifier", Identifier);
            return xe;
        }

        /// <summary>
        /// ��ȡXML��Ϣ
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public Tags FromXml(XmlElement xe)
        {
            id = xe.GetAttribute("id");
            identifier = xe.GetAttribute("identifier");
            return this;
        }
    }
}
