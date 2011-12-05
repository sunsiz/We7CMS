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
using We7.CMS.Common;

namespace We7.CMS.Web.Admin
{
    public partial class AddMenuUserNew : BasePage
    {

        string EntityID
        {
            get
            {
                return "System.User";

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
                    ModelInfo model = ModelHelper.GetModelInfoByName(ModelName);
                    InitMenuData(model.Label, ModelName);
                    ListItem item = new ListItem(model.Label, ModelName);
                    MenuDropDownList.Items.Add(item);
                }
                else
                {
                    BindingData();
                }
                BindChildIndex();
                ReturnHyperLink.NavigateUrl = "UserMenulist.aspx?type=1";
            }
        }
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

            foreach (We7.CMS.Common.MenuItem menuItem in menus)
            {
                string name = "��" + menuItem.Title.ToString();
                string value = menuItem.ID + ",0";
                if (menuItem.Items.Count > 0) value = menuItem.Items[0].ID + ",0";
                ListItem item = new ListItem(name, value);
                SecondIndexDropDownList.Items.Add(item);
                int i = 0;
                foreach (We7.CMS.Common.MenuItem submenu in menuItem.Items)
                {
                    string sname = "������" + submenu.Title.ToString();
                    string svalue = submenu.ID + "," + submenu.Index.ToString();
                    ListItem sitem = new ListItem(sname, svalue);
                    i = submenu.Index;
                    SecondIndexDropDownList.Items.Add(sitem);
                    int j = 0;
                    foreach (We7.CMS.Common.MenuItem thirdmenu in submenu.Items)
                    {
                        string tname = "����������" + thirdmenu.Title.ToString();
                        string tvalue = submenu.ParentID + "," + thirdmenu.ParentID + "," + thirdmenu.Index.ToString();
                        ListItem thitem = new ListItem(tname, tvalue);
                        j = thirdmenu.Index;
                        SecondIndexDropDownList.Items.Add(thitem);
                    }
                    ListItem appendItem3 = new ListItem("������������׷�ӵ�����-�����˵���", submenu.ID + "," + (j + 2).ToString());
                    SecondIndexDropDownList.Items.Add(appendItem3);
                }
                //ListItem appendItem = new ListItem("��������׷�ӵ����", menuItem.ID + "," + (i + 2).ToString());                
                //SecondIndexDropDownList.Items.Add(appendItem);
            }            

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (!SecondIndexDropDownList.SelectedItem.Text.Contains("��׷�ӵ�����-�����˵���"))
                {
                    Messages.ShowError("��ѡ��׷�ӵ�����-�����˵���ѡ�");
                    return;
                }
                string parentID = SecondIndexDropDownList.SelectedItem.Value.Split(',')[0];
                int index = Convert.ToInt32(SecondIndexDropDownList.SelectedItem.Value.Split(',')[1]);
              
                string firstTitle = DesTextBox.Text.Trim();
                string firstNameText = TitleTextBox.Text.Trim();
                string firstUrl = UrlTextBox.Text.Trim();

                /*
                 * parentID��
                 * ��������ʱ ӦΪGUID��������ýڵ��oldParent����һ��
                 * ����XMLʱ Ϊint����
                 * ������ 2011-7-13
                 */
                string oldId = MenuHelper.CreateModelMenu(parentID, index, firstNameText, firstTitle, firstUrl, index, EntityID);

               
                Messages.ShowMessage("����ɹ���");

                //string url = We7Helper.AddParamToUrl(ReturnHyperLink.NavigateUrl, "reload", "menu");
                //url = We7Helper.AddParamToUrl(url, "add", firstTitle + "��"+ secondTitle + "��");
                //HttpContext.Current.Session.Clear();
                //Response.Redirect(url);
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
            //if (MenuHelper.ExistMenuItemByType(title, "System.User") != "" || MenuHelper.ExistMenuItemByType(title + "����", "System.User") != "")
            //{
            //    Messages.ShowMessage(title + "����ģ���Ѿ����ڣ������ٴ���");
            //    return;
            //}
            //MainTitleTextBox.Text = title;
            //MainDesTextBox.Text = title;
            //DisplayTextBox.Text = title;
            TitleTextBox.Text = title + "����";
            DesTextBox.Text = title + "����";
            UrlTextBox.Text = "/User/ModelHandler.aspx?model=" + value + "";
            //IndexTextBox.Text = "1";



        }
    }
}
