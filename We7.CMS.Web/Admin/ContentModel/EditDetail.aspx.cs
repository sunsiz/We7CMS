﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using We7.Framework.Util;
using We7.Model.Core.Data;
using We7.Model.Core;
using We7.CMS.Accounts;
using We7.Framework;
using We7.Model.Core.Config;
using System.Data;
using We7.CMS.Common.Enum;


namespace We7.CMS.Web.Admin.ContentModel
{
    /// <summary>
    /// 编辑内容模型布局和部件
    /// </summary>
    public partial class EditDetail : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.NoMenu;
            }
        }
        /// <summary>
        /// 向客户端输出脚本
        /// </summary>
        protected string strScript;
        /// <summary>
        /// True:文章类型，False :反馈
        /// </summary>
        public bool isArticle;

        /// <summary>
        /// 操作类型：widget:部件,layout:布局
        /// </summary>
        public string Action;
        /// <summary>
        /// page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
               
            }
        }

        private ModelInfo modelInfo;
        /// <summary>
        /// Model  Info Entity
        /// </summary>
        public ModelInfo ModelInfo
        {
            get
            {
                if (modelInfo == null)
                {
                    string modelName = Request["modelname"];
                    if (String.IsNullOrEmpty(modelName))
                        throw new Exception("当前模型不存在");
                    modelInfo = ModelHelper.GetModelInfo(modelName);
                    if (modelInfo.Type == We7.Model.Core.ModelType.ARTICLE)
                    {
                        isArticle = true;
                    }
                    //反馈模型只支持 生成部件功能
                    else if (modelInfo.Type == We7.Model.Core.ModelType.ADVICE)
                    {
                        isArticle = false;
                    }
                    //用户模型不支持
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "tip", "alert('内容模型不支持高级应用!');", true);
                    }
                }
                return modelInfo;
            }
        }

        /// <summary>
        /// 检查各项是否已经创建
        /// </summary>
        void Exist(string Action)
        {
            IDataBaseHelper helper = DataBaseHelperFactory.Create();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"modelName\":\"" + ModelInfo.ModelName + "\",\"Data\":[");

            if (Action == "widget")
            {
                #region 检查部件
                //部件
                int widgetCount = 0;
                string viewPath = ModelHelper.GetWidgetDirectory(modelInfo, "View");
                string listPath = ModelHelper.GetWidgetDirectory(modelInfo, "List");
                string pageListPath = ModelHelper.GetWidgetDirectory(modelInfo, "PagedList");
                if (Directory.Exists(viewPath))
                {
                    widgetCount++;
                }
                if (Directory.Exists(listPath))
                {
                    widgetCount++;
                }
                if (Directory.Exists(pageListPath))
                {
                    widgetCount++;
                }
                //存在至少一个部件
                if (widgetCount > 0)
                {
                    sb.Append("{\"name\":\"createWidget\",\"exist\":true},");
                }
                else
                {
                    sb.Append("{\"name\":\"createWidget\",\"exist\":false},");
                }
                #endregion
            }

            if (isArticle)
            {

                #region 检查表结构
                //int rowCount = 0;
                //try
                //{
                //    rowCount = helper.Count(ModelInfo.Name, "");
                //    sb.Append("{\"name\":\"createTable\",\"exist\":true},");
                //}
                //catch
                //{
                //    sb.Append("{\"name\":\"createTable\",\"exist\":false},");
                //}

                #endregion

                #region 检查左侧菜单
                //MenuHelper MenuHelper = HelperFactory.Instance.GetHelper<MenuHelper>();
                //We7.CMS.Common.MenuItem item = MenuHelper.GetMenuItemByTitle(ModelInfo.Label + "管理");
                //if (item != null && !string.IsNullOrEmpty(item.ID))
                //{
                //    sb.Append("{\"name\":\"addLeftMenu\",\"exist\":true},");
                //}
                //else
                //{
                //    sb.Append("{\"name\":\"addLeftMenu\",\"exist\":false},");
                //}
                #endregion

                if (Action == "layout")
                {
                    #region 检查布局

                    string layoutPath = ModelHelper.GetModelLayoutDirectory(ModelInfo.ModelName) + "GenerateLayout.ascx";
                    if (File.Exists(layoutPath))
                    {
                        layoutPath = String.Format("{0}/{1}/{2}/{3}", ModelConfig.ModelsDirectory, ModelInfo.GroupName, ModelInfo.Name, "GenerateLayout.ascx");
                        EditInfo entity = ModelInfo.Layout.Panels["edit"].EditInfo;
                        sb.Append("{\"name\":\"createLayout\",\"exist\":true,\"path\":\"" + layoutPath + "\"},");

                        //ModelInfo.Layout.Panels["edit"].EditInfo.Layout;
                        if (!string.IsNullOrEmpty(entity.Layout))
                            chkAE.Checked = true;

                        if (!string.IsNullOrEmpty(entity.ViewerLayout))
                            chkView.Checked = true;

                        if (!string.IsNullOrEmpty(entity.UcLayout))
                            chkUC.Checked = true;
                    }
                    else
                    {
                        sb.Append("{\"name\":\"createLayout\",\"exist\":false},");
                    }


                    #endregion
                }

            }
            sb.Append("]}");
            strScript = sb.ToString();
            strScript = strScript.Remove(strScript.LastIndexOf(","), 1);

            strScript = new JavaScriptSerializer().Serialize(strScript);

        }

        /// <summary>
        /// BindData()
        /// </summary>
        void BindData()
        {
           Action = We7Request.GetString("t").ToLower();
           if (Action == "widget")

            {
                List<string> DisplayFields = new List<string>();
                We7DataColumnCollection dcs = new We7DataColumnCollection();
                foreach (We7DataColumn col in ModelInfo.DataSet.Tables[0].Columns)
                {
                    if (col.Direction == ParameterDirection.ReturnValue || (col.IsSystem && !DisplayFields.Contains(col.Name)))
                        continue;
                    dcs.Add(col);
                }

                chklstWidgetList.DataSource = dcs;
                chklstWidgetList.DataTextField = "Label";
                chklstWidgetList.DataValueField = "Name";

                chklstWidgetView.DataSource = dcs;
                chklstWidgetView.DataTextField = "Label";
                chklstWidgetView.DataValueField = "Name";

                chklstWidgetList.DataBind();
                chklstWidgetView.DataBind();

            }
           
                if (ModelInfo.Layout != null && ModelInfo.Layout.UCContrl != null)
                {
                    foreach (ListItem item in chklstWidgetView.Items)
                    {
                        item.Attributes["mvalue"] = item.Value;
                        if (ModelInfo.Layout.UCContrl.WidgetDetailFieldArray != null)
                        {
                            item.Selected = Array.Exists(ModelInfo.Layout.UCContrl.WidgetDetailFieldArray, s => s == item.Value);
                        }
                    }

                    foreach (ListItem item in chklstWidgetList.Items)
                    {
                        item.Attributes["mvalue"] = item.Value;
                        if (ModelInfo.Layout.UCContrl.WidgetListFieldArray != null)
                        {
                            item.Selected = Array.Exists(ModelInfo.Layout.UCContrl.WidgetListFieldArray, s => s == item.Value);
                        }
                    }
                }
           
           Exist(Action);
        }

    }
}