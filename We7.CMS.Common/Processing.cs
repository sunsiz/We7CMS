using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    /// <summary>
    /// ��������Ϣʵ����
    /// </summary>
    [Serializable]
    public class Processing
    {
        public Processing()
        {
            ProcessTotalLayer = 0;
            ProcessEndAction = ProcessEnding.Stop;
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ��Ӧ��ת��ʷ��¼�ı��
        /// </summary>
        public int ItemNum { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ObjectID { get; set; }

        /// <summary>
        /// ����ܼ���
        /// </summary>
        public int ProcessTotalLayer { get; set; }

        /// <summary>
        /// ��˽�������
        /// </summary>
        public ProcessEnding ProcessEndAction { get; set; }

        /// <summary>
        /// ��ǰ��˽��̼���
        /// </summary>
        public string CurLayerNO { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string ProcessDirection { get; set; }

        /// <summary>
        /// �����û�ID
        /// </summary>
        public string ProcessAccountID { get; set; }

        /// <summary>
        /// ǩ����
        /// </summary>
        public string ApproveTitle { get; set; }

        /// <summary>
        /// ǩ������
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
        /// ��ǰ���״̬
        /// </summary>
        public ProcessStates ProcessState
        {
            get
            {
                return (ProcessStates)int.Parse(CurLayerNO);
            }
            set
            {
                CurLayerNO = ((int)value).ToString();
            }
        }

        /// <summary>
        /// ��ǰ����״̬�ı�����
        /// </summary>
        public string ProcessText
        {
            get
            {
                if (string.IsNullOrEmpty(CurLayerNO))
                    CurLayerNO = "-100";
                string site = "";
                if (FromOtherSite && !string.IsNullOrEmpty(CurrentSiteName))
                    site = "[" + CurrentSiteName + "]";
                switch ((ProcessStates)int.Parse(CurLayerNO))
                {
                    case ProcessStates.Unaudit: return site + "���������";
                    case ProcessStates.FirstAudit: return site + "һ����";
                    case ProcessStates.SecondAudit: return site + "������";
                    case ProcessStates.ThirdAudit: return site + "������";
                    case ProcessStates.EndAudit: return site + "�������";
                    case ProcessStates.WaitAccept: return "������";
                    case ProcessStates.WaitHandle: return "������";
                    case ProcessStates.Finished: return "�Ѱ��";

                    default: return site + "";
                }
            }
        }

        /// <summary>
        /// ���������ı��������˻أ�����
        /// </summary>
        public string ProcessDirectionText
        {
            get
            {
                string ret = "";
                if (!string.IsNullOrEmpty(ProcessDirection) && !string.IsNullOrEmpty(CurLayerNO))
                {
                    ProcessAction pa = (ProcessAction)int.Parse(ProcessDirection);
                    if (((ProcessStates)int.Parse(CurLayerNO)) != ProcessStates.EndAudit)
                    {
                        switch (pa)
                        {
                            case ProcessAction.Previous:
                            case ProcessAction.Restart:
                                ret = "�˻�";
                                break;
                            case ProcessAction.Next:
                            case ProcessAction.SubmitSite:
                                ret = "��";
                                break;
                            default:
                                break;
                        }
                    }
                }
                return ret;
            }
        }

        /// <summary>
        /// ��ǰ��Ŀ��������ؼ���������Channel.FirstAudit ��Դ�� CurLayerNO=1
        /// </summary>
        public string CurLayerNOToChannel
        {
            get
            {
                switch (CurLayerNO)
                {
                    case "1": return "Channel.FirstAudit";
                    case "2": return "Channel.SecondAudit";
                     case "3": return "Channel.ThirdAudit";
                    default: return "";

                }
            }
        }

        /// <summary>
        /// ��ǰ������������ؼ���������Advice.FirstAudit ��Դ�� CurLayerNO=1
        /// </summary>
        public string CurLayerNOToAdvice
        {
            get{
                switch (CurLayerNO)
                {
                    case "1": return "Advice.FirstAudit";
                    case "2": return "Advice.SecondAudit";
                    case "3": return "Advice.ThirdAudit";
                    default: return "";
                }
            }
        }

        /// <summary>
        /// �������վ��
        /// </summary>
        public string SourceSiteID { get; set; }

        /// <summary>
        /// ���վ������
        /// </summary>
        public string SourceSiteName { get; set; }

        /// <summary>
        /// ǰһ��վ��
        /// </summary>
        public string PreviousSiteID { get; set; }

        /// <summary>
        /// �Ƿ���������վ��
        /// </summary>
        public bool FromOtherSite { get; set; }

        /// <summary>
        /// ��վ���Ŀ��վ��
        /// </summary>
        public string TargetSites { get; set; }

        /// <summary>
        /// ��վ����Ŀ��վ��ID
        /// </summary>
        public string TargetSiteID { get; set; }

        /// <summary>
        /// ��ǰվ������
        /// </summary>
        public string CurrentSiteName { get; set; }

        /// <summary>
        /// ��ǰվ��ID
        /// </summary>
        public string CurrentSiteID { get; set; }

        /// <summary>
        /// ����״̬���Բ�ͬվ��Ȩ�޽������ж�
        /// </summary>
        public ArticleStates ArticleState { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public AdviceState AdviceState { get; set; }

    }
}
