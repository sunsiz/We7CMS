using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// ������
    /// </summary>
    [Serializable]
    public class Attachment
    {
        string id;
        string articleID;
        int sequenceIndex;
        string fileType;
        string fileName;
        long fileSize;
        string filePath;
        string description;
        DateTime uploadDate;
        int downloadTimes;
        string enumState;

        string imgPath;
        DateTime created=DateTime.Now;
        DateTime updated=DateTime.Now;

        public Attachment()
        {
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string ArticleID
        {
            get { return articleID; }
            set { articleID = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Index
        {
            get { return sequenceIndex; }
            set { sequenceIndex = value; }
        }

        /// <summary>
        /// �ļ�����
        /// </summary>
        public string FileType
        {
            get { return fileType; }
            set { fileType = value; }
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
        /// �ļ���С
        /// </summary>
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// �ļ���С��ʽ����ʾ����2.5M
        /// </summary>
        public string FileSizeText
        {
            get
            {
                return We7Helper.FormatFileSize(FileSize);
            }
        }

        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        /// <summary>
        /// ��ע
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// �ϴ�����
        /// </summary>
        public DateTime UploadDate
        {
            get { return uploadDate; }
            set { uploadDate = value; }
        }

        /// <summary>
        /// ���ش���
        /// </summary>
        public int DownloadTimes
        {
            get { return downloadTimes; }
            set { downloadTimes = value; }
        }

        /// <summary>
        /// ״̬
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        /// <summary>
        /// ͼƬ��ַ
        /// </summary>
        public string ImgPath
        {
            get
            {
                return "/images/icon_attach.gif";
            }
            set { imgPath = value; }
        }

        /// <summary>
        /// �������ص�ַ
        /// </summary>
        public string DownloadUrl
        {
            get
            {
                return string.Format("/go/AttachmentDownload.aspx?id={0}", ID);
            }
        }
    }
}
