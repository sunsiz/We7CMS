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
using We7.CMS.Common.PF;

namespace We7.CMS.Web.Admin
{
    public partial class Role_Basic : BaseUserControl
    {
        string RoleID
        {
            get { return Request["id"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["saved"] != null && Request["saved"].ToString() == "1")
                {
                    Messages.ShowMessage("��ɫ��Ϣ�Ѿ��ɹ����¡�");
                }
                Initialize();
            }
        }

        protected void Initialize()
        {
            if (RoleID != null)
            {
                Role r = AccountHelper.GetRole(RoleID);
                ShowRole(r);
            }
        }

        void ShowMessage(string m)
        {
            Messages.ShowMessage(m);
        }

        void ShowRole(Role r)
        {
            IDLabel.Text = r.ID;
            NameTextBox.Value = r.Name;
            DescriptionTextBox.Value = r.Description;
            CreatedLabel.Text = r.Created.ToString();
            RoleIDTextBox.Text = r.ID;
            TypeDropDownList1.SelectedValue = r.RoleType.ToString();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            string id = IDLabel.Text;
            string name = NameTextBox.Value;
            string description = DescriptionTextBox.Value;
            string roletype = "";
            if (TypeDropDownList1.Visible==true)
            {
                roletype = TypeDropDownList1.SelectedValue;
            }
            else
            {
                roletype = TypeDropDownList2.SelectedValue;
            }
            if (We7Helper.IsEmptyID(id))
            {
                if (AccountHelper.GetRoleBytitle(name) != null)
                    Messages.ShowError(name + " �Ľ�ɫ�Ѿ����ڡ�");
                else
                {
                    string idNew = Guid.NewGuid().ToString();
                    Role r = new Role(idNew, name, description, roletype);
                    AccountHelper.AddRole(r);
                    ShowRole(r);

                    //��¼��־
                    string content = string.Format("�½���ɫ��{0}��", name);
                    AddLog("�½���ɫ", content);

                    string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                    rawurl = We7Helper.AddParamToUrl(rawurl, "id", r.ID);
                    Response.Redirect(rawurl);
                }
            }
            else
            {
                Role r = new Role(id, name, description, roletype);
                AccountHelper.UpdateRole(r);
                ShowMessage("��ɫ��Ϣ�Ѿ����¡�");

                //��¼��־
                string content = string.Format("�޸��˽�ɫ��{0}������Ϣ", name);
                AddLog("�༭��ɫ", content);
            }
        }
    }
}