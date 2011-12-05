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
using We7.CMS.Common.Enum;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ChannelNodesGet : BasePage
    {
        static string TopTitle = "����Ŀ";
        static string TopSummary = "����Ŀ�µ�������Ŀ������Ϊ��һ����Ŀ��";

        /// <summary>
        /// ���ﲻ�ж�Ȩ�ޣ�Menu�в�������ҳ���ַ��������ajax���͹����������ܴ���
        /// </summary>
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        string ChannelID
        {
            get
            {
                if (Request["node"] != null && Request["node"].ToString()!="root")
                    return We7Helper.FormatToGUID(Request["node"]);
                else
                    return string.Empty;
            }
        }

        string DataType
        {
            get
            {
                return Request["type"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DataType == null)
                Response.Write(getChannelTree());
            else if (DataType == "detail")
                Response.Write(GetNodeContent());
            //Response.Flush();
            //Response.End();
        }

        string GetNodeContent()
        {
            if (!We7Helper.IsEmptyID(ChannelID))
            {
                string id = ChannelID;
                Channel ch = ChannelHelper.GetChannel(id, null);
                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=itemDetail style=line-height:220%><ul>");
                sb.Append("<li>��Ŀ���ƣ�" + ch.Name + "</li>");
                //sb.Append("<li>Ψһ���ƣ�" + ch.ChannelName + "</li>");
                sb.Append("<li>Url��ַ��<a href=\"" + ch.FullUrl + "\" target=\"_blank\"><u>" + ch.FullUrl + "</u></a> <a href='/go/rss.aspx?ChannelUrl=" + ch.FullUrl + "' title='RSS��ַ' target='_blank'><img src='/admin/images/icon_share.gif' /></a></li>");
                //sb.Append("<li>������" + ch.Alias + "</li>");
                //sb.Append(string.Format("<li>�ļ����Ŀ¼��<a href=Folder.aspx?folder={0}&filter=*&create=auto target=ifRightDetail><u>{0}</u></a></li>", string.Format("\\{0}\\{1}", Constants.ChannelPath, ch.ChannelFolder)));
                //if(TemplateHelper.GetTemplateName(ch.TemplateName)==string.Empty)
                //    sb.Append(string.Format("<li>����ģ�壺<u>{0}(ģ���ļ�������)</u></li>", ch.TemplateName));
                //else
                //    sb.Append(string.Format("<li>����ģ�壺<a href=Compose.aspx?file={0} target=_blank><u>{1}</u></a></li>", ch.TemplateName, TemplateHelper.GetTemplateName(ch.TemplateName)));

                //if (TemplateHelper.GetTemplateName(ch.DetailTemplate) == string.Empty)
                //    sb.Append(string.Format("<li>��ϸҳģ�壺<u>{0}(ģ���ļ�������)</u></li>", ch.DetailTemplate));
                //else
                //    sb.Append(string.Format("<li>��ϸҳģ�壺<a href=Compose.aspx?file={0} target=_blank><u>{1}</u></a></li>", ch.DetailTemplate, TemplateHelper.GetTemplateName(ch.DetailTemplate)));

                sb.Append("<li>��ȫ����" + ch.SecurityLevelText + "</li>");
                sb.Append("<li>���ͣ�" + ch.TypeText + "</li>");
                sb.Append("<li>״̬��" + ch.StateText + "</li>");
                sb.Append("</ul><div>");
                return sb.ToString();
            }
            else
                return string.Empty;
        }

        string getChannelTree()
        {
            string id = ChannelID;

            Channel c = new Channel();
            if (We7Helper.IsEmptyID(id))
            {
                if (id == We7Helper.EmptyWapGUID)
                {
                    c.ID = We7Helper.EmptyWapGUID;
                    c.ParentID = We7Helper.EmptyWapGUID;
                }
                else
                {
                    c.ID = We7Helper.EmptyGUID;
                    c.ParentID = We7Helper.EmptyGUID;
                }
                c.Name = TopTitle;
                c.Description = TopSummary;
            }
            else
            {
                c.ID = id;
                c = ChannelHelper.GetChannel(id, null);
            }

            List<Channel> channelList = ChannelHelper.GetChannels(c.ID);
            List<Channel> list = new List<Channel>();

            if (channelList != null)
            {
                foreach (Channel ch in channelList)
                {
                    if (We7Helper.IsEmptyID(AccountID) || HavePermission(ch.ID))
                    {
                        list.Add(ch);
                    }
                }
            }

            string TopStr = @"[";
            string BottomStr = "]";
            string MiddleStr = "";

            foreach (Channel ch in list)
            {
                List<Channel> listSon = ChannelHelper.GetChannels(ch.ID);

                string name=ch.Name;
                if(ch.Type==((int)TypeOfChannel.QuoteChannel).ToString())
                    name="[ר��]"+name;
                if (listSon!=null && listSon.Count > 0)   //���Ӳ˵�
                {
                    string strHaveSubMenu = @"text:'{0}',id:'{1}'";
                    MiddleStr = MiddleStr + "{" + string.Format(strHaveSubMenu, name, We7Helper.RemoveBrarket(ch.ID)) + "},";
                }
                else
                {
                    string strNotHaveSubMenu = @"text:'{0}',id:'{1}',leaf:true";
                    MiddleStr = MiddleStr + "{" + string.Format(strNotHaveSubMenu, name, We7Helper.RemoveBrarket(ch.ID)) + "},";
                }
            }

            if (MiddleStr.EndsWith(",")) MiddleStr = MiddleStr.Remove(MiddleStr.Length - 1);

            return TopStr + MiddleStr + BottomStr;

        }

        bool HavePermission(string chID)
        {
            List<string> channels = AccountHelper.GetObjectsByPermission(AccountID, "Channel.Input");
            return channels.Exists(delegate(string s) { return (s == chID) ? true : false; });            
        }

    }
}