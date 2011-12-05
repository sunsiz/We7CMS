using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// ����ʵ����
    /// </summary>
    [Serializable]
    public class Department
    {
        /// <summary>
        /// ������
        /// </summary>
        public Department()
        {
            Created = DateTime.Now;
            ParentID = We7Helper.EmptyGUID;
            Children = new List<Department>();
            Updated=DateTime.Now;
            FromSiteID = SiteConfigs.GetConfig().SiteID;
        }
        /// <summary>
        /// ����ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// ��Դվ��ID
        /// </summary>
        public string FromSiteID { get; set; }
        /// <summary>
        /// ��λȫ�ƣ���������������λ��
        /// </summary>
        public string FullName{ get; set; }
        /// <summary>
        /// ������λID
        /// </summary>
        public string ParentID{ get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Name{ get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Description{ get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// �Ӳ����б�
        /// </summary>
        public List<Department> Children{ get; set; }
        /// <summary>
        /// ��λ��ַ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ����վ��URL
        /// </summary>
        public string SiteUrl { get; set; }
        /// <summary>
        /// ��ͼλ�ñ�Ǵ���
        /// </summary>
        public string MapScript { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// ��ϸְ������
        /// </summary>
        public string Text { get; set; }

    }
}
