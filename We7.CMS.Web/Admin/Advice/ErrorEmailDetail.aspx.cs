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
using System.IO;
using System.Xml;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using We7.Model.Core.UI;
using We7.Model.UI.Data;
using We7.Model.Core;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin
{
    public partial class ErrorEmailDetail :  BasePage
    {
        //protected override string[] Permissions
        //{
        //    get
        //    {
        //        return new string[] { "Admin.Advices" };
        //    }
        //}

        public string FileName
        {
            get 
            {
                if (Request["fileName"] != null)
                    return Request["fileName"];
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

        public bool HavePermission
        {
            get
            {
                if (We7Helper.IsEmptyID(AccountID))
                    return true;
                else
                {
                    Account a = AccountHelper.GetAccount(AccountID, null);
                    if (a != null)
                        return true;
                    else
                        return false;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //���ط������
            BindAdvice();
            PagePathLiteral.Text = BuildPagePath();
            LoadErrorEmailInfo();
            BindReplayList();
        }

        /// <summary>
        /// ҳ�������Ϣ
        /// </summary>
        void LoadErrorEmailInfo()
        {
            //TitleTextBox.Text = Title;
           
            string root = Server.MapPath("/_Data/ErrorEmail/"+FileName);
            if (File.Exists(root))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(root);

                XmlNode node = doc.SelectSingleNode("/root/infoBody");
                if (node != null)
                    ReplayContent.InnerHtml = We7Helper.Base64Decode(node.InnerText);
                node = doc.SelectSingleNode("/root/infoUser");
                if (node != null)
                    UserLabel.Text = We7Helper.Base64Decode(node.InnerText);
                node = doc.SelectSingleNode("/root/infoRawManage");
                if (node != null)
                    InfoRawManage.InnerHtml = We7Helper.Base64Decode(node.InnerText);
                node = doc.SelectSingleNode("/root/infoSubject");
                if (node != null)
                    TitleTextBox.Text = EmailTitleLabel.Text = We7Helper.Base64Decode(node.InnerText);
            }
        }

        /// <summary>
        /// ������ǰλ�õ���
        /// </summary>
        /// <returns></returns>
        string BuildPagePath()
        {
            string pos = "��ʼ > <a >����</a> >  <a href=\"../Advice/AdviceProcessManage.aspx\" >������ع���</a> >  <a>��"
                        + FileName + "�������ʼ���ϸ��Ϣ</a>";
            return pos;
        }

        void BindReplayList()
        {
            if (!HavePermission)
            {
                Response.Write("��û��Ȩ�޷��ʴ���Ϣ��");
                Response.End();
            }
        }

        /// <summary>
        /// �������ʼ��ֶ��Ķ�Ӧ��������Ϣ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdviceBtn_Click(object sender, EventArgs e)
        {
            string adviceID = AdviceIDTextBox.Text;
            if (adviceID != null && adviceID != "")
            {
                Advice advice = AdviceHelper.GetAdvice(adviceID);
                OperationInfo(advice.ID);

                string xmlTitle = FileName;
                string root = Server.MapPath("/_Data/ErrorEmail/");
                File.Delete(root + xmlTitle);
                Messages.ShowMessage("���ѳɹ����ʼ���Ӧ����"+advice.Title+"���Ļظ��С�");
            }
        }

        /// <summary>
        /// �󶨷������
        /// </summary>
        void BindAdvice()
        {
            AdviceTypeDropDownList.Items.Clear();
            List<AdviceType> adviceType = AdviceTypeHelper.GetAdviceTypes();           
            if (adviceType != null)
            {
                for (int i = 0; i < adviceType.Count; i++)
                {
                    if (adviceType[i].MailMode != "")
                    {
                        string name = adviceType[i].Title;
                        string value =  adviceType[i].ID;
                        ListItem item = new ListItem(name, value);
                        AdviceTypeDropDownList.Items.Add(item);
                    }
                }
            }
        }


        /// <summary>
        /// ���ݴ��ʼ������µķ�����Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AdviceBtnCreate_Click(object sender, EventArgs e)
        {
            //Advice advice = new Advice();
            //advice.OwnID = Security.CurrentAccountID;
            //advice.TypeID = AdviceTypeDropDownList.SelectedValue;
            ////advice.ID = GetValue<string>(data, "ID");
            //advice.Title = EmailTitleLabel.Text;
            //advice.UserID = "";
            //advice.Content = ReplayContent.InnerHtml;
            //advice.CreateDate = DateTime.Now;
            //advice.Updated = DateTime.Now;

            //advice.Name = "";
            //advice.Email = UserLabel.Text;
            //advice.Address = "";
            //advice.Phone = "";
            //advice.Fax = "";

            //advice.State = (int)AdviceState.WaitAccept;
            //advice.SN = AdviceHelper.CreateArticleSN();
            //int isshow = 1;
            //string stateStr = StateMgr.StateProcess(advice.EnumState, EnumLibrary.Business.AdviceDisplay, isshow);
            //advice.IsShow = isshow;
            //advice.EnumState = "00000000000000000000";
            //advice.Display1 = "";
            //advice.Display2 = "";
            //advice.Display3 = "";
            //if (advice.SN < 100000)
            //{
            //    advice.SN = advice.SN + 100000;
            //}
            //advice.MyQueryPwd = We7Helper.CreateNewID().Substring(1, 8);                      
            //���������ģ����Ϣ
            //string config, schema;
            AdviceDataProvider provider = new AdviceDataProvider();
            PanelContext ctx = ModelHelper.GetPanelContext("Advice.EmailAdvice", "edit");
            ctx.Row["ID"] = We7Helper.CreateNewID();
            ctx.Row["Title"] = EmailTitleLabel.Text;
            ctx.Row["Email"]= UserLabel.Text;
            ctx.Row["Content"] = ReplayContent.InnerHtml;
            ctx.Row["TypeID"] = AdviceTypeDropDownList.SelectedValue;
           

            //advice.ModelXml = ""; //GetModelDataXml(data, advice.ModelXml, out schema, out config);//��ȡģ������
            //advice.ModelConfig =""; //config;
            //advice.ModelName = "";// data.ModelName;
            //advice.ModelSchema = "";// schema;            
            //advice = AdviceHelper.AddAdvice(advice);
            //AdviceHelper.SendNotifyMail(advice.ID);
            //string adviceID = AdviceIDTextBox.Text;
            if (provider.InsertEmailAdvice(ctx))
            {
                Messages.ShowMessage("���ѳɹ������µķ�����Ϣ��");
            }
        }

        string GetAdviceTypeID(string ModelName)
        {
            List<AdviceType> list = AdviceTypeHelper.GetAdviceTypes();
            foreach (AdviceType advicetype in list)
            {
                if (advicetype.ModelName == ModelName)
                    return advicetype.ID;
            }
            return String.Empty;
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <returns></returns>
        protected void OperationInfo(string adviceID)
        {
            try
            {
                AdviceReply adviceReply = new AdviceReply();
                adviceReply.AdviceID = adviceID;
                adviceReply.Content = ReplayContent.InnerHtml;
                adviceReply.UserID = AccountID;
                adviceReply.Title = EmailTitleLabel.Text;
                adviceReply.MailBody = InfoRawManage.InnerText;
                AdviceReplyHelper.AddAdviceReply(adviceReply);
                AdviceHelper.UpdateAdviceType(adviceID, (int)AdviceState.Finished);

                Advice a = AdviceHelper.GetAdvice(adviceID);
                if ( a.State == (int)AdviceState.Checking)
                {
                    AdviceHelper.UpdateAdviceProcess(adviceID, "1", AdviceState.Checking);
                    Processing ap = ProcessHelper.CreateAdviceProcess(adviceID, AccountID);
                    ProcessHelper.UpdateAdviceProcess(ap,a);
                    InsertArticleProcessHistory(adviceID);
                }
                else if (a.State == (int)AdviceState.Checking && a.ProcessState == ((int)ProcessStates.Unaudit).ToString())
                {
                    AdviceHelper.UpdateAdviceProcess(adviceID, ((int)ProcessStates.FirstAudit).ToString(), AdviceState.Checking);
                    Processing ap = ProcessHelper.GetAdviceProcess(a);
                    ap.CurLayerNO = "1";
                    ap.ProcessAccountID = AccountID;
                    ap.ProcessDirection = "1";
                    ap.Remark = ReplayContent.InnerText;
                    ProcessHelper.UpdateAdviceProcess(ap, a);
                    InsertArticleProcessHistory(adviceID);
                }
            }
            catch (Exception)
            {
                Messages.ShowMessage(" :( ��Ϣ����ʧ�ܣ�");
            }
        }
        void InsertArticleProcessHistory(string id)
        {
            Advice a = AdviceHelper.GetAdvice(id);
            Processing ap = ProcessHelper.GetAdviceProcess(a);
            ProcessHistory aph = new ProcessHistory();
            aph.ObjectID = ap.ObjectID;
            aph.ToProcessState = ap.CurLayerNO;
            aph.ProcessDirection = ap.ProcessDirection;
            aph.ProcessAccountID = ap.ProcessAccountID;
            aph.Remark = ap.Remark;
            aph.CreateDate = DateTime.Now;
            aph.UpdateDate = DateTime.Now;
            ProcessHistoryHelper.InsertAdviceProcessHistory(aph);
        }
    }
}
