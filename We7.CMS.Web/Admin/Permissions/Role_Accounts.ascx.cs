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
using We7.CMS.Common.PF;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin
{
    public partial class Role_Accounts : BaseUserControl
    {
       public string RoleID
        {
            get { return Request["id"]; }
        }

        string Keyword
        {
            get
            {
                string key = "";
                if (Request["keyword"] != null)
                {
                    key=Request["keyword"];
                }
                return key;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
        }
        private int _resultsPageNumber = 1;
        /// <summary>
        /// ��ǰҳ
        /// </summary>
        protected int PageNumber
        {
            get
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber;
            }
        }
        protected void Initialize()
        {
            List<string> accounts = new List<string>();

            ArticleUPager.PageIndex = PageNumber;
            ArticleUPager.ItemCount = AccountHelper.GetAccountCountOfRole(RoleID);

            ArticleUPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
            ArticleUPager.PrefixText = "�� " + ArticleUPager.MaxPages + "  ҳ ��   �� " + ArticleUPager.PageIndex + "  ҳ �� ";

            accounts = AccountHelper.GetAccountsOfRole(RoleID, ArticleUPager.Begin - 1, ArticleUPager.Count);

            List<InnerItem> items = new List<InnerItem>();
            foreach (string aid in accounts)
            {
                InnerItem item = new InnerItem();
                item.AccountID = aid;
                item.RoleID = RoleID;
                Account a = AccountHelper.GetAccount(aid, new string[] { "LoginName", "DepartmentID", "Department" });
                if (a != null)
                {
                    item.AccountName = a.LoginName;
                    item.DepartmentName = a.Department;
                }
                items.Add(item);
            }
            personalForm.DataSource = items;
            personalForm.DataBind();
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            Initialize();
        }

        [Serializable]
        public class InnerItem
        {
            string id;
            string roleID;
            string accountID;
            string accountName;
            string departmentName;

            public string DepartmentName
            {
                get { return departmentName; }
                set { departmentName = value; }
            }

            public InnerItem()
            {
            }

            public string ID
            {
                get { return id; }
                set { id = value; }
            }

            public string RoleID
            {
                get { return roleID; }
                set { roleID = value; }
            }

            public string AccountID
            {
                get { return accountID; }
                set { accountID = value; }
            }

            public string AccountName
            {
                get { return accountName; }
                set { accountName = value; }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            string id = IDTextBox.Text;
            string name = NameTextBox.Text;
            try
            {
                AccountHelper.DeleteAccountRole(id);
                Messages.ShowMessage("�û���" + name + "���ɹ��˳���ǰ��ɫ������");
            }
            catch (Exception ex)
            {
                Messages.ShowMessage("�û���" + name
                    + "���˳���ǰ��ɫ���󣡴���ԭ��" + ex.Message);
            }

        }
    }
}
