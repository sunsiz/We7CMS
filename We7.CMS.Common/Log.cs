using System;
using System.Collections.Generic;
using System.Text;
//2007-9-3 ��־��
namespace We7.CMS.Common
{
    [Serializable]
    public class Log
    {
        string id;
        string userID;
        string content;
        DateTime created=DateTime.Now;
        string page;
        private string remark;
        DateTime updated=DateTime.Now;

        public Log()
        {
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
        /// ��־ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
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
        /// �����û�ID
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        /// <summary>
        /// ����ҳ��
        /// </summary>
        public string Page
        {
            get { return page; }
            set { page = value; }
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
        /// ��ע
        /// </summary>
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }



    }
}
