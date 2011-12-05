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
using System.Text;

using Thinkment.Data;

namespace We7.CMS.Web.Admin
{
    public partial class ContentTypeDetail : BasePage
    {
        /// <summary>
        /// Config�ļ���
        /// </summary>
        string FileName
        {
            get { return Request["file"]; }
        }

        string CurrentFolder
        {
            get
            {
                string p = Request["folder"];
                if (p == null)
                {
                    p = "\\Config\\ContentModel";
                    ReturnHyperLink.NavigateUrl = "ContentTypeList.aspx";
                }
                 return p;
            }
        }
        protected override void Initialize()
        {
            if (FileName != null)
            {
                SummaryLabel.Text = String.Format("�༭Config�ļ�{0}", FileName);
                string path = Server.MapPath(Path.Combine(CurrentFolder, FileName));
                if (File.Exists(path))
                {
                    Read(Server.MapPath(Path.Combine(CurrentFolder, FileName)));
                }
                else
                {
                    SummaryLabel.Text = "����һ���µ�����ģ���ļ�";
                    ConfigNameLabel.Visible = ConfigNameTextBox.Visible = ConfigFileExt.Visible = true;
                    ConfigNameLabel.Text = ConfigNameTextBox.Text = FileName;
                    ConfigNameTextBox.Enabled = false;
                }
            }
            else
            {
                SummaryLabel.Text = "����һ���µ�����ģ���ļ�";
                ConfigNameLabel.Visible = ConfigNameTextBox.Visible =ConfigFileExt.Visible= true;
            }
        }
        /// <summary>
        /// ��ȡConfig�ļ�
        /// </summary>
        /// <param name="path"></param>
        public void Read(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    ConfigTextBox.Text += s + "\r\n";
                }
            }
        }
        /// <summary>
        /// д��Config�ļ�
        /// </summary>
        /// <param name="path"></param>
        public void Write(string path)
        {
            if(File.Exists(path)) File.SetAttributes(path, FileAttributes.Normal);//�޸��ļ�ֻ������ 
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(ConfigTextBox.Text);
                sw.Flush();
                sw.Close();
            }
            Messages.ShowMessage( "�ѳɹ����������ļ���"+DateTime.Now.ToString());
        }
        /// <summary>
        /// ����Config�ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (FileName != null)
            {
                Write(Server.MapPath(Path.Combine(CurrentFolder, FileName)));

                //��¼��־
                string content = string.Format("�޸�������ģ���ļ���{0}��", FileName);
                AddLog("�༭����ģ���ļ�", content);
            }
            else
            {
                CreateFile();
            }
        }
        /// <summary>
        /// ����Config�ļ�
        /// </summary>
        /// <param name="path"></param>
        protected void CreateFile()
        {
            string path = Server.MapPath(Path.Combine(CurrentFolder, ConfigNameTextBox.Text + ".xml"));
            if (!File.Exists(path))
            {
                Write(Server.MapPath(Path.Combine(CurrentFolder, ConfigNameTextBox.Text + ".xml")));

                //��¼��־
                string content = string.Format("����������ģ���ļ���{0}��", ConfigNameTextBox.Text + ".xml");
                AddLog("��������ģ���ļ�", content);
            }
            else
            {
                Messages.ShowError("���д��ļ������뻻һ���ļ������ԡ�");
            }
        }
    }
}
