using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.Framework.Zip;

namespace We7.CMS
{
    /// <summary>
    /// ��Դ�ϴ�������
    /// </summary>
    [Serializable]
    public abstract class Uploader
    {
        string temporaryPath;
        string fileName;
        string uniqueID;
        string webRoot;
        string deployGroupPath;

        /// <summary>
        /// �ļ��ϴ�
        /// </summary>
        public Uploader()
        {
            uniqueID = We7Helper.CreateNewID();
        }

        /// <summary>
        /// UID
        /// </summary>
        public string UniqueID
        {
            get { return uniqueID; }
        }

        /// <summary>
        /// ��ʱ�ļ�·��
        /// </summary>
        public string TemporaryPath
        {
            get { return temporaryPath; }
            set { temporaryPath = value; }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// ��վ��·��
        /// </summary>
        public string WebRoot
        {
            get { return webRoot; }
            set { webRoot = value; }
        }

        /// <summary>
        /// ����·��
        /// </summary>
        public string BasePath
        {
            get { return Path.Combine(TemporaryPath, uniqueID); }
        }

        /// <summary>
        /// �Ƿ�������վƤ��
        /// </summary>
        public static bool EnableSiteSkins
        {
            get
            {
                string _default = SiteSettingHelper.Instance.Config.EnableSiteSkins;
                if (_default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        /// <summary>
        /// �ؼ�·��
        /// </summary>
        protected abstract string ControlPath
        {
            get;
        }

        /// <summary>
        /// ��Դ·��
        /// </summary>
        protected abstract string ResourcePath
        {
            get;
        }

        /// <summary>
        /// �ļ���չ��
        /// </summary>
        protected abstract string FileExtension
        {
            get;
        }

        /// <summary>
        /// �ؼ�����·��
        /// </summary>
        protected abstract string DeployControlPath
        {
            get;
        }

        public string SkinPath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_skins"); }
        }

        /// <summary>
        /// ��Դ·��
        /// </summary>
        protected abstract string DeployResourcePath
        {
            get;
        }

        /// <summary>
        /// �����ģ����
        /// </summary>
        public string DeployGroupPath
        {
            get { return deployGroupPath; }
            set { deployGroupPath = value; }
        }

        /// <summary>
        /// ������Դ·��
        /// </summary>
        public string DeployResPath
        {
            get { return Path.Combine(WebRoot, Constants.TempBasePath); }
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="file">�ļ���</param>
        protected abstract void ProcessFile(FileInfo file);

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="file">�ļ���</param>
        /// <param name="templateGroupName">ģ����</param>
        protected abstract void ProcessFile(FileInfo file, string templateGroupName);

        /// <summary>
        /// ѹ���ļ�
        /// </summary>
        /// <param name="zipName">ѹ���ļ���</param>
        public void Process(string zipName)
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            using (FileStream fs = File.Open(FileName, FileMode.Open))
            {
                ZipUtils.ExtractZip(fs, BasePath);
            }

            string templatePath = ControlPath;

            HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            TemplateVersionHelper templateVersionHelper = helperFactory.GetHelper<TemplateVersionHelper>();

            DirectoryInfo di = new DirectoryInfo(BasePath);
            FileInfo[] fis=di.GetFiles("*.xml");
            foreach (FileInfo f in fis)
            {
                try
                {
                    ProcessFile(f);
                }
                catch { }
            }
            //TemplateVersion tv = templateVersionHelper.GetTemplateVersion(String.Format("{0}\\res", BasePath));
            //if (tv != null)
            //{
            //    if (tv.TemplatePath == "cgi-bin\\templates\\groups")
            //        tv.TemplatePath = "cgi-bin\\templates";
            //    DeployGroupPath = Path.Combine(WebRoot, tv.TemplatePath);

            //    if (tv.UseSkin)
            //    {
            //        templatePath = String.Format("{0}\\{1}", ControlPath, zipName.Replace("Package.Templates.", ""));
            //    }
            //    if (Directory.Exists(templatePath))
            //    {
            //        DirectoryInfo di = new DirectoryInfo(templatePath);
            //        foreach (FileInfo fi in di.GetFiles(FileExtension))
            //        {
            //            if (tv.UseSkin)
            //            {
            //                ProcessFile(fi, templatePath);
            //            }
            //            else
            //            {
            //                ProcessFile(fi);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    DeployGroupPath = DeployControlPath;
            //    if (DeployControlPath == "_skins")
            //        DeployGroupPath = "_templates";
            //    if (Directory.Exists(ControlPath))
            //    {
            //        DirectoryInfo di = new DirectoryInfo(ControlPath);
            //        foreach (FileInfo fi in di.GetFiles(FileExtension))
            //        {
            //            ProcessFile(fi);
            //        }
            //    }
            //}
        }

       ��/// <summary>
       ��/// �����ļ�
       ��/// </summary>
        public void Process()
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            using (FileStream fs = File.Open(FileName, FileMode.Open))
            {
                ZipUtils.ExtractZip(fs, BasePath);
            }

            if (Directory.Exists(ControlPath))
            {
                DirectoryInfo di = new DirectoryInfo(ControlPath);
                foreach (FileInfo fi in di.GetFiles(FileExtension))
                {
                    ProcessFile(fi);
                }
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        public void Deploy()
        {
            if (Directory.Exists(BasePath))
            {
                if (!Directory.Exists(SkinPath))
                    Directory.CreateDirectory(SkinPath);
                We7Helper.CopyDirectory(BasePath, SkinPath);
            }
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        public void Clean()
        {
            if (Directory.Exists(BasePath))
            {
                Directory.Delete(BasePath, true);
            }
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }
    }
}
