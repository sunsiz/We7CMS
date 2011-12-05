using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// վȺͨ��֤������Ϣ
    /// </summary>
    [Serializable]
    public class IDToken
    {
       private string passportID;
       private string userName;
       private string providerSiteID;
       private string providerSiteTitle;
       private string loginSiteID;
       private string loginSiteTitle;
       private int status;
       private string returnUrl;
       private string action;
       private List<string> existList;

        public IDToken()
        {
        }

        /// <summary>
        /// ����ID
        /// </summary>
        public string PassportID
        {
            get { return passportID; }
            set { passportID = value; }
        }

        /// <summary>
        /// �û�����
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// ���ṩ��վ��ID
        /// </summary>
        public string ProviderSiteID
        {
            get { return providerSiteID; }
            set { providerSiteID = value; }
        }

        /// <summary>
        /// ���ṩ��վ������
        /// </summary>
        public string ProviderSiteTitle
        {
            get { return providerSiteTitle; }
            set { providerSiteTitle = value; }
        }

        /// <summary>
        /// ע���վ��ID
        /// </summary>
        public string LoginSiteID
        {
            get { return loginSiteID; }
            set { loginSiteID = value; }
        }

        /// <summary>
        /// ע���վ������
        /// </summary>
        public string LoginSiteTitle
        {
            get { return loginSiteTitle; }
            set { loginSiteTitle = value; }
        }

        /// <summary>
        /// ״̬
        /// </summary>
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// վ����ת��ַ
        /// </summary>
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        /// <summary>
        /// �������Ķ���
        /// </summary>
        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> ExistList
        {
            get { return existList; }
            set { existList = value; }
        }


    }
}
