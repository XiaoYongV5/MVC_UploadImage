using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
namespace MVC图片上传.Models.Base
{
    public interface IRepository<T> where T : BaseEntity
    {

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isNoTracking">(默认不跟踪实体状态)使用NoTracking的查询会在性能方面得到改善</param>
        /// <returns></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> exp = null, bool isNoTracking = true);

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="isNoTracking">(默认不跟踪实体状态)使用NoTracking的查询会在性能方面得到改善</param>
        /// <param name="values"></param>
        /// <returns></returns>
        IQueryable<T> Find(string whereLambda = null, bool isNoTracking = true, params object[] values);


        IQueryable<TEntity> OtherTable<TEntity>() where TEntity : BaseEntity;
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        bool IsExist(Expression<Func<T, bool>> exp);
        /// <summary>
        /// 查找单个(如果没找到则返回为NULL)
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="isNoTracking">(默认不跟踪实体状态)使用NoTracking的查询会在性能方面得到改善</param>
        /// <returns></returns>
        T FindSingle(Expression<Func<T, bool>> exp, bool isNoTracking = true);

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageindex.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="total">总条数</param>
        /// <param name="exp">条件谓词</param>
        /// <param name="orderBy">排序，格式如："Id"/"Id descending"</param>
        IQueryable<T> Find(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> exp = null, string orderBy = "");

        /// <summary>
        /// 得到分页记录
        /// </summary>
        /// <param name="pageIndex">The pageindex.</param>
        /// <param name="pageSize">The pagesize.</param>
        /// <param name="total">总条数</param>
        /// <param name="whereLambda">条件谓词</param>
        /// <param name="orderBy">排序，格式如："Id"/"Id descending"</param>
        IQueryable<T> Find(int pageIndex, int pageSize, out int total, string whereLambda = "", string orderBy = "", params object[] values);

        /// <summary>
        /// 根据过滤条件获取记录数
        /// </summary>
        int GetCount(Expression<Func<T, bool>> exp = null);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        /// <returns></returns>
        int Add(T entity, bool isComit = true);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        int Adds(List<T> entitis, bool isComit = true);
        /// <summary>
        /// 更新实体（会更新实体的所有属性）
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        /// <returns></returns>
        int Update(T entity, bool isComit = true);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="isComit">是否提交（true）</param>
        /// <returns></returns>
        int Delete(T entity, bool isComit = true);


        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update(u =>u.Id==1,u =>new User{Name="ok"});</para>
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="entity">The entity.</param>
        int Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);
        /// <summary>
        /// 批量按条件删除
        /// </summary>
        /// <param name="exp"></param>
        int Delete(Expression<Func<T, bool>> exp);


        /// <summary>
        /// 对数据库执行给定的 DDL/DML 命令。
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params SqlParameter[] parameters);
        /// <summary>
        /// 执行SQL查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DbRawSqlQuery<int> ExecuteSqlQuery(string sql);

        /// <summary>
        /// 执行原始的sql查询
        /// </summary>
        /// <typeparam name="TElement">返回的泛型类型</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        IList<TElement> SqlQuery<TElement>(string sql, params SqlParameter[] parameters);

        /// <summary>
        /// 开启一个事务
        /// </summary>
        /// <param name="fun"></param>
        bool BeginTransaction(Func<bool> fun);

        /// <summary>
        /// 执行SQL语句或存储过程
        /// 返回Datatable数据集
        /// </summary>
        /// <param name="sql">SQL语句或存储过程 例如：exec usp_procedure</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        DataTable SqlQueryForDataTable(string sql, DbParameter[] parameters);

        /// <summary>
        /// 返回Datatable数据集
        /// </summary>
        /// <param name="proName">存储过程名</param>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        [Obsolete("此方法已过时,请改用SqlQueryForDataTable")]
        DataTable ExecuteForDataTable(string proName, IDataParameter[] parameters);
    }
}