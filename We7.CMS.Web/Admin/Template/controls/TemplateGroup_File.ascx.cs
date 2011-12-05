using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using We7.CMS.Common;
using System.Text.RegularExpressions;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroup_File : BaseUserControl
    {
        /// <summary>
        /// ģ��������
        /// </summary>
        string FileName
        {
            get
            {
                string fileName = String.Empty;

                if (Request["file"] != null && Request["file"].Trim().ToLower() == "default")
                    fileName = CDHelper.Config.DefaultTemplateGroupFileName;
                else
                    fileName = Request["file"];
                if (!String.IsNullOrEmpty(fileName))
                {
                    fileName = Regex.Replace(fileName, ".xml$", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                }
                return fileName;
            }
        }

        /// <summary>
        /// ��ǰ��������
        /// </summary>
        protected TemplateType CurrentState
        {
            get
            {
                TemplateType s = TemplateType.All;
                if (Request["state"] != null)
                {
                    if (We7Helper.IsNumber(Request["state"].ToString()))
                        s = (TemplateType)int.Parse(Request["state"].ToString());
                }
                return s;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StateLiteral.Text = BuildStateLinks();
                ShowTempalteFile();
            }
        }

        /// <summary>
        /// ��ȡģ���ļ�
        /// </summary>
        void ShowTempalteFile()
        {
            Template[] tps = TemplateHelper.GetTemplates(null, Path.GetFileNameWithoutExtension(FileName), CurrentState);
            TempldatesRepeater.DataSource = tps;
            TempldatesRepeater.DataBind();
        }

        public string GetUrlByTpName(string tpName)
        {
            string url = String.Format("/admin/Template/TemplateFileDetail.aspx?tgfile={0}&tfile={1}", FileName, tpName);
            return url;
        }

        public string GetEditUrl(string tp)
        {
            string url = "";

            Template t = TemplateHelper.GetTemplate(tp + ".xml", Path.GetFileNameWithoutExtension(FileName));
            if (t != null)
            {
                string fileName = We7Helper.Base64Encode(t.FileName);
                string path = We7Helper.Base64Encode(Path.GetFileNameWithoutExtension(FileName));
                if (t.IsSubTemplate)
                {
                    url = String.Format("/admin/DataControlUI/Compose.aspx?file={0}&folder={1}&templateSub=sub", fileName, path);
                }
                else if (t.TemplateType == "9")
                    url = String.Format("/admin/DataControlUI/Compose.aspx?file={0}&folder={1}&templateSub=master", fileName, path);
                else
                    url = String.Format("/admin/DataControlUI/Compose.aspx?file={0}&folder={1}", fileName, path);
            }
            return url;
        }

        protected string GetVisualEditUrl(Object isVisualTemplate, string fileName,object text)
        {
            if ((bool)isVisualTemplate)
            {
                string url = String.Format("<a href='/admin/VisualTemplate/VisualDesign.aspx?file={0}&folder={1}&state=design1&virtualdata=virtualdata' title='�������ģ��༭' target='_blank' >{2}</a>", fileName, Path.GetFileNameWithoutExtension(FileName), text);
                return url;
            }
            else
                return String.Format("<a href='{0}' title='�������ģ��༭' target='_blank' >{1}</a>",GetEditUrl(fileName),text);
        }

        protected string BindTemplateDisplay(object bindText)
        {
            if (bindText == null || bindText.ToString() == "")
                return "none";
            else
                return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tpName"></param>
        /// <returns></returns>
        public string GetDeleteUrlByTpName(string tpName)
        {
            string url = String.Format("/admin/Template/TemplateDelete.aspx?folder={0}&file={1}", Path.GetFileNameWithoutExtension(FileName), tpName);
            return url;
        }

        /// <summary>
        /// ģ���ļ�ɾ���¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteFileButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//�Ƿ�����ʾվ��
            try
            {
                string tf = FileTextBox.Text;
                TemplateHelper.RemoveTemplateBind(null, FileName, tf);
                string fn = Server.MapPath(Path.Combine(String.Format("\\{0}\\{1}", Constants.TemplateBasePath, Path.GetFileNameWithoutExtension(FileName)), tf));
                string PreviewFn = Server.MapPath(Path.Combine(String.Format("\\{0}\\{1}", Constants.TemplateBasePath, Path.GetFileNameWithoutExtension("~"+FileName)), tf));
                if (File.Exists(PreviewFn))
                {
                    File.Delete(PreviewFn);
                }
                if (File.Exists(fn))
                {
                    File.Delete(fn);
                }
                if (File.Exists(fn + ".xml"))
                {
                    File.Delete(fn + ".xml");
                }

                //��¼��־
                string content = string.Format("ɾ����ģ���ļ�:��{0}��", tf);
                AddLog("ģ���ļ�����", content);
                Messages.ShowMessage("���ѳɹ�ɾ��ģ���ļ���" + tf + "��");
            }
            catch (Exception ex)
            {
                //log
            }
            ShowTempalteFile();
        }

          /// <summary>
        /// ģ��ˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowTempalteFile();
        }

        /// <summary>
        /// ����������/״̬���˵ĳ��������ַ���
        /// </summary>
        /// <returns></returns>
        string BuildStateLinks()
        {
            string url = We7Helper.AddParamToUrl("TemplateGroupEdit.aspx", "file", FileName);
            string links = @"<li> <a href='{10}'   {0} >ȫ��<span class=""count"">({1})</span></a> |</li>
            <li><a href='{10}&state=1'  {2} >��ͨģ��<span class=""count"">({3})</span></a> |</li>
            <li><a href='{10}&state=0'  {4} >��ģ��<span class=""count"">({5})</span></a> |</li>
            <li><a href='{10}&state=9'  {6}  >ĸ��<span class=""count"">({7})</span></a> | </li>
            <li><a href='{10}&state=-1'  {8}  >Ĭ��ģ��<span class=""count"">({9})</span></a></li>";

            string css100, css0, css1, css2, css3;
            css100 = css0 = css1 = css2 = css3 = "";
            if (CurrentState == TemplateType.All) css100 = "class=\"current\"";
            if (CurrentState == TemplateType.Common) css0 = "class=\"current\"";
            if (CurrentState == TemplateType.Sub) css1 = "class=\"current\"";
            if (CurrentState == TemplateType.MasterPage) css2 = "class=\"current\"";
            if (CurrentState == TemplateType.HaveBinded) css3 = "class=\"current\"";
            links = string.Format(links, css100, TemplateHelper.GetTemplateCount(TemplateType.All, FileName),
                css0, TemplateHelper.GetTemplateCount(TemplateType.Common, FileName), css1, TemplateHelper.GetTemplateCount(TemplateType.Sub, FileName),
                css2, TemplateHelper.GetTemplateCount(TemplateType.MasterPage, FileName), css3, TemplateHelper.GetTemplateCount(TemplateType.HaveBinded, FileName), url);

            return links;
        }


    }
}