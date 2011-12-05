using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;

using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// ģ��屾ҵ����
    /// </summary>
    [Serializable]
    [Helper("We7.TemplateVersionHelper")]
    public class TemplateVersionHelper : BaseHelper
    {
        /// <summary>
        /// �汾��Ϣ�ļ�·��
        /// </summary>
        public string TemplateVersionFileName
        {
            get { return  Constants.TemplateVersionFileName; }
        }

        /// <summary>
        /// ����汾��Ϣ
        /// </summary>
        /// <param name="tv">�汾��Ϣ</param>
        public void SaveTemplateVersion(TemplateVersion tv)
        {
            string target = Path.Combine(tv.BasePath, tv.FileName);
            tv.ToFile(target);
        }
        /// <summary>
        /// ȡ�ð汾��Ϣ
        /// </summary>
        /// <param name="tvPath">�汾��Ϣ�ļ�·��</param>
        /// <returns></returns>
        public TemplateVersion GetTemplateVersion(string tvPath)
        {
            string templateVersionPath = tvPath;

            if (File.Exists(Path.Combine(templateVersionPath, TemplateVersionFileName)))
            {
                TemplateVersion t = new TemplateVersion();
                t.FromFile(templateVersionPath,TemplateVersionFileName);
                return t;
            }
            return null;
        }
    }
}
