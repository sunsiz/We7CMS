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
using System.IO;

using We7.CMS;
using We7.CMS.Controls;
using System.Xml;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Model.Core;
using We7.Framework;
using We7.Framework.Util;
using System.Collections.Generic;
using We7.CMS.Helpers;

namespace We7.CMS.Web.Admin.controls
{
	public partial class Channel_basic : BaseUserControl
	{
		static string TextNewColumn = "�ڡ�{0}����Ŀ�´����µ���Ŀ";
		static string TextCannotModifyColumn = "�����޸���Ŀ";
		static string TextModifyColumn = "�޸���Ŀ��{0}��";

		string ParentID
		{
			get
			{
				string pid = Request["pid"];
				if (pid == null || pid.Length == 0)
				{
					if (ParentIDTextBox.Text.Length == 0)
					{
						pid = We7Helper.EmptyGUID;
					}
					else
						pid = ParentIDTextBox.Text;
				}
				return pid;
			}
			set
			{
				ParentIDTextBox.Text = value;
			}
		}

		public string ChannelID
		{
			get { return Request["id"]; }
		}

		public string ModelRowDisplay { get; set; }

		public string ReturnUrlRowDisplay { get; set; }

		public string UrlTitle { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			InitControls();
			if (!IsPostBack)
			{

				if (ChannelID == null)
				{
					if (!We7Helper.IsEmptyID(ParentID))
					{
						Channel ch = ChannelHelper.GetChannel(ParentID, new string[] { "FullPath", "Name" });
						string pn = ch.FullPath;
						channelUrlLabel.InnerHtml = ch.FullUrl;
					}
					SaveButton2.Value = "������Ŀ";
					CreatedLabel.Text = "������������";
					ReturnUrlRowDisplay = "none";
				}
				else
				{
					if (We7Helper.IsEmptyID(ChannelID))
					{
						throw new CDException(TextCannotModifyColumn);
					}

					ShowInfomation();

					if (Request["saved"] != null && Request["saved"].ToString() == "1")
					{
						Messages.ShowMessage("��Ŀ�ѳɹ�������");
						Response.Write(string.Format("<script language=javascript>window.parent.freshNodeTree('{0}','{1}');</script>", We7Helper.RemoveBrarket(ChannelID), NameTextBox.Text));
					}
				}
				CheckChannelPermission();
			}
		}

		protected void SaveButton_ServerClick(object sender, EventArgs e)
		{
			if (DemoSiteMessage) return;//�Ƿ�����ʾվ��

			List<ChannelModuleMapping> mappings = HelperFactory.GetHelper<ChannelModuleHelper>().GetMappingByChannelID(ChannelID);
			if (mappings == null || mappings.Count <= 0)
				HelperFactory.GetHelper<ChannelModuleHelper>().CreateMapping(ChannelID, "{928b8e2f-b004-47bb-adef-2053bd8a0db0}", string.Empty);

			if (ChannelNameTextBox.Text.Contains("_"))
			{
				Messages.ShowMessage("����Ŀ��ʶ�����Ƿ���_��������������");
				return;
			}
			if (ChannelID == null || ChannelID == "" || ChannelNameHidden.Text.Trim() == "")
			{
				string channelName = CoverToPinyin.Convert(ChannelNameTextBox.Text.ToLower().Trim());
				if (!CheckName(channelName))
				{
					Messages.ShowError("�Ѵ��ڴ���Ŀ��ʶ��" + channelName + "������Ŀ��ʶ�����ظ��������±༭��");
					return;
				}
				ChannelNameHidden.Text = channelName;
			}
			SaveInformation();
		}

		void InitControls()
		{
			ContentTypeDropDownList.Items.Clear();
			SaveButton.Attributes["onclick"] = "return channelBasicCheck('" + this.ClientID + "');";
			SaveButton2.Attributes["onclick"] = "return channelBasicCheck('" + this.ClientID + "');";
			NameTextBox.Attributes["onblur"] = "autoFillUrlName('" + this.ClientID + "')";
			//ChannelNameTextBox.Attributes["onkeyup"] = "autoChangeFolderName('" + this.ClientID + "')";
			TypeDropDownList.Attributes["onchange"] = "autoChangeTypeList('" + this.ClientID + "')";

			//ChannelFolderLiteral.Text = string.Format("{0}\\", Constants.ChannelPath);

			ContentTypeDropDownList.DataSource = ModelHelper.GetContentModel(ModelType.ARTICLE);
			ContentTypeDropDownList.DataTextField = "Label";
			ContentTypeDropDownList.DataValueField = "Name";
			ContentTypeDropDownList.DataBind();
			ContentTypeDropDownList.Items.Insert(0, new ListItem("������Ϣ", ""));

		}

