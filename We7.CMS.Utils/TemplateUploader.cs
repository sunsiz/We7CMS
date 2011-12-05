using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// ģ���ϴ�������
    /// </summary>
    [Serializable]
    public class TemplateUploader : Uploader
    {
        List<Template> templates;

        /// <summary>
        /// ���캯��
        /// </summary>
        public TemplateUploader()
        {
            templates = new List<Template>();
        }

        /// <summary>
        /// ģ����
        /// </summary>
        public Template[] Templates
        {
            get { return templates.ToArray(); }
        }

        /// <summary>
        /// �ؼ�Ŀ¼
        /// </summary>
        protected override string ControlPath
        {
            get { return Path.Combine(BasePath, "controls"); }
        }

        /// <summary>
        /// �ؼ�����Ŀ¼
        /// </summary>
        protected override string DeployControlPath
        {
            get { return Path.Combine(WebRoot, Constants.TemplateBasePath); }
        }

        /// <summary>
        /// �ؼ�����Ŀ¼
        /// </summary>
        protected override string DeployResourcePath
        {
            get { return WebRoot; }
        }

        /// <summary>
        /// �ļ���չ��
        /// </summary>
        protected override string FileExtension
        {
            get { return "*" + Constants.TemplateFileExtension; }
        }

        /// <summary>
        /// ��Դ·��
        /// </summary>
        protected override string ResourcePath
        {
            get { return Path.Combine(BasePath, "res"); }
        }

        /// <summary>
        ///  �����ļ�
        /// </summary>
        /// <param name="file">�ļ�����</param>
        protected override void ProcessFile(FileInfo file)
        {
            Template tp = new Template();
            //tp.FromFile(ControlPath, file.Name);
            tp.FromFile(file.Directory.FullName,file.Name);
            templates.Add(tp);
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="file">�ļ�����</param>
        /// <param name="templatePath">ģ��·��</param>
        protected override void ProcessFile(FileInfo file, string templatePath)
        {
            Template tp = new Template();

            tp.FromFile(templatePath, file.Name);
            templates.Add(tp);
        }
    }
}
