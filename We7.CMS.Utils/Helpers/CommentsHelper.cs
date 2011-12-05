using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.SessionState;


using Thinkment.Data;
using We7.CMS.Common;
using We7.Framework;

//2007-8-21 ���۱�
namespace We7.CMS
{
    /// <summary>
    /// ����ҵ��������
    /// </summary>
    [Serializable]
    [Helper("We7.CommentsHelper")]
    public class CommentsHelper:BaseHelper
    {
        /// <summary>
        /// �õ�����������
        /// </summary>
        /// <returns></returns>
        public int QueryAllCommentsCount(bool OnlyInUser)
        {
            if (OnlyInUser)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "State", 1);
            }
            return Assistant.Count<Comments>(null);
        }

        /// <summary>
        /// �õ�����������
        /// </summary>
        /// <returns></returns>
        public int QueryAllCommentsCount()
        {
            return QueryAllCommentsCount(false);
        }

        //2007-9-25 zjq
        /// <summary>
        /// �õ�һƪ���µ�������������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ArticleIDQueryCommentsCount(string ArticleID, bool OnlyInUser)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", ArticleID);
            if (OnlyInUser)
                c.Add(CriteriaType.Equals, "State", 1);

            return Assistant.Count<Comments>(c);
        }
        public int ArticleIDQueryCommentsCount(string ArticleID)
        {
            return ArticleIDQueryCommentsCount(ArticleID, false);
        }
        /// <summary>
        /// �õ���������
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <param name="OnlyInUser"></param>
        /// <returns></returns>
        public List<Comments> QueryAllComments(int from, int count, string[] fields, bool OnlyInUser)
        { 
            if (OnlyInUser)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "State", 1);
            }
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Comments>(null, o, from, count, fields);
        }
       
        public List<Comments> QueryAllComments(int from, int count, string[] fields)
        { 
            return QueryAllComments(from,count,fields,false);
        }

        //2007-9-25 zjq
        /// <summary>
        /// �õ�һƪ���µ���������
        /// </summary>
        /// <param name="id"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<Comments> ArticleIDQueryComments(string ArticleID, int from, int count, string[] fields,bool OnlyInUser)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", ArticleID);
            if (OnlyInUser)
                c.Add(CriteriaType.Equals, "State", 1);
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Comments>(c, orders, from, count, fields);
        }

        /// <summary>
        /// ��������ID��ѯ����
        /// </summary>
        /// <param name="ArticleID">����ID</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��¼����</param>
        /// <param name="fields">��ѯ�ֶ�</param>
        /// <returns></returns>
        public List<Comments> ArticleIDQueryComments(string ArticleID, int from, int count, string[] fields)
        {
            return ArticleIDQueryComments(ArticleID, from, count, fields,false);
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id"></param>
        public void DeleteComment(string id)
        {
            Comments c = new Comments();
            c.ID = id;
            Assistant.Delete(c);
        }
        /// <summary>
        /// �õ�һ������
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Comments GetComment(string id, string[] fields)
        {
            Comments c = new Comments();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<Comments> cList = Assistant.List<Comments>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                c = cList[0];
            }
            return c;
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="c"></param>
        public void AddComments(Comments c)
        {
            HttpContext Context = HttpContext.Current;
            HttpBrowserCapabilities bc = Context.Request.Browser;   
            
            c.Created = DateTime.Now;
            c.Updated = c.Created;
            c.ID = We7Helper.CreateNewID();
            c.IP = Context.Request.ServerVariables["REMOTE_ADDR"];
            Assistant.Insert(c);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="c"></param>
        /// <param name="fields"></param>
        public void UpdateComments(Comments c, string[] fields)
        {
            c.Updated = DateTime.Now;
            Assistant.Update(c, fields);
        }

        /// <summary>
        /// ȡ���û�������
        /// </summary>
        /// <param name="id">�û�ID</param>
        /// <param name="begin">��ʼ��¼</param>
        /// <param name="count">��ѯ������</param>
        /// <returns></returns>
        public List<Comments> GetAllComments(string id, int begin, int count)
        {
            if (id == "{00000000-0000-0000-0000-000000000000}")
            {
                return null;
            }
            Criteria c = new Criteria(CriteriaType.Equals, "AccountID", id);
           List<Article> articleList= Assistant.List<Article>(c,null);
           if (articleList != null)
           {
               Criteria subc = new Criteria(CriteriaType.None);
               subc.Mode = CriteriaMode.Or;
               foreach (Article article in articleList)
               { 
                   subc.AddOr(CriteriaType.Equals, "ArticleID", article.ID);
               }
               Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
               return Assistant.List<Comments>(subc, orders, begin, count);
           }
           else
           {
               return null;
           }
        }
    }
}
