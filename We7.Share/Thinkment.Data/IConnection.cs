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
using System.Data;

namespace Thinkment.Data
{
    /// <summary>
    /// ���ݿ����Ӷ���ӿ�
    /// </summary>
	public interface IConnection : IDisposable
	{
        IDbDriver Driver { get; set; }

        bool IsTransaction { get; set; }

		DataTable Query(SqlStatement sql);

		object QueryScalar(SqlStatement sql);

		int Update(SqlStatement sql);

        void Commit();

        void Rollback();

        bool TableExist(string table);
    }

    /// <summary>
    /// ���ݿ�������չ
    /// </summary>
    public interface IConnectionEx:IConnection
    {
        string ConnectionString { get; set; }

        bool Create { get; set; }
    }
}