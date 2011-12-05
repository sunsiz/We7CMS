using System;
using System.Collections.Generic;
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
using We7.CMS.Common;
using We7.Framework;
using We7.CMS.Accounts;

namespace We7.CMS.Web
{
    public partial class AttachmentDownload : Page
    {
        AttachmentHelper AttachmentHelper
        {
            get { return HelperFactory.Instance.GetHelper<AttachmentHelper>(); }
        }

        ArticleHelper ArticleHelper
        {
            get { return HelperFactory.Instance.GetHelper<ArticleHelper>(); }
        }

        ChannelHelper ChannelHelper
        {
            get { return HelperFactory.Instance.GetHelper<ChannelHelper>(); }
        }

        string AccountID
        {
            get { return Security.CurrentAccountID; }
        }

        string AttachmentID
        {
            get { return Request["id"]; }
        }

        string OwnerID
        {
            get 
            { 
                if(Request["oid"]!=null)
                    return Request["oid"];
                else if (Request["id"] != null)
                {
                    Attachment a = AttachmentHelper.GetAttachment(Request["id"].ToString());
                    if (a != null)
                        return a.ArticleID;
                }
                return null;
            }
        }

        string FileType
        {
            get { return Request["type"]; }
        }
        string FilesName
        {
            get { return Context.Server.UrlDecode(Request["fileName"]); }
        }

        Attachment ThisAttachment { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HasReadPermission();
            }
        }

        string FormatUrl()
        {
            string attachmentPath = "";
            if (!We7Helper.IsEmptyID(AttachmentID))
                ThisAttachment = AttachmentHelper.GetAttachment(AttachmentID);
            else if (!We7Helper.IsEmptyID(OwnerID) && FileType != "")
            {
                ThisAttachment = AttachmentHelper.GetFirstAttachment(OwnerID, FileType, FilesName);
            }
            if (ThisAttachment != null)
            {
                attachmentPath = string.Format("\\{0}\\{1}", ThisAttachment.FilePath, ThisAttachment.FileName);
            }
            return attachmentPath;
        }

        void FileDownload(string attachmentPath)
        {
            if (attachmentPath != "")
            {
                string FullFileName = Server.MapPath(attachmentPath);
                try
                {
                    //ˢ�����ش���
                    ThisAttachment.DownloadTimes += 1;
                    AttachmentHelper.UpdateAttachment(ThisAttachment);

                    FileInfo DownloadFile = new FileInfo(FullFileName); //����Ҫ���ص��ļ�
                    Response.Clear(); //������������е������������
                    Response.ClearHeaders(); //������������е�����ͷ
                    Response.Buffer = false; //���û������Ϊfalse
                    //����������� HTTP MIME ����Ϊapplication/octet-stream
                    Response.ContentType = "application/octet-stream";
                    //�� HTTP ͷ��ӵ������
                    Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFile.Name, System.Text.Encoding.UTF8));

                    Response.AppendHeader("Content-Length", DownloadFile.Length.ToString());

                    //��ָ�����ļ�ֱ��д�� HTTP �����������

                    Response.WriteFile(DownloadFile.FullName);
                    Response.Flush(); //��ͻ��˷��͵�ǰ���л�������
                    Response.End(); //����ǰ���л����������͵��ͻ���


                }
                catch (IOException)
                {
                    AttachmentMsgLabel.Text = "�ļ�����ʧ��,��ȷ����Դ�Ƿ����!";
                    AttachmentMsgLabel.Visible = true;
                    return;
                }
            }
            else
            {
                AttachmentMsgLabel.Text = "�ļ�����ʧ��,���ļ�������!";
                AttachmentMsgLabel.Visible = true;
            }
        }

        /// <summary>
        /// �жϷ��ʸ���Ŀ��Ȩ��
        /// </summary>
        /// <returns></returns>
        void HasReadPermission()
        {
            string BindColumnID = ArticleHelper.GetArticle(OwnerID, new string[] { "OwnerID" }).OwnerID;

            Channel ch = ChannelHelper.GetChannel(BindColumnID, new string[] { "SecurityLevel" });
            if (ch.SecurityLevel == 1)
            {
                if (AccountID != null)
                {
                    FileDownload(FormatUrl());
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('����û�е�½,���½�������ظø���');window.location='Login.aspx';</script>");
                }
            }
            else if (ch.SecurityLevel == 2)
            {
                if (AccountID != null)
                {
                    // "��û��Ȩ�����ظø���";
                }
                else
                {
                    Response.Write("<script language='javascript'>alert('����û�е�½,���½�������ظø���');window.location='Login.aspx';</script>");
                }
            }
            else
            {
                FileDownload(FormatUrl());
            }
        }
    }
}
