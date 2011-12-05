using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// ��Ŀ����
    /// </summary>
    public enum TypeOfChannel : int
    {
        /// <summary>
        /// ԭ������Ŀ
        /// </summary>
        NormalChannel = 0,

        /// <summary>
        /// ��������Ŀ
        /// </summary>
        QuoteChannel = 1,

        /// <summary>
        /// ������Ŀ
        /// </summary>
        ShareChannel = 2,

        /// <summary>
        /// ��ת����Ŀ
        /// </summary>
        ReturnChannel = 3,

        /// <summary>
        /// RssԴ
        /// </summary>
        RssOriginal=4,

        /// <summary>
        /// �սڵ���Ŀ
        /// </summary>
        BlankChannel=5

    }
}
