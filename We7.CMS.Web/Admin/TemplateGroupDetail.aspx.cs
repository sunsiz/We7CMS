using System;
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

using System.IO;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroupDetail : BasePage
    {

        string FileName
        {
            get 
            {
                if (Request["file"] != null && Request["file"].Trim().ToLower() == "default")
                    return CDHelper.Config.DefaultTemplateGroupFileName;
                else
                    return Request["file"]; 
            }
        }

        public static bool EnableSiteSkins
        {
            get
            {
                string _default = SiteSettingHelper.Instance.Config.EnableSiteSkins;
                if (_default != null && _default.ToLower() == "true")
                    return true;
                return false;
            }
        }

        protected TemplateGroup Data
        {
            get { return ViewState["$VS_TEMPLATEGROUP_DATA"] as TemplateGroup; }
            set { ViewState["$VS_TEMPLATEGROUP_DATA"] = value; }
        }

        protected SkinInfo SkinData
        {
            get { return ViewState["$VS_SKIN_DATA"] as SkinInfo; }
            set { ViewState["$VS_SKIN_DATA"] = value; }
        }
        protected string path;
        protected override void Initialize()
        {
            if (!string.IsNullOrEmpty(FileName) && File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, FileName)))
            {
                string file = FileName;
                SkinData = TemplateHelper.GetSkinInfo(file);//��Ϊ��ģ������ת����ģ��༭
                ECompareRelut eCompareRelut = VersioncComparison.Compare(SkinData.Ver, "v2.2", true);
                if (!(eCompareRelut == ECompareRelut.Less))
                    Response.Redirect(String.Format("/admin/Template/TemplateGroupEdit.aspx?file={0}", file),false);
                Data = TemplateHelper.GetTemplateGroup(file);
                foreach (TemplateGroup.Item it in Data.Items)
                {
                    try
                    {
                        Template t = TemplateHelper.GetTemplate(it.Template + ".xml");
                        if (t != null)
                        {
                            it.TemplateText = t.Name;
                        }
                    }
                    catch
                    {
                        it.TemplateText = "";
                    }
                }
                NameLabel.Text = String.Format("�༭ģ���� {0}", Data.Name);
                NameTextBox.Enabled = false;
                NewHyperLink.NavigateUrl = String.Format("TemplateDetail.aspx?tgfile={0}", FileName);
                path = string.Format("{0}/{1}", Constants.TemplateBasePath, Path.GetFileNameWithoutExtension(FileName));
                ShowTempalteGroup();
            }
            else
            {
                Response.Redirect("/admin/TemplateGroups.aspx",false);
            }
        }

        protected void DeleteItemButton_Click(object sender, EventArgs e)
        {
            DeleteTemplateGroupItem();
            Initialize();
        }

        protected void DeleteFileButton_Click(object sender, EventArgs e)
        {
            DeleteTemplateGroupFile();
            Initialize();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            Data.Name = NameTextBox.Text;
            Data.Description = DescriptionTextBox.Text;

            string folderName = NameTextBox.Text.ToLower().Trim();
            string FileName = "";
            if (CreateFolder(folderName))
            {
                FileName = TemplateHelper.SaveTemplateGroupAndPreviewFile(Data, folderName);

                string content = "";
                string title = "";
                content = string.Format("�༭ģ���顰{0}��", Data.Name);
                title = "�༭ģ����";
                if (NameLabel.Text == "�½�ģ����")
                {
                    content = string.Format("�½�ģ���顰{0}��", Data.Name);
                    title = "�½�ģ����";
                }
                AddLog(title, content);

                NameLabel.Text = String.Format("�༭ģ���� {0}", Data.Name);
                UploadPreviewFile(FileName);
                Response.Redirect(String.Format("{0}?file={1}.xml", "TemplateGroupDetail.aspx", folderName),false);
            }
            else
            {
                if (NameLabel.Text == "�½�ģ����")
                {
                    Messages.ShowError("���д�ģ����,��������д���ƣ�");
                }
                else
                {
                    FileName = TemplateHelper.SaveTemplateGroupAndPreviewFile(Data, folderName);
                    UploadPreviewFile(FileName);
                    Messages.ShowMessage("�ɹ��޸�ģ������Ϣ");
                }
            }
        }

        void ShowTempalteGroup()
        {
            NameTextBox.Text = Data.Name;
            DescriptionTextBox.Text = Data.Description;
            CreatedLabel.Text = Data.Created.ToString();
            FileTextBox.Text = Data.FileName;

            DetailGridView.DataSource = Data.Items;
            DetailGridView.DataBind();
            DetailLabel.Text = String.Format("����{0}��ģ�塣", Data.Items.Count);

            Template[] tps = TemplateHelper.GetTemplates(null, Path.GetFileNameWithoutExtension(FileName));
            TempldatesGridView.DataSource = tps;
            TempldatesGridView.DataBind();
            TempldatesLabel.Text = String.Format("����{0}��ģ���ļ���",tps.Length);
        }

        void UploadPreviewFile(string PreviewFileName)
        {
            if (PreviewFileUploador.PostedFile.FileName != "")
            {
                if (String.Compare(Path.GetExtension(PreviewFileUploador.FileName), ".jpg", true) == 0)
                {
                    string path = PreviewFileName + Path.GetExtension(PreviewFileUploador.FileName);
                    path = "~/" + Path.Combine(Constants.TemplateGroupBasePath, path);
                    string phyPath = Server.MapPath(path);
                    PreviewFileUploador.SaveAs(phyPath);
                }
                else
                {
                    Messages.ShowMessage("ͼƬ������jpg�ļ���");
                }
            }
            else
            {

            }
        }

        bool CreateFolder(string folderName)
        {
            string folderPath = Server.MapPath("../" + string.Format("{0}\\{1}", Constants.SiteSkinsBasePath, folderName));
            bool isCreate = false;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                isCreate = true;
            }
            return isCreate;
        }

        public string GetUrl(string fa, string isd, string tpFile)
        {
            string url = String.Format("TemplateDetail.aspx?tgfile={0}&fa={1}&isd={2}", FileName, fa, isd);
            Template t = TemplateHelper.GetTemplate(tpFile + ".xml", Path.GetFileNameWithoutExtension(FileName));
            if (t == null)
                url = "";
            return url;
        }

        public string GetUrlByTpName(string tpName)
        {
            string url = String.Format("TemplateDetail.aspx?tgfile={0}&file={1}", FileName, tpName);
            return url;
        }

        public string GeDeletetUrlByTpName(string tpName)
        {
            string url = String.Format("TemplateDelete.aspx?folder={0}&file={1}", Path.GetFileNameWithoutExtension(FileName), tpName);
            return url;
        }

        public string GetEditUrl(string tp)
        {
            string url = "";

            Template t = TemplateHelper.GetTemplate(tp + ".xml", Path.GetFileNameWithoutExtension(FileName));
            if (t != null)
            {
                if (t.IsSubTemplate)
                {
                    url = String.Format("Compose.aspx?file={0}&folder={1}&templateSub=sub", t.FileName, Path.GetFileNameWithoutExtension(FileName));
                }
                else
                    url = String.Format("Compose.aspx?file={0}&folder={1}", t.FileName, Path.GetFileNameWithoutExtension(FileName));
            }
            return url;
        }

        public string GetTemplateNamel(string tpFile)
        {
            string tpName = "���ļ�������,����ɾ������ü�¼";
            Template t = TemplateHelper.GetTemplate(tpFile + ".xml", Path.GetFileNameWithoutExtension(FileName));
            if(t !=null)
                tpName = t.Name;
            return tpName;
        }

        protected string GetIsDetailText(bool isDetail)
        {
            if (isDetail) return "��ϸҳģ��";
            return "";
        }

        void DeleteTemplateGroupItem()
        {
            TemplateGroup.Item del = null;

            TemplateGroup tg = TemplateHelper.GetTemplateGroup(FileName);

            foreach (TemplateGroup.Item it in tg.Items)
            {
                if (it.Alias == DeleteItemAliasTextBox.Text && it.IsDetailTemplate.ToString() == DeleteItemIsDetailTextBox.Text)
                {
                    del = it;
                    break;
                }
            }
            if (del != null)
            {
                tg.Items.Remove(del);
                string fn = TemplateHelper.SaveTemplateGroupAndPreviewFile(tg, Path.GetFileNameWithoutExtension(FileName));
            }
            DeleteItemAliasTextBox.Text = "";
            DeleteItemIsDetailTextBox.Text = "";
        }

        void DeleteTemplateGroupFile()
        {
            try
            {
                string tf = FileTextBox.Text;
                string fn = Server.MapPath(Path.Combine(String.Format("\\{0}\\{1}", Constants.TemplateBasePath, Path.GetFileNameWithoutExtension(FileName)), tf));
                if (File.Exists(fn))
                {
                    File.Delete(fn);
                }
                if (File.Exists(fn + ".xml"))
                {
                    File.Delete(fn + ".xml");
                }

                //��¼��־
                string content = string.Format("ɾ����ģ��:��{0}��", tf);
                AddLog("ģ�����", content);

                Response.Clear();
                Response.Redirect(
                    String.Format("TemplateGroupDetail.aspx?file={0}.xml", Path.GetFileNameWithoutExtension(FileName)),false);
            }
            catch (CDException ce)
            {
                HandleException(ce);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

    }
}
