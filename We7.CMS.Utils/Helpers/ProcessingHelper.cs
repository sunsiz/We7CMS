using System;
using System.Collections.Generic;
using System.Text;

using Thinkment.Data;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Config;
using We7.Framework.Helper;
using System.Data;

namespace We7.CMS
{
    /// <summary>
    /// �����Ϣҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.ArticleProcessHelper")]
    public class ProcessingHelper : BaseHelper
    {
        #region helper
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)(System.Web.HttpContext.Current.Application[HelperFactory.ApplicationID]); }
        }

        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        protected AdviceHelper AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper>(); }
        }

        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }

        protected ChannelHelper ChannelHelper
        {
            get { return HelperFactory.GetHelper<ChannelHelper>(); }
        }

        protected ProcessHistoryHelper ProcessHistoryHelper
        {
            get { return HelperFactory.GetHelper<ProcessHistoryHelper>(); }
        }

        #endregion

        #region Advice ������ת����

        public string GetCurLayerNOAdviceText(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Advice> a = Assistant.List<Advice>(c, null);
            if (a.Count > 0)
            {
                Processing p = GetAdviceProcess(a[0]);
                string mysiteID = SiteConfigs.GetConfig().SiteID;

                //�Ƿ�վȺ
                bool siteGroup = SiteConfigs.GetConfig().SiteGroupEnabled;
                if (siteGroup)
                {
                    if (p.CurrentSiteID == mysiteID)
                        return p.CurLayerNOToAdvice;
                    else
                        return string.Empty;
                }
                else
                {
                    //ֱ�ӷ���
                    return p.CurLayerNOToAdvice;
                }
            }
            return string.Empty;
        }

        public ProcessStates GetAdviceNextProcess(ProcessAction action, Advice advice)
        {
            Processing ap = GetAdviceProcess(advice);
            return _GetNextProcessState(action, ap);
        }

        /// <summary>
        /// ���ݷ�����ȡ�����Ϣ
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Processing GetAdviceProcess(Advice a)
        {
            if (a == null)
                return null;
            else
            {
                Processing p = new Processing();
                if (string.IsNullOrEmpty(a.ProcessState))
                    p.CurLayerNO = "-1";
                else
                    p.CurLayerNO = a.ProcessState;

                p.ProcessDirection = a.ProcessDirection;
                p.ObjectID = a.ID;
                p.FromOtherSite = false;
                p.AdviceState = (AdviceState)a.State;
                if (We7Helper.IsNumber(a.ProcessState) && ((ProcessStates)int.Parse(a.ProcessState)) == ProcessStates.Unaudit)
                    p.AdviceState = AdviceState.WaitAccept;

                AdviceType type = AdviceTypeHelper.GetAdviceType(a.TypeID);
                if (type != null)
                {
                    if (!string.IsNullOrEmpty(type.ProcessEnd) && We7Helper.IsNumber(type.ProcessEnd))
                        p.ProcessEndAction = (ProcessEnding)(int.Parse(type.ProcessEnd));
                    p.ProcessTotalLayer = type.FlowSeries;
                }
                p = FillDataByFlowXml(p, (ProcessObject)a);
                return p;
            }
        }

        /// <summary>
        /// ����һ�������Ϣ����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ownID"></param>
        /// <returns></returns>
        public Processing CreateAdviceProcess(string id, string ownID)
        {
            Processing ap = new Processing();
            ap.ObjectID = id;
            ap.CurLayerNO = "1";
            ap.ProcessAccountID = ownID;
            ap.ProcessDirection = "1";
            ap.Remark = "";
            return ap;
        }

        /// <summary>
        /// ���淴��������Ϣ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="newProcess"></param>
        public void SaveAdviceFlowInfoToDB(Advice a, Processing newProcess)
        {
            InsertAdviceProcessHistory(newProcess, a);
            UpdateAdviceProcess(newProcess, a);
        }

        void InsertAdviceProcessHistory(Processing newProcess, Advice advice)
        {
            ProcessHistory aph = new ProcessHistory();
            if (newProcess != null)
            {
                aph.ObjectID = newProcess.ObjectID;
                if (string.IsNullOrEmpty(advice.ProcessState))
                    aph.FromProcessState = "-1";
                else
                    aph.FromProcessState = advice.ProcessState;

                aph.ToProcessState = newProcess.CurLayerNO;
                aph.ProcessDirection = newProcess.ProcessDirection;
                aph.ProcessAccountID = newProcess.ProcessAccountID;
                aph.TargetSites = newProcess.TargetSites;
                aph.Remark = newProcess.Remark;
                aph.CreateDate = DateTime.Now;
                aph.UpdateDate = DateTime.Now;
                aph.ItemNum = newProcess.ItemNum + 1;
                aph.ApproveName = newProcess.ApproveName;
                aph.ApproveTitle = newProcess.ApproveTitle;
                aph.ChannelID = advice.TypeID;
                aph.ChannelName = advice.TypeTitle;
                aph.SiteApiUrl = SiteConfigs.GetConfig().RootUrl;
                aph.SiteID = SiteConfigs.GetConfig().SiteID;
                aph.SiteName = SiteConfigs.GetConfig().SiteName;

                ProcessHistoryHelper.InsertAdviceProcessHistory(aph);
            }
        }

        /// <summary>
        /// ���·�����˽���
        /// </summary>
        /// <param name="ap"></param>
        public void UpdateAdviceProcess(Processing ap, Advice a)
        {
            a.ID = ap.ObjectID;
            a.ProcessState = ap.CurLayerNO;
            a.ProcessDirection = ap.ProcessDirection;
            a.ProcessSiteID = ap.TargetSiteID;
            a.State = (int)ap.AdviceState;
            Assistant.Update(a, new string[] { "ProcessState", "ProcessDirection", "ProcessSiteID", "State" });
        }

        #endregion

        #region Article ������ת����

        /// <summary>
        /// ͨ������ID����Ȩ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCurLayerNOText(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<Article> a = Assistant.List<Article>(c, null);
            if (a.Count > 0)
            {
                Processing p = GetArticleProcess(a[0]);
                string mysiteID = SiteConfigs.GetConfig().SiteID;
                if (p.CurrentSiteID == mysiteID || string.IsNullOrEmpty(p.CurrentSiteID) && string.IsNullOrEmpty(mysiteID))
                    return p.CurLayerNOToChannel;
                else
                    return "";
            }
            return "";
        }

        #endregion
        /// <summary>
        /// ��ȡ���������һ����״ֵ̬���¸��ڵ㣩
        /// </summary>
        /// <param name="action"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public ProcessStates GetNextProcessState(ProcessAction action, Article article)
        {
            Processing ap = GetArticleProcess(article);
            return _GetNextProcessState(action, ap);
        }

        ProcessStates _GetNextProcessState(ProcessAction action, Processing ap)
        {
            int channelProcess = ap.ProcessTotalLayer;
            ProcessStates state = ProcessStates.Unaudit;
            int currentProcess = 0;
            if (ap != null) { currentProcess = int.Parse(ap.CurLayerNO); }
            switch (action)
            {
                case ProcessAction.Next:
                    if (currentProcess == 8)
                    {
                        currentProcess = 1;
                    }
                    else
                    {
                        currentProcess = currentProcess + 1;
                    }
                    break;
                case ProcessAction.Previous:
                    currentProcess = currentProcess - 1;
                    break;
                case ProcessAction.Restart:
                    currentProcess = 0;
                    break;
                case ProcessAction.SubmitSite:
                    currentProcess = 1;
                    state = (ProcessStates)currentProcess;
                    return state;
                    break;
                default:
                    break;
            }
            if (currentProcess > channelProcess)
                currentProcess = (int)ProcessStates.EndAudit;
            if (currentProcess < 0) currentProcess = 0;

            state = (ProcessStates)currentProcess;
            return state;
        }

        /// <summary>
        /// �������¶��󣨼�flowxml����ȡ�����Ϣ
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public Processing GetArticleProcess(Article a)
        {
            if (a == null)
                return null;
            else
            {
                Processing p = new Processing();
                if (string.IsNullOrEmpty(a.ProcessState))
                    p.CurLayerNO = "0";
                else
                    p.CurLayerNO = a.ProcessState;

                p.ProcessDirection = a.ProcessDirection;
                p.ObjectID = a.ID;
                p.FromOtherSite = false;
                p.ArticleState = (ArticleStates)a.State;
                if (We7Helper.IsNumber(a.ProcessState) && ((ProcessStates)int.Parse(a.ProcessState)) == ProcessStates.Unaudit)
                    p.ArticleState = ArticleStates.Stopped;

                Channel ch = ChannelHelper.GetChannel(a.OwnerID, null);
                if (ch != null)
                {
                    if (!string.IsNullOrEmpty(ch.ProcessEnd) && We7Helper.IsNumber(ch.ProcessEnd))
                        p.ProcessEndAction = (ProcessEnding)(int.Parse(ch.ProcessEnd));
                    if (!string.IsNullOrEmpty(ch.ProcessLayerNO) && We7Helper.IsNumber(ch.ProcessLayerNO))
                        p.ProcessTotalLayer = int.Parse(ch.ProcessLayerNO);
                }

                p = FillDataByFlowXml(p, (ProcessObject)a);
                return p;
            }
        }

        /// <summary>
        /// ����FlowXml�ֶ����processing ����
        /// </summary>
        /// <param name="p"></param>
        /// <param name="flowXml"></param>
        /// <returns></returns>
        Processing FillDataByFlowXml(Processing p, ProcessObject a)
        {
            List<ProcessHistory> list = ProcessHistoryHelper.StrToList(a.FlowXml);
            string mysiteID = SiteConfigs.GetConfig().SiteID;
            p.CurrentSiteID = p.TargetSiteID = a.ProcessSiteID;
            if (list.Count > 0)
            {
                p.SourceSiteID = list[0].SiteID;
                p.SourceSiteName = list[0].SiteName;
                p.TargetSites = list[list.Count - 1].TargetSites;
                p.ItemNum = list[list.Count - 1].ItemNum;

                string lastSite = p.SourceSiteID;
                List<string> siteIDs = new List<string>();
                foreach (ProcessHistory ph in list)
                {
                    if (!string.IsNullOrEmpty(ph.SiteID) && ph.SiteID != mysiteID)
                        p.FromOtherSite = true;
                    if (!siteIDs.Contains(ph.SiteID))
                        siteIDs.Add(ph.SiteID);
                    if (ph.SiteID == p.CurrentSiteID)
                        p.CurrentSiteName = ph.SiteName;
                }
                if (string.IsNullOrEmpty(p.CurrentSiteName))
                    p.CurrentSiteName = p.TargetSites;

                if (siteIDs.Count > 1)
                {
                    p.PreviousSiteID = siteIDs[siteIDs.Count - 2];
                }
                else if (siteIDs.Count > 0)
                {
                    p.PreviousSiteID = siteIDs[0];
                }
                if (!string.IsNullOrEmpty(p.CurrentSiteID) && p.CurrentSiteID != mysiteID)
                {
                    //p.ArticleState = ArticleStates.Checking;
                    p.FromOtherSite = true;
                }
            }
            return p;
        }


        /// <summary>
        /// ����һ�������Ϣ����
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ownID"></param>
        /// <returns></returns>
        public Processing CreateArticleProcess(string id, string ownID)
        {
            Processing ap = new Processing();
            ap.ObjectID = id;
            ap.CurLayerNO = "0";
            ap.ProcessAccountID = ownID;
            ap.ProcessDirection = "1";
            ap.Remark = "";
            return ap;
        }

        /// <summary>
        /// ����ID�ж��Ƿ�����˼�¼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsExistProcess(string id)
        {
            Article a = ArticleHelper.GetArticle(id);
            if (a != null)
                return ((ArticleStates)a.State == ArticleStates.Checking);
            else
                return false;
        }

        /// <summary>
        /// ����������˽���
        /// </summary>
        /// <param name="ap"></param>
        public void UpdateArticleProcess(Processing ap, Article oldArticle)
        {
            Article a = new Article();
            a.ID = ap.ObjectID;
            a.State = oldArticle.State;
            a.ProcessState = ap.CurLayerNO;
            a.ProcessDirection = ap.ProcessDirection;
            a.ProcessSiteID = ap.TargetSiteID;
            UpdateModelProcess(ap, oldArticle);
            Assistant.Update(a, new string[] { "ProcessState", "ProcessDirection", "ProcessSiteID", "State" });
        }

        /// <summary>
        /// ���µ�������״̬
        /// </summary>
        /// <param name="ap"></param>
        /// <param name="oldArticle"></param>
        private void UpdateModelProcess(Processing ap, Article oldArticle)
        {
            if (!String.IsNullOrEmpty(oldArticle.ModelName) && !String.IsNullOrEmpty(oldArticle.TableName) && DbHelper.CheckTableExits(oldArticle.TableName))
            {
                DataTable dt = DbHelper.Query("SELECT * FROM [" + oldArticle.TableName + "] WHERE 1<>1");
                if (dt.Columns.Contains("ProcessState") && dt.Columns.Contains("ProcessDirection") && dt.Columns.Contains("ProcessSiteID") && dt.Columns.Contains("State"))
                {
                    DbHelper.ExecuteSql(String.Format("UPDATE [{0}] SET [ProcessState]='{1}',[ProcessDirection]='{2}',[ProcessSiteID]='{3}',[State]={4} WHERE [ID]='{5}'", oldArticle.TableName, ap.CurLayerNO, ap.ProcessDirection, ap.TargetSiteID, oldArticle.State, oldArticle.ID));
                }
            }
        }



        /// <summary>
        /// ͨ������ID��ȡ��˽�����Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Processing GetArticleProcess(string id)
        {
            Article a = ArticleHelper.GetArticle(id);
            if (a != null)
                return GetArticleProcess(a);
            else
                return null;
        }

        /// <summary>
        /// ������ת״̬��Ϣ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="newProcess"></param>
        public void SaveFlowInfoToDB(Article a, Processing newProcess)
        {
            InsertArticleProcessHistory(newProcess, a);
            UpdateArticleProcess(newProcess, a);
        }

        void InsertArticleProcessHistory(Processing newProcess, Article article)
        {
            ProcessHistory aph = new ProcessHistory();
            if (newProcess != null)
            {
                aph.ObjectID = newProcess.ObjectID;
                aph.FromProcessState = article.ProcessState;
                aph.ToProcessState = newProcess.CurLayerNO;
                aph.ProcessDirection = newProcess.ProcessDirection;
                aph.ProcessAccountID = newProcess.ProcessAccountID;
                aph.TargetSites = newProcess.TargetSites;
                aph.Remark = newProcess.Remark;
                aph.CreateDate = DateTime.Now;
                aph.UpdateDate = DateTime.Now;
                aph.ItemNum = newProcess.ItemNum + 1;
                aph.ApproveName = newProcess.ApproveName;
                aph.ApproveTitle = newProcess.ApproveTitle;
                aph.ChannelID = article.OwnerID;
                aph.ChannelName = article.ChannelName;
                aph.SiteApiUrl = SiteConfigs.GetConfig().RootUrl;
                aph.SiteID = SiteConfigs.GetConfig().SiteID;
                aph.SiteName = SiteConfigs.GetConfig().SiteName;

                ProcessHistoryHelper.InsertArticleProcessHistory(aph, article);
            }
        }

        /// <summary>
        /// ������������·��html����
        /// </summary>
        /// <param name="article"></param>
        /// <param name="targetSite"></param>
        /// <returns></returns>
        public string CreateArticleFlowPathHtml(Article article, string targetSite)
        {
            Processing p = GetArticleProcess(article);
            ProcessObject po = (ProcessObject)article;
            return CreateFlowPathHtml(po, p, targetSite); ;
        }

        /// <summary>
        /// ���ɷ�������·��html����
        /// </summary>
        /// <param name="advice"></param>
        /// <param name="targetSite"></param>
        /// <returns></returns>
        public string CreateAdviceFlowPathHtml(Advice advice, string targetSite)
        {
            Processing p = GetAdviceProcess(advice);
            ProcessObject po = (ProcessObject)advice;
            return CreateFlowPathHtml(po, p, targetSite); ;
        }

        /// <summary>
        /// ��������·��html����
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public string CreateFlowPathHtml(ProcessObject po, Processing p, string targetSite)
        {
            StringBuilder sb = new StringBuilder();

            List<ProcessHistory> hisList = ProcessHistoryHelper.StrToList(po.FlowXml);
            string mysiteID = SiteConfigs.GetConfig().SiteID;
            if (po.ShareSource != string.Empty)
                sb.Append(string.Format("����վ��[{0}]����", po.ShareSource));
            else
                sb.Append("���");
            sb.Append(" �� ");

            if (p.ProcessState == ProcessStates.FirstAudit && mysiteID == p.CurrentSiteID)
                sb.Append("<em>");
            if (p.ProcessTotalLayer <= 1)
                sb.Append("��ˣ�ͨ����");
            else
            {
                sb.Append("һ��");
                if (p.ProcessState == ProcessStates.FirstAudit && mysiteID == p.CurrentSiteID)
                    sb.Append("</em>");
                sb.Append(" �� ");
            }

            if (p.ProcessTotalLayer > 1)
            {
                if (p.ProcessState == ProcessStates.SecondAudit && mysiteID == p.CurrentSiteID)
                    sb.Append("<em>");
                sb.Append("����");
                if (p.ProcessTotalLayer == 2)
                    sb.Append("��ͨ����");
                else
                {
                    if (p.ProcessState == ProcessStates.SecondAudit && mysiteID == p.CurrentSiteID)
                        sb.Append("</em>");
                    sb.Append(" �� ");
                }
            }
            if (p.ProcessTotalLayer == 3)
            {
                sb.Append("����ͨ����");
            }

            if (p.ProcessEndAction == ProcessEnding.Start)
                sb.Append("ֱ�ӷ�������");
            else if (p.ProcessEndAction == ProcessEnding.Stop)
                sb.Append("����δ����");
            else if (p.ProcessEndAction == ProcessEnding.SubmitSite)
            {
                sb.Append(string.Format("��վ��[{0}]����", targetSite));
            }
            sb.Append("��");

            if ((int)p.ProcessState == p.ProcessTotalLayer && mysiteID == p.CurrentSiteID)
                sb.Append("</em>");

            return sb.ToString();
        }

    }
}
