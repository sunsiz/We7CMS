using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// ����������ʽ
    /// </summary>
    public enum AdviceParticipateMode : int
    {
        /// <summary>
        /// �ʼ�����
        /// </summary>
        Mail = 1,

        /// <summary>
        /// ����֪ͨ
        /// </summary>
        SMS = 2,

        /// <summary>
        /// �ʼ�����Ͷ���֪ͨ
        /// </summary>
        All = 3,
        /// <summary>
        /// ��������
        /// </summary>
        None=-1
    }
}