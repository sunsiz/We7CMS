using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// ģ����Ƥ����Ϣ
    /// </summary>
    [Serializable]
    public class SkinInfo : IXml
    {
        string basePath;
        string fileName;
        string name;
        string description;
        DateTime created = DateTime.Now;
        private string ver;
        List<SkinItem> items;

        /// <summary>
        /// ģ����Ƥ����Ϣʵ�����
        /// </summary>
        public SkinInfo()
        {
            items = new List<SkinItem>();
        }

        /// <summary>
        /// ģ��������
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ԭʼ·��
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        /// <summary>
        /// ģ����Ƥ������
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }


        /// <summary>
        /// ģ����汾
        /// </summary>
        public string Ver
        {
            get { return ver; }
            set { ver = value; }
        }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
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
        /// ģ����Ƥ����Ϣ���϶���
        /// </summary>
        public List<SkinItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// ��ȡģ�������
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fn"></param>
        public void FromFile(string path, string fn)
        {
            BasePath = path;
            FileName = fn;
            XmlDocument doc = new XmlDocument();
            string file = Path.Combine(BasePath, FileName);
            if (File.Exists(file))
            {
                doc.Load(file);
                this.FromXml(doc.DocumentElement);
            }
            else
            {
                throw new Exception("û���ҵ�ģ����������ļ�" + fn);
            }
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fn"></param>
        public void ToFile(string path, string fn)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(dec);
            doc.AppendChild(this.ToXml(doc));
            doc.Save(Path.Combine(path, fn));
        }

        /// <summary>
        /// ģ����Ƥ����չ��
        /// </summary>
        [Serializable]
        public class SkinItem : IXml
        {
            string name;
            string description;
            string template;
            string templateText;
            string location;
            string c_model;
            string layout;
            string tag;
            string type;
            string c_modelText;
            string locationText;

            /// <summary>
            /// ����
            /// </summary>
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            /// ������Ϣ
            /// </summary>
            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            /// <summary>
            /// λ��
            /// </summary>
            public string Location
            {
                get { return location; }
                set { location = value; }
            }

            /// <summary>
            /// ����ģ��
            /// </summary>
            public string C_Model
            {
                get { return c_model; }
                set { c_model = value; }
            }

            /// <summary>
            /// ��������
            /// </summary>
            public string Layout
            {
                get { return layout; }
                set { layout = value; }
            }

            /// <summary>
            /// ��ǩ
            /// </summary>
            public string Tag
            {
                get { return tag; }
                set { tag = value; }
            }

            /// <summary>
            /// ģ���ļ����ƣ�.ascx��
            /// </summary>
            public string Template
            {
                get { return template; }
                set { template = value; }
            }

            /// <summary>
            /// ģ������
            /// </summary>
            public string TemplateText
            {
                get { return templateText; }
                set { templateText = value; }
            }

            /// <summary>
            /// ����
            /// </summary>
            public string Type
            {
                get { return type; }
                set { type = value; }
            }

            bool isSubTemplate;
            /// <summary>
            /// �Ƿ�����ģ��
            /// </summary>
            public bool IsSubTemplate
            {
                get { return isSubTemplate; }
                set { isSubTemplate = value; }
            }

            DateTime created = DateTime.Now;
            /// <summary>
            /// ��ǰģ�������ʱ��
            /// </summary>
            public DateTime Created
            {
                get { return created; }
                set { created = value; }
            }

            DateTime updated = DateTime.Now;
            /// <summary>
            /// ��ǰģ���������ʱ��
            /// </summary>
            public DateTime Updated
            {
                get { return updated; }
                set { updated = value; }
            }

            /// <summary>
            /// ģ��Text
            /// </summary>
            public string C_ModelText
            {
                get { return c_modelText; }
                set { c_modelText = value; }
            }

            /// <summary>
            /// λ��Text
            /// </summary>
            public string LocationText
            {
                get { return locationText; }
                set { locationText = value; }
            }

            /// <summary>
            /// ���ģ��������Ϣ
            /// </summary>
            /// <param name="doc"></param>
            /// <returns></returns>
            public XmlElement ToXml(XmlDocument doc)
            {
                XmlElement xe = doc.CreateElement("Item");
                xe.SetAttribute("name",Name);
                xe.SetAttribute("template", Template);
                xe.SetAttribute("location", Location);
                xe.SetAttribute("c_model", C_Model);
                xe.SetAttribute("layout", Layout);
                xe.SetAttribute("tag",Tag);
                xe.SetAttribute("type",Type);
                xe.SetAttribute("description", Description);
                xe.SetAttribute("created", Created.ToString());
                xe.SetAttribute("updated", Updated.ToString());
                xe.SetAttribute("isSub", IsSubTemplate ? Boolean.TrueString : Boolean.FalseString);
                xe.SetAttribute("c_modelText",C_ModelText);
                xe.SetAttribute("locationText", LocationText);
                return xe;
            }

            /// <summary>
            /// ��ȡģ��������Ϣ
            /// </summary>
            /// <param name="xe"></param>
            /// <returns></returns>
            public IXml FromXml(XmlElement xe)
            {
                name = xe.GetAttribute("name");
                template = xe.GetAttribute("template");
                location = xe.GetAttribute("location");
                c_model = xe.GetAttribute("c_model");
                layout = xe.GetAttribute("layout");
                tag = xe.GetAttribute("tag");
                type = xe.GetAttribute("type");
                description = xe.GetAttribute("description");
                if(xe.GetAttribute("created") != null && xe.GetAttribute("created").Trim() != "")
                    created = Convert.ToDateTime(xe.GetAttribute("created"));
                if (xe.GetAttribute("updated") != null && xe.GetAttribute("updated").Trim() != "")
                    updated = Convert.ToDateTime(xe.GetAttribute("updated"));
                IsSubTemplate = xe.GetAttribute("isSub") == Boolean.TrueString;
                c_modelText = xe.GetAttribute("c_modelText");
                locationText = xe.GetAttribute("locationText");
                return this;
            }
        }

        /// <summary>
        /// ���ģ������Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("TempateGroup");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("created", Created.ToString());
            xe.SetAttribute("ver", Ver);
            foreach (SkinItem it in Items)
            {
                xe.AppendChild(it.ToXml(doc));
            }
            return xe;
        }

        /// <summary>
        /// ���ģ������Ϣ
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public IXml FromXml(XmlElement xe)
        {
            Name = xe.GetAttribute("name");
            Description = xe.GetAttribute("description");
            xe.GetAttribute("");
            Created = Convert.ToDateTime(xe.GetAttribute("created"));
            Ver = xe.GetAttribute("ver");
            foreach (XmlElement node in xe.SelectNodes("Item"))
            {
                SkinItem it = new SkinItem();
                it.FromXml(node);
                Items.Add(it);
            }
            return this;
        }
    }
}
