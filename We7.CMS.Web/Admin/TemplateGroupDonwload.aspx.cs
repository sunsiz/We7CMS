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
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.Framework.Zip;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroupDonwload : BasePage
    {

        string FileName
        {
            get { return Request["file"]; }
        }

        protected TemplateGroup Data
        {
            get { return ViewState["$VS_TEMPLATEGROUP_DATA"] as TemplateGroup; }
            set { ViewState["$VS_TEMPLATEGROUP_DATA"] = value; }
        }


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

        protected override void Initialize()
        {
            Data = TemplateHelper.GetTemplateGroup(FileName);
            TemplateGroupNameLabel.Text = Path.GetFileNameWithoutExtension(FileName);
            if (EnableSiteSkins)
                CreateTempFile();
            else
                CreateZip(FileName);
        }

        protected void CreateZip(string FileName)
        {
            //string CopyToPath = Server.MapPath(Constants.TempBasePath);//Ŀ���ļ���
            string CopyToPath = String.Format("{0}\\Templates.{1}\\controls", Server.MapPath("~/_temp/"), Path.GetFileNameWithoutExtension(FileName));
            //��Ŀ���ļ��� �����ļ���.Files�����ļ���
            string stylesPath = String.Format("{0}\\styles", CopyToPath);//css�ļ�Ŀ¼
            string groupsPath = String.Format("{0}\\groups", CopyToPath);//ģ�����ļ�Ŀ¼
            string imagePath = String.Format("{0}\\images", CopyToPath);//ģ�����ļ�Ŀ¼
            Directory.CreateDirectory(stylesPath);
            Directory.CreateDirectory(groupsPath);

            //ģ����JpgԤ��ͼƬ�ļ�
            string JpgFile = Server.MapPath(String.Format("/{0}/{1}.jpg", Constants.TemplateGroupBasePath, FileName));
            //ģ����Xml�ļ�
            string XmlFile = Server.MapPath(String.Format("/{0}/{1}", Constants.TemplateGroupBasePath, FileName));
            
            

            //����ģ�����ļ�
            if (File.Exists(JpgFile))
            {
                File.Copy(JpgFile, String.Format("{0}/{1}.jpg", groupsPath, FileName), true);
            }
            if (File.Exists(XmlFile))
            {
                File.Copy(XmlFile, String.Format("{0}/{1}", groupsPath, FileName), true);
            }

            string imgPath = String.Format("{0}/images", Server.MapPath(Constants.TemplateBasePath));
            CopyFolder(imagePath, imgPath,true);
            string templateFile = Server.MapPath(Constants.TemplateBasePath);
            CopyFolder(CopyToPath,templateFile,false);
            string cssPath = String.Format("{0}/styles", Server.MapPath(Constants.TemplateBasePath));
            CopyFolder(stylesPath, cssPath, true);
            //����res�ļ���
            string resPath = String.Format("{0}\\Templates.{1}\\res", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));
            Directory.CreateDirectory(resPath);
            //�����汾�ļ�
            //CreateVersion(resPath);

            string[] FileProperties = new string[2];
            FileProperties[0] = String.Format("{0}\\Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName)); ;//ѹ��Ŀ¼
            //FileProperties[1] = String.Format("{0}/{1}.zip", Server.MapPath(Constants.TemplateGroupBasePath), FileName);//ѹ�����Ŀ¼
            FileProperties[1] = String.Format("{0}/Templates.{1}.zip", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));//ѹ�����Ŀ¼
            //ѹ���ļ�
            try
            {
                ZipClass.ZipFileMain(FileProperties);
                DonwloadHyperLink.NavigateUrl = String.Format("~/{0}/Templates.{1}.zip", Constants.TempBasePath.TrimStart('\\').Trim('/'), Path.GetFileNameWithoutExtension(FileName));
                DeleteFolder(String.Format("{0}\\Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName)));
            }
            catch
            {

            }
        }

        void CreateTempFile()
        {
            string DirectoryName = String.Format("Templates.{0}", Path.GetFileNameWithoutExtension(FileName));
            //Ŀ���ļ���
            string CopyToPath = String.Format("{0}/Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));
            //Դ�ļ���
            string CopyFromPath = TemplateHelper.TemplateGroupPath;

            CopyFromPath = String.Format("{0}/{1}", CopyFromPath, Path.GetFileNameWithoutExtension(FileName));
            CopyToPath = String.Format("{0}/{2}/Skin/{1}", CopyToPath, Path.GetFileNameWithoutExtension(FileName), DirectoryName);
            //����ģ���ļ���
            CopyFolder(CopyToPath, CopyFromPath,true);

            //Ԥ��ͼƬԴ
            string JpgFile = String.Format("{0}.xml.jpg", CopyFromPath);
            //Xml�ļ�Դ
            string XmlFile = String.Format("{0}.xml", CopyFromPath);

            CopyToPath = String.Format("{0}/Templates.{1}", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));//Ŀ���ļ���
            //����XML�ļ���Ԥ��ͼƬ
            if (File.Exists(JpgFile))
            {
                string str = String.Format("{0}/{2}/Skin/{1}.xml.jpg", CopyToPath, Path.GetFileNameWithoutExtension(FileName), DirectoryName);
                File.Copy(JpgFile, str, true);
            }
            if (File.Exists(XmlFile))
            {
                File.Copy(XmlFile, String.Format("{0}/{2}/Skin/{1}", CopyToPath, FileName, DirectoryName), true);
            }

            //����res�ļ���
            //string resPath = String.Format("{0}/res", CopyToPath);
            //Directory.CreateDirectory(resPath);
            //�����汾�ļ�
            //CreateVersion(resPath);

            //TODO:css��ascx�ļ�·������
            //���
            string[] FileProperties = new string[2];
            FileProperties[0] = CopyToPath;//ѹ��Ŀ¼
            FileProperties[1] = String.Format("{0}/Templates.{1}.zip", Server.MapPath(Constants.TempBasePath), Path.GetFileNameWithoutExtension(FileName));//ѹ�����Ŀ¼
            //ѹ���ļ�
            try
            {
                ZipClass.CreateTemplateZip(FileProperties);
                DonwloadHyperLink.NavigateUrl = String.Format("~/{0}/Templates.{1}.zip", Constants.TempBasePath.TrimStart('/').TrimStart('\\'), Path.GetFileNameWithoutExtension(FileName));
                DeleteFolder(CopyToPath);
            }
            catch
            {

            }
        }

        void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //�����������ļ���ɾ��֮ 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //ֱ��ɾ�����е��ļ� 
                    else
                        DeleteFolder(d); //�ݹ�ɾ�����ļ��� 
                }
                Directory.Delete(dir); //ɾ���ѿ��ļ��� 
            }
        }

        void CopyStyleFile(string templateName, string tempPath, string filePath)
        {
            FileInfo[] styleFiles = GetCssStyles(templateName,filePath);
            foreach (FileInfo f in styleFiles)
            {
                //����ģ���CSS�ļ�
                File.Copy(f.FullName, String.Format("{0}\\{1}", tempPath, f.Name));
                File.SetAttributes(String.Format("{0}\\{1}", tempPath, f.Name), FileAttributes.Normal);
            }
        }
        /// <summary>
        /// �õ�ģ���CSS�ļ��б�
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns`></returns>
        public FileInfo[] GetCssStyles(string queryName, string cssPath)
        {
            string styleName = String.Format("{0}_CD", queryName);
            DirectoryInfo di = new DirectoryInfo(cssPath);
            FileInfo[] files = di.GetFiles(String.Format("{0}*.css", styleName), SearchOption.TopDirectoryOnly);
            return files;
        }

        void CopyFolder(string aimPath, string srcPath, bool isDirectory)
        {
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                aimPath += Path.DirectorySeparatorChar;

            if (!Directory.Exists(aimPath))
                Directory.CreateDirectory(aimPath);

            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            foreach (string file in fileList)
            {
                //   �ȵ���Ŀ¼��������������Ŀ¼�͵ݹ�Copy��Ŀ¼������ļ�
                if (isDirectory)
                {
                    if (Directory.Exists(file))
                    {
                        CopyFolder(aimPath + Path.GetFileName(file), file,true);
                    }
                    //   ����ֱ��Copy�ļ�   
                    else
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                        File.SetAttributes(aimPath + Path.GetFileName(file), FileAttributes.Normal);
                    }
                }
                else
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                        File.SetAttributes(aimPath + Path.GetFileName(file), FileAttributes.Normal);
                    }
                }
            }
        }

        void CreateVersion(string basePath)
        {
            TemplateVersion tv = new TemplateVersion();
            GeneralConfigInfo si = GeneralConfigs.GetConfig();

            tv.TemplatePath = Constants.TemplateBasePath;
            tv.Version = si.ProductVersion;
            tv.UseSkin = EnableSiteSkins;
            tv.FileName = "TemplateVersion.config";
            tv.BasePath = basePath;

            TemplateVersionHelper.SaveTemplateVersion(tv);
        }

    }

}
