using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web;
using System.Collections.Specialized;
using We7.CMS.Config;
using We7.CMS.Common.Enum;
using System.Data;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// ������Ϣ��
    /// </summary>
    [Serializable]
    public class Article : ProcessObject
    {
        /// <summary>
        /// ����Article����
        /// </summary>
        public Article()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            AccountID = We7Helper.EmptyGUID;
            Overdue = DateTime.Now.AddMonths(12);
            ContentType = 2;
            IsDeleted = 0;
            IsImage = 0;
            IsShow = 0;
            State = 1;
            Clicks = 0;
            CommentCount = 0;
            EnumState = StateMgr.StateInitialize();
            int enumValue = (int)EnumLibrary.ArticleType.Article;
            EnumState = StateMgr.StateProcess(EnumState, EnumLibrary.Business.ArticleType, enumValue);
            ProcessState = "0";
            ProcessSiteID = SiteConfigs.GetConfig().SiteID;
            ParentID = We7Helper.EmptyGUID;
            Attachments = new List<Attachment>();
        }

        /// <summary>
        /// ���modelXml���ݣ��������ṹ��
        /// </summary>
        public string ListKeys { get; set; }

        /// <summary>
        /// ���modelXml���ݣ��������ṹ��
        /// </summary>
        public string ListKeys1 { get; set; }

        /// <summary>
        /// ���modelXml���ݣ��������ṹ��
        /// </summary>
        public string ListKeys2 { get; set; }

        /// <summary>
        /// ���modelXml���ݣ��������ṹ��
        /// </summary>
        public string ListKeys3 { get; set; }

        /// <summary>
        /// ���modelXml���ݣ��������ṹ��
        /// </summary>
        public string ListKeys4 { get; set; }

        /// <summary>
        /// ���modelXml���ݣ��������ṹ��
        /// </summary>
        public string ListKeys5 { get; set; }

        /// <summary>
        /// �����Ƶ����
        /// </summary>
        public string VideoCode { get; set; }

        /// <summary>
        /// SEO �Ż����±���
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// SEOul�Ż����¼��
        /// </summary>
        public string DescriptionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UptoTime { get; set; }

        string tags;
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
        /// ��Ŀ��ַ
        /// </summary>
        public string ChannelFullUrl { get; set; }

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// �����չ��ϢXML����
        /// </summary>
        public string ModelXml { get; set; }

        ///// <summary>
        ///// ״̬��Ϣ,�������ѹ�ʱ���벻Ҫ��ʹ��
        ///// </summary>
        [Obsolete]
        public string EnumState { get; set; }

        /// <summary>
        /// ������ԴID
        /// </summary>
        public string FromRowID { get; set; }

        /// <summary>
        /// ������Դ���µ�ַ
        /// </summary>
        public string FromSiteUrl { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Overdue { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 1��Ϊ�������ۣ�0��Ϊ����������
        /// </summary>
        public int AllowComments { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ���¸�ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// ���±���
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ժҪ������Description��Content
        /// </summary>
        public string Summary
        {
            get
            {
                string s = "";
                if (string.IsNullOrEmpty(Description))
                {
                    string content = We7Helper.RemoveHtml(Content);
                    if (content.Length > 50)
                        s = content.Substring(0, 50) + "...";
                    else
                        s = content;
                }
                else
                    s = Description;

                return s;
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string Author { get; set; }

        private string content;
        /// <summary>
        /// ��������
        /// </summary>
        public string Content
        {
            get
            {
                return We7Helper.ConvertPageBreakToHtml(content);
            }
            set { this.content = value; }
        }

        /// <summary>
        /// ���´���ʱ��
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated { get; set; }

        public int state;
        /// <summary>
        /// ��������ã�1�����ã�0�����ã�ͣ�ã���2������У�3�����ڣ�4������վ
        /// </summary>
        public int State
        {
            get
            {
                //if (state == 3)
                //{
                //    if (Overdue > DateTime.Now)
                //    {
                //        return 1;
                //    }
                //}
                return state;
            }
            set { state = value; }
        }

        /// <summary>
        /// ��ĿID
        /// </summary>
        public string OwnerID { get; set; }

        /// <summary>
        /// �û�ID
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// ���ӵ�ַ
        /// </summary>
        public string LinkUrl { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// ʱ��ڵ�
        /// </summary>
        public string TimeNote { get; set; }

        /// <summary>
        /// ״̬ת�������ַ���
        /// </summary>
        public string AuditText
        {
            get
            {
                switch ((ArticleStates)State)
                {
                    case ArticleStates.Started: return "<font color=green>�ѷ���</font>";
                    case ArticleStates.Checking: return "<font color=#aa0>�����</font>";
                    case ArticleStates.Overdued: return "<font color=#888>�ѹ���</font>";
                    case ArticleStates.Recycled: return "<font color=#009>��ɾ��</font>";
                    default:
                    case ArticleStates.Stopped: return "<font color=red>��ͣ��</font>";
                }
            }
        }

        /// <summary>
        /// �Ƿ�������ͼ
        /// </summary>
        public int IsImage { get; set; }

        /// <summary>
        /// �Ƿ��ö�
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// ����ͼ��ŵ�ַ��С����ͼ��
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// ͨ��ID���ɵ�url����e6b4ed25_263c_4dc6_81f8_f7e06c214099.shtml��1008.html
        /// </summary>
        public string FullUrl
        {
            get
            {
                return GenerateArticleUrl(SN, Created, ID);
            }
        }

        /// <summary>
        /// ���ձ�����������URL
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="create"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GenerateArticleUrl(long sn, DateTime create, string id)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string ext = si.UrlFormat;
            string snRex = si.ArticleUrlGenerator;
            if (snRex != null && snRex.Trim().Length > 0)
            {
                if (snRex == "0")
                    return sn.ToString() + "." + ext;
                else
                    return create.ToString(snRex) + "-" + sn.ToString() + "." + ext;
            }
            else
                return We7Helper.GUIDToFormatString(id) + "." + ext;
        }

        /// <summary>
        /// ���ձ�����������URL
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="create"></param>
        /// <param name="id"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public string GenerateArticleUrl(long sn, DateTime create, string id, string modelName)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string ext = si.UrlFormat;
            if (!String.IsNullOrEmpty(modelName))
            {
                string snRex = si.ArticleUrlGenerator;
                if (snRex != null && snRex.Trim().Length > 0)
                {
                    if (snRex == "0")
                        return sn.ToString() + "." + ext;
                    else
                        return create.ToString(snRex) + "-" + sn.ToString() + "." + ext;
                }
                else
                    return We7Helper.GUIDToFormatString(id) + "." + ext;
            }
            else
            {
                return We7Helper.GUIDToFormatString(id) + "." + ext;
            }

        }

        /// <summary>
        /// �ο�TypeOfArticleö��
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsLinkArticle { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string TypeText
        {
            get
            {
                switch ((TypeOfArticle)ContentType)
                {
                    case TypeOfArticle.LinkArticle:
                        return "��������";
                    case TypeOfArticle.ShareArticle:
                        return "��������";
                    case TypeOfArticle.WapArticle:
                        return "WAP����";
                    default:
                    case TypeOfArticle.NormalArticle:
                        return "ԭ������";
                }
            }
        }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// ģ�����ݼܹ�
        /// </summary>
        public string ModelSchema { get; set; }

        /// <summary>
        /// ���ܼ���0,����;0|����,1|�ڲ�,2|����,3|����,4|����
        /// </summary>
        public int PrivacyLevel { get; set; }

        /// <summary>
        /// ��ͬ����ͼƬ
        /// </summary>
        public string TypeIcon
        {
            get
            {
                switch ((TypeOfArticle)ContentType)
                {
                    case TypeOfArticle.LinkArticle:
                        return "/admin/images/filetype/link.gif";
                    case TypeOfArticle.ShareArticle:
                        return "/admin/images/filetype/mpg.gif";
                    default:
                    case TypeOfArticle.NormalArticle:
                        return "";
                }
            }
        }

        public string IsShowText
        {
            get
            {
                return IsShow == 1 ? "��" : "��";
            }
        }

        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        public int IsDeleted { get; set; }

        /// <summary>
        /// ����URL
        /// </summary>
        public string ContentUrl { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public int Clicks { get; set; }

        /// <summary>
        /// �����ı���
        /// </summary>
        public string FullTitle { get; set; }


        /// <summary>
        /// ʧ��ǰ�˿��õ�Url;
        /// </summary>
        public string Url
        {
            get
            {
                return ChannelFullUrl + FullUrl;
            }
        }

        /// <summary>
        /// ������ͼ
        /// </summary>
        public string WapImage
        {
            get
            {
                return GetImageName(Thumbnail, "_W");
            }
        }

        /// <summary>
        /// ����ԭʼͼƬ
        /// </summary>
        public string OriginalImage
        {
            get
            {
                return GetImageName(Thumbnail, "");
            }
        }

        /// <summary>
        /// ���ݱ�ǩtag����ȡ�ö�Ӧ����ͼ
        /// �磬Thumbnail["wap"] Ϊ mysource_wap.jpg
        /// </summary>
        public string TagThumbnail { get; set; }

        /// <summary>
        /// ��ȡ��ǩͼƬ����
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetTagThumbnail(string tag)
        {
            if (string.IsNullOrEmpty(Thumbnail))
                return string.Empty;
            else
            {
                string ext = Path.GetExtension(Thumbnail);
                string imgName = Path.GetFileNameWithoutExtension(Thumbnail);
                int nameLength = ext.Length + imgName.Length;
                string url = Thumbnail.Substring(0, Thumbnail.Length - nameLength);
                return String.Format("{3}{0}_{1}{2}", imgName, tag, ext, url);
            }
        }

        /// <summary>
        /// ��ȡͼƬ����
        /// </summary>
        /// <param name="thumbnailName"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        public string GetImageName(string thumbnailName, string imgType)
        {
            string ext = Path.GetExtension(thumbnailName);
            string imgName = Path.GetFileNameWithoutExtension(thumbnailName);

            imgName = imgName.Substring(0, imgName.Length - 2);
            int nameLength = 2 + ext.Length + imgName.Length;
            string url = thumbnailName.Substring(0, thumbnailName.Length - nameLength);

            return String.Format("{3}{0}{1}{2}", imgName, imgType, ext, url);
        }

        /// <summary>
        /// ��ȡ���µ�����url��ǰ̨������ʹ�ô����ԣ�������FullUrl
        /// </summary>
        /// <param name="channelUrl">������Ŀurl</param>
        /// <returns></returns>
        public string GetFullUrlWithChannel(string channelUrl)
        {
            if (ContentType == (int)TypeOfArticle.LinkArticle)
                return ContentUrl;
            else
                return String.Format("{0}{1}", channelUrl, FullUrl);
        }

        /// <summary>
        /// �������
        /// </summary>
        public List<Article> RelatedArticles { get; set; }

        /// <summary>
        /// ������Ŀ��ȫ·��������ʾ���磬/����/ͼƬ����
        /// </summary>
        public string FullChannelPath { get; set; }

        /// <summary>
        /// ����/wap���͵�ԭ����ID
        /// </summary>
        public string SourceID { get; set; }

        /// <summary>
        /// ������ˮ��
        /// </summary>
        public long SN { get; set; }

        /// <summary>
        /// IP����
        /// </summary>
        public string IPStrategy { get; set; }

        /// <summary>
        /// ���¸�������·������ /_data/2010/02/25/64a55027_062d_4f78_8c51_aeb6500fdacb/
        /// </summary>
        public string AttachmentUrlPath
        {
            get
            {
                string year = Created.ToString("yyyy");
                string month = Created.ToString("MM");
                string day = Created.ToString("dd");
                string sn = We7Helper.GUIDToFormatString(ID);
                return string.Format("/_data/{0}/{1}/{2}/{3}", year, month, day, sn);
            }
        }

        /// <summary>
        /// ��ǰ���µĸ����б�
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// ��ƪ��������վ�㣻������վȺ���������
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// ��ƪ��������վ��URL��������վȺ���������
        /// </summary>
        public string SiteUrl { get; set; }

        public string Photos { get; set; }

        /// <summary>
        /// ��ɫ
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// �����ض�
        /// </summary>
        public string FontWeight { get; set; }
        /// <summary>
        /// ������ʽ
        /// </summary>
        public string FontStyle { get; set; }

        private string titleStyle;
        public string TitleStyle
        {
            get
            {
                if (String.IsNullOrEmpty(titleStyle))
                {
                    StringBuilder sb = new StringBuilder();
                    if (!String.IsNullOrEmpty(Color))
                    {
                        sb.AppendFormat("color:{0};", Color);
                    }
                    if (!String.IsNullOrEmpty(FontWeight))
                    {
                        sb.AppendFormat("font-weight:{0};", FontWeight);
                    }
                    if (!String.IsNullOrEmpty(FontStyle))
                    {
                        sb.AppendFormat("font-style:{0};", FontStyle);
                    }
                    titleStyle = sb.ToString();
                }
                return titleStyle;
            }
        }

        /// <summary>
        /// �յ����
        /// </summary>
        public int DayClicks { get; set; }

        /// <summary>
        /// ���յ����
        /// </summary>
        public int YesterdayClicks { get; set; }

        /// <summary>
        /// �ܵ����
        /// </summary>
        public int WeekClicks { get; set; }

        /// <summary>
        /// �µ����
        /// </summary>
        public int MonthClicks { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public int QuarterClicks { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int YearClicks { get; set; }

        #region
        [NonSerialized]
        private DataSet dataSet;
        [NonSerialized]
        private DataRow row;

        /// <summary>
        /// ����ģ���е���Ϣ
        /// </summary>
        /// <param name="name">�ֶ���</param>
        /// <returns>ģ������</returns>
        public object this[string name]
        {
            get
            {
                return Row != null && Row.Table.Columns.Contains(name) ? Row[name] : null;
            }
        }

        private DataRow Row
        {
            get
            {
                if (row == null)
                {
                    row = DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 ? DataSet.Tables[0].Rows[0] : null;
                }
                return row;
            }
        }

        private DataSet DataSet
        {
            get
            {
                if (dataSet == null)
                {
                    dataSet = CreateDataSet();
                    if (dataSet != null)
                    {
                        using (StringReader reader = new StringReader(ModelXml))
                        {
                            dataSet.ReadXml(reader);
                        }
                    }
                }
                return dataSet;
            }
        }

        DataSet CreateDataSet()
        {
            if (!String.IsNullOrEmpty(ModelSchema))
            {
                DataSet ds = new DataSet();
                using (StringReader reader = new StringReader(ModelSchema))
                {
                    ds.ReadXmlSchema(reader);
                }
                return ds;
            }
            return null;
        }
        #endregion

    }

    /// <summary>
    /// ���������
    /// </summary>
    [Serializable]
    public class RelatedArticle
    {

        public RelatedArticle() { }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ArticleID { get; set; }
        /// <summary>
        /// ��������ID
        /// </summary>
        public string RelatedID { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated { get; set; }

    }
    /// <summary>
    /// ����ͳ����
    /// </summary>
    public class StatisticsArticle
    {
        /// <summary>
        /// ��������
        /// </summary>
        public int TotalArticles { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int TotalComments { get; set; }
        /// <summary>
        /// ���·���������
        /// </summary>
        public int MonthArticles { get; set; }

        /// <summary>
        /// //��������
        /// </summary>
        public int MonthComments { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        public int WeekArticles { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int WeekComments { get; set; }

    }

    /// <summary>
    /// ����ͼ������Ϣ��
    /// </summary>
    public class ThumbnailConfig
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ֵ
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// ��ǩ-��ʶ�������ļ����Ĺ���
        /// </summary>
        public string Tag { get; set; }

    }
}
