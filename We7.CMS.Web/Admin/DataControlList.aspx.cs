using System;
using System.IO;
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
using Thinkment.Data;
using We7.CMS.Common;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class DataControlList : BasePage
    {

        protected override void Initialize()
        {
            string selectQuery = this.FieldDropDownList.SelectedItem.ToString();
            string query = SearchTextBox.Text;
            DataControl[] ds = TemplateHelper.GetDataControls(query,selectQuery);

                DetailGridView.DataSource = ds;
                DetailGridView.DataBind();

                ModeDataList.DataSource = ds;
                ModeDataList.DataBind();
            
            if (ds.Length == 0)
            {
                ShowMessage("û�з��������Ŀؼ���");
            }
            else
            {
                ShowMessage(String.Format("�ܹ� {0} ���ؼ���", ds.Length));
            }
        }

        protected override We7.CMS.Common.Enum.MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        protected void Sort(string sortName)
        {
            string sortText = sortName;
            DataControl[] ds = TemplateHelper.SortDataControls(sortText);

                DetailGridView.DataSource = ds;
                DetailGridView.DataBind();

                ModeDataList.DataSource = ds;
                ModeDataList.DataBind();

            if (ds.Length == 0)
            {
                ShowMessage("û�з��������Ŀؼ���");
            }
            else
            {
                ShowMessage(String.Format("�ܹ� {0} ���ؼ���", ds.Length));
            }
        }
        protected void ArticleButton_Click(object sender, EventArgs e)
        {
            string sort ="����";
            Sort(sort);
        }
        protected void ChannelButton_Click(object sender, EventArgs e)
        {
            string sort ="��Ŀ";
            Sort(sort);
        }
        protected void ImgButton_Click(object sender, EventArgs e)
        {
            string sort = "ͼƬ";
            Sort(sort);
        }
        protected void ListButton_Click(object sender, EventArgs e)
        {
            string sort = "�б�";
            Sort(sort);
        }
        protected void MenuButton_Click(object sender, EventArgs e)
        {
            string sort = "�˵�";
            Sort(sort);
        }
        protected void AdButton_Click(object sender, EventArgs e)
        {
            string sort = "���";
            Sort(sort);
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string sort = "��¼";
            Sort(sort);
        }
        protected void StoreButton_Click(object sender, EventArgs e)
        {
            string sort = "����";
            Sort(sort);
        }
        protected void OtherButton_Click(object sender, EventArgs e)
        {
            string sort = "����";
            Sort(sort);
        }
        void ShowMessage(string s)
        {
            MessageLabel.Text = s;
        }
    }
}
