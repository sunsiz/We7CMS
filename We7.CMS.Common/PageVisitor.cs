using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ���ܣ�ҳ������û�ͳ��ʵ�� theheim 2007-11-08
    /// </summary>
    [Serializable]
    public class PageVisitor
    {
        private string id;
        private int typeCode;
        private string userID;
        private string userName;
        private DateTime visitDate;
        private DateTime logoutDate;
        private string visitorIP;
        private string url;
        private string http_referer;
        private string searchEngine;
        private string keyword;
        private int clicks;
        private DateTime onlineTime;
        private string platform;
        private string browser;
        private string screen;
        private string city;
        private int pageView;
        private DateTime updated=DateTime.Now;
        private DateTime created=DateTime.Now;
        string fromSite;
        string province;

        public PageVisitor()
        { }

        /// <summary>
        /// ���
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ���0Ϊ��¼ͳ�ƣ�1Ϊ����ͳ��
        /// </summary>
        public int TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; }
        }

        /// <summary>
        /// �û�ID
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// �û���
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }


        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime VisitDate
        {
            get { return visitDate; }
            set { visitDate = value; }
        }

        /// <summary>
        /// �˳�ʱ��
        /// </summary>
        public DateTime LogoutDate
        {
            get { return logoutDate; }
            set { logoutDate = value; }
        }

        /// <summary>
        /// ������IP
        /// </summary>
        public string VisitorIP
        {
            get { return visitorIP; }
            set { visitorIP = value; }
        }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// ��ԴӦ����ַ
        /// </summary>
        public string HttpReferer
        {
            get { return http_referer; }
            set { http_referer = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public string SearchEngine
        {
            get { return searchEngine; }
            set { searchEngine = value; }
        }

        /// <summary>
        /// ��������ؼ���
        /// </summary>
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        /// <summary>
        /// �����
        /// </summary>
        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime OnlineTime
        {
            get { return onlineTime; }
            set { onlineTime = value; }
        }

        /// <summary>
        /// ����ϵͳ
        /// </summary>
        public string Platform
        {
            get { return platform; }
            set { platform = value; }
        }

        /// <summary>
        /// �����
        /// </summary>
        public string Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        /// <summary>
        /// ��Ļ�ֱ���
        /// </summary>
        public string Screen
        {
            get { return screen; }
            set { screen = value; }
        }
        /// <summary>
        /// ʡ��
        /// </summary>
        /// <summary>
        /// ʡ��
        /// </summary>
        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        public int PageView
        {
            get { return pageView; }
            set { pageView = value; }
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
        /// ����ʱ��
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        /// <summary>
        /// ��Դ������
        /// </summary>
        public string FromSite
        {
            get { return fromSite; }
            set { fromSite = value; }
        }

    }

    public class VisiteCount:ICloneable
    {
        private int totalPageView;
        private int totalVisitors;
        private int yearVisitors;
        private int monthVisitors;
        private int dayVisitors;
        private int yestodayVisitors;
        private int averageDayVisitors;
        private int yearPageview;
        private int monthPageview;
        private int dayPageview;
        private int yestodayPageview;
        private int averageDayPageview;
        private int onlineVisitors;
        private DateTime startDate = DateTime.Now;

        public VisiteCount() { }

        /// <summary>
        /// 
        /// </summary>
        public int TotalPageView
        {
            get { return totalPageView; }
            set { totalPageView = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TotalVisitors
        {
            get { return totalVisitors; }
            set { totalVisitors = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int YearVisitors
        {
            get { return yearVisitors; }
            set { yearVisitors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MonthVisitors
        {
            get { return monthVisitors; }
            set { monthVisitors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int DayVisitors
        {
            get { return dayVisitors; }
            set { dayVisitors = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int YestodayVisitors
        {
            get { return yestodayVisitors; }
            set { yestodayVisitors = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AverageDayVisitors
        {
            get { return averageDayVisitors; }
            set { averageDayVisitors = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int YearPageview
        {
            get { return yearPageview; }
            set { yearPageview = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MonthPageview
        {
            get { return monthPageview; }
            set { monthPageview = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int DayPageview
        {
            get { return dayPageview; }
            set { dayPageview = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int YestodayPageview
        {
            get { return yestodayPageview; }
            set { yestodayPageview = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int AverageDayPageview
        {
            get { return averageDayPageview; }
            set { averageDayPageview = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int OnlineVisitors
        {
            get { return onlineVisitors; }
            set { onlineVisitors = value; }
        }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        /// <summary>
        /// ��ǰ���ݴ���ʱ��
        /// </summary>
        public DateTime CreateDate { get; set; }


        #region ICloneable ��Ա

        object ICloneable.Clone() //�����ֶν�Ϊֵ���ͣ����Խ���ǳ����
        {
            return this.MemberwiseClone();
        }

        public VisiteCount Clone() //�����ֶν�Ϊֵ���ͣ����Խ���ǳ����
        {
            return this.MemberwiseClone() as VisiteCount;
        }

        #endregion
    }

    public class PageVisitorHistory : PageVisitor
    {
        public PageVisitorHistory() { }
    }
}
