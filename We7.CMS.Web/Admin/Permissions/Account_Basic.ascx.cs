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
using System.Xml;
using System.IO;
using System.Collections.Generic;

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Config;
using We7.Model.Core;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Util;
using System.Text.RegularExpressions;
using We7.CMS.Accounts;

namespace We7.CMS.Web.Admin.Permissions
{
    public partial class Account_Basic : BaseUserControl
    {
        string CurrentAccountID
        {
            get
            {
                return Request["id"];
            }
        }

        string DepartmentID
        {
            get { return Request["d"]; }
        }

        protected void Initialize()
        {
            ResetPasswordCheckBox.Checked = false;
            ResetPasswordSpan.Visible = false;
            if (We7Helper.IsEmptyID(Security.CurrentAccountID))
            {
                UserTypeDropDownList.Items.Add(new ListItem("����Ա", "0"));
            }
            UserTypeDropDownList.Items.Add(new ListItem("��ͨ�û�","1"));

            if (We7Helper.IsEmptyID(CurrentAccountID))//�½�
            {
                PassWordText.Visible = true;
                We7Helper.AssertNotNull(DepartmentID, "AccountDetail.p");
                if (!We7Helper.IsEmptyID(DepartmentID))
                {
                    Department dpt = AccountHelper.GetDepartment(DepartmentID, new string[] { "FullName" });
                    FullPathLabel.Text = dpt.FullName;
                    ParentTextBox.Text = DepartmentID;
                }
                else
                {
                    ParentTextBox.Text = We7Helper.EmptyGUID;
                }
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "�½��û�֪ͨ");
                MailBodyTextBox.Text = mt.Body;
                SaveButton.Value = "�����˻�";
                DeleteButtun.Visible = false;
            }
            else
            {
                ShowAccount(AccountHelper.GetAccount(CurrentAccountID, null));
                ResetPasswordSpan.Visible = true;
                MailMessageTemplate mt = new MailMessageTemplate("UserEmailConfig.xml", "�˺����ͨ��֪ͨ");
                MailBodyTextBox.Text = mt.Body;
            }
        }

