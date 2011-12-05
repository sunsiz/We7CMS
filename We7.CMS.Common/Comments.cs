using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ������
    /// </summary>
    [Serializable]
    public class Comments
    {
        string id;
        string articleID;
        string content;
        string author;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;
        int state;
        int index;
        string timeNote;
        string ip;
        string accountID;
        string articleName;
        string articleTitle;

        /// <summary>
        /// ���캯��
        /// </summary>
        public Comments()
        {
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
        /// ��������ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
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
        /// ���۸���ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        /// <summary>
        /// ����״̬
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// ʱ��ڵ�
        /// </summary>
        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
        }

        /// <summary>
        /// IP��ַ
        /// </summary>
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }

        /// <summary>
        /// �û��ɣ�
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// ״̬ת���ַ���
        /// </summary>
        public string AuditText
        {
            get
            {
                switch (State)
                {
                    case 0: return "<font color=red>������</font>";
                    default:
                    case 1: return "<font color=green>������</font>";
                }
            }
        }

        /// <summary>
        /// ���±���
        /// </summary>
        public string ArticleTitle
        {
            get { return articleTitle; }
            set { articleTitle = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string ArticleName
        {
            get { return articleName; }
            set { articleName = value; }
        }
    }

}
