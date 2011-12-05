using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Web;

using We7.CMS.Common.PF;
using We7.Framework;
using Thinkment.Data;
using We7.Framework.Util;
using We7.CMS.Accounts;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// ����ҵ����
    /// </summary>
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {
        private static readonly string[] DEPARTMENTGENERALKEYS = new string[] { "FromSiteID", "ParentID", "ID", "Name", "Index", "Created", "FullName" };

        #region �½�����
        
        /// <summary>
        /// ���һ��������Ϣ
        /// </summary>
        /// <param name="dpt">һ��������Ϣ</param>
        /// <returns>����������ŵ���Ϣ</returns>
        public Department AddDepartment(Department dpt)
        {
            dpt.ID = We7Helper.CreateNewID();
            dpt.Created = DateTime.Now;
            Assistant.Insert(dpt);
            return dpt;
        }

        #endregion

        #region ɾ������

        /// <summary>
        /// ɾ��һ��������Ϣ
        /// </summary>
        /// <param name="departmentID">����ID</param>
        public void DeleteDepartment(string departmentID)
        {
            List<Department> sub = GetDepartments(string.Empty,departmentID,string.Empty,new string[] { "ID", "ParentID" });
            foreach (Department d in sub)
            {
                DeleteDepartment(d.ID);
            }

            Criteria c = new Criteria(CriteriaType.Equals, "DepartmentID", departmentID);
            if (Assistant.Count<Account>(c) > 0)
            {
                List<Account> list = Assistant.List<Account>(c, null);
                if (list != null)
                {
                    foreach (Account a in list)
                    {
                        DeleteAccont(a.ID);
                    }
                }
            }

            Department dpt = new Department();
            dpt.ID = departmentID;
            Assistant.Delete(dpt);
        }


        #endregion

        #region ��ȡ����

        /// <summary>
        /// ���ݲ������Ʋ�ѯ����ID
        /// </summary>
        /// <param name="name">��������</param>
        /// <returns></returns>
        public Department GetDepartmentByName(string name)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Title", name);
            if (Assistant.Count<Department>(c) > 0)
            {
                List<Department> departments = Assistant.List<Department>(c, null);
                return departments[0];
            }
            else
                return null;
        }

        /// <summary>
        /// ��ȡһ��������Ϣ
        /// </summary>
        /// <param name="departmentID">����ID</param>
        /// <param name="fields">���ص��ֶμ���</param>
        /// <returns>һ��������Ϣ</returns>
        public Department GetDepartment(string departmentID, string[] fields)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", departmentID);
            if (Assistant.Count<Department>(c) > 0)
            {
                List<Department> departments = Assistant.List<Department>(c, null);
                return departments[0];
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// ����IDȡ�ò�����Ϣ
        ///// </summary>
        ///// <param name="id">����ID</param>
        ///// <returns></returns>
        //public Department GetDepartmentName(string id)
        //{
        //    Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
        //    List<Department> Department = Assistant.List<Department>(c, null);
        //    if (Department.Count > 0)
        //    {
        //        return Department[0];
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        ///// <summary>
        ///// ����ID��ȡ��������
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public string GetDepartmentNameById(string id)
        //{
        //    Department Department = GetDepartmentName(id);
        //    return (Department != null && Department.Name != null) ? Department.Name : null;
        //}

        #endregion

        #region �����б�

        /// <summary>
        /// ��ȡһ�������µ�������ز���
        /// </summary>
        /// <param name="parentID">������ID</param>
        /// <param name="selectName">��ѯ�Ĳ�������</param>
        /// <param name="fields">���ص��ֶμ���</param>
        /// <returns>һ�����Ŷ���</returns>
        public List<Department> GetDepartments(string siteID, string parentID, string selectName, string[] fields)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);
            if (!string.IsNullOrEmpty(parentID))
                c.Add(CriteriaType.Equals, "ParentID", parentID);
            if(!string.IsNullOrEmpty(selectName))
                c.Add(CriteriaType.Like, "Name", "%" + selectName + "%");
            if (c.Criterias.Count == 0) c = null;

            Order[] o = new Order[] { new Order("Index") };
            List<Department> dts = Assistant.List<Department>(c, o, 0, 0, fields);
            return dts;
        }

        /// <summary>
        /// ȡ�����еĲ�����Ϣ
        /// </summary>
        /// <param name="begin">��ʼ��¼</param>
        /// <param name="count">��¼����</param>
        /// <returns></returns>
        public List<Department> GetAllDepartment(string siteID,int begin, int count)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);

            return Assistant.List<Department>(c, null, begin, count);
        }

        /// <summary>
        /// ��ѯ���в��ż�¼����
        /// </summary>
        /// <returns></returns>
        public int QueryAllCount(string siteID)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);
            List<Department> items = Assistant.List<Department>(c, null);
            return items.Count;
        }

        #endregion

        #region ���²���

        /// <summary>
        /// ����һ��������Ϣ
        /// </summary>
        /// <param name="dpt">���Ŷ���</param>
        /// <param name="fields">���µ��ֶμ���</param>
        public void UpdateDepartment(Department dpt, string[] fields)
        {
            Assistant.Update(dpt, fields);
        }

        /// <summary>
        /// ���²�������
        ///// </summary>
        /// <param name="departmentID">����ID</param>
        public void UpdateDepartmentFullName(string departmentID)
        {
            string fullName = GetDepartmentFullPath(departmentID);
            Department dpt = new Department();
            dpt.ID = departmentID;
            dpt.FullName = fullName;
            Assistant.Update(dpt, new string[] { "FullName" });
        }

        /// <summary>
        /// ���²���url
        /// </summary>
        /// <param name="departmentId">����ID</param>
        /// <returns>����url</returns>
        string GetDepartmentFullPath(string departmentId)
        {
            if (We7Helper.IsEmptyID(departmentId))
            {
                return "/";
            }
            StringBuilder sb = new StringBuilder();
            string pid = departmentId;
            do
            {
                Department dpt = new Department();
                Criteria c = new Criteria(CriteriaType.Equals, "ID", pid);
                List<Department> dpts = Assistant.List<Department>(c, null);
                if (dpts != null && dpts.Count > 0)
                {
                    dpt = dpts[0];
                    sb.Insert(0, dpt.Name);
                    sb.Insert(0, "/");
                    pid = dpt.ParentID;
                }
                else
                {
                    pid = string.Empty;
                }
            }
            while (!We7Helper.IsEmptyID(pid));
            return sb.ToString();

        }

        #endregion

        #region ���첿�����ṹ

        /// <summary>
        /// ȡ��δ��ʽ����Departments
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Department> GetDepartmentTree(string siteID,string parentId)
        {
            parentId = String.IsNullOrEmpty(parentId) ? We7Helper.EmptyGUID : parentId;
            List<Department> cats = new List<Department>();
            GetNoFmtChildren(siteID,parentId, cats);
            return cats;
        }


        /// <summary>
        /// ȡ��δ��ʽ����Departments������
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Department> GetOrderDepartments(string siteID,string parentId)
        {
            parentId = String.IsNullOrEmpty(parentId) ? We7Helper.EmptyGUID : parentId;
            return GetDepartments(siteID,parentId, string.Empty,DEPARTMENTGENERALKEYS);
        }

        /// <summary>
        /// ȡ�ø�ʽ����Departments
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<Department> GetDepartmentTreeWithFormat(string siteID,string parentId)
        {
            parentId = String.IsNullOrEmpty(parentId) ? We7Helper.EmptyGUID : parentId;
            List<Department> cats = new List<Department>();
            GetFmtChildren(siteID,parentId, cats, "");
            return cats;
        }

        private void GetNoFmtChildren(string siteID,string parentId, List<Department> cats)
        {
            List<Department> children = GetDepartments(siteID, parentId, string.Empty,DEPARTMENTGENERALKEYS);
            for (int i = 0; i < children.Count; i++)
            {
                cats.Add(children[i]);
                GetNoFmtChildren(siteID,children[i].ID, cats);
            }
        }

        private void GetFmtChildren(string siteID,string parentId, List<Department> cats, string prefix)
        {
            List<Department> children = GetDepartments(siteID, parentId, string.Empty,DEPARTMENTGENERALKEYS);
            for (int i = 0; i < children.Count; i++)
            {
                cats.Add(children[i]);
                if (i == children.Count - 1)
                {
                    children[i].Name = prefix + "��" + children[i].Name;
                    GetFmtChildren(siteID,children[i].ID, cats, prefix + "��");
                }
                else
                {
                    children[i].Name = prefix + "��" + children[i].Name;
                    GetFmtChildren(siteID,children[i].ID, cats, prefix + "��");
                }
            }
        }

        #endregion

    }
}
