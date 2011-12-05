// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
//
// (C) 2005 Marek Sieradzki
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Reflection;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace Thinkment.Data
{
    /// <summary>
    /// ���������࣬��������CD.XML�е�ӳ������Ϣ
    /// </summary>
    [Serializable]
    public class Property
    {
        string name;
        string field;
        DbType type;
        int size;
        int scale;
        bool nullable;
        PropertyInfo info;
        bool _readonly;
        string description;

        public Property()
        {
            name = "";
            field = "";
            type = DbType.String;
            size = 10;
            scale = 0;
            nullable = true;
            description = string.Empty;
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// ���ݿ��ֶ�
        /// </summary>
        public string Field
        {
            get { return field; }
            set { field = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public DbType Type
        {
            get { return type; }
            set { type = value; }
        }
        /// <summary>
        /// ��С
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        /// <summary>
        /// ��Ӧdll����������Ϣ
        /// </summary>
        public PropertyInfo Info
        {
            get { return info; }
            set { info = value; }
        }
        /// <summary>
        /// ��ֵ��Χ
        /// </summary>
        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        /// <summary>
        /// ֻ��
        /// </summary>
        public bool Readonly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        /// <summary>
        /// ����Ϊ��
        /// </summary>
        public bool Nullable
        {
            get { return nullable; }
            set { nullable = value; }
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// �����XML
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public XmlElement ToXml(XmlDocument doc)
        {
            XmlElement xe = doc.CreateElement("Property");
            UpdateXmlElement.SetXEAttribute(xe, "name", name);
            UpdateXmlElement.SetXEAttribute(xe, "field", field);
            UpdateXmlElement.SetXEAttribute(xe, "type", type.ToString());
            UpdateXmlElement.SetXEAttribute(xe, "size", size);
            UpdateXmlElement.SetXEAttribute(xe, "scale", scale);
            UpdateXmlElement.SetXEAttribute(xe, "nullable", nullable);
            UpdateXmlElement.SetXEAttribute(xe, "readonly", _readonly);
            UpdateXmlElement.SetXEAttribute(xe, "description",description);
            return xe;
        }

        /// <summary>
        /// ��XML��ȡ������ֵ
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public Property FromXml(XmlElement xe)
        {
            name = xe.GetAttribute("name");
            field = xe.GetAttribute("field");
            type = (DbType)Enum.Parse(typeof(DbType), xe.GetAttribute("type"), true);
            size = UpdateXmlElement.GetXEAttribute(xe, "size", 0);
            scale = UpdateXmlElement.GetXEAttribute(xe, "scale", 0);
            nullable = UpdateXmlElement.GetXEAttribute(xe, "nullable", false);
            _readonly = UpdateXmlElement.GetXEAttribute(xe, "readonly", false);
            description = xe.GetAttribute("description");
            return this;
        }
    }
}