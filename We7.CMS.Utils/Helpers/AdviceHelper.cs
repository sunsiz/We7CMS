using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;

using Thinkment.Data;
using System.Xml;
using System.IO;
using System.Web;
using System.Xml.Schema;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using We7.CMS.Config;
using OpenPOP;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using We7.Model.Core;
using System.Data;
using We7.CMS.Accounts;

namespace We7.CMS
{
    public interface IAdviceHelper
    {
        /// <summary>
        /// ��ӷ���
        /// </summary>
        /// <param name="advice">��������</param>
        void AddAdvice(AdviceInfo advice);

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id">����ID</param>
        void DeleteAdvice(string id);

        /// <summary>
        /// ɾ��ת����Ϣ
        /// </summary>
        /// <param name="id">ת����ϢID��</param>
        void DeleteTransfer(string id);

        /// <summary>
        /// ����ID��ȡ�÷���ʵ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AdviceInfo GetAdvice(string id);

        /// <summary>
        /// ȡ��ת����¼
        /// </summary>
        /// <param name="id">��¼ID</param>
        /// <returns></returns>
        AdviceTransfer GetTransfer(string id);

        /// <summary>
        /// ���·�����Ϣ
        /// </summary>
        /// <param name="advice">����ʵ��</param>
        void UpdateAdvice(AdviceInfo advice);

        /// <summary>
        /// ���·���״̬
        /// </summary>
        /// <param name="id">������ϢID</param>
        /// <param name="state">����״̬</param>
        void UpdateAdviceState(string id, int state);

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="id">ת������ID</param>
        /// <param name="typeID">��������ID</param>
        /// <param name="suggest">ת�����</param>
        void TransferAdvice(string id, string typeID, string suggest);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="content">������ԭ��</param>
        void RefuseAdvice(string id, string content);

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="id">����ID</param>
        void AcceptAdvice(string id);

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="id">����ID��</param>
        /// <param name="priority">�������ȼ�</param>
        /// <param name="isShow">�Ƿ�ǰ̨��ʾ</param>
        void AcceptAdvice(string id, int priority, bool isShow);

        /// <summary>
        /// ���÷������ȼ�
        /// </summary>
        /// <param name="id">����ID��</param>
        /// <param name="priority">�������ȼ�(0:��ͨ,1:�ذ�,2:�߰�)</param>
        void SetAdvicePriority(string id, int priority);

        /// <summary>
        /// ǰ̨��ʾ����
        /// </summary>
        /// <param name="id">����ID</param>
        void ShowAdvice(string id);

        /// <summary>
        /// ǰ̨���ط���
        /// </summary>
        /// <param name="id">����ID</param>
        void HideAdvice(string id);

        /// <summary>
        /// �����ö�
        /// </summary>
        /// <param name="id"></param>
        void SetTop(string id);

        /// <summary>
        /// ȡ���ö�
        /// </summary>
        /// <param name="id"></param>
        void CancelTop(string id);

        /// <summary>
        /// �ظ�������Ϣ
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="content">��������</param>
        void ReplyAdvice(string id, string content);

        /// <summary>
        /// �����û���ѯ�ᶨ���͵ķ�������
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <returns></returns>
        List<AdviceInfo> QueryAdvice(string typeID, int states);

        /// <summary>
        /// ��ѯָ���������ͣ�ָ���û��µģ�ָ��״̬�ļ�¼����
        /// </summary>
        /// <param name="typeID">��������</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <returns></returns>
        int QueryAdviceCount(string typeID, int state);

        /// <summary>
        /// ��ѯָ���������ͣ�ָ���û��µģ�ָ��״̬�ļ�¼����
        /// </summary>
        /// <param name="typeID">��������</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <param name="queryInfo">���Ӳ�ѯ��Ϣ</param>
        /// <returns></returns>
        int QueryAdviceCount(string typeID, int state, Dictionary<string, object> queryInfo);

        /// <summary>
        /// ��ҳ��ѯ���巴�����£�ָ���û���ָ��״̬����Ϣ
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��ѯ������</param>
        /// <returns></returns>
        List<AdviceInfo> QueryAdvice(string typeID, int state, int from, int count);

        /// <summary>
        /// ��ҳ��ѯ���巴�����£�ָ���û���ָ��״̬����Ϣ
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <param name="queryInfo">��ѯ��Ϣ</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��ѯ������</param>
        /// <returns></returns>
        List<AdviceInfo> QueryAdvice(string typeID, int state, Dictionary<string, object> queryInfo, int from, int count);

        /// <summary>
        /// ����ָ�����������µ�ת����Ϣ
        /// </summary>
        /// <param name="typeID">��������</param>
        /// <returns></returns>
        List<AdviceTransfer> QueryTransfers(string typeID, int from, int count);

        /// <summary>
        /// ��ѯָ���������µķ�������
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <returns></returns>
        List<AdviceTransfer> QueryTransferHistories(string adviceID);

        /// <summary>
        /// ���ݰ���ID��ѯ��������Ϣ��Ϣ
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        List<AdviceReplyInfo> QueryReplies(string adviceID);

        /// <summary>
        /// ���ݰ���ID��ѯ��������Ϣ��¼��
        /// </summary>
        /// <param name="adviceID"></param>
        /// <returns></returns>
        int QueryRepliesCount(string adviceID);

        /// <summary>
        /// ��ѯָ�����������µĻظ���Ϣ
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<AdviceReplyInfo> QueryReplies(string adviceID, int from, int count);

        /// <summary>
        /// ȡ��ָ�û�ӵ�е�ָ���������͵�Ȩ��
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <param name="userID">�û�ID</param>
        /// <returns></returns>
        List<string> GetPermissions(string typeID, string userID);

        /// <summary>
        /// ���ݷ�������ID��ȡ����Ȩ��Ϣ
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <returns></returns>
        List<AdviceAuth> GetAdviceAuth(string typeID);

        /// <summary>
        /// ���ݷ�������ID���Լ���Ȩ����ȡ����Ȩ��Ϣ
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <param name="authType">��Ȩ����(0���û���1����ɫ,2:����)</param>
        /// <returns></returns>
        List<AdviceAuth> GetAdviceAuth(string typeID, string authType);

        /// <summary>
        /// ���ݷ�������IDȡ�÷���������Ϣ
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <returns></returns>
        AdviceType GetAdviceType(string typeID);

        /// <summary>
        /// ���ص�ǰ������¼�µ���ط�������
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <returns></returns>
        List<AdviceType> GetRelatedAdviceTypes(string adviceID);


    }

    /// <summary>
    /// ��������
    /// </summary>
    public class AdviceFactory
    {
        /// <summary>
        /// ��������ҵ����
        /// </summary>
        /// <returns></returns>
        public static IAdviceHelper Create()
        {
            return HelperFactory.Instance.GetHelper<AdviceHelper2>();
        }
    }

    [Helper("We7.AdviceHelper2")]
    public class AdviceHelper2 : BaseHelper, IAdviceHelper, IObjectClickHelper
    {
        /// <summary>
        /// ��ӷ���
        /// </summary>
        /// <param name="advice">��������</param>
        public void AddAdvice(AdviceInfo advice)
        {
            if (String.IsNullOrEmpty(advice.ID))
            {
                advice.ID = We7Helper.CreateNewID();
            }

            if (advice.Created == default(DateTime))
            {
                advice.Created = DateTime.Now;
            }

            advice.SN = CreateSN();

            Assistant.Insert(advice);
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="id">����ID</param>
        public void DeleteAdvice(string id)
        {
            AdviceInfo adivce = GetAdvice(id);
            if (adivce != null)
            {
                Assistant.Delete(adivce);
            }
        }

        /// <summary>
        /// ɾ��ת����Ϣ
        /// </summary>
        /// <param name="id">ת����ϢID��</param>
        public void DeleteTransfer(string id)
        {
            AdviceTransfer tran = GetTransfer(id);
            if (tran != null)
            {
                Assistant.Delete(tran);
            }
        }

        /// <summary>
        /// ����ID��ȡ�÷���ʵ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdviceInfo GetAdvice(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceInfo> list = Assistant.List<AdviceInfo>(c, null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// ȡ��ת����¼
        /// </summary>
        /// <param name="id">��¼ID</param>
        /// <returns></returns>
        public AdviceTransfer GetTransfer(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceTransfer> list = Assistant.List<AdviceTransfer>(c, null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// ���·�����Ϣ
        /// </summary>
        /// <param name="advice">����ʵ��</param>
        public void UpdateAdvice(AdviceInfo advice)
        {
            Assistant.Update(advice);
        }

        /// <summary>
        /// ���·���״̬
        /// </summary>
        /// <param name="id">������ϢID</param>
        /// <param name="state">����״̬</param>
        public void UpdateAdviceState(string id, int state)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.State = state;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="content">������ԭ��</param>
        public void RefuseAdvice(string id, string content)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                IConnection ic = Assistant.GetConnections()[db];
                ic.IsTransaction = true;

                try
                {
                    AdviceReplyInfo reply = new AdviceReplyInfo();
                    reply.ID = We7Helper.CreateNewID();
                    reply.AdviceID = id;
                    reply.Content = content;
                    reply.Created = DateTime.Now;
                    reply.Title = "������ǰ��Ϣ";
                    reply.UserID = Security.CurrentAccountID;

                    Assistant.Insert(reply);

                    advice.State = 1;
                    Assistant.Update(advice);
                    ic.Commit();
                }
                catch (Exception ex)
                {
                    ic.Rollback();
                    throw ex;
                }
                finally
                {
                    ic.Dispose();
                }


            }
        }

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="id">ת������ID</param>
        /// <param name="typeID">��������ID</param>
        /// <param name="suggest">ת�����</param>
        public void TransferAdvice(string id, string typeID, string suggest)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                IConnection ic = Assistant.GetConnections()[db];
                ic.IsTransaction = true;

                try
                {

                    AdviceTransfer tran = new AdviceTransfer();
                    tran.ID = We7Helper.CreateNewID();
                    tran.State = 0;
                    tran.Suggest = suggest;
                    tran.FromTypeID = advice.TypeID;
                    tran.ToTypeID = typeID;
                    tran.Created = DateTime.Now;
                    tran.AdviceID = id;
                    tran.UserID = Security.CurrentAccountID;
                    Assistant.Insert(tran); //���ת����Ϣ

                    advice.State = 3;
                    advice.TypeID = typeID;
                    Assistant.Update(advice);//���·���״̬

                    ic.Commit();
                }
                catch (Exception ex)
                {
                    ic.Rollback();
                    throw ex;
                }
                finally
                {
                    ic.Dispose();
                }
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="id">����ID</param>
        public void AcceptAdvice(string id)
        {
            AcceptAdvice(id, 0, true);
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="id">����ID��</param>
        /// <param name="priority">�������ȼ�</param>
        /// <param name="isShow">�Ƿ�ǰ̨��ʾ</param>
        public void AcceptAdvice(string id, int priority, bool isShow)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.Priority = priority;
                advice.IsShow = isShow ? 1 : 0;
                advice.State = 2;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// ���÷������ȼ�
        /// </summary>
        /// <param name="id">����ID��</param>
        /// <param name="priority">�������ȼ�(0:��ͨ,1:�ذ�,2:�߰�)</param>
        public void SetAdvicePriority(string id, int priority)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.Priority = priority;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// ǰ̨��ʾ����
        /// </summary>
        /// <param name="id">����ID</param>
        public void ShowAdvice(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsShow = 1;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// ǰ̨���ط���
        /// </summary>
        /// <param name="id">����ID</param>
        public void HideAdvice(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsShow = 0;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// �����ö�
        /// </summary>
        /// <param name="id"></param>
        public void SetTop(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsTop = 1;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// ȡ���ö�
        /// </summary>
        /// <param name="id"></param>
        public void CancelTop(string id)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                advice.IsTop = 0;
                Assistant.Update(advice);
            }
        }

        /// <summary>
        /// �ظ�������Ϣ
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="content">��������</param>
        public void ReplyAdvice(string id, string content)
        {
            AdviceInfo advice = GetAdvice(id);
            if (advice != null)
            {
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                IConnection ic = Assistant.GetConnections()[db];
                ic.IsTransaction = true;

                try
                {
                    AdviceReplyInfo reply = new AdviceReplyInfo();
                    reply.ID = We7Helper.CreateNewID();
                    reply.AdviceID = id;
                    reply.Content = content;
                    reply.Created = DateTime.Now;
                    reply.Title = "������Ϣ";
                    reply.UserID = Security.CurrentAccountID;

                    Assistant.Insert(reply);

                    advice.State = 9;
                    Assistant.Update(advice);
                    ic.Commit();
                }
                catch (Exception ex)
                {
                    ic.Rollback();
                    throw ex;
                }
                finally
                {
                    ic.Dispose();
                }
            }
        }

        /// <summary>
        /// �����û���ѯ�ᶨ���͵ķ�������
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��)</param>
        /// <returns></returns>
        public List<AdviceInfo> QueryAdvice(string typeID, int state)
        {
            Order[] os = new Order[] { new Order("Priority", OrderMode.Desc), new Order("Created", OrderMode.Desc) };

            if (state == 3) //����ת�������
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) });
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                return c.Criterias.Count > 0 ? Assistant.List<AdviceInfo>(c, os) : new List<AdviceInfo>();
            }
            else
            {
                return Assistant.List<AdviceInfo>(CreateAdviceQueryCriteria(typeID, state), os);
            }
        }

        /// <summary>
        /// ��ѯָ���������ͣ�ָ���û��µģ�ָ��״̬�ļ�¼����
        /// </summary>
        /// <param name="typeID">��������</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <returns></returns>
        public int QueryAdviceCount(string typeID, int state)
        {
            if (state == 3) //����ת�������
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                return Assistant.Count<AdviceTransfer>(c);
            }
            else
            {
                return Assistant.Count<AdviceInfo>(CreateAdviceQueryCriteria(typeID, state));
            }
        }

        /// <summary>
        /// ��ѯָ���������ͣ�ָ���û��µģ�ָ��״̬�ļ�¼����
        /// </summary>
        /// <param name="typeID">��������</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <param name="queryInfo">���Ӳ�ѯ��Ϣ</param>
        /// <returns></returns>
        public int QueryAdviceCount(string typeID, int state, Dictionary<string, object> queryInfo)
        {
            if (state == 3) //����ת�������
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                AppendQueryInfo(c, queryInfo);
                return Assistant.Count<AdviceTransfer>(c);
            }
            else
            {
                Criteria c = CreateAdviceQueryCriteria(typeID, state);
                AppendQueryInfo(c, queryInfo);
                return Assistant.Count<AdviceInfo>(c);
            }
        }

        /// <summary>
        /// ��ҳ��ѯ���巴�����£�ָ���û���ָ��״̬����Ϣ
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��)</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��ѯ������</param>
        /// <returns></returns>
        public List<AdviceInfo> QueryAdvice(string typeID, int state, int from, int count)
        {
            Order[] os = new Order[] { new Order("Priority", OrderMode.Desc), new Order("Created", OrderMode.Desc) };

            if (state == 3) //����ת�������
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) }, from, count);
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                return c.Criterias.Count > 0 ? Assistant.List<AdviceInfo>(c, os) : new List<AdviceInfo>();
            }
            else
            {
                return Assistant.List<AdviceInfo>(CreateAdviceQueryCriteria(typeID, state), os, from, count);
            }
        }

        /// <summary>
        /// ��ҳ��ѯ���巴�����£�ָ���û���ָ��״̬����Ϣ
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <param name="queryInfo">��ѯ��Ϣ</param>
        /// <param name="from">��ʼ��¼</param>
        /// <param name="count">��ѯ������</param>
        /// <returns></returns>
        public List<AdviceInfo> QueryAdvice(string typeID, int state, Dictionary<string, object> queryInfo, int from, int count)
        {
            Order[] os = new Order[] { new Order("Priority", OrderMode.Desc), new Order("Created", OrderMode.Desc) };

            if (state == 3) //����ת�������
            {
                Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
                List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) }, from, count);
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                if (queryInfo != null)
                {
                    AppendQueryInfo(c, queryInfo);
                }
                return c.Criterias.Count > 0 ? Assistant.List<AdviceInfo>(c, os) : new List<AdviceInfo>();
            }
            else
            {
                Criteria c = CreateAdviceQueryCriteria(typeID, state);
                if (queryInfo != null)
                {
                    AppendQueryInfo(c, queryInfo);
                }
                return Assistant.List<AdviceInfo>(c, os, from, count);
            }
        }

        /// <summary>
        /// ����ָ�����������µ�ת����Ϣ
        /// </summary>
        /// <param name="typeID">��������</param>
        /// <returns></returns>
        public List<AdviceTransfer> QueryTransfers(string typeID, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "FromTypeID", typeID);
            List<AdviceTransfer> trans = Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created", OrderMode.Desc) }, from, count);
            if (trans != null && trans.Count > 0)
            {
                c = new Criteria(CriteriaType.None);
                c.Mode = CriteriaMode.Or;
                foreach (AdviceTransfer tran in trans)
                {
                    c.AddOr(CriteriaType.Equals, "ID", tran.AdviceID);
                }
                List<AdviceInfo> advices = Assistant.List<AdviceInfo>(c, null);
                foreach (AdviceTransfer tran in trans)
                {
                    foreach (AdviceInfo advice in advices)
                    {
                        if (tran.AdviceID == advice.ID)
                            tran.Advice = advice;
                    }
                }
            }
            return trans;
        }

        /// <summary>
        /// ��ѯָ���������µķ�������
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <returns></returns>
        public List<AdviceTransfer> QueryTransferHistories(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.List<AdviceTransfer>(c, new Order[] { new Order("Created") }) ?? new List<AdviceTransfer>();
        }

        /// <summary>
        /// ���ݰ���ID��ѯ��������Ϣ��Ϣ
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <returns></returns>
        public List<AdviceReplyInfo> QueryReplies(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.List<AdviceReplyInfo>(c, new Order[] { new Order("Created", OrderMode.Asc) });
        }

        /// <summary>
        /// ��ѯָ�������µļ�¼��
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <returns></returns>
        public int QueryRepliesCount(string adviceID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.Count<AdviceReplyInfo>(c);
        }

        /// <summary>
        /// ��ѯָ�����������µĻظ���Ϣ
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<AdviceReplyInfo> QueryReplies(string adviceID, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "AdviceID", adviceID);
            return Assistant.List<AdviceReplyInfo>(c, new Order[] { new Order("Created", OrderMode.Asc) }, from, count);
        }


        /// <summary>
        /// ȡ��ָ�û�ӵ�е�ָ���������͵�Ȩ��
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <param name="userID">�û�ID</param>
        /// <returns></returns>
        public List<string> GetPermissions(string typeID, string userID)
        {
            IAccountHelper accountHelper = AccountFactory.CreateInstance();
            if (userID == We7Helper.EmptyGUID)
            {
                return new List<string>() { "Advice.Accept", "Advice.Read", "Advice.Admin", "Advice.Handle" };
            }
            else
            {
                return accountHelper.GetPermissionContents(userID, typeID);
            }
        }

        /// <summary>
        /// ���ݷ�������ID��ȡ����Ȩ��Ϣ
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <returns></returns>
        public List<AdviceAuth> GetAdviceAuth(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            return Assistant.List<AdviceAuth>(c, null);
        }

        /// <summary>
        /// ���ݷ�������ID���Լ���Ȩ����ȡ����Ȩ��Ϣ
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <param name="authType">��Ȩ����(0���û���1����ɫ,2:����)</param>
        /// <returns></returns>
        public List<AdviceAuth> GetAdviceAuth(string typeID, string authType)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            c.Add(CriteriaType.Equals, "AuthType", authType);
            return Assistant.List<AdviceAuth>(c, null);
        }

        /// <summary>
        /// ���ݷ�������IDȡ�÷���������Ϣ
        /// </summary>
        /// <param name="typeID">��������ID</param>
        /// <returns></returns>
        public AdviceType GetAdviceType(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", typeID);
            List<AdviceType> list = Assistant.List<AdviceType>(c, null);
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// ���ص�ǰ������¼�µ���ط�������
        /// </summary>
        /// <param name="adviceID">����ID</param>
        /// <returns></returns>
        public List<AdviceType> GetRelatedAdviceTypes(string adviceID)
        {
            AdviceInfo advice = GetAdvice(adviceID);
            if (advice != null)
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ModelName", advice.ModelName);
                c.Add(CriteriaType.NotEquals, "ID", advice.TypeID);
                return Assistant.List<AdviceType>(c, new Order[] { new Order("CreateDate", OrderMode.Desc) });
            }
            return new List<AdviceType>();
        }


        /// <summary>
        /// ȡ��ת����Ϣ
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        private List<AdviceTransfer> GetTransfers(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            c.Add(CriteriaType.Equals, "FromTypeID", typeID);

            Criteria subC = new Criteria(CriteriaType.None);
            subC.Add(CriteriaType.Equals, "ToTypeID", typeID);
            subC.Add(CriteriaType.Equals, "State", 0);
            c.Criterias.Add(subC);

            return Assistant.List<AdviceTransfer>(c, null);
        }

        /// <summary>
        /// ����������ѯ����
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <param name="state">��������(0��δ����1��������2�������У�3��ת���У�9���Ѱ��,10��ȫ��)</param>
        /// <returns></returns>
        private Criteria CreateAdviceQueryCriteria(string typeID, int state)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "TypeID", typeID);

            if (state == 0) //δ������
            {
                c.Add(CriteriaType.Equals, "State", 0);
            }
            else if (state == 1) //������
            {
                c.Add(CriteriaType.Equals, "State", 1);
            }
            else if (state == 2) //�����е�����
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                subC.AddOr(CriteriaType.Equals, "State", 2);
                subC.AddOr(CriteriaType.Equals, "State", 3);
                c.Criterias.Add(subC);
            }
            else if (state == 9) //�Ѱ��
            {
                c.Add(CriteriaType.Equals, "State", 9);
            }
            else if (state == 10) //ȫ��
            {
            }

            return c;
        }

        private void AppendQueryInfo(Criteria c, Dictionary<string, object> queryInfo)
        {
            foreach (string key in queryInfo.Keys)
            {
                if (key.ToLower() == "title")
                {
                    c.Add(CriteriaType.Like, "Title", "%" + queryInfo[key] + "%");
                }

                if (key.ToLower() == "user")
                {
                    c.Add(CriteriaType.Like, "Name", "%" + queryInfo[key] + "%");
                }

                if (key.ToLower() == "relationid")
                {
                    c.Add(CriteriaType.Equals, "RelationID", "" + queryInfo[key] + "");
                }
            }
        }

        /// <summary>
        /// ��������SN
        /// </summary>
        private string CreateSN()
        {
            Criteria c = new Criteria(CriteriaType.MoreThanEquals, "Created", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            int count = Assistant.Count<AdviceInfo>(c);
            string sn = String.Empty;
            if (count > 0)
            {
                List<AdviceInfo> list = Assistant.List<AdviceInfo>(c, new Order[] { new Order("SN", OrderMode.Desc) }, 0, 1);
                if (list[0].SN.Length == 12)
                {
                    string s = list[0].SN.Substring(list[0].SN.Length - 4, 4);
                    long l;
                    if (long.TryParse(s, out l))
                    {

                        sn = DateTime.Now.ToString("yyyyMMdd") + (++l).ToString().PadLeft(4, '0');
                    }
                }
            }
            if (String.IsNullOrEmpty(sn))
            {
                sn = DateTime.Now.ToString("yyyyMMdd") + "0001";
            }
            return sn;
        }

        #region IObjectClickHelper ��Ա

        public void UpdateClicks(string modelName, string id, Dictionary<string, int> dictClickReport)
        {
            AdviceInfo targetObject = GetAdvice(id);
            if (targetObject != null)
            {
                targetObject.DayClicks = dictClickReport["�յ����"];
                targetObject.YesterdayClicks = dictClickReport["���յ����"];
                targetObject.WeekClicks = dictClickReport["�ܵ����"];
                targetObject.MonthClicks = dictClickReport["�µ����"];
                targetObject.QuarterClicks = dictClickReport["�������"];
                targetObject.YearClicks = dictClickReport["������"];
                targetObject.Clicks = dictClickReport["�ܵ����"];
                UpdateAdvice(targetObject);
            }
        }

        #endregion
    }




    #region ��ȥ��

    /// <summary>
    /// ����ҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.AdviceHelper")]
    public partial class AdviceHelper : BaseHelper
    {
        #region HelperFactory
        /// <summary>
        /// ҵ�����ֹ���
        /// </summary>
        private HelperFactory HelperFactory
        {
            get
            {
                return (HelperFactory)(HttpContext.Current.Application[HelperFactory.ApplicationID]);
            }
        }
        /// <summary>
        /// Ȩ��ҵ������
        /// </summary>
        protected IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }
        /// <summary>
        /// ��ѯͶ������ҵ������
        /// </summary>
        private AdviceTypeHelper AdviceTypeHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceTypeHelper>();
            }
        }
        /// <summary>
        /// �����ظ�ҵ������
        /// </summary>
        private AdviceReplyHelper AdviceReplyHelper
        {
            get
            {
                return HelperFactory.GetHelper<AdviceReplyHelper>();
            }
        }

        /// <summary>
        /// �����Ϣҵ������
        /// </summary>
        private ProcessingHelper ProcessingHelper
        {
            get
            {
                return HelperFactory.GetHelper<ProcessingHelper>();
            }
        }

        private ProcessHistoryHelper ProcessHistoryHelper
        {
            get
            {
                return HelperFactory.GetHelper<ProcessHistoryHelper>();
            }
        }

        #endregion

        #region Should Deleted
        /// <summary>
        /// XmlSwitcherMode ��ժҪ˵����
        /// </summary>
        public enum AdviceMode
        {
            /// <summary>
            /// ���ģʽ��
            /// </summary>
            Add,
            /// <summary>
            /// �鿴ģʽ��
            /// </summary>
            Browse,
            /// <summary>
            /// �޸�ģʽ��
            /// </summary>
            Modify
        }

        private AdviceMode appMode;
        /// <summary>
        /// ��ǰ��Ӧ��ģʽ����ӡ�������޸�
        /// </summary>
        public AdviceMode AppMode
        {
            get { return appMode; }
            set { appMode = value; }
        }

        private string dataXml;
        /// <summary>
        /// �����������޸�ģʽ��������Ӧ���Ǵ���ֵ��XML�ִ���
        /// ��������ģʽ��������Ӧ����ģ��XML�ִ���
        /// </summary>
        public string DataXml
        {
            get
            {
                return dataXml;
            }
            set
            {
                if (value == null || value == "")
                    throw new Exception("Ҫ�޸Ļ�鿴�� XML Ϊ�գ����飡");
                dataXml = value;
            }
        }

        private Hashtable ctrls;
        /// <summary>
        /// ���ɿؼ��ļ���
        /// </summary>
        public Hashtable Ctrls
        {
            get { return ctrls; }
            set { ctrls = value; }
        }

        private bool isXmlValid = true;


        public AdviceHelper()
        {
            ctrls = new Hashtable();
        }

        /// <summary>
        /// ����������ʼ������ؼ�
        /// </summary>
        public void InitControl(ref Control ctrl)
        {
            ctrl.Controls.Clear();
            //try
            //{
            //    ValidateXmlData(dataXml, false);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            // �����������
            Table tblContainer = new Table();
            tblContainer.Attributes["id"] = "personalForm";
            tblContainer.EnableViewState = true;
            ctrl.Controls.Add(tblContainer);
            //tblContainer.GridLines = GridLines.Both;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(dataXml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("fw", doc.DocumentElement.NamespaceURI);

            // ���ɿؼ�
            XmlNodeList itemNodes = doc.SelectNodes("/fw:Document/fw:Items/fw:Item", nsmgr);
            TableRow itemRow = null;
            TableCell itemCell = null;
            string itemLabel;
            ctrls.Clear();
            foreach (XmlNode itemNode in itemNodes)
            {
                itemRow = new TableRow();
                itemRow.EnableViewState = true;
                tblContainer.Rows.Add(itemRow);

                //����Label�ؼ�
                itemCell = new TableCell();
                itemCell.EnableViewState = true;
                itemCell.CssClass = "formTitle";
                //itemCell.Wrap = false;
                itemLabel = itemNode.SelectSingleNode("fw:Label", nsmgr).InnerText;
                itemCell.Text = itemLabel;
                itemRow.Cells.Add(itemCell);

                WebControl itemCtrl = null;
                XmlNode ctrlNode = itemNode.SelectSingleNode("fw:Control", nsmgr);
                string ctrlName = null;
                XmlNodeList valueNodes = itemNode.SelectNodes("fw:Content/fw:Value", nsmgr);

                //���ɵ���ؼ�
                ctrlName = ctrlNode.Attributes["Type"].Value;
                Assembly asm = Assembly.GetAssembly(typeof(WebControl));
                string nameSpace = typeof(WebControl).Namespace;
                string controlFullName = nameSpace + "." + ctrlName;
                itemCtrl = (WebControl)asm.CreateInstance(controlFullName);
                itemCtrl.EnableViewState = true;
                itemCtrl.ID = "MyCtrl" + ctrls.Count;
                //itemCtrl.Width = Unit.Pixel(250);

                //���� ListControl ����
                if (itemCtrl is ListControl)
                {
                    XmlNodeList listItemNodes = itemNode.SelectNodes("fw:Content/fw:ListItem", nsmgr);
                    ListControl listCtrl = (ListControl)itemCtrl;
                    listCtrl.EnableViewState = true;
                    foreach (XmlNode listItemNode in listItemNodes)
                    {
                        listCtrl.Items.Add(listItemNode.InnerText);
                    }
                }

                //������޸Ļ�鿴ģʽ����Ҫ��ListControl��ֵ
                if (appMode == AdviceMode.Modify || appMode == AdviceMode.Browse)
                {
                    if (itemCtrl is ListControl)
                    {
                        ListControl listCtrl = (ListControl)itemCtrl;
                        foreach (XmlNode valueNode in valueNodes)
                            listCtrl.Items.FindByValue(valueNode.InnerText).Selected = true;
                    }

                    else if (ctrlName == "TextBox")
                        ((TextBox)itemCtrl).Text = valueNodes[0].InnerText;
                }

                //����ÿ���ؼ�������
                PropertyInfo ctrlProp;
                foreach (XmlAttribute ctrlAttr in ctrlNode.Attributes)
                {
                    if ((ctrlProp = itemCtrl.GetType().GetProperty(ctrlAttr.Name)) == null)
                    {
                        if (ctrlAttr.Name != "Type")
                        {
                            itemCtrl.Attributes.Add(ctrlAttr.Name, ctrlAttr.Value);
                        }
                        continue;
                    }

                    // ��ö�����͸�ֵ
                    if (ctrlProp.PropertyType.IsEnum)
                    {
                        System.Enum enumPropValue = (System.Enum)System.Enum.Parse(ctrlProp.PropertyType, ctrlAttr.Value);
                        ctrlProp.SetValue(itemCtrl, enumPropValue, null);
                        continue;
                    }

                     //��������Ե����;��� ��Parse(string) ������������ Width��Height������丳ֵ
                    else if (ctrlProp.PropertyType.GetMethod("Parse", new Type[] { typeof(string) }) != null)
                    {
                        MethodInfo parseMethod = ctrlProp.PropertyType.GetMethod("Parse", new Type[] { typeof(string) });
                        object classValue = parseMethod.Invoke(null, new object[] { ctrlAttr.Value });
                        ctrlProp.SetValue(itemCtrl, classValue, null);
                        continue;
                    }

                    //��ֵ���͸�ֵ
                    else if (ctrlProp.PropertyType.IsValueType)
                    {
                        ctrlProp.SetValue(itemCtrl, Convert.ChangeType(ctrlAttr.Value, ctrlProp.PropertyType), null);
                        continue;
                    }
                }

                //����ǲ鿴ģʽ���ؼ���ֻ��
                if (appMode == AdviceMode.Browse)
                { itemCtrl.Enabled = false; }

                itemCell = new TableCell();
                itemCell.CssClass = "formValue";
                //itemCell.Wrap = false;
                itemRow.Cells.Add(itemCell);
                itemCell.Controls.Add(itemCtrl);

                //����Additional�ؼ�
                itemCell = new TableCell();
                //itemCell.Wrap = false;
                string itemLabel1 = itemNode.SelectSingleNode("fw:Additional", nsmgr).InnerText;
                itemCell.Text = itemLabel1;
                itemCell.CssClass = "formExtend";
                itemRow.Cells.Add(itemCell);

                // ���ղŴ�������Ŀ�Ŀؼ��洢�������Ա��ύʱ��ȡ�ؼ���ֵ
                //if (AppMode == AdviceMode.Add || appMode == AdviceMode.Modify)
                ctrls.Add(itemLabel, itemCtrl);
            }
        }

        /// <summary>
        /// ��ȡ���ɿؼ��ĸ�ֵ
        /// �˲���ע�����׽��
        /// </summary>
        /// <param name="xmlTemplate">��XMLģ���ִ�</param>
        /// <returns>���ش�ֵ��XMLģ��</returns>
        public string GetControlsValue(string xmlTemplate)
        {
            if (xmlTemplate.Trim() == string.Empty)
            {
                throw new Exception("ģ��XML�ִ�Ϊ�գ�");
            }

            //if (!ValidateXmlData(xmlTemplate,false))
            //{
            //    throw new Exception("ģ��XML�ִ�����XML���ʽ����");
            //}

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlTemplate);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("fw", doc.DocumentElement.NamespaceURI);

            foreach (string itemLabel in ctrls.Keys)
            {
                //�õ��� itemLabel ��Ӧ�� Content �ڵ�
                XmlNode labelNode = doc.SelectSingleNode("/fw:Document/fw:Items/fw:Item/fw:Label[.='" + itemLabel + "'] ", nsmgr);
                XmlNode contentNode = labelNode.SelectSingleNode("../fw:Content", nsmgr);
                WebControl itemCtrl = (WebControl)ctrls[itemLabel];

                //ɾ�� Content/Value �ڵ�
                XmlNodeList selectedItemNodes = contentNode.SelectNodes("fw:Value", nsmgr);
                foreach (XmlNode oldNode in selectedItemNodes)
                    contentNode.RemoveChild(oldNode);

                // ��ȡ�ؼ���ֵ���浽 XmlDocument ��
                if (itemCtrl is ListControl)
                { // �б�ؼ�
                    ListControl listCtrl = (ListControl)itemCtrl;
                    foreach (ListItem item in listCtrl.Items)
                    {
                        if (item.Selected)
                        {
                            XmlNode valueNode = doc.CreateNode(XmlNodeType.Element, "Value", doc.DocumentElement.NamespaceURI);
                            valueNode.InnerText = item.Value;
                            contentNode.PrependChild(valueNode);
                        }
                    }
                }
                else if (itemCtrl is TextBox)
                { // TextBox
                    TextBox tbCtrl = (TextBox)itemCtrl;
                    XmlNode valueNode = doc.CreateNode(XmlNodeType.Element, "Value", nsmgr.LookupNamespace("fw"));
                    valueNode.InnerText = tbCtrl.Text;
                    contentNode.PrependChild(valueNode);
                }

            }

            return doc.OuterXml;
        }

        #endregion

        /// <summary>
        /// ����ID��ȡһ������ʵ�����
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        /// <returns></returns>
        public Advice GetAdvice(string id)
        {
            Advice a = new Advice();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Advice> aList = Assistant.List<Advice>(c, null);
            if (aList != null && aList.Count > 0)
            {
                a = aList[0];
            }
            return a;
        }

        /// <summary>
        /// ���Ҵ˷������ʹ���
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        /// <returns></returns>
        public bool Exist(string id)
        {
            bool exist = false;
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            if (Assistant.Count<Advice>(c) > 0)
            {
                exist = true;
            }
            return exist;
        }

        /// <summary>
        /// �����ض�������ȡһ������ʵ�����
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        /// <param name="fields">�����ֶμ���</param>
        /// <returns></returns>
        public Advice GetAdvice(string id, string[] fields)
        {
            Advice a = new Advice();
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Advice> aList = Assistant.List<Advice>(c, null, 0, 0, fields);
            if (aList != null && aList.Count > 0)
            {
                a = aList[0];
            }
            return a;
        }

        /// <summary>
        /// ��ȡ��ѯͶ���б���ҳ
        /// </summary>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <param name="deptid">�ظ�ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(int from, int count, string deptid)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ReplyDepID", deptid);
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// ��ȡ��ѯͶ���б�
        /// </summary>
        /// <param name="c">��ѯ����</param>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(Criteria c, int from, int count)
        {
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// ����������ȡ��ѯͶ���б�
        /// </summary>
        /// <param name="c">��ѯ����</param>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <param name="typeID">����ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(Criteria c, int from, int count, string typeID)
        {
            if (c == null)
                c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            else
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeID));
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// ǰ�˿ؼ����ã�IsShow >0 
        /// </summary>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <param name="typeID">����ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvicesByType(int from, int count, string typeID)
        {
            Criteria c = new Criteria(CriteriaType.MoreThan, "IsShow", 0);
            if (typeID != null && typeID.Length > 0)
            {
                c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            }

            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);

        }

        /// <summary>
        /// ��ȡ��ѯͶ������
        /// </summary>
        /// <param name="c">��ѯ����</param>
        /// <returns></returns>
        public int GetAdviceCount(Criteria c)
        {
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        ///  �������ͻ�ȡ��ѯͶ������
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <returns></returns>
        public int GetAdviceCounts(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// ����������ȡ��ѯͶ������
        /// </summary>
        /// <param name="c">��ѯ����</param>
        /// <param name="deptid">�ظ�ID</param>
        /// <returns></returns>
        public int GetAdviceCount(Criteria c, string deptid)
        {
            if (c == null)
                c = new Criteria(CriteriaType.None);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "ReplyDepID", deptid));
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// ͨ������ID��ȡ��ѯͶ������
        /// </summary>
        /// <param name="typeid">����ID</param>
        /// <returns></returns>
        public int GetAdviceCountByType(string typeid)
        {
            Criteria c = new Criteria(CriteriaType.MoreThan, "IsShow", 0);
            if (typeid == null || typeid.Length == 0)
            {
                return Assistant.Count<Advice>(c);
            }
            else
            {
                c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeid));
                return Assistant.Count<Advice>(c);
            }
        }

        /// <summary>
        /// ��ȡ��ѯͶ���б�
        /// </summary>
        /// <param name="typeID">����ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvices(string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", typeID);
            return Assistant.List<Advice>(c, null);
        }

        /// <summary>
        /// ��ȡ��ѯͶ���б�
        /// </summary>
        /// <returns></returns>
        public List<Advice> GetAdvices()
        {
            return Assistant.List<Advice>(null, null);
        }

        /// <summary>
        /// ����һ����ѯͶ��
        /// </summary>
        /// <param name="a"></param>
        /// 
        public Advice AddAdvice(Advice a)
        {
            a.ID = String.IsNullOrEmpty(a.ID) ? We7Helper.CreateNewID() : a.ID;
            a.CreateDate = DateTime.Now;
            Assistant.Insert(a);
            return a;
        }

        /// <summary>
        /// ɾ��һ����ѯͶ��
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        public void DeleteAdvice(string id)
        {
            Advice a = GetAdvice(id);
            Assistant.Delete(a);
        }

        /// <summary>
        /// ɾ��һ����ѯͶ��
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteAdvice(List<string> ids)
        {
            foreach (string id in ids)
            {
                DeleteAdvice(id);
            }
        }

        /// <summary>
        /// ����һ����ѯͶ��
        /// </summary>
        /// <param name="advice"></param>
        /// <param name="fields">�����ֶμ���</param>
        public void UpdateAdvice(Advice advice, string[] fields)
        {
            if (advice != null)
                Assistant.Update(advice, fields);
        }

        /// <summary>
        /// ͨ���û�ID��ȡ��ѯͶ����
        /// </summary>
        /// <param name="accountID">�û�ID</param>
        /// <param name="typeID">����ID</param>
        /// <returns></returns>
        public int GetAdviceCountByAccountID(string accountID, string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "UserID", accountID);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeID));
            return Assistant.Count<Advice>(c);

        }

        /// <summary>
        ///  ͨ���û�ID��ȡ��ѯͶ���б�
        /// </summary>
        /// <param name="accountID">�û�ID</param>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <param name="typeID">����ID</param>
        /// <returns></returns>
        public List<Advice> GetAdvicesByAccountID(string accountID, int from, int count, string typeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "UserID", accountID);
            c.Criterias.Add(new Criteria(CriteriaType.Equals, "TypeID", typeID));
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            if (Assistant.Count<Advice>(c) > 0)
            {
                return Assistant.List<Advice>(c, o, from, count);
            }
            else
            {
                return null;
            }
        }

        ////<summary>
        ////����ID��ȡ��ǰ��������
        ////</summary>
        ////<param name="ID"></param>
        ////<returns>��������XML�ִ���δ�ҵ��򷵻�string.Empty</returns>
        public string GetAdviceModel(string Advice)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", Advice);
            if (Assistant.Count<Advice>(c) <= 0)
            {
                return string.Empty;
            }
            string xml = Assistant.List<Advice>(c, null)[0].ModelXml;
            if (xml != null)
            {
                return xml;
            }
            return string.Empty;
        }

        /// <summary>
        /// ��������XML
        /// </summary>
        /// <param name="xmlData">�������ݵ�XML�ִ�</param>
        /// <param name="id">����ʵ��ID</param>
        public void SaveArticleModel(string xmlData, string adviceTypeID, string title)
        {
            //Criteria c = new Criteria(CriteriaType.Equals, "ID", adviceTypeID);
            try
            {
                Criteria c = new Criteria(CriteriaType.Equals, "ID", adviceTypeID);
                if (Assistant.Count<AdviceType>(c) <= 0)
                {
                    throw new Exception("������Ϣ�����Ѿ������ڻ������Ѿ����ƻ���");
                }
                Advice advice = new Advice();
                advice.ID = We7Helper.CreateNewID();
                advice.Title = title;
                advice.TypeID = adviceTypeID;
                advice.Updated = DateTime.Now;
                advice.ModelXml = xmlData;
                Assistant.Insert(advice);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  ������ѯͶ�ߵ�sn
        /// </summary>
        /// <returns></returns>
        public long CreateArticleSN()
        {
            long maxSn = 0;
            Criteria c = new Criteria(CriteriaType.MoreThan, "SN", 0);
            long totalHave = Assistant.Count<Article>(c);
            long totalAll = Assistant.Count<Article>(null);
            if (totalAll > totalHave)
            {
                Order[] orders = new Order[] { new Order("Updated", OrderMode.Asc) };
                List<Advice> articles = Assistant.List<Advice>(null, orders);
                foreach (Advice a in articles)
                {
                    if (a.SN > maxSn) maxSn = a.SN;
                }

                foreach (Advice a in articles)
                {
                    if (a.SN <= 0)
                    {
                        a.SN = maxSn++;
                        UpdateAdvice(a, new string[] { "SN" });
                    }
                }
            }
            else
            {
                Order[] orders = new Order[] { new Order("SN", OrderMode.Desc) };
                if (Assistant.Count<Advice>(null) > 0)
                {
                    List<Advice> articles = Assistant.List<Advice>(null, orders, 0, 1);
                    if (articles.Count > 0)
                        maxSn = articles[0].SN;
                }
                else
                    maxSn = 0;
            }
            return maxSn + 1;
        }

        /// <summary>
        /// ���ݲ�ѯ���÷�������
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryAdviceCountByAll(AdviceQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// ���ݲ�ѯ������Criteria
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Criteria CreateCriteriaByAll(AdviceQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);

            if (query.State != 0)
            {
                c.Add(CriteriaType.Equals, "State", query.State);
            }
            else
            {
                c.Add(CriteriaType.NotEquals, "State", 999);//��999��ȡ����״ֵ̬����
            }

            if (query.State == (int)AdviceState.WaitHandle && !We7Helper.IsEmptyID(query.AccountID))
            {
                c.Add(CriteriaType.Equals, "ToOtherHandleUserID", query.AccountID);
                //Criteria cri = GetAdviceIDByReplyUserID(query.AccountID);
                //if (cri != null)
                //{
                //    c.Criterias.Add(cri);
                //}
            }
            if (query.NotState > 0)
            {
                c.Add(CriteriaType.NotEquals, "State", query.NotState);
            }
            if (query.NotEnumState > 0)
            {
                c.Add(CriteriaType.NotEquals, "EnumState", query.NotEnumState);
            }

            if (query.AdviceTypeID != null && query.AdviceTypeID != "")
            {
                c.Add(CriteriaType.Equals, "TypeID", query.AdviceTypeID);
            }

            if (query.AccountID != null && query.AccountID != "")
            {
                Criteria cr = CreateSubCriteriaByAccount(query.AccountID, query.AdviceTypeID);
                if (cr != null)
                    c.Criterias.Add(cr);
            }

            if (query.Title != null && query.Title != "")
            {
                c.Add(CriteriaType.Like, "Title", "%" + query.Title.Trim() + "%");
            }


            if (query.IsShow != 9999)
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;

                //ǿ����ʾ��ǰ̨
                string display = StateMgr.ConvertEnumToStr(EnumLibrary.AdviceDisplay.DisplayFront);
                Criteria displayC = new Criteria(CriteriaType.Equals, "EnumState", display);
                displayC.Adorn = Adorns.Substring;
                displayC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.AdviceDisplay];
                displayC.Length = EnumLibrary.PlaceLenth;
                subC.Criterias.Add(displayC);

                Criteria sub2c = new Criteria(CriteriaType.None);
                display = StateMgr.ConvertEnumToStr(EnumLibrary.AdviceDisplay.UnDisplayFront);
                displayC = new Criteria(CriteriaType.NotEquals, "EnumState", display);
                displayC.Adorn = Adorns.Substring;
                displayC.Start = EnumLibrary.Position[(int)EnumLibrary.Business.AdviceDisplay];
                displayC.Length = EnumLibrary.PlaceLenth;
                sub2c.Mode = CriteriaMode.And;
                sub2c.Add(CriteriaType.Equals, "IsShow", query.IsShow);
                sub2c.Criterias.Add(displayC);

                subC.Criterias.Add(sub2c);

                c.Criterias.Add(subC);
            }

            if (query.SN != 0 && query.SN > 0)
            {
                c.Add(CriteriaType.Equals, "SN", query.SN);
            }

            if (query.MyQueryPwd != null && query.MyQueryPwd != "")
            {
                c.Add(CriteriaType.Equals, "MyQueryPwd", query.MyQueryPwd);
            }

            if (query.StartCreated != null && query.StartCreated != DateTime.MinValue)
            {
                c.Add(CriteriaType.MoreThanEquals, "CreateDate", query.StartCreated);
                c.Add(CriteriaType.LessThan, "CreateDate", query.EndCreated);
            }
            if (query.AdviceInfoType != null && query.AdviceInfoType != "")
            {
                c.Add(CriteriaType.Equals, "AdviceInfoType", query.AdviceInfoType);
            }
            if (query.MustHandle > 0)
            {
                c.Add(CriteriaType.Equals, "MustHandle", query.MustHandle);
            }

            if (query.Name != null && query.Name != "")
            {
                c.Add(CriteriaType.Like, "Name", "%" + query.Name.Trim() + "%");
            }

            if (query.Phone != null && query.Phone != "")
            {
                c.Add(CriteriaType.Like, "Phone", "%" + query.Phone.Trim() + "%");
            }
            if (query.Email != null && query.Email != "")
            {
                c.Add(CriteriaType.Like, "Email", "%" + query.Email.Trim() + "%");
            }
            if (query.Fax != null && query.Fax != "")
            {
                c.Add(CriteriaType.Like, "Fax", "%" + query.Fax.Trim() + "%");
            }
            if (query.Address != null && query.Address != "")
            {
                c.Add(CriteriaType.Like, "Address", "%" + query.Address.Trim() + "%");
            }
            if (query.Content != null && query.Content != "")
            {
                c.Add(CriteriaType.Like, "Content", "%" + query.Content.Trim() + "%");
            }
            if (query.AdviceTag != null && query.AdviceTag != "")
            {
                c.Add(CriteriaType.Equals, "AdviceTag", query.AdviceTag);
            }

            return c;
        }

        /// <summary>
        /// �����˲�ͬȨ��ʱ��ӵ�еĲ�ͬ״̬�ķ�����Ϣ
        /// </summary>
        /// <param name="accountID">�û�ID</param>
        /// <param name="adviceTypeID"></param>
        /// <returns></returns>
        Criteria CreateSubCriteriaByAccount(string accountID, string adviceTypeID)
        {
            if (accountID != We7Helper.EmptyGUID)
            {
                List<string> allOwners = AccountHelper.GetRolesOfAccount(accountID);
                allOwners.Add(accountID);
                List<string> objList = new List<string>();
                objList.Add(adviceTypeID);
                List<Permission> permission = AccountHelper.GetPermissions(allOwners, objList);
                if (permission != null && permission.Count > 0)
                {
                    Criteria states = new Criteria(CriteriaType.None);
                    states.Mode = CriteriaMode.Or;

                    int stateInt = 0;
                    for (int i = 0; i < permission.Count; i++)
                    {
                        switch (permission[i].Content)
                        {
                            case "Advice.FirstAudit":
                                stateInt = (int)ProcessStates.FirstAudit;
                                states.Add(CriteriaType.Equals, "ProcessState", stateInt);
                                break;
                            case "Advice.SecondAudit":
                                stateInt = (int)ProcessStates.SecondAudit;
                                states.Add(CriteriaType.Equals, "ProcessState", stateInt);
                                break;
                            case "Advice.ThirdAudit":
                                stateInt = (int)ProcessStates.ThirdAudit;
                                states.Add(CriteriaType.Equals, "ProcessState", stateInt);
                                break;
                            case "Advice.Admin":
                                stateInt = (int)AdviceState.All;
                                states.Add(CriteriaType.Equals, "State", stateInt);
                                break;
                            case "Advice.Handle":
                                stateInt = (int)AdviceState.WaitHandle;
                                states.Add(CriteriaType.Equals, "State", stateInt);
                                break;
                            case "Advice.Accept":
                                stateInt = (int)AdviceState.WaitAccept;
                                states.Add(CriteriaType.Equals, "State", stateInt);
                                break;
                            default:
                                break;
                        }
                    }
                    return states;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// �����ѯͶ��
        /// </summary>
        /// <param name="oldList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ArrayList StatesAdd(ArrayList oldList, object value)
        {
            if (!oldList.Contains(value))
            {
                oldList.Add(value);
            }
            return oldList;
        }

        /// <summary>
        /// ͨ���û�ID��ȡ��ѯͶ��ID
        /// </summary>
        /// <param name="accountID">�û�ID</param>
        /// <returns></returns>
        public Criteria GetAdviceIDByReplyUserID(string accountID)
        {
            List<AdviceReply> adviceReplyList = AdviceReplyHelper.GetAdviceByUserID(accountID);
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            if (adviceReplyList != null)
            {
                for (int i = 0; i < adviceReplyList.Count; i++)
                {
                    c.Add(CriteriaType.Equals, "ID", adviceReplyList[i].AdviceID);
                }
                return c;
            }
            else
                return null;
        }

        /// <summary>
        /// ���ݲ�ѯ���ѯ��������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <returns></returns>
        public List<Advice> GetAdviceByQuery(AdviceQuery query, int from, int count)
        {
            Criteria c = CreateCriteriaByAll(query);
            Order[] o = new Order[] { new Order("CreateDate", OrderMode.Desc) };
            return Assistant.List<Advice>(c, o, from, count);
        }

        /// <summary>
        /// ���ݲ�ѯ��Ϣ��ȡ������
        /// </summary>
        /// <param name="query">��ѯ��Ϣ����</param>
        /// <returns>��ѯ���ķ�����Ϣ��Ŀ</returns>
        public int GetAdviceCount(AdviceQuery query)
        {
            Criteria c = CreateCriteriaByAll(query);
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        ///����ID����״̬ 
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        /// <param name="enumState"></param>
        public void UpdateAdviceType(string id, int state)
        {
            Advice a = GetAdvice(id);
            a.State = state;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "State", "Updated" });
        }

        /// <summary>
        /// ����ID��������״̬
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        /// <param name="processState"></param>
        public void UpdateAdviceProcessState(string id, string processState)
        {
            Advice a = GetAdvice(id);
            a.ProcessState = processState;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "ProcessState", "Updated" });
        }

        /// <summary>
        /// ����һ����ѯͶ�߼�¼
        /// </summary>
        /// <param name="id">����ʵ��ID</param>
        /// <param name="ProcessState">��ת״̬</param>
        /// <param name="state">����״̬</param>
        public void UpdateAdviceProcess(string id, string ProcessState, AdviceState state)
        {
            Advice a = GetAdvice(id);
            a.ProcessState = ProcessState;
            a.State = (int)state;
            a.Updated = DateTime.Now;
            Assistant.Update(a, new string[] { "Updated", "ProcessState", "State" });
        }

        /// <summary>
        /// ȡ��ģ���·���״̬�ķ���
        /// </summary>
        /// <param name="adviceIDlist"></param>
        /// <param name="state"></param>
        /// <param name="from">�ӵڼ�����¼��ʼ</param>
        /// <param name="count">���صļ�¼����</param>
        /// <returns></returns>
        public List<Advice> GetArticlesByAdviceTypeID(List<string> adviceIDlist, AdviceState state, int from, int count)
        {
            List<Advice> list = new List<Advice>();

            Criteria c = new Criteria(CriteriaType.Equals, "State", (int)state);
            if (adviceIDlist != null && adviceIDlist.Count > 0)
            {
                Criteria subc = new Criteria(CriteriaType.None);
                subc.Mode = CriteriaMode.Or;
                foreach (string adID in adviceIDlist)
                {
                    subc.AddOr(CriteriaType.Equals, "TypeID", adID);
                }
                c.Criterias.Add(subc);
            }
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            list = Assistant.List<Advice>(c, orders, from, count);
            return list;
        }

        /// <summary>
        /// ����ĿID������״̬��ѯ�б�
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="state"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Advice> GetList(string ownerId, AdviceState state, bool IsShow, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "OwnID", ownerId);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Advice>(c, orders, from, count);
        }

        /// <summary>
        /// ����ĿID������״̬��ѯ�б�
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="state"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Advice> GetListByType(string typeID, AdviceState state, bool IsShow, int from, int count)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "TypeID", typeID);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            Order[] orders = new Order[] { new Order("Updated", OrderMode.Desc) };
            return Assistant.List<Advice>(c, orders, from, count);
        }

        /// <summary>
        /// ����ĿID������״̬ͳ�Ƽ�¼
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int GetCount(string ownerId, AdviceState state, bool IsShow)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "OwnID", ownerId);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// ���ݷ�������ȡ�÷�����Ϣ
        /// </summary>
        /// <param name="typeId">��������ID</param>
        /// <param name="state">����״̬</param>
        /// <param name="IsShow">�Ƿ���ʾ</param>
        /// <returns></returns>
        public int GetCountByType(string typeId, AdviceState state, bool IsShow)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (state != AdviceState.All)
            {
                c.Add(CriteriaType.Equals, "State", (int)state);
            }
            c.Add(CriteriaType.Equals, "TypeID", typeId);
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            return Assistant.Count<Advice>(c);
        }

        /// <summary>
        /// ȡ�����о���ָ��Ȩ�޵��û�
        /// </summary>
        /// <param name="adviceTypeID">����ģ��</param>
        /// <param name="content">ָ��Ȩ��</param>
        /// <returns></returns>
        public List<string> GetAllReceivers(string adviceTypeID, string content)
        {
            List<string> accountIDs = new List<string>();
            List<Permission> ps = AccountHelper.GetPermissions(adviceTypeID, content);
            if (ps != null && ps.Count > 0)
            {
                foreach (Permission p in ps)
                {
                    if (p.OwnerType == 0 && !accountIDs.Contains(p.OwnerID))
                        accountIDs.Add(p.OwnerID);
                    else if (p.OwnerType == 1)
                    {
                        accountIDs.AddRange(AccountHelper.GetAccountsOfRole(p.OwnerID));
                    }
                }
            }
            return accountIDs;
        }


        /// <summary>
        /// ���������ۺϴ���
        /// </summary>
        /// <param name="ar"></param>
        /// <param name="a"></param>
        /// <param name="ap"></param>
        public void OperationAdviceInfo(AdviceReply ar, Advice a, Processing ap)
        {
            if (ar != null)
            {
                AdviceReplyHelper.AddAdviceReply(ar);
            }
            if (a != null)
            {
                ProcessingHelper.SaveAdviceFlowInfoToDB(a, ap);
            }
        }

        /// <summary>
        /// ��ȡ���еķ�����Ϣ����
        /// </summary>
        /// <returns></returns>
        public List<Advice> CreatedAdviceRate(string adTypeId)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "TypeID", adTypeId);

            ListField[] fields = new ListField[1];
            fields[0] = new ListField();
            fields[0].FieldName = "AdviceInfoType";
            fields[0].Adorn = Adorns.Distinct;

            Order[] orders = new Order[1];
            orders[0] = new Order();
            orders[0].Name = "AdviceInfoType";
            orders[0].Mode = OrderMode.Asc;

            List<Advice> list = Assistant.List<Advice>(c, orders, 0, 0, fields);
            return list;
        }

        /// <summary>
        /// ������ͳ��
        /// </summary>
        /// <param name="create">��ʼʱ��</param>
        /// <returns></returns>
        public int GetWorkingDays(DateTime create)
        {
            int days = 0;
            string fileName = HttpContext.Current.Server.MapPath("/Config/WorkingSet.config");
            NonWorkingDays workdays = NonWorkingDays.LoadNonWorkingDays(fileName);
            for (DateTime tmpTime = create; tmpTime < DateTime.Now; tmpTime = tmpTime.AddDays(1))
            {
                bool add = true;
                if (workdays.Weekends != null)
                {
                    foreach (DayOfWeek item in workdays.Weekends)
                    {
                        if (tmpTime.DayOfWeek == item)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if (workdays.NonworkingDays != null)
                {
                    foreach (ExceptionDays item in workdays.NonworkingDays)
                    {
                        if (tmpTime > item.StartTime && tmpTime <= item.EndTime)
                        {
                            add = false;
                            break;
                        }
                    }
                }

                if (workdays.WorkingDays != null)
                {
                    foreach (ExceptionDays item in workdays.WorkingDays)
                    {
                        if (tmpTime > item.StartTime && tmpTime <= item.EndTime)
                        {
                            add = true;
                            break;
                        }
                    }
                }

                if (add)
                {
                    days++;
                }
            }
            return days;
        }

        /// <summary>
        /// �ض�Page��ȡ���ؼ��Ƿ���Form�е���֤
        /// </summary>
        class MyPage : Page
        {
            public override void VerifyRenderingInServerForm(Control control) { }
        }

        /// <summary>
        /// ��ӻ��޸ķ�����ǩ
        /// </summary>
        /// <param name="editTagName"></param>
        /// <param name="newTagName"></param>
        /// <param name="fileName"></param>
        /// <param name="xPath"></param>
        public void InsertAdviceTagToAdviceTagXml(string editTagName, string newTagName, string fileName, String xPath)
        {

            Dictionary<string, string> tag = new Dictionary<string, string>();
            tag.Add("name", newTagName);
            if (string.IsNullOrEmpty(editTagName))
            {
                We7.Framework.Util.XmlHelper.CreateXmlNode(fileName, xPath, "tag", "", tag);
            }
            else
            {
                We7.Framework.Util.XmlHelper.CreateOrUpdateAttribute(fileName, xPath + "/tag[@name='" + editTagName + "']", tag);
            }


        }

    }

    #endregion
}

