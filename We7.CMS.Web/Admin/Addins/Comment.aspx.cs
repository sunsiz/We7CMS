using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.Addins
{
	public partial class Comment : BasePage
	{


		protected int TitleMaxLength = 25;

		string ArticleID
		{
			get { return Request["aid"]; }
		}

		protected override void Initialize()
		{
			LoadComments();
		}

		protected void DataGridView_RowDataBound(object sender, GridViewRowEventArgs e)
		{

		}

		protected void Pager_Fired(object sender, EventArgs e)
		{
			LoadComments();
		}

		protected void DeleteBtn_Click(object sender, EventArgs e)
		{
			if (DemoSiteMessage) return;//�Ƿ�����ʾվ��
			List<string> ids = GetIDs();
			if (ids.Count < 1)
			{
				MessageLabel.Text = "��û��ѡ���κ�һ����¼";
				MessagePanel.Visible = true;
				return;
			}

			string aTitle = "";

			foreach (string id in ids)
			{
				Comments c = CommentsHelper.GetComment(id, new string[] { "Content" });
				CommentsHelper.DeleteComment(id);

				string con = c.Content;
				if (c.Content.Length > 10)
				{
					con = c.Content.Substring(0, 10);
				}

				aTitle += String.Format("{0};", con);
			}

			//��¼��־
			string content = string.Format("ɾ����{0}������:��{1}��", ids.Count.ToString(), aTitle);
			AddLog("���۹���", content);

			MessageLabel.Text = string.Format("���Ѿ��ɹ�ɾ��{0}����¼", ids.Count.ToString());
			MessagePanel.Visible = true;
			LoadComments();
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void StartButton_Click(object sender, EventArgs e)
		{
			SetState(1);
		}

		/// <summary>
		/// ��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void StopButton_Click(object sender, EventArgs e)
		{
			SetState(0);
		}

		/// <summary>
		/// ���������б�
		/// </summary>
		void LoadComments()
		{
			List<Comments> list = new List<Comments>();

			if (ArticleID == null)
			{
				CommentPager.RecorderCount = CommentsHelper.QueryAllCommentsCount();
				list = CommentsHelper.QueryAllComments(CommentPager.Begin, CommentPager.Count, null);

				SummaryLabel.Text = "����ȫ������";
			}
			else
			{
				CommentPager.RecorderCount = CommentsHelper.ArticleIDQueryCommentsCount(ArticleID);
				list = CommentsHelper.ArticleIDQueryComments(ArticleID, CommentPager.Begin, CommentPager.Count, null);

				try
				{
					SummaryLabel.Text = String.Format("�������£���{0}��������", ArticleHelper.GetArticle(ArticleID, new string[] { "Title" }).Title);
				}
				catch
				{
					string chID = ChannelHelper.GetChannelIDByOnlyName(ArticleID);
					Channel ch = ChannelHelper.GetChannel(chID, new string[] { "FullPath" });
					if (ch != null)
					{
						SummaryLabel.Text = String.Format("������Ŀ����{0}��������", ch.FullPath);
					}
				}
			}

			foreach (Comments c in list)
			{
				if (c.Content.Length > TitleMaxLength)
				{
					c.Content = String.Format("{0}...", c.Content.Substring(0, TitleMaxLength));
				}

				string[] fields = new string[] { "Title","ModelName" };
				string acticleTitle = "";
				if (!We7Helper.IsEmptyID(c.ArticleID))
				{
					//�����µ�����
					try
					{
						Article ac = ArticleHelper.GetArticle(c.ArticleID, fields);
						if (ac != null)
						{
							acticleTitle = String.Format("{1}:{0}", ac.Title, "System.Article" == ac.ModelName ? "����" : Model.Core.ModelHelper.GetModelInfoByName(ac.ModelName).Label);
						}
					}
					//��Ŀ�µ�����
					catch
					{
						string chID = ChannelHelper.GetChannelIDByOnlyName(c.ArticleID);
						Channel ch = ChannelHelper.GetChannel(chID, new string[] { "FullPath" });
						if (ch != null)
						{
							acticleTitle = String.Format("��Ŀ:{0}", ch.FullPath);
						}
					}
				}

				if (acticleTitle.Length > TitleMaxLength)
				{
					c.ArticleTitle = String.Format("{0}...", acticleTitle.Substring(0, TitleMaxLength));
				}
				else
				{
					c.ArticleTitle = acticleTitle;
				}
			}
			DataGridView.DataSource = list;
			DataGridView.DataBind();
		}

		/// <summary>
		/// ��ȡѡ�����۵�ID
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// ���û��������
		/// </summary>
		/// <param name="state"></param>
		void SetState(int state)
		{
			List<string> ids = GetIDs();

			if (ids.Count < 1)
			{
				MessageLabel.Text = "��û��ѡ���κ�һ����¼";
				MessagePanel.Visible = true;
				return;
			}

			string aTitle = "";
			string content = "";
			foreach (string id in ids)
			{
				Comments c = new Comments();
				c.ID = id;
				c.State = state;
				CommentsHelper.UpdateComments(c, new string[] { "ID", "State" });

				c = CommentsHelper.GetComment(id, new string[] { "Content" });

				string con = c.Content;
				if (c.Content.Length > 10)
				{
					con = c.Content.Substring(0, 10);
				}
				aTitle += String.Format("{0};", con);
			}

			MessageLabel.Text = string.Format("���Ѿ��ɹ�����{0}������", ids.Count.ToString());
			content = string.Format("������{0}������:��{1}��", ids.Count.ToString(), aTitle);
			if (state == 0)
			{
				MessageLabel.Text = string.Format("���Ѿ��ɹ�����{0}������", ids.Count.ToString());
				content = string.Format("������{0}������:��{1}��", ids.Count.ToString(), aTitle);
			}

			AddLog("���۹���", content);

			MessagePanel.Visible = true;
			LoadComments();
		}

	}
}
