using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{

    /// <summary>
    /// �����ظ���Ϣ
    /// </summary>
    [Serializable]
    public class AdviceReplyInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string AdviceID { get; set; }

        /// <summary>
        /// ������û�ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// ����ı���
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// �ʼ�����
        /// </summary>
        public string MailBody { get; set; }

    }


    #region ��ǰ������

    /// <summary>
    /// �����ظ���Ϣ
    /// </summary>
    [Serializable]
    public class AdviceReply
    {
        private string id;
        private string adviceID;
        private string userID;
        private string title;
        private string content;
        private DateTime createDate = DateTime.Now;
        DateTime updated = DateTime.Now;

        string mailBody;
        string enumState;
        string comment;
        string suggest;
        string mailFile;
        string userEmail;

        /// <summary>
        /// �����ʼ��ļ�����
        /// </summary>
        public string MailFile
        {
            get { return mailFile; }
            set { mailFile = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        public AdviceReply()
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
        /// ��ѯͶ�߱��
        /// </summary>
        public string AdviceID
        {
            get { return adviceID; }
            set { adviceID = value; }
        }

        /// <summary>
        /// �û����
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// �ظ�����
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// �ظ�����
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// ���ݻظ��ʼ�����
        /// </summary>
        public string MailBody
        {
            get { return mailBody; }
            set { mailBody = value; }
        }

        /// <summary>
        /// ״̬
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// �ͻ����������
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public string Suggest
        {
            get { return suggest; }
            set { suggest = value; }
        }

        /// <summary>
        /// �ظ��ʼ����յ�ַ
        /// </summary>
        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        string formEmail;
        /// <summary>
        /// �����ʼ���ַ
        /// </summary>
        public string FormEmail
        {
            get { return formEmail; }
            set { formEmail = value; }
        }
    }

    #endregion
}
