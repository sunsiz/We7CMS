using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// ��Դ�ļ������ڿؼ���ģ��
    /// </summary>
    [Serializable]
    public class ResourceFile : IXml, IJsonResult
    {
        string type;
        string fileName;

        public ResourceFile()
        {
        }

        public ResourceFile(string tp, string fn)
        {
            type = tp;
            fileName = fn;
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("File");
            xe.SetAttribute("fileName", FileName);
            xe.SetAttribute("type", Type);
            return xe;
        }

        /// <summary>
        /// ��ȡXML��Ϣ
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public IXml FromXml(XmlElement xe)
        {
            FileName = xe.GetAttribute("fileName");
            Type = xe.GetAttribute("type");
            return this;
        }

        /// <summary>
        /// ���ΪJson��ʽ�ı�
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("{0}:'{1}',", "fileName", fileName);
            sb.AppendFormat("{0}:'{1}'", "type", type);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
