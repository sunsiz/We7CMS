using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using We7.Framework;

namespace We7.CMS.Common
{
    /// <summary>
    /// ���ݿؼ�������
    /// </summary>
    [Serializable]
    public class DataControlParameter : IXml, IJsonResult
    {
        string name;
        string title;
        string description;
        string type;
        bool required;
        string maximum;
        string minium;
        int length;
        string defaultValue;
        string data;
        string supportCopy;

        /// <summary>
        /// ���ݿؼ������๹�캯��
        /// </summary>
        public DataControlParameter()
        {
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// ���ֵ
        /// </summary>
        public string Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }

        /// <summary>
        /// ��Сֵ
        /// </summary>
        public string Minium
        {
            get { return minium; }
            set { minium = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }        

        /// <summary>
        /// ��ע��Ϣ
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// �Ƿ��Ǳ����
        /// </summary>
        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        /// <summary>
        /// Ĭ��ֵ
        /// </summary>
        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public bool Ignore { get; set; }


        public bool SupportCopy { get; set; }
        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Parameter");
            xe.SetAttribute("name", Name);
            xe.SetAttribute("title", Title);
            xe.SetAttribute("type", Type);
            xe.SetAttribute("maximum", Maximum);
            xe.SetAttribute("minimum", Minium);
            xe.SetAttribute("length", Length.ToString());
            xe.SetAttribute("description", Description);
            xe.SetAttribute("required", Required.ToString());
            xe.SetAttribute("defaultValue", DefaultValue);
            xe.SetAttribute("supportCopy", SupportCopy.ToString());
            xe.SetAttribute("data", Data);
            return xe;
        }

        /// <summary>
        /// ��ȡXML��Ϣ
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public IXml FromXml(XmlElement xe)
        {
            Name = xe.GetAttribute("name");
            Title = xe.GetAttribute("title");
            Type = xe.GetAttribute("type");
            Description = xe.GetAttribute("description");
            Required = xe.GetAttribute("required") == Boolean.TrueString;
            Maximum = xe.GetAttribute("maximum");
            Minium = xe.GetAttribute("minimum");
            Length = Convert.ToInt32(xe.GetAttribute("length"));
            DefaultValue = xe.GetAttribute("defaultValue");
            Data = xe.GetAttribute("data");
            SupportCopy = xe.GetAttribute("supportCopy") == Boolean.TrueString;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            int num;
            StringBuilder sb = new StringBuilder("{");
            sb.AppendFormat("{0}:'{1}',", "name", name);
            sb.AppendFormat("{0}:'{1}',", "title", string.IsNullOrEmpty(title)?"ȱ��������":title);
            sb.AppendFormat("{0}:'{1}',", "description", description);
            sb.AppendFormat("{0}:'{1}',", "type", type);
            sb.AppendFormat("{0}:{1},", "required", required.ToString().ToLower());
            int.TryParse(maximum, out num);
            sb.AppendFormat("{0}:{1},", "maximum", num);
            int.TryParse(minium, out num);
            sb.AppendFormat("{0}:{1},", "minium", num);
            sb.AppendFormat("{0}:{1},", "length", length);
            sb.AppendFormat("{0}:'{1}',", "data", Data);
            sb.AppendFormat("{0}:'{1}',", "defaultValue", defaultValue);
            sb.AppendFormat("{0}:{1},", "weight", Weight);
            sb.AppendFormat("{0}:{1},", "ignore", Ignore.ToString().ToLower());
            sb.AppendFormat("{0}:{1}", "supportCopy", SupportCopy.ToString().ToLower());
            sb.Append("}");
            return sb.ToString();
        }       


        #region IX ��Ա

        XmlElement IXml.ToXml(XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        IXml IXml.FromXml(XmlElement xe)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
