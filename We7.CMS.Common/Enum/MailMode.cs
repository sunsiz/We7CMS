using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// �ʼ���������
    /// </summary>
    public enum MailMode
    {
        /// <summary>
        /// �·����ʼ�֪ͨ������
        /// </summary>
        MailNotify = 01,

        /// <summary>
        ///���ʼ���ʽֱ��ת��������
        /// </summary>
        HandleByMail = 02,

        /// <summary>
        /// �ʼ�֪ͨ�����ˣ������ӽ����̨����
        /// </summary>
        MailHyperLink = 03,

        ///// <summary>
        ///// �ʼ��߰�����
        ///// </summary>
        //MailRemind = 04,
    }
}