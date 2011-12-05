using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using System.Xml;
using System.Data;
using System.IO;
using We7.Framework;

namespace We7.CMS.Common
{

    /// <summary>
    /// ������Ϣ
    /// </summary>
    [Serializable]
    public class AdviceInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ��ˮ��
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// �û�ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// �û���
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// �绰
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// ��ַ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// ��������ID
        /// </summary>
        public string TypeID { get; set; }

        /// <summary>
        /// �Ƿ�ǰ̨��ʾ
        /// 0��ǰ̨����ʾ,1:ǰ̨��ʾ
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// ��ʾ״̬�ı�
        /// </summary>
        public string IsShowText
        {
            get { return IsShow == 1 ? "��ʾ" : "����ʾ"; }
        }

        /// <summary>
        /// ����״̬
        /// 0��δ����1��������2�������������У�3�������������У�9���Ѱ��
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// ����״̬����
        /// </summary>
        public string StateText
        {
            get
            {
                switch (State)
                {
                    case 0:
                        return "δ����";
                    case 1:
                        return "������";
                    case 2:
                        return "������������";
                    case 3:
                        return "������ת����";
                    case 9:
                        return "�Ѱ��";
                    default:
                        return "δ����";
                }
            }
        }

        /// <summary>
        /// �������ȼ�
        /// 0���ɰ�ɲ��죬1���ذ죬2���߰�
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// ���ȼ�״̬����
        /// </summary>
        public string PriorityText
        {
            get
            {
                switch (Priority)
                {
                    case 0:
                        return "��ͨ";
                    case 1:
                        return "�ذ�";
                    case 2:
                        return "�߰�";
                    default:
                        return "��ͨ";
                }
            }
        }

