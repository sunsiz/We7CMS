using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// �������� 
    /// </summary>
    public enum TypeOfPasswordHashed
    {
        /// <summary>
        /// û�м��ܣ������š�
        /// </summary>
        [Description("���ü���")]
        noneEncrypt = 0,

        /// <summary>
        /// ��վ���ܷ�ʽ
        /// </summary>
        [Description("cms���ܷ�ʽ")]
        webEncrypt = 1,

        /// <summary>
        /// ��̳���ܷ�ʽ
        /// </summary>
        [Description("BBS���ܷ�ʽ")]
        bbsEncrypt = 2
    }
}
