using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace We7.CMS.Common
{
    /// <summary>
    /// �ļ���Ϣ��
    /// </summary>
    [Serializable]
    public class FileItem
    {
        string fullName;
        string name;
        bool isDirectory;
        string fileType;
        string size;
        string created;
        string url;
        string icon;

        /// <summary>
        /// �ļ���Ϣ�๹�캯��
        /// </summary>
        public FileItem()
        {
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        /// <summary>
        /// �Ƿ���ʾĿ¼
        /// </summary>
        public bool IsDirectory
        {
            get { return isDirectory; }
            set { isDirectory = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public string Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// ��С
        /// </summary>
        public string Size
        {
            get { return size; }
            set { size = value; }
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
        /// ͼƬ
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// ·��
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
    }

    /// <summary>
    /// Ŀ¼�����
    /// </summary>
    public class DirectoryDiscover
    {
        string basePath;
        string path;
        List<FileItem> items;
        string folderUrl;
        string fileUrl="{0}";
        string filter;
        string folderFilter;
        bool autoCreate;
        bool onlyFolder = false;

        /// <summary>
        /// �Ƿ�ֻ���ļ���
        /// </summary>
        public bool OnlyFolder
        {
            get { return onlyFolder; }
            set { onlyFolder = value; }
        }

        /// <summary>
        /// Ŀ¼���
        /// </summary>
        public DirectoryDiscover()
        {
            items = new List<FileItem>();
            Filter = "*";
            FolderFilter = "*";
        }

        /// <summary>
        /// ·������
        /// </summary>
        public string PathName
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// �ļ���Ϣ���϶���
        /// </summary>
        public List<FileItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// ԭʼ·��
        /// </summary>
        public string BasePath
        {
            get { return basePath; }
            set { basePath = value; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        /// <summary>
        /// �ļ��й�����
        /// </summary>
        public string FolderFilter
        {
            get { return folderFilter; }
            set { folderFilter = value; }
        }

        /// <summary>
        /// �ļ���·��
        /// </summary>
        public string FolderUrl
        {
            get { return folderUrl; }
            set { folderUrl = value; }
        }

        /// <summary>
        /// �ļ�·��
        /// </summary>
        public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }

        /// <summary>
        /// �������ͻ�ȡͼƬ
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetIcon(string type)
        {
            switch (type.ToLower())
            {
                case ".bmp":
                case ".jpg":
                case ".gif":
                case ".jpeg":
                case ".tiff":
                    return "gif.gif";

                case ".doc":
                    return "doc.gif";

                case ".zip":
                case ".rar":
                    return "zip.gif";

                case ".cs":
                    return "code.gif";

                case ".pdf":
                    return "pdf.gif";

                default:
                    return "file.gif";
            }
        }

        /// <summary>
        /// �Ƿ��Զ�����
        /// </summary>
        public bool AutoCreate
        {
            get { return autoCreate; }
            set { autoCreate = value; }
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        public void Process()
        {
            Items.Clear();

            string dir = Path.Combine(BasePath, PathName);

            if (!Directory.Exists(dir))
            {
                if (AutoCreate)
                {
                    DirectoryInfo newDir = new DirectoryInfo(dir);
                    newDir.Create();
                }
                else
                {
                    return;
                }
            }

            if (!(PathName == "" && FolderFilter == "_*" && OnlyFolder))//��ʾ��Ŀ¼�������ļ���
            {
                string fn = Path.Combine(dir, "forbid");
                if (File.Exists(fn))
                {
                    return;
                }
            }

            DirectoryInfo di = new DirectoryInfo(dir);
            DirectoryInfo[] ds = di.GetDirectories(FolderFilter);
            foreach (DirectoryInfo d in ds)
            {
                FileItem it = new FileItem();
                it.Created = d.CreationTime.ToString();
                it.FileType = "�ļ���";
                it.FullName = Path.Combine(PathName, d.Name);
                it.FullName = it.FullName.Replace("\\", "/");
                it.IsDirectory = true;
                it.Name = d.Name;
                it.Size = "";
                it.Url = String.Format(FolderUrl, it.FullName,it.Name);
                it.Icon = "folder.gif";
                Items.Add(it);
            }
            if (!OnlyFolder)
            {
                FileInfo[] files = di.GetFiles(Filter);
                foreach (FileInfo f in files)
                {
                    FileItem it = new FileItem();
                    it.Created = f.CreationTime.ToString();
                    it.FileType = Path.GetExtension(f.Name);
                    if (it.FileType.StartsWith("."))
                    {
                        it.FileType.Remove(0, 1);
                    }
                    it.FullName = Path.Combine(PathName, f.Name);
                    it.FullName = it.FullName.Replace("\\", "/");
                    if (!it.FullName.StartsWith("/")) it.FullName = "/" + it.FullName;

                    it.IsDirectory = false;
                    it.Name = f.Name;
                    it.Size = String.Format("{0:N0}", f.Length);
                    it.Url = String.Format(FileUrl, it.FullName, it.Name);
                    it.Icon = GetIcon(it.FileType);
                    Items.Add(it);
                }
            }
        }
    }
}
