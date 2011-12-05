using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using We7.Framework;
using We7.Framework.Config;
using System.Xml.Serialization;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// �û�������Ϣ
    /// </summary>
    [Serializable]
    public class Account
    {
        /// <summary>
        /// ������
        /// </summary>
        public Account()
        {
            DepartmentID = We7Helper.EmptyGUID;
            UserType = (int)OwnerRank.Admin;
            State = 1;
            PasswordHashed = 1;
            Password = "111111";
            Created = DateTime.Now;
            ModelState = 0;
            FromSiteID = SiteConfigs.GetConfig().SiteID;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Overdue = DateTime.Today.AddYears(10);
        }

        /// <summary>
        /// ��Ϣ����ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ��Դվ��ID
        /// </summary>
        public string FromSiteID { get; set; }

        /// <summary>
        /// �������ID
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// ���������ֶΣ�Department -> Name
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// ��Ա��¼����
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// �û���¼����
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// �û����0������Ա��1����ͨ�û�
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// �û������ʾ����ת��
        /// </summary>
        [XmlIgnoreAttribute]
        public string TypeText
        {
            get
            {
                string type = "";
                switch ((OwnerRank)UserType)
                {
                    case OwnerRank.Admin:
                        type = "����Ա";
                        break;

                    case OwnerRank.Normal:
                        type = "��ͨ�û�";
                        break;

                    default:
                        type = "δ����";
                        break;
                }

                return type;
            }
        }

        /// <summary>
        /// ��Ա����
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// ��Ա�����ַ
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// �ֻ���
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// QQ��
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// ��Ա���
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// ��Ա״̬��0-���ã�1-���ã�2-����
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Email��֤
        /// </summary>
        public int EmailValidate { get; set; }

        /// <summary>
        /// ��Ϣ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// ��ʽ�����ע��ʱ��
        /// </summary>
        [XmlIgnoreAttribute]
        public string CreatedNoteTime
        {
            get
            {
                return We7Helper.FormatTimeNote(Created);
            }
        }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ״̬��ʾ����ת��
        /// </summary>
        [XmlIgnoreAttribute]
        public string StateText
        {
            get { return State == 0 ? "����" : "����"; }
        }

        /// <summary>
        /// ͨ��֤ID
        /// </summary>
        public string Home { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public int PasswordHashed { get; set; }

        /// <summary>
        /// ���ü���
        /// </summary>
        [XmlIgnoreAttribute]
        public bool IsPasswordHashed
        {
            get { return PasswordHashed > 0; }
            set
            {
                if (value)
                    PasswordHashed = 1;
                else
                    PasswordHashed = 0;
            }
        }


        /// <summary>
        ///��Ա����ʱ��
        /// </summary>
        public DateTime Overdue { get; set; }

        /// <summary>
        ///��Ϣ����ʱ�� 
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// �û�ģ�����ƣ��磺���˻�Ա����ҵ��Ա
        /// </summary>
        public string UserModelName { get; set; }

        /// <summary>
        /// �û�ģ������
        /// </summary>
        public string ModelXml { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// ģ�����ݼܹ�
        /// </summary>
        public string ModelSchema { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Point { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int Prestige { get; set; }
        /// <summary>
        /// ��Ǯ
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        public int PublishCount { get; set; }

        /// <summary>
        /// ͷ����Ϣ
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// ģ��״̬����Ϊ 0-δ���ã�1-�����룬2-ͨ��
        /// </summary>
        public int ModelState { get; set; }

        /// <summary>
        /// �绰
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// �Ա�
        /// </summary>
        public string Sex { get; set; }

    }
}
