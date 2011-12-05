using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// ϵͳ�˵��ͬʱ���ڱ�ʶ�ɷ��ʹ�����
    /// </summary>
    [Serializable]
    public class MenuItem
    {
        string id;
        string parentID;
        string name;
        string title;
        string href;
        int type; 
        int index;
        DateTime created=DateTime.Now;
        List<MenuItem> items;
        DateTime updated=DateTime.Now;

        string icon;
        string iconHover;
        int group;
        int menuType;
        string referenceID;

        /// <summary>
        /// ������ţ���0-3
        /// </summary>
        public int Group
        {
            get { return group; }
            set { group = value; }
        }

        /// <summary>
        /// �˵�ͼ�꣺��껬��ͼƬ
        /// </summary>
        public string IconHover
        {
            get { return iconHover; }
            set { iconHover = value; }
        }

        /// <summary>
        /// �˵�ͼ��
        /// </summary>
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        public MenuItem()
        {
            items = new List<MenuItem>();
        }

        /// <summary>
        /// ����
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ���˵�ID
        /// </summary>
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 0 - ϵͳ�˵���1 - �û��Զ���˵���2 - ϵͳ�˵�����
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// �Ӳ˵��б�
        /// </summary>
        public List<MenuItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// �ɷ���URL
        /// </summary>
        public string Href
        {
            get { return href; }
            set { href = value; }
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
        /// Ȩ������ʾ��
        /// </summary>
        public string EntityID { get; set; }

        /// <summary>
        /// ����������URL·��
        /// </summary>
        public string PageLocation
        {
            get
            {
                if (string.IsNullOrEmpty(href))
                    return null;
                else
                {
                    if (Href.IndexOf("?") > -1)
                        return Href.Substring(0, Href.IndexOf("?"));
                    else
                        return Href;
                }
            }
        }

        /// <summary>
        /// URL��������
        /// </summary>
        public string QueryKey
        {
            get
            {
                if (string.IsNullOrEmpty(href))
                    return null;
                else
                {
                    if (Href.IndexOf("?") > -1)
                        return Href.Substring(Href.IndexOf("?")+1);
                    else
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// ����Ȩ����
        /// checked-�ӽڵ�ӵ��ȫ��Ȩ�ޣ�
        /// undetermined-�ӽڵ�ӵ�в���Ȩ��
        /// unchecked-û��Ȩ��
        /// </summary>
        public string PermissionState { get; set; }

        /// <summary>
        /// �˵����ͣ�0����ͨ�˵�,1:����һ���˵�,2:��������,3:�������ͣ�
        /// </summary>
        public int MenuType
        {
            get
            {
                return menuType;
            }
            set
            {
                menuType = value;
            }
        }

        /// <summary>
        ///���õĲ˵�ID 
        /// </summary>
        public string ReferenceID
        {
            get
            {
                return referenceID;
            }
            set
            {
                referenceID = value;
            }
        }




    }

}