        /// <summary>
        /// �Ѷ�
        /// 0:δ��,1:�Ѷ�
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// �Ѷ�״̬����
        /// </summary>
        public string IsReadText
        {
            get
            {
                return IsRead == 1 ? "�Ѷ�" : "δ��";
            }
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        public int Public { get; set; }

        /// <summary>
        /// ����״̬��������
        /// </summary>
        public string PublicText
        {
            get
            {
                switch (Public)
                {
                    case 0:
                        return "������";
                    case 1:
                        return "����";
                    default:
                        return "������";
                }
            }
        }

        /// <summary>
        /// �ö�
        /// </summary>
        public int IsTop { get; set; }

        public string IsTopText
        {
            get
            {
                return IsTop == 1 ? "�ö�" : "δ�ö�";
            }
        }


        /// <summary>
        /// ��ѯ����
        /// </summary>
        public string MyQueryPwd { get; set; }

        /// <summary>
        /// ��ѯ�ؼ���1
        /// </summary>
        public string Display1 { get; set; }

        /// <summary>
        /// ��ѯ�ؼ���2
        /// </summary>
        public string Display2 { get; set; }

        /// <summary>
        /// ��ѯ�ؼ���3
        /// </summary>
        public string Display3 { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelXml { get; set; }

        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// ģ�Ͷ����ļ�
        /// </summary>
        public string ModelSchema { get; set; }

        #region ���ģ�͵���չ

        /// <summary>
        /// ��ȡģ������
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object this[string field]
        {
            get
            {
                try
                {
                    return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 && DataSet.Tables[0].Columns.Contains(field) ? DataSet.Tables[0].Rows[0][field] : null;
                }
                catch { }
                return null;
            }
        }
        /// <summary>
        /// ��������ģ�����ݼ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataSet CreateDataSet(string path)
        {
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(SchemaPath);
            return ds;
        }

        /// <summary>
        /// ��������ģ�����ݼ�
        /// </summary>
        /// <returns></returns>
        DataSet CreateDataSet()
        {
            DataSet ds = new DataSet();
            using (TextReader reader = new StringReader(ModelSchema))
            {
                ds.ReadXmlSchema(reader);
            }
            return ds;
        }

        private string schemapath;
        /// <summary>
        /// Schema·��
        /// </summary>
        public string SchemaPath
        {
            get
            {
                if (String.IsNullOrEmpty(schemapath))
                {
                    throw new Exception("û���趨Schema·��");
                }
                return schemapath;
            }
            set { schemapath = value; }
        }

        /// <summary>
        /// ȡ�����µ�����ģ�����ݼ�
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        DataSet GetDataSet()
        {
            DataSet ds = !String.IsNullOrEmpty(ModelSchema) ? CreateDataSet() : CreateDataSet(SchemaPath);
            TextReader reader = new StringReader(ModelXml);
            ds.ReadXml(reader);
            return ds;
        }

        private DataSet dataset;
        public DataSet DataSet
        {
            get
            {
                if (!isdirty)
                {
                    dataset = GetDataSet();
                    isdirty = true;
                }
                return dataset;
            }
        }

        public DataRow DataRow
        {
            get
            {
                return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 ? DataSet.Tables[0].Rows[0] : null;
            }
        }

        private bool isdirty = false;

        #endregion

        #region ��Ե����

        /// <summary>
        /// �յ����
        /// </summary>
        public int DayClicks { get; set; }

        /// <summary>
        /// ���յ����
        /// </summary>
        public int YesterdayClicks { get; set; }

        /// <summary>
        /// �ܵ����
        /// </summary>
        public int WeekClicks { get; set; }

        /// <summary>
        /// �µ����
        /// </summary>
        public int MonthClicks { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public int QuarterClicks { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int YearClicks { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public int Clicks { get; set; }


        #endregion

        /// <summary>
        /// ������Ϣ���
        /// </summary>
        public string RelationID { get; set; }

        /// <summary>
        /// ��������ģ��
        /// </summary>
        public string RelationModelName { get; set; }
    }


    /// <summary>
    /// ����ת��
    /// </summary>
    [Serializable]
    public class AdviceTransfer
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// ����ID
        /// </summary>
        public string AdviceID { get; set; }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public AdviceInfo Advice { get; set; }

        /// <summary>
        /// ת����Դ
        /// </summary>
        public string FromTypeID { get; set; }

        /// <summary>
        /// ת�Ʒ���
        /// </summary>
        public string ToTypeID { get; set; }

        /// <summary>
        /// �����û�ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string Suggest { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }
    }


    /// <summary>
    /// ������Ȩ
    /// </summary>
    [Serializable]
    public class AdviceAuth
    {
        public string ID { get; set; }

        /// <summary>
        /// ��Ȩʵ��ID
        /// </summary>
        public string AuthID { get; set; }

        /// <summary>
        /// ��Ȩ����
        /// 0���û�,1:��ɫ,2:����
        /// </summary>
        public int AuthType { get; set; }

        /// <summary>
        /// ��������ID
        /// </summary>
        public string AdviceTypeID { get; set; }

        /// <summary>
        /// ����
        /// 1���鿴��2������3������4������
        /// </summary>
        public int Function { get; set; }
    }


    #region ��ǰ�Ĵ���

    /// <summary>
    /// ������Ϣ
    /// </summary>
    [Serializable]
    public class Advice : ProcessObject
    {
        /// <summary>
        ///������Ϣ��
        /// </summary>
        private string id;
        private string typeID;
        private string typeTitle;
        private string userID;
        private string userName;
        private string title;
        private string content;
        private DateTime createDate = DateTime.Now;
        private int replyCount;
        private string email;
        private string replyDepID;
        private int isShow;
        private long sn;

        private string replyDept;
        private int state;
        string modelXml;
        DateTime updated = DateTime.Now;
        string myQueryPwd;
        string curProcessState;
        string enumState;

        private string snString;
        private string replyTime;
        private string replyMan;
        string fullTitle;
        string replyState;
        string linkUrl;
        string timeNote;
        int mustHandle;
        DateTime toHandleTime;

        int alertNote;
        string adviceUrl;

        string adviceInfoType;
        string toOtherHandleUserID;

        string phone;
        string fax;
        string address;
        string adviceTag;
        int isRead = 0;

        public Advice()
        {
            ProcessState = ((int)ProcessStates.WaitAccept).ToString();
        }

        /// <summary>
        ///ת��������
        /// </summary>
        public string ToOtherHandleUserID
        {
            get { return toOtherHandleUserID; }
            set { toOtherHandleUserID = value; }
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        public string AdviceInfoType
        {
            get { return adviceInfoType; }
            set { adviceInfoType = value; }
        }

        public string AdviceUrl
        {
            get { return adviceUrl; }
            set { adviceUrl = value; }
        }

        /// <summary>
        /// ת������ʱ��
        /// </summary>
        public DateTime ToHandleTime
        {
            get { return toHandleTime; }
            set { toHandleTime = value; }
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
        /// ״̬��Ϣ
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private string stateText;
        /// <summary>
        /// ����״̬����
        /// </summary>
        public string StateText
        {
            get
            {
                AdviceState ast = (AdviceState)System.Enum.Parse(typeof(AdviceState), State.ToString());
                string stateText = "";
                switch (ast)
                {
                    case AdviceState.All:
                        stateText = "ȫ��";
                        break;
                    case AdviceState.WaitAccept:
                        stateText = "������";
                        break;
                    case AdviceState.WaitHandle:
                        stateText = "������";
                        break;
                    case AdviceState.Checking:
                        stateText = "�����";
                        break;
                    case AdviceState.Finished:
                        stateText = "���";
                        break;
                    default:
                        break;
                }

                return stateText;
            }
        }

        private string ownID;

        /// <summary>
        /// ��ĿΨһ��ʾ��
        /// </summary>
        public string OwnID
        {
            get { return ownID; }
            set { ownID = value; }
        }

        /// <summary>
        /// ���
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// �����
        /// </summary>
        public string TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public string TypeTitle
        {
            get { return typeTitle; }
            set { typeTitle = value; }
        }

        /// <summary>
        /// �û����
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        public string Name
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// �ظ��ʼ�
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
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
        /// ����
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
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
        /// �ظ���
        /// </summary>
        public int ReplyCount
        {
            get { return replyCount; }
            set { replyCount = value; }
        }

        /// <summary>
        /// ��������Ψһ��ʾ��
        /// </summary>
        public string ReplyDepID
        {
            get { return replyDepID; }
            set { replyDepID = value; }
        }

        /// <summary>
        /// ������Ϣ�Ƿ񹫿���ʾ
        /// </summary>
        public int IsShow
        {
            get { return isShow; }
            set { isShow = value; }
        }

        /// <summary>
        /// ������Ϣ��ˮ��
        /// </summary>
        public long SN
        {
            get { return sn; }
            set { sn = value; }
        }

        /// <summary>
        /// ��ˮ����Ϣ
        /// </summary>
        public string SnString
        {
            get { return snString; }
            set { snString = value; }
        }

        /// <summary>
        /// ���ӵ�ַ
        /// </summary>
        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }

        /// <summary>
        /// ʱ���¼
        /// </summary>
        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
        }

        /// <summary>
        /// �ظ�ʱ��
        /// </summary>
        public string ReplyTime
        {
            get { return replyTime; }
            set { replyTime = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string ReplyMan
        {
            get { return replyMan; }
            set { replyMan = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string ReplyDept
        {
            get { return replyDept; }
            set { replyDept = value; }
        }

        /// <summary>
        /// ������Ϣ��������
        /// </summary>
        public string FullTitle
        {
            get { return fullTitle; }
            set { fullTitle = value; }
        }

        /// <summary>
        /// ������Ϣ���ӵ�ַ
        /// </summary>
        public string FullUrl
        {
            get
            {
                return We7Helper.GUIDToFormatString(this.ID) + ".html";
            }
        }

        /// <summary>
        /// �����չ��ϢXML����
        /// </summary>
        public string ModelXml
        {
            get { return modelXml; }
            set { modelXml = value; }
        }

        /// <summary>
        /// ���Բ�ѯ����
        /// </summary>
        public string MyQueryPwd
        {
            get { return myQueryPwd; }
            set { myQueryPwd = value; }
        }

        /// <summary>
        ///  Ĭ�ϡ�0��Ϊ�ɲ��죬��1��Ϊ�ذ�, ��2��Ϊ�߰졣
        ///  ���״̬Ϊ�߰죬�Ǳض�Ҳ�Ǳذ�ġ����ԣ��ذ�����Ϊ >=1
        /// </summary>
        public int MustHandle
        {
            get { return mustHandle; }
            set { mustHandle = value; }
        }

        /// <summary>
        /// ����״̬��ʾͼƬ��Ϣ
        /// </summary>
        public string MustHandleText
        {
            get
            {
                switch (MustHandle)
                {
                    case 0:
                        return "";
                        break;

                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return "<img src='/admin/images/ok.gif' />";
                        break;

                    default:
                        return "";
                        break;
                }
            }
        }

        /// <summary>
        /// ״̬
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        string processText;
        /// <summary>
        /// ������Ϣ�����Ϣ
        /// </summary>
        public string ProcessText
        {
            get
            {
                ProcessStates pst = (ProcessStates)System.Enum.Parse(typeof(ProcessStates), ProcessState);
                string processText = "";
                switch (pst)
                {
                    case ProcessStates.Unaudit:
                        processText = "�ݸ�";
                        break;
                    case ProcessStates.FirstAudit:
                        processText = "һ��";
                        break;
                    case ProcessStates.SecondAudit:
                        processText = "����";
                        break;
                    case ProcessStates.ThirdAudit:
                        processText = "����";
                        break;
                    case ProcessStates.EndAudit:
                        processText = "���";
                        break;
                    default:
                        break;
                }
                return stateText;
            }
        }

        /// <summary>
        /// ת������ʱ��
        /// </summary>
        public string ToHandleTimeText
        {
            get
            {
                if (ToHandleTime != DateTime.MaxValue && ToHandleTime != DateTime.MinValue && State != (int)AdviceState.WaitAccept)
                {
                    TimeSpan c = DateTime.Now.Subtract(ToHandleTime);
                    if (c.Days == 0)
                    {
                        if (c.Hours == 0)
                            return c.Minutes.ToString() + "����ǰ";
                        else
                            return c.Hours.ToString() + "Сʱǰ";
                    }
                    else
                        return c.Days.ToString() + "��ǰ";
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// �Ƿ����ѱ�ʶ
        /// </summary>
        public int AlertNote
        {
            get { return alertNote; }
            set { alertNote = value; }
        }

        string display1;
        /// <summary>
        /// �����ֶ�1��������ǰ̨�б���ʾ
        /// </summary>
        public string Display1
        {
            get { return display1; }
            set { display1 = value; }
        }

        string display2;
        /// <summary>
        /// �����ֶ�1��������ǰ̨�б���ʾ
        /// </summary>
        public string Display2
        {
            get { return display2; }
            set { display2 = value; }
        }

        string display3;
        /// <summary>
        /// �����ֶ�1��������ǰ̨�б���ʾ
        /// </summary>
        public string Display3
        {
            get { return display3; }
            set { display3 = value; }
        }

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

        #region ���ģ�͵���չ

        /// <summary>
        /// ��ȡģ������
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object this[string field]
        {
            get
            {
                return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 && DataSet.Tables[0].Columns.Contains(field) ? DataSet.Tables[0].Rows[0][field] : null;
            }
        }
        /// <summary>
        /// ��������ģ�����ݼ�
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataSet CreateDataSet(string path)
        {
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(SchemaPath);
            return ds;
        }

        /// <summary>
        /// ��������ģ�����ݼ�
        /// </summary>
        /// <returns></returns>
        DataSet CreateDataSet()
        {
            DataSet ds = new DataSet();
            using (TextReader reader = new StringReader(ModelSchema))
            {
                ds.ReadXmlSchema(reader);
            }
            return ds;
        }

        private string schemapath;
        /// <summary>
        /// Schema·��
        /// </summary>
        public string SchemaPath
        {
            get
            {
                if (String.IsNullOrEmpty(schemapath))
                {
                    throw new Exception("û���趨Schema·��");
                }
                return schemapath;
            }
            set { schemapath = value; }
        }

        /// <summary>
        /// ȡ�����µ�����ģ�����ݼ�
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        DataSet GetDataSet()
        {
            DataSet ds = !String.IsNullOrEmpty(ModelSchema) ? CreateDataSet() : CreateDataSet(SchemaPath);
            TextReader reader = new StringReader(ModelXml);
            ds.ReadXml(reader);
            return ds;
        }

        private DataSet dataset;
        public DataSet DataSet
        {
            get
            {
                if (!isdirty)
                {
                    dataset = GetDataSet();
                    isdirty = true;
                }
                return dataset;
            }
        }

        public DataRow DataRow
        {
            get
            {
                return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 ? DataSet.Tables[0].Rows[0] : null;
            }
        }

        private bool isdirty = false;

        #endregion

        /// <summary>
        /// �û��绰
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        /// <summary>
        /// �û��������
        /// </summary>
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        /// <summary>
        /// �û�סַ
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        public string AdviceTag
        {
            get { return adviceTag; }
            set { adviceTag = value; }
        }


        /// <summary>
        /// �Ƿ���ʾδ�����
        /// </summary>
        public int IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }

    }

    /// <summary>
    /// ����ͳ��
    /// </summary>
    [Serializable]
    public class AdviceRate
    {
        string adviceTypeID;
        string adviceTypeTitle;
        int adviceCount;
        int handleNumber;
        int noHandleNumber;
        int handleCount;
        int noHandleCount;
        string handleRate;
        int noAdminHandleCount;
        string adviceInfoType;
        int notAdminMustHandleCount;

        /// <summary>
        /// �������������״̬Ϊ����������Ϣ��
        /// </summary>
        public int NotAdminMustHandleCount
        {
            get { return notAdminMustHandleCount; }
            set { notAdminMustHandleCount = value; }
        }

        /// <summary>
        /// ����ģ��ID
        /// </summary>
        public string AdviceTypeID
        {
            get { return adviceTypeID; }
            set { adviceTypeID = value; }
        }

        /// <summary>
        /// ����ģ������
        /// </summary>
        public string AdviceTypeTitle
        {
            get { return adviceTypeTitle; }
            set { adviceTypeTitle = value; }
        }

        /// <summary>
        ///�ܼ���
        /// </summary>
        public int AdviceCount
        {
            get { return adviceCount; }
            set { adviceCount = value; }
        }

        /// <summary>
        /// Ӧ������
        /// </summary>
        public int HandleNumber
        {
            get { return handleNumber; }
            set { handleNumber = value; }
        }


        /// <summary>
        /// ���账����
        /// </summary>
        public int NoHandleNumber
        {
            get { return noHandleNumber; }
            set { noHandleNumber = value; }
        }

        /// <summary>
        /// �ܴ�����
        /// </summary>
        public int HandleCount
        {
            get { return handleCount; }
            set { handleCount = value; }
        }

        /// <summary>
        /// �����˴��������ǹ���Ա����Ĵ�������
        /// </summary>
        public int NoAdminHandleCount
        {
            get { return noAdminHandleCount; }
            set { noAdminHandleCount = value; }
        }
        /// <summary>
        /// �ܹ�δ������
        /// </summary>
        public int NoHandleCount
        {
            get { return noHandleCount; }
            set { noHandleCount = value; }
        }

        /// <summary>
        /// ����ʣ�������
        /// </summary>
        public string HandleRate
        {
            get { return handleRate; }
            set { handleRate = value; }
        }

        /// <summary>
        /// ������Ϣ����
        /// </summary>
        public string AdviceInfoType
        {
            get { return adviceInfoType; }
            set { adviceInfoType = value; }
        }
    }

    #endregion
}
