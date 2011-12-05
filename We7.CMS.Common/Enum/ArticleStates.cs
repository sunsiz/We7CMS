using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// ����״̬
    /// </summary>
    public enum ArticleStates
    {
        /// <summary>
        /// ��ͣ��
        /// </summary>
        Stopped = 0,

        /// <summary>
        /// ������
        /// </summary>
        Started = 1,

        /// <summary>
        /// �����
        /// </summary>
        Checking = 2,

        /// <summary>
        /// �ѹ���
        /// </summary>
        Overdued = 3,

        /// <summary>
        /// ����վ
        /// </summary>
        Recycled = -1,

        /// <summary>
        /// ȫ��
        /// </summary>
        All = 99
    }
}