using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ��Ȩ��Ϣʵ����
    /// </summary>
    [Serializable]
    public class Permission
    {
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

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
        /// ����ID
        /// </summary>
        public string ID { get; set; }

        /// �û�ID��RoleID
        /// </summary>
        public string OwnerID { get; set; }

        /// <summary>
        /// �������ͣ�0���ʻ���1����ɫ
        /// </summary>
        public int OwnerType { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// Ȩ������
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ��ĿUrl��˵�Url
        /// </summary>
        public string Url { get; set; }

    }
}
