using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ���Ŷ�������ŷ�����ʷ��¼��
    /// </summary>
    [Serializable]
    public class ShortMessage
    {
        string id;
        string accountID;
        string content;
        string receivers;
        string description;
        DateTime created=DateTime.Now;
        DateTime sendTime;
        int state;
        string success;
        string accountName;
        string passWord;
        DateTime updated=DateTime.Now;


        public ShortMessage()
        {
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// ������ID
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string AccountName
        {
            get { return accountName; }
            set { accountName = value; }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public string Receivers
        {
            get { return receivers; }
            set { receivers = value; }
        }

        /// <summary>
        /// ������ID
        /// </summary>
        public string ReceiverID { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        public DateTime SendTime
        {
            get { return sendTime; }
            set { sendTime = value; }
        }

        public int State
        {
            get { return state; }
            set { state = value; }
        }

        public string Success
        {
            get { return success; }
            set { success = value; }
        }

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

    }

    public enum MessageState
    {
        /// <summary>
        /// �ݸ���
        /// </summary>
        Draft,
        /// <summary>
        /// ����Ϣ
        /// </summary>
        NewMessage,
        /// <summary>
        /// �ռ������Ѷ�
        /// </summary>
        Inbox,
        /// <summary>
        /// ������浵
        /// </summary>
        Outbox,
        /// <summary>
        /// �ֻ����ŷ���ʧ��
        /// </summary>
        Failure,
        /// <summary>
        /// �ֻ����ŷ��ͳɹ�
        /// </summary>
        Success,
        /// <summary>
        /// ���ж���
        /// </summary>
        AllSMS,
        /// <summary>
        /// δ֪
        /// </summary>
        Unknown
    }

}
