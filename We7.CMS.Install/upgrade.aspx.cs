using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Xml;

using We7.CMS.Config;
using System.Web.Configuration;
using We7.Framework.Config;
using We7.Framework;
using We7.Framework.Zip;
using System.Collections.Generic;
using We7.Framework.Util;

namespace We7.CMS.Install
{
    public class upgrade : SetupPage
    {
        protected FileUpload UpdateFileUpload;
        protected Label UploadSummary;
        protected Button CopyFilesButton;
        protected Button UploadButton;
        protected Literal UnZipLiteral;
        protected Panel NewversionPanel;
        protected Label NewVersionLabel;
        protected Button DownloadInstallButton;
        protected HyperLink DownloadLocalHyperLink;
        protected CheckBox BackUpCheckBox;
        protected CheckBox ClearOldCheckBox;
        protected Panel BackUpPanel;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ShowErrorLabel;
        protected System.Web.UI.HtmlControls.HtmlInputButton FinishButton;

        protected string UnZipPath
        {
            get
            {
                return (string)ViewState["We7$UnZipPath"];
            }
            set
            {
                ViewState["We7$UnZipPath"] = value;
            }
        }

        protected string NewVersion
        {
            get
            {
                return (string)ViewState["We7$NewVersion"];
            }
            set
            {
                ViewState["We7$NewVersion"] = value;
            }
        }

        protected string UploadFile
        {
            get
            {
                return (string)ViewState["We7$UploadFile"];
            }
            set
            {
                ViewState["We7$UploadFile"] = value;
            }
        }



        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Init();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DisableSubmitBotton(this, this.CopyFilesButton);
            if (!IsPostBack)
            {
                if (IsAdminLogin())
                {
                    CheckWebConfig();
                    //CheckNewVersion();
                    CopyFilesButton.Enabled = false;
                }
                else
                {
                    Response.Redirect("signin.aspx?returnURL=" + Server.UrlEncode(HttpContext.Current.Request.RawUrl));
                }
            }
        }

        bool IsAdminLogin()
        {
            return (HttpContext.Current.Request.IsAuthenticated && HttpContext.Current.User.Identity.Name == "administrator");
        }