        /// <summary>
        /// ����û����Ƿ���Ч
        /// </summary>
        string CheckUserName(string userName)
        {
            HttpContext.Current.Response.Clear();
            int length = GetStrLen(userName);
            if (userName == "")
            {
                return "�û�������Ϊ��";
            }
            else if (length < 5 || length > 20)
            {
                return "�û���������5-20λ";
            }
            else if (AccountHelper.ExistUserName(userName))
            {
                return "�û�Ա���ѱ�ʹ��";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ��������Ƿ���Ч
        /// </summary>
        string CheckEmail(string email)
        {
            HttpContext.Current.Response.Clear();
            if (email == "")
            {
                return "Email����Ϊ��";
            }
            else if (!Regex.IsMatch(email, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                return "Email��ʽ����ȷ";
            }
            else if (AccountHelper.ExistEmail(email))
            {
                return "�õ����ʼ����ѱ�ʹ��";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// ��������Ƿ���Ч
        /// </summary>
        /// <returns></returns>
        string CheckPWD(string password)
        {
            if (!(password.Length >= 6 && password.Length <= 16))
            {
                return "���������6-16���ַ���";
            }
            else
            {
                return "";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (NameTextBox.Text == SiteConfigs.GetConfig().AdministratorName)
                {
                    Messages.ShowError("�޷������û���ԭ���û�����" + NameTextBox.Text + "��Ϊϵͳ�ؼ��֣��뻻һ���������ԡ�");
                }
                else
                    SaveAccount();
            }
            catch (Exception ce)
            {
                string msg = ce.Message.Replace(AppDomain.CurrentDomain.BaseDirectory, "/").Replace("\\", "/");
                Messages.ShowError("�û���Ϣ����ʱ��������ԭ��" + msg);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//�Ƿ�����ʾվ��
            AccountHelper.DeleteAccont(CurrentAccountID);
            Response.Redirect("../Departments.aspx");
        }

        /// <summary>
        /// ���ݲ���ID��ȡ��������
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        public string GetDepartmentNameByID(string departmentID)
        {
            if (departmentID != null && departmentID != "")
            {
                Department dpt = AccountHelper.GetDepartment(departmentID, new string[] { "FullName" });
                if (dpt != null && dpt.FullName != "")
                {
                    return dpt.FullName;
                }
            }
            return "";
            
        }

        void ShowAccount(Account act)
        {
            if (!We7Helper.IsEmptyID(act.DepartmentID))
            {
                FullPathLabel.Text = GetDepartmentNameByID(act.DepartmentID);
            }
            CreatedLabel.Text = act.Created.ToString();
            DescriptionTextBox.Text = act.Description;
            EmailTextBox.Text = act.Email;
            IDLabel.Text = act.ID.ToString();
            IndexTextBox.Text = act.Index.ToString();
            LastNameTextBox.Text = act.LastName;
            NameTextBox.Text = act.LoginName;

            ParentTextBox.Text = act.DepartmentID.ToString();
            SetDropdownList(UserTypeDropDownList, act.UserType.ToString());
            SetDropdownList(UserStateDropDownList, act.State.ToString());      
            if(!string.IsNullOrEmpty(act.ModelName))
                SetDropdownList(UserModelDropDownList, act.ModelName.ToLower());

            SetDropdownList(ModelStateDropDownList, act.ModelState.ToString());

            AccountIDTextBox.Text = act.ID;

            IsHashedCheckBox.Checked = act.IsPasswordHashed;
            OverdueTextBox.Text = act.Overdue.ToLongDateString();

            NameTextBox.Enabled = false;
            ResetPasswordCheckBox.Visible = true;
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

        protected void SetRadioButtonList(RadioButtonList list, string value)
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

        void SaveAccount()
        {
            bool addModelRole = false;
            Account act = new Account();
            act.ID = IDLabel.Text;
            act.LoginName = NameTextBox.Text;
            act.LastName = LastNameTextBox.Text;
            act.Index = Convert.ToInt32(IndexTextBox.Text);
            act.State = Convert.ToInt32(UserStateDropDownList.SelectedValue);
            act.UserType = Convert.ToInt32(UserTypeDropDownList.SelectedValue);
            act.Description = DescriptionTextBox.Text;
            act.Email = EmailTextBox.Text;
            if (!String.IsNullOrEmpty(UserModelDropDownList.SelectedValue))
            {
                if (File.Exists(ModelHelper.GetModelPath(UserModelDropDownList.SelectedValue)))
                {
                    act.ModelName = UserModelDropDownList.SelectedValue;
                    act.ModelConfig = ModelHelper.GetModelConfigXml(act.ModelName);
                    act.ModelSchema = ModelHelper.GetModelSchema(act.ModelName);
                }
                else
                    throw new Exception(UserModelDropDownList.SelectedValue + " ģ�������ļ�û���ҵ���");
            }

            if (act.ModelState != 2 && ModelStateDropDownList.SelectedValue == "2"
                && !String.IsNullOrEmpty(UserModelDropDownList.SelectedValue))
            {
                string moldelStr = UserModelDropDownList.SelectedValue;
                act.ModelState = Int32.Parse(ModelStateDropDownList.SelectedValue);
                addModelRole = true;
                if (SendMailCheckBox.Checked)
                    AccountMails.SendMailOfPassNotify(act, UserModelDropDownList.SelectedItem.Text, MailBodyTextBox.Text);
            }
           
            if (DepartmentIDTextBox.Text != null && DepartmentIDTextBox.Text.Trim() != "")
            {
                act.DepartmentID = DepartmentIDTextBox.Text;
                act.Department= AccountHelper.GetDepartment(act.DepartmentID, new string[] { "Name" }).Name;
            }


            string chkEmail = CheckEmail(this.EmailTextBox.Text.Trim());
            if (act.ID == String.Empty)
            {
                string checkName = CheckUserName(NameTextBox.Text);
                if (checkName == "" && chkEmail == "")
                {
                    if (String.IsNullOrEmpty(PassWordText.Text) || String.IsNullOrEmpty(PassWordText.Text.Trim()))
                    {
                        Messages.ShowError("���벻��Ϊ��");
                        return;
                    }
                    act.DepartmentID = ParentTextBox.Text;
                    act.IsPasswordHashed = IsHashedCheckBox.Checked;
                    if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                    {
                        act.Password = Security.Encrypt(PassWordText.Text);
                    }
                    else if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                    {
                        act.Password = Security.BbsEncrypt(PassWordText.Text);
                    }
                    else
                    {
                        act.Password = PassWordText.Text;
                    }

                    if (SendMailCheckBox.Checked)
                    {
                        MailHelper mailHelper = AccountMails.GetMailHelper();
                        if (String.IsNullOrEmpty(mailHelper.AdminEmail))
                        {
                            Messages.ShowError("û�����ù���Ա���䡣�粻��Ҫ�������䣬��ȥ�������ʼ�ѡ�");
                            return;
                        }
                        if (String.IsNullOrEmpty(act.Email))
                        {
                            Messages.ShowError("�û����䲻��Ϊ��");
                            return;
                        }
                    }

                    act = AccountHelper.AddAccount(act);
                    ShowAccount(act);

                    if (SendMailCheckBox.Checked)
                        AccountMails.SendMailOfRegister(act, PassWordText.Text, MailBodyTextBox.Text);

                    //��¼��־
                    string content = string.Format("�������ʻ���{0}��", act.LoginName);
                    AddLog("�½��ʻ�", content);

                    string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                    rawurl = We7Helper.AddParamToUrl(rawurl, "id", act.ID);
                    Response.Redirect(rawurl);
                }
                else
                {
                    Messages.ShowError("�޷�ע���û���ԭ��" + checkName + chkEmail);
                }
            }
            else
            {
                List<string> fields = new List<string>();
                fields.Add("LoginName");
                fields.Add("LastName");
                fields.Add("MiddleName");
                fields.Add("FirstName");
                fields.Add("Index");
                fields.Add("State");
                fields.Add("UserType");
                fields.Add("Description");
                fields.Add("Email");
                fields.Add("ModelName");
                fields.Add("ModelState");
                fields.Add("ModelConfig");
                fields.Add("ModelSchema");
                fields.Add("Overdue");
                if (We7Utils.IsDateString(OverdueTextBox.Text))
                    act.Overdue = DateTime.Parse(OverdueTextBox.Text);
                if (DepartmentIDTextBox.Text != null && DepartmentIDTextBox.Text.Trim() != "")
                {
                    fields.Add("DepartmentID");
                    fields.Add("Department");
                }
                string repassword = "";
                if (ResetPasswordCheckBox.Checked)
                {

                    if (String.IsNullOrEmpty(PasswordTextBox.Text) || String.IsNullOrEmpty(PasswordTextBox.Text.Trim()))
                    {
                        Messages.ShowError("���벻��Ϊ��");
                        return;
                    }
                    fields.Add("PasswordHashed");
                    act.IsPasswordHashed = IsHashedCheckBox.Checked;
                    if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                    {
                        act.Password = Security.Encrypt(PasswordTextBox.Text);
                    }
                    else if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
                    {
                        act.Password = Security.BbsEncrypt(PasswordTextBox.Text);

                    }
                    else
                    {
                        act.Password = PasswordTextBox.Text;
                    }
                    repassword = PasswordTextBox.Text.Trim();//��������ʱ������session���Ա��޸�BBS���ݿ�ʹ�á�

                    fields.Add("Password");
                }
                AccountHelper.UpdateAccount(act, fields.ToArray());
                if (addModelRole)
                    AddAccountModelRole(act);

                Messages.ShowMessage("�ʻ���Ϣ�Ѹ��¡�");
                //��¼��־
                string content = string.Format("�������ʻ���{0}������Ϣ", act.LoginName);
                AddLog("�����ʻ�", content);

                ResetPasswordCheckBox.Checked = false;
            }
        }

        private void AddAccountModelRole(Account act)
        {
            ModelInfo mi = ModelHelper.GetModelInfoByName(act.ModelName);
            string roleName = "";
            if (!string.IsNullOrEmpty(mi.Parameters))
            {
                string[] p = mi.Parameters.Split(':');
                if (p[0] == "role" && p.Length > 1)
                    roleName = p[1];
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                Role r = AccountHelper.GetRoleBytitle(roleName);
                AccountHelper.AssignAccountRole(act.ID, r.ID);
            }
        }

        void InitUserModelDropDownList()
        {
            UserModelDropDownList.DataSource = ModelHelper.GetContentModel(ModelType.ACCOUNT);
            UserModelDropDownList.DataTextField = "Label";
            UserModelDropDownList.DataValueField = "name";
            UserModelDropDownList.DataBind();
            UserModelDropDownList.Items.Insert(0, new ListItem("----", ""));
            for (int i = 0; i < UserModelDropDownList.Items.Count; i++)
                UserModelDropDownList.Items[i].Value = UserModelDropDownList.Items[i].Value.ToLower();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetPasswordCheckBox.Checked = false;
                if (Request["saved"] != null && Request["saved"].ToString() == "1")
                {
                    Messages.ShowMessage("�û���Ϣ�Ѿ��ɹ����¡�");
                }
                InitUserModelDropDownList();
                Initialize();
            }
        }


        //�õ��ַ����ȣ�һ������ռ�����ַ���
        int GetStrLen(String ss)
        {
            Char[] cc = ss.ToCharArray();
            int intLen = ss.Length;
            int i;
            if ("����".Length == 4)
            {
                //�Ƿ� ���� �� ƽ̨
                return intLen;
            }
            for (i = 0; i < cc.Length; i++)
            {
                if (Convert.ToInt32(cc[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }

    }
}