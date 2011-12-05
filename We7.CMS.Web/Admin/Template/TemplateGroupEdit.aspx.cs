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

using Thinkment.Data;
using We7.CMS.Common;
using We7.Model.Core;
using System.Text;
using We7.CMS.Module.VisualTemplate.Helpers;
using We7.CMS.WebControls.Core;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroupEdit : BasePage
    {
        /// <summary>
        /// ��ǩҳ
        /// </summary>
        public string TabID
        {
            get { return Request["tab"]; }
        }

        /// <summary>
        /// ģ��Ƥ������
        /// </summary>
        public string SkinName
        {
            get { return Request["skinName"]; }
        }

        string FileName
        {
            get
            {
                if (Request["file"] != null && Request["file"].Trim().ToLower() == "default")
                    return CDHelper.Config.DefaultTemplateGroupFileName;
                else
                {
                    string file = Request["file"];
                    if (!file.ToLower().EndsWith(".xml")) file += ".xml";
                    return file;
                }
            }
        }

        public string ToolbarVisible;
        protected string path;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (FileName != null && FileName.Trim() != "" && File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, FileName)))
            {
                TemplateConfigLiteral.Text = LoadTemplateDictionary("/admin/DataControlUI/Compose.aspx");
                VisualTemplateConfigLiteral.Text = LoadTemplateDictionary("/admin/VisualTemplate/TemplateCreate.aspx");
                Control ctl = this.LoadControl("../Template/controls/TemplateGroup_File.ascx");
                ContentHolder.Controls.Add(ctl);

                EditHyperLink.NavigateUrl = string.Format("TemplateGroupInfo.aspx?file={0}", FileName);

                path = string.Format("{0}/{1}", Constants.TemplateBasePath, Path.GetFileNameWithoutExtension(FileName));

                NewSubHyperLink.NavigateUrl = string.Format("/admin/DataControlUI/Compose.aspx?folder={0}", We7Helper.Base64Encode(Path.GetFileNameWithoutExtension(FileName)))+"&templatesub=sub";
                NewSubHyperLink.Target = "_blank";
                NewMasterPageHyperLink.NavigateUrl = string.Format("/admin/DataControlUI/Compose.aspx?folder={0}", We7Helper.Base64Encode(Path.GetFileNameWithoutExtension(FileName))) +"&templatesub=master";
                NewMasterPageHyperLink.Target = "_blank"; ;
                ToolbarVisible = "";

                LoadSkininfo();
                RefreshHyperLink.NavigateUrl = this.Page.Request.RawUrl;
            }
            else
            {
                Response.Redirect("/admin/TemplateGroups.aspx");
            }
        }

        void LoadSkininfo()
        {
            string file = FileName;
            if (file != null && file != "" && File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, file)))
            {
                SkinInfo data = TemplateHelper.GetSkinInfo(file);
                NameLabel.Text = "�༭ģ����" + data.Name;
                SummaryLabel.Text = data.Description;
                GroupImage.ImageUrl = GetImageUrl(FileName);
                CreatedLabel.Text = data.Created.ToLongDateString();
            }
        }

        string LoadTemplateDictionary(string editorUrl)
        {
            string itemString = "<a href=\"{4}?handler={0}&mode={1}&model={2}\" title=\"�½� {3} ģ�壿 \" target=\"_blank\"  >{3}</a> ";
            string itemTitle = "<H3 >{0}</H3>";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<h1>���봴����һ����Ӧҳ���ģ�壿</h1>");
            SkinInfo data = TemplateHelper.GetSkinInfo(FileName);

            List<TemplateBindConfig> list = TemplateHelper.GetTemplateBindConfigList();
            string title = "";
            List<TemplateBindConfig> modelList = new List<TemplateBindConfig>();
            foreach (TemplateBindConfig tb in list)
            {
                if (tb.Handler == "model")
                    modelList.Add(tb);
                else
                {
                    SkinInfo.SkinItem item = new SkinInfo.SkinItem();
                    item.C_Model = "";
                    item.Location = tb.Mode;
                    item.Tag = "";
                    item.Type = tb.Handler;
                    if (!TemplateHelper.HaveSameItem(item, data))
                    {
                        if (title != tb.HandlerName)
                        {
                            sb.AppendLine(string.Format(itemTitle, tb.HandlerName));
                            title = tb.HandlerName;
                        }
                        sb.AppendLine(string.Format(itemString, tb.Handler, tb.Mode, "", tb.Description,editorUrl));
                        sb.Append(" �� ");
                    }
                }
            }

            ContentModelCollection cmc = ModelHelper.GetContentModel(ModelType.ARTICLE);
            foreach (We7.Model.Core.ContentModel cm in cmc)
            {
                sb.AppendLine(string.Format(itemTitle, cm.Label + "ģ��"));
                foreach (TemplateBindConfig tb in modelList)
                {
                    SkinInfo.SkinItem item = new SkinInfo.SkinItem();
                    item.C_Model = cm.Name;
                    item.Location = tb.Mode;
                    item.Tag = "";
                    item.Type = tb.Handler;
                    if (!TemplateHelper.HaveSameItem(item, data))
                    {
                        sb.AppendLine(string.Format(itemString, tb.Handler, tb.Mode, cm.Name, cm.Label + tb.Description,editorUrl));
                        sb.Append(" �� ");
                    }
                }
            }

            sb.AppendLine("<br>");
            sb.AppendLine("<h2>" + string.Format(itemString, "", "", "", "���Ի�ģ�壨����Ĭ��ָ����", editorUrl) + "</h2>");
            sb.AppendLine("<br>");
            return sb.ToString();
        }

        protected string GetImageUrl(string FileName)
        {
            string PreviewFileName = FileName + ".jpg";
            string path = "/" + Path.Combine(Constants.TemplateGroupBasePath, PreviewFileName);
            if (!File.Exists(Server.MapPath(path)))
                path = "../images/template_default.jpg";
            string phyPath = path.Replace("\\", "/");

            return phyPath;
        }


        protected void CreateMapLink_Click(object sender, EventArgs e)
        {
             string file = FileName;
             if (file != null && file != "" && File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, file)))
             {
                 SkinInfo data = TemplateHelper.GetSkinInfo(file);
                 TemplateHelper.CreateMapFileFromSkinInfo(data);
                 TemplateHelper.RefreshTemplateDefaultBindText(data);
                 Messages.ShowMessage("ģ���ͼ�ѳɹ����ɣ�");
             }
        }

        protected void CreateControlIndex_Click(object sender, EventArgs e)
        {
            try
            {
                DataControlHelper dchelper = HelperFactory.GetHelper<DataControlHelper>();
                dchelper.CreateDataControlIndex();
                //BaseControlHelper Helper = new BaseControlHelper();
                //Helper.CreateIntegrationIndexConfig();
                Messages.ShowMessage("���������ɹ�");
            }
            catch (Exception ex)
            {
                Messages.ShowError("�ؽ�����ʧ�ܣ�"+ex.Message);
            }
        }

        /// <summary>
        /// �ؽ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateWidgetIndex_Click(object sender, EventArgs e)
        {
            try
            {
                BaseControlHelper Helper = new BaseControlHelper();
                Helper.CreateWidegetsIndex();
                Messages.ShowMessage("���²��������ɹ�");
            }
            catch (Exception ex)
            {
                Messages.ShowError("�ؽ���������ʧ�ܣ�" + ex.Message);
            }
        }
        /// <summary>
        /// �ؽ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateThemeIndex_Click(object sender, EventArgs e)
        {
            try
            {
                BaseControlHelper Helper = new BaseControlHelper();
                Helper.CreateThemeIndex();
                Messages.ShowMessage("�������������ɹ�");
            }
            catch (Exception ex)
            {
                Messages.ShowError("�ؽ���������ʧ�ܣ�" + ex.Message);
            }
        }
    }
}
