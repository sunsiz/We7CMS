using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateUpload : BasePage
    {
        protected override void Initialize()
        {
            Uploader = new TemplateUploader();
            Uploader.TemporaryPath = Path.GetTempPath();
            Uploader.WebRoot = Server.MapPath(".");
        }

        public TemplateUploader Uploader
        {
            get { return (TemplateUploader)ViewState["$VS_STORED_FILE"]; }
            set { ViewState["$VS_STORED_FILE"] = value; }
        }

        void ShowMessage(string m)
        {
            MessageLabel.Text = m;
            MessagePanel.Visible = true;
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DataControlFileUpload.HasFile)
                {
                    ShowMessage("����ѡ��һ���ؼ����ݰ��ļ���.zip����");
                }
                else
                {
                    string fn = DataControlFileUpload.FileName;
                    if (String.Compare(Path.GetExtension(fn), ".zip", true) != 0)
                    {
                        ShowMessage("�ؼ����ݰ��ļ�������zip�ļ���");
                    }
                    else
                    {
                        Uploader.FileName = Path.GetTempFileName() + ".zip";
                        DataControlFileUpload.SaveAs(Uploader.FileName);

                        Uploader.Process(Path.GetFileNameWithoutExtension(fn));
                        DataControlsGridView.DataSource = Uploader.Templates;
                        DataControlsGridView.DataBind();

                        ReviewPanel.Visible = true;
                        UploadPanel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Uploader.Deploy();
                Uploader.Clean();

                ReviewPanel.Visible = false;
                MakeTPGHyperLink.Visible = DonePanel.Visible = true;
                DoneLabel.Text = String.Format("ģ�����ϴ��ɹ�����Ҫ����ģ���飬�뽫ģ��������Ϊ��ǰ�ϴ�ģ���顣");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}
