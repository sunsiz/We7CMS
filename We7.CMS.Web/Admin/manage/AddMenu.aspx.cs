using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using We7.Model.Core;
using We7.Framework.Util;
using We7.Framework;
using We7.CMS.Common.Enum;

namespace We7.CMS.Web.Admin
{
    public partial class AddMenu : BasePage
    {
        string EntityID
        {
            get
            {
                if (Request["type"] != null && Request["type"].ToString() == "1")
                    return "System.User";
                else
                    return "System.Administration";
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["nomenu"] != null)
                    return MasterPageMode.NoMenu;
                else
                    return MasterPageMode.FullMenu;
            }
        }

        string ModelName
        {
            get
            {
                return Request["modelname"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ModelName != null)
                {
                    ContentSelectTable.Visible = false;
                    TitleTextBox.Enabled = false;
                    ModelInfo model = ModelHelper.GetModelInfoByName(ModelName);
                    InitMenuData(model.Label, ModelName);
                    ListItem item = new ListItem(model.Label, ModelName);
                    MenuDropDownList.Items.Add(item);

                    //���û�����˵��������ڵ����򣬲���ʾ�����ء���ť
                    if (Request["nomenu"] != null)
                    {
                        ReturnHyperLink.Visible = false;
                    }
                }
                else
                {
                    BindingData();
                }

                BindChildIndex();
                if (EntityID == "System.User")
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx?type=1";
                else
                    ReturnHyperLink.NavigateUrl = "Menulist.aspx";
                if (EntityID == "System.User")
                {
                    trAddMenu.Visible = false;
                }
                else
                {
                    trAddMenu.Visible = true;
                }
            }
        }

        #region ��ע��
        /*
        string GetIconFileName()
        {
            string theme = SiteSettingHelper.Instance.Config.CMSTheme;
            if (theme == null || theme == "") theme = "classic";
            string path = "~/admin/" + Constants.ThemePath + "/" + theme + "/images";

            Boolean fileOk = false;
            //�ж��Ƿ�ѡ�����ļ�
            if (IconFileUpload.HasFile && HoverIconFileUpload.HasFile)
            {
                //�����ļ�����չ��
                string fileExtension1 = System.IO.Path.GetExtension(IconFileUpload.FileName).ToLower();
                string fileExtension2 = System.IO.Path.GetExtension(HoverIconFileUpload.FileName).ToLower();
                //�����ļ�����
                string[] allowExtensions = { ".gif", ".png" };
                //�ж�ѡ���ļ��Ƿ��������
                for (int i = 0; i < allowExtensions.Length; i++)
                {
                    if (fileExtension1 == allowExtensions[i] && fileExtension2 == allowExtensions[i])
                    {
                        fileOk = true;
                    }
                }
                //�ļ���С������
                if (IconFileUpload.PostedFile.ContentLength > 1024000 || HoverIconFileUpload.PostedFile.ContentLength > 1024000)
                {
                    fileOk = false;
                }
                if (fileOk)
                {
                    string tmpPath = "/_temp";
                    if (!Directory.Exists(Server.MapPath(tmpPath)))
                        Directory.CreateDirectory(Server.MapPath(tmpPath));
                    string NewpathFile = Path.Combine(Server.MapPath(tmpPath), IconFileUpload.FileName);
                    string hoverNewPath = Path.Combine(Server.MapPath(tmpPath), HoverIconFileUpload.FileName);
                    IconFileUpload.PostedFile.SaveAs(NewpathFile);
                    HoverIconFileUpload.PostedFile.SaveAs(hoverNewPath);
                    CreateUploadImageIcon(path, NewpathFile, hoverNewPath, ".gif");
                    return CreateUploadImageIcon(path, NewpathFile, hoverNewPath, ".png");
                }
                else
                {
                    throw new Exception("ͼƬ���Ͳ��Ի��ļ�����1024KB��");
                }
            }
            else if (!IconFileUpload.HasFile && !HoverIconFileUpload.HasFile)
            {
                CreateLetterNewIcon(path, ".gif");
                return CreateLetterNewIcon(path, ".png");
            }
            else
                throw new Exception("����ͼ�궼��ѡ������ѡ�Ļ���Ĭ����������ĸͼ�ꡣ");
        }

        /// <summary>
        /// �ϴ���ͼƬ����ͼ��
        /// </summary>
        /// <param name="pathFile1"></param>
        /// <param name="pathFile2"></param>
        /// <returns></returns>
        string CreateUploadImageIcon(string path,string pathFile1,string pathFile2,string imgType)
        {
            string imgID = MenuDropDownList.SelectedItem.Value;
            imgID = imgID.Replace(".", "_");
            string iconFile = Path.Combine(Server.MapPath(path), "menu_c_" + imgID + imgType);
            string iconHoverFile = Path.Combine(Server.MapPath(path),  "menu_c_" + imgID + "_hover" + imgType);
            ImageUtils.GenerateIcon(pathFile1, iconFile, 30, 30, 15, 15, imgType); 
            ImageUtils.GenerateIcon(pathFile2,iconHoverFile, 30, 30, 15, 15, imgType);
            return Path.GetFileName(iconFile);
        }

        /// <summary>
        /// �˵���������ĸ����ͼ��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string CreateLetterNewIcon(string path, string imgType)
        {
            string imgID = MenuDropDownList.SelectedItem.Value;
            imgID = imgID.Replace(".", "_");
            string firstChar = MainTitleTextBox.Text.Substring(0, 1);
            firstChar = MenuHelper.GetChineseSpell(firstChar);
            firstChar = firstChar.Substring(0, 1).ToUpper();
            string iconFile = Path.Combine(Server.MapPath(path), "menu_c_" + imgID + imgType);
            string iconHoverFile = Path.Combine(Server.MapPath(path), "menu_c_" + imgID + "_hover" + imgType);
            ImageUtils.GenerateImgFromText(iconFile, firstChar, 30, 30, "Times New Roman", 14, System.Drawing.Color.DarkGray, imgType);
            ImageUtils.GenerateImgFromText(iconHoverFile, firstChar, 30, 30, "Times New Roman", 14, System.Drawing.Color.Red, imgType);
            return Path.GetFileName(iconFile);
        }
       
         * void BindingIndex()
        {
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 0, EntityID);

            foreach (We7.CMS.Common.MenuItem menuItem in menus)
            {
                string name =  menuItem.Title.ToString();
                string value = menuItem.Group.ToString() + "," + menuItem.Index.ToString();
                ListItem item = new ListItem(name, value);
                DropDownListType.Items.Add(item);
            }

            if (DropDownListType.Items.Count == 0)
            {
                Messages.ShowMessage("�����˵�");
                return;
            }

            ListItem currentItem = DropDownListType.Items.FindByText("�ļ�");
            if (currentItem != null)
                currentItem.Selected = true;
        }
         * 
        */
        #endregion


        void BindingData()
        {
            ContentModelCollection cmc = ModelHelper.GetContentModel(ModelType.ARTICLE);
            MenuDropDownList.DataSource = cmc;
            MenuDropDownList.DataTextField = "Label";
            MenuDropDownList.DataValueField = "Name";
            MenuDropDownList.DataBind();
        }

        void BindChildIndex()
        {
            List<We7.CMS.Common.MenuItem> menus = MenuHelper.GetMenus(We7Helper.EmptyGUID, 2, EntityID);

            string myname = "�����ݷ���";
            int selectIndex = 0, j = 0;

            foreach (We7.CMS.Common.MenuItem menuItem in menus)
            {
                string name = "��" + menuItem.Title.ToString();
                string value = menuItem.ID + ",0";
                ListItem item = new ListItem(name, value);
                SecondIndexDropDownList.Items.Add(item);
                int i = 0;
                foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
                {
                    string sname = "������" + submenu.Title.ToString();
                    string svalue = submenu.ParentID + "," + submenu.Index.ToString();
                    ListItem sitem = new ListItem(sname, svalue);
                    i = submenu.Index;
                    SecondIndexDropDownList.Items.Add(sitem);
                    j++;
                }
                ListItem appendItem = new ListItem("��������׷�ӵ����", menuItem.ID + "," + (i + 2).ToString());
                SecondIndexDropDownList.Items.Add(appendItem);
                j++;

                if (name == myname)
                {
                    selectIndex = j;
                }

                j++;
            }
            SecondIndexDropDownList.SelectedIndex = selectIndex;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                //string mainIconName = GetIconFileName();
                //string mainTitle = MainDesTextBox.Text.Trim();
                //string mianNameText = MainTitleTextBox.Text.Trim();
                //int maingroup = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[0]);

                //int mainIndex =0; 
                //if (Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]) > 0)
                //{ 
                //    mainIndex = Int32.Parse(DropDownListType.SelectedItem.Value.Split(',')[1]) - 1; 
                //}

                //int maingroup = Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]);
                int firstIndex = 0;
                if (Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) > 0)
                {
                    firstIndex = Int32.Parse(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]) - 1;
                }
                string parentID = SecondIndexDropDownList.SelectedItem.Value.Split(',')[0];

                string firstTitle = DesTextBox.Text.Trim();
                string firstNameText = TitleTextBox.Text.Trim();
                string firstUrl = UrlTextBox.Text.Trim();

                //�����˵�
                string secondTitle = ReleaseDesTextBox.Text.Trim();
                string secondnameText = ReleaseTitleTextBox.Text.Trim();
                string secondurl = ReleaseUrlTextBox.Text.Trim();
                int secondindex = firstIndex;
                if (EntityID == "System.User")
                {
                    MenuHelper.CreateModelMenu(parentID, firstIndex, firstNameText, firstTitle, firstUrl, firstIndex, EntityID);
                }
                else
                {
                    MenuHelper.CreateModelMenu(parentID, firstIndex, firstNameText, firstTitle, firstUrl, firstIndex, secondnameText, secondTitle, secondurl, secondindex, EntityID);
                }
                //MenuHelper.CreateContentMenu(firstNameText, firstTitle, maingroup, firstIndex, "", firstNameText, firstTitle, firstUrl, firstIndex, secondnameText, secondTitle, secondurl, secondindex, EntityID);
                //if (EntityID == "System.User")
                //{
                //    string path = HttpContext.Current.Server.MapPath("~/User/Resource/menuItems.xml");
                //    if (File.Exists(path))
                //    {
                //        XmlNodeList nodes = XmlHelper.GetXmlNodeList(path, "//items");
                //        XmlNode itemsNode = XmlHelper.GetXmlNode(path, "//items");

                //    }
                //}

                //���û�����˵��������ڵ����򣬱���ɹ�����ʾ�ɹ������ø�����ر�Form����
                if (Request["nomenu"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.parent.CloseForm('��ӳɹ�',window.parent.ReloadMenu)", true);
                }
                else
                {
                    string url = We7Helper.AddParamToUrl(ReturnHyperLink.NavigateUrl, "reload", "menu");
                    url = We7Helper.AddParamToUrl(url, "add", firstTitle + "��" + firstTitle + "," + secondTitle + "��");
                    HttpContext.Current.Session.Clear();
                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                Messages.ShowError("�޷����棺" + ex.Message);
            }
        }
        protected void InitButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(MenuDropDownList.SelectedValue))
            {
                Messages.ShowMessage("��ѡ������ģ��");
                return;
            }
            string title = MenuDropDownList.SelectedItem.Text;
            string value = MenuDropDownList.SelectedItem.Value;
            InitMenuData(title, value);
        }

        void InitMenuData(string title, string value)
        {
            if (MenuHelper.ExistMenuItem(title) != "")
            {
                Messages.ShowMessage(title + "����ģ���Ѿ����ڣ������ٴ���");
                return;
            }
            //MainTitleTextBox.Text = title;
            //MainDesTextBox.Text = title;
            //DisplayTextBox.Text = title;

            //���������������Ҫ��� ��ͬ�����ģ�\We7.CMS.Web\Admin\ContentModel\ajax\ContentModel.asmx.cs
            //                                �е� public string DeleteModel(string model)����
            TitleTextBox.Text = title + "����";
            DesTextBox.Text = title + "����";

            //2011-10-10 Ϊ��ɾ������ģ�͵�ʱ��ͬ��ɾ�����˵����ݣ���������ģ�ͼ������˵������Ʋ��ɸ���
            TitleTextBox.Enabled = false;

            if (EntityID == "System.User")
                UrlTextBox.Text = "/User/ModelHandler.aspx?model=" + value + "";
            else
                UrlTextBox.Text = "/admin/AddIns/ModelList.aspx?notiframe=1&model=" + value + "";
            //IndexTextBox.Text = "1";
            ReleaseTitleTextBox.Text = "����" + title;
            ReleaseDesTextBox.Text = "����";
            //ReleaseIndexTextBox.Text = "2";
            ReleaseUrlTextBox.Text = "/admin/addins/ModelEditor.aspx?notiframe=1&model=" + value + "";




        }
    }
}
