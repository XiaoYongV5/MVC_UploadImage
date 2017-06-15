using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web;
using System.Data.Common;
using EntityFramework.Extensions;
using MVC图片上传.Models;

namespace MVC图片上传.Models.Base
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {

        private DbContext Context
        {
            get
            {
                DbContext db = (DbContext)CallContext.GetData("DbContext");
                if (db == null)
                {
                    db = new DbContext();
                    // db.Database.Log = o => LoggingHelper.Instance.Logging(LogLevel.Debug, o);
                    CallContext.SetData("DbContext", db);
                }
                return db;
            }
        }


        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isNoTracking">(默认不跟踪实体状态)使用NoTracking的查询会在性能方面得到改善</param>
        /// <returns></returns>
        public IQueryable<T> Find(Expression<Func<T, bool>> exp = null, bool isNoTracking = true)
        {
            return Filter(exp, isNoTracking);
        }

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="isNoTracking">(默认不跟踪实体状态)使用NoTracking的查询会在性能方面得到改善</param>
        /// <param name="values"></param>
        /// <returns></returns>
        public IQueryable<T> Find(string whereLambda = null, bool isNoTracking = true, params object[] values)
        {
            return Filter(whereLambda, isNoTracking, values);
        }

        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool IsExist(Expression<Func<T, bool>> exp)
        {
            return Context.Set<T>().Any(exp);
        }

        /// <summary>
        /// 查找单个(如果没找到则返回为NULL)
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isNoTracking">(默认不跟踪实体状态)使用NoTracking的查询会在性能方面得到改善</param>
        /// <returns></returns>
        public T FindSingle(Expression<Func<T, bool>> exp, bool isNoTracking = true)
        {
            return Filter(exp, isNoTracking).FirstOrDefault();
        }

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageindex.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="total">总条数</param>
        /// <param name="exp">条件谓词</param>
        /// <param name="orderBy">排序，格式如："Id"/"Id descending"</param>
        public IQueryable<T> Find(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> exp = null, string orderBy = "")
        {
            if (pageIndex < 1) pageIndex = 1;
            var query = Filter(exp);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderBy(orderBy);
            total = query.Count();
            ///return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return null;
        }

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageindex.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="total">总条数</param>
        /// <param name="whereLambda">条件谓词</param>
        /// <param name="orderBy">排序，格式如："Id"/"Id descending"</param>
        public IQueryable<T> Find(int pageIndex, int pageSize, out int total, string whereLambda = "", string orderBy = "", params object[] values)
        {
            if (pageIndex < 1) pageIndex = 1;
            var query = Filter(whereLambda);
            if (string.IsNullOrEmpty(orderBy))
                query = query.OrderBy(orderBy);
            total = query.Count();
            // return query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return null;
        }

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        public int GetCount(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp).Count();
        }

        /// <summary>
        /// 添加书体
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        /// <returns></returns>
        public int Add(T entity, bool isComit = true)
        {

            Context.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
            return isComit ? Context.SaveChanges() : 0;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        public int Adds(List<T> entitis, bool isComit = true)
        {
            foreach (T item in entitis)
            {
                Context.Entry<T>(item).State = System.Data.Entity.EntityState.Added;
            }
            return isComit ? Context.SaveChanges() : 0;
        }
        /// <summary>
        /// 更新实体（会更新实体的所有属性）
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        /// <returns></returns>
        public int Update(T entity, bool isComit = true)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return isComit ? Context.SaveChanges() : 0;
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        /// <returns></returns>
        public int Delete(T entity, bool isComit = true)
        {
            Context.Set<T>().Remove(entity);
            return isComit ? Context.SaveChanges() : 0;
        }


        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="entity">The entity.</param>
        public int Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            return Context.Set<T>().Where(where).Update(entity);
        }

        /// <summary>
        /// 批量按条件删除
        /// </summary>
        /// <param name="exp"></param>
        public int Delete(Expression<Func<T, bool>> exp)
        {
            return Context.Set<T>().Where(exp).Delete();
        }

        /// <summary>
        /// 对数据库执行给定的 DDL/DML 命令。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteSqlCommand(string sql, params SqlParameter[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 执行原始的sql查询
        /// </summary>
        /// <typeparam name="TElement">返回的泛型类型</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IList<TElement> SqlQuery<TElement>(string sql, params SqlParameter[] parameters)
        {
            return Context.Database.SqlQuery<TElement>(sql, parameters).ToList();
        }


        public bool BeginTransaction(Func<bool> fun)
        {
            using (var trans = Context.Database.BeginTransaction())
            {
                try
                {
                    var result = fun();
                    trans.Commit();
                    return result;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }

            }
        }

        private IQueryable<T> Filter(Expression<Func<T, bool>> exp = null, bool isNoTracking = true)
        {
            var dbSet = Context.Set<T>().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            if (isNoTracking)
                dbSet = dbSet.AsNoTracking();
            return dbSet;
        }

        private IQueryable<T> Filter(string whereLambda = null, bool isNoTracking = true, params object[] values)
        {
            var dbSet = Context.Set<T>().AsQueryable();
            if (whereLambda != null)
                dbSet = dbSet.Where(whereLambda, values);
            if (isNoTracking)
                dbSet = dbSet.AsNoTracking();
            return dbSet;
        }
        public IQueryable<T> Table
        {
            get
            {
                return Find(whereLambda: null);
            }
        }

        public IQueryable<TEntity> OtherTable<TEntity>() where TEntity : BaseEntity
        {
            return Context.Set<TEntity>().AsNoTracking().AsQueryable();
        }
        public IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return Context.Set<TEntity>();
        }

        /// <summary>
        /// 执行SQL查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbRawSqlQuery<int> ExecuteSqlQuery(string sql)
        {
            try
            {
                return Context.Database.SqlQuery<int>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 执行SQL语句或存储过程
        /// 返回Datatable数据集
        /// </summary>
        /// <param name="sql">SQL语句或存储过程 例如：exec usp_procedure</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        public System.Data.DataTable SqlQueryForDataTable(string sql, params System.Data.Common.DbParameter[] parameters)
        {
            SqlConnection conn = new System.Data.SqlClient.SqlConnection();
            try
            {
                conn = (SqlConnection)Context.Database.Connection;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand();
                StringBuilder sb = new StringBuilder(sql);
                if (parameters != null && parameters.Length > 0)
                {
                    if (sql.StartsWith("exec ", StringComparison.OrdinalIgnoreCase))
                        sb.AppendFormat(" {0}", string.Join(",", parameters.Select(o => o.ParameterName).ToArray()));
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                cmd.Connection = conn;
                cmd.CommandText = sb.ToString();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);

                conn.Close();//连接需要关闭
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        /// <summary>
        /// 返回Datatable数据集
        /// </summary>
        /// <param name="proName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        [Obsolete("此方法已过时,请改用SqlQueryForDataTable")]
        public System.Data.DataTable ExecuteForDataTable(string proName, IDataParameter[] parameters)
        {
            try
            {
                SqlConnection conn = new System.Data.SqlClient.SqlConnection();
                conn.ConnectionString = Context.Database.Connection.ConnectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(proName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Length > 0)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}