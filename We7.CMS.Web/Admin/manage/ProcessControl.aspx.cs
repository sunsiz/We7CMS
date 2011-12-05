using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using We7.CMS.Controls;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class ProcessControl : BasePage
    {
        public string OwnerID
        {
            get
            { return AccountID; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadArticles();
            }

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //this.Master.SiteHeadTitle = SiteHeadTitle;
            //this.Master.TitleName = "������ת����";
        }

        #region ��ȡ����
        void LoadArticles()
        {
            ArticleQuery query = new ArticleQuery();
            query.State = ArticleStates.Checking;
            List<Article> articles = ArticleHelper.QueryArtilcesByAll(query);
            if (articles != null)
            { Pager.RecorderCount = articles.Count; }
            else
            {
                Pager.RecorderCount = 0;
            }
            if (Pager.Count < 0)
                Pager.PageIndex = 0;
            Pager.FreshMyself();
            if (Pager.Count <= 0)
            {
                DataGridView.DataSource = null;
                DataGridView.DataBind();
                return;
            }

            DataGridView.DataSource = articles.GetRange(Pager.Begin, Pager.Count);
            DataGridView.DataBind();
        }

        #endregion
        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//�Ƿ�����ʾվ��

            List<string> list = new List<string>();
            list = GetIDs();
            int count = 0;
            if (list.Count > 0)
            {
                foreach (string id in list)
                {
                    try
                    {
                        InsertArticleProcessHistory(id);
                        //ArticleProcessHelper.DelArticleProcess(id);
                        ArticleHelper.DeleteArticle(id);
                        count++;
                    }
                    catch
                    { }
                }
            }
            //��¼��־
            string content = string.Format(DateTime.Now.ToString() + "���Ѿ��ɹ�ɾ��{0}����¼", count.ToString());
            AddLog("���¼�ع���", content);

            string message = string.Format("���Ѿ��ɹ�ɾ��{0}����¼", count);
            Messages.ShowMessage(message);
            LoadArticles();
        }

        void InsertArticleProcessHistory(string id)
        {
            Processing ap = ProcessHelper.GetArticleProcess(id);
            ProcessHistory aph = new ProcessHistory();
            //aph.ID = ap.ID;
            aph.ObjectID = ap.ObjectID;
            aph.ToProcessState = "-1";
            aph.ProcessDirection = ap.ProcessDirection;
            aph.ProcessAccountID = ap.ProcessAccountID;
            aph.Remark = ap.Remark;
            aph.CreateDate = DateTime.Now;
            aph.UpdateDate = DateTime.Now;
            //ArticleProcessHistoryHelper.InsertArticleProcessHistory(aph);
        }
        /// <summary>
        /// ȡ����ת
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UntreadBtn_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//�Ƿ�����ʾվ��

            List<string> list = new List<string>();
            list = GetIDs();
            int count = 0;
            if (list.Count > 0)
            {
                foreach (string id in list)
                {
                    try
                    {
                        InsertArticleProcessHistory(id);
                        //ArticleProcessHelper.DelArticleProcess(id); 
                        ArticleHelper.UpdateArticleProcess(id, "-1",ArticleStates.Stopped);
                        count++;
                    }
                    catch
                    { }
                }
            }
            //��¼��־
            string content = string.Format(DateTime.Now.ToString() + "���Ѿ��ɹ���{0}����¼ȡ����ת", count.ToString());
            AddLog("���¼�ع���", content);

            string message = string.Format("���Ѿ��ɹ���{0}����¼ȡ����ת", count);
            Messages.ShowMessage(message);
            LoadArticles();
        }

        public string GetProcessState(string id)
        {
            Article a = ArticleHelper.GetArticle(id);
            Processing ap = ProcessHelper.GetArticleProcess(a);
            string processText = "�ݸ�";
            if (ap != null)
                processText = ap.ProcessDirectionText + ap.ProcessText;
            return processText;
        }

        public string GetChannelText(string id)
        {
            return ChannelHelper.GetChannelName(ArticleHelper.GetArticle(id).OwnerID);
        }

        public string GetTotalAudit(string ownerID)
        {
            Channel ch = ChannelHelper.GetChannel(ownerID, null);
            if (ch != null)
                return ch.ProcessLayerNOText;
            else
                return "";
        }

        List<string> GetIDs()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                if (((CheckBox)DataGridView.Rows[i].FindControl("chkItem")).Checked)
                {
                    list.Add(((Label)(DataGridView.Rows[i].FindControl("lblID"))).Text);
                }
            }
            return list;
        }
        protected void Pager_Fired(object sender, EventArgs e)
        {
            LoadArticles();
        }

    }
}
