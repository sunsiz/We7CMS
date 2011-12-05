using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Web;
using System.Xml;
using System.Data;
using System.Text.RegularExpressions;

using Thinkment.Data;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Util;
using System.Reflection;
using We7.CMS.Accounts;

namespace We7.CMS
{
    /// <summary>
    /// ���´�������
    /// </summary>
    [Serializable]
    [Helper("We7.ArticleHelper")]
    public partial class ArticleHelper : BaseHelper, IObjectClickHelper
    {


        #region �������������롢ɾ�������¡���ȡ

        /// <summary>
        /// ����������ȡ������
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int QueryArtilceCount(Criteria c)
        {
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        /// ����IDɾ��һƪ����
        /// </summary>
        /// <param name="id">����ID</param>
        public void DeleteArticle(string id)
        {
            //ɾ������
            Article a = new Article();
            a.ID = id;
            Assistant.Delete(a);
            //ɾ���������
            DeleteRelatedArticles(id);
            Criteria ch = new Criteria(CriteriaType.Equals, "ArticleID", id);
            //ɾ������

            //ɾ������
            List<Attachment> atts = Assistant.List<Attachment>(ch, null);
            foreach (Attachment att in atts)
            {
                string file = HttpContext.Current.Server.MapPath(att.FilePath + "/" + att.FileName);
                if (File.Exists(file))
                    File.Delete(file);
                Assistant.Delete(att);
            }
            //���wap���

            //ɾ�����±�ǩ

            //ɾ������
            List<Comments> coms = Assistant.List<Comments>(ch, null);
            foreach (Comments c in coms)
            {
                Assistant.Delete(c);
            }
        }

        /// <summary>
        /// ��ȡһƪ����
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="fields">���ص��ֶμ���</param>
        /// <returns></returns>
        public Article GetArticle(string id, string[] fields)
        {
            Article a = new Article();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Article> aList = Assistant.List<Article>(c, null, 0, 0, fields);
            if (aList != null && aList.Count > 0)
            {
                a = aList[0];
            }
            return a;
        }

        private static readonly string ArticleKeyID = "$ArticleID{0}";
        /// <summary>
        /// ��ȡһƪ���£�ʹ���˻��棩
        /// </summary>
        /// <param name="ArticleID">����ID</param>
        /// <returns></returns>
        public Article GetArticle(string ArticleID)
        {
            if (ArticleID != null && ArticleID != string.Empty)
            {
                HttpContext Context = HttpContext.Current;
                string key = string.Format(ArticleKeyID, ArticleID);
                Article article = (Article)Context.Items[key];//�ڴ�
                if (article == null)
                {
                    article = (Article)Context.Cache[key];//����
                    if (article == null)
                    {
                        if (ArticleID != null && ArticleID != string.Empty)
                        {
                            //��ȡ���ݿ�
                            Order[] o = new Order[] { new Order("ID") };
                            Criteria c = new Criteria(CriteriaType.Equals, "ID", ArticleID);
                            List<Article> articles = Assistant.List<Article>(c, o);
                            if (articles.Count > 0)
                            {
                                article = articles[0];
                            }
                        }

                        if (article != null)
                        {
                            CacherCache(key, Context, article, CacheTime.Short);
                        }
                    }

                    if (Context.Items[key] == null)
                    {
                        Context.Items.Remove(key);
                        Context.Items.Add(key, article);
                    }
                }
                return article;
            }
            else
                return null;

        }

        /// <summary>
        /// ͨ��ID��ȡ���±���
        /// </summary>
        /// <param name="id">����ID</param>
        /// <returns></returns>
        public string GetArticleName(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Article> articles = Assistant.List<Article>(c, null);
            if (articles.Count > 0)
                return articles[0].Title;
            else
                return "";
        }

        /// <summary>
        /// ͨ��SN��ȡ��ĿID
        /// </summary>
        /// <param name="sn">��ĿSN</param>
        /// <returns></returns>
        public string GetArticleIDBySN(string sn)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "SN", sn);
            List<Article> articles = Assistant.List<Article>(c, null);
            if (articles.Count > 0)
                return articles[0].ID;
            else
                return "";
        }

        /// <summary>
        /// ���һƪ����
        /// </summary>
        /// <param name="a">һƪ���¶���</param>
        /// <returns></returns>
        public Article AddArticles(Article a)
        {
            if (a.Created == DateTime.MinValue)
                a.Created = DateTime.Now;
            if (a.Updated == DateTime.MinValue)
                a.Updated = a.Created;
            if (string.IsNullOrEmpty(a.ID))
                a.ID = We7Helper.CreateNewID();
            a.SN = CreateArticleSN();
            a.Clicks = 0;
            a.CommentCount = 0;
            if (String.IsNullOrEmpty(a.ModelName))
            {
                a.ModelName = Constants.ArticleModelName;
            }
            Assistant.Insert(a);
            return a;
        }

        /// <summary>
        /// ���һƪ����
        /// </summary>
        /// <param name="a">һƪ���¶���</param>
        public void AddArticle(Article a)
        {
            if (a.Created == DateTime.MinValue)
                a.Created = DateTime.Now;
            if (a.Updated == DateTime.MinValue)
                a.Updated = a.Created;
            if (string.IsNullOrEmpty(a.ID))
                a.ID = We7Helper.CreateNewID();
            a.SN = CreateArticleSN();
            if (String.IsNullOrEmpty(a.ModelName))
            {
                a.ModelName = Constants.ArticleModelName;
            }
            a.Clicks = 0;
            a.CommentCount = 0;
            Assistant.Insert(a);
        }

        /// <summary>
        /// �������µ�sn
        /// </summary>
        /// <returns></returns>
        public long CreateArticleSN()
        {
            CreateSNHelper helper = new CreateSNHelper();
            helper.Assistant = Assistant;
            return helper.SnBase;
        }

        /// <summary>
        /// ����һƪ���¼�¼
        /// </summary>
        /// <param name="a">һƪ���¼�¼</param>
        /// <param name="fields">��Ҫ���µ��ֶ�</param>
        public void UpdateArticle(Article a, string[] fields)
        {
            //�������
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ArticleKeyID, a.ID);
            Context.Cache.Remove(key);
            Context.Items.Remove(key);

