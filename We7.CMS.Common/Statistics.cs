using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ����ͳ����
    /// </summary>
    [Serializable]
    public class Statistics
    {
        private string id;
        //private int typeCode;
        private string visitorID;
        private string userName;
        private string articleID;
        private string channelID;
        private string articleName;
        private DateTime visitDate;
        private string url;
        private string visitorIP;
        string timeNote;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// ���캯��
        /// </summary>
        public Statistics()
        { }

        /// <summary>
        /// ���
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        /// <summary>
        /// �û�ID
        /// </summary>
        public string VisitorID
        {
            get { return visitorID; }
            set { visitorID = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        /// <summary>
        /// ��ĿID
        /// </summary>
        public string ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        /// <summary>
        /// ��ַ��Ϣ
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// �û�IP
        /// </summary>
        public string VisitorIP
        {
            get { return visitorIP; }
            set { visitorIP = value; }
        }

        /// <summary>
        /// �ڵ�ʱ��
        /// </summary>
        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
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

    }

    /// <summary>
    /// ����ͳ����ʷ��
    /// </summary>
    public class StatisticsHistory : Statistics
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public StatisticsHistory() { }
    }
}
