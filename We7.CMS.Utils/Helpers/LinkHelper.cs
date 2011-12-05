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
    /// ����ҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.LinkHelper")]
    public class LinkHelper:BaseHelper
    {
        /// <summary>
        /// ��ȡȫ����������
        /// </summary>
        /// <returns></returns>
        public int QueryAllLinkCount()
        {
            Order[] o = new Order[] { new Order("ID") };
            return Assistant.List<Link>(null, o).Count;
        }
        /// <summary>
        /// ����������ȡ��������
        /// </summary>
        /// <param name="c">Criteria</param>
        /// <returns></returns>
        public int QueryLinkCount(Criteria c)
        {
            return Assistant.Count<Link>(c);
        }
        /// <summary>
        /// �õ�����
        /// </summary>
        /// <param name="logID">����ID</param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public Link GetLink(string id, string[] fields)
        {
            Link l = new Link();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<Link> cList = Assistant.List<Link>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                l = cList[0];
            }
            return l;
        }
        /// <summary>
        /// ���������õ������б�
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<Link> GetLinks(Criteria c)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            List<Link> ts = Assistant.List<Link>(c, o);
            return ts;
        }

        /// <summary>
        /// ���ݱ�ǩ��˳��ŵõ������б�
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public List<Link> GetLinksByTagAndOrder(string tag,string orderNumber)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (tag != null && tag.Length > 0)
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "Tag", tag));
            if (orderNumber != null && orderNumber == "-1")
            {
                Order[] o = new Order[] { new Order("OrderNumber", OrderMode.Desc) };
                List<Link> ts = Assistant.List<Link>(c, o);
                return ts;
            }
            else
            {
                Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
                List<Link> ts = Assistant.List<Link>(c, o);
                return ts;
            }

        }

        /// <summary>
        /// ���������õ������б�
        /// </summary>
        /// <param name="tag">��ǩ</param>
        /// <returns></returns>
        public List<Link> GetLinksByTag(string tag)
        {
            Criteria c =new Criteria(CriteriaType.None);
            if(tag!=null && tag.Length>0)
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "Tag", tag));
            
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            List<Link> ts = Assistant.List<Link>(c, o);
            return ts;
        }

        /// <summary>
        /// ���������õ���ҳ�����б�
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Link> GetPagedLinks(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<Link>(c, o, from, count);
        }
        /// <summary>
        /// �õ�ȫ�������б�
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Link> GetPagedAllLinks(int from, int count)
        {
            Order orderByNO = new Order("OrderNumber", OrderMode.Desc);
            Order orderByCreate = new Order("Created", OrderMode.Desc);
            Order[] o = new Order[] {orderByNO,orderByCreate };
            return Assistant.List<Link>(null, o, from, count);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="l">����</param>
        public void AddLink(Link l)
        {
            l.Created = DateTime.Now;
            l.ID = We7Helper.CreateNewID();
            Assistant.Insert(l);
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="logID">����ID</param>
        public void DeleteLink(string id)
        {
            Link l = new Link();
            l.ID = id;
            Assistant.Delete(l);
        }

        public void UpdateLink(Link l, string[] fields)
        {
            l.Created = DateTime.Now;
            Assistant.Update(l, fields);
        }
        /// <summary>
        /// ��ȡ��ѯ��������
        /// </summary>
        /// <returns></returns>
        public int QueryAllLinkCount(string title, string tag)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (title != null && title.Length > 0)
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "Title", title));
            if (tag != null && tag.Length > 0)
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "Tag", tag));
            Order[] o = new Order[] { new Order("ID") };
            return Assistant.List<Link>(c, o).Count;
        }
        /// <summary>
        /// �õ���ѯ�����б�
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Link> GetPagedAllLinks(int from, int count, string title, string tag)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (title != null && title.Length > 0)
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "Title", title));
            if (tag != null && tag.Length > 0)
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "Tag", tag));
            Order orderByNO = new Order("OrderNumber", OrderMode.Desc);
            Order orderByCreate = new Order("Created", OrderMode.Desc);
            Order[] o = new Order[] { orderByNO, orderByCreate };
            return Assistant.List<Link>(c, o, from, count);
        }

    }
}