        void CheckWebConfig()
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            object o = config.GetSection("system.web/httpRuntime");
            HttpRuntimeSection section = o as HttpRuntimeSection;
            int max = section.MaxRequestLength;
            if (max < 10 * 1024)
            {
                section.MaxRequestLength = 40 * 1024;
                section.AppRequestQueueLimit = 5000;
                section.ExecutionTimeout = new TimeSpan(0, 0, 3000);

                config.Save();

                ShowErrorLabel.InnerText = "ԭ��վ������֧�ִ��ļ��ϴ������Ѹ���Ϊ" + section.MaxRequestLength + "KB������������IIS!";
                ShowErrorLabel.Style.Remove("display");

                UploadButton.Enabled = false;
                DownloadInstallButton.Enabled = false;
                UpdateFileUpload.Enabled = false;
                CopyFilesButton.Enabled = false;
                //FinishButton.Disabled = true;
            }
        }

        void CheckNewVersion()
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            string url = "http://www.westengine.com/go/latestCore.aspx";
            string latestVersion = Installer.GetRemoteWebString(url, 8000, 0, Encoding.UTF8);
            string oldVersion = "";
            if (si != null && si.ProductVersion != null)
                oldVersion = si.ProductVersion;
            if (latestVersion != "" && Installer.VersionLater(oldVersion, latestVersion))
            {
                DownloadLocalHyperLink.NavigateUrl = "http://www.westengine.com/_data/We7.CMS-" + latestVersion + ".zip";
                DownloadLocalHyperLink.Target = "_blank";
                DownloadLocalHyperLink.Text = "����" + latestVersion;
                NewversionPanel.Visible = true;
                NewVersionLabel.Text = string.Format("��һ���µ� {0} CMS �汾�ɹ�����", ProductBrand) + latestVersion;
                NewVersion = latestVersion;
            }
        }

        #region Web ������������ɵĴ���
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DownloadInstallButton.Click += new EventHandler(this.DownloadInstallButton_Click);
            this.CopyFilesButton.Click += new EventHandler(this.CopyFilesButton_Click);
            this.UploadButton.Click += new EventHandler(this.UploadButton_Click);
            this.Load += new EventHandler(this.Page_Load);
        }
        #endregion

        private void DownloadInstallButton_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("/_temp");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            Installer.DownloadFileFromUrl(DownloadLocalHyperLink.NavigateUrl, folderPath);
            string filename = DownloadLocalHyperLink.NavigateUrl.Substring(DownloadLocalHyperLink.NavigateUrl.LastIndexOf('/') + 1);
            UploadFile = Path.Combine(folderPath, filename);
            UnZipPath = UnZipFile(Path.Combine(folderPath, filename));
            if (UnZipPath == "")
            {
                RegisterScript("alert('�޷���ȷ��ѹ������ѹ���ļ��Ƿ�Ϊ�Ϸ���Zipѹ����ʽ��');");
                return;
            }
            else
                UnZipLiteral.Text = FileListSummary(UnZipPath);
        }

        private void RegisterScript(string script)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script, true);
        }


        private void CopyFilesButton_Click(object sender, EventArgs e)
        {
            //configuration�ļ��и��� ����2.7��ǰ�汾����
            string oldConfigPath = Path.Combine(Server.MapPath("~/"), "configuration");
            if (Directory.Exists(oldConfigPath))
            {
                string destDirName = Path.Combine(Server.MapPath("~/"), "config");
                if (Directory.Exists(destDirName))
                {
                    Directory.Delete(destDirName, true);
                }
                Directory.Move(oldConfigPath, Path.Combine(Server.MapPath("~/"), "config"));
            }

            if (BackUpPanel.Visible)
            {
                if (BackUpCheckBox.Checked)
                    Installer.BackupOldFiles(Server.MapPath("~/"), Server.MapPath("~/_backup/update/"));

                //����Ƿ��������ļ�
                if (ClearOldCheckBox.Visible && ClearOldCheckBox.Checked)
                {
                    //�˴�Ӧ�÷��ش�����Ϣ��Ŀǰ�洢��LOG�ļ���
                    DeleteFiles();
                }
            }

            string ext = Path.GetExtension(UploadFile);
            switch (ext.ToLower())
            {
                case ".zip":
                    try
                    {
                        //DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~/bin"));
                        //Helper.DeleteFileTree(directoryInfo, false);
                        We7Helper.CopyDirectory(UnZipPath, Server.MapPath("~/"));

                        Directory.Delete(UnZipPath, true);
                        //
                        /*
                         * 
                         * �����°汾��
                        GeneralConfigInfo si = GeneralConfigs.GetConfig();
                        if (si != null)
                        {
                            //si.ProductVersion = NewVersion;
                            GeneralConfigs.SaveConfig(si);
                        }
                         */
                        RegisterScript("alert('�����ɹ�!');location.href='upgrade-db.aspx?from=upgrade.aspx'");
                    }
                    catch (IOException ex)
                    {
                        RegisterScript("alert('�ļ�����ʧ�ܡ�ԭ��" + ex.Message + "');");
                    }
                    break;

                case ".dll":
                    try
                    {
                        string targetfile = Path.Combine(Server.MapPath("~/bin/"), Path.GetFileName(UploadFile));
                        File.Copy(UploadFile, targetfile, true);
                        RegisterScript("alert('�ļ����³ɹ���');");
                    }
                    catch (IOException ex)
                    {
                        RegisterScript("alert('�ļ�����ʧ�ܡ�ԭ��" + ex.Message + "');");
                    }
                    break;

                case ".xml":
                    try
                    {
                        //��ȡĬ��db.config�ļ�����
                        BaseConfigInfo bci = BaseConfigs.GetBaseConfig();
                        if (bci != null && bci.DBType != "" && bci.DBConnectionString != "")
                        {
                            Installer.ExcuteSQL(bci, UploadFile);
                        }
                        RegisterScript("alert('XML�ļ�ִ�гɹ���');");
                    }
                    catch (IOException ex)
                    {
                        RegisterScript("alert(''XML�ļ�ִ�г��ִ���ԭ��" + ex.Message + "');");
                    }
                    break;
            }
        }

        private string DeleteFiles()
        {
            string result = "";

            string fileStr = We7Request.GetString("delFiles");
            if (fileStr.Length > 0)
            {
                string[] filePathList = Utils.SplitString(fileStr, ",");
                int totalCount = 0;
                int errorCount = 0;
                foreach (string filePath in filePathList)
                {
                    bool exist = false;
                    bool isFile = false;
                    string physicalPath = "";
                    if (filePath.Length > 0)
                    {
                        totalCount++;
                        try
                        {
                            physicalPath = Server.MapPath(filePath);
                            isFile = (Path.GetExtension(physicalPath).Length > 0 ? true : false);
                            if (isFile)
                                exist = File.Exists(physicalPath);
                            else
                                exist = Directory.Exists(physicalPath);
                        }
                        catch
                        {

                        }
                        if (exist)
                        {
                            try
                            {
                                if (isFile)
                                {
                                    File.Delete(physicalPath);
                                }
                                else
                                {
                                    Directory.Delete(physicalPath, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                result += "<br/>��" + physicalPath + "�� ɾ��ʧ�ܣ�ԭ��" + ex.Message;


                                errorCount++;
                            }
                        }
                    }
                }
                result = "�����ļ��쳣�����ļ�����" + totalCount + ",����ɾ��ʧ�ܣ�" + errorCount + ",��ϸ��Ϣ��" + result;
                if (errorCount > 0)
                    We7.Framework.LogHelper.WriteLog(typeof(upgrade), result);
            }

            return result;
        }

        /// <summary>
        /// �ϴ����°�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (UpdateFileUpload.FileName == null || UpdateFileUpload.FileName == "")
            {
                RegisterScript("alert('����û��ѡ��Ҫ�ϴ����ļ���');");
                return;
            }

            string ap = GenerateFileName(Path.GetFileName(UpdateFileUpload.FileName));
            UploadFile = ap;
            string ext = Path.GetExtension(ap).ToLower();
            if (ext != ".zip" && ext != ".dll" && ext != ".xml")
            {
                RegisterScript("alert('���°���ʽ����ʶ�����飡');");
                return;
            }

            BackUpPanel.Visible = false;
            try
            {
                UpdateFileUpload.SaveAs(ap);

                switch (ext.ToLower())
                {
                    case ".zip":
                        //��ѹ���ļ���Temp�ļ���
                        UnZipPath = UnZipFile(ap);
                        if (UnZipPath == "")
                        {
                            RegisterScript("alert('�޷���ȷ��ѹ������ѹ���ļ��Ƿ�Ϊ�Ϸ���Zipѹ����ʽ��');");
                            return;
                        }
                        else
                        {
                            UnZipLiteral.Text = FileListSummary(UnZipPath);


                            try
                            {
                                //ѹ�����ѽ�ѹ ɾ��ѹ����
                                File.Delete(ap);
                            }
                            catch
                            {
                                We7.Framework.LogHelper.WriteLog(typeof(upgrade), "��ʱ�ļ���" + ap+"ɾ��ʧ�ܣ����ֶ�ɾ��");
                            }

                            //����Ƿ�����Ҫɾ�����ļ�
                            string pathJson = GetOperatorPath(UnZipPath);
                            ClientScript.RegisterStartupScript(this.GetType(), "", "$('#files').val('" + pathJson + "')", true);
                        }
                        BackUpPanel.Visible = true;

                        break;

                    case ".dll":
                        UnZipLiteral.Text = Path.GetFileName(ap) + "����ֱ�Ӹ��ǵ� bin Ŀ¼�£������ȷ��Ҫ���������������������ʼ���¡�ִ�и��£�";
                        break;

                    case ".xml":
                        UnZipLiteral.Text = Path.GetFileName(ap) + "Ϊ���ݿ�ṹ�����ļ���ϵͳ����ֱ�Ӹ��µ�ǰ���ݿ⣬�����ȷ��Ҫ���������������������ʼ���¡�ִ�и��£�";
                        break;

                    default:
                        RegisterScript("alert('���°���ʽ����ʶ�����飡');");
                        break;
                }

            }
            catch (IOException ex)
            {
                RegisterScript("alert('���°��ϴ�ʧ�ܡ�ԭ��" + ex.Message + "');");
                return;
            }

            CopyFilesButton.Enabled = true;
        }

        string GetOperatorPath(string path)
        {
            string strScript = "";
            path += @"\install\Files\Delete.xml";
            if (File.Exists(path))
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);
                    XmlNodeList nodeList = doc.SelectNodes("/Delete/File");
                  

                    if (nodeList != null && nodeList.Count > 0)
                    {
                        sb.Append("{\"Exist\":true,\"Files\":[");
                        foreach (XmlNode node in nodeList)
                        {
                            sb.Append("{\"Path\":\"" + node.InnerText + "\",\"Type\":\"" + node.Attributes["Type"].Value + "\"},");
                        }
                        sb.Append("]");
                        sb.Append("}");
                        strScript = sb.ToString();
                        strScript = We7.Framework.Util.Utils.JsonCharFilter(strScript.Remove(strScript.LastIndexOf(","), 1));

                        return strScript;
                    }
                }
                catch
                {
                   
                }
            }

            return "";
        }

        string GenerateFileName(string fileName)
        {
            string folderPath = Server.MapPath("/_temp");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fn = Path.Combine(folderPath, fileName);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
            return fn;
        }

        /// <summary>
        /// ��ѹ
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string UnZipFile(string file)
        {
            string path = "";
            if (String.Compare(Path.GetExtension(file), ".zip", true) == 0)
            {
                path = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file));
                if (Directory.Exists(path))
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    We7Helper.DeleteFileTree(dir);
                }

                FileStream s = File.OpenRead(file);
                ZipUtils.ExtractZip((Stream)s, path);
            }
            return path;
        }

        /// <summary>
        /// ѹ�����б�
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        string FileListSummary(string dir)
        {

            StringBuilder sb = new StringBuilder();


            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo[] ds = di.GetDirectories();
            FileInfo[] files = di.GetFiles();

            sb.Append("<p style='font-weight:bold'>���°������ļ�����,����鿴������ʾ�����б���� �����ļ� ���и��¸���</p>");
            sb.Append("<table cellSpacing='0' cellPadding='0' width='90%' border='0' bgcolor='#f8f8f8' style='font-size:12px;'>");

            sb.Append(string.Format("<tr ><td width=20><img src='images/folder.gif' /></td><td>{0}</td> <td>{1}</td></tr>", "<a id='showMore' show='t' href='javascript:void(0)' onclick=\"ShowMoreFiles('.moreFiles',this)\" >�鿴��ϸ</a>", ""));
            foreach (DirectoryInfo d in ds)
            {
                string rowFolder = "<tr class='moreFiles' style='display:none;'><td width=20><img src='images/folder.gif' /></td><td>{0}</td> <td>{1}</td></tr>";
                sb.Append(string.Format(rowFolder, d.Name, d.LastWriteTime));
            }

            foreach (FileInfo f in files)
            {
                string rowFile = "<tr class='moreFiles' style='display:none;'><td width=20><img src='images/file.gif' /></td><td>{0}</td> <td>{1}</td></tr>";
                sb.Append(string.Format(rowFile, f.Name, f.LastWriteTime));
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }
}
