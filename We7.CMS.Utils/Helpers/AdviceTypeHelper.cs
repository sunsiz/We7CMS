using System;
using System.Collections.Generic;
using System.Text;


using Thinkment.Data;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Reflection;
using We7.CMS.Common;
using We7.Framework;

namespace We7.CMS
{
    /// <summary>
    /// ��������
    /// </summary>
    [Serializable]
    [Helper("We7.AdviceTypeHelper")]
    public class AdviceTypeHelper:BaseHelper
    {
        /// <summary>
        /// ��ȡһ����ѯͶ������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdviceType GetAdviceType(string id)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", id);
            List<AdviceType> list = Assistant.List<AdviceType>(c, null);
            if (list != null && list.Count > 0)
                return list[0];
            else
                return null;

        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns></returns>
        public List<AdviceType> GetAdviceTypes()
        {
            return Assistant.List<AdviceType>(null, null);
        }

        public AdviceType GetAdviceTypeByModelName(string modelName)
        {
            Criteria c=new Criteria(CriteriaType.Equals,"ModelName",modelName);
            List<AdviceType> types=Assistant.List<AdviceType>(c, null);
            return types != null && types.Count > 0 ? types[0] : null;
        }

        /// <summary>
        ///  ���ݲ�ѯ������ѯ��������
        /// </summary>
        /// <param name="selectName"></param>
        /// <param name="ids"></param>
        /// <param name="accountName"></param>
        /// <returns></returns>
        public int GetAdviceTypeCountByName(string selectName, List<string> ids, bool accountName)
        {
            if (selectName != "" || accountName == true)
            {
                Criteria c = new Criteria(CriteriaType.None);
                if (selectName != null && selectName != "")
                {
                    c.Add(CriteriaType.Like, "Title", "%" + selectName + "%");
                }
                if (accountName == true)
                {
                    if (ids != null && ids.Count > 0)
                    {
                        Criteria keyCriteria = new Criteria(CriteriaType.None);
                        foreach (string id in ids)
                        {
                            keyCriteria.Mode = CriteriaMode.Or;
                            keyCriteria.AddOr(CriteriaType.Equals, "AccountID", id);
                            c.Criterias.Add(keyCriteria);
                        }
                    }
                }
                if (c.Criterias.Count > 0)
                {
                    return Assistant.Count<AdviceType>(c);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Assistant.Count<AdviceType>(null);
            }
        }
        /// <summary>
        /// ��ѯ������ɸѡģ�����
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ids"></param>
        /// <param name="accountName"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<AdviceType> SearchAdviceTypeByName(string name, List<string> ids, bool accountName, int from, int count)
        {
            try
            {
                Order[] o = new Order[] { new Order("Updated", OrderMode.Desc) };
                if (name != "" || accountName == true)
                {
                    Criteria c = new Criteria(CriteriaType.None);
                    if (name != null && name != "")
                    {
                        c.Add(CriteriaType.Like, "Title", "%" + name + "%");
                    }

                    if (accountName == true)
                    {
                        if (ids != null && ids.Count > 0)
                        {
                            Criteria keyCriteria = new Criteria(CriteriaType.None);
                            foreach (string id in ids)
                            {
                                keyCriteria.Mode = CriteriaMode.Or;
                                keyCriteria.AddOr(CriteriaType.Equals, "AccountID", id);
                                c.Criterias.Add(keyCriteria);
                            }
                        }
                    }
                    if (c.Criterias.Count > 0)
                    {
                        return Assistant.List<AdviceType>(c, o, from, count);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return Assistant.List<AdviceType>(null, o, from, count);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="at"></param>
        public void AddAdviceType(AdviceType at)
        {
            //at.ID = Helper.CreateNewID();
            at.CreateDate = DateTime.Now;
            Assistant.Insert(at);
            SaveToConfigFile();
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAdviceType(string id)
        {
            AdviceType at = GetAdviceType(id);
            Assistant.Delete(at);
            SaveToConfigFile();
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteAdviceType(List<string> ids)
        {
            foreach (string id in ids)
                DeleteAdviceType(id);

            SaveToConfigFile();
        }

        /// <summary>
        /// �����Ƿ���ڣ�true:���ڣ�false:������
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool IsExistTitle(string title)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Title", title);
            return (Assistant.Count<AdviceType>(c) > 0) ? true : false;
        }

        /// <summary>
        /// ��ȡһ���������
        /// </summary>
        /// <param name="title">�����б�����</param>
        /// <returns></returns>
        public string GetAdviceTypeID(string title)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Title", title);
            if (Assistant.Count<AdviceType>(c) > 0)
            {
                return Assistant.List<AdviceType>(c, null)[0].ID;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// ����ģ�����ID��ȡ��ǰ��������
        /// </summary>
        /// <param name="bindTypeID"></param>
        /// <returns>��������XML�ִ���δ�ҵ��򷵻�string.Empty</returns>
        public string GetAdviceModel(string bindTypeID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", bindTypeID);
            if (Assistant.Count<AdviceType>(c) > 0)
            {
                string xml = Assistant.List<AdviceType>(c, null)[0].ModelXml;
                if (xml != "")
                {
                    return xml;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// ���·�������
        /// </summary>
        /// <param name="id">id����ID</param>
        /// <param name="adviceXml">���͵�xml�ĸ���</param>
        public void UpdateAdviceType(string id, string modelName)
        {
            AdviceType adviceType = GetAdviceType(id);
            if (adviceType != null)
            {
                adviceType.Updated = DateTime.Now;
                adviceType.ModelName = modelName;
                Assistant.Update(adviceType, new string[] { "ModelName", "Updated" });
            }
        }

        /// <summary>
        /// ����һ���������
        /// </summary>
        /// <param name="adviceType">�������ʵ��</param>
        public void UpdateAdviceType(AdviceType adviceType)
        {
            if (adviceType != null)
            {
                Assistant.Update(adviceType);
                SaveToConfigFile();
            }
        }

        /// <summary>
        /// ����һ���������
        /// </summary>
        /// <param name="adviceType">�������ʵ��</param>
        /// <param name="fields">Ҫ���µ��ֶμ���</param>
        public void UpdateAdviceType(AdviceType adviceType, string[] fields)
        {
            if (adviceType != null)
            {
                Assistant.Update(adviceType,fields);
                SaveToConfigFile();
            }
        }

        /// <summary>
        /// ���·�������XML�ļ�
        /// </summary>
        public void SaveToConfigFile()
        {
            string adviceTypeFile = "/config/advicetype.xml";
            HttpContext Context = HttpContext.Current;
            if (Context != null)
            {
                string path = Context.Server.MapPath(adviceTypeFile);
                if (File.Exists(path))
                    File.Delete(Context.Server.MapPath(adviceTypeFile));
                XmlDocument doc = new XmlDocument();
                string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<configuration>{0}</configuration>";
                List<AdviceType> alltypes = GetAdviceTypes();
                string items = "";
                foreach (AdviceType atype in alltypes)
                {
                    string itemString = "<item name=\"{0}\" value=\"{1}\" />\r\n";
                    items += string.Format(itemString, atype.Title, atype.ID);
                }
                xml = string.Format(xml, items);
                doc.LoadXml(xml);
                doc.Save(path);
            }
        }
    }
}
