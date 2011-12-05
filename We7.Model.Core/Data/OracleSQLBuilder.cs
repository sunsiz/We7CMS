﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using System.Data;
using We7.Framework.Util;
using We7.Framework;

namespace We7.Model.Core.Data
{
    public class OracleSQLBuilder:ISQLBuilder
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        const string SQLINSERT = "INSERT INTO [{0}]({1}) VALUES({2})";
        /// <summary>
        /// 分页列表
        /// </summary>
        const string SQLPAGELIST = "SELECT * FROM (SELECT * FROM {1} {2} ORDER BY {3}) AS TB_1 WHERE ROWNUM<={0} AND ROWNUM>={4}";
//@"SELECT * FROM (SELECT TOP {4} * FROM (SELECT TOP {0} * FROM {1} {2} ORDER BY {3}) AS TB_1 ORDER BY {5}) AS TB_2 ORDER BY {3}";
        /// <summary>
        /// 前多少条记录
        /// </summary>
        const string SQLTOPLIST = @"SELECT * FROM (SELECT * FROM {1} {2} ORDER BY {3}) WHERE ROWNUM<={0}"; //@" SELECT TOP {0} * FROM {1} {2} ORDER BY {3}";
        /// <summary>
        /// 列表数据
        /// </summary>
        const string SQLLIST = "SELECT * FROM {0} {1} ORDER BY {2}";
        /// <summary>
        /// 符合条件的记录条数
        /// </summary>
        const string SQLCOUNT = "SELECT COUNT(*) FROM {0} {1}";
        /// <summary>
        /// 更新数据记录
        /// </summary>
        const string SQLUPDATE = "UPDATE {0} SET {1} {2}";
        /// <summary>
        /// 删除数据行
        /// </summary>
        const string SQLDELETE = "DELETE FROM {0} {1}";

