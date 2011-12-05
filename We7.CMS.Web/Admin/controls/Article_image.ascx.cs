using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using We7.CMS.Config;
using We7.CMS;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.Framework;

namespace We7.CMS.Web.Admin.controls
{
	/// <summary>
	/// ����ͼ�ϴ�
	/// </summary>
	public partial class Article_image : BaseUserControl
	{
		#region ����
		private string reName;
		/// <summary>
		/// ����ǲ��Ǳ༭��ͬ����Ƭ����ͼ�ı�����
		/// Ŀ���Ǳ�ʶ�ǲ�����Ҫ�滻�ļ�����
		/// </summary>
		public string ReName
		{
			get
			{
				if (this.Request.Form["reName"] != null)
				{
					this.reName = this.Request.Form["reName"].Trim();
				}
				return reName;
			}

		}
		public string OwnerID
		{
			get
			{
				if (Request["oid"] != null)
					return Request["oid"];
				else
				{
					if (ViewState["$VS_OwnerID"] == null)
					{
						if (ArticleID != null)
						{
							Article a = ArticleHelper.GetArticle(ArticleID, null);
							ViewState["$VS_OwnerID"] = a.OwnerID;
						}
					}
					return (string)ViewState["$VS_OwnerID"];
				}
			}
		}

		public string ArticleID
		{
			get { return Request["id"]; }
		}
		public string ImagePaths
		{
			get
			{
				string imagepath = "0";
				if (ArticleID != null && ArticleID != "")
				{
					Article a = ArticleHelper.GetArticle(ArticleID);
					if (a != null && a.Thumbnail != null && a.Thumbnail != "")
					{
						imagepath = "1";
					}
				}
				return imagepath;
			}
		}
		public string Succeed
		{
			get { return Request["succeed"]; }
		}
		GeneralConfigInfo config;
		/// <summary>
		/// ˮӡ����
		/// </summary>
		public GeneralConfigInfo ImageConfig
		{
			get
			{
				if (config == null)
					config = GeneralConfigs.Deserialize(We7Utils.GetMapPath("~/Config/general.config"));
				return config;
			}
		}
		public string OriginalImagePath { get; set; }

		public int width;
		public int height;
		public string bigHeadImage;

		#endregion

		/// <summary>
		/// ҳ�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Request["uploaded"] != null)
					Messages.ShowMessage("���ѳɹ��ϴ�ԭͼ" + Request["uploaded"].ToString() + " ��");
				else if (Request["generated"] != null)
					Messages.ShowMessage(string.Format("���ɹ����ɱ�ǩΪ {0} ������ͼ��", Request["generated"].ToString()));