		#region ������Ϣ
		void SaveInformation()
		{
			if (DemoSiteMessage)
			{
				return;
			}

			Channel ch = new Channel();
			string channelname = "";
			string oldpath = "";
			if (!We7Helper.IsEmptyID(ChannelID))
			{
				ch = ChannelHelper.GetChannel(ChannelID, null);
				channelname = ch.Name;
				oldpath = ch.FullPath;
			}


			ch.Name = NameTextBox.Text;
			ch.Description = DescriptionTextBox.Text;
			ch.State = Convert.ToInt32(StateDropDownList.SelectedValue);

			//TODO::��һ�����µİ汾����ɾȥ��
			//ch.EnumState = StateMgr.StateProcess(ch.EnumState,
			//    EnumLibrary.Business.ChannelContentType, ModelHelper.GetModelValue(ContentTypeDropDownList.SelectedValue));

			ch.ParentID = ParentID;
			ch.Type = TypeDropDownList.SelectedValue;
			ch.ChannelName = ChannelNameHidden.Text;
			ch.ReturnUrl = ReturnUrlTextBox.Text;
			//ch.ChannelFolder = CreateFolder(ChannelNameHidden.Text);
			ch.ModelName = ContentTypeDropDownList.SelectedValue;

			if (We7Helper.IsEmptyID(ch.ID))
			{
				Channel channel = ChannelHelper.AddChanel(ch);
				//IDLabel.Text = ch.ID.ToString();
				//CreatedLabel.Text = ch.Created.ToString();

				//��¼��־
				string content = string.Format("�½���Ŀ��{0}��", NameTextBox.Text);
				AddLog("�½���Ŀ", content);

				string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
				rawurl = We7Helper.RemoveParamFromUrl(rawurl, "pid");
				rawurl = We7Helper.AddParamToUrl(rawurl, "id", ch.ID);
				Response.Redirect(rawurl);
			}
			else
			{
				//ch.FullPath = ch.FullPath.Replace(channelname, NameTextBox.Text.Trim());
				ChannelHelper.UpdateChannel(ch);
				if (channelname != NameTextBox.Text.Trim())
					ChannelHelper.UpdateChannelPathBatch(ch, oldpath);

				Messages.ShowMessage("��Ŀ��Ϣ�Ѿ��ɹ����¡�");
				ShowInfomation();

				Response.Write("<script language=javascript>window.parent.freshNodeText('" + ch.Name + "');</script>");
				//��¼��־
				string content = string.Format("�޸�����Ŀ��{0}������Ϣ", NameTextBox.Text);
				AddLog("�༭��Ŀ", content);
			}
		}
		#endregion

		bool CheckName(string channelName)
		{
			bool value = true;
			string url = channelUrlLabel.InnerText + channelName + "/";
			string id = ChannelHelper.GetChannelIDByFullUrl(url);
			return id == We7Helper.NotFoundID;
		}

		void ShowInfomation()
		{
			Channel ch = ChannelHelper.GetChannel(ChannelID, null);
			ParentID = ch.ParentID;

			IDLabel.Text = ch.ID;
			NameTextBox.Text = ch.Name;
			DescriptionTextBox.Text = ch.Description;
			CreatedLabel.Text = ch.Created.ToString();

			SetDropdownList(ContentTypeDropDownList, ch.ModelName);
			SetDropdownList(StateDropDownList, ch.State.ToString());
			SetDropdownList(TypeDropDownList, ch.Type);

			ChannelNameTextBox.Text = ch.ChannelName;
			channelUrlLabel.InnerHtml = string.Format(
				"<a href='{0}' target='_blank'>{0}</a> <a href='/go/rss.aspx?ChannelUrl={0}' title='RSS��ַ' target='_blank'><img src='/admin/images/icon_share.gif' /></a>", ch.FullUrl);
			//ChannelNameLabel.Text = ch.ChannelName;
			ChannelNameHidden.Text = ch.ChannelName;
			ChannelNameTextBox.Visible = false;
			//ChannelFolderLiteral.Text = string.Format("{0}\\", Constants.ChannelPath);

			//��ĿΨһ���Ʋ����޸�
			if (ch.ChannelName != null && ch.ChannelName != "")
			{
				ChannelNameTextBox.Enabled = false;
			}

			if ((TypeOfChannel)int.Parse(ch.Type) == TypeOfChannel.NormalChannel)
			{
				ModelRowDisplay = "";
				ReturnUrlRowDisplay = "none";
			}
			else if ((TypeOfChannel)int.Parse(ch.Type) == TypeOfChannel.ReturnChannel || (TypeOfChannel)int.Parse(ch.Type) == TypeOfChannel.RssOriginal)
			{
				ReturnUrlRowDisplay = "";
				ModelRowDisplay = "none";
				ReturnUrlTextBox.Text = ch.ReturnUrl;
				UrlTitle = "��ת��ַ";
				if ((TypeOfChannel)int.Parse(ch.Type) == TypeOfChannel.RssOriginal)
					UrlTitle = "RSSԴ��ַ";
			}
			else
			{
				ModelRowDisplay = "none";
				ReturnUrlRowDisplay = "none";
			}
		}

		/*
		string CreateFolder(string folderName)
		{
			string folderPath = Server.MapPath(Path.Combine("~/",Path.Combine(Constants.ChannelPath, folderName)));

			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
			return folderName;
		}
		*/
		void CheckChannelPermission()
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
			if (!canCreate)
			{
				Messages.ShowError("û��Ȩ�ޡ�");
			}
		}

		protected void SetDropdownList(DropDownList list, string value)
		{
			int i = 0;
			foreach (ListItem item in list.Items)
			{
				if (item.Value == value)
				{
					list.SelectedIndex = i;
					return;
				}
				i++;
			}
		}
	}
}