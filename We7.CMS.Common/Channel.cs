using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// ��Ŀ��Ϣ��
    /// </summary>
    [Serializable]
    public class Channel:IComparable<Channel>
    {
        public static int MaxLevels = 8;

        string id;
        string parentID;
        string alias;
        string name;
        string description;
        string templateName;
        string detailTemplate;
        int state;
        int index;
        int securityLevel;
        string referenceID;
        string parameter;
        DateTime created=DateTime.Now;
        string fullPath;
        string templateText;
        string detailTemplateText;
        List<Channel> channels;
        string defaultContentID;
        string channelFolder;
        string titleImage;
        string process;
        string type;
        string channelName;
        string refAreaID;
        int isComment;
        string fullUrl;
        string returnUrl;
        private string processLayerNO;
        DateTime updated=DateTime.Now;
        string enumState;
        int articlesCount;
        string tags;
        string keyWord;
        string descriptionKey;
        string ipstrategy;
        bool isOldFullUrl;

        /// <summary>
        /// ��Ŀ�๹�캯��
        /// </summary>
        public Channel()
        {
            created = DateTime.Now;
            updated = DateTime.Now;
            channels = new List<Channel>();
            securityLevel = 0;
            state = 1;
            type = "0";
            isComment = 0;
            enumState = StateMgr.StateInitialize();
            int enumValue = (int)EnumLibrary.ChannelContentType.Article;
            enumState = StateMgr.StateProcess(enumState, EnumLibrary.Business.ChannelContentType, enumValue);
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        /// <summary>
        /// ������ĿID
        /// </summary>
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        /// <summary>
        /// ��Ŀ���⣬������ʾ����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// ��ʾȫ·�����磺/����/ͼƬ����
        /// </summary>
        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
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
        /// ��ϸҳģ��
        /// </summary>
        public string DetailTemplate
        {
            get { return detailTemplate; }
            set { detailTemplate = value; }
        }

        /// <summary>
        /// ģ������
        /// </summary>
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        /// <summary>
        /// ״̬
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// ״̬ת���ַ���
        /// </summary>
        public string StateText
        {
            get
            {
                return State == 0 ? "������" : "����";
            }
        }

        /// <summary>
        /// ��ȫ����
        /// </summary>
        public int SecurityLevel
        {
            get { return securityLevel; }
            set { securityLevel = value; }
        }

        /// <summary>
        /// ��ȫ����ת���ַ���
        /// </summary>
        public string SecurityLevelText
        {
            get
            {
                switch (SecurityLevel)
                {
                    case 1: return "��";
                    case 2: return "��";

                    default:
                    case 0: return "��";
                }
            }
        }

        /// <summary>
        /// �ο���ϢID
        /// </summary>
        public string ReferenceID
        {
            get { return referenceID; }
            set { referenceID = value; }
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
        /// ģ����Ϣ
        /// </summary>
        public string TemplateText
        {
            get { return templateText; }
            set { templateText = value; }
        }

        /// <summary>
        /// ��ϸģ����Ϣ
        /// </summary>
        public string DetailTemplateText
        {
            get { return detailTemplateText; }
            set { detailTemplateText = value; }
        }

        public List<Channel> Channels
        {
            get { return channels; }
            set { channels = value; }
        }

        /// <summary>
        /// Ĭ����ϸ����ID
        /// </summary>
        public string DefaultContentID
        {
            get { return defaultContentID; }
            set { defaultContentID = value; }
        }

        /// <summary>
        /// ��Ŀ�ļ���
        /// </summary>
        public string ChannelFolder
        {
            get
            {
                return channelFolder;
            }
            set { channelFolder = value; }
        }

        /// <summary>
        /// ����ͼƬ
        /// ���磺/_data/Channels/zwgk_first.jpg
        /// </summary>
        /// </summary>
        public string TitleImage
        {
            get { return titleImage; }
            set { titleImage = value; }
        }

        /// <summary>
        /// �Ƿ����������̣�1-����������-������
        /// </summary>
        public string Process
        {
            get { return process; }
            set { process = value; }
        }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// IP����
        /// </summary>
        public string IPStrategy
        {
            get { return ipstrategy; }
            set { ipstrategy = value; }
        }
        /// <summary>
        /// ��Ŀ����ת���ַ���
        /// </summary>
        public string TypeText
        {
            get
            {
                switch ((TypeOfChannel)int.Parse(Type))
                {
                    case TypeOfChannel.QuoteChannel:
                        return "ר����";
                    case TypeOfChannel.RssOriginal:
                        return "RSSԴ";
                    case TypeOfChannel.BlankChannel:
                        return "�սڵ�";
                    case TypeOfChannel.ReturnChannel:
                        return "��ת��";
                    default:
                    case TypeOfChannel.NormalChannel:
                        return "ԭ����";
                }
            }
        }

        /// <summary>
        /// Ƶ��Ψһ���ƣ�����URL
        /// </summary>
        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        /// <summary>
        /// ��Ŀ��Դ
        /// </summary>
        public string RefAreaID
        {
            get { return refAreaID; }
            set { refAreaID = value; }
        }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public int IsComment
        {
            get { return isComment; }
            set { isComment = value; }
        }


        /// <summary>
        /// �Ƿ񷵻�ԭ����FullUrl
        /// Ĭ����false 
        /// �޸�WJZ 2010��8��3��
        /// </summary>
        public bool IsOldFullUrl
        {
            get { return isOldFullUrl; }
            set { isOldFullUrl = value; }
        }

        /// <summary>
        /// ����ģ������
        /// </summary>
        public string ModelName
        { get; set; }

        /// <summary>
        /// �Ƿ�����ת���ַ���
        /// </summary>

        public string IsCommentText
        {
            get
            {
                switch (IsComment)
                {
                    case 1: return "�����¼�û�����";
                    case 2: return "������������";

                    default:
                    case 0: return "����������";
                }
            }
        }

        /// <summary>
        /// thehim 2009-2-26���޸ģ�
        /// 1��FullUrl Ϊԭ����Ŀ��channelname�����
        /// 2���������� RealUrl �������ת���⣬ǰ̨�˵��ؼ���ʹ��RealUrl
        /// </summary>
        public string FullUrl
        {
            get
            {
                if (IsOldFullUrl)
                {
                    return fullUrl;
                }
                else
                {

                    if (fullUrl != null)
                    {
                        string cleanUrl = fullUrl;

                        while (cleanUrl.StartsWith("/"))
                            cleanUrl = cleanUrl.Remove(0, 1);

                        if (cleanUrl.EndsWith("/"))
                            cleanUrl = cleanUrl.Remove(cleanUrl.Length - 1);

                        cleanUrl = "/" + cleanUrl + "/";

                        return cleanUrl;
                    }
                    else
                        return fullUrl;
                }
            }
            set { fullUrl = value; }
                
                
        }

        string realUrl;
        /// <summary>
        /// �������� RealUrl �������ת���⣬ǰ̨�˵��ؼ���ʹ��RealUrl
        /// 1��������ת�ͣ����� ReturnUrl;
        /// 2���粻�ǣ����� FullUrl
        /// </summary>
        public string RealUrl
        {
            get
            {
                if (FullUrl != null)
                {
                    string cleanUrl = FullUrl;

                    if (Type != null && (TypeOfChannel)int.Parse(Type) == TypeOfChannel.ReturnChannel)
                    {
                        //�жϵ�ַ���Ƿ��С�.��;���������⣬��ʱע��
                        //if (ReturnUrl != null && ReturnUrl.IndexOf(".") != -1)
                        //{
                        //    //�жϵ�ַ���Ƿ��С�http://��
                        //    if (!ReturnUrl.ToLower().StartsWith("http://"))
                        //    {
                        //        ReturnUrl = "http://" + ReturnUrl;
                        //    }
                        //}
                        cleanUrl = ReturnUrl;
                    }
                    else
                    {
                        GeneralConfigInfo si = GeneralConfigs.GetConfig();
                        string ext = si.UrlFormat;
                        if (ext == "aspx") cleanUrl = cleanUrl + "default.aspx";
                    }
                    return cleanUrl;
                }
                else
                    return FullUrl;
            }
            set { realUrl = value; }
        }

        /// <summary>
        /// ��������Ŀ�ļ���ַ
        /// </summary>
        public string FullFolderPath
        {
            get
            {
                return string.Format("{0}/{1}", ChannelUrlPath, ChannelFolder);
            }
        }

        /// <summary>
        /// ��ת��ַ
        /// </summary>
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Channel");
            xe.SetAttribute("id", ID);
            xe.SetAttribute("parentID", ParentID);
            xe.SetAttribute("alias", Alias);
            xe.SetAttribute("name", Name);
            xe.SetAttribute("description", Description);
            xe.SetAttribute("fullPath", FullPath);
            xe.SetAttribute("template", TemplateName);
            xe.SetAttribute("detailTemplate", DetailTemplate);
            xe.SetAttribute("securityLevel", SecurityLevel.ToString());
            xe.SetAttribute("state", State.ToString());
            xe.SetAttribute("reference", ReferenceID);
            xe.SetAttribute("defaultContentID", DefaultContentID);
            xe.SetAttribute("index", Index.ToString());
            xe.SetAttribute("parameter", Parameter);
            xe.SetAttribute("channelFolder", ChannelFolder);
            xe.SetAttribute("titleImage", TitleImage);
            xe.SetAttribute("process", Process);
            xe.SetAttribute("type", Type);
            xe.SetAttribute("channelName", ChannelName);
            xe.SetAttribute("refAreaID", RefAreaID);
            xe.SetAttribute("isComment", IsComment.ToString());
            xe.SetAttribute("fullUrl", FullUrl);
            xe.SetAttribute("returnUrl", ReturnUrl);
            xe.SetAttribute("processLayerNO", ProcessLayerNO);
            xe.SetAttribute("enumState", EnumState);

            foreach (Channel ch in Channels)
            {
                xe.AppendChild(ch.ToXml(doc));
            }
            return xe;
        }

        /// <summary>
        /// ��ȡXML������Ϣ
        /// </summary>
        /// <param name="xe"></param>
        public void FromXml(XmlElement xe)
        {
            Channels.Clear();
            ID = xe.GetAttribute("id");
            ParentID = xe.GetAttribute("parentID");
            Alias = xe.GetAttribute("alias");
            Name = xe.GetAttribute("name");
            Description = xe.GetAttribute("description");
            FullPath = xe.GetAttribute("fullPath");
            TemplateName = xe.GetAttribute("template");
            DetailTemplate = xe.GetAttribute("detailTemplate");
            SecurityLevel = Convert.ToInt16(xe.GetAttribute("securityLevel"));
            State = Convert.ToInt16(xe.GetAttribute("state"));
            ReferenceID = xe.GetAttribute("reference");
            DefaultContentID = xe.GetAttribute("defaultContentID");
            Index = Convert.ToInt32(xe.GetAttribute("index"));
            Parameter = xe.GetAttribute("parameter");
            ChannelFolder = xe.GetAttribute("channelFolder");
            TitleImage = xe.GetAttribute("titleImage");
            Process = xe.GetAttribute("process");
            Type = xe.GetAttribute("type");
            ChannelName = xe.GetAttribute("channelName");
            RefAreaID = xe.GetAttribute("refAreaID");
            IsComment = Convert.ToInt16(xe.GetAttribute("isComment"));
            FullUrl = xe.GetAttribute("fullUrl");
            ReturnUrl = xe.GetAttribute("returnUrl");
            ProcessLayerNO = xe.GetAttribute("processLayerNO");
            EnumState = xe.GetAttribute("enumState");

            foreach (XmlNode node in xe.SelectNodes("Channel"))
            {
                XmlElement el = node as XmlElement;
                if (el != null)
                {
                    Channel ch = new Channel();
                    ch.FromXml(el);
                    Channels.Add(ch);
                }
            }
        }

        //�˴���ԭλ��We7.CMS.Common.Utils�µ�Constants.cs��
        public static string ChannelPath = "_data\\Channels";

        //�˴���ԭλ��We7.CMS.Common.Utils�µ�Constants.cs��
        public static string ChannelUrlPath
        {
            get
            {
                string temp = ChannelPath.Replace("\\", "/");
                if (!temp.StartsWith("/")) temp = "/" + temp;
                //temp = "~" + temp;
                return temp;
            }
        }
        /// <summary>
        /// SEO�Ż��ؼ���
        /// </summary>
        public string KeyWord
        {
            get { return keyWord; }
            set { keyWord = value; }
        }

        /// <summary>
        /// SEO�Ż�
        /// </summary>
        public string DescriptionKey
        {
            get { return descriptionKey; }
            set { descriptionKey = value; }
        }

        /// <summary>
        /// ��ǩ
        /// </summary>
        public string Tags
        {
            get 
            {
                if (tags == null)
                    return string.Empty;
                else
                    return tags;
            }
            set { tags = value; }
        }

        /// <summary>
        /// ͳ�Ƹ���Ŀ�µ�������
        /// </summary>
        public int ArticlesCount
        {
            get { return articlesCount; }
            set { articlesCount = value; }
        }

        /// <summary>
        /// ��Ŀ״̬
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
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
        /// ��˲��裺1��2��3�࣬�ַ���
        /// </summary>
        public string ProcessLayerNO
        {
            get { return processLayerNO; }
            set { processLayerNO = value; }
        }

        /// <summary>
        /// ��˼���
        /// </summary>
        public string ProcessLayerNOText
        {
            get
            {
                switch (ProcessLayerNO)
                {
                    case "1": return "һ��";
                    case "2": return "����";
                    case "3": return "����";
                    default: return "";
                }
            }
        }

        /// <summary>
        /// �����϶�����0-��ᣬ������ã�1-����ֱ�����ã�2-�Ϳ�վ���
        /// </summary>
        public string ProcessEnd { get; set; }

        /// <summary>
        /// ��Ŀ�б�ҳURL
        /// </summary>
        public string ListUrl
        {
            get
            {
                string ext = "aspx";
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null) ext = si.UrlFormat;
                if (type == ((int)TypeOfChannel.BlankChannel).ToString())
                {
                    return FullUrl;
                }
                else
                {
                    return FullUrl + "list." + ext;
                }
            }
        }

        /// <summary>
        /// ��Ŀ����ҳURL
        /// </summary>
        public string SearchUrl
        {
            get
            {
                string ext = "aspx";
                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si != null) ext = si.UrlFormat;
                return FullUrl + "search." + ext;
            }
        }

        /// <summary>
        ///��ʾ���⣨��ʱ���ԣ�
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// ����Ŀ�Ƿ�ѡ�У������жϲ˵�״̬��
        /// </summary>
        public bool MenuIsSelected { get; set; }
        /// <summary>
        /// �Ƿ�������Ŀ���������ɲ˵�״̬��
        /// </summary>
        public bool HaveSon { get; set; }
        /// <summary>
        /// ����Ŀ�б��������ɲ˵�����
        /// </summary>
        public List<Channel> SubChannels { get; set; }
        /// <summary>
        /// ���������б�����ǰ��Ĳ��֣�
        /// </summary>
        public List<Article> Articles { get; set; }
        /// <summary>
        /// ���ɶ༶��Ŀ����
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string BuildLinkHtml(string separator)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(FullPath) && !string.IsNullOrEmpty(FullUrl))
            {
                string[] urls = FullUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] names = FullPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string longUrl = "/";
                for (int i = 0; i < names.Length; i++)
                {
                    if (i < urls.Length)
                    {
                        sb.Append(string.Format("<a href='{0}'>{1}</a>", longUrl + urls[i] + "/", names[i]));
                        sb.Append(separator);
                        longUrl += urls[i] + "/";
                    }
                }
                sb.Remove(sb.Length - separator.Length, separator.Length);
            }
            return sb.ToString();
        }

        #region IComparable<Channel> ��Ա

        /// <summary>
        /// ����Ŀ����
        /// </summary>
        public int CompareTo(Channel other)
        {
            return string.Compare(this.FullPath.Replace("//", "/").Replace("/", " ��"), other.FullPath.Replace("//", "/").Replace("/", " ��"), false);
        }

        #endregion
    }
}