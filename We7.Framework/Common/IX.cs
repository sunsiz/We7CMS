using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace We7
{
    /// <summary>
    /// We7 XML���л��뷴���л��ӿ�
    /// </summary>
    public interface IXml
    {
        /// <summary>
        /// �����XML�ļ�
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        XmlElement ToXml(XmlDocument doc);
        /// <summary>
        /// ��XML�ļ���������
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        IXml FromXml(XmlElement xe);
    }
}
