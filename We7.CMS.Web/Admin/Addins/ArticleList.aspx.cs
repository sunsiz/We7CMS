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
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using System.Xml;
using We7.Framework;
using We7.Model.Core;
using System.IO;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ArticleList : BasePage
    {
        /// <summary>
        /// �Ƿ��ж��û�Ȩ��
        /// </summary>
        protected override bool NeedAnPermission
        {
            get
            {
                if (AccountHelper.GetAccount(AccountID, new string[] { "UserType" }).UserType == 0)
                {
                    return false;
                }
                return true;
            }
        }

        public string OwnerID
        {
            get
            {
                string oid = Request["oid"];
                return oid;
            }
        }

        protected string Tag
        {
            get
            {
                return Request["tag"];
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["notiframe"] != null)
                    return MasterPageMode.FullMenu;
                else
                    return MasterPageMode.NoMenu;
            }
        }

        void DoModel()
        {
            if (!String.IsNullOrEmpty(OwnerID))
            {
                Channel ch = ChannelHelper.GetChannel(OwnerID, null);
                if (ch != null && !String.IsNullOrEmpty(ch.ModelName) && String.Compare(ch.ModelName, "Article", true) != 0)
                {
                    string modelFileName = ModelHelper.GetModelPath(ch.ModelName);
                    if (File.Exists(modelFileName))
                    {
                        Response.Redirect(String.Format("~/Admin/Addins/ModelListNoMenu.aspx?notiframe=0&model={0}&oid={1}", ch.ModelName, ch.ID), true);
                    }
                    else
                    {
                        ch.ModelName = "";
                        ChannelHelper.UpdateChannel(ch);
                    }
                }
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DoChannelType();
            DoModel();
            if (Request["notiframe"] != null)
            {
                NameLabel.Text = "ȫ��";
                TitleH2.Visible = true;
                if (!We7Helper.IsEmptyID(OwnerID))
                {
                    string chTitle = ChannelHelper.GetChannel(OwnerID, new string[] { "Name" }).Name;
                    NameLabel.Text = string.Format("��Ŀ��{0}����", chTitle);
                }
                if (!string.IsNullOrEmpty(Tag))
                {
                    NameLabel.Text += string.Format("��ǩΪ��{0}��", Tag);
                }
                NameLabel.Text += "��Ϣ";

                ListTypeHyperLink.NavigateUrl = "articlelist.aspx" + Request.Url.Query;
                TreeTypeHyperLink.NavigateUrl = "articles.aspx" + Request.Url.Query;
            }
            else
            {
                if (!CheckChannelPermission())
                    Response.Write("��û��Ȩ�޹������Ŀ����Ϣ��");
            }
        }

        void DoChannelType()
        {
            string msg = "";
            Channel ch = ChannelHelper.GetChannel(OwnerID, null);
            if (ch != null)
            {
                switch ((TypeOfChannel)int.Parse(ch.Type))
                {
                    case TypeOfChannel.RssOriginal:
                        msg = "����ĿΪRSSԴ��Ϣչʾ�������Է�����Ϣ��";
                        break;
                    case TypeOfChannel.BlankChannel:
                        msg = "����ĿΪ�սڵ���Ŀ���������ڴ���Ŀ�·�����Ϣ��";
                        break;
                    case TypeOfChannel.ReturnChannel:
                        msg = "����Ŀ�Ѿ���ת������ĵ�ַ�� " + ch.ReturnUrl;
                        break;
                    default:
                        break;
                }
                if (msg != "")
                {
                    Response.Write(msg);
                    Response.End();
                }
            }
        }

        bool CheckChannelPermission()
        {
            bool canList = AccountID == We7Helper.EmptyGUID;

            if (!canList)
            {
                if (OwnerID != null)
                {
                    List<string> ps = AccountHelper.GetPermissionContents(AccountID, OwnerID);
                    if (ps.Contains("Channel.Article"))
                    {
                        canList = true;
                    }
                }
            }
            return canList;
        }
    }
}
