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
using System.Data;

namespace Thinkment.Data
{
    /// <summary>
    /// SQL������
    /// </summary>
	public class DataParameter : IDbDataParameter
	{
		byte precision;
		byte scale;
		int size;
		string sourceColumn;
		string parameterName;
		object oValue;
		DbType dbType;
		ParameterDirection direction;
		DataRowVersion sourceVersion;
        bool isNullable;

		public DataParameter()
		{
			direction = ParameterDirection.Input;
			sourceVersion = DataRowVersion.Default;
			dbType = DbType.String;
			size = 10;
			scale = 0;
			precision = 0;
		}

		public DataParameter(string paraName, object value)
			: this()
		{
            parameterName = paraName;
			oValue = value;
		}
        /// <summary>
        /// ����
        /// </summary>
		public byte Precision
		{
			get { return precision; }
			set { precision = value; }
		}
        /// <summary>
        /// ��ֵ��Χ
        /// </summary>
		public byte Scale
		{
			get { return scale; }
			set { scale = value; }
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
        /// �����������
        /// </summary>
		public ParameterDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}

        /// <summary>
        /// ��������
        /// </summary>
		public DbType DbType
		{
			get { return dbType; }
			set { dbType = value; }
		}

        /// <summary>
        /// ֵ
        /// </summary>
		public object Value
		{
			get { return oValue; }
			set { oValue = value; }
		}

        /// <summary>
        /// ����Ϊ��
        /// </summary>
		public bool IsNullable
		{
            get { return isNullable; }
            set { isNullable = value; }
		}

		public DataRowVersion SourceVersion
		{
			get { return sourceVersion; }
			set { sourceVersion = value; }
		}

        /// <summary>
        /// ��������
        /// </summary>
		public string ParameterName
		{
			get { return parameterName; }
			set { parameterName = value; }
		}
        /// <summary>
        /// ��Դ��
        /// </summary>
		public string SourceColumn
		{
			get { return sourceColumn; }
			set { sourceColumn = value; }
		}
	}
}