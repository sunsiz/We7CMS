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
using System.Collections.Generic;
using System.Text;

namespace Thinkment.Data
{
    /// <summary>
    /// SQL������
    /// </summary>
	[Serializable]
	public class Criteria
	{
		CriteriaMode mode;
		CriteriaType type;
		string name;
		object value;
		List<Criteria> criterias;
        Adorns adorn;
        int start;
        int length;

		public Criteria()
		{
			criterias = new List<Criteria>();
			mode = CriteriaMode.And;
			//type = CriteriaType.Equals;
            type = CriteriaType.None;
		}

        public Criteria(CriteriaType t)
            : this()
        {
            type = t;
        }

		public Criteria(CriteriaType t, string n, object v)
			: this(t)
		{
			name = n;
			value = v;
		}
        /// <summary>
        /// �ֶ���
        /// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

        /// <summary>
        /// �ֶ�ֵ
        /// </summary>
		public object Value
		{
			get { return this.value; }
			set { this.value = value; }
		}

        /// <summary>
        /// ���ģʽ
        /// </summary>
		public CriteriaMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}
        /// <summary>
        /// �������������
        /// </summary>
		public CriteriaType Type
		{
			get { return type; }
			set { type = value; }
		}

        /// <summary>
        /// ���������ʽ�б�
        /// </summary>
		public List<Criteria> Criterias
		{
			get { return criterias; }
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        /// <param name="type">�����</param>
        /// <param name="name">�ֶ�</param>
        /// <param name="value">ֵ</param>
        public void Add(CriteriaType type, string name, object value)
        {
            Add(type, CriteriaMode.And, name, value);
        }

        /// <summary>
        /// ����һ�����򡱵�����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddOr(CriteriaType type, string name, object value)
        {
            Add(type, CriteriaMode.Or, name, value);
        }

        void Add(CriteriaType type, CriteriaMode mode, string name, object value)
        {
            Criteria c = new Criteria(type, name, value);
            c.Mode = mode;
            Criterias.Add(c);
        }

        /// <summary>
        /// �ֶκ���
        /// </summary>
        public Adorns Adorn
        {
            get { return adorn; }
            set { adorn = value; }
        }
        /// <summary>
        /// ��ʼ
        /// </summary>
        public int Start
        {
            get { return start; }
            set { start = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

	}
}
