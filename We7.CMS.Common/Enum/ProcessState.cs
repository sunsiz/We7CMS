using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// ���״̬
    /// </summary>
    public enum ProcessStates
    {
        /// <summary>
        /// �ݸ�
        /// </summary>
        Unaudit = 0,

        /// <summary>
        /// ����
        /// </summary>
        WaitAccept = -1,

        /// <summary>
        /// ������
        /// </summary>
        WaitHandle = -3,

        /// <summary>
        /// һ��
        /// </summary>
        FirstAudit = 1,

        /// <summary>
        /// ����
        /// </summary>
        SecondAudit = 2,

        /// <summary>
        /// ����
        /// </summary>
        ThirdAudit = 3,

        /// <summary>
        /// վ�����
        /// </summary>
        SiteAudit = 8,

        /// <summary>
        /// ��ϣ����
        /// </summary>
        EndAudit = 99,

        /// <summary>
        /// ���
        /// </summary>
        Finished=100,
        /// <summary>
        /// δ֪
        /// </summary>
        Unknown=-100

    }
}