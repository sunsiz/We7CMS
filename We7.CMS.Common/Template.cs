using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ģ��ҳ����
    /// </summary>
    [Serializable]
    public class Template
    {
        string name;
        string description;
        string fileName;
        string basePath;
        bool isSubTemplate;
        bool isCode;
        DateTime created=DateTime.Now;
        DateTime updated = DateTime.Now;
        List<string> controls;
        List<ResourceFile> files;
        string templateType;
        string templateTypeText;

        /// <summary>
        /// ģ��ʵ�����
        /// </summary>
        public Template()
        {
            controls = new List<string>();
            files = new List<ResourceFile>();
        }

        /// <summary>
        /// ģ������
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> Controls
        {
            get { return controls; }
        }
        
        public List<ResourceFile> Files
        {
            get { return files; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
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
        /// ·��
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
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

        /// <summary>
        /// �Ƿ�����ģ��
        /// </summary>
        public bool IsSubTemplate
        {
            get { return isSubTemplate; }
            set { isSubTemplate = value; }
        }

        /// <summary>
        /// �Ƿ����༭
        /// </summary>
        public bool IsCode
        {
            get { return isCode; }
            set { isCode = value; }
        }

        /// <summary>
        /// �Ƿ�ĸ��
        /// </summary>
        public bool IsMasterPage { get; set; }

        public string IsSubTemplateText
        {
            get 
            {
                if (IsSubTemplate)
                    return "��ģ��";
                else if (TemplateType == "9")
                    return "ĸ��";
                else
                    return "";
            }
        }


        /// <summary>
        /// ģ�����͡���ͨģ�� 1����ģ�� 0��ĸ�� 9
        /// </summary>
        public string TemplateType
        {
            get { return templateType; }
            set { templateType = value; }
        }

        public string Version { get; set; }

        /// <summary>
        /// �Ƿ�Ϊ���ӻ�ģ��
        /// </summary>
        public bool IsVisualTemplate { get; set; }

        /// <summary>
        /// ģ�������ı�״̬
        /// </summary>
        public string TemplateTypeText
        {
            get
            {
                switch (TemplateType)
                {
                    case "0":
                        return "��ģ��";
                    case "1":
                        return "ģ��";
                    case "9":
                        return "ĸ��";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Ĭ�ϰ��б�
        /// </summary>
        public string DefaultBindText { get; set; }

        #region ��ʱ����
        /// <summary>
        /// ���ڱ༭��ʵ���ļ�ȫ����ָ�����ļ�
        /// </summary>
        public string EditFileFullPath { get; set; }
        /// <summary>
        /// ����ģ���ļ�·������/_skins/mydefault/home.ascx
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// ������Ŀ��
        /// </summary>
        public string SkinFolder { get; set; }
        /// <summary>
        /// �Ƿ��½�
        /// </summary>
        public bool IsNew { get; set; }

        #endregion

        public void FromFile(string bp, string fn)
        {
            BasePath = bp;
            FileName = Path.GetFileNameWithoutExtension(fn);
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(basePath, fn));
            FromXml(doc.DocumentElement);
        }

        public void ToFile(string fn)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(xd);
            doc.AppendChild(ToXml(doc));
            doc.Save(fn);
        }

        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("template");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("templateType", TemplateType);
            xe.SetAttribute("templateTypeText", TemplateTypeText);
            xe.SetAttribute("created", Created.ToString());
            xe.SetAttribute("updated", Created.ToString());
            xe.SetAttribute("isSub", IsSubTemplate ? Boolean.TrueString : Boolean.FalseString);
            xe.SetAttribute("isCode", IsCode ? Boolean.TrueString : Boolean.FalseString);
            xe.SetAttribute("bindText", DefaultBindText);
            xe.SetAttribute("isVisualTemplate", IsVisualTemplate ? Boolean.TrueString : Boolean.FalseString);
            return xe;
        }

        public Template FromXml(XmlElement xe)
        {
            name = xe.GetAttribute("name");
            description = xe.GetAttribute("description");
            if(!string.IsNullOrEmpty(xe.GetAttribute("created")))
                created = Convert.ToDateTime(xe.GetAttribute("created"));
            if (!string.IsNullOrEmpty(xe.GetAttribute("updated")))
                updated = Convert.ToDateTime(xe.GetAttribute("updated"));
            IsSubTemplate = xe.GetAttribute("isSub") == Boolean.TrueString;
            IsCode = xe.GetAttribute("isCode") == Boolean.TrueString;
            IsVisualTemplate = xe.GetAttribute("isVisualTemplate") == Boolean.TrueString;
            Version = xe.GetAttribute("ver");

            templateType = xe.GetAttribute("templateType");
            templateTypeText = xe.GetAttribute("templateTypeText");
            DefaultBindText = xe.GetAttribute("bindText");

            //foreach (XmlElement e in xe.SelectNodes("File"))
            //{
            //    ResourceFile f = new ResourceFile();
            //    f.FromXml(e);
            //    Files.Add(f);
            //}
            //foreach (XmlElement e in xe.SelectNodes("DataControl"))
            //{
            //    Controls.Add(e.InnerText);
            //}
            return this;
        }

        /// <summary>
        /// ��֤ģ���ļ��Ƿ��ѱ��ⲿ�༭���༭
        /// </summary>
        /// <param name="fn">ģ���ļ�·��</param>
        /// <returns></returns>
        public bool VerifyUpdated(string fn)
        {
            FileInfo f = new FileInfo(fn);
            return f.LastWriteTime > Updated;
        }
    }

    /// <summary>
    /// ģ���������
    /// </summary>
    public class TemplateBindConfig
    {
        /// <summary>
        ///������
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// ��������������
        /// </summary>
        public string HandlerName { get; set; }
        /// <summary>
        /// �ӷ��࣬ģʽ����¼��ע�᣿��ҳ��
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// ģʽ����
        /// </summary>
        public string ModeText { get; set; }
        /// <summary>
        /// ����ģ��
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// ����ģ����������
        /// </summary>
        public string ModelText { get; set; }
        /// <summary>
        /// ��ǩ
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ��Ϣ�Ƿ��㹻
        /// </summary>
        public bool Enough
        {
            get
            {
                return !string.IsNullOrEmpty(Handler) && !string.IsNullOrEmpty(Mode);
            }
        }
    }

    public enum TemplateType : int
    {
        /// <summary>
        /// ��ͨģ��
        /// </summary>
        Common = 1,
        /// <summary>
        /// ��ģ��
        /// </summary>
        Sub= 0,
        /// <summary>
        /// ĸ��
        /// </summary>
        MasterPage = 9,
        /// <summary>
        /// ��ָ��ΪĬ��ģ��
        /// </summary>
        HaveBinded = -1,
        /// <summary>
        /// ȫ��
        /// </summary>
        All= 100
    }
}