            //a.Updated = DateTime.Now;
            Assistant.Update(a, fields);
        }

        /// <summary>
        /// ����ĳ��վ�㹲�����������
        /// </summary>
        /// <param name="ownerID">��ĿID</param>
        /// <param name="sourceID">վ��ID</param>
        /// <returns></returns>
        public Article GetArticleBySource(string ownerID, string sourceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "SourceID", sourceID);
            if (ownerID != null)
                c.Add(CriteriaType.Equals, "OwnerID", ownerID);
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            List<Article> articles = Assistant.List<Article>(c, orders);
            if (articles.Count > 0)
                return articles[0];
            else
                return null;
        }

        /// <summary>
        /// ����һƪ���µ�Wap
        /// </summary>
        /// <param name="sourceArticle">������Դ</param>
        /// <returns></returns>
        public Article CopyToWapArticle(Article sourceArticle)
        {
            Article wap = new Article();
            wap.Content = We7Helper.RemoveHtml(sourceArticle.Content);
            wap.Description = sourceArticle.Description;
            wap.Title = sourceArticle.Title;
            wap.SourceID = sourceArticle.ID;
            wap.Index = sourceArticle.Index;
            wap.Source = sourceArticle.Source;
            wap.AllowComments = sourceArticle.AllowComments;
            wap.IsImage = sourceArticle.IsImage;
            wap.Author = sourceArticle.Author;
            wap.State = 0;
            wap.ContentType = Convert.ToInt32(TypeOfArticle.WapArticle);
            wap.Overdue = sourceArticle.Overdue;
            wap.Thumbnail = sourceArticle.Thumbnail;
            wap.Updated = sourceArticle.Updated;
            return wap;
        }

        /// <summary>
        /// ����һƪ���µ���ת״̬
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="ProcessState">��ת״̬</param>
        /// <param name="state">����״̬</param>
        public void UpdateArticleProcess(string id, string ProcessState, ArticleStates state)
        {
            Article a = GetArticle(id);
            a.ProcessState = ProcessState;
            a.State = (int)state;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "Updated", "ProcessState", "State" });
        }

        /// <summary>
        /// ͨ��Urlȡ�������б�
        /// </summary>
        /// <param name="url">������ѯ��Url</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��ѯ����</param>
        /// <param name="fields">��ѯ���ֶ�</param>
        /// <param name="OnlyInUser">�Ƿ���ֻ��ѯ��ǰ�û�������</param>
        /// <returns>�����б�</returns>
        public List<Article> GetArticlesByUrl(string url, int from, int count, string[] fields, bool OnlyInUser)
        {
            Criteria c = new Criteria(CriteriaType.Like, "ChannelFullUrl", url);
            if (OnlyInUser)
                c.Add(CriteriaType.Equals, "State", 1);

            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Article>(c, orders, from, count, fields);
        }

        #endregion

        #region tag

        /// <summary>
        /// ͨ����ǩ���ϻ�ȡ������Щ��ǩ������
        /// </summary>
        /// <param name="tags">��ǩ����</param>
        /// <returns>������</returns>
        public int QueryArticlesCountByTags(List<string> tags)
        {
            if (tags.Count > 0)
            {
                Criteria c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (string tag in tags)
                {
                    c.Add(CriteriaType.Like, "Tags", "'" + tag + "'");
                }

                return Assistant.Count<Article>(c);
            }
            else
                return 0;
        }


        /// <summary>
        /// ��ȡһ�����µ����б�ǩ����
        /// </summary>
        /// <param name="articleID">����ID</param>
        /// <returns></returns>
        public List<string> GetTags(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", articleID);

            List<Article> articles = Assistant.List<Article>(c, null);
            if (articles.Count > 0 && articles[0] != null)
            {
                string tags = articles[0].Tags.Replace("'", ",");
                List<string> list = new List<string>();
                if (tags.Length > 0)
                {
                    string[] temp = tags.Split(',');
                    if (temp.Length > 0)
                    {
                        foreach (string str in temp)
                        {
                            if (str.Trim().Length > 0 && !str.Equals(","))
                            {
                                list.Add(str);
                            }
                        }
                    }
                }
                return list;
            }
            else
                return null;
        }






        /// <summary>
        /// ��ȡһ�����±�ǩ��¼
        /// </summary>
        /// <param name="tag">���±�ǩ</param>
        /// <param name="articleID">����ID</param>
        /// <returns></returns>
        //public int GetTagCount(string tag, string articleID)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
        //    c.Criterias.Add(new Criteria(CriteriaType.Equals, "Identifier", tag));
        //    return Assistant.Count<ArticleTag>(c);
        //}
        


        #endregion

        #region �������

        /// <summary>
        /// ���һ���������
        /// </summary>
        /// <param name="at">������¼�¼</param>
        public void AddRelatedArticle(RelatedArticle at)
        {
            at.ID = Guid.NewGuid().ToString();
            Assistant.Insert(at);
        }

        /// <summary>
        /// ��ȡһ��ƪ���µ����������
        /// </summary>
        /// <param name="tag">�˲�������</param>
        /// <param name="articleID">����ID</param>
        /// <returns></returns>
        public int GetRelatedArticleCount(string tag, string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            return Assistant.Count<RelatedArticle>(c);
        }

        /// <summary>
        ///  ɾ��һƪ��������articleID�йص�������µ�һ����¼
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="relateID">�������ID</param>
        public void DeleteRelatedArticle(string id, string relateID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", id);
            c.Add(CriteriaType.Equals, "RelatedID", relateID);
            List<RelatedArticle> ras = Assistant.List<RelatedArticle>(c, null);
            if (ras.Count > 0)
            {
                Assistant.Delete(ras[0]);
            }
        }

        /// <summary>
        /// ɾ��������articleID�йص�����������¼�¼
        /// </summary>
        /// <param name="articleID">����ID</param>
        public void DeleteRelatedArticles(string articleID)
        {
            //��ɾ��ArticleID=articleID
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            List<RelatedArticle> ras = Assistant.List<RelatedArticle>(c, null);
            foreach (RelatedArticle ra in ras)
            {
                Assistant.Delete(ra);
            }
            //��ɾ��RelatedID=articleID
            c = new Criteria(CriteriaType.Equals, "RelatedID", articleID);
            ras = Assistant.List<RelatedArticle>(c, null);
            foreach (RelatedArticle ra in ras)
            {
                Assistant.Delete(ra);
            }
        }

        /// <summary>
        /// ��ȡһƪ���µ�������¼�¼
        /// </summary>
        /// <param name="articleID">����ID</param>
        /// <returns></returns>
        public List<Article> GetRelatedArticles(string articleID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ArticleID", articleID);
            List<RelatedArticle> ts = Assistant.List<RelatedArticle>(c, null);
            List<Article> articles = new List<Article>();
            List<string> ids = new List<string>();
            foreach (RelatedArticle ra in ts)
            {
                if (!ids.Contains(ra.RelatedID))
                {
                    Article a = GetArticle(ra.RelatedID, null);
                    if (a != null)
                    {
                        articles.Add(a);
                        ids.Add(ra.RelatedID);
                    }
                }
            }
            return articles;
        }

        # endregion

        #region IP����
        /// <summary>
        /// ���������µĲ���
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="stragegy">������Ϣ</param>
        /// <returns></returns>
        public void UpdateStrategy(string id, string strategy)
        {
            Article article = GetArticle(id);

            if (article == null)
                return;

            article.IPStrategy = strategy;
            Assistant.Update(article, new string[] { "IPStrategy" });
        }

        /// <summary>
        /// ��ѯ���µİ�ȫ����
        /// </summary>
        /// <param name="id">����ID</param>
        /// <returns></returns>
        public string QueryStrategy(string id)
        {
            Article article = GetArticle(id);
            return article != null ? article.IPStrategy : String.Empty;
        }

        #endregion

        #region ���²�ѯ������ArticleQuery�ĸ��Ӳ�ѯ

        /// <summary>
        /// ���ݲ�ѯ������Criteria
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <returns>��ѯ����</returns>
        Criteria CreateCriteriaByAll(ArticleQuery query)
        {
            string parameters;
            string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);

            Criteria c = new Criteria(CriteriaType.None);
            //if (String.IsNullOrEmpty(modelname) && query.EnumState != null && query.EnumState != "")
            //{
            //    Criteria csubC = new Criteria();
            //    csubC.Name = "EnumState";
            //    csubC.Value = query.EnumState;
            //    if (query.ExcludeThisChannel)
            //        csubC.Type = CriteriaType.NotEquals;
            //    else
            //        csubC.Type = CriteriaType.Equals;

            //    csubC.Adorn = Adorns.Substring;
            //    csubC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ArticleType];
            //    csubC.Length = EnumLibrary.PlaceLenth;
            //    c.Criterias.Add(csubC);
            //}
            if (String.IsNullOrEmpty(query.ModelName) || Constants.ArticleModelName.Equals(query.ModelName, StringComparison.OrdinalIgnoreCase))
            {
                AppendModelCondition(c);
            }

            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);

            if (query.ArticleType > 0)
                c.Add(CriteriaType.Equals, "ContentType", query.ArticleType);
            else
                c.Add(CriteriaType.NotEquals, "ContentType", 16);//��ȥwap����

            if (query.KeyWord != null && query.KeyWord.Length > 0)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "Title", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "Description", "%" + query.KeyWord + "%");
                c.Criterias.Add(keyCriteria);
            }

            if (query.Author != null && query.Author.Length > 0)
            {
                c.Add(CriteriaType.Like, "Author", "%" + query.Author + "%");
            }

            if (query.BeginDate <= query.EndDate)
            {
                if (query.BeginDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.BeginDate);
                if (query.EndDate != DateTime.MinValue && query.EndDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.EndDate.AddDays(1));
            }
            else
            {
                if (query.EndDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.EndDate);
                if (query.BeginDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.BeginDate.AddDays(1));
            }

            if (!We7Helper.IsEmptyID(query.ChannelID))
            {
                if (CheckModel(modelname))
                {
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (!String.IsNullOrEmpty(parameters))
                    {
                        CriteriaExpressionHelper.Execute(c, parameters);
                    }
                }
                else
                {
                    if (query.ExcludeThisChannel)
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.NotLike, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                            c.Add(CriteriaType.NotEquals, "OwnerID", query.ChannelID);
                    }
                    else
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.Like, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                        {
                            if (query.ChannelID.Contains(","))
                            {
                                string[] oids = query.ChannelID.Split(',');
                                Criteria subC = new Criteria(CriteriaType.None);
                                subC.Mode = CriteriaMode.Or;
                                foreach (string s in oids)
                                {
                                    subC.Add(CriteriaType.Equals, "OwnerID", s);
                                }
                                c.Criterias.Add(subC);
                            }
                            else
                            {
                                c.Add(CriteriaType.Equals, "OwnerID", query.ChannelID);
                            }
                        }
                    }
                }
            }

            if (!We7Helper.IsEmptyID(query.AccountID))
            {
                Channel channel = ChannelHelper.GetChannel(query.ChannelID, null);

                if (query.IncludeAdministrable)
                {
                    List<string> channels = AccountHelper.GetObjectsByPermission(query.AccountID, "Channel.Article");

                    Criteria keyCriteria = new Criteria(CriteriaType.None);
                    if (channels != null && channels.Count > 0)
                    {
                        keyCriteria.Mode = CriteriaMode.Or;
                        foreach (string ownerID in channels)
                        {
                            keyCriteria.AddOr(CriteriaType.Equals, "OwnerID", ownerID);
                        }
                    }

                    keyCriteria.AddOr(CriteriaType.Equals, "AccountID", query.AccountID);

                    if (keyCriteria.Criterias.Count > 0)
                    {
                        c.Criterias.Add(keyCriteria);
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "AccountID", query.AccountID);
            }

            if (query.IsShowHome != null && query.IsShowHome == "1")
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (query.Tag != null && query.Tag != "")
            {

                c.Add(CriteriaType.Like, "Tags", "%" + query.Tag + "%");
            }
            if (query.IsImage != null && query.IsImage == "1")
            {
                c.Add(CriteriaType.Equals, "IsImage", 1);
            }
            if (query.Overdue)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                Criteria subChildC1 = new Criteria(CriteriaType.MoreThanEquals, "Overdue", DateTime.Now);
                subC.Criterias.Add(subChildC1);
                //Criteria subChildC2 = new Criteria(CriteriaType.Equals, "Overdue", DateTime.MinValue);
                //subC.Criterias.Add(subChildC2);
                Criteria subChildC3 = new Criteria(CriteriaType.LessThanEquals, "Overdue", DateTime.Today.AddYears(-30));
                subC.Criterias.Add(subChildC3);
                c.Criterias.Add(subC);
            }

            if (!string.IsNullOrEmpty(query.ArticleParentID))
            {
                c.Add(CriteriaType.Equals, "ParentID", query.ArticleParentID);
            }

            //if (query.SearcherKey != null && query.SearcherKey != "")
            //{
            //    c.Add(CriteriaType.Like, "Title", "%" + query.SearcherKey + "%");
            //}

            if (query.ArticleID != null && query.ArticleID != "")
            {
                c.Add(CriteriaType.Like, "ListKeys", "%" + query.ArticleID + "%");
            }
            return c;
        }

        /// <summary>
        /// ���Ƿ�������ģ�͵ķ�ʽ��ѯ��Ϣ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <returns>��ѯ����</returns>
        Criteria CreateCriteriaByModel(ArticleQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);

            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);

            if (query.ArticleType > 0)
                c.Add(CriteriaType.Equals, "ContentType", query.ArticleType);
            else
                c.Add(CriteriaType.NotEquals, "ContentType", 16);//��ȥwap����

            if (query.KeyWord != null && query.KeyWord.Length > 0)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "Title", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "Description", "%" + query.KeyWord + "%");
                c.Criterias.Add(keyCriteria);
            }

            if (query.Author != null && query.Author.Length > 0)
            {
                c.Add(CriteriaType.Like, "Author", "%" + query.Author + "%");
            }

            if (query.BeginDate <= query.EndDate)
            {
                if (query.BeginDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.BeginDate);
                if (query.EndDate != DateTime.MinValue && query.EndDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.EndDate.AddDays(1));
            }
            else
            {
                if (query.EndDate != DateTime.MinValue)
                    c.Add(CriteriaType.MoreThanEquals, "Updated", query.EndDate);
                if (query.BeginDate != DateTime.MaxValue)
                    c.Add(CriteriaType.LessThanEquals, "Updated", query.BeginDate.AddDays(1));
            }



            if (query.UseModel)
            {
                if (!We7Helper.IsEmptyID(query.ChannelID) && (String.IsNullOrEmpty(query.ModelName) || String.IsNullOrEmpty(query.ModelName.Trim())))
                {
                    string parameters;
                    string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (!String.IsNullOrEmpty(parameters))
                    {
                        CriteriaExpressionHelper.Execute(c, parameters);
                    }
                }
                else
                {
                    c.Add(CriteriaType.Equals, "ModelName", query.ModelName);
                }
            }
            else
            {
                if (!We7Helper.IsEmptyID(query.ChannelID))
                {
                    string parameters;
                    string modelname = ChannelHelper.GetModelName(query.ChannelID, out parameters);
                    c.Add(CriteriaType.Equals, "ModelName", modelname);
                    if (query.ExcludeThisChannel)
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.NotLike, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                            c.Add(CriteriaType.NotEquals, "OwnerID", query.ChannelID);
                    }
                    else
                    {
                        if (query.IncludeAllSons)
                        {
                            c.Add(CriteriaType.Like, "ChannelFullUrl", query.ChannelFullUrl + "%");
                        }
                        else
                        {
                            if (query.ChannelID.Contains(","))
                            {
                                string[] oids = query.ChannelID.Split(',');
                                Criteria subC = new Criteria(CriteriaType.None);
                                subC.Mode = CriteriaMode.Or;
                                foreach (string s in oids)
                                {
                                    subC.Add(CriteriaType.Equals, "OwnerID", s);
                                }
                                c.Criterias.Add(subC);
                            }
                            else
                            {
                                c.Add(CriteriaType.Equals, "OwnerID", query.ChannelID);
                            }
                        }
                    }
                }
            }

            if (!We7Helper.IsEmptyID(query.AccountID))
            {
                Channel channel = ChannelHelper.GetChannel(query.ChannelID, null);

                if (query.IncludeAdministrable)
                {
                    List<string> channels = AccountHelper.GetObjectsByPermission(query.AccountID, "Channel.Article");

                    Criteria keyCriteria = new Criteria(CriteriaType.None);
                    if (channels != null && channels.Count > 0)
                    {
                        keyCriteria.Mode = CriteriaMode.Or;
                        foreach (string ownerID in channels)
                        {
                            keyCriteria.AddOr(CriteriaType.Equals, "OwnerID", ownerID);
                        }
                    }

                    keyCriteria.AddOr(CriteriaType.Equals, "AccountID", query.AccountID);

                    if (keyCriteria.Criterias.Count > 0)
                    {
                        c.Criterias.Add(keyCriteria);
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "AccountID", query.AccountID);
            }

            if (query.IsShowHome != null && query.IsShowHome == "1")
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (query.Tag != null && query.Tag != "")
            {

                c.Add(CriteriaType.Like, "Tags", "%" + query.Tag + "%");
            }
            if (query.IsImage != null && query.IsImage == "1")
            {
                c.Add(CriteriaType.Equals, "IsImage", 1);
            }
            if (query.Overdue)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                Criteria subChildC1 = new Criteria(CriteriaType.MoreThanEquals, "Overdue", DateTime.Now);
                subC.Criterias.Add(subChildC1);
                //Criteria subChildC2 = new Criteria(CriteriaType.Equals, "Overdue", DateTime.MinValue);
                //subC.Criterias.Add(subChildC2);
                Criteria subChildC3 = new Criteria(CriteriaType.LessThanEquals, "Overdue", DateTime.Today.AddYears(-30));
                subC.Criterias.Add(subChildC3);
                c.Criterias.Add(subC);
            }

            //if (query.SearcherKey != null && query.SearcherKey != "")
            //{
            //    c.Add(CriteriaType.Like, "Title", "%" + query.SearcherKey + "%");
            //}

            if (query.ArticleID != null && query.ArticleID != "")
            {
                c.Add(CriteriaType.Like, "ListKeys", "%" + query.ArticleID + "%");
            }
            return c;
        }


        /// <summary>
        /// ���ݲ�ѯ������Criteria
        /// </summary>
        /// <param name="queryParam">��������Hashtable��ֵ��</param>
        /// <returns></returns>
        Criteria CreateCriteriaByParameter(Hashtable queryParam)
        {
            Criteria c = new Criteria(CriteriaType.None);
            foreach (DictionaryEntry de in queryParam)
            {
                c.Add(CriteriaType.Equals, de.Key.ToString(), de.Value);
            }
            return c;
        }

        /// <summary>
        /// ���ݲ�ѯ������������
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilceCountByAll(ArticleQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        /// ���ݲ�ѯ������������(��������ģ����Ϣ)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilceModelCountByAll(ArticleQuery query)
        {
            Criteria c = CreateCriteriaByModel(query);
            return Assistant.Count<Article>(c);
        }


        /// <summary>
        /// ���ݲ�ѯ���ȡ�����б���ҳ��
        /// </summary>
        /// <param name="query">��ѯ��</param>
        /// <param name="from">�ڼ�����ʼ</param>
        /// <param name="count">��ȡ����</param>
        /// <param name="fields">string[]�ֶ��б�nullΪȫ��</param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByAll(ArticleQuery query, int from, int count, string[] fields)
        {
            try
            {
                Criteria c = CreateCriteriaByAll(query);
                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = null;
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Article>(c, o, from, count, fields);
            }
            catch (Exception ex)
            {
            }
            return new List<Article>();
        }

        /// <summary>
        /// ���ݲ�ѯ���ȡ�����б���ҳ��
        /// </summary>
        /// <param name="query">��ѯ��</param>
        /// <param name="from">�ڼ�����ʼ</param>
        /// <param name="count">��ȡ����</param>
        /// <param name="fields">string[]�ֶ��б�nullΪȫ��</param>
        /// <returns></returns>
        public List<Article> QueryArtilceModelByAll(ArticleQuery query, int from, int count, string[] fields)
        {
            Criteria c = CreateCriteriaByModel(query);
            List<Order> orders = CreateOrdersByAll(query.OrderKeys);
            Order[] o = null;
            if (orders != null) o = orders.ToArray();

            return Assistant.List<Article>(c, o, from, count, fields);
        }



        /// <summary>
        /// ���ݲ�ѯʵ��õ���ѯ��������
        /// </summary>
        /// <param name="queryEntity">��ѯ����ʵ��</param>
        /// <returns>��ѯ�������� Criteria</returns>
        private Criteria GetCriteriaByQueryEntity(QueryEntity queryEntity)
        {
            Criteria c = null;
            if (queryEntity != null)
            {
                c = new Criteria(CriteriaType.Equals, "ModelName", queryEntity.ModelName);
                List<QueryParam> queryPanamList = queryEntity.QueryParams;
                for (int i = 0; i < queryPanamList.Count; i++)
                {
                    QueryParam qp = queryPanamList[i];
                    if (qp.CriteriaType == CriteriaType.Like)
                        qp.ColumnValue = String.Format("%{0}%", qp.ColumnValue);
                    c.Add(qp.CriteriaType, qp.ColumnKey, qp.ColumnValue);
                }
            }

            return c;
        }

        /// <summary>
        /// ����ģ�Ͳ�ѯ����
        /// </summary>
        /// <returns></returns>
        public static Criteria CreateModelCondition()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            c.AddOr(CriteriaType.Equals, "ModelName", Constants.ArticleModelName);
            c.AddOr(CriteriaType.Equals, "ModelName", String.Empty);
            c.AddOr(CriteriaType.IsNull, "ModelName", null);
            return c;
        }

        /// <summary>
        /// Ϊ��ѯ����׷��ģ�Ͳ���
        /// </summary>
        /// <param name="c"></param>
        public static void AppendModelCondition(Criteria c)
        {
            if (c == null)
            {
                throw new ArgumentNullException("��������Ϊ��");
            }
            c.Criterias.Add(CreateModelCondition());
        }

        #region ���²�ѯ��¼
        /*
        /// <summary>
        /// ���ݲ�ѯ������������
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryArtilceCountByAll(ArticleQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        /// ���ݲ�ѯ���ȡ�����б���ҳ��
        /// </summary>
        /// <param name="query">��ѯ��</param>
        /// <param name="from">�ڼ�����ʼ</param>
        /// <param name="count">��ȡ����</param>
        /// <param name="fields">string[]�ֶ��б�nullΪȫ��</param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByAll(ArticleQuery query, int from, int count, string[] fields)
        {
            Criteria c = CreateCriteriaByAll(query);
            List<Order> orders = CreateOrdersByAll(query.OrderKeys);
            Order[] o = null;
            if (orders != null) o = orders.ToArray();

            return Assistant.List<Article>(c, o, from, count, fields);
        }
        */

        #endregion

        /// <summary>
        /// ����Ŀ��������
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="includechildren"></param>
        /// <returns></returns>
        public List<Article> QueryArticlesByChannel(string cid, bool includechildren)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (includechildren)
            {
                Channel ch = ChannelHelper.GetChannel(cid, null);
                c.Add(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl + "%");
            }
            else
            {
                c.Add(CriteriaType.Equals, "OwnerID", cid);
            }

            return Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
        }

        /// <summary>
        /// ����Ŀ��������(��ҳ)
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="includechildren"></param>
        /// <param name="from"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public List<Article> QueryArticlesByChannel(string cid, bool includechildren, int from, int PageSize)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (includechildren)
            {
                Channel ch = ChannelHelper.GetChannel(cid, null);
                c.Add(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl + "%");
                c.Add(CriteriaType.Equals, "State", 1);
            }
            else
            {
                c.Add(CriteriaType.Equals, "OwnerID", cid);
                c.Add(CriteriaType.Equals, "State", 1);
            }

            return Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) }, from, PageSize);
        }

        public List<Article> QueryModelRecordByChannelID(string cid, int start, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };

            return Assistant.List<Article>(c, orders, start, count);
        }

        public int QueryModelRecordCountByChannelID(string cid)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            return Assistant.Count<Article>(c);
        }

        public List<Article> QueryModelRecordByChannelID2(string cid, int start, int count)
        {

            //Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            Criteria c = new Criteria(CriteriaType.None);
            ExtendCriteria(c, cid);
            Order[] orders = new Order[] { new Order("IsShow", OrderMode.Desc), new Order("Updated", OrderMode.Desc) };

            return Assistant.List<Article>(c, orders, start, count);
        }

        public int QueryModelRecordCountByChannelID2(string cid)
        {
            //Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", cid);
            Criteria c = new Criteria(CriteriaType.None);
            ExtendCriteria(c, cid);
            return Assistant.Count<Article>(c);
        }

        /// <summary>
        ///  ���ݲ�ѯ���ȡ�����б�
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Article> QueryArtilcesByAll(ArticleQuery query)
        {
            try
            {
                Criteria c = CreateCriteriaByAll(query);

                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = null;
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Article>(c, o);
            }
            catch (Exception ex)
            {
            }
            return new List<Article>();
        }

        /// <summary>
        /// ��ȡĳ��ʱ���ڷ����������
        /// </summary>
        /// <param name="begin">�ӵڼ�����¼��ʼ</param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetArticleCountByTime(DateTime begin, DateTime end)
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "Created", begin);
            c.Add(CriteriaType.LessThanEquals, "Created", end);
            if (Assistant.Count<Article>(c) > 0)
            {
                return Assistant.Count<Article>(c);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// ��ģ�Ͳ�ѯ����
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public List<Article> QueryArticleByModel(string modelName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ModelName", modelName);
            return Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
        }

        #endregion

        #region ���ⷽ��
        /// <summary>
        /// ���������ļ���ˮӡ��ͼƬ
        /// </summary>
        /// <param name="ImageConfig">ͼƬ������</param>
        /// <param name="thumbnailFile">��ˮӡ���ļ�</param>
        /// <param name="originalFilePath">ԭ�ļ�</param>
        public static void AddWatermarkToImage(GeneralConfigInfo ImageConfig, string thumbnailFile, string originalFilePath)
        {
            if (ImageConfig.WaterMarkStatus != 0)
            {
                string waterparkedFile = ImageUtils.GenerateWatermarkFile(originalFilePath);
                System.Drawing.Image img = System.Drawing.Image.FromFile(thumbnailFile);
                System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                img.Dispose();
                img = null;

                if (ImageConfig.WaterMarkType == 1 && File.Exists(We7Utils.GetMapPath(ImageConfig.WaterMarkPicfile)))
                {
                    ImageUtils.AddImageSignPic(bmp, waterparkedFile, We7Utils.GetMapPath(ImageConfig.WaterMarkPicfile), ImageConfig.WaterMarkStatus, ImageConfig.AttachImageQuality, ImageConfig.WaterMarkTransparency);
                }
                else
                {
                    string watermarkText = ImageConfig.WaterMarkText;
                    //watermarkText = ImageConfig.Watermarktext.Replace("{1}", ImageConfig.Forumtitle);
                    //watermarkText = watermarkText.Replace("{2}", "http://" + DNTRequest.GetCurrentFullHost() + "/");
                    //watermarkText = watermarkText.Replace("{3}", Utils.GetDate());
                    //watermarkText = watermarkText.Replace("{4}", Utils.GetTime());
                    ImageUtils.AddImageSignText(bmp, waterparkedFile, watermarkText, ImageConfig.WaterMarkStatus, ImageConfig.AttachImageQuality, ImageConfig.WaterMarkFontName, ImageConfig.WaterMarkFontSize);
                }

                bmp.Dispose();
                bmp = null;

                if (File.Exists(waterparkedFile))
                {
                    System.IO.File.Delete(thumbnailFile);
                    System.IO.File.Copy(waterparkedFile, thumbnailFile);
                    System.IO.File.Delete(waterparkedFile);
                }
            }
        }
        /// <summary>
        /// ������д��Config�ļ�
        /// </summary>
        /// <param name="path"></param>
        public void Write(string path)
        {
            string str = "<?xml version=\"" + "1.0\"?>" +
"<GeneralConfigInfo xmlns:xsi=\"" + "http://www.w3.org/2001/XMLSchema-instance\"" + " xmlns:xsd=\"" + "http://www.w3.org/2001/XMLSchema\"" + ">" +
"<SiteTitle>We7</SiteTitle>" +
"<IcpInfo />" +
"<RewriteUrl />" +
"<UrlExtName>.aspx</UrlExtName>" +
"<PostInterval>0</PostInterval>" +
"<WaterMarkStatus>3</WaterMarkStatus>" +
"<WaterMarkType>0</WaterMarkType>" +
"<WaterMarkTransparency>5</WaterMarkTransparency>" +
"<WaterMarkText>We7.cn</WaterMarkText>" +
"<WaterMarkPic>watermark.gif</WaterMarkPic>" +
"<WaterMarkFontName>Tahoma</WaterMarkFontName>" +
"<WaterMarkFontSize>12</WaterMarkFontSize>" +
"<AttachImageQuality>80</AttachImageQuality>" +
"<OverdueDateTime>365</OverdueDateTime>" +
"<ADVisbleToSite>0</ADVisbleToSite>" +
"</GeneralConfigInfo>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            doc.Save(path);
        }

        #endregion

        #region ��̬URL
        private static readonly string ArticleKeyName = "$ArticleID{0}";

        /// <summary>
        /// ͨ��URLȡ������ID��
        /// </summary>
        /// <returns></returns>
        public string GetArticleIDFromURL()
        {
            HttpContext Context = HttpContext.Current;
            if (Context.Request["aid"] != null)
                return Context.Request["aid"];
            else
            {
                return GetArticleIDFromURL(Context.Request.RawUrl);
            }
        }

        /// <summary>
        /// ͨ��URLȡ������ID��
        /// </summary>
        /// <returns></returns>
        public string GetArticleIDFromURL(string url)
        {
            string id = GetArticleIDOrSNFromUrl(url);
            if (We7Helper.IsNumber(id))
                return GetArticleIDBySN(id);
            return id;
        }

        /// <summary>
        /// ��url��ȡ����id����SN
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetArticleIDOrSNFromUrl(string path)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("article=") > -1)
                    path = path.Substring(path.ToLower().IndexOf("article=") + 8);
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            string mathstr = @"/(\w|\s|(-)|(_))+\." + ext + "$";
            if (path.ToLower().EndsWith("default." + ext))
                path = path.Remove(path.Length - 12);
            if (path.ToLower().EndsWith("index." + ext))
                path = path.Remove(path.Length - 10);

            if (Regex.IsMatch(path, mathstr))
            {
                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                int lastDot = path.LastIndexOf(".");
                if (lastDot > -1)
                {
                    path = path.Remove(lastDot, path.Length - lastDot);
                }

                if (We7Helper.IsGUID(We7Helper.FormatToGUID(path)))
                    path = We7Helper.FormatToGUID(path);
                else
                {
                    int lastSub = path.LastIndexOf("-");
                    if (lastSub > -1)
                    {
                        path = path.Remove(0, lastSub + 1);
                    }

                    if (!We7Helper.IsNumber(path))
                        path = "";
                }

                return path;
            }
            else
                return string.Empty;

        }


        #endregion

        #region ���������ֶΣ�����Helper����
        /// <summary>
        /// Http������
        /// </summary>
        HttpContext context
        { get { return HttpContext.Current; } }
        /// <summary>
        /// ҵ���󹤳�
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)context.Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// ͳ��ҵ�����
        /// </summary>
        protected StatisticsHelper StatisticsHelper
        {
            get { return HelperFactory.GetHelper<StatisticsHelper>(); }
        }

        /// <summary>
        /// ����ҵ�����
        /// </summary>
        protected CommentsHelper CommentsHelper
        {
            get { return HelperFactory.GetHelper<CommentsHelper>(); }
        }
        /// <summary>
        /// ��Ŀҵ�����
        /// </summary>
        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        /// <summary>
        /// Ȩ��ҵ�����
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        /// <summary>
        /// ������ʱ���ȡ������Ϣ��¼
        /// </summary>
        /// <returns></returns>
        public List<Article> GetAllArticle()
        {
            Order[] o = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Article>(null, o);
        }

        /// <summary>
        /// ���µ����
        /// </summary>
        /// <returns></returns>
        public int UpdateClicks()
        {
            int count = 0;
            List<Article> allList = GetAllArticle();
            if (allList != null)
            {
                foreach (Article article in allList)
                {
                    article.Clicks = StatisticsHelper.GetArticleStatisticsCount(article.ID);
                    Assistant.Update(article, new string[] { "Clicks" });
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// ��������ͳ����
        /// </summary>
        /// <returns></returns>
        public int UpdateCommentCount()
        {
            int count = 0;
            List<Article> allList = GetAllArticle();
            if (allList != null)
            {
                foreach (Article article in allList)
                {
                    article.CommentCount = CommentsHelper.ArticleIDQueryCommentsCount(article.ID, true);
                    Assistant.Update(article, new string[] { "CommentCount" });
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// ���Ұ�������Ŀ����
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <param name="IncludeAllSons"></param>
        /// <returns></returns>
        Criteria CreateCriteriaBySubChannelList(string ChannelID, bool IncludeAllSons)
        {
            Criteria subCriteria = new Criteria(CriteriaType.None);
            //������������Ŀ������
            if (IncludeAllSons)
            {
                Channel channel = ChannelHelper.GetChannel(ChannelID, new string[] { "FullUrl" });
                if (channel != null)
                    subCriteria.AddOr(CriteriaType.Like, "ChannelFullUrl", "%" + channel.FullUrl + "%");
                else
                    subCriteria.AddOr(CriteriaType.Equals, "OwnerID", "-1");
            }
            else
                subCriteria.Add(CriteriaType.Equals, "OwnerID", ChannelID);

            return subCriteria;
        }

        /// <summary>
        /// �������±�ǩ
        /// 2011-11-9 ��ǩ�Ѿ����ϣ�δ���ִ˷���������
        /// 
        /// </summary>
        /// <returns></returns>
        //public int UpdateArticleTags()
        //{
        //    int count = 0;
        //    List<Article> allList = GetAllArticle();
        //    if (allList != null)
        //    {
        //        foreach (Article article in allList)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //           
        //            //List<ArticleTag> allTagsByArticleID = GetTags(article.ID);
        //            //if (allTagsByArticleID != null)
        //            //{
        //            //    int i = 0;
        //            //    foreach (ArticleTag articleTag in allTagsByArticleID)
        //            //    {
        //            //        sb.Append(articleTag.Identifier);
        //            //        if (allTagsByArticleID.Count > i + 1)
        //            //            sb.Append(",");
        //            //        i++;
        //            //    }
        //            //}
        //            //if (sb.ToString() != "")
        //            //{
        //            //    article.Tags = sb.ToString();
        //            //    Assistant.Update(article, new string[] { "Tags" });
        //            //    count++;
        //            //}
        //        }
        //    }
        //    return count;
        //}

        /// <summary>
        /// ����������ĿChannelFullUrl��������EnumStateΪ�վͳ�ʼ�������£���Ʒ�ȹ����ѹ�������״̬��ɹ���״̬��Ĭ�ϵĹ���ʱ��Ϊ100��
        /// </summary>
        /// <returns></returns>
        public int UpdateOtherFieldArticle()
        {
            int count = 0;
            List<Article> allList = GetAllArticle();
            if (allList != null)
            {
                foreach (Article article in allList)
                {
                    Channel ch = null;
                    if (String.IsNullOrEmpty(article.OwnerID) && !String.IsNullOrEmpty(article.ModelName))
                    {
                        ch = ChannelHelper.FirestByModelName(article.ModelName);
                    }
                    else
                    {
                        ch = ChannelHelper.GetChannel(article.OwnerID, new string[] { "FullUrl", "FullPath", "ChannelName" });
                    }
                    if (ch != null)
                    {
                        article.ChannelFullUrl = ch.FullUrl;
                        article.FullChannelPath = ch.FullPath;
                        article.ChannelName = ch.Name;
                        List<string> listString = new List<string>();
                        listString.Add("ChannelFullUrl");
                        listString.Add("FullChannelPath");
                        listString.Add("ChannelName");
                        string[] updateString = listString.ToArray();
                        Assistant.Update(article, updateString);
                        count++;
                    }
                }
            }
            return count;
        }
        /// <summary>
        ///��ǩ��ѯ
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼��</param>
        /// <param name="fields">���ص��ֶμ���</param>
        /// <returns></returns>
        public List<Article> QueryArticlesByTags(List<string> tags, int from, int count, string[] fields)
        {
            if (tags.Count > 0)
            {
                Criteria c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (string tag in tags)
                {
                    c.Add(CriteriaType.Like, "Tags", "%'" + tag + "'%");
                }

                Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
                return Assistant.List<Article>(c, orders, from, count, fields);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ȡ���ҵ������Ŀ
        /// </summary>
        /// <param name="state"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public List<Article> GetArticlesByState(ArticleStates state, string accountID, int from, int count)
        {
            List<Article> list = new List<Article>();
            Criteria c = new Criteria(CriteriaType.Equals, "State", (int)state);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "AccountID", accountID));
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            list = Assistant.List<Article>(c, orders, from, count);
            return list;
        }
        #endregion


        #region ������ֲ�����ݲɼ����������������

        /// <summary>
        /// ������ֲ֮���ԭ�����ݽ�������
        /// ���ã�ͣ�ã�������У����ڣ�����վ�����ݲ���Ӧ��д������������
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state">��Ϣ״̬��1�����ã�0�����ã�ͣ�ã���2������У�3�����ڣ�4������վ</param>
        /// <returns></returns>
        public List<string> GetDataBystate(int state)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "State", state);
            List<Article> articleList = Assistant.List<Article>(c, null);
            List<string> stringList = new List<string>();
            foreach (Article a in articleList)
            {
                stringList.Add(a.ID);
            }
            return stringList;
        }

        /// <summary>
        /// �������ݲɼ��������ݽ�����������SN
        /// </summary>
        /// <returns></returns>
        public int UpdateSN()
        {
            int count = 0;
            long sn = 0;
            Criteria c = new Criteria(CriteriaType.Equals, "SN", 0);
            List<Article> articleList = Assistant.List<Article>(c, null);
            if (articleList != null && articleList.Count > 0)
            {
                foreach (Article a in articleList)
                {
                    if (count == 0)
                    {
                        sn = CreateArticleSN();
                    }
                    a.SN = sn;
                    UpdateArticle(a, new string[] { "SN" });
                    sn++;
                    count++;
                }
            }
            return count;
        }

        #endregion

        #region

        bool CheckModel(string modelname)
        {
            if (!String.IsNullOrEmpty(modelname))
            {
                modelname = modelname.ToLower();
                List<string> list = new List<string>() { "article", "system.article" };
                return !list.Contains(modelname.ToLower());
            }
            return false;
        }

        void ExtendCriteria(Criteria c, string oid)
        {
            string parameters, modelname;
            modelname = ChannelHelper.GetModelName(oid, out parameters);
            if (CheckModel(modelname))
            {
                c.Add(CriteriaType.Equals, "ModelName", modelname);
                if (!String.IsNullOrEmpty(parameters))
                {
                    CriteriaExpressionHelper.Execute(c, parameters);
                }
            }
        }
        #endregion

        #region ���ݲ�ѯʵ��õ�Artilce
        /// <summary>
        /// ������ѯ�õ�����������
        /// </summary>
        /// <param name="queryEntity">��ѯʵ��</param>
        /// <returns>����������</returns>
        public int QueryArtilceModelCountByParameter(QueryEntity queryEntity)
        {
            if (queryEntity != null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ModelName", queryEntity.ModelName);
                List<QueryParam> queryPanamList = queryEntity.QueryParams;
                for (int i = 0; i < queryPanamList.Count; i++)
                {
                    QueryParam qp = queryPanamList[i];
                    if (qp.CriteriaType == CriteriaType.Like)
                    {
                        qp.ColumnValue = string.Format("%{0}%", qp.ColumnValue);
                    }
                    c.Add(qp.CriteriaType, qp.ColumnKey, qp.ColumnValue);
                }
                return Assistant.Count<Article>(c);
            }
            return 0;
        }

        /// <summary>
        /// ��ѯ���¼���
        /// </summary>
        /// <param name="queryEntity">��ѯ����ʵ��</param>
        /// <param name="orders">�������ʵ������</param>
        /// <param name="from">��ʼ����</param>
        /// <param name="count">ÿҳ����</param>
        /// <param name="fields">�ڵ�����</param>
        /// <returns>����ʵ�巺�ͼ���</returns>
        public List<Article> QueryArticles(QueryEntity queryEntity, int from, int count, string[] fields)
        {

            if (queryEntity != null)
            {
                Criteria c = GetCriteriaByQueryEntity(queryEntity);
                return Assistant.List<Article>(c, queryEntity.Orders, from, count, fields);


            }
            else
            {
                return new List<Article>();
            }
        }

        #endregion

        #region IObjectClickHelperʵ��
        /// <summary>
        /// ����ָ������ĵ��������
        /// </summary>
        /// <param name="modelName">ģ������</param>
        /// <param name="id">����ID</param>
        /// <param name="dictClickReport">���������</param>
        public void UpdateClicks(string modelName, string id, Dictionary<string, int> dictClickReport)
        {
            ArticleHelper helper = HelperFactory.GetHelper<ArticleHelper>();
            Article targetObject = helper.GetArticle(id);
            if (targetObject != null)
            {
                targetObject.DayClicks = dictClickReport["�յ����"];
                targetObject.YesterdayClicks = dictClickReport["���յ����"];
                targetObject.WeekClicks = dictClickReport["�ܵ����"];
                targetObject.MonthClicks = dictClickReport["�µ����"];
                targetObject.QuarterClicks = dictClickReport["�������"];
                targetObject.YearClicks = dictClickReport["������"];
                targetObject.Clicks = dictClickReport["�ܵ����"];

                Assistant.Update(targetObject, new string[] { "DayClicks", "YesterdayClicks", "WeekClicks", "MonthClicks", "QuarterClicks", "YearClicks", "Clicks" });
            }
        }
        #endregion
    }
    /// <summary>
    /// �������к�SN������    /// </summary>
    [Serializable]
    public class CreateSNHelper
    {
        static object lockHelper = new object();//������

        private static long snBase = 0;
        private static long AppSN = 0;

        //public long SnBase
        //{
        //    get
        //    {
        //        lock (lockHelper)
        //        {

        //            long result = 0;
        //            //if (snBase != 0)
        //            //{
        //            long maxSn = 0;

        //            ListField[] fields = new ListField[1];
        //            ListField field = new ListField("SN");
        //            field.Adorn = Adorns.Distinct;
        //            fields[0] = field;
        //            Order[] tmpOrders = new Order[] { new Order("SN", OrderMode.Desc) };

        //            List<Article> tempList = assistant.List<Article>(null, tmpOrders, 0, 0, fields);
        //            long totalHave = (long)tempList.Count;

        //            long totalAll = Assistant.Count<Article>(null);
        //            if (totalAll > totalHave)
        //            {
        //                Order[] orders = new Order[] { new Order("SN", OrderMode.Asc) };
        //                List<Article> articles = Assistant.List<Article>(null, orders);
        //                foreach (Article a in articles)
        //                {
        //                    if (a.SN > maxSn) maxSn = a.SN;
        //                }

        //                //����û��SN��SN�ظ�������
        //                long lastSn = 0;//������һ��SN
        //                long curSn = 0;//���ر����޸�ǰ��SN
        //                foreach (Article a in articles)
        //                {
        //                    curSn = a.SN;
        //                    if (a.SN > 0 && a.SN == lastSn)
        //                    {
        //                        a.SN = ++maxSn;
        //                        UpdateArticle(a, new string[] { "SN" });
        //                    }

        //                    if (a.SN <= 0)
        //                    {
        //                        a.SN = ++maxSn;
        //                        UpdateArticle(a, new string[] { "SN" });
        //                    }
        //                    lastSn = curSn;
        //                }
        //            }
        //            else
        //            {
        //                Order[] orders = new Order[] { new Order("SN", OrderMode.Desc) };
        //                List<Article> articles = Assistant.List<Article>(null, orders, 0,0);
        //                if (articles != null && articles.Count > 0)
        //                    maxSn = articles[0].SN;
        //                else
        //                    maxSn = 0;
        //            }
        //            result = maxSn + 1;
        //            //}
        //            //else
        //            //{
        //            //    result = snBase + 1;
        //            //}
        //            snBase = result;
        //            return result;
        //        }
        //    }
        //}

        public long SnBase
        {
            get
            {
                lock (lockHelper)
                {
                    Criteria c = new Criteria(CriteriaType.Equals, "SN", ++AppSN);
                    long totalAll = Assistant.Count<Article>(c);
                    if (totalAll > 0)
                    {
                        List<Article> articles = Assistant.List<Article>(null, new Order[] { new Order("SN", OrderMode.Desc) }, 0, 1);
                        AppSN = articles[0].SN + 1;
                    }
                    return AppSN;
                }
            }
        }

        private ObjectAssistant assistant;
        /// <summary>
        /// ��ǰHelper�����ݷ��ʶ���
        /// </summary>
        public ObjectAssistant Assistant
        {
            get { return assistant; }
            set { assistant = value; }
        }

        private static readonly string ArticleKeyID = "ArticleID{0}";

        /// <summary>
        /// ����һƪ���¼�¼
        /// </summary>
        /// <param name="a">һƪ���¼�¼</param>
        /// <param name="fields">��Ҫ���µ��ֶ�</param>
        void UpdateArticle(Article a, string[] fields)
        {
            try
            {
                //�������
                HttpContext Context = HttpContext.Current;
                string key = string.Format(ArticleKeyID, a.ID);
                Context.Cache.Remove(key);
                Context.Items.Remove(key);
            }
            catch (Exception)
            {
            }

            Assistant.Update(a, fields);
        }

    }

    public class CriteriaExpressionHelper
    {
        static List<CriteriaExpression> expList = new List<CriteriaExpression>();
        static CriteriaExpressionHelper()
        {
            expList.Add(new LikeExpression());
        }

        public static void Execute(Criteria c, string expr)
        {
            foreach (CriteriaExpression exp in expList)
            {
                exp.Execute(c, expr);
            }
        }
    }

    public interface CriteriaExpression
    {
        void Execute(Criteria c, string expr);
    }

    public class LikeExpression : CriteriaExpression
    {
        Regex regex = new Regex(@"like\((?<field>\S*),(?<value>\S*)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public void Execute(Criteria c, string expr)
        {
            StringReader reader = new StringReader(expr);
            string s = null;
            while (!String.IsNullOrEmpty(s = reader.ReadLine()))
            {
                s = s.Trim();
                Match m = regex.Match(s);
                if (m != null && m.Success)
                {
                    c.Add(CriteriaType.Like, m.Groups["field"].Value, m.Groups["value"].Value);
                }
            }
        }
    }
}