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
using We7.CMS.Web.Admin.controls;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework;
using System.Collections.Generic;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
	public partial class ChannelEdit : BasePage
	{
		protected override bool NeedAnPermission
		{
			get
			{
				return false;
			}
		}

		public string TabID
		{
			get { return Request["tab"]; }
		}

		public bool IsWap
		{
			get { return Request["wap"] != null; }
		}

		public string ChannelID
		{
			get { return Request["id"]; }
		}

		string ParentID
		{
			get
			{
				return Request["pid"];
			}
		}

		protected override MasterPageMode MasterPageIs
		{
			get
			{
				return MasterPageMode.NoMenu;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (CheckChannelPermission())
			{
				MenuTabLabel.Text = BuildNavString();
				PagePathLiteral.Text = BuildPagePath();
			}
			else
			{
				Response.Write("��û�й������Ŀ��Ȩ�ޣ�");
				Response.End();
			}
		}

		bool CheckChannelPermission()
		{
			bool canCreate = AccountID == We7Helper.EmptyGUID;

			if (!canCreate)
			{
				if (ChannelID != null)
				{
					List<string> ps = AccountHelper.GetPermissionContents(AccountID, ChannelID);
					if (ps.Contains("Channel.Admin"))
					{
						canCreate = true;
					}
				}
				if (ParentID != null)
				{
					List<string> ps = AccountHelper.GetPermissionContents(AccountID, ParentID);
					if (ps.Contains("Channel.Admin"))
					{
						canCreate = true;
					}
				}

			}

			return canCreate;
		}

		string BuildNavString()
		{
			string strActive = @"<LI class=TabIn id=tab{0} style='display:{2}'><A>{1}</A> </LI>";
			string strLink = @"<LI class=TabOut id=tab{0}  style='display:{2}'><A  href={3}>{1}</A> </LI>";
			int tab = 1;
			string tabString = "";
			string dispay = "";
			string rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "tab");
			rawurl = We7Helper.RemoveParamFromUrl(Request.RawUrl, "saved");

			//��¼������ʷ�����ղ�����ʷ����
			if (!We7Helper.IsEmptyID(ChannelID) && Session["$We7_Channel_Tab"] != null)
				tab = (int)Session["$We7_Channel_Tab"];

			if (TabID != null && We7Helper.IsNumber(TabID) && int.Parse(TabID) > 0)
				tab = int.Parse(TabID);

			if (tab == 1)
			{
				tabString += string.Format(strActive, 1, "������Ϣ", dispay);
				Control ctl = this.LoadControl("controls/Channel_basic.ascx");
				ContentHolder.Controls.Add(ctl);
			}
			else
				tabString += string.Format(strLink, 1, "������Ϣ", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "1"));

			if (!We7Helper.IsEmptyID(ChannelID))
			{
				if (tab == 2)
				{
					tabString += string.Format(strActive, 2, "ѡ��", dispay);
					Control ctl = this.LoadControl("controls/Channel_option.ascx");
					ContentHolder.Controls.Add(ctl);
				}
				else
					tabString += string.Format(strLink, 2, "ѡ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "2"));


				if (tab == 3)
				{
					tabString += string.Format(strActive, 3, "ģ��", dispay);
					Control ctl = this.LoadControl("controls/Channel_template.ascx");
					ContentHolder.Controls.Add(ctl);
				}
				else
					tabString += string.Format(strLink, 3, "ģ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "3"));


				if (tab == 4)
				{
					tabString += string.Format(strActive, 4, "��ǩ", dispay);
					Control ctl = this.LoadControl("controls/Channel_tag.ascx");
					ContentHolder.Controls.Add(ctl);
				}
				else
					tabString += string.Format(strLink, 4, "��ǩ", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "4"));


				if (tab == 5)
				{
					tabString += string.Format(strActive, 5, "Ȩ��", dispay);
					Control ctl = this.LoadControl("controls/Channel_authorize.ascx");
					ContentHolder.Controls.Add(ctl);
				}
				else
					tabString += string.Format(strLink, 5, "Ȩ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "5"));

				if (SiteConfigs.GetConfig().SiteGroupEnabled)
				{
					MoreEventArgs evenArgs = new MoreEventArgs();
					//ShareEvent shareEvent =  new ShareEvent();
					//shareEvent
					ShareEventFactory.Instance.OnLoadChannelShareConfig(tab, evenArgs);
					tabString += evenArgs.ReturnValue;
					Control c = evenArgs.ReturnObject as Control;
					if (c != null)
						ContentHolder.Controls.Add(c);
				}
				if (System.IO.File.Exists(Server.MapPath("/Plugins/IPStrategyPlugin/Plugin.xml")))
					if (tab == 7)
					{
						tabString += string.Format(strActive, 7, "�ɣй���", dispay);
						Control ctl = this.LoadControl("controls/StrategySet.ascx");
						if (ctl is StrategySet)
						{
							((StrategySet)ctl).Mode = StrategyMode.CHANNEL;
						}
						ContentHolder.Controls.Add(ctl);
					}
					else
						tabString += string.Format(strLink, 7, "IP����", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "7"));


				//if (tab == 8)
				//{
				//    tabString += string.Format(strActive, 8, "����ģ��", dispay);
				//    Control ctl = this.LoadControl("controls/Channel_Module.ascx");
				//    ContentHolder.Controls.Add(ctl);
				//}
				//else
				//    tabString += string.Format(strLink, 8, "����ģ��", dispay, We7Helper.AddParamToUrl(rawurl, "tab", "8"));

			}

			Session["$We7_Channel_Tab"] = tab;
			return tabString;
		}

		/// <summary>
		/// ������ǰλ�õ���
		/// </summary>
		/// <returns></returns>
		string BuildPagePath()
		{
			string pos = "��ʼ > <a >��վ����</a> >  <a >��Ŀ����</a> >  {0} > {1}";
			string channelFullPath = "";
			string action = "��������Ŀ";

			if (ChannelID != null)
			{
				Channel ch = ChannelHelper.GetChannel(ChannelID, null);
				if (ch != null)
				{
					channelFullPath = ChannelHelper.FullPathFormat(ch.FullPath, " > ");
					action = "��Ŀ�޸�";
				}
			}
			else if (ParentID != null)
			{
				Channel pch = ChannelHelper.GetChannel(ParentID, null);
				if (pch != null)
				{
					channelFullPath = ChannelHelper.FullPathFormat(pch.FullPath, " > ");
					action = "��Ŀ�޸�";
				}
			}

			return string.Format(pos, channelFullPath, action);

		}
	}
}
