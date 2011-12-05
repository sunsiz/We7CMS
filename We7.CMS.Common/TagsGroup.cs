using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// ϵͳ��ǩ�࣬����һ���ǩ
    /// </summary>
    [Serializable]
    public class TagsGroup : IXml
    {
        string name;
        string basePath;
        string fileName;
        List<Item> items;

        public TagsGroup()
        {
            items = new List<Item>();
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// �̳�·��
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public List<Item> Items
        {
            get { return items; }
        }

        /// <summary>
        /// ��ȡģ����·��
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
        /// ģ���鱣��·��
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fn"></param>
        public void ToFile(string path, string fn)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "gb2312", "");
            doc.AppendChild(dec);
            doc.AppendChild(this.ToXml(doc));
            doc.Save(Path.Combine(path, fn));
        }

        [Serializable]
        public class Item : IXml
        {
            string aliasType;

            string words;

            public string Words
            {
                get { return words; }
                set { words = value; }
            }

            public string Type
            {
                get { return aliasType; }
                set { aliasType = value; }
            }           

            public IXml FromXml(XmlElement xe)
            {
                words = xe.GetAttribute("words");
                aliasType = xe.GetAttribute("type");
                return this;
            }
            public XmlElement ToXml(XmlDocument doc)
            {
                XmlElement xe = doc.CreateElement("Item");
                return xe;
            }
        }
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Tags");
            return xe;
        }
        public IXml FromXml(XmlElement xe)
        {
            foreach (XmlElement node in xe.SelectNodes("Item"))
            {
                Item it = new Item();
                it.FromXml(node);
                Items.Add(it);
            }
            return this;
        }
    }
}