        Regex regex = new Regex(@"\bDESC\b|\bASC\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #region ISQLBuilder 成员

        public string BuildInsertSQL(PanelContext data, System.Data.IDataParameterCollection parameters)
        {
            StringBuilder fieldlist = new StringBuilder();
            StringBuilder paramlist = new StringBuilder();
            foreach (DataField field in data.Row)
            {
                string paramname = String.Format("@{0}", field.Column.Name);
                fieldlist.AppendFormat("[{0}],", field.Column.Name);
                paramlist.Append(paramname).Append(",");
                if (field.Column.DataType == TypeCode.DateTime)
                {
                    OracleParameter param = new OracleParameter(field.Column.Name,OracleType.DateTime);
                    param.Value = field.Value;
                    parameters.Add(param);
                }
                else
                {
                    parameters.Add(new OracleParameter(paramname, field.Value));
                }
            }
            Utils.TrimEndStringBuilder(fieldlist, ",");
            Utils.TrimEndStringBuilder(paramlist, ",");
            return String.Format(SQLINSERT, data.Table.Name, fieldlist, paramlist);
        }

        public string BuildDeleteSQL(PanelContext data, System.Data.IDataParameterCollection parameters)
        {
            StringBuilder sqlwhere = BuilderWhereSqlByDataKey(data, parameters, "");
            return String.Format(SQLDELETE, data.Table.Name, sqlwhere);
        }

        public string BuildUpdateSQL(PanelContext data, System.Data.IDataParameterCollection parameters)
        {
            StringBuilder sqlupdatefield = new StringBuilder();
            foreach (DataField field in data.Row)
            {
                sqlupdatefield.AppendFormat("{0}=@{0},", field.Column.Name);
                if (field.Column.DataType == TypeCode.DateTime)
                {
                    OracleParameter param = new OracleParameter(field.Column.Name, OracleType.DateTime);
                    param.Value = field.Value;
                    parameters.Add(param);
                }
                else
                {
                    parameters.Add(new OracleParameter(field.Column.Name, field.Value));
                }
            }
            Utils.TrimEndStringBuilder(sqlupdatefield, ",");
            StringBuilder sqlwhere = BuilderWhereSqlByDataKey(data, parameters, "old");
            return String.Format(SQLUPDATE, data.Table.Name, sqlupdatefield, sqlwhere);
        }

        public string BuildCountSQL(PanelContext data, System.Data.IDataParameterCollection parameters)
        {
            StringBuilder sqlwehre = BuildWhereSql(data, parameters);

            return String.Format(SQLCOUNT, data.Table.Name, sqlwehre);
        }

        public string BuildListSQL(PanelContext data, System.Data.IDataParameterCollection parameters, int startindex, int itemcount)
        {
            StringBuilder sqlwehre = BuildWhereSql(data, parameters);

            string sql = "";
            if (startindex > 0)
            {
                string revorders = regex.Replace(data.Orders, delegate(Match m)
                {
                    if (String.Compare(m.Value, "desc", true) == 0)
                        return "ASC";
                    else
                        return "DESC";
                });
                int firstcount = itemcount + startindex;
                //sql = String.Format(SQLPAGELIST, firstcount, data.Table.Name, sqlwehre, data.Orders, itemcount, revorders, data.Orders);
                sql = String.Format(SQLPAGELIST, firstcount, data.Table.Name, sqlwehre, data.Orders, itemcount);
            }
            else if (itemcount > 0)
            {
                sql = String.Format(SQLTOPLIST, itemcount, data.Table.Name, sqlwehre, data.Orders);
            }
            else
            {

                sql = String.Format(SQLLIST, data.Table.Name, sqlwehre, data.Orders);
            }
            return sql;
        }

        public string BuildGetSQL(PanelContext data, System.Data.IDataParameterCollection parameters)
        {
            StringBuilder sqlwehre = BuilderWhereSqlByDataKey(data, parameters, "");
            string sql = String.Format(SQLLIST, data.Table.Name, sqlwehre, data.Panel.Context.Orders);
            return sql;
        }

        public System.Data.IDbConnection GetConnection()
        {
            return new OracleConnection();
        }

        public System.Data.IDbCommand GetCommand()
        {
            return new OracleCommand();
        }

        public System.Data.IDbDataAdapter GetDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public System.Data.IDbDataParameter GetDataparameter()
        {
            return new OracleParameter();
        }

        #endregion

        StringBuilder BuilderWhereSqlByDataKey(PanelContext data, IDataParameterCollection parameters, string prefix)
        {
            if (data.Panel.Context.DataKeys == null)
            {
                Logger.Error(this.GetType() + "::不存在主键字段");
                throw new Exception("不存在主键");
            }
            StringBuilder sqlwehre = new StringBuilder();
            for (int i = 0; i < data.Panel.Context.DataKeys.Length; i++)
            {
                if (i == 0)
                {
                    sqlwehre.Append(" WHERE ");
                }
                else
                {
                    sqlwehre.Append(" AND ");
                }

                string fieldname = data.Panel.Context.DataKeys[i];
                string paramname = String.Format("@{0}{1}", prefix, fieldname);
                sqlwehre.AppendFormat(" {0}={1} ", fieldname, paramname);
                We7DataColumn column = data.DataSet.Tables[0].Columns[fieldname];
                if (column != null && column.DataType == TypeCode.DateTime)
                {
                    OracleParameter param = new OracleParameter(column.Name,OracleType.DateTime);
                    param.Value = data.DataKey[fieldname];
                    parameters.Add(param);
                }
                else
                {
                    parameters.Add(new OracleParameter(paramname, data.DataKey[fieldname]));
                }
            }
            return sqlwehre;
        }

        StringBuilder BuildWhereSql(PanelContext data, IDataParameterCollection parameters)
        {
            return BuildWhereSql(data, parameters, "");
        }

        StringBuilder BuildWhereSql(PanelContext data, IDataParameterCollection parameters, string prefix)
        {
            StringBuilder sqlwehre = new StringBuilder();
            for (int i = 0; i < data.QueryFields.Count; i++)
            {
                QueryField qf = data.QueryFields[i];
                if (i == 0)
                {
                    sqlwehre.Append(" WHERE ");
                }
                else
                {
                    sqlwehre.Append(" AND ");
                    //sqlwehre.Append(fd.Operator == QueryMode.OR ? " OR " : " AND ");
                }

                string paramname = String.Format("@{0}{1}", prefix, qf.Column.Name);
                switch (qf.Operator)
                {
                    case OperationType.EQUER:
                        sqlwehre.AppendFormat(" {0}={1} ", qf.Column.Name, paramname);
                        break;
                    case OperationType.NOTEQUER:
                        sqlwehre.AppendFormat(" {0}<>{1} ", qf.Column.Name, paramname);
                        break;
                    case OperationType.LIKE:
                        sqlwehre.AppendFormat(" {0} like '%'+{1}+'%' ", qf.Column.Name, paramname);
                        break;
                    case OperationType.LESSTHAN:
                        sqlwehre.AppendFormat(" {0}<{1} ", qf.Column.Name, paramname);
                        break;
                    case OperationType.MORETHAN:
                        sqlwehre.AppendFormat(" {0}>{1} ", qf.Column.Name, paramname);
                        break;
                    case OperationType.LESSTHANEQURE:
                        sqlwehre.AppendFormat(" {0}<={1} ", qf.Column.Name, paramname);
                        break;
                    case OperationType.MORETHANEQURE:
                        sqlwehre.AppendFormat(" {0}>={1} ", qf.Column.Name, paramname);
                        break;
                    default:
                        sqlwehre.AppendFormat(" {0}={1} ", qf.Column.Name, paramname);
                        break;
                }
                parameters.Add(new OracleParameter(paramname, qf.Value));
            }
            return sqlwehre;
        }
    }
}
