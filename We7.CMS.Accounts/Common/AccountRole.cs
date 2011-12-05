using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// �û���ɫ
    /// </summary>
    [Serializable]
    public class AccountRole
    {
        string id;
        string accountID;
        string roleID;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;
        string roleTitle;

        /// <summary>
        /// ��Ϣ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// �û���ɫ
        /// </summary>
        public AccountRole()
        {
            created = DateTime.Now;
        }

        /// <summary>
        /// ��Ϣ����ʱ��
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
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
        /// ����û�ID
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// �����ɫID
        /// </summary>
        public string RoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }

        /// <summary>
        /// ��ɫ����
        /// </summary>
        public string RoleTitle
        {
            get { return roleTitle; }
            set { roleTitle = value; }
        }
    }
}
