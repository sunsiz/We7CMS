using System;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using Thinkment.Data;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using System.Threading;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// �û�����ͳ�����ݲ�
    /// </summary>
    [Serializable]
    [Helper("We7.StatisticsHelper")]
    public class StatisticsHelper:BaseHelper
    {
        /*���ܣ��û�����ͳ�����ݲ�
         *���ߣ��ų���
         *���ڣ�2007-8-27
         */

        /// <summary>
        /// �����ͣ���¼��Ϣ
        /// </summary>
        public const int TypeCode_User = 0;

        /// <summary>
        /// �����ͣ����·���
        /// </summary>
        public const int TypeCode_Article = 1;

        /// <summary>
        /// ȡ��ͳ����Ϣ
        /// </summary>
        /// <param name="id">ͳ��ID</param>
        /// <returns></returns>
        public Statistics GetStatistics(string id)
        {
            Statistics s = new Statistics();
            s.ID = id;
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            int i=Assistant.Count<Statistics>(c);
            if (i > 0)
            {
                Assistant.Select(s);
                return s;
            }
            else
                return null;
        }

        /// <summary>
        /// ȡ��ͳ����Ϣ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="fields">��ѯ�ֶ�</param>
        /// <returns></returns>
        public Statistics GetStatistics(string id,string[] fields)
        {
            Statistics s = new Statistics();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<Statistics> cList = Assistant.List<Statistics>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                s = cList[0];
            }
            return s;
        }

        /// <summary>
        /// ��ȡ����ͳ���б�
        /// </summary>
        /// <returns></returns>
        public List<Statistics> GetStatisticses()
        {
            return Assistant.List<Statistics>(null, null);
        }

        /// <summary>
        /// ��ȡͳ����Ϣ�б�
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(Criteria c)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c,o);
        }

        /// <summary>
        /// ��ȡͳ����Ϣ�б���ҳ
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c, o, from, count);
        }

        /// <summary>
        /// ȡ������ͳ����Ϣ����ҳ
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetArticleStatisticses(int from, int count)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            Criteria c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Like, "ArticleID", "{%"));
            return Assistant.List<StatisticsHistory>(c, o, from, count);
        }

        /// <summary>
        /// ��ȡָ���ֶ�ͳ����Ϣ�б���ҳ
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(Criteria c, int from, int count,string[] fields)
        {
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c, o, from, count,fields);
        }

        /// <summary>
        /// ȡ��ĳһ�����ߵ�ͳ����Ϣ
        /// </summary>
        /// <param name="visitorid">������ID</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<StatisticsHistory> GetStatisticses(string visitorid, int from, int count, string[] fields)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorid);
            Order[] o = new Order[] { new Order("VisitDate", OrderMode.Desc) };
            return Assistant.List<StatisticsHistory>(c, o, from, count, fields);
        }

        /// <summary>
        /// ��ȡͳ����Ϣ����
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int GetStatisticsCount(Criteria c)
        {
            return Assistant.Count<Statistics>(c);
        }

        /// <summary>
        /// ȡ�����µķ�����
        /// </summary>
        /// <returns></returns>
        public int GetArticleStatisticsCount()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Like, "ArticleID","{%"));
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// ȡ��ĳһƪ���µķ�����
        /// </summary>
        /// <param name="articleid"></param>
        /// <returns></returns>
        public int GetArticleStatisticsCount(string articleid)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "ArticleID", articleid));
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// ȡ��ĳһ�������ߵķ�����
        /// </summary>
        /// <param name="visitorId">������</param>
        /// <returns></returns>
        public int GetStatisticsCount(string visitorId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// ȡ��ĳһλ������ĳ��ʱ�������ķ�����
        /// </summary>
        /// <param name="visitorId">������</param>
        /// <param name="startTime">��ʼʱ��</param>
        /// <returns></returns>
        public int GetStatisticsCount(string visitorId,DateTime startTime)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "VisitorID", visitorId);
            c.Add(CriteriaType.MoreThanEquals, "VisitDate", startTime);
            return Assistant.Count<StatisticsHistory>(c);
        }

        /// <summary>
        /// ����һ��ͳ����Ϣ
        /// </summary>
        /// <param name="s">ͳ����Ϣ</param>
        /// <param name="type"></param>
        public void AddStatistics(Statistics s)
        {
            s.ID = We7Helper.CreateNewID();
            s.VisitDate = DateTime.Now;
            Assistant.Insert(s);
        }

        /// <summary>
        /// ���ͳ����Ϣ
        /// </summary>
        /// <param name="pv">�����</param>
        /// <param name="ArticleID">����ID</param>
        /// <param name="ColumnID">��ĿID</param>
        public void AddStatistics(PageVisitor pv, string ArticleID, string ColumnID)
        {
            HttpContext Context = HttpContext.Current;
            Statistics s = new Statistics();
            s.VisitorID = pv.ID;
            s.ArticleID = ArticleID;
            s.ChannelID = ColumnID;
            s.Url = Context.Request.RawUrl;
            AddStatistics(s);
        }
        /// <summary>
        /// ɾ��һ��ͳ����Ϣ
        /// </summary>
        /// <param name="id"></param>
        public void DeleteStatistics(string id)
        {
            Statistics s = GetStatistics(id);
            if(s!=null)
                Assistant.Delete(s);
        }

        /// <summary>
        /// ɾ��һ��ͳ����Ϣ
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteStatistics(List<string> ids)
        {
            foreach (string id in ids)
            {
                DeleteStatistics(id);
            }
        }

    }
}
