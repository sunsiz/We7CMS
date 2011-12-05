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
using System.IO;
using We7.CMS;
using We7.CMS.Controls;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Advice_Option : BaseUserControl
    {
        /// <summary>
        /// ��ȡ����ģ��ID
        /// </summary>
        public string AdviceTypeID
        {
            get
            {
                if (Request["adviceTypeID"] != null)
                {
                    return We7Helper.FormatToGUID((string)Request["adviceTypeID"]);
                }
                return
                    Request["adviceTypeID"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AdviceTypeID == null || AdviceTypeID == "")
                {
					ExtraProperties.Visible = false;
					#region ��ע�͵Ĵ���

					// ���½�ģ��ʱ����Ǽ���Щ��Ϣ�������������Щ�ؼ�
					// ����hideExtraProperties

					//if (AccountID == "{00000000-0000-0000-0000-000000000000}")
					//{
					//    AdviceCreatedText.Text = "��������Ա";
					//}
					//else
					//{
					//    AdviceCreatedText.Text = AccountHelper.GetAccount(AccountID, new string[] { "LoginName" }).LoginName;
					//}
					//StartTimeText.Text = DateTime.Now.ToString();

					#endregion
                }
                else
                {
					ExtraProperties.Visible = true;
                    //���·���ģ����Ϣ
                    InitializePage();
                }
                if (Request["saved"] != null)
                {
                    Messages.ShowMessage("���ѳɹ���������ģ�ͣ�����ѡ����������̡���������������������Ȩ�ޡ���һ������ģ�͡�");
                }
            }
        }
        /// <summary>
        /// ����ʱ��ʼ��ҳ������
        /// </summary>
        void InitializePage()
        {
			// ����hideExtraProperties

            AdviceType adviceType = AdviceTypeHelper.GetAdviceType(AdviceTypeID);
            if (adviceType != null)
            {
                AdviceNameText.Text = adviceType.Title;
                RemarkText.Text = adviceType.Description;
				StartTimeText.Text = adviceType.CreateDate.ToString();
				AdviceCreatedText.Text = GetAccountNameText(adviceType.AccountID);
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            AdviceType adviceType = new AdviceType();
			adviceType.CreateDate = DateTime.Now;
			adviceType.Title = AdviceNameText.Text.Trim();
            adviceType.Description = RemarkText.Text.Trim();

			if (string.IsNullOrEmpty(adviceType.Title))
			{
				Messages.ShowError("ģ�����Ʋ���Ϊ��");
				return;
			}

            if (AdviceTypeID == null || AdviceTypeID == "")		// �½�
            {
                adviceType.AccountID = AccountID;
                string adviceTypeID = We7Helper.CreateNewID();
                adviceType.ID = adviceTypeID;
                AdviceTypeHelper.AddAdviceType(adviceType);
            }
            else		// �޸�
            {
                adviceType.ID = AdviceTypeID;
                AdviceTypeHelper.UpdateAdviceType(adviceType);
                Messages.ShowMessage("" + AdviceNameText.Text + " ģ���޸ĳɹ�!!");
            }
            //��¼��־
            string content = string.Format("�༭��ģ�͡�{0}������Ϣ", adviceType.Title);
            AddLog("�༭����ģ��", content);

            if (AdviceTypeID == null || AdviceTypeID == "")
            {
                string rawurl = We7Helper.AddParamToUrl(Request.RawUrl, "saved", "1");
                rawurl = We7Helper.RemoveParamFromUrl(rawurl, "adviceTypeID");
                rawurl = We7Helper.AddParamToUrl(rawurl, "adviceTypeID", We7Helper.GUIDToFormatString(adviceType.ID));
                Response.Redirect(rawurl);
            }
        }

		/// <summary>
		/// �� AccountID ������ʾΪ����
		/// <remarks>
		/// ���ۣ��Ƿ�ɸ�Ϊ����� GetAccountName ��
		/// </remarks>
		/// </summary>
		private string GetAccountNameText(string accountID)
		{
			if (accountID == null || accountID == "")
			{
				return string.Empty;
			}
			else if (accountID == "{00000000-0000-0000-0000-000000000000}")
			{
				return "��������Ա";
			}
			else
			{
				return AccountHelper.GetAccount(accountID, new string[] { "LoginName" }).LoginName;
			}
		}
    }
}