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
using System.Collections.Generic;

using We7.CMS.Controls;
using System.IO;
using System.Xml;
using System.Reflection;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceReplyStatisticsControl : BaseUserControl
    {

        //protected override string[] Permissions
        //{
        //    get { return new string[] { "Advice.AdviceReplyStatisticsControl" }; }
        //}

        #region ����
        
        public bool IsAdviceModel
        {
            get
            {
                if (Request["adTypeID"] != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        string adviceTypeID;
        string AdviceTypeID
        {
            get
            {
                return Request["adTypeID"];
            }
            set
            {
                adviceTypeID = value;
            }
        }
        AdviceQuery query = null;
        AdviceQuery CurrentQuery
        {
            get
            {
                if (query == null)
                {
                    query = CreateQuery();
                }
                return query;
            }
            set
            {
                query = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                starttime.Value = DateTime.Now.ToString("yyyy-MM-01");
                endtime.Value = DateTime.Today.ToString("yyyy-MM-dd");

                BindData();
            }
        }
        protected void Pager_Fired(object sender, EventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// ͳ�����ݰ�
        /// </summary>
        void BindData()
        {
            if (AdviceTypeID != null && AdviceTypeID != "")
                AdviceTypeStatistics();
            else
                AdviceModuleStatistics();
        }

        /// <summary>
        /// ������Ϣ������ģ��ͳ�ƣ�AdviceType��
        /// ͳ�Ʊ�׼ΪAdvice��State = ��int��AdviceState.Finished
        /// </summary>
        protected void AdviceModuleStatistics()
        {
            //1.��ȡ���еķ���ģ�� adTypeList
            List<AdviceType> adTypeList = new List<AdviceType>();
            adTypeList = AdviceTypeHelper.GetAdviceTypes();

            if (adTypeList != null)
            {
                //��ҳ�ؼ����Ը�ֵ
                if (AdviceReplyStatisticsPager.Count < 0)
                {
                    AdviceReplyStatisticsPager.PageIndex = 0;
                }
                AdviceReplyStatisticsPager.FreshMyself();
                AdviceReplyStatisticsPager.RecorderCount = adTypeList.Count;
                if (AdviceReplyStatisticsPager.RecorderCount < 1)
                {
                    StatisticsDataGridView.DataSource = null;
                }
                else
                {
                    //����ͳ�ƶ���
                    List<AdviceRate> adRateList = new List<AdviceRate>();
                    foreach (AdviceType adType in adTypeList)
                    {
                        AdviceRate adRate = new AdviceRate();
                        if (adType != null && adType.ID != "")
                        {
                            adRate.AdviceTypeID = CurrentQuery.AdviceTypeID = adType.ID;
                            adRate.AdviceTypeTitle = adType.Title;
                            //2.��������ѭ����ȡÿ��ģ�͵ķ�����Ϣ���� adList
                            List<Advice> adList = AdviceHelper.GetAdviceByQuery(CurrentQuery, 0, 0);
                            adRate.AdviceCount = adList.Count;
                            for (int i = 0; i < adList.Count; i++)
                            {
                                if (adList != null && adList.Count != 0)
                                {
                                    //���ݻ�ȡ���ķ�����Ϣ���ݸ�����ͳ�ƶ���ֵ
                                    if (adList[i].MustHandle == 1)
                                        adRate.HandleNumber += 1;
                                    if (adList[i].State == (int)AdviceState.Finished)
                                    {
                                        adRate.HandleCount += 1;
                                        if (adList[i].EnumState != EnumLibrary.AdviceEnum.AdminHandle.ToString())//����Ա����ĳ�ȥ
                                        {
                                            adRate.NoAdminHandleCount += 1;
                                            if(adList[i].MustHandle == 1)
                                            {
                                                adRate.NotAdminMustHandleCount += 1;
                                            }
                                        }
                                    }
                                    if (adList[i].State != (int)AdviceState.Finished && adList[i].MustHandle == 1)
                                    {
                                        adRate.NoHandleCount += 1;
                                    }
                                }
                            }
                           
                        }
                        adRate.NoHandleNumber = adRate.AdviceCount - adRate.HandleNumber;
                        int count = 0;
                        if (adRate.NoAdminHandleCount > 0 && adRate.HandleNumber > 0)
                            count = (int)(((double)adRate.NotAdminMustHandleCount / (double)adRate.HandleNumber) * 100);
                        if (count > 100)
                            count = 100;
                        adRate.HandleRate = count + "%";
                        adRateList.Add(adRate);
                    }
                    StatisticsDataGridView.DataSource = adRateList;
                }
            }
            StatisticsDataGridView.DataBind();
        }

        /// <summary>
        /// ������Ϣ���������ͳ�ƣ�Advice������
        /// ͳ�Ʊ�׼ΪAdvice��State = ��int��AdviceState.Finished
        /// </summary>
        protected void AdviceTypeStatistics()
        {
             List<AdviceRate> items = CreateAdviceRateList();
             if (items != null)
             {
                 if (AdviceReplyStatisticsPager.Count < 0)
                 {
                     AdviceReplyStatisticsPager.PageIndex = 0;
                 }
                 AdviceReplyStatisticsPager.FreshMyself();
                 AdviceReplyStatisticsPager.RecorderCount = items.Count;
                 if (AdviceReplyStatisticsPager.RecorderCount < 1)
                 {
                     StatisticsDataGridView.DataSource = null;
                 }
                 else
                 {
                     AdviceReplyStatisticsPager.FreshMyself();
                     int tmpCount = 0;
                     int start = AdviceReplyStatisticsPager.Begin;
                     int size = AdviceReplyStatisticsPager.PageSize;
                     int itmCount = items.Count - start;

                     if (itmCount > size)
                     {
                         tmpCount = size;
                     }
                     else
                     {
                         tmpCount = itmCount;
                     }

                     StatisticsDataGridView.DataSource = items.GetRange(start, tmpCount);
                 }
                 StatisticsDataGridView.DataBind();
             }
        }

        /// <summary>
        /// ���췴��ͳ�ư����ݵĶ���
        /// </summary>
        /// <returns></returns>
        List<AdviceRate> CreateAdviceRateList()
        {
            List<AdviceRate> adviceRateList = new List<AdviceRate>();//���ݶ���
            List<Advice> adviceInfoTypeList = AdviceHelper.CreatedAdviceRate(AdviceTypeID);//�˶����Ϊ������Ϣ����ȥ�غ�Ľ����
            
            CurrentQuery.AdviceTypeID = AdviceTypeID;
            for (int i = 0; i < adviceInfoTypeList.Count; i++)
            {
                CurrentQuery.AdviceInfoType = adviceInfoTypeList[i].AdviceInfoType;
                //CurrentQuery.State = 0;
                //CurrentQuery.NotEnumState = -1;
                AdviceRate adviceRate = new AdviceRate();
                adviceRate.AdviceCount = AdviceHelper.GetAdviceCount(CurrentQuery);//�ܼ���
                adviceRate.AdviceInfoType = adviceInfoTypeList[i].AdviceInfoType;
                adviceRate.AdviceTypeID = AdviceTypeID;
                adviceRate.AdviceTypeTitle = adviceInfoTypeList[i].TypeTitle;
                CurrentQuery.State = (int)AdviceState.Finished;
                adviceRate.HandleCount = AdviceHelper.GetAdviceCount(CurrentQuery);//�ܴ�����

                CurrentQuery.State = 0;
                CurrentQuery.MustHandle = 1;
                adviceRate.HandleNumber = AdviceHelper.GetAdviceCount(CurrentQuery);//Ӧ������

                CurrentQuery.NotState = (int)AdviceState.Finished;
                adviceRate.NoHandleCount = AdviceHelper.GetAdviceCount(CurrentQuery);//δ������

                CurrentQuery.NotEnumState = (int)EnumLibrary.AdviceEnum.AdminHandle;
                CurrentQuery.State = (int)AdviceState.Finished;
                CurrentQuery.NotState = -1;
                adviceRate.NotAdminMustHandleCount = AdviceHelper.GetAdviceCount(CurrentQuery);//�����˴���ı���������Ϣ��

                CurrentQuery.MustHandle = -1;
                adviceRate.NoAdminHandleCount = AdviceHelper.GetAdviceCount(CurrentQuery);//�����˴�����

                adviceRate.NoHandleNumber = adviceRate.AdviceCount - adviceRate.HandleNumber;//���账����

                int count = 0;
                if (adviceRate.NoAdminHandleCount > 0 && adviceRate.HandleNumber > 0)
                    count = (int)(((double)adviceRate.NotAdminMustHandleCount / (double)adviceRate.HandleNumber) * 100);
                if (count > 100)
                    count = 100;
                adviceRate.HandleRate = count + "%";

                adviceRateList.Add(adviceRate);
              
                CurrentQuery = CreateQuery();
            }
            return adviceRateList;
        }

        /// <summary>
        /// �����ѯ����
        /// </summary>
        /// <returns></returns>
        AdviceQuery CreateQuery()
        {
            AdviceQuery tempAQ = new AdviceQuery();
            tempAQ.AdviceTypeID = AdviceTypeID;
            tempAQ.EndCreated = Convert.ToDateTime(endtime.Value);
            tempAQ.StartCreated = Convert.ToDateTime(starttime.Value);
            tempAQ.IsShow = 9999;
            return tempAQ;
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
