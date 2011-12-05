using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework.Config;

namespace We7.CMS.Common
{
    /// <summary>
    /// ������ʷ��¼��
    /// </summary>
    [Serializable]
    public class ProcessHistory
    {
        public ProcessHistory()
        {
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ��ǰ��Ŀ�����»�����ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// ��ǰ�������̲�����������һ����
        /// </summary>
        public string FromProcessState { get; set; }

        /// <summary>
        /// ��˷���/����
        /// </summary>
        public string ProcessDirection { get; set; }

        /// <summary>
        /// ��һ�������ڲ㼶
        /// </summary>
        public string ToProcessState { get; set; }

        /// <summary>
        /// ��վ���Ŀ��վ��
        /// </summary>
        public string TargetSites { get; set; }

        /// <summary>
        /// �����û�ID
        /// </summary>
        public string ProcessAccountID { get; set; }

        /// <summary>
        /// ǩ����
        /// </summary>
        public string ApproveTitle { get; set; }

        /// <summary>
        /// ǩ��������
        /// </summary>
        public string ApproveName { get; set; }


        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateDate { get; set; }


        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// ��һ���������������
        /// </summary>
        public string ProcessText
        {
            get
            {
                string site = "";
                if (!string.IsNullOrEmpty(TargetSites) && SiteConfigs.GetConfig().SiteGroupEnabled)
                    site = "վ��[" + TargetSites + "]";

                switch ((ProcessStates)int.Parse(ToProcessState))
                {
                    case ProcessStates.FirstAudit: return site + "һ��";
                    case ProcessStates.SecondAudit: return site + "����";
                    case ProcessStates.ThirdAudit: return site + "����";
                    case ProcessStates.EndAudit: return site + "�������";
                    case ProcessStates.WaitAccept: return site + "����";
                    case ProcessStates.WaitHandle: return site + "����";

                    default: return site + "�༭";
                }
            }
        }

        /// <summary>
        /// ��ǰ��������������������һ���������
        /// </summary>
        public string ProcessingText 
        {
            get
            {
                return GetProcessingText(FromProcessState);
            }
        }

        /// <summary>
        /// ״̬ת��Ϊ����
        /// </summary>
        /// <param name="layno"></param>
        /// <returns></returns>
        public string GetProcessingText(string layno)
        {
            string site = "";
            if (!string.IsNullOrEmpty(SiteName) && SiteConfigs.GetConfig().SiteGroupEnabled)
                site = "վ��[" + SiteName + "]";
            switch (layno)
            {
                case "-1": return site + "������";
                case "-3": return site + "������";
                case "0": return site + "���������";
                case "1": return site + "һ����";
                case "2": return site + "������";
                case "3": return site + "������";
                default: return site + "���������";
            }
        }

        /// <summary>
        /// Ȩ�޼���
        /// </summary>
        public string CurLayerNOToChannel
        {
            get
            {
                switch (ToProcessState)
                {
                    case "1": return "Channel.FirstAudit";
                    case "2": return "Channel.SecondAudit";
                    case "3": return "Channel.ThirdAudit";
                    default: return "";
                }
            }
        }

        /// <summary>
        /// ���������ַ�ת��
        /// </summary>
        public string ProcessDirectionText
        {
            get
            {
                string ret = "";
                if (string.IsNullOrEmpty(ToProcessState)) ToProcessState = "0";
                ProcessAction pa = (ProcessAction)int.Parse(ProcessDirection);
                if (((ProcessStates)int.Parse(ToProcessState)) != ProcessStates.EndAudit)
                {
                    switch (pa)
                    {
                        case ProcessAction.Previous:
                        case ProcessAction.Restart:
                            ret= "�˻�";
                            break;
                        case ProcessAction.Next:
                        case ProcessAction.SubmitSite:
                            ret= "��";
                            break;
                        default:
                            break;
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// ��ǰ�����������վ������
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// ��ǰ�����������վ��ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// ��ǰ�����������վ��Ӧ�ýӿڵ�ַ
        /// </summary>
        public string SiteApiUrl { get; set; }

        /// <summary>
        /// ����������ĿID
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// ����������Ŀ����
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public int ItemNum { get; set; }

    }
}

