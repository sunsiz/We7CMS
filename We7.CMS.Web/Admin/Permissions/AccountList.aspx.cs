using System;
using System.Text;
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

using We7.CMS.Controls;
using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common.Enum;
using We7.Framework.Cache;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class AccountList : BasePage
    {

        string departmentID;
        string DepartmentID
        {
            get
            {
                string id;

                id = Request["id"];
                if (id == null)
                {
                    id = We7Helper.EmptyGUID;
                }
                return id;
            }
            set
            {
                departmentID = value;
            }
        }

        /// <summary>
        /// ��ǰ��������
        /// </summary>
        public OwnerRank CurrentState
        {
            get
            {
                OwnerRank s = OwnerRank.All;
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                        s =(OwnerRank)int.Parse(Request["state"].ToString());
                }
                return s;
            }
        }

        string Keyword
        {
            get
            {
                return Request["keyword"];
            }
        }

        private int _resultsPageNumber = 1;
        /// <summary>
        /// ��ǰҳ
        /// </summary>
        public int PageNumber
        {
            get 
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber; 
            }
        }

        private string siteID;
        /// <summary>
        /// վ��ID
        /// </summary>
        public string SiteID
        {
            get
            {
                if (string.IsNullOrEmpty(siteID))
                {
                     siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
                }
                return siteID;
            }
        }

        protected override void Initialize()
        {
            RefreshHyperLink.NavigateUrl = String.Format("AccountList.aspx?id={0}", DepartmentID);
            //NewDepartmentHyperLink.NavigateUrl = String.Format("DepartmentDetail.aspx?pid={0}", DepartmentID);
            NewUserHyperLink.NavigateUrl = String.Format("AccountEdit.aspx?d={0}", DepartmentID);

            FullPathLabel.Text = BuildPagePath();
            StateLiteral.Text = BuildStateLinks();
            BindData();
        }


        void BindData()
        {
            List<ViewItem> items = new List<ViewItem>();
            List<Department> dts = null;
            List<Account> acts = null;
           

            AccountQuery aq = new AccountQuery();
            aq.KeyWord = Keyword;
            aq.SiteID = SiteID;
            aq.UserType = (int)CurrentState;

            UPager.PageIndex = PageNumber;
            UPager.ItemCount = AccountHelper.QueryAccountCountByQuery(aq);
            UPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl, Keys.QRYSTR_PAGEINDEX, "{0}");
            UPager.PrefixText = "�� " + UPager.MaxPages + "  ҳ ��   �� " + UPager.PageIndex + "  ҳ �� ";

            acts = AccountHelper.QueryAccountsByQuery(aq, UPager.Begin - 1, UPager.Count,
                new string[] { "ID", "LoginName", "Email", "CreatedNoteTime", "EmailValidate", "ModelState", "ModelName", "State","Created","UserType","Department" });


            if (acts != null)
            {
                foreach (Account act in acts)
                {
                    ViewItem vi = new ViewItem();
                    vi.Text = "<b>" + act.LoginName + "</b> " + act.LastName + "";
                    vi.Summary = act.Department;
                    vi.Mode = "User";
                    vi.State = act.TypeText;
                    vi.Url = String.Format("AccountEdit.aspx?id={0}", act.ID);
                    vi.DeleteUrl = String.Format("javascript:DeleteConfirm('{0}','{1}','Account');", act.ID, act.LoginName);
                    vi.EditUrl = String.Format("AccountEdit.aspx?id={0}", act.ID);
                    string mng = @"<a href=""AccountEdit.aspx?id={0}&tab=2"">
                            ��ɫ����</a> 
                        <a href=""AccountEdit.aspx?id={0}&tab=6"">
                            ģ��Ȩ��</a>    ";
                    vi.ManageLinks = string.Format(mng, act.ID);
                    vi.ID = act.ID;
                    vi.RegisterDate = act.Created.ToLongDateString();
                    if (DepartmentID != We7Helper.EmptyGUID)
                    {
                        if (act.DepartmentID == DepartmentID)
                        {
                            items.Add(vi);
                        }
                    }
                    else
                    {
                        if (act.DepartmentID == We7Helper.EmptyGUID)
                        {
                            items.Add(vi);
                        }
                    }
                }
            }
            AccountsGridView.DataSource = items;
            AccountsGridView.DataBind();
        }


        protected void DeleteDepartmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (DemoSiteMessage) return;//�Ƿ�����ʾվ��
                string dptID = IDTextBox.Text;
                string dptName = NameTextBox.Text;

                Department dpt = AccountHelper.GetDepartment(dptID, new string[] { "ParentID" });
                AccountHelper.DeleteDepartment(dptID);
                CacheRecord.Create(typeof(AccountLocalHelper)).Release();
                string content = string.Format("ɾ�����š�{0}��", dptName);
                AddLog("ɾ������", content);


                Response.Redirect(String.Format("AccountList.aspx?id={0}", dpt.ParentID));
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("ɾ�����ų�������ԭ��" + ex.Message);
            }
        }

        protected void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (DemoSiteMessage) return;//�Ƿ�����ʾվ��
                string actID = IDTextBox.Text;
                string actName = NameTextBox.Text;

                AccountHelper.DeleteAccont(actID);

                string content = string.Format("ɾ���ʻ���{0}��", actName);
                AddLog("ɾ���ʻ�", content);

                Response.Redirect(String.Format("AccountList.aspx?id={0}", DepartmentID));
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("ɾ���û���������ԭ��" + ex.Message);
            }
        }

        /// <summary>
        /// ����������/״̬���˵ĳ��������ַ���
        /// </summary>
        /// <returns></returns>
        string BuildStateLinks()
        {
            string links = @"<li> <a href='AccountList.aspx?state=100'   {0} >ȫ���û�<span class=""count"">({1})</span></a> |</li>
            <li><a href='AccountList.aspx?state=1'  {2} >��ͨ�û�<span class=""count"">({3})</span></a> |</li>
            <li><a href='AccountList.aspx?state=0'  {4}  >����Ա�û�<span class=""count"">({5})</span></a></li>
";
            string css100, css0, css1, css2;//, css3 css3 =
            css100 = css0 = css1 = css2 = "";
            if (CurrentState == OwnerRank.All) css100 = "class=\"current\"";
            if (CurrentState == OwnerRank.Normal) css0 = "class=\"current\"";
            if (CurrentState == OwnerRank.Admin) css1 = "class=\"current\"";

            AccountQuery aq = new AccountQuery();
            string siteID = SiteConfigs.GetConfig().SiteGroupEnabled ? SiteConfigs.GetConfig().SiteID : string.Empty;
            aq.SiteID = siteID;
            aq.UserType =(int)OwnerRank.All;
            int count = AccountHelper.QueryAccountCountByQuery(aq);
            aq.UserType = (int)OwnerRank.Normal;
            int count0 = AccountHelper.QueryAccountCountByQuery(aq);
            aq.UserType = (int)OwnerRank.Admin;
            int count1 = AccountHelper.QueryAccountCountByQuery(aq);

            links = string.Format(links, css100, count, css0, count0, css1, count1);

            return links;
        }

        /// <summary>
        /// ������ǰλ�õ���
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "<a href='/admin/'>����̨</a> > <a >�û�</a> >  <a href='AccountList.aspx'>�û�����</a>  {0} ";
            return string.Format(pos, DepartMappath(DepartmentID));
        }

        string DepartMappath(string departID)
        {
            if (We7Helper.IsEmptyID(departID))
            {
                return "";
            }
            else
            {
                string navString = "<a href='../Departments.aspx?id={0}'>���� <b>{1}</b></a>";
                Department dpt = AccountHelper.GetDepartment(departID,
                      new string[] { "Name", "Description", "ParentID" });

                navString = string.Format(navString, dpt.ID, dpt.Name);
                return DepartMappath(dpt.ParentID) + " > " + navString;
            }
        }
    }
         
}