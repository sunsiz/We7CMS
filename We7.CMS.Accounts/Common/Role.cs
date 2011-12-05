using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
using We7.CMS.Common.Enum;
using We7.Framework.Config;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// ��ɫ������
    /// </summary>
    [Serializable]
    public class Role
    {
        /// <summary>
        /// ������
        /// </summary>
        public Role()
        {
            State = "1";
            RoleType = (int)OwnerRank.Normal;
            FromSiteID = SiteConfigs.GetConfig().SiteID;
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public Role(string id, string name, string description,string roletype)
        {
            ID = id; Name = name; Description = description; RoleType =int.Parse(roletype);
            FromSiteID = SiteConfigs.GetConfig().SiteID;
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// ��Դվ��ID
        /// </summary>
        public string FromSiteID { get; set; }
        /// <summary>
        /// ��ɫ����
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��ɫ��ע
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ��ɫ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// ��ɫ״̬�����á�����
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// ��ɫ״̬����
        /// </summary>
        public string StateText
        {
            get
            {
                switch (State)
                {
                    case "1":
                        return "����";
                    default:
                        return "����";
                }
            }
        }

        /// <summary>
        /// ��ɫ��� 0- ����Ա��ɫ ��1-��ͨ�û���ɫ��
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        /// ��ɫ�������
        /// </summary>
        public string TypeText
        {
            get
            {
                string type = "";
                switch ((OwnerRank)RoleType)
                {
                    case OwnerRank.Normal:
                        type= "��ͨ�û���ɫ";
                        break;
                    case OwnerRank.Admin:
                        type= "����Ա��ɫ";
                        break;
                    default:
                        type= "";
                        break;
                }
                return type;
            }
        }

        /// <summary>
        /// վȺ��ɫ
        /// </summary>
        public int GroupRole { get; set; }

        /// <summary>
        /// �Ƿ�վȺ��ɫ
        /// </summary>
        public bool IsGroupRole
        {
            get { return GroupRole > 0; }
            set
            {
                if (value)
                    GroupRole = 1;
                else
                    GroupRole = 0;
            }
        }
    }

}
