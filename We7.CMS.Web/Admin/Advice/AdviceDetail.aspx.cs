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
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.CMS.Common.PF;
using System.Xml;
using System.IO;
using We7.Framework.Util;
using We7.Model.UI.Panel.system;
using We7.Framework;
using We7.Framework.Config;
using We7.Model.Core;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceDetail : BasePage
    {
        #region ����

        public string AdviceID
        {
            get
            {
                if (Request["ID"] != null)
                    return Request["ID"];
                return string.Empty;
            }
        }
        public bool IsFromDepartment
        {
            get
            {
                return Request["from"] != null && Request["from"].ToString() == "depart";
            }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }


        Advice advice;
        Advice ThisAdvice
        {
            get
            {
                if (advice == null)
                    advice = AdviceHelper.GetAdvice(AdviceID);
                return advice;
            }
        }

        public string AdviceTypeID
        {
            get
            {
                return ThisAdvice.TypeID;
            }
        }
        private string FileName
        {
            get { return Server.MapPath("~/Config/AdviceTag.xml"); }
        }

        private string XPath
        {
            get { return "/AdviceTag"; }
        }

        protected string adviceTag = "";
        #endregion

        #region ҳ���ʼ����Ϣ

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Initialize();
            }
               
        }

        void Initialize()
        {
            BindReplayList();
            Advice adviceModel = ThisAdvice;
            adviceModel.IsRead = 0;
            AdviceHelper.UpdateAdvice(adviceModel, null);
            if (AdviceTypeID != null && AdviceTypeID != "")
            {
                NameLabel.Text = AdviceTypeHelper.GetAdviceType(AdviceTypeID).Title.ToString() + "��ϸ��Ϣ"; ;
            }
            PagePathLiteral.Text = BuildPagePath();
            InitializeButtons();            
            DataBindAdviceTag();
            BindReplyUserList();
        }

        /// <summary>
        /// �󶨰�����
        /// </summary>
        private void BindReplyUserList()
        {
            //��ȡ����ӵ�����Ȩ�޵��û�ID
            string content = "Advice.Handle";
            List<string> accountIDs = AdviceHelper.GetAllReceivers(AdviceTypeID, content);
            List<Account> account = new List<Account>();
            this.ddlToOtherHandleUserID.Items.Clear();
            foreach (string aID in accountIDs)
            {
                if (aID != "")
                {
                    Account a = AccountHelper.GetAccount(aID, new string[] { "ID", "DepartmentID", "LoginName", "LastName" });
                    if (a != null)
                    {
                        Department dp = AccountHelper.GetDepartment(a.DepartmentID, new string[] { "Name" });
                        string text = a.LastName;
                        if (string.IsNullOrEmpty(text)) text = a.LoginName;
                        if (dp != null && !string.IsNullOrEmpty(dp.Name))
                        {
                            text = dp.Name + " - " + text;
                        }
                        ListItem lsTemp = new ListItem(text, a.ID);
                        if (!this.ddlToOtherHandleUserID.Items.Contains(lsTemp))
                            this.ddlToOtherHandleUserID.Items.Add(lsTemp);
                    }
                }
            }
        }
        /// <summary>
        ///  ������ť״̬��ʼ��
        /// </summary>
        private void InitializeButtons()
        {
            bool canAccept = false; //��������
            bool canAdmin = false;//��������
            bool canHandle = false;//��������
            bool canRead = false;//�鿴
            bool canCheck = false; //���

            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
            //ģ�Ͳ�Ϊ�գ��������û���½ʱ��֤�Ƿ����Ȩ��
            if (AdviceTypeID != null && !We7Helper.IsEmptyID(AccountID))
            {
                List<string> contents = AccountHelper.GetPermissionContents(AccountID, AdviceTypeID);
                canAccept = contents.Contains("Advice.Accept");
                canAdmin = contents.Contains("Advice.Admin");
                canHandle = contents.Contains("Advice.Handle");
                canRead = contents.Contains("Advice.Read");

                //canAccept = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Accept");
                //canAdmin = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Admin");
                //canHandle = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Handle");
                //canRead = AccountHelper.HavePermission(AccountID, AdviceTypeID, "Advice.Read");
                if (adviceType.FlowSeries > 0)
                {
                    canCheck = true;
                }
            }
            else
            {
                canAccept = canAdmin = canCheck = canHandle = true;
            }

            canCheck = canCheck && (ThisAdvice.State == (int)AdviceState.Checking);
            canHandle = canHandle && (ThisAdvice.State == (int)AdviceState.WaitHandle || ThisAdvice.State == (int)AdviceState.WaitAccept && adviceType.StateText == "ֱ�Ӱ���");
            canAccept = canAccept && (ThisAdvice.State == (int)AdviceState.WaitAccept);

            TransactHyperLink.Visible = canHandle && (adviceType.StateText != "�ϱ�����");//����
            ToOtherHyperLink.Visible = (canHandle || canAccept) && (adviceType.StateText != "ֱ�Ӱ���");//ת�� ģ������ж�
            trToOtherHandleUser.Visible = ToOtherHyperLink.Visible;//�Ƿ�ת��
            trHandleRemark.Visible = ToOtherHyperLink.Visible;//ת�챸ע
            trPriority.Visible = ToOtherHyperLink.Visible;//�ʼ����ȼ�

            AuditReportHyperLink.Visible = canHandle && (adviceType.StateText == "�ϱ�����");//�ϱ����
            ReportHyperLink.Visible = canCheck;
            chbSendEmail.Visible = canCheck;
            fontSendEmail.Visible = canCheck;
            ReturnHyperLink.Visible = (canHandle || canCheck) && (adviceType.StateText != "ֱ�Ӱ���");//�˻��ذ� �������
            ReplyContentTextBox.Visible = canHandle || ThisAdvice.State == (int)AdviceState.Checking;
            AdminHandHyperLink.Visible = (canHandle || canAccept) && (ThisAdvice.State != (int)AdviceState.Finished);

            switch (adviceType.StateText)
            {
                case "ת������":

                    if (canHandle)
                    {
                        ToAdviceTextBox.Visible = true;
                        toAdviceLabel.Text = "ת����ע��";
                    }
                    break;
                case "�ϱ�����":
                    switch (ThisAdvice.State)
                    {
                        case (int)AdviceState.Checking:

                            toAdviceLabel.Text = "��������";
                            ToAdviceTextBox.Visible = true;
                            break;

                        case (int)AdviceState.WaitHandle:
                            toAdviceLabel.Visible = false;
                            ToAdviceTextBox.Visible = false;

                            break;

                        case (int)AdviceState.WaitAccept:

                            toAdviceLabel.Visible = false;
                            ToAdviceTextBox.Visible = false;
                            break;
                    }

                    break;
                case "ֱ�Ӱ���":
                    ToAdviceTextBox.Visible = false;
                    break;
                default:
                    break;
            }

            AdviceReply reply = AdviceReplyHelper.GetAdviceReplyByAdviceID(AdviceID);
            if (reply != null)
            {
                if (reply.Content != null && reply.Content != "")
                {
                    Account accountModel = AccountHelper.GetAccount(ThisAdvice.ToOtherHandleUserID, new string[] { "LastName", "DepartmentID" });
                    string departmentAndUser = "";
                    if (accountModel != null)
                    {
                        Department dp = AccountHelper.GetDepartment(accountModel.DepartmentID, new string[] { "Name" });
                        if (dp != null && !string.IsNullOrEmpty(dp.Name))
                            departmentAndUser = "<p>" + dp.Name + " - " + accountModel.LastName;
                        else
                            departmentAndUser = "<p>" + accountModel.LastName;
                    }

                    if (ReplyContentTextBox.Visible)
                        ReplyContentTextBox.Value = We7Helper.ConvertPageBreakFromCharToVisual(reply.Content);
                    else
                    {
                        replyDiv.InnerHtml = We7Helper.ConvertPageBreakFromCharToVisual(reply.Content) + departmentAndUser;
                    }
                }
            }
        }

        /// <summary>
        /// ������ǰλ�õ���
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "";

            if (AdviceID != null)
            {
                if (ThisAdvice != null)
                {
                    pos = "��ʼ > <a >����</a> >  <a href=\"../Advice/AdviceList.aspx?AdviceTypeID=" + AdviceTypeID + "\" >�����б�</a> >  <a>��"
                        + advice.Title + "��������ϸ��Ϣ</a>";
                }
            }
            else
            {
                pos = "��ʼ > <a >����</a> >  <a href=\"../Advice/AdviceList.aspx?AdviceTypeID=" + AdviceTypeID + "\" >�����б�</a> >  <a>������ϸ��Ϣ</a>";
            }
            return pos;
        }

        protected void Pager_Fired(object sender, EventArgs e)
        {
            BindReplayList();
        }

        void BindReplayList()
        {
            if (ThisAdvice != null)
            {
                TitleLabel.Text = "��Ϣ" + ThisAdvice.StateText + "��" + "��" + ThisAdvice.Title + "��";
                SimpleEditorPanel uc = this.LoadControl("/ModelUI/Panel/System/SimpleEditorPanel.ascx") as SimpleEditorPanel;
                uc.PanelName = "adminView";
                uc.ModelName = ThisAdvice.ModelName;
                uc.PanelContext.CtrVersion = CtrVersion.V26;
                uc.IsViewer = true;
                ModelDetails.Controls.Add(uc);
            }
        }


        string GetParentNodeAccountName(ProcessHistory[] ph, int j, string curLayerNO)
        {
            string name = "";
            for (int i = j - 1; i >= 0; i--)
            {
                string parentLayerNO = "";
                if (curLayerNO == "1")
                {
                    parentLayerNO = "-3";
                }
                else if (curLayerNO == "-3")
                {
                    parentLayerNO = "-1";
                }
                else
                {
                    parentLayerNO = (Convert.ToInt16(curLayerNO) - 1).ToString();
                }

                if (ph[i].ToProcessState == parentLayerNO)
                {
                    name = GetAccountName(ph[i].ProcessAccountID);
                    break;
                }
            }

            if (name == null || name == "") name = "δ֪�û�";
            return name;
        }

        string GetAccountLoginName(string accountID)
        {
            string name;
            if (We7Helper.IsEmptyID(accountID))
                name = "Administration";
            else
                name = AccountHelper.GetAccount(accountID,new string[]{"LoginName"}).LoginName;
            if (name == null || name == "") name = "user";
            return name;
        }

        string GetAccountName(string accountID)
        {
            string name;
            if (We7Helper.IsEmptyID(accountID))
                name = "����Ա";
            else
                name = AccountHelper.GetAccount(accountID, new string[] { "LastName" }).LastName;
            if (name == null || name == "") name = "δ֪�û�";
            return name;
        }

        public string GetRemark(string adviceID, string id)
        {
            string remark = ProcessHistoryHelper.GetAdviceProcessHistory(adviceID, id).Remark;
            if (remark != "") remark = "�������������" + remark;
            return remark;
        }
        /// <summary>
        /// �󶨷������
        /// </summary>
        private void DataBindAdviceTag()
        {
            AdviceState ast = (AdviceState)System.Enum.Parse(typeof(AdviceState), ThisAdvice.State.ToString());
            if (ast == AdviceState.WaitAccept)
            {
                XmlNode adviceTagNodes = XmlHelper.GetXmlNode(FileName, XPath);
                Dictionary<string, string> tagList = new Dictionary<string, string>();
                foreach (XmlNode node in adviceTagNodes)
                {
                    tagList.Add(node.Attributes["name"].Value, node.Attributes["name"].Value);
                }
                ddlAdviceTag.DataSource = tagList;
                ddlAdviceTag.DataTextField = "key";
                ddlAdviceTag.DataValueField = "value";
                ddlAdviceTag.DataBind();
                ddlAdviceTag.Attributes.Add("onchange", "SelectedAdviceTag('" + ddlAdviceTag.ClientID + "')");
            }
            else
            {
                ddlAdviceTag.Visible = false; 
                //newTag.Visible = false;
                lbAdviceTag.Text = ThisAdvice.AdviceTag;
            }
        }

        #endregion

        #region �¼�����

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TransactButton_Click(object sender, EventArgs e)
        {
            UserIDTextBox.Text = "";
            if (OperationInfo(AdviceState.Finished, "1", true))
            {
                Messages.ShowMessage(" :) ����ɹ���");
                //��¼��־
                string content = string.Format("���������Է���:��{0}������ϸ��Ϣ", AdviceID);
                AddLog("���Է�����ϸ��Ϣ", content);
                BindReplayList();
            }
        }

        /// <summary>
        /// ת��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ToOtherButton_Click(object sender, EventArgs e)
        {
            UpdateAdviceTag();
            if (OperationInfo(AdviceState.WaitHandle))
            {
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                if (adviceType != null)
                {
                    if (adviceType.ParticipateMode == (int)AdviceParticipateMode.Mail || adviceType.ParticipateMode == (int)AdviceParticipateMode.All)
                    {
                        List<string> contents = AccountHelper.GetPermissionContents(AccountID, AdviceTypeID);
                        if (contents.Contains("Advice.Admin") || We7Helper.IsEmptyID(AccountID))
                        {
                            ToOtherReplyUser();
                        }
                    }
                }
                Messages.ShowMessage(" :) ת��ɹ���");
                //��¼��־
                string content = string.Format("ת�������Է���:��{0}������ϸ��Ϣ", AdviceID);
                AddLog("���Է�����ϸ��Ϣ", content);
                BindReplayList();
                actionTable.Visible = false;
            }
        }

        /// <summary>
        /// �ϱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AuditReportButton_Click(object sender, EventArgs e)
        {
            UserIDTextBox.Text = "";
            Advice a = AdviceHelper.GetAdvice(AdviceID);
            bool success = false;
            if (a.State == (int)AdviceState.WaitHandle)
            {
                success = OperationInfo(AdviceState.Checking,"1", true);
            }
            else
            {
                success = OperationInfo(AdviceState.Checking);
            }

            if (success)
            {
                Messages.ShowMessage(" :) �ϱ���˳ɹ���");
                //��¼��־
                string content = string.Format("�ϱ���������Է���:��{0}������ϸ��Ϣ", AdviceID);
                AddLog("���Է�����ϸ��Ϣ", content);
            }

            Initialize();
        }

        /// <summary>
        /// ���ͨ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReportButton_Click(object sender, EventArgs e)
        {
            if (OperationInfo(AdviceState.Checking))
            {
                Advice a = ThisAdvice;                
                AdviceReply reply = AdviceReplyHelper.GetAdviceReplyByAdviceID(AdviceID);
                if (reply == null)
                {
                    reply = new AdviceReply();
                    reply.AdviceID = AdviceID;                    
                    reply.Suggest = ToAdviceTextBox.Text;
                    reply.UserID = AccountID;
                    reply.CreateDate = DateTime.Now;
                    reply.Updated = DateTime.Now;
                }
                reply.Content = We7Helper.ConvertPageBreakFromVisualToChar(ReplyContentTextBox.Value);
                AdviceReplyHelper.UpdateReplyByAdviceID(reply, null);
                if (a.State == (int)AdviceState.Finished)
                {
                    if (chbSendEmail.Checked)
                    {
                        AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                        AdviceEmailConfigInfo info = adviceEmailConfigs["ReplyUser"];
                        AdviceHelper.SendResultMailToAdvicer(a, reply, null, info);
                    }
                }
                Messages.ShowMessage(" :) ��˳ɹ���");
                //��¼��־
                string content = string.Format("���ͨ�������Է���:��{0}������ϸ��Ϣ", AdviceID);
                AddLog("���Է�����ϸ��Ϣ", content);
                Response.Write("<script>alert('��˳ɹ���');location.href='AdviceList.aspx?adviceTypeID=" + ThisAdvice.TypeID + "';</script>");                
                //Initialize();
            }
        }

        /// <summary>
        /// �˻��ذ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReturnButton_Click(object sender, EventArgs e)
        {
            string action = "";
            bool ret = false;
            if (ThisAdvice.State == (int)AdviceState.WaitHandle)
            {
                action = "�ط�";
                ret = OperationInfo(AdviceState.WaitAccept, "0", false);
            }
            else if (ThisAdvice.State == (int)AdviceState.Checking)
            {
                action = "�ذ�";
                ret = OperationInfo(AdviceState.WaitHandle, "0", false);
            }
            if (ret)
            {
                Messages.ShowMessage("�ѳɹ��˻�" + action + "��");
                //��¼��־
                string content = string.Format("�˻�" + action + "�����Է���:��{0}������ϸ��Ϣ", AdviceID);
                AddLog("���Է�����ϸ��Ϣ", content);
            }
            Initialize();
        }

        /// <summary>
        /// ����Ա���Ϊ�Ѵ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdminHandButton_Click(object sender, EventArgs e)
        {
            Advice advice = new Advice();
            advice.EnumState = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceEnum, (int)EnumLibrary.AdviceEnum.AdminHandle);
            advice.State = (int)AdviceState.Finished;
            advice.Updated = DateTime.Now;
            advice.ID = AdviceID;
            advice.AdviceTag = ddlAdviceTag.SelectedItem.Value;
            advice.ProcessState = ((int)ProcessStates.Finished).ToString();
            string[] fields = new string[] { "ID", "State", "EnumState", "Updated", "AdviceTag", "ProcessState" };
            AdviceHelper.UpdateAdvice(advice, fields);
            actionTable.Visible = false;
            Messages.ShowMessage("�����ѱ��Ϊ����");
        }

        protected bool OperationInfo(AdviceState state)
        {
            return OperationInfo(state, "1", false);
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <returns></returns>
        protected bool OperationInfo(AdviceState state, string direction, bool saveReply)
        {
            try
            {
                AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
                Advice a = ThisAdvice;

                //�������ظ���Ϣ
                AdviceReply adviceReply = null;
                if (saveReply)
                {
                    adviceReply = new AdviceReply();
                    adviceReply.AdviceID = AdviceID;
                    adviceReply.Content = We7Helper.ConvertPageBreakFromVisualToChar(ReplyContentTextBox.Value);
                    adviceReply.Suggest = ToAdviceTextBox.Text;
                    adviceReply.UserID = AccountID;
                    adviceReply.CreateDate = DateTime.Now;
                    adviceReply.Updated = DateTime.Now;

                    //���ӻظ���
                    a.ReplyCount += 1;
                }

                //���·�����Ϣ
                a.Updated = DateTime.Now;
                a.ToHandleTime = DateTime.Now;
                if (UserIDTextBox.Text.Trim() != "")                    
                    a.ToOtherHandleUserID = UserIDTextBox.Text.ToString();
                else
                    a.ToOtherHandleUserID = AccountID;
                a.State = (int)state;

                //����������
                Advice oldAdvice = AdviceHelper.GetAdvice(AdviceID);
                Processing ap = ProcessHelper.GetAdviceProcess(oldAdvice);
                ap.UpdateDate = DateTime.Now;
                ap.ProcessAccountID = AccountID;
                ap.ApproveName = AccountHelper.GetAccount(AccountID,new string[]{"LastName"}).LastName;
                ap.ProcessDirection = direction.ToString();
                ap.Remark = ToAdviceTextBox.Text;
                if (state == AdviceState.WaitHandle)
                {
                    a.ProcessState = ((int)state).ToString();
                    string myText = "�� {0} ����һ�·�����{1}����";
                    string userName = AccountHelper.GetAccount(UserIDTextBox.Text, new string[] { "LastName" }).LastName;
                    ap.Remark = string.Format(myText, userName, a.Title) + "<br>" + ap.Remark;
                }
                switch (state)
                {
                    case AdviceState.All:
                        break;
                    case AdviceState.WaitAccept:
                    case AdviceState.WaitHandle:
                    case AdviceState.Finished:
                        break;
                    case AdviceState.Checking:
                        int auditLevel = 0;
                        if (We7Helper.IsNumber(a.ProcessState))
                            auditLevel = int.Parse(a.ProcessState);
                        if (auditLevel < 0)
                        {
                            auditLevel = 0;
                        }
                        auditLevel += 1;
                        if (auditLevel > adviceType.FlowSeries)
                        {
                            a.ProcessState = ((int)AdviceState.Finished).ToString();
                            a.State = (int)AdviceState.Finished;
                            a.MustHandle = 0;
                        }
                        else
                        {
                            a.ProcessState = auditLevel.ToString();
                        }
                        break;
                    default:
                        break;
                }
                ap.CurLayerNO = a.ProcessState;
                ap.AdviceState = (AdviceState)a.State;

                AdviceHelper.OperationAdviceInfo(adviceReply, oldAdvice, ap);
                if (state == AdviceState.WaitHandle)
                    AdviceHelper.UpdateAdvice(a, new string[] { "ToHandleTime", "ToOtherHandleUserID" });

                if (state == AdviceState.Finished)
                {
                    AdviceEmailConfigs adviceEmailConfigs = new AdviceEmailConfigs();
                    AdviceEmailConfigInfo info = adviceEmailConfigs["ReplyUser"];
                    AdviceHelper.SendResultMailToAdvicer(a, adviceReply, adviceType,info);
                }
                return true;
            }
            catch (Exception ex)
            {
                Messages.ShowError(" ��Ϣ����ʧ�ܣ�ԭ��" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// �ʼ�ת������
        /// </summary>
        public void ToOtherReplyUser()
        {
            AdviceReply adviceReply = new AdviceReply();
            //adviceReply.UserID = UserIDTextBox.Text;
            adviceReply.UserID = ddlToOtherHandleUserID.SelectedValue;

            if (AdviceID.Trim() != "")
            {
                adviceReply.AdviceID = AdviceID;
                AdviceReply ar = AdviceReplyHelper.GetAdviceReplyByAdviceID(AdviceID);
                if (ar == null)
                {
                    AdviceReplyHelper.AddAdviceReply(adviceReply);
                }
            }
            List<string> list = new List<string>();
            list.Add(AdviceID);
            //�����ʼ���������
            AdviceHelper.SendMailToHandler(list, adviceReply.UserID, AdviceTypeID, txtRemark.Text,rblPriority.SelectedValue);
        }

        #endregion



        /// <summary>
        /// ���·�����ǩ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateAdviceTag()
        {
          
            if(this.ddlAdviceTag.Items.Count <1)
            {
                DataBindAdviceTag();
            }
            string adviceTag = txtAdviceTag.Text.Trim();
            if (string.IsNullOrEmpty(adviceTag) && this.ddlAdviceTag.Items.Count > 0)
            {
                adviceTag = this.ddlAdviceTag.Items[0].Value;                
            }
            else if (adviceTag == "noTag" && this.ddlAdviceTag.Items.Count > 0)
            {
                adviceTag = this.ddlAdviceTag.Items[0].Value;
            }
            Advice advice = new Advice();
            advice.ID = AdviceID;
            advice.AdviceTag = adviceTag;
            string[] fields = new string[] { "ID", "Updated", "AdviceTag" };
            AdviceHelper.UpdateAdvice(advice, fields);
        }


    }
}