				InitControls();
				InitSizeDroplistFromXML();
				InitOriginalImagePath();
				LoadImages();
			}
			Context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
		}

		#region ��ʼ��
		/// <summary>
		/// ���ݳ�ʼ��
		/// </summary>
		void InitControls()
		{
			UploadImage.Attributes["onclick"] = "return articleImageCheck('" + this.ClientID + "','" + ImagePaths + "');";
			SizesDropDownList.Attributes["onchange"] = "changeFrame(this)";
			SizesDropDownList.Attributes["onblur"] = "saveImageSizeToCookies(this)";
		}

		/// <summary>
		/// ��ʼ��ԭͼ
		/// </summary>
		void InitOriginalImagePath()
		{
			string imagepath = "/admin/images/article_small.gif";
			Article a = ArticleHelper.GetArticle(ArticleID);
			if (a.Thumbnail != null && a.Thumbnail.Length > 0)
			{
				string path = a.Thumbnail;
				path = Server.MapPath(path);
				string filename = Path.GetFileNameWithoutExtension(path);
				if (!filename.Contains("_S") && File.Exists(path))
				{
					imagepath = a.Thumbnail;
				}
				if (filename.Contains("_S"))
				{
					path = path.Replace("_S", "");
					if (File.Exists(path))
					{
						imagepath = a.Thumbnail.Replace("_S", "");
					}
				}
			}
			OriginalImagePath = imagepath;
			ThumbnailImages imageHelp = new ThumbnailImages();
			ImageInformation imageInfo = (ImageInformation)imageHelp.GetImageInfo(Server.MapPath(OriginalImagePath));
			width = imageInfo.Width;
			height = imageInfo.Height;
		}

		/// <summary>
		/// ��������ͼ�б�
		/// </summary>
		void LoadImages()
		{
			if (ArticleID != null)
			{
				Article a = ArticleHelper.GetArticle(ArticleID);
				List<ImageDetail> imageList = new List<ImageDetail>();
				string folderPath = ExistFilePath();
				if (!Directory.Exists(folderPath))
				{
					return;
				}
				else
				{
					List<ThumbnailConfig> configList = GetAllThumbnailConfigs();
					System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
					//string[] fields = new string[] { "ChannelFolder" };
					//Channel ch = ChannelHelper.GetChannel(OwnerID, fields);
					//string aid = Helper.GUIDToFormatString(ArticleID);
					StringBuilder sb = new StringBuilder();
					foreach (System.IO.FileInfo fi in dir.GetFiles())
					{
						if (Server.MapPath(a.Thumbnail) == fi.FullName)
						{
							continue;
						}
						ImageDetail image = new ImageDetail();
						image.ImagePath = string.Format("/{0}/{1}", VFolderPath.Trim('/'), fi.Name);

						image.ImagePath = image.ImagePath.Replace("\\", "/");
						string idChar = fi.Name.Substring(fi.Name.LastIndexOf("_") + 1, fi.Name.LastIndexOf(".") - fi.Name.LastIndexOf("_") - 1);
						ThumbnailConfig config = SearchThumbnailConfig(configList, idChar);
						image.FileName = fi.Name;
						if (config != null)
						{
							image.Name = config.Name;
							image.Tag = config.Tag;
							imageList.Add(image);
						}
					}
				}

				ImagesRepeater.DataSource = imageList;
				ImagesRepeater.DataBind();
			}
		}

		/// <summary>
		/// ���ݱ�ǩtag����ȡ����ͼ��Ӧ���
		/// </summary>
		/// <param name="list"></param>
		/// <param name="IdentityChar"></param>
		/// <returns></returns>
		ThumbnailConfig SearchThumbnailConfig(List<ThumbnailConfig> list, string IdentityChar)
		{
			ThumbnailConfig tc = new ThumbnailConfig();
			if (list != null && list.Count > 0)
			{
				foreach (ThumbnailConfig thumbnailConfig in list)
				{
					if (thumbnailConfig.Tag == IdentityChar)
					{
						tc = thumbnailConfig;
						return tc;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// �������й���б�
		/// </summary>
		/// <returns></returns>
		List<ThumbnailConfig> GetAllThumbnailConfigs()
		{
			List<ThumbnailConfig> thumbnailConfigList = Context.Cache["$THUMBNAILCONFIGLIST"] as List<ThumbnailConfig>;

			if (thumbnailConfigList == null)
			{
				if (File.Exists(Server.MapPath("/Config/thumbnail.xml")))
				{
					thumbnailConfigList = new List<ThumbnailConfig>();
					XmlDocument doc = new XmlDocument();
					doc.Load(Server.MapPath("/Config/thumbnail.xml"));
					XmlNodeList ItemListNodes = doc.SelectNodes("/configuration/item");

					foreach (XmlNode oldNode in ItemListNodes)
					{
						ThumbnailConfig tc = new ThumbnailConfig();
						tc.Name = oldNode.Attributes["name"].Value;
						string[] v = tc.Name.Split(new string[] { ":", "��" }, StringSplitOptions.RemoveEmptyEntries);
						if (v.Length > 1)
							tc.Value = v[1];
						tc.Tag = oldNode.Attributes["value"].Value;
						thumbnailConfigList.Add(tc);
					}
					ChannelHelper.CacherCache("$THUMBNAILCONFIGLIST", Context, thumbnailConfigList, CacheTime.Short);
				}
			}
			return thumbnailConfigList;
		}

		/// <summary>
		/// �������ļ���������ͼ���
		/// </summary>
		void InitSizeDroplistFromXML()
		{
			//С����ͼ
			SizesDropDownList.Items.Clear();
			ListItem item = new ListItem("-��ѡ���С���- ", "");
			SizesDropDownList.Items.Add(item);
			List<ThumbnailConfig> configList = GetAllThumbnailConfigs();
			foreach (ThumbnailConfig config in configList)
			{
				item = new ListItem(config.Name, config.Tag);
				SizesDropDownList.Items.Add(item);
			}

			SizesDropDownList.SelectedIndex = 0;
			SizesDropDownList.Items[0].Selected = true;
		}

		#endregion

		#region ͼƬ�ϴ�
		/// <summary>
		/// ͼƬ�ϴ�
		/// </summary>
		void UploadImageFile()
		{
			if (ImageFileUpload.FileName.Length < 1)
			{
				this.Messages.ShowError("��ѡ��ͼƬ�ļ����ϴ���");
				return;
			}
			if (!CDHelper.CanUpload(ImageFileUpload.FileName))
			{
				this.Messages.ShowError("ϵͳ��֧���ϴ������͵�ͼƬ�����ڱ���ת��Ϊgif��jpg��ʽ�����ԡ�");
				return;
			}

			try
			{
				UploadAndCreateThumbnails(Path.GetFileName(ImageFileUpload.FileName));
				Response.Redirect(Request.RawUrl + "&uploaded=" + Path.GetFileName(ImageFileUpload.FileName));
			}
			catch (IOException)
			{
				this.Messages.ShowError("ͼƬ�ϴ�ʧ��,������!");
				return;
			}
		}

		/// <summary>
		/// ��ȡҪ�ϴ�ͼƬ·��
		/// </summary>
		/// <returns></returns>
		public string ExistFilePath()
		{
			return Server.MapPath(VFolderPath);
		}

		private string vFolderPath;
		/// <summary>
		/// ����ͼ�����ļ��е����·��
		/// </summary>
		public string VFolderPath
		{
			get
			{
				if (string.IsNullOrEmpty(vFolderPath))
				{
					Article article = ArticleHelper.GetArticle(ArticleID, null);
					if (!string.IsNullOrEmpty(article.Thumbnail))
					{
						vFolderPath = Server.MapPath(article.Thumbnail.Remove(article.Thumbnail.LastIndexOf("/")));
						if (Directory.Exists(vFolderPath)) vFolderPath = article.Thumbnail.Remove(article.Thumbnail.LastIndexOf("/"));
						else vFolderPath = article.AttachmentUrlPath;
					}
					else vFolderPath = article.AttachmentUrlPath;
				}
				return vFolderPath;
			}
		}

		/// <summary>
		/// �ϴ�ԭͼ
		/// </summary>
		/// <param name="fileName"></param>
		void UploadAndCreateThumbnails(string fileName)
		{
			string folderPath = ExistFilePath();
			//�ж�·���Ƿ����
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}
			//�ϴ���ͼƬ��·��
			string fn = Path.Combine(folderPath, fileName);

			//�ϴ�
			ImageFileUpload.SaveAs(fn);

			//�ļ���׺
			string ext = Path.GetExtension(fn);

			//ȥ����׺���ļ���
			string imgName = string.Format("{0}", Path.GetFileNameWithoutExtension(fn));

			string originalFilePath = CheckFileName(imgName, ext, folderPath, "");
			//ɾ��ԭͼ
			Article article = ArticleHelper.GetArticle(ArticleID, null);

			if (article != null && article.Thumbnail != null && article.Thumbnail != "")
			{
				string thumbnail = article.Thumbnail.Replace("_S", "");
				if (File.Exists(Server.MapPath(thumbnail)))
				{
					File.Delete(Server.MapPath(thumbnail));
				}
			}

			SaveImageToDB(originalFilePath);
		}
		/// <summary>
		/// ��������ͼԴ�ļ��������ݿ�
		/// </summary>
		/// <param name="thumbnailFile"></param>
		void SaveImageToDB(string thumbnailFile)
		{
			string rootPath = Server.MapPath("/");
			thumbnailFile = thumbnailFile.Remove(0, rootPath.Length);
			thumbnailFile = string.Format("/{0}", thumbnailFile.Replace("\\", "/"));

			Article a = new Article();
			a.ID = ArticleID;
			a.Thumbnail = thumbnailFile;
			a.IsImage = 1;

			if (a.ID == null)
			{
				Article article = ArticleHelper.AddArticles(a);
			}
			else
			{
				string[] fields = new string[] { "Thumbnail", "WapImage", "OriginalImage", "IsImage" };
				ArticleHelper.UpdateArticle(a, fields);
			}
		}

		/// <summary>
		/// �ϴ�ͼƬ��ť����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void UploadImage_ServerClick(object sender, EventArgs e)
		{
			UploadImageFile();
		}


		#endregion

		#region ��������ͼ

		/// <summary>
		/// ��������ͼ
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="ext"></param>
		/// <param name="sn"></param>
		string GenerateThumbImage(string originalFilePath, string ext, string sn)
		{
			string smallCutType = "Cut";
			if (CutTypeDropDownList.SelectedIndex > 0)
				smallCutType = CutTypeDropDownList.SelectedItem.Value; ;
			string type = SizesDropDownList.SelectedValue;
			if (type != "")
			{
				string[] units = SizesDropDownList.SelectedItem.Text.Split(new string[] { ":", "��" }, StringSplitOptions.RemoveEmptyEntries);
				string imageSize = "";
				if (units.Length > 1)
					imageSize = units[1];
				else
					imageSize = units[0];
				string[] izeSplit = imageSize.Split('*');
				int width = int.Parse(izeSplit[0]);
				int height = int.Parse(izeSplit[1]);
				string folderPath = ExistFilePath();
				string thumbnailFilePath = CheckFileName(sn + "_" + type, ext, folderPath, "");
				System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folderPath);
				foreach (System.IO.FileInfo fi in dir.GetFiles())
				{
					if (fi.Name.Contains(type))
					{
						fi.Delete();
					}
				}
				if (smallCutType == "CustomerCut")
					GenarateHandTypeThumbImage(originalFilePath, thumbnailFilePath);
				else
					ImageUtils.MakeThumbnail(originalFilePath, thumbnailFilePath, width, height, smallCutType);

				if (AddWatermarkCheckbox.Checked)
					AddWatermarkToImage(thumbnailFilePath, originalFilePath);

				return type;
			}
			else
			{
				Messages.ShowError("����ͼ�ߴ����ò��Ϸ���");
				return "";
			}

		}

		/// <summary>
		/// ��ȡ����ͼ�ļ���
		/// </summary>
		/// <param name="fileName">�ļ���</param>
		/// <param name="ext">�ļ���չ��</param>
		/// <param name="folderPath">�ļ������ļ���</param>
		/// <param name="imgType">����ͼ��ʶ</param>
		/// <returns></returns>
		string CheckFileName(string fileName, string ext, string folderPath, string imgType)
		{
			//�ļ����ظ� ����������ͼ����
			string newFileName = Path.Combine(folderPath, String.Format("{0}{1}{2}", fileName, imgType, ext));
			return newFileName;
		}

		/// <summary>
		/// �ֹ�����ģʽ��������ͼ
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="thumbFilePath"></param>
		void GenarateHandTypeThumbImage(string originalFilePath, string thumbFilePath)
		{
			int imageWidth = Int32.Parse(txt_width2.Text.Replace("px", ""));
			int imageHeight = Int32.Parse(txt_height2.Text.Replace("px", ""));
			int cutTop = Int32.Parse(txt_top2.Text);
			int cutLeft = Int32.Parse(txt_left2.Text);
			int dropWidth = Int32.Parse(txt_DropWidth2.Text);
			int dropHeight = Int32.Parse(txt_DropHeight2.Text);
			ThumbnailImages imgHelp = new ThumbnailImages();
			imgHelp.GetPart(originalFilePath, thumbFilePath, 0, 0, dropWidth, dropHeight, cutLeft, cutTop, imageWidth, imageHeight);
		}

		/// <summary>
		/// ��������ͼ��ť����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GenarateButton_ServerClick(object sender, EventArgs e)
		{
			Article a = ArticleHelper.GetArticle(ArticleID);
			string fn = Server.MapPath(a.Thumbnail);
			string ext = Path.GetExtension(fn);
			string path = a.Thumbnail;
			if (String.IsNullOrEmpty(a.Thumbnail) || String.IsNullOrEmpty(a.Thumbnail.Trim()))
			{
				Messages.ShowError("�����ϴ�ԭʼͼƬ");
			}
			else
			{
				string fileName = Path.GetFileNameWithoutExtension(path);

				string ret = GenerateThumbImage(Server.MapPath(a.Thumbnail), ext, fileName);
				if (ret != "")
				{
					Response.Redirect(We7Helper.AddParamToUrl(Request.RawUrl, "generated", ret));
				}
			}
		}
		/// <summary>
		/// ��ˮӡ
		/// </summary>
		/// <param name="thumbnailFile"></param>
		/// <param name="originalFilePath"></param>
		void AddWatermarkToImage(string thumbnailFile, string originalFilePath)
		{
			ImageConfig.WaterMarkPicfile = Constants.DataUrlPath + "/watermark/" + config.WaterMarkPic;
			ArticleHelper.AddWatermarkToImage(ImageConfig, thumbnailFile, originalFilePath);
		}
		#endregion

		#region ɾ������ͼ

		protected void DeleteButton_Click(object sender, EventArgs e)
		{
			string path = IDTextBox.Text.Trim();
			if (path != "")
			{
				File.Delete(Server.MapPath(path));
				Messages.ShowMessage("���ɹ�ɾ��һ������ͼ��");
				LoadImages();
				InitOriginalImagePath();
			}
		}


		#endregion

		/// <summary>
		/// ͼƬ��Ϣ��
		/// </summary>
		class ImageDetail
		{
			private string name;
			public string Name
			{
				get { return name; }
				set { name = value; }
			}

			private string imagePath;
			public string ImagePath
			{
				get { return imagePath; }
				set { imagePath = value; }
			}

			private string tag;

			public string Tag
			{
				get { return tag; }
				set { tag = value; }
			}

			string fileName;

			public string FileName
			{
				get { return fileName; }
				set { fileName = value; }
			}
		}
	}
}