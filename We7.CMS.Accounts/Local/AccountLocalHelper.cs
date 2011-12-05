using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;

using Thinkment.Data;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Common.Enum;
using System.Web.Caching;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    /// <summary>
    /// �û�Ȩ�޷����������ش洢����
    /// �����û������š���ɫ��Ȩ�ޣ�
    /// </summary>
    [Serializable]
    [Helper("We7.AccountHelper")]
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {

        #region Ԥ�������

        /// <summary>
        /// Ȩ�޵�Session�ؼ���
        /// </summary>
        public static readonly string AccountSessionKey = "We7.Session.Account.Key";

        /// <summary>
        /// ��ǰ��http������
        /// </summary>
        HttpContext context { get { return HttpContext.Current; } }

        /// <summary>
        ///��ҵ����󹤳�
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)context.Application[HelperFactory.ApplicationID]; }
        }

        #endregion

        #region ע���˺�

        /// <summary>
        /// ���һ���û�
        /// </summary>
        /// <param name="act">�û�����</param>
        /// <returns>�û�����</returns>
        public Account AddAccount(Account act)
        {
            if (act.LoginName.Length < 3)
                throw new Exception("�û�������С��3λ��");

            if (act.Password.Length < 6)
                throw new Exception("���벻��С��6λ��");

            if (GetAccountByLoginName(act.LoginName) != null)
                throw new Exception(string.Format("��¼�� {0} �Ѵ��ڡ�", act.LoginName));

            if (GetAccountByEmail(act.Email) != null)
                throw new Exception(string.Format("�ʼ���ַ {0} �ѱ�ʹ�á�", act.Email));

            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(act, "AddAccount.act");

                        //������������ʻ������Ȩ��ɾ��
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            IConnection ic = Assistant.GetConnections()[db];
            ic.IsTransaction = true;
            try
            {
                act.ID = We7Helper.CreateNewID();
                act.Created = DateTime.Now;
                if (GeneralConfigs.GetConfig().UserRegisterMode == "none")
                    act.State = 1;

                AccountRole ar = new AccountRole();
                ar.AccountID = act.ID;
                ar.RoleID = "1";
                ar.RoleTitle = "ע���û�";
                ar.ID = We7Helper.CreateNewID();

                OnUserAdded(act);
                UpdateUserPassword(act);

                Assistant.Insert(ic, act, null);
                Assistant.Insert(ic, ar, null);

                ic.Commit();
                ic.Dispose();
                return act;
            }
            catch (Exception ex)
            {
                ic.Rollback();
                ic.Dispose();
                throw ex;
             }
        }

        /// <summary>
        /// �жϻ�Ա��¼�����Ƿ��Ѿ�����
        /// </summary>
        /// <param name="userName">��Ա��¼����</param>
        /// <returns>�Ƿ����</returns>
        public bool ExistUserName(string userName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "LoginName", userName);
            List<Account> Account = Assistant.List<Account>(c, null);
            if (Account.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �жϻ�Ա�����ַ�Ƿ��Ѿ�����
        /// </summary>
        /// <param name="email">��Ա�����ַ</param>
        /// <returns></returns>
        public bool ExistEmail(string email)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Email", email);
            List<Account> Account = Assistant.List<Account>(c, null);
            if (Account.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ��ȡ�û�

        /// <summary>
        /// ͨ����¼����ȡһ���û�
        /// </summary>
        /// <param name="loginName">��¼��</param>
        /// <returns>�û�����</returns>
        public Account GetAccountByLoginName(string loginName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "LoginName", loginName);
            if (Assistant.Count<Account>(c) > 0)
            {
                List<Account> act = Assistant.List<Account>(c, null);
                return act[0];
            }
            return null;
        }

        /// <summary>
        /// ��ȡһ���û�
        /// </summary>
        /// <param name="accountID">�û�ID</param>
        /// <param name="fields">���ص��ֶμ���</param>
        /// <returns>�û�����</returns>
        public Account GetAccount(string accountID, string[] fields)
        {
            if (accountID == We7Helper.EmptyGUID)
            {
                Account sa = new Account();
                sa.ID = accountID;
                sa.LastName = "ϵͳ����Ա";
                sa.LoginName = SiteConfigs.GetConfig().AdministratorName;
                return sa;
            }
            else if (!string.IsNullOrEmpty(accountID))
            {
                We7Helper.AssertNotNull(accountID, "GetAccount.accountID");
                Criteria c = new Criteria(CriteriaType.Equals, "ID", accountID);
                List<Account> act = Assistant.List<Account>(c, null, 0, 1, fields);
                if (act.Count > 0)
                {
                    return act[0];
                }
            }

            Account a = new Account();
            a.ID = accountID;
            a.LoginName = "";
            a.LastName = "δ֪�û�";
            return a;
        }

        /// <summary>
        /// �����û������ȡ�û���Ϣ
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <returns>�û���Ϣ</returns>
        public Account GetAccountByEmail(string email)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "Email", email);
            List<Account> account = Assistant.List<Account>(c, null);
            if (account.Count > 0)
            {
                return account[0];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ɾ���˻�

        /// <summary>
        /// ɾ���ʻ���ͬʱɾ��Ȩ���Լ����˺������ʺŵȣ�
        /// </summary>
        /// <param name="accountID">�û�ID</param>
        public void DeleteAccont(string accountID)
        {
            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(accountID, "DeleteAccont.accountID");

            //������������ʻ������Ȩ��ɾ��
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            IConnection ic = Assistant.GetConnections()[db];
            ic.IsTransaction = true;
            try
            {
                //ɾ��Permissions
                Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", accountID);
                Assistant.DeleteList<Permission>(ic, c);
                //ɾ��AccountRole
                Criteria ca = new Criteria(CriteriaType.Equals, "AccountID", accountID);
                Assistant.DeleteList<AccountRole>(ic, ca);
                //���ɾ����ǰ�ʻ�
                Account act = new Account();
                act.ID = accountID;
                Assistant.Delete(ic, act);
                OnUserDeleted(act);

                ic.Commit();
            }
            catch (Exception)
            {
                try { ic.Rollback(); }
                catch (Exception)
                { }
            }
            finally { ic.Dispose(); }
        }

        /// <summary>
        /// ɾ���ʻ���ͬʱɾ��Ȩ���Լ����˺������ʺŵȣ�
        /// </summary>
        /// <param name="ic">����</param>
        /// <param name="accountID">�û�ID</param>
        public void DeleteAccont(IConnection ic, string accountID)
        {
            We7Helper.AssertNotNull(Assistant, "AccountHelper.Assistant");
            We7Helper.AssertNotNull(accountID, "DeleteAccont.accountID");

            //������������ʻ������Ȩ��ɾ��
            try
            {
                //ɾ��Permissions
                Criteria c = new Criteria(CriteriaType.Equals, "OwnerID", accountID);
                try
                {
                    Assistant.DeleteList<Permission>(ic, c);
                }
                catch (Exception)
                {
                }

                //ɾ��AccountRole
                Criteria ca = new Criteria(CriteriaType.Equals, "AccountID", accountID);
                try
                {
                    Assistant.DeleteList<AccountRole>(ic, ca);
                }
                catch (Exception)
                {
                }

                //���ɾ����ǰ�ʻ�
                Account act = new Account();
                act.ID = accountID;
                Assistant.Delete(ic, act);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region �����˻�

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="account">�û�����</param>
        /// <param name="newPassword">������</param>
        /// <returns>�޸Ĺ��������</returns>
        public string UpdatePassword(Account account, string newPassword)
        {
            account.Password = newPassword;
            account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt;
            Assistant.Update(account, new string[] { "Password", "PasswordHashed" });
            OnPasswordUpdated(account);
            UpdateUserPassword(account);
            return "";
        }

        /// <summary>
        /// ���û����������Ϊ������
        /// </summary>
        /// <param name="account"></param>
        public void UpdateUserPassword(Account account)
        {
            if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.noneEncrypt)
            {
                string oldPassword = account.Password;
                account.Password = Security.Encrypt(oldPassword);
                account.PasswordHashed = (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt;
                Assistant.Update(account, new string[] { "Password", "PasswordHashed" });
            }
        }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="act">�û�����</param>
        /// <param name="fields">��Ҫ���µ��ֶμ���</param>
        public void UpdateAccount(Account act, string[] fields)
        {
            Assistant.Update(act, fields);
        }

        /// <summary>
        /// ��ʼ���û���ɫ��ÿ���û�������ϵͳ��ɫ��ע���û�
        /// </summary>
        /// <returns></returns>
        public int InitAllUserRole()
        {
            int total = 0;
            List<Account> allUser = Assistant.List<Account>(null, null);
            foreach (Account a in allUser)
            {
                if (AssignAccountRole(a.ID, "1"))
                    total++;
            }
            return total;
        }

        #endregion

        #region ��ȡ�û��б�

        /// <summary>
        /// ����������ȡһ���û�
        /// </summary>
        /// <param name="departmentID">����ID</param>
        /// <param name="selectName">��½��</param>
        /// <param name="state">�û�״̬</param>
        /// <returns>һ���û���Ϣ</returns>
        public List<Account> GetAccounts(string siteID, string departmentID, string searchKey, OwnerRank type)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (!string.IsNullOrEmpty(siteID))
                c.Add(CriteriaType.Equals, "FromSiteID", siteID);
            if (departmentID != null && departmentID != "")
            {
                Criteria subC = new Criteria(CriteriaType.None);
                subC.Mode = CriteriaMode.Or;
                subC.AddOr(CriteriaType.Equals, "DepartmentID", departmentID);

                List<Department> list=GetDepartmentTree(siteID, departmentID);
                foreach (Department depart in list)
                {
                    subC.AddOr(CriteriaType.Equals, "DepartmentID", depart.ID);
                }
                c.Criterias.Add(subC);
            }
            if (type != OwnerRank.All)
                c.Add(CriteriaType.Equals, "UserType", (int)type);
            if (!string.IsNullOrEmpty(searchKey))
                c.Add(CriteriaType.Like, "LoginName", "%" + searchKey + "%");
            Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
            return c.Criterias.Count > 0 ? Assistant.List<Account>(c, o) : new List<Account>();
        }

        /// <summary>
        /// �����û�ID�б�,��ѯ�û���Ϣ
        /// </summary>
        /// <param name="ownerIds">�û�ID�б�</param>
        /// <returns>�û���Ϣ�б�</returns>
        public List<Account> GetAccountList(List<string> ownerIds)
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Mode = CriteriaMode.Or;
            if (ownerIds != null && ownerIds.Count > 0)
            {
                foreach (string id in ownerIds)
                {
                    c.AddOr(CriteriaType.Equals, "ID", id);
                }
                return Assistant.List<Account>(c, null);
            }
            else
                return null;
        }

        /// <summary>
        /// ͨ����¼����ȡ�û��û���
        /// </summary>
        /// <param name="username">�û���</param>
        /// <returns>��½��</returns>
        public List<string> GetAccountIDSByLoginName(string username)
        {
            Criteria c = new Criteria(CriteriaType.Like, "LoginName", "%" + username + "%");
            List<Account> ars = Assistant.List<Account>(c, null);
            List<string> ids = new List<string>();
            if (ars != null)
            {
                foreach (Account account in ars)
                {
                    ids.Add(account.ID);
                }
            }
            return ids;
        }

        public List<Account> GetAccountList(Criteria c, Order[] o, int begin, int count)
        {
            return Assistant.List<Account>(c, o, begin, count);
        }

        public int GetAccountCountByCriteria(Criteria c)
        {
            return Assistant.Count<Account>(c);
        }

        public List<Account> GetAccounts(Criteria c, Order[] o)
        {
            return Assistant.List<Account>(c, o);
        }

        #endregion

        #region ��¼
        /// <summary>
        /// ��¼
        /// </summary>
        /// <param name="name">��¼��</param>
        /// <param name="password">����</param>
        /// <returns>�ɰ���Ϣ</returns>
        public string[] Login(string name, string password)
        {
            string[] result = { "false", "" };
            Account act = GetAccountByLoginName(name);
            if (act == null)
            {
                result[0] = "false";
                result[1] = "���û�������!";
                return result;
            }
            if (!IsValidPassword(act, password))
            {
                result[0] = "false";
                result[1] = "���벻��ȷ!";
                return result;
            }
            if (GeneralConfigs.GetConfig().UserRegisterMode == "email" && act.EmailValidate != 1)
            {
                result[0] = "false";
                result[1] = "���û���δͨ��Email��֤!";
                return result;
            }
            if (GeneralConfigs.GetConfig().UserRegisterMode == "manual" && act.State != 1)
            {
                result[0] = "false";
                result[1] = "���û���δͨ���˹����!";
                return result;
            }
            if (act.Overdue < DateTime.Today)
            {
                result[0] = "false";
                result[1] = "���Ļ�Աʹ����������ֹ��";
                return result;
            }

            result[0] = "true";
            result[1] = act.ID;
            Security.SetAccountID(act.ID);
            OnUserLogined(act);
            return result;
        }

        /// <summary>
        /// �˳�
        /// </summary>
        /// <returns>�ɰ���Ϣ</returns>
        public string SignOut()
        {
            string result = "";
            try
            {
                Security.SignOut();
            }
            catch (Exception ex)
            {
                result = "�˳�ʧ�ܣ�" + ex.Message;
                return result;
            }
            try
            {
                OnUserSignOut();
            }
            catch (Exception ex)
            {
                result = "ͬ���˳�ʧ�ܣ�" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// ��֤�����Ƿ���ȷ
        /// </summary>
        /// <param name="account">�û�����</param>
        /// <param name="password">����</param>
        /// <returns>������ȷ����true�����󷵻�false</returns>
        public bool IsValidPassword(Account account, string password)
        {
            if (account == null)
            {
                return false;
            }

            if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
            {
                password = Security.Encrypt(password);
            }
            else if (account.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.bbsEncrypt)
            {
                password = Security.BbsEncrypt(password);
            }
            return string.Compare(password, account.Password, false) == 0;
        }

        /// <summary>
        /// ��ȡ��ǰ��¼�˻�
        /// </summary>
        /// <returns></returns>
        public Account GetAuthenticatedAccount()
        {
            if (Security.IsAuthenticated())
                return GetAccount(Security.CurrentAccountID, null);
            else
                return null;
        }
        #endregion

        #region �û���ѯ

        Criteria CreateCriteriaByQuey(AccountQuery query)
        {
            Criteria c = new Criteria(CriteriaType.None);
            if (query.State != 100)
                c.Add(CriteriaType.Equals, "State", query.State);
            if (query.EmailValidate != 100)
                c.Add(CriteriaType.Equals, "EmailValidate", query.EmailValidate);
            if (query.ModelState != 100)
                c.Add(CriteriaType.Equals, "ModelState", query.ModelState);

            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                Criteria keyCriteria = new Criteria(CriteriaType.None);
                keyCriteria.Mode = CriteriaMode.Or;
                keyCriteria.AddOr(CriteriaType.Like, "LastName", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "LoginName", "%" + query.KeyWord + "%");
                keyCriteria.AddOr(CriteriaType.Like, "Email", "%" + query.KeyWord + "%");
                c.Criterias.Add(keyCriteria);
            }

            if (!string.IsNullOrEmpty(query.SiteID))
            {
                c.Add(CriteriaType.Equals, "FromSiteID", query.SiteID);
            }

            if (!string.IsNullOrEmpty(query.ModelName ))
            {
                c.Add(CriteriaType.Equals, "ModelName", query.ModelName );
            }
            if (!string.IsNullOrEmpty(query.DepartmentID))
            {
                c.Add(CriteriaType.Equals, "DepartmentID", query.DepartmentID);
            }

            if (query.UserType!=100)
            {
                c.Add(CriteriaType.Equals, "UserType", query.UserType);
            }
            if (c.Criterias.Count == 0)
                return null;
            else 
                return c;
        }

        /// <summary>
        /// ���ݲ�ѯ�����û�����
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int QueryAccountCountByQuery(AccountQuery query)
        {
            Criteria c = CreateCriteriaByQuey(query);
            return Assistant.Count<Account>(c);
        }

        /// <summary>
        /// ���ղ�ѯ���ṩ��������ѯ�û�
        /// </summary>
        /// <param name="query"></param>
        /// <param name="from"></param>
        /// <param name="count"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public List<Account> QueryAccountsByQuery(AccountQuery query, int from, int count, string[] fields)
        {
            try
            {
                Criteria c = CreateCriteriaByQuey(query);
                List<Order> orders = CreateOrdersByAll(query.OrderKeys);
                Order[] o = new Order[] { new Order("Created", OrderMode.Desc) };
                if (orders != null) o = orders.ToArray();

                return Assistant.List<Account>(c, o, from, count, fields);
            }
            catch (Exception ex)
            {
                return new List<Account>();
            }
        }

        #endregion

    }
}