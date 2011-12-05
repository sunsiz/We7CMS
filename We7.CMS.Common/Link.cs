using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// �������ӱ�
    /// </summary>
    [Serializable]
    public class Link
    {
        string id;
        string title;
        string url;
        DateTime created=DateTime.Now;
        string thumbnail;
        string tag;
        int orderNumber;
        DateTime updated=DateTime.Now;

        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        public Link()
        {
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            get { return title; }
            set 
            {
                 title = value; 
            }
        }
        /// <summary>
        /// url��ַ
        /// </summary>
        public string Url
        {
            get 
            {
                if (url!=null && !url.ToLower().StartsWith("http://"))
                    return "http://" + url;
                else
                    return url;  
            }
            set 
            {
                if (value!=null && !value.ToLower().StartsWith("http://"))
                    url = "http://" + value;
                else
                    url = value;  
             }
        }
        /// <summary>
        /// ͼƬ
        /// </summary>
        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        /// <summary>
        /// ��ǩ
        /// </summary>
        public string Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public int OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }

        }
    }
}
