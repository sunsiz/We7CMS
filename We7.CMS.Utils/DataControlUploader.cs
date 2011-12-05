using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// �¿ؼ��ϴ���
    /// </summary>
    [Serializable]
    public class DataControlUploader : Uploader
    {
        List<DataControl> controls;

        /// <summary>
        /// ���캯��
        /// </summary>
        public DataControlUploader()
            : base()
        {
            controls = new List<DataControl>();
        }

        /// <summary>
        /// ���пؼ�
        /// </summary>
        public DataControl[] Controls
        {
            get { return controls.ToArray(); }
        }

        /// <summary>
        /// �ؼ�·��
        /// </summary>
        protected override string ControlPath
        {
            get { return Path.Combine(BasePath, "controls"); }
        }

        /// <summary>
        /// �ؼ�����·��
        /// </summary>
        protected override string DeployControlPath
        {
            get { return Path.Combine(WebRoot, Constants.ControlBasePath); }
        }

        /// <summary>
        /// �ؼ�������Դ
        /// </summary>
        protected override string DeployResourcePath
        {
            get { return WebRoot; }
        }

        /// <summary>
        /// ͨ�����ʾ��·��
        /// </summary>
        protected override string FileExtension
        {
            get { return "*" + Constants.ControlFileExtension; }
        }

        /// <summary>
        /// ��Դ·��
        /// </summary>
        protected override string ResourcePath
        {
            get { return Path.Combine(BasePath, "res"); }
        }

        /// <summary>
        /// ��ӿؼ���Ϣ
        /// </summary>
        /// <param name="file">�ؼ������ļ�</param>
        protected override void ProcessFile(FileInfo file)
        {
            DataControl dc = new DataControl();
            dc.FromFile(ControlPath, file.Name);
            controls.Add(dc);
        }

        /// <summary>
        /// ��ӿؼ���Ϣ
        /// </summary>
        /// <param name="file">�ؼ������ļ�</param>
        /// <param name="templateGroupName">ģ����</param>
        protected override void ProcessFile(FileInfo file, string templateGroupName)
        {
        }
    }
}
