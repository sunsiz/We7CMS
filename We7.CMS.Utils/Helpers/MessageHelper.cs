using System;
using System.Collections.Generic;
using System.Text;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// ����ҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.MessageHelper")]
    public class MessageHelper:BaseHelper
    {
        /// <summary>
        /// ��ȡȫ�����ŷ�����ʷ����
        /// </summary>
        /// <returns></returns>
        public int QueryAllMessageCount()
        {
            Order[] o = new Order[] { new Order("ID") };
            return Assistant.List<ShortMessage>(null, o).Count;
        }
        /// <summary>
        /// ����������ȡ���ŷ�����ʷ����
        /// </summary>
        /// <param name="c">Criteria</param>
        /// <returns></returns>
        public int QueryMessageCount(Criteria c)
        {
            return Assistant.Count<ShortMessage>(c);
        }
        /// <summary>
        /// �õ����ŷ�����ʷ
        /// </summary>
        /// <param name="logID">���ŷ�����ʷID</param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public ShortMessage GetMessage(string id, string[] fields)
        {
            ShortMessage l = new ShortMessage();
            Criteria cTmp = new Criteria(CriteriaType.Equals, "ID", id);
            List<ShortMessage> cList = Assistant.List<ShortMessage>(cTmp, null, 0, 0, fields);
            if (cList != null && cList.Count > 0)
            {
                l = cList[0];
            }
            return l;
        }
        /// <summary>
        /// ���������õ����ŷ�����ʷ�б�
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public List<ShortMessage> GetMessages(Criteria c)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            List<ShortMessage> ts = Assistant.List<ShortMessage>(c, o);
            return ts;
        }

        /// <summary>
        /// ���������õ����ŷ�����ʷ�б�
        /// </summary>
        /// <param name="tag">��ǩ</param>
        /// <returns></returns>
        public List<ShortMessage> GetMessagesByTag(string tag)
        {
            Criteria c =new Criteria(CriteriaType.None);
            if (tag != null && tag.Length > 0)
            {
                c.Criterias.Add(new Criteria(CriteriaType.Like, "Content", "%" + tag + "%"));
                c.Criterias.Add(new Criteria(CriteriaType.Like, "Description", "%" + tag + "%"));
            }
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            List<ShortMessage> ts = Assistant.List<ShortMessage>(c, o);
            return ts;
        }

        /// <summary>
        /// ���������õ���ҳ���ŷ�����ʷ�б�
        /// </summary>
        /// <param name="c"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ShortMessage> GetPagedMessages(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<ShortMessage>(c, o, from, count);
        }

        public int QueryMessageCount(MessageState state, string senderID, string receiverID)
        {
            Criteria c = CreateCriteriaFromState(state, senderID, receiverID);
            return Assistant.Count<ShortMessage>(c);
        }

        /// <summary>
        /// ����״̬ȡ�ö���Ϣ��ҳ�б�
        /// </summary>
        /// <param name="state"></param>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ShortMessage> GetPagedMessages(MessageState state,string senderID,string receiverID, int from, int count)
        {
            Criteria c = CreateCriteriaFromState(state, senderID, receiverID);
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<ShortMessage>(c, o, from, count);
        }

        Criteria CreateCriteriaFromState(MessageState state, string senderID, string receiverID)
        {
            Criteria c = null;
            if (state == MessageState.Inbox)
            {
                //TODO::�˴�����������
                Criteria sc = new Criteria(CriteriaType.Equals, "State", ((int)MessageState.NewMessage).ToString());

            //    sc.AddOr(CriteriaType.Equals, "State", ((int)state).ToString());
                c = new Criteria(CriteriaType.Equals, "ReceiverID", receiverID);
                c.Criterias.Add(sc);
            }
            else if (state == MessageState.Outbox)
            {
                c = new Criteria(CriteriaType.Equals, "State", ((int)state).ToString());
                c.Add(CriteriaType.Equals, "AccountID", senderID);
            }
            else if (state == MessageState.AllSMS)
            {
                Criteria sc = new Criteria(CriteriaType.Equals, "State", ((int)MessageState.Failure).ToString());
                sc.AddOr(CriteriaType.Equals, "State", ((int)MessageState.Success).ToString());
                c = new Criteria(CriteriaType.Equals, "AccountID", senderID);
                c.Criterias.Add(sc);
            }
            else
            {
                c = new Criteria(CriteriaType.Equals, "State", ((int)state).ToString());
                if (!string.IsNullOrEmpty(senderID))
                    c.Add(CriteriaType.Equals, "AccountID", senderID);
                if (!string.IsNullOrEmpty(receiverID))
                    c.Add(CriteriaType.Equals, "ReceiverID", receiverID);
            }
            return c;
        }

        /// <summary>
        /// �õ�ȫ�����ŷ�����ʷ�б�
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ShortMessage> GetPagedAllMessages(int from, int count)
        {
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return Assistant.List<ShortMessage>(null, o, from, count);
        }
        /// <summary>
        /// ��Ӷ��ŷ�����ʷ
        /// </summary>
        /// <param name="l">���ŷ�����ʷ</param>
        public void AddMessage(ShortMessage l)
        {
            l.Created = DateTime.Now;
            l.ID = We7Helper.CreateNewID();
            Assistant.Insert(l);
        }
        /// <summary>
        /// ɾ�����ŷ�����ʷ
        /// </summary>
        /// <param name="logID">���ŷ�����ʷID</param>
        public void DeleteMessage(string id)
        {
            ShortMessage l = new ShortMessage();
            l.ID = id;
            Assistant.Delete(l);
        }

        public void UpdateMessage(ShortMessage l, string[] fields)
        {
            Assistant.Update(l, fields);
        }

    }
}
