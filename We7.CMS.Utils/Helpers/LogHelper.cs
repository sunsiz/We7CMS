using System;
using System.Collections.Generic;
using System.Text;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// ��־ҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.LogHelper")]
    public class LogHelper:BaseHelper
    {
        /// <summary>
        /// ��ȡȫ����־����
        /// </summary>
        /// <returns></returns>
        public int QueryAllLogCount()
        {
            Order[] o = new Order[] { new Order("ID") };
            return Assistant.List<Log>(null, o).Count;
        }
        /// <summary>
        /// ����������ȡ��־����
        /// </summary>
        /// <param name="c">Criteria</param>
        /// <returns></returns>
        public int QueryLogCount(Criteria c)
        {
            return Assistant.Count<Log>(c);
        }
        /// <summary>
        /// �õ���־
        /// </summary>
        /// <param name="logID">��־ID</param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Log GetLog(string id, string[] fields)
        {
            Log l = new Log();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<Log> cList = Assistant.List<Log>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                l = cList[0];
            }
            return l;
        }
        /// <summary>
        /// ���������õ���־�б�
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Log> GetLogs(Criteria c)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            List<Log> ts = Assistant.List<Log>(c, o);
            return ts;
        }
        /// <summary>
        /// ���������õ���ҳ��־�б�
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Log> GetPagedLogs(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Log>(c, o, from, count);
        }
        /// <summary>
        /// �õ�ȫ����־�б�
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Log> GetPagedAllLogs(int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Log>(null, o, from, count);
        }
        /// <summary>
        /// �����־
        /// </summary>
        /// <param name="l">��־</param>
        public void AddLog(Log l)
        {
            l.Created = DateTime.Now;
            l.ID = We7Helper.CreateNewID();
            Assistant.Insert(l);
        }
        /// <summary>
        /// ɾ����־
        /// </summary>
        /// <param name="logID">��־ID</param>
        public void DeleteLog(string id)
        {
            Log l = new Log();
            l.ID = id;
            Assistant.Delete(l);
        }
        /// <summary>
        /// ��¼��־
        /// </summary>
        /// <param name="accountID">��ǰ�û�ID</param>
        /// <param name="page">��ǰҳ��</param>
        /// <param name="content">��������</param>
        public void WriteLog(string accountID,string page,string content,string remark)
        {
            Log l = new Log();
            l.UserID = accountID;
            l.Page = page;
            l.Content = content;
            l.Remark = remark;
            
            AddLog(l);
        }

        /// <summary>
        /// ��ѯ��־��¼��
        /// </summary>
        /// <param name="Author">�û�</param>
        /// <param name="BeginDate">��ʼʱ��</param>
        /// <param name="EndDate">����ʱ��</param>
        /// <param name="Page">ʲôҳ��</param>
        /// <returns></returns>
        public int QueryLogCountByAll(string Author, DateTime BeginDate, DateTime EndDate,string Page)
        {
            Criteria c = CreateCriteriaByAll(Author, BeginDate,EndDate,Page);
            return Assistant.Count<Log>(c);
        }

        /// <summary>
        /// ��ѯ��־��¼
        /// </summary>
        /// <param name="Author">�û�</param>
        /// <param name="BeginDate">��ʼʱ��</param>
        /// <param name="EndDate">����ʱ��</param>
        /// <param name="Page">ʲôҳ��</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��ѯ����</param>
        /// <param name="fields">��ѯ�ֶ�</param>
        /// <param name="orderKey">����ؼ���</param>
        /// <param name="up"></param>
        /// <returns></returns>
        public List<Log> QueryLogsByAll(string Author, DateTime BeginDate, DateTime EndDate,string Page, int from, int count, string[] fields, string orderKey,bool up)
        {
            Criteria c = CreateCriteriaByAll(Author,BeginDate,EndDate,Page);

            OrderMode mode = OrderMode.Asc;
            if (!up) mode = OrderMode.Desc;
            Order[] orders = new Order[] { new Order(orderKey, mode) };

            return Assistant.List<Log>(c, orders, from, count, fields);
        }

        /// <summary>
        /// �����ѯ��������
        /// </summary>
        /// <param name="Author"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        Criteria CreateCriteriaByAll(string Author, DateTime BeginDate, DateTime EndDate, string Page)
        {
            Criteria c = new Criteria(CriteriaType.NotEquals,"ID","");

            if (Author != null && Author.Length > 0)
            {
                c.Add(CriteriaType.Like, "UserID", Author);
            }

            if (Page != null && Page.Length > 0)
            {
                c.Add(CriteriaType.Like, "Page", "%" + Author + "%");
            }

            if (BeginDate <= EndDate)
            {
                if (BeginDate.ToString() != DateTime.MinValue.ToString())
                    c.Add(CriteriaType.MoreThanEquals, "Created", BeginDate);
                if (EndDate.ToString() != DateTime.MaxValue.ToString())
                    c.Add(CriteriaType.LessThanEquals, "Created", EndDate.AddDays(1));
            }
            else
            {
                if (EndDate.ToString() != DateTime.MinValue.ToString())
                    c.Add(CriteriaType.MoreThanEquals, "Created", EndDate);
                if (BeginDate.ToString() != DateTime.MaxValue.ToString())
                    c.Add(CriteriaType.LessThanEquals, "Created", BeginDate.AddDays(1));
            }

            return c;
        }
    }
}
