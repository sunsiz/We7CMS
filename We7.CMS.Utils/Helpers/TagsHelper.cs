using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using Thinkment.Data;

namespace We7.CMS
{
    /// <summary>
    /// ����ҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.TagsHelper")]
    public class TagsHelper : BaseHelper
    {

        /// <summary>
        /// ����·��
        /// </summary>
        public string SystemTagsPath
        {
            get { return Path.Combine(Root, Constants.TagsPath); }
        }

        /// <summary>
        /// ��ȡһ������ʵ��
        /// </summary>
        /// <returns></returns>
        public TagsGroup GetTagsGroup()
        {
            TagsGroup ag = new TagsGroup();
            ag.FromFile(SystemTagsPath, "Tags.xml");
            return ag;
        }
        /// <summary>
        /// ����Ѿ����ڱ�ǩ
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns>"":������ .Length>0 ����</returns>
        public Tags GetTag(string tagName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Identifier", tagName);
            List<Tags> tags = Assistant.List<Tags>(c, null);
            if (tags != null && tags.Count > 0)
            {
                return tags[0];
            }
            return null;
        }


        /// <summary>
        /// if(exist) update frequency+1 else insert 
        /// </summary>
        /// <param name="tagName">tag's name</param>
        /// <returns>tag's id</returns>
        public string Add(string tagName)
        {
            Tags tag = GetTag(tagName);
            //exist or not
            if (tag != null && tag.ID.Length > 0)
            {
                //update frequency
                tag.Frequency += 1;
                Update(tag, new string[] { "Frequency" });

            }
            else
            {
                //add 
                tag = new Tags();
                tag.Identifier = tagName;
                tag.ID = Utils.CreateGUID();
                tag.Frequency = 1;
                Assistant.Insert(tag);
            }
            return tag.ID;
        }
        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="tag">entity</param>
        /// <param name="fields">update fields</param>
        public void Update(Tags tag, string[] fields)
        {
            Assistant.Update(tag, fields);
        }

        /// <summary>
        /// Get tags
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Tags> GetTags(int pageIndex, int pageSize)
        {
            int start = 0;
            int itemCount = 0;
            int totalCount = GetCount(null);
            BuidlPagerParam(totalCount, pageSize, ref pageIndex, out start, out itemCount);
            if (itemCount > 0)
            {
                Criteria c = new Criteria();

                return Assistant.List<Tags>(c, new Order[] { new Order("Frequency", OrderMode.Desc), new Order("ID", OrderMode.Desc) }, start, itemCount);
            }
            return null;

        }

        /// <summary>
        /// Get Tags Count
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <returns></returns>
        public int GetCount(QueryEntity queryEntity)
        {
            Criteria c = new Criteria();
            if (queryEntity != null)
            {
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
            }
            return Assistant.Count<Tags>(c);
        }
        /// <summary>
        /// ������ҳ����
        /// </summary>
        /// <param name="recordcount">�ܼ�¼��</param>
        /// <param name="pagesize">ҳ��¼��</param>
        /// <param name="pageindex">��ǰҳ��¼</param>
        /// <param name="startindex">��ʼ��¼�к�</param>
        /// <param name="itemscount">��ǰ��ҳ��¼��</param>
        protected void BuidlPagerParam(int recordcount, int pagesize, ref int pageindex, out int startindex, out int itemscount)
        {
            if (pagesize <= 0)
                pagesize = 1;
            if (pageindex <= 0)
                pageindex = 1;

            int totalpagecount = recordcount / pagesize;
            if (recordcount % pagesize != 0)
                totalpagecount++;

            if (pageindex > totalpagecount)
            {
                itemscount = 0;
            }
            if (pageindex < totalpagecount)
            {
                itemscount = pagesize;
            }
            else if (pageindex == totalpagecount)
            {
                itemscount = recordcount - (pageindex - 1) * pagesize;
            }
            else
            {
                itemscount = 0;
            }
            startindex = (pageindex - 1) * pagesize;
        }
    }
}
