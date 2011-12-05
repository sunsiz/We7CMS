using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ���������
    /// </summary>
    [Serializable]
    public class ErrorCode
    {
        int id;
        string title;
        string description;
        string helpLink;
        DateTime created=DateTime.Now;
        int level;
        DateTime updated=DateTime.Now;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public ErrorCode()
        {
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
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
        /// ��������
        /// </summary>
        public string HelpLink
        {
            get { return helpLink; }
            set { helpLink = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
    }
}
