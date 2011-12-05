using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;
using System.Text.RegularExpressions;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

using Thinkment.Data;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.Data;

namespace We7.CMS
{
    /// <summary>
    /// ��Ŀ����������
    /// </summary>
    [Serializable]
    [Helper("We7.ChannelHelper")]
    public class ChannelHelper : BaseHelper
    {
        /// <summary>
        /// ��ȡ��ҳ��Ŀ��Ϣ
        /// </summary>
        /// <returns>��Ŀ��Ϣ</returns>
        public Channel GetFirstChannel()
        {
            return GetFirstChannel(We7Helper.EmptyGUID);
        }

        /// <summary>
        /// ��ȡĳ������Ŀ�µĵ�һ������Ŀ����������һ������Ŀ��
        /// </summary>
        /// <param name="ParentID">����ĿID</param>
        /// <returns>��Ŀ��Ϣ</returns>
        public Channel GetFirstChannel(string ParentID)
        {
            Order[] ods = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", ParentID);
            List<Channel> channels = Assistant.List<Channel>(c, ods, 0, 1);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            return null;
        }

        /// <summary>
        /// ��ȡ������Ŀ������ĿIndex��������
        /// </summary>
        /// <returns>һ����Ŀ��Ϣ</returns>
        public Channel[] GetAllChannels()
        {
            Order[] o = new Order[] { new Order("Index") };
            return Assistant.List<Channel>(null, o).ToArray();
        }

        public List<Channel> GetAllLinkChannels()
        {
            Order[] o = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "Type", (int)TypeOfChannel.QuoteChannel);
            return Assistant.List<Channel>(c, o);
        }

        /// <summary>
        /// ͨ����������ȡһ����Ŀ
        /// </summary>
        /// <param name="alias">��Ŀ����</param>
        /// <returns>��Ŀ��Ϣ</returns>
        public Channel GetChannelByAlias(string alias)
        {
            Order[] o = new Order[] { new Order("ID") };
            Criteria c = new Criteria(CriteriaType.Equals, "Alias", alias);
            List<Channel> channels = Assistant.List<Channel>(c, o);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            else
                return null;
        }

        /// <summary>
        /// ͨ����������ȡһ����Ŀ
        /// </summary>
        /// <param name="alias">��Ŀ����</param>
        /// <returns>��Ŀ��Ϣ</returns>
        public Channel GetChannelById(string id)
        {
            Order[] o = new Order[] { new Order("ID") };
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Channel> channels = Assistant.List<Channel>(c, o);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            else
                return null;
        }


        /// <summary>
        /// ��ȡWAP����Ŀ�б�
        /// </summary>
        /// <returns>��Ŀ�б�</returns>
        public List<Channel> GetWapRootChannels()
        {
            Order[] o = new Order[] { new Order("ID") };
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", We7Helper.EmptyWapGUID);
            return Assistant.List<Channel>(c, o);
        }

        private static readonly string ChannelKeyID = "ChannelID{0}";

        /// <summary>
        /// ͨ����ĿID��ȡ����Ŀ��Ϣ���浽��������ȥ�ˣ�
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <param name="fields">�����ֶ����</param>
        /// <returns>��Ŀ��¼</returns>
        public Channel GetChannel(string channelID, string[] fields)
        {
            if (channelID != null && channelID != string.Empty)
            {
                Channel channel=null;
                HttpContext Context = HttpContext.Current;
                if (Context != null)
                {
                    string key = string.Format(ChannelKeyID, channelID);
                    channel = (Channel)Context.Items[key];//�ڴ�
                    if (channel == null)
                    {
                        channel = (Channel)Context.Cache[key];//����
                        if (channel == null)
                        {
                            if (channelID != null && channelID != string.Empty)
                            {
                                //��ȡ���ݿ�
                                Order[] o = new Order[] { new Order("ID") };
                                Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);
                                List<Channel> channels = Assistant.List<Channel>(c, o);
                                if (channels.Count > 0)
                                {
                                    channel = channels[0];
                                }
                            }

                            if (channel != null)
                            {
                                CacherCache(key, Context, channel, CacheTime.Short);
                            }
                        }

                        if (Context.Items[key] == null)
                        {
                            Context.Items.Remove(key);
                            Context.Items.Add(key, channel);
                        }
                    }
                }
                else
                {
                    if (channelID != null && channelID != string.Empty)
                    {
                        //��ȡ���ݿ�
                        Order[] o = new Order[] { new Order("ID") };
                        Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);
                        List<Channel> channels = Assistant.List<Channel>(c, o);
                        if (channels.Count > 0)
                        {
                            channel = channels[0];
                        }
                    }

                }
                return channel;
            }
            else
                return null;


        }

        /// <summary>
        /// ͨ��Url��ȡchannel
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Channel GetChannel(string url)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "FullUrl", url);
            List<Channel> channels = Assistant.List<Channel>(c, null);
            if (channels.Count > 0)
            {
                return channels[0];
            }
            else
                return null;
        }

        /// <summary>
        /// ����ָ����Ŀ�µ�����Ŀ
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public List<Channel> GetChildren(string oid)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                Criteria c = new Criteria(CriteriaType.Like,"FullUrl",ch.FullUrl+"%");
                c.Add(CriteriaType.Equals, "State", 1);
                return Assistant.List<Channel>(c, new Order[]{new Order("ID") });                
            }
            return null;
        }

        /// <summary>
        /// ȡ��ָ����Ŀ�µ���������Ŀ
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="includeDisableChannel"></param>
        /// <returns></returns>
        public List<Channel> GetChildren(string oid, bool includeDisableChannel)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                Criteria c = new Criteria(CriteriaType.Like, "FullUrl", ch.FullUrl + "%");
                if (!includeDisableChannel)
                {
                    c.Add(CriteriaType.Equals, "State", 1);
                }
                return Assistant.List<Channel>(c, new Order[] { new Order("ID") });
            }
            return null;
        }

        /// <summary>
        /// ����һ����Ŀ
        /// </summary>
        /// <param name="ch">��Ŀ��¼</param>
        public void UpdateChannel(Channel ch)
        {
            ///���»���
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ChannelKeyID, ch.ID);
            Context.Cache.Remove(key);

            string[] fields = new string[] {
                "Name", "Description", "State", "TemplateName",
                "ReferenceID", "Index", "Alias", "Parameter","ProcessEnd",
                "FullPath", "DetailTemplate","ChannelFolder","FullUrl","SecurityLevel",
                "TitleImage","Process","ProcessLayerNO","Type","ChannelName","RefAreaID","ParentID","IsComment","ReturnUrl", "EnumState","KeyWord","DescriptionKey","ModelName"};
            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;

            //���õ�ǰ�ڵ����
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);
            Assistant.Update(ch, fields);
        }

        /// <summary>
        /// ������Ŀ·���ֶΣ�FullPath��ͬʱ����Article���ж�Ӧ�������ֶ�FullChannelPath
        /// </summary>
        /// <param name="ch">���º��channel����</param>
        /// <param name="oldChannelPath">�ɵ�FullPath</param>
        public void UpdateChannelPathBatch(Channel ch, string oldChannelPath)
        {
            //����������������Ŀ
            Criteria c = new Criteria(CriteriaType.Like, "FullUrl", oldChannelPath+"%");
             List<Channel> channels = Assistant.List<Channel>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
            foreach (Channel mychannel in channels)
            {
                try
                {
                    Regex reg = new Regex("^" + oldChannelPath, RegexOptions.IgnoreCase);
                    mychannel.FullPath = reg.Replace(mychannel.FullPath, ch.FullPath);
                    HelperFactory.Instance.GetHelper<ChannelHelper>().UpdateChannel(mychannel, new string[] { "FullPath" });
                }
                catch { }
            }

             //������������������Ϣ
            c = new Criteria(CriteriaType.Like, "ChannelFullUrl", ch.FullUrl+"%");
            List<Article> articles = Assistant.List<Article>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
            foreach (Article a in articles)
            {
                try
                {
                    Regex reg = new Regex("^" + oldChannelPath, RegexOptions.IgnoreCase);
                    a.FullChannelPath = reg.Replace(a.FullChannelPath, ch.FullPath);
                    a.ChannelName=ch.Name;
                    HelperFactory.Instance.GetHelper<ArticleHelper>().UpdateArticle(a, new string[] { "FullChannelPath","ChannelName" });
                }
                catch { }
            }
         }

        /// <summary>
        /// ����������ĿUrl��ַ
        /// </summary>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        public void UpdateChannelUrlBatch2(string oldUrl, string newUrl)
        {
            Criteria c=new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Like, "ChannelFullUrl",  oldUrl+"%" );
            List<Article> articles=Assistant.List<Article>(c,new Order[]{new Order("Updated",OrderMode.Desc)});
            foreach (Article a in articles)
            {
                try
                {
                    Regex reg = new Regex("^" + oldUrl, RegexOptions.IgnoreCase);
                    a.ChannelFullUrl = reg.Replace(a.ChannelFullUrl, newUrl);
                    HelperFactory.Instance.GetHelper<ArticleHelper>().UpdateArticle(a, new string[] { "ChannelFullUrl" });
                }
                catch { }
            }

            c = new Criteria(CriteriaType.Like, "FullUrl",  oldUrl+"%" );

            List<Channel> channels = Assistant.List<Channel>(c, new Order[] { new Order("Updated", OrderMode.Desc) });
            foreach (Channel ch in channels)
            {
                try
                {
                    Regex reg = new Regex("^" + oldUrl, RegexOptions.IgnoreCase);
                    ch.FullUrl = reg.Replace(ch.FullUrl, newUrl);
                    HelperFactory.Instance.GetHelper<ChannelHelper>().UpdateChannel(ch, new string[] { "FullUrl" });
                }
                catch { }
            }
        }

        /// <summary>
        /// ����������ĿUrl���ݣ�Channel��Article��
        /// </summary>
        /// <param name="oldUrl"></param>
        /// <param name="newUrl"></param>
        public void UpdateChannelUrlBatch(string oldUrl, string newUrl)
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sql = new SqlStatement();

            string sqlCommandTxt =
       @"update {1} set {2}=replace({2},{0}OLDURL,{0}NEWURL) 
            where {2} like {0}KEY";

            DataParameter dp = new DataParameter();
            dp.ParameterName = db.DbDriver.Prefix + "OLDURL";
            dp.DbType = DbType.String;
            dp.Value = oldUrl;
            dp.Size = 255;
            sql.Parameters.Add(dp);

            DataParameter dp2 = new DataParameter();
            dp2.ParameterName = db.DbDriver.Prefix + "NEWURL";
            dp2.Value = newUrl;
            dp2.DbType = DbType.String;
            dp2.Size = 255;
            sql.Parameters.Add(dp2);

            DataParameter dp3 = new DataParameter();
            dp3.ParameterName = db.DbDriver.Prefix + "KEY";
            dp3.Value = oldUrl + "%";
            dp3.DbType = DbType.String;
            dp3.Size = 255;
            sql.Parameters.Add(dp3);


            using (IConnection conn = db.CreateConnection())
            {
                sql.SqlClause = string.Format(sqlCommandTxt, db.DbDriver.Prefix, "[Article]", "[ChannelFullUrl]");
                db.DbDriver.FormatSQL(sql);
                conn.Update(sql);

                sql.SqlClause = string.Format(sqlCommandTxt, db.DbDriver.Prefix, "[Channel]", "[FullUrl]");
                db.DbDriver.FormatSQL(sql);
                conn.Update(sql);
            }

        }


        /// <summary>
        /// ɾ��Ȩ��
        /// </summary>
        /// <param name="typeID">Ȩ�޶��������</param>
        /// <param name="ownerID">Ȩ�޶����ID��</param>
        /// <param name="parentID">�ϼ���Ŀ</param>
        public void DeleteChildrenPermission(int typeID, string ownerID, string parentID)
        {
            List<Channel> chs = GetChildren(parentID,true);
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    List<Permission> permissions = AccountFactory.CreateInstance().GetPermissions(typeID, ownerID, ch.ID);
                    if (permissions != null && permissions.Count > 0)
                    {
                        AccountFactory.CreateInstance().DeletePermission(typeID, ownerID, ch.ID);
                    }
                }
            }
        }

        /// <summary>
        /// ɾ��Ȩ��
        /// </summary>
        /// <param name="typeID">Ȩ�޶��������</param>
        /// <param name="ownerID">Ȩ�޶����ID��</param>
        /// <param name="parentID">�ϼ���Ŀ</param>
        /// <param name="contents">��Ŀ����</param>
        public void DeleteChildrenPermission(int typeID, string ownerID, string parentID,string[] contents)
        {
            List<Channel> chs = GetChildren(parentID,true);
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    List<Permission> permissions = AccountFactory.CreateInstance().GetPermissions(typeID, ownerID, ch.ID);
                    if (permissions != null && permissions.Count > 0)
                    {
                        AccountFactory.CreateInstance().DeletePermission(typeID, ownerID, ch.ID, contents);
                    }
                }
            }
        }

        /// <summary>
        /// Ϊ����Ŀ���Ȩ��
        /// </summary>
        /// <param name="typeID">Ȩ�޶�������</param>
        /// <param name="ownerID">Ȩ��ӵ���ߵ�ID</param>
        /// <param name="parentID">�ϼ���Ŀ</param>
        /// <param name="contents">Ȩ������</param>
        public void AddChildrenPermission(int typeID, string ownerID, string parentID, string[] contents)
        {
            List<Channel> chs = GetChildren(parentID,true);
            if (chs != null)
            {
                foreach (Channel ch in chs)
                {
                    AccountFactory.CreateInstance().DeletePermission(typeID, ownerID, ch.ID);
                    AccountFactory.CreateInstance().AddPermission(typeID, ownerID, ch.ID, contents);
                }
            }
        }

        /// <summary>
        /// ȡ��ģ������
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public string GetModelName(string oid, out string Parameter)
        {
            Channel ch = GetChannel(oid, null);
            if (ch != null)
            {
                //if (String.IsNullOrEmpty(ch.ModelName))
                //{
                //    UpdateModelType(ch);
                //}
                Parameter = ch.Parameter;
                return ch.ModelName;
            }
            else
            {
                Parameter = String.Empty;
                return String.Empty;
            }
        }


        /// <summary>
        /// ����һ����Ŀ��¼
        /// </summary>
        /// <param name="ch">��Ŀ��¼</param>
        /// <param name="fields">Ҫ���µ��ֶ�</param>
        public void UpdateChannel(Channel ch, string[] fields)
        {
            ///���»���
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ChannelKeyID, ch.ID);
            Context.Cache.Remove(key);

            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;

            //���õ�ǰ�ڵ����
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);

            Assistant.Update(ch, fields);
        }

        /// <summary>
        /// ����һ����Ŀ�ĸ���ĿID
        /// </summary>
        /// <param name="id">��ĿID</param>
        /// <param name="parent">����ĿID</param>
        public void UpdateChannelParent(string id, string parent)
        {
            Channel ch = new Channel();
            ch.ID = id;
            ch.ParentID = parent;
            Assistant.Update(ch, new string[] { "ParentID" });
        }

        /// <summary>
        /// �ж�һ����Ŀ�Ƿ���һ����Ŀ������Ŀ
        /// </summary>
        /// <param name="parent">����ĿID</param>
        /// <param name="child">����ĿID</param>
        /// <returns></returns>
        public bool IsChild(string parent, string child)
        {
            Channel ch = new Channel();
            ch.ID = child;
            Criteria c = new Criteria(CriteriaType.Equals, "ID", child);
            string[] fields = new string[] { "ParentID", "ID" };
            while (Assistant.Count<Channel>(c) > 0)
            {
                Assistant.Select(ch, fields);
                if (ch.ParentID == parent)
                {
                    return true;
                }
                c.Value = ch.ParentID;
                ch.ID = ch.ParentID;
            }
            return false;
        }

        /// <summary>
        /// �ж�һ����Ŀ�Ƿ�������Ŀ
        /// </summary>
        /// <param name="id">����ĿID</param>
        /// <returns></returns>
        public bool HasChild(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", id);
            return (Assistant.Count<Channel>(c) > 0) ? true : false;
        }

        /// <summary>
        /// ���һ����Ŀ
        /// </summary>
        /// <param name="ch">��Ŀ��Ϣ</param>
        /// <returns></returns>
        public Channel AddChanel(Channel ch)
        {
            string emptyID = "{00000000-";

            if (ch.ID == null || ch.ID.Length < 1)
            {
                ch.ID = We7Helper.CreateNewID();
            }
            ch.Created = DateTime.Now;
            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;

            //���õ�ǰ�ڵ����
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);

            Assistant.Insert(ch);
            return ch;
        }


        /// <summary>
        /// wjz ���һ����Ŀ
        /// </summary>
        /// <param name="ch">��Ŀ��Ϣ ADD TIME 2010-08-03</param>
        /// <returns></returns>
        public Channel AddChanelByDBImport(Channel ch)
        {
            string emptyID = "{00000000-";

            if (ch.ID == null || ch.ID.Length < 1)
            {
                ch.ID = We7Helper.CreateNewID();
            }
            ch.Created = DateTime.Now;
            ch.FullPath = GetChannelFullPath(ch.ParentID) + "/" + ch.Name;
            //ch.FullUrl = GetChannelFullUrl(ch.ParentID) + "/" + ch.ChannelName;
            //���õ�ǰ�ڵ����
            string tmpStr1 = ch.FullUrl.Substring(0, ch.FullUrl.Length - 1);
            int chNum = 1;
            if (tmpStr1.Length != 0)
            {
                string tmpStr2 = tmpStr1.Replace("/", "");
                chNum = tmpStr1.Length - tmpStr2.Length;
            }
            ch.EnumState = StateMgr.StateProcess(ch.EnumState, EnumLibrary.Business.ChannelNodeLevel, chNum);

            Assistant.Insert(ch);
            return ch;
        }

        /// <summary>
        /// ͨ����ĿIDɾ����Ŀ��ͬʱɾ������Ŀ����ص�����
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        public void DeleteChannel(string channelID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", channelID);
            List<Channel> channels = Assistant.List<Channel>(c, null, 0, 0, new string[] { "ID", "ChannelName" });
            foreach (Channel channel in channels)
            {
                DeleteChannel(channel.ID);
            }

            Channel ch = new Channel();
            ch.ID = channelID;
            Assistant.Delete(ch);

            //ͬʱɾ������
            Criteria ca = new Criteria(CriteriaType.Equals, "OwnerID", channelID);
            List<Article> artcles = Assistant.List<Article>(ca, null);
            foreach (Article a in artcles)
            {
                Assistant.Delete(a);
                //ɾ����������
                Criteria articleCommentsCriteria = new Criteria(CriteriaType.Equals, "ArticleID", a.ID);
                List<Comments> articleComments = Assistant.List<Comments>(articleCommentsCriteria, null);
                foreach (Comments articleComment in articleComments)
                {
                    Assistant.Delete(articleComment);
                }
            }
            //ͬʱɾ����Ŀ����
            Criteria channelCommentsCriteria = new Criteria(CriteriaType.Equals, "ArticleID", ch.ChannelName);
            List<Comments> channelComments = Assistant.List<Comments>(channelCommentsCriteria, null);
            foreach (Comments channelComment in channelComments)
            {
                Assistant.Delete(channelComment);
            }
        }

        /// <summary>
        ///����һ����Ŀ����������Ŀ
        /// </summary>
        /// <param name="parentID">����ĿID</param>
        /// <returns></returns>
        public List<Channel> GetChannels(string parentID)
        {
            return GetChannels(parentID, false);
        }

        /// <summary>
        ///  ����һ����Ŀ���µ����õ�����Ŀ����
        /// </summary>
        /// <param name="parentID">һ����ĿID</param>
        /// <param name="OnlyInUser">�Ƿ�ֻ�������õ���Ŀ</param>
        /// <returns></returns>
        public List<Channel> GetChannels(string parentID, bool OnlyInUser)
        {
            Criteria condition = new Criteria(CriteriaType.Equals, "ParentID", parentID);
            if (OnlyInUser)
                condition.Add(CriteriaType.Equals, "State", 1);

            Order[] ods = new Order[] { new Order("Index") };

            List<Channel> list = null;
            if (Assistant.Count<Channel>(condition) > 0)
            {
                list = Assistant.List<Channel>(condition, ods);
            }
            else
            {
                list = null;
            }
            return list;
        }

        /// <summary>
        /// ȡ��url���� /news/*����Ŀ�б��ǰ��λ
        /// </summary>
        /// <param name="urlPatern"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Channel> GetChannels(string urlPatern, int top)
        {
            //�˴�ȱ�ٻ��洦��
            Criteria condition = new Criteria(CriteriaType.Like, "FullUrl", urlPatern);
            Order[] ods = new Order[] { new Order("Index") };
            return Assistant.List<Channel>(condition, ods, 0, top);
        }

        /// <summary>
        /// ���ݱ�ǩ�б��ȡ��Ŀ�б�
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public List<Channel> GetChannelsByTags(string[] tags)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (tags.Length > 0)
            {
                c.Mode = CriteriaMode.Or;
                foreach (string tag in tags)
                {
                    c.AddOr(CriteriaType.Like, "Tags", "%'" + tag + "'%");
                }
                Order[] o = new Order[] { new Order("Index") };
                return Assistant.List<Channel>(c, o);
            }
            else
                return null;
        }

        /// <summary>
        /// ͨ����ĿID��ȡ�����Ŀ�ı�ǩ
        ///
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <returns></returns>
        public List<string> GetTags(string channelID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", channelID);

            List<Channel> articles = Assistant.List<Channel>(c, null);
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
        /// ��ȡ������Ŀ��ǩ��¼
        ///  2011-11-9 ��ǩ�Ѿ����ϣ�δ���ִ˷���������
        /// </summary>
        /// <returns></returns>
        //public List<ChannelTag> GetAllTags()
        //{
        //    return Assistant.List<ChannelTag>(null, null);
        //}

      
 
 
        /// <summary>
        /// ��ȡһ����Ŀ��FullPath
        /// </summary>
        /// <param name="id">��ĿID</param>
        /// <returns>��Ŀ��FullPath</returns>
        string GetChannelFullPath(string id)
        {
            if (We7Helper.IsEmptyID(id))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            string pid = id;
            do
            {
                Channel c = new Channel();
                Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", pid);
                List<Channel> cList = Assistant.List<Channel>(cTmp, null, 0, 0, new string[] { "ID", "Name", "ParentID" });
                if (cList != null && cList.Count > 0)
                {
                    c = cList[0];
                    sb.Insert(0, c.Name);
                    sb.Insert(0, "/");
                    pid = c.ParentID;
                }
                else
                {
                    pid = string.Empty;
                }
            }
            while (!We7Helper.IsEmptyID(pid));
            return sb.ToString();
        }

        /// <summary>
        /// ��ȡһ����Ŀ��FullUrl
        /// </summary>
        /// <param name="id">��ĿID</param>
        /// <returns></returns>
        string GetChannelFullUrl(string id)
        {
            if (We7Helper.IsEmptyID(id))
            {
                return "/";
            }
            StringBuilder sb = new StringBuilder();
            string pid = id;
            do
            {
                Channel c = new Channel();
                Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", pid);
                List<Channel> cList = Assistant.List<Channel>(cTmp, null, 0, 0, new string[] { "ID", "ChannelName", "ParentID" });
                if (cList != null && cList.Count > 0)
                {
                    c = cList[0];
                    sb.Insert(0, c.ChannelName);
                    sb.Insert(0, "/");
                    pid = c.ParentID;
                }
                else
                {
                    pid = string.Empty;
                }
            }
            while (!We7Helper.IsEmptyID(pid));
            return sb.ToString();
        }

        /// <summary>
        /// ��ȡһ����Ŀ������
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <returns>��Ŀ����</returns>
        public string GetChannelName(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "FullPath", "Name" });
            if (ch != null)
                return ch.Name;
            else
                return "";
        }

        /// <summary>
        /// ��ȡһ����Ŀ�Ĵ�ŵ�����·��
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <returns></returns>
        public string GetChannelFilePath(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "ChannelFolder", "Name" });
            if (ch != null && ch.ChannelFolder != null && ch.ChannelFolder.Length > 0)
            {
                if (ch.ChannelFolder.IndexOf("/") > -1 || ch.ChannelFolder.IndexOf("\\") > -1)
                //���ݾɰ汾�д�� /_data/channels/sasa/����·�������
                {
                    ch.ChannelFolder = ch.ChannelFolder.Replace("\\", "/");
                    ch.ChannelFolder = ch.ChannelFolder.Replace("//", "/");
                    if (ch.ChannelFolder.EndsWith("/")) ch.ChannelFolder = ch.ChannelFolder.Remove(ch.ChannelFolder.Length - 1);
                    ch.ChannelFolder = ch.ChannelFolder.Substring(ch.ChannelFolder.LastIndexOf("/") + 1);
                }
                string path = "/" + Constants.ChannelPath.Replace("\\", "/") + "/" + ch.ChannelFolder;
                HttpContext Context = HttpContext.Current;
                if (!Directory.Exists(Context.Server.MapPath(path)))
                    Directory.CreateDirectory(Context.Server.MapPath(path));
                return path;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// ��ͬ�˺Ž�����ͬ�ļ���
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetChannelFilePath(string channelID, string username)
        {
            string chpath = GetChannelFilePath(channelID);
            if (We7Helper.IsEmptyID(username))
                return chpath;
            else
            {
                if (!chpath.EndsWith("/")) chpath = chpath + "/";
                chpath = chpath + username;
                return chpath;
            }
        }

        /// <summary>
        /// ֱ�ӻ�ȡ���ݿ���Ӧ�ֶ�FullUrl
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <returns></returns>
        public string GetFullUrl(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "FullUrl", "Name" });
            if (ch != null)
            {
                return ch.FullUrl;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ֱ�ӻ�ȡ���ݿ���Ӧ�ֶ�FullPath
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <returns></returns>
        public string GetFullPath(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "FullPath", "Name" });
            if (ch != null)
            {
                return ch.FullPath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ��ȡ������Ŀ�����Ƽ���
        /// </summary>
        /// <returns></returns>
        public ArrayList GetAllChannelNames()
        {
            Order[] o = new Order[] { new Order("Index") };
            Channel[] cs = Assistant.List<Channel>(null, o).ToArray();
            ArrayList channelNames = new ArrayList();

            foreach (Channel c in cs)
            {
                channelNames.Add(c.Name);
            }
            return channelNames;
        }

        /// <summary>
        /// ͨ����Ŀ���ƻ�ȡ��ĿID
        /// </summary>
        /// <param name="OnlyName">��Ŀ����</param>
        /// <returns></returns>
        public string GetChannelIDByOnlyName(string OnlyName)
        {
            Order[] ods = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "ChannelName", OnlyName);
            List<Channel> channels = Assistant.List<Channel>(c, ods, 0, 1);
            if (channels.Count > 0)
            {
                return channels[0].ID;
            }
            else
                return We7Helper.NotFoundID;
        }

        /// <summary>
        /// ͨ��url��ȡ��ĿID
        /// </summary>
        /// <param name="fullurl"></param>
        /// <returns></returns>
        public string GetChannelIDByFullUrl(string fullurl)
        {
            Order[] ods = new Order[] { new Order("Index") };
            Criteria c = new Criteria(CriteriaType.Equals, "FullUrl", fullurl);
            List<Channel> channels = Assistant.List<Channel>(c, ods, 0, 1);
            if (channels.Count > 0)
            {
                return channels[0].ID;
            }
            else
                return We7Helper.NotFoundID;
        }

        /// <summary>
        /// ��ȡһ����Ŀ������Ŀ
        /// </summary>
        /// <param name="parentID">��ĿID</param>
        /// <param name="recusive">true������������Ŀ��false����ֻΪ�����Ŀ������Ŀ</param>
        /// <returns></returns>
        public List<Channel> GetSubChannelList(string parentID, bool recusive)
        {
            return GetSubChannelList(parentID, recusive, false);
        }
        #region ��ȡר��ͼƬ
        /// <summary>
        /// ͨ����ǩ����Ŀ����ȡ��ר����Ŀ
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        //public List<Channel> QueryTopPhotos(string tag, int count)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "Type", 3);
        //    Order[] orders = new Order[] { new Order("Created", OrderMode.Desc) };
        //    List<Channel> channels = Assistant.List<Channel>(c, orders);
        //    if (tag != null && tag.Trim().Length > 0)
        //    {
        //        List<Channel> list = new List<Channel>();
        //        foreach (Channel a in channels)
        //        {
        //            List<ChannelTag> tags = GetTags(a.ID);
        //            List<String> ts = new List<string>();
        //            foreach (ChannelTag t in tags)
        //            {
        //                ts.Add(t.Identifier);
        //            }
        //            if (!ts.Contains(tag))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                if (list.Count < count)
        //                    list.Add(a);
        //                else
        //                    break;
        //            }
        //        }
        //        channels = list;
        //    }
        //    return channels;

        //}

        #endregion

        /// <summary>
        /// ��ȡһ����Ŀ������Ŀ
        /// </summary>
        /// <param name="parentID">��ĿID</param>
        /// <param name="recusive">�Ƿ�ֻ���������Ŀ�ĵ�һ������Ŀ</param>
        /// <param name="OnlyInUser">�Ƿ�ֻ�������õ���Ŀ</param>
        /// <returns></returns>
        public List<Channel> GetSubChannelList(string parentID, bool recusive, bool OnlyInUser)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", parentID);
            if (OnlyInUser)
                c.Add(CriteriaType.Equals, "State", 1);

            Order od = new Order("Index", OrderMode.Asc);
            List<Channel> chs = Assistant.List<Channel>(c, new Order[] { od });
            List<Channel> allchs = Assistant.List<Channel>(c, new Order[] { od });
            if (recusive)
            {
                foreach (Channel ch in chs)
                {
                    List<Channel> subs = GetSubChannelList(ch.ID, recusive);
                    foreach (Channel sub in subs)
                    {
                        ch.Channels.Add(sub);
                        allchs.Add(sub);
                    }
                }
            }
            return allchs;
        }

        /// <summary>
        /// ��ȡ������������Ŀ
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Channel[] QueryAllChannel(int begin, int end)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Type", "1");
            Order[] ods = new Order[] { new Order("ID") };
            List<Channel> items = Assistant.List<Channel>(c, ods, begin, end);
            if (Assistant.List<Channel>(c, ods, begin, end).Count > 0)
            {
                return items.ToArray();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��ȡ��������Ŀ��
        /// </summary>
        /// <returns></returns>
        public int QueryAllCounts()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Type", "1");
            List<Channel> items = Assistant.List<Channel>(c, null);
            if (items.Count > 0)
            {
                return items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// �ж��Ƿ�Ϊ������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChannelType(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            c.Add(CriteriaType.Equals, "Type", "2");
            if (Assistant.List<Channel>(c, null).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ͨ����ĿID������Ŀ�ı���
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public string GetChannelAlias(string channelID)
        {
            Channel ch = GetChannel(channelID, new string[] { "ID", "Alias" });
            if (ch != null)
                return ch.Alias;
            else
                return "";
        }

        /// <summary>
        /// ��ʽ����Ŀ��FullPath
        /// </summary>
        /// <param name="channelFullPath">��Ŀ��FullPath</param>
        /// <param name="dotChar"></param>
        /// <returns></returns>
        public string FullPathFormat(string channelFullPath, string dotChar)
        {
            channelFullPath = channelFullPath.Replace("//", "/");
            if (channelFullPath.StartsWith("/"))
                channelFullPath = channelFullPath.Remove(0, 1);
            channelFullPath = channelFullPath.Replace("/", dotChar);
            return channelFullPath;
        }

        /// <summary>
        /// ����һ����Ŀ
        /// </summary>
        /// <param name="ch">��Ŀ����</param>
        /// <param name="fields">��Ҫ���µ��ֶ�</param>
        public void UpdateChannelTitle(Channel ch, string[] fields)
        {
            Assistant.Update(ch, fields);
        }

        /// <summary>
        /// ����EnumState��ȡ��Ŀ����
        /// </summary>
        /// <param name="channelEnumState">��Ŀ����</param>
        /// <returns></returns>
        public List<Channel> GetChannelsByType(string channelEnumState)
        {
            HttpContext Context = HttpContext.Current;
            string key = string.Format(ChannelKeyID, channelEnumState);
            List<Channel> ChannelList = (List<Channel>)Context.Items[key];//�ڴ�
            if (ChannelList == null)
            {
                ChannelList = (List<Channel>)Context.Cache[key];//����
                if (ChannelList == null)
                {
                    Criteria c = null;
                    if (!string.IsNullOrEmpty(channelEnumState))
                    {
                        c = new Criteria(CriteriaType.Equals, "EnumState", channelEnumState);
                        c.Adorn = Adorns.Substring;
                        c.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ChannelContentType];
                        c.Length = EnumLibrary.PlaceLenth;
                    }
                    Order[] ods = new Order[] { new Order("Index") };
                    ChannelList = Assistant.List<Channel>(c, ods);
                }

                if (Context.Items[key] == null)
                {
                    Context.Items.Remove(key);
                    Context.Items.Add(key, ChannelList);
                }
            }
            return ChannelList;
        }

        /// <summary>
        /// ��ģ������ȡ����Ŀ�б�
        /// </summary>
        /// <param name="modelname">ģ������</param>
        /// <returns></returns>
        public List<Channel> GetChannelByModelName(string modelname)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ModelName", modelname);
            //c.Add(CriteriaType.Equals,"State",1);

            return Assistant.List<Channel>(c, new Order[] { new Order("Created", OrderMode.Desc) });
        }

        /// <summary>
        /// ��ģ������ȡ�õ���Ŀ�ĵ�һ��ֵ
        /// </summary>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public Channel FirestByModelName(string modelname)
        {
            List<Channel> chs = GetChannelByModelName(modelname);
            return chs.Count > 0 ? chs[0] : null;
        }

        /// <summary>
        /// ����Title��ѯID
        /// </summary>
        /// <param name="channelTitle">��Ŀ����</param>
        /// <returns></returns>
        public string GetChannelIDByTitle(string channelTitle)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Name", channelTitle);
            List<Channel> ch = Assistant.List<Channel>(c, null);
            if (ch.Count > 0)
            {
                return ch[0].ID;
            }
            return "";
        }

        /// <summary>
        /// ��ȡһ����ǩ����Ŀ����
        ///  2011-11-9 ��ǩ�Ѿ����ϣ�δ���ִ˷���������
        /// </summary>
        /// <param name="Tag">��ǩ</param>
        /// <returns></returns>
        //public List<Channel> QueryChannelByTag(string Tag)
        //{
        //    List<Channel> list = Assistant.List<Channel>(null, null, 0, 0, new string[] { "ID", "Name" });
        //    if (Tag == null)
        //    {
        //        return list;
        //    }
        //    else
        //    {
        //        List<Channel> returnlist = new List<Channel>();
        //        foreach (Channel c in list)
        //        {
        //            List<ChannelTag> tags = GetChannelTags(c.ID);
        //            List<String> ts = new List<string>();
        //            foreach (ChannelTag ct in tags)
        //            {
        //                //ts.Add(ct.Identifier);
        //            }
        //            if (!ts.Contains(Tag))
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                returnlist.Add(c);
        //            }
        //        }
        //        return returnlist;
        //    }
        //}

        /// <summary>
        /// ��ȡһ����Ŀ�ı�ǩ����
        ///  2011-11-9 ��ǩ�Ѿ����ϣ�δ���ִ˷���������
        /// </summary>
        /// <param name="channelID">��ĿID</param>
        /// <returns></returns>
        //public List<ChannelTag> GetChannelTags(string channelID)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "ColumnID", channelID);
        //    List<ChannelTag> ts = Assistant.List<ChannelTag>(c, null);
        //    return ts;
        //}

        /// <summary>
        /// ��ȡ��Ŀ��IP����
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public string QueryStrategy(string channelID)
        {
            Channel channel = GetChannel(channelID, new string[] { "IPStrategy" });
            return channel != null ? channel.IPStrategy : String.Empty;
        }

        /// <summary>
        /// ����IP��ȫ����
        /// </summary>
        /// <param name="channelID"></param>
        /// <param name="ipStrategy"></param>
        /// <returns></returns>
        public void UpdateStrategy(string channelID, string ipStrategy)
        {
            Channel channel = GetChannel(channelID, new string[] { "IPStrategy" });
            channel.IPStrategy = ipStrategy;
            Assistant.Update(channel, new string[] { "IPStrategy" });
        }


        #region ��Ŀ�Ż�
        /// <summary>
        /// ���ո���ʱ���ȡ���е���Ŀ����
        /// </summary>
        /// <returns></returns>
        public List<Channel> GetAllChannel()
        {
            Order[] o = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Channel>(null, o);
        }

        /// <summary>
        /// ������Ŀ��ǩ
        ///  2011-11-9 ��ǩ�Ѿ����ϣ�δ���ִ˷���������
        /// </summary>
        /// <returns></returns>
        //public int UpdateArticleTags()
        //{
        //    int count = 0;
        //    List<Channel> allList = GetAllChannel();
        //    if (allList != null)
        //    {
        //        foreach (Channel channel in allList)
        //        {
        //            StringBuilder sb = new StringBuilder();
                    
        //            //List<ChannelTag> allTagsByChannelID = GetTags(channel.ID);
        //            //if (allTagsByChannelID != null)
        //            //{
        //            //    int i = 0;
        //            //    foreach (ChannelTag channelTag in allTagsByChannelID)
        //            //    {
        //            //        sb.Append("'" + channelTag.Identifier + "'");
                             
        //            //        i++;
        //            //    }
        //            //}
        //            //if (sb.ToString() != "")
        //            //{
        //            //    channel.Tags = sb.ToString();
        //            //    Assistant.Update(channel, new string[] { "Tags" });
        //            //    count++;
        //            //}
        //        }
        //    }
        //    return count;
        //}

        /// <summary>
        /// �Ż���Ŀ��ǩ��ѯ
        /// </summary>
        /// <param name="channelEnumState"></param>
        /// <param name="stringList"></param>
        /// <returns></returns>
        public List<Channel> GetChannelType(string channelEnumState, List<string> stringList)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "EnumState", channelEnumState);
            c.Adorn = Adorns.Substring;
            c.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ChannelContentType];
            c.Length = EnumLibrary.PlaceLenth;
            c.Mode = CriteriaMode.Or;
            if (stringList != null)
            {
                foreach (string tag in stringList)
                {
                    c.AddOr(CriteriaType.Like, "Tags", "%" + tag + "%");
                }
            }
            List<Channel> channels = Assistant.List<Channel>(c, null);
            return channels;
        }



        /// <summary>
        ///  ͨ����ĿȨ�޺���Ŀ״̬��ȡ��Ӧ����Ŀ����
        /// </summary>
        /// <param name="channelEnumState">��Ŀ״̬</param>
        /// <param name="iDList">��ĿID����</param>
        /// <returns></returns>
        public List<Channel> GetChannelByIDList(string channelEnumState, List<string> iDList)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "EnumState", channelEnumState);
            c.Adorn = Adorns.Substring;
            c.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ChannelContentType];
            c.Length = EnumLibrary.PlaceLenth;

            if (iDList != null)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                foreach (string id in iDList)
                {
                    keyCriteria.Add(CriteriaType.Equals, "ID", id);
                }
                c.Criterias.Add(keyCriteria);
            }
            List<Channel> channels = Assistant.List<Channel>(c, null);
            return channels;
        }


        /// <summary>
        /// ͨ��Ȩ�޺���Ŀ����ȡ��Ŀ����
        /// </summary>
        /// <param name="stringList">��ĿID����</param>
        /// <param name="name">����Ŀ��������</param>
        /// <param name="filedName">��Ŀ����</param>
        /// <returns></returns>
        public List<Channel> GetChannelByParentID(List<string> stringList, string name, string filedName)
        {
            Criteria c = new Criteria(CriteriaType.None);

            if (stringList != null)
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                foreach (string id in stringList)
                {
                    keyCriteria.Add(CriteriaType.Equals, "ID", id);
                }
                c.Criterias.Add(keyCriteria);
            }
            if (name != null && name.Length > 0 && filedName != null && filedName.Length > 0)
            {
                c.Criterias.Add(new Criteria(CriteriaType.Like, filedName, name + "%"));

            }
            Order[] o = new Order[] { new Order("Name", OrderMode.Desc) };
            List<Channel> channels = Assistant.List<Channel>(c, o);
            return channels;
        }
        #endregion


        /// <summary>
        /// ��ʽ����ĿID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string FormatChannelGUID(string id)
        {
            if (We7Helper.IsEmptyID(id) || We7Helper.IsGUID(id))
                return id;
            else
            {
                Channel ch = GetChannelByAlias(id);
                if (ch != null)
                    return ch.ID;
                else
                    return null;
            }
        }

        /// <summary>
        /// ��Ŀ��ѯ��ͨ����ѯ����и��Ӳ�ѯ
        /// </summary>
        /// <param name="query">��ѯ��</param>
        /// <returns></returns>
        public List<Channel> QueryChannels(ChannelQuery query)
        {
            Criteria c = CreateCriteriaByQuery(query);
            List<Order> orders = CreateOrdersByAll(query.OrderKeys);
            Order[] o = null;
            if (orders != null) o = orders.ToArray();
            return Assistant.List<Channel>(c, o);
        }

        /// <summary>
        /// ������ѯ����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private Criteria CreateCriteriaByQuery(ChannelQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (query.EnumState != null && query.EnumState != "")
            {
                Criteria csubC = new Criteria();
                csubC.Name = "EnumState";
                csubC.Value = query.EnumState;
                csubC.Type = CriteriaType.Equals;
                csubC.Adorn = Adorns.Substring;
                csubC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.ArticleType];
                csubC.Length = EnumLibrary.PlaceLenth;
                c.Criterias.Add(csubC);
            }

            if (query.State != ArticleStates.All)
                c.Add(CriteriaType.Equals, "State", (int)query.State);

            if (query.ParentID != null && query.ParentID != "")
            {
                if (query.IncludeAllSons)
                {
                    Channel ch = GetChannel(query.ParentID, null);
                    if (ch != null)
                    {
                        c.Add(CriteriaType.Like, "FullUrl", ch.FullUrl + "%");
                    }
                }
                else
                    c.Add(CriteriaType.Equals, "ParentID", query.ParentID);
            }
            if (!string.IsNullOrEmpty(query.Tag))
                c.Add(CriteriaType.Like, "Tags", "%'" + query.Tag + "'%");

            return c;
        }

        #region ��̬URL

        private static readonly string ChannelKeyName = "Channel:{0}";

        /// <summary>
        /// ͨ��URLȡ����ĿID��
        /// </summary>
        /// <returns></returns>
        public string GetChannelIDFromURL()
        {
            HttpContext Context = HttpContext.Current;
            if (Context.Request["id"] != null)
                return Context.Request["id"];
            else
            {
                string chID = string.Empty;
                string channelUrl = GetChannelUrlFromUrl(Context.Request.RawUrl, Context.Request.ApplicationPath);
                string key = string.Format(ChannelKeyName, channelUrl);
                string channelID = (string)Context.Items[key];
                if (string.IsNullOrEmpty(channelUrl)) channelID = We7Helper.EmptyGUID;
                if (channelID == null || channelID.Length == 0)
                {
                    channelID = (string)Context.Cache[key];
                    if (channelID == null || channelID.Length == 0)
                    {
                        if (channelUrl != string.Empty)
                            channelID = GetChannelIDByFullUrl(channelUrl);
                        if (channelID != null && channelID.Length > 0)
                        {
                            CacherCache(key, Context, channelID, CacheTime.Short);
                        }
                    }
                    if (Context.Items[key] == null)
                    {
                        Context.Items.Remove(key);
                        Context.Items.Add(key, channelID);
                    }
                }
                return channelID;
            }
        }


        /// <summary>
        /// ͨ��URLȡ����ĿΨһ����
        /// </summary>
        /// <param name="path"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public string GetChannelNameFromUrl(string path, string app)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("channel=") > -1)
                {
                    path = path.Substring(path.ToLower().IndexOf("channel=") + 8);
                    if (path.IndexOf("&") > -1)
                        path = path.Remove(path.IndexOf("&"));
                }
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            if (path.ToLower().EndsWith(".aspx") || path.ToLower().EndsWith("." + ext))
                path = path.Remove(path.LastIndexOf("/") + 1);

            if (!path.StartsWith("/")) path = "/" + path;
            string mathstr = @"(?:\/(\w|\s|(-)|(_))+((\/?))?)$";
            if (Regex.IsMatch(path, mathstr))
            {
                if (!app.StartsWith("/"))
                {
                    app = "/" + app;
                }
                if (!app.EndsWith("/"))
                {
                    app += "/";
                }
                path = path.Replace("//", "/");
                if (path.ToLower().StartsWith(app.ToLower()))
                {
                    path = path.Remove(0, app.Length);
                }
                if (path.EndsWith("/"))
                {
                    path = path.Remove(path.Length - 1);
                }

                int lastSlash = path.LastIndexOf("/");
                if (lastSlash > -1)
                {
                    path = path.Remove(0, lastSlash + 1);
                }

                if (path.ToLower() == "go") path = string.Empty;
                return path;
            }
            else
                return string.Empty;
        }

        ///// <summary>
        ///// ͨ��url��ȡ��ǰ��Ŀ����
        ///// </summary>
        ///// <returns></returns>
        //public string GetChannelNameFromURL()
        //{
        //    HttpContext Context = HttpContext.Current;
        //    if (Context.Request["id"] != null)
        //        return Context.Request["id"];
        //    else
        //    {
        //        string chID = string.Empty;
        //        string channelName = GetChannelUrlFromUrl(Context.Request.RawUrl, Context.Request.ApplicationPath);
        //        return channelName;
        //    }
        //}

        /// <summary>
        /// ͨ��URLȡ����ĿΨһ����
        /// 2.6���޸ģ���ĿΨһ���Ʊ��Ϊ FullUrl
        /// </summary>
        /// <param name="path"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        public string GetChannelUrlFromUrl(string path, string app)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si == null) return "";
            string ext = si.UrlFormat;
            if (ext == null || ext.Length == 0) ext = "html";

            if (path.LastIndexOf("?") > -1)
            {
                if (path.ToLower().IndexOf("channel=") > -1)
                {
                    path = path.Substring(path.ToLower().IndexOf("channel=") + 8);
                    if (path.IndexOf("&") > -1)
                        path = path.Remove(path.IndexOf("&"));
                }
                else
                    path = path.Remove(path.LastIndexOf("?"));
            }

            if (path.ToLower().EndsWith(".aspx") || path.ToLower().EndsWith("." + ext))
                path = path.Remove(path.LastIndexOf("/") + 1);

            if (!path.StartsWith("/")) path = "/" + path;
            string mathstr = @"(?:\/(\w|\s|(-)|(_))+((\/?))?)$";
            if (Regex.IsMatch(path, mathstr))
            {
                if (!app.StartsWith("/"))
                {
                    app = "/" + app;
                }
                if (!app.EndsWith("/"))
                {
                    app += "/";
                }
                path = path.Replace("//", "/");
                if (path.ToLower().StartsWith(app.ToLower()))
                {
                    path = path.Remove(0, app.Length);
                }
                //if (path.EndsWith("/"))
                //{
                //    path = path.Remove(path.Length - 1);
                //}

                //int lastSlash = path.LastIndexOf("/");
                //if (lastSlash > -1)
                //{
                //    path = path.Remove(0, lastSlash + 1);
                //}

                if (path.ToLower() == "go") path = string.Empty;
                if (!path.EndsWith("/")) path += "/";
                if (!path.StartsWith("/")) path = "/" + path;
                return path;
            }
            else
                return string.Empty;
        }


        #endregion
    }



    /// <summary>
    /// ��Ŀ��ѯ��
    /// </summary>
    public class ChannelQuery
    {
        public string ParentID { set; get; }
        public string Tag { set; get; }
        public string EnumState { set; get; }
        public ArticleStates State { set; get; }
        public bool IncludeAllSons { get; set; }
        public string ChannelFullUrl { get; set; }
        /// <summary>
        ///  �����ֶ��밴��Created|Asc,Clicks|Desc��ģʽ����
        /// </summary>
        public string OrderKeys { set; get; }

    }

}
