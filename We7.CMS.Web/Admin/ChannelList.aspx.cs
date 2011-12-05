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
using System.Text.RegularExpressions;

using Thinkment.Data;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class ChannelList : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
        static ArrayList MyChannelList;
        string KeyWord
        {
            get { return Request["keyword"]; }
        }
        public string ColumnID
        {
            get
            {
                if (Request["wap"] == null)
                {
                    string id = Request["id"];
                    if (id == null)
                    {
                        return We7Helper.EmptyGUID;
                    }
                    return id;
                }
                else
                {
                    string id = Request["id"];
                    if (id == null)
                    {
                        return We7Helper.EmptyWapGUID;
                    }
                    return id;
                }
            }
        }

        string ChannelType
        {
            get { return Request["type"]; }
        }

        protected override void Initialize()
        {
            string rawurl = Request.RawUrl;
            rawurl = We7Helper.RemoveParamFromUrl(rawurl, "keyword");
            string  qString = @"<label class=""hidden"" for=""user-search-input"">����{0}:</label>
                <input type=""text"" class=""search-input"" id=""KeyWord"" name=""KeyWord"" value=""""  onKeyPress=""javascript:KeyPressSearch('{1}',event);""  />
                <input type=""button"" value=""����"" class=""button"" id=""SearchButton""  onclick=""javascript:doSearch('{1}');""  />";
            qString = string.Format(qString, "��Ŀ", rawurl);
            SearchSimpleLiteral.Text = qString;

            MyChannelList = new ArrayList();
            List<Channel> allChannel = null;
            if (!string.IsNullOrEmpty(ChannelType) && ChannelType.ToString() == "link")
            {
                allChannel = ChannelHelper.GetAllLinkChannels();
            }
            else if (!string.IsNullOrEmpty(ChannelType) && ChannelType.ToString() == "article")
            {
                allChannel = ChannelHelper.GetChannelByModelName("");
            }
            else
            {
                Channel ch = ChannelHelper.GetChannel(ColumnID, null);
                string enumState = "";
                if (ch != null) enumState = ch.EnumState;
                allChannel = ChannelHelper.GetChannelsByType(enumState);
            }
            if (allChannel != null)
            {
                GetSubChannels(We7Helper.EmptyGUID, "", allChannel);
            }
            MyChannelList = FilterByKeyword();
            DetailGridView.DataSource = MyChannelList;
            DetailGridView.DataBind();
        }

        void ShowMessage(string s)
        {
            MessageLabel.Text = s;
        }

        /// <summary>
        /// �����Ŀ�µ���������Ŀ
        ///     v1.1 2011-1-11 moye
        /// </summary>
        /// <param name="parentID"></param>
        /// <param name="prefix"></param>
        protected void GetSubChannels(string parentID, string prefix, List<Channel> allChannel)
        {
            List<string> chids = new List<string>();
            if (!We7Helper.IsEmptyID(AccountID))
                chids = AccountHelper.GetObjectsByPermission(AccountID, "Channel.Article");

            foreach (Channel ch in allChannel)
            {
                if (parentID == We7Helper.EmptyGUID)
                {
                    Channel parent = allChannel.FindLast(p => p.ID == ch.ParentID);
                    if (parent != null)
                    {
                        DrawTreeMenu(parent, prefix, chids, ref allChannel);
                    }
                    else
                    {
                        DrawTreeMenu(ch, prefix, chids, ref allChannel);
                    }
                }
                else if (parentID == ch.ParentID)
                {
                    DrawTreeMenu(ch, prefix, chids, ref allChannel);
                }
            }
        }

        protected void DrawTreeMenu(Channel currentChannel, string prefix, List<string> chids, ref List<Channel> allChannel)
        {
            if (We7Helper.IsEmptyID(AccountID) || chids.Contains(currentChannel.ID))
            {
                currentChannel.FullPath = prefix + "<img src=\"/admin/images/filetype/folder.gif\" />&nbsp;" + currentChannel.Name;
                if(!MyChannelList.Contains(currentChannel))
                    MyChannelList.Add(currentChannel);
            }
            GetSubChannels(currentChannel.ID, prefix + "&nbsp;����&nbsp;", allChannel);
        }

        /// <summary>
        /// �ؼ��ʹ���
        /// </summary>
        /// <returns></returns>
        ArrayList FilterByKeyword()
        {
            if (KeyWord == null || KeyWord == "") return MyChannelList;

            ArrayList temp = new ArrayList();
            temp = (ArrayList)MyChannelList.Clone();

            for (int i = temp.Count - 1; i >= 0; i--)
            {
                Channel ws = (Channel)temp[i];
                if (!Like(ws.Name, KeyWord))
                {
                    temp.RemoveAt(i);
                }
            }
            return temp;
        }

        /// <summary>
        /// ģ��ƥ��,��strPattern(ƥ��ؼ���)��strText(Ҫ�Ƚϵ�ԭ�ַ���)ģ��ƥ��,�򷵻�true
        /// </summary>
        /// <param name="strText">Ҫ�Ƚϵ�ԭ�ַ���</param>
        /// <param name="strPattern">ƥ��ؼ���</param>
        /// <returns>true or false</returns>
        public static bool Like(string strText, string strPattern)
        {
            strText = strText.ToLower();
            strPattern = strPattern.ToLower();

            //�滻ͨ���*,?
            strPattern = strPattern.Replace("*", @"\w*");
            strPattern = strPattern.Replace("?", @"\w");

            string inputStrP = @"\w*" + strPattern + @"\w*";
            Regex myReg = new Regex(inputStrP);
            Match myMatch = myReg.Match(strText);
            if (myMatch.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
