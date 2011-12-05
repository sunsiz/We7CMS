using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    /// <summary>
    /// ����ģ��
    /// </summary>
    [Serializable]
    public class AdviceType
    {
        private string id;
        private string title;
        private string description;
        private DateTime createDate=DateTime.Now;
        DateTime updated=DateTime.Now;
        string modelXml;
        string modelXmlName;

        string accountID;
        string enumState;
        int toWhichDepartment;
        int flowSeries;
        int flowInnerDepart;
        string mailMode;
        int useSystemMail;
        string mailSMTPServer;
        string pOPServer;
        string mailUser;
        string mailPassword;
        string sMSUser;
        int remindDays;
        string mailAddress;
        int participateMode;
        string stateText;

        public AdviceType()
        {}

        /// <summary>
        /// ���
        /// </summary>
        public string ID
        {
            get{return id;}
            set{id=value;}
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
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
        /// �����չ��ϢXML��������
        /// </summary>
        public string ModelXml
        {
            get { return modelXml; }
            set { modelXml = value; }
        }

        /// <summary>
        /// ������������
        /// </summary>
        public string ModelXmlName
        {
            get { return modelXmlName; }
            set { modelXmlName = value; }
        }

        /// <summary>
        /// ģ�ʹ�����
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// ģʽ״̬��ֱ�Ӱ���ת�������ϱ�����
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// ת���������Ź��ˣ��ṩ����һ�鲿�Ź�ѡ��1��ͬ�����š�2���¼����ţ�3�����в��ţ�
        /// </summary>
        public int ToWhichDepartment
        {
            get { return toWhichDepartment; }
            set { toWhichDepartment = value; }
        }

        /// <summary>
        /// �ϱ���˼���
        /// </summary>
        public int FlowSeries
        {
            get { return flowSeries; }
            set { flowSeries = value; }
        }

        /// <summary>
        /// �����϶�����0-��ᣬ������ã�1-����ֱ�����ã�2-�Ϳ�վ���
        /// </summary>
        public string ProcessEnd { get; set; }

        /// <summary>
        /// �Ƿ��ڲ��������  0 �� ��1 ��
        /// </summary>
        public int FlowInnerDepart
        {
            get { return flowInnerDepart; }
            set { flowInnerDepart = value; }
        }

        /// <summary>
        /// �ʼ�������ʽ
        /// </summary>
        public string MailMode
        {
            get { return mailMode; }
            set { mailMode = value; }
        }

        /// <summary>
        /// �Ƿ�ʹ��Ĭ���ʼ���ַ 0 Ĭ�ϣ� 1 ר��
        /// </summary>
        public int UseSystemMail
        {
            get { return useSystemMail; }
            set { useSystemMail = value; }
        }

        /// <summary>
        /// �ʼ�SMTP������
        /// </summary>
        public string MailSMTPServer
        {
            get { return mailSMTPServer; }
            set { mailSMTPServer = value; }
        }

        /// <summary>
        /// POP������
        /// </summary>
        public string POPServer
        {
            get { return pOPServer; }
            set { pOPServer = value; }
        }

        /// <summary>
        /// �����û���
        /// </summary>
        public string MailUser
        {
            get { return mailUser; }
            set { mailUser = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string MailPassword
        {
            get { return mailPassword; }
            set { mailPassword = value; }
        }

        /// <summary>
        /// ����֪ͨ��
        /// </summary>
        public string SMSUser
        {
            get { return sMSUser; }
            set { sMSUser = value; }
        }

        /// <summary>
        /// ��ʱ�Զ��߰�
        /// </summary>
        public int RemindDays
        {
            get { return remindDays; }
            set { remindDays = value; }
        }

        /// <summary>
        /// �����Ͱ�����
        /// </summary>
        public string MailAddress
        {
            get { return mailAddress; }
            set { mailAddress = value; }
        }

        /// <summary>
        /// ������ʽ��0 ���ʼ����룻1 �Ƕ���֪ͨ
        /// </summary>
        public int ParticipateMode
        {
            get { return participateMode; }
            set { participateMode = value; }
        }

        /// <summary>
        ///ģ������
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// ģʽ״̬
        /// </summary>
        public string StateText
        {
            get
            {
                switch (EnumState)
                {
                    case "00000000000000000000": return "ֱ�Ӱ���";
                    case "00010000000000000000": return "ת������";
                    case "00020000000000000000": return "�ϱ�����";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// ת������
        /// </summary>
        public string ToWhichDepartmentText
        {
            get
            {
                switch ((AdviceToWhichDepartment)ToWhichDepartment)
                {
                    case AdviceToWhichDepartment.LowLevel: return "�¼�����";
                    case AdviceToWhichDepartment.Samelevel: return "ͬ������";
                    case AdviceToWhichDepartment.All: return "���в���";
                    default:
                        return "���в���";
                }
            }
        }
    }
}
