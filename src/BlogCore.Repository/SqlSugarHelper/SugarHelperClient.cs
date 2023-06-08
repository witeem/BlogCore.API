﻿// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Core.CommonHelper.CommonDto;
using SqlSugar;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using witeem.CoreHelper.ExtensionTools.CommonTools;

public class SugarHelperClient<TEntity> : ISugarHelperClient<TEntity> where TEntity : class, new()
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SqlSugarScope _sqlSugar;
    private ISqlSugarClient Sqldb => _sqlSugar.GetConnectionScopeWithAttr<TEntity>();

    public SugarHelperClient(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _sqlSugar = _unitOfWork.DbClient();
    }

    #region 新增操作

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>受影响行数</returns>
    public async Task<int> AddAsync(TEntity entity)
    {
        var insert = Sqldb.Insertable(entity);
        return await insert.ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量新增实体
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <returns>受影响行数</returns>
    public async Task<int> AddAsync(List<TEntity> entitys)
    {
        return await Sqldb.Insertable(entitys.ToArray()).ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="keyValues">键：字段名称，值：字段值</param>
    /// <returns>受影响行数</returns>
    public async Task<int> AddAsync(Dictionary<string, object> keyValues)
    {
        var result = await Sqldb.Insertable(keyValues).ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>返回当前实体</returns>
    public async Task<TEntity> AddReturnEntityAsync(TEntity entity)
    {
        var result = await Sqldb.Insertable(entity).ExecuteReturnEntityAsync();
        return result;
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>自增ID</returns>
    public async Task<int> AddReturnIdentityAsync(TEntity entity)
    {
        var result = await Sqldb.Insertable(entity).ExecuteReturnIdentityAsync();
        return result;
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>成功或失败</returns>
    public async Task<bool> AddReturnBoolAsync(TEntity entity)
    {
        var result = await Sqldb.Insertable(entity).ExecuteCommandAsync() > 0;
        return result;
    }

    /// <summary>
    /// 批量新增实体
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <returns>成功或失败</returns>
    public async Task<bool> AddReturnBoolAsync(List<TEntity> entitys)
    {
        var result = await Sqldb.Insertable(entitys).ExecuteCommandAsync() > 0;
        return result;
    }

    #endregion

    #region 更新操作

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="lstIgnoreColumns">忽略列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateAsync(TEntity entity, List<string> lstIgnoreColumns = null, bool isLock = true)
    {
        IUpdateable<TEntity> up = Sqldb.Updateable(entity);
        if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
        {
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <param name="lstIgnoreColumns">忽略列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateAsync(List<TEntity> entitys, List<string> lstIgnoreColumns = null,
        bool isLock = true)
    {
        IUpdateable<TEntity> up = Sqldb.Updateable(entitys);
        if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
        {
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="where">条件表达式</param>
    /// <param name="lstIgnoreColumns">忽略列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateIgnoreColumnsAsync(TEntity entity, Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, object>> lstIgnoreColumns = null, bool isLock = true)
    {
        IUpdateable<TEntity> up = Sqldb.Updateable(entity);
        if (lstIgnoreColumns != null)
        {
            up = up.IgnoreColumns(lstIgnoreColumns);
        }

        up = up.Where(where);
        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="where">条件表达式</param>
    /// <param name="columns">更新列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, object>> columns, bool isLock = true)
    {
        IUpdateable<TEntity> up = Sqldb.Updateable(entity);
        up = up.Where(where);
        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        if (columns != null)
        {
            up.UpdateColumns(columns);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 批量更新实体列
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <param name="updateColumns">要更新的列</param>
    /// <param name="wherecolumns">条件列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateColumnsAsync(TEntity entity,
        Expression<Func<TEntity, object>> updateColumns, Expression<Func<TEntity, object>> wherecolumns = null,
        bool isLock = true)
    {
        IUpdateable<TEntity> up = Sqldb.Updateable(entity).UpdateColumns(updateColumns);
        if (wherecolumns != null)
        {
            up = up.WhereColumns(wherecolumns);
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }
    
    /// <summary>
    /// 批量更新实体列
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <param name="updateColumns">要更新的列</param>
    /// <param name="wherecolumns">条件列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateColumnsAsync(List<TEntity> entitys,
        Expression<Func<TEntity, object>> updateColumns, Expression<Func<TEntity, object>> wherecolumns = null,
        bool isLock = true)
    {
        IUpdateable<TEntity> up = Sqldb.Updateable(entitys).UpdateColumns(updateColumns);
        if (wherecolumns != null)
        {
            up = up.WhereColumns(wherecolumns);
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="lstIgnoreColumns">忽略列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateRowVerAsync(TEntity entity, List<string> lstIgnoreColumns = null,
        bool isLock = true)
    {
        Type ts = entity.GetType();
        var rowVerProperty = ts.GetProperty("RowVer");
        if (rowVerProperty == null)
        {
            throw new Exception("Column RowVer Not Exist");
        }

        if (rowVerProperty.GetValue(entity, null) == null)
        {
            throw new Exception("RowVer Value Is Null");
        }

        var codeProperty = ts.GetProperty("Code");
        if (codeProperty == null)
        {
            throw new Exception("Column Code Not Exist");
        }

        if (codeProperty.GetValue(entity, null) == null)
        {
            throw new Exception("Code Value Is Null");
        }

        var rowVerValue = int.Parse(rowVerProperty.GetValue(entity, null).ToString());
        var codeValue = codeProperty.GetValue(entity, null).ToString();
        var sqlWhere = $" RowVer={rowVerValue} AND Code='{codeValue}'";
        rowVerProperty.SetValue(entity, rowVerValue + 1, null);
        IUpdateable<TEntity> up = Sqldb.Updateable(entity);
        if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
        {
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
        }

        up = up.Where(sqlWhere);
        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="update">实体对象</param>
    /// <param name="where">键:字段名称 值:值</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> UpdateRowVerAsync(Expression<Func<TEntity, TEntity>> update,
        Dictionary<string, object> where, bool isLock = true)
    {
        if (!where.ContainsKey("RowVer"))
        {
            throw new Exception("Column RowVer Not Exist");
        }

        if (where["RowVer"] == null)
        {
            throw new Exception("RowVer Value Is Null");
        }

        if (update.Body.ToString().IndexOf("RowVer", StringComparison.Ordinal) == -1)
        {
            throw new Exception("Column RowVer Update Is Null");
        }

        var sqlWhere = "";
        foreach (var item in where)
        {
            sqlWhere += string.IsNullOrWhiteSpace(sqlWhere)
                ? $" {item.Key}='{item.Value}'"
                : $" and {item.Key}='{item.Value}'";
        }

        IUpdateable<TEntity> up = Sqldb.Updateable<TEntity>().SetColumns(update).Where(sqlWhere);
        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    #endregion

    #region 删除操作

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id">主键ID</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<bool> DeleteByPrimaryAsync(object id, bool isLock = true)
    {
        //return await Sqldb.Deleteable<TEntity>(id).ExecuteCommandHasChangeAsync();

        var del = Sqldb.Deleteable<TEntity>(id);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync() > 0;
    }

    /// <summary>
    /// 批量删除实体
    /// </summary>
    /// <param name="primaryKeyValues">主键ID集合</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> DeleteByPrimaryAsync(List<object> primaryKeyValues, bool isLock = true)
    {
        var del = Sqldb.Deleteable<TEntity>().In(primaryKeyValues);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> DeleteAsync(TEntity entity, bool isLock = true)
    {
        var del = Sqldb.Deleteable(entity);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    /// <summary>
    /// 批量删除实体
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> DeleteAsync(List<TEntity> entitys, bool isLock = true)
    {
        var del = Sqldb.Deleteable(entitys);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereLambda, bool isLock = true)
    {
        var del = Sqldb.Deleteable<TEntity>().Where(whereLambda);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="inValues">主键集合</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public async Task<int> DeleteInAsync(List<dynamic> inValues, bool isLock = true)
    {
        var del = Sqldb.Deleteable<TEntity>().In(inValues);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    #endregion

    #region 单表查询

    /// <summary>
    /// 查询单个
    /// </summary>
    /// <param name="expression">返回表达式</param>
    /// <param name="whereLambda">条件表达式</param>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <returns>自定义数据</returns>
    public async Task<TResult> QueryAsync<TResult>(Expression<Func<TEntity, TResult>> expression,
        Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await Sqldb.Queryable<TEntity>().WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda).Select(expression)
            .FirstAsync();
    }

    /// <summary>
    /// 实体列表
    /// </summary>
    /// <param name="expression">返回表达式</param>
    /// <param name="whereLambda">条件表达式</param>
    /// <typeparam name="TResult">返回对象</typeparam>
    /// <returns>自定义数据</returns>
    public async Task<List<TResult>> QueryListExpAsync<TResult>(Expression<Func<TEntity, TResult>> expression,
        Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await Sqldb.Queryable<TEntity>().WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda).Select(expression)
            .ToListAsync();
    }

    /// <summary>
    /// 查询单个
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <returns>实体对象</returns>
    public async Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await Sqldb.Queryable<TEntity>().FirstAsync(whereLambda);
    }

    /// <summary>
    /// 实体列表
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <param name="orderFileds"></param>
    /// <param name="orderByType"></param>
    /// <returns>实体列表</returns>
    public ISugarQueryable<TEntity> QueryableAsync(Expression<Func<TEntity, bool>> whereLambda,
        Expression<Func<TEntity, object>> orderFileds = null, OrderByType orderByType = OrderByType.Desc)
    {
        return Sqldb.Queryable<TEntity>().WhereIF(whereLambda != null, whereLambda)
            .OrderByIF(orderFileds != null, orderFileds, orderByType);
    }

    /// <summary>
    /// 实体列表
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <param name="orderFileds"></param>
    /// <param name="orderByType"></param>
    /// <returns>实体列表</returns>
    public async Task<List<TEntity>> QueryListAsync(Expression<Func<TEntity, bool>> whereLambda,
        Expression<Func<TEntity, object>> orderFileds = null, OrderByType orderByType = OrderByType.Desc)
    {
        var query = Sqldb.Queryable<TEntity>().WhereIF(whereLambda != null, whereLambda)
            .OrderByIF(orderFileds != null, orderFileds, orderByType);
        return await query.ToListAsync();
    }

    /// <summary>
    /// 实体列表
    /// </summary>
    /// <param name="sql">SQL</param>
    /// <returns>实体列表</returns>
    public async Task<List<TEntity>> QuerySqlListAsync(string sql)
    {
        return await Sqldb.SqlQueryable<TEntity>(sql).ToListAsync();
    }

    /// <summary>
    /// 实体列表 分页查询
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <param name="pageRequest">分页对象</param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> QueryPageListAsync(Expression<Func<TEntity, bool>> whereLambda,
        ApiPageRequest pageRequest, Expression<Func<TEntity, TEntity>> expression = null)
    {
        RefAsync<int> totalCount = 0;
        //var list = await Sqldb.Queryable<TEntity>()
        //    .WhereIF(whereLambda != null, whereLambda)
        //    .OrderByIF(!string.IsNullOrEmpty(pageRequest.SortField), pageRequest.SortField)
        //    .ToPageListAsync(pageRequest.Page, pageRequest.Size, totalCount);
        var query = Sqldb.Queryable<TEntity>();
        query = query.WhereIF(whereLambda != null, whereLambda);
        if (expression != null)
        {
            query = query.Select(expression);
        }

        query = query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields));
        var list = await query.ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    /// <summary>
    /// 实体列表
    /// </summary>
    /// <param name="inFieldName">指定字段名</param>
    /// <param name="inValues">值</param>
    /// <returns>实体列表</returns>
    public async Task<List<TEntity>> QueryListInAsync(string inFieldName, List<dynamic> inValues)
    {
        return await Sqldb.Queryable<TEntity>().In(inFieldName, inValues).ToListAsync();
    }

    /// <summary>
    /// 查询单个对象
    /// </summary>
    /// <param name="id">列值</param>
    /// <param name="columnName">列名 默认ID</param>
    /// <returns>实体对象</returns>
    public async Task<TEntity> QuerySingleAsync(object id, string columnName = "id")
    {
        if (id == null)
        {
            throw new Exception("请传入id");
        }

        var conModels = new List<IConditionalModel>
            {
                new ConditionalModel
                {
                    FieldName = columnName, ConditionalType = ConditionalType.Equal, FieldValue = id.ToString()
                },
                new ConditionalModel
                {
                    FieldName = "is_deleted", ConditionalType = ConditionalType.Equal, FieldValue = "0"
                }
            };
        return await Sqldb.Queryable<TEntity>().Where(conModels).SingleAsync();
        // 这种方式不适合软删除模式
        // return await Sqldb.Queryable<TEntity>().InSingleAsync(id);
    }

    /// <summary>
    /// 实体列表
    /// </summary>
    /// <param name="values">列值集合</param>
    /// <param name="columnName">列名 默认ID</param>
    /// <returns>实体列表</returns>
    public async Task<List<TEntity>> QueryListInAsync(List<long> values, string columnName = "id")
    {
        var conModels = new List<IConditionalModel>
            {
                new ConditionalModel
                {
                    FieldName = columnName, ConditionalType = ConditionalType.In, FieldValue = string.Join(",", values)
                },
                new ConditionalModel
                {
                    FieldName = "is_deleted", ConditionalType = ConditionalType.Equal, FieldValue = "0"
                }
            };
        return await Sqldb.Queryable<TEntity>().Where(conModels).ToListAsync();
        //return await Sqldb.Queryable<TEntity>().In(values).ToListAsync();
    }

    /// <summary>
    /// DataTable数据源
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <returns>DataTable</returns>
    public async Task<DataTable> QueryDataTableAsync(Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await Sqldb.Queryable<TEntity>().WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda).ToDataTableAsync();
    }

    /// <summary>
    /// DataTable数据源
    /// </summary>
    /// <param name="sql">SQL</param>
    /// <returns>DataTable</returns>
    public async Task<DataTable> QueryDataTableAsync(string sql)
    {
        return await Sqldb.Ado.GetDataTableAsync(sql);
    }

    /// <summary>
    /// Object
    /// </summary> 
    /// <param name="sql">SQL</param> 
    /// <returns>Object</returns>
    public async Task<object> QuerySqlScalarAsync(string sql)
    {
        return await Sqldb.Ado.GetScalarAsync(sql);
    }

    /// <summary>
    /// 查询单个对象
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <returns>对象.json</returns>
    public async Task<string> QueryJsonAsync(Expression<Func<TEntity, bool>> whereLambda = null)
    {
        ISugarQueryable<TEntity> up = Sqldb.Queryable<TEntity>();
        if (whereLambda != null)
        {
            up = up.Where(whereLambda);
        }

        return await up.ToJsonAsync();
    }

    #endregion

    #region 多表联查，最大支持16个表

    public async Task<List<TResult>> QueryMuchAsync<T, T2, TResult>(
        Expression<Func<T, T2, object[]>> joinExpression, Expression<Func<T, T2, TResult>> selectExpression,
        Expression<Func<T, T2, bool>> whereLambda = null, Expression<Func<T, T2, object>> groupExpression = null,
        string sortField = "")
    {
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        if (!sortField.IsNullOrEmpty())
        {
            query = query.OrderBy(sortField);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, TResult>(
        Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression,
        Expression<Func<T, T2, T3, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, TResult>(
        Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, TResult>(
        Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, TResult>(
        Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, TResult>(
        Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> groupExpression = null)
    {
        var query = Sqldb.Queryable(joinExpression);
        if (!groupExpression.IsNullOrEmpty())
        {
            query = query.GroupBy(groupExpression);
        }

        return await query.WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression)
            .ToListAsync();
    }

    #endregion

    #region 多表联查分页 最大支持16个表

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, TResult>(ApiPageRequest pageRequest,
        Expression<Func<T, T2, object[]>> joinExpression, Expression<Func<T, T2, TResult>> selectExpression,
        Expression<Func<T, T2, bool>> whereLambda = null, Expression<Func<T, T2, object>> groupExpression = null)
        where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, TResult>(ApiPageRequest pageRequest,
        Expression<Func<T, T2, T3, object[]>> joinExpression,
        Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, object>> groupExpression = null) where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, TResult>(ApiPageRequest pageRequest,
        Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, object>> groupExpression = null) where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, TResult>(ApiPageRequest pageRequest,
        Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, object>> groupExpression = null) where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, TResult>(ApiPageRequest pageRequest,
        Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, object>> groupExpression = null) where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, T7, TResult>(ApiPageRequest pageRequest,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> groupExpression = null) where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
        ApiPageRequest pageRequest, Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null,
        Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> groupExpression = null) where T : class, new()
    {
        RefAsync<int> totalCount = 0;
        var query = Sqldb.Queryable(joinExpression);
        if (groupExpression != null)
        {
            query = query.GroupBy(groupExpression);
        }

        var list = await query.OrderByIF(pageRequest.SortFields.Count > 0, string.Join(",", pageRequest.SortFields))
            .WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda)
            .Select(selectExpression).ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return list;
    }

    #endregion

    #region 一对一 一对多查询

    public async Task<List<TEntity>> QueryMapperAsync(Action<TEntity> mapperAction,
        Expression<Func<TEntity, bool>> whereLambda = null)
    {
        ISugarQueryable<TEntity> up = Sqldb.Queryable<TEntity>();
        if (whereLambda != null)
        {
            up = up.Where(whereLambda);
        }

        var datas = await up.Mapper(mapperAction).ToListAsync();
        return datas;
    }

    public async Task<List<TEntity>> QueryMapperPageListAsync(Action<TEntity> mapperAction,
        Expression<Func<TEntity, bool>> whereLambda, ApiPageRequest pageRequest)
    {
        RefAsync<int> totalCount = 0;
        ISugarQueryable<TEntity> up = Sqldb.Queryable<TEntity>();
        if (!StringHelper.IsNullOrEmpty(whereLambda))
        {
            up = up.Where(whereLambda);
        }

        if (pageRequest.SortFields.Count > 0)
        {
            up = up.OrderBy(string.Join(",", pageRequest.SortFields));
        }

        var datas = await up.Mapper(mapperAction)
            .ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return datas;
    }

    public async Task<List<TEntity>> QueryMapperPageListAsync<TObject>(
        Expression<Func<TEntity, List<TObject>>> mapperObject,
        Expression<Func<TEntity, object>> mapperField, Expression<Func<TEntity, bool>> whereLambda,
        ApiPageRequest pageRequest)
    {
        RefAsync<int> totalCount = 0;
        ISugarQueryable<TEntity> up = Sqldb.Queryable<TEntity>();
        if (!StringHelper.IsNullOrEmpty(whereLambda))
        {
            up = up.Where(whereLambda);
        }

        if (pageRequest.SortFields.Count > 0)
        {
            up = up.OrderBy(string.Join("", pageRequest.SortFields));
        }

        var datas = await up.Mapper(mapperObject, mapperField)
            .ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return datas;
    }

    public async Task<List<TEntity>> QueryMapperAsync(Action<TEntity, MapperCache<TEntity>> mapperAction,
        Expression<Func<TEntity, bool>> whereLambda, string sortField = "")
    {
        ISugarQueryable<TEntity> up = Sqldb.Queryable<TEntity>();
        if (!StringHelper.IsNullOrEmpty(whereLambda))
        {
            up = up.Where(whereLambda);
        }

        if (!sortField.IsNullOrEmpty())
        {
            up = up.OrderBy(sortField);
        }

        var datas = await up.Mapper(mapperAction).ToListAsync();
        return datas;
    }

    public async Task<List<TEntity>> QueryMapperPageListAsync(Action<TEntity, MapperCache<TEntity>> mapperAction,
        Expression<Func<TEntity, bool>> whereLambda, ApiPageRequest pageRequest)
    {
        RefAsync<int> totalCount = 0;
        ISugarQueryable<TEntity> up = Sqldb.Queryable<TEntity>();
        if (!StringHelper.IsNullOrEmpty(whereLambda))
        {
            up = up.Where(whereLambda);
        }

        if (pageRequest.SortFields.Count > 0)
        {
            up = up.OrderBy(string.Join(",", pageRequest.SortFields));
        }

        var datas = await up.Mapper(mapperAction)
            .ToPageListAsync(pageRequest.PageIndex, pageRequest.PageSize, totalCount);
        pageRequest.Total = totalCount;
        return datas;
    }

    #endregion

    #region 存储过程

    /// <summary>
    /// 执行存储过程DataSet
    /// </summary>
    /// <param name="procedureName">存储过程名称</param>
    /// <param name="parameters">参数集合</param>
    /// <returns>DataSet</returns>
    public async Task<DataSet> QueryProcedureDataSetAsync(string procedureName, List<SqlParameter> parameters)
    {
        var listParams = ConvetParameter(parameters);
        var datas = await Sqldb.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName, listParams);
        return datas;
    }

    /// <summary>
    /// 执行存储过程DataTable
    /// </summary>
    /// <param name="procedureName">存储过程名称</param>
    /// <param name="parameters">参数集合</param>
    /// <returns>DataTable</returns>
    public async Task<DataTable> QueryProcedureAsync(string procedureName, List<SqlParameter> parameters)
    {
        var listParams = ConvetParameter(parameters);
        var datas = await Sqldb.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, listParams);
        return datas;
    }

    /// <summary>
    /// 执行存储过程Object
    /// </summary>
    /// <param name="procedureName">存储过程名称</param>
    /// <param name="parameters">参数集合</param>
    /// <returns>Object</returns>
    public async Task<object> QueryProcedureScalarAsync(string procedureName, List<SqlParameter> parameters)
    {
        var listParams = ConvetParameter(parameters);
        var datas = await Sqldb.Ado.UseStoredProcedure().GetScalarAsync(procedureName, listParams);
        return datas;
    }

    #endregion

    #region 常用函数

    /// <summary>
    /// 查询前面几条
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <param name="topNum">要多少条</param>
    /// <returns>泛型对象集合</returns>
    public async Task<List<TEntity>> TakeAsync(int topNum, Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await Sqldb.Queryable<TEntity>().WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda).Take(topNum)
            .ToListAsync();
    }

    /// <summary>
    /// 对象是否存在
    /// </summary>
    /// <param name="whereLambda">条件表达式</param>
    /// <returns>True or False</returns>
    public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await Sqldb.Queryable<TEntity>().WhereIF(!StringHelper.IsNullOrEmpty(whereLambda), whereLambda).AnyAsync();
    }

    /// <summary>
    /// 总和
    /// </summary>
    /// <param name="field">字段名</param>
    /// <returns>总和</returns>
    public async Task<int> SumAsync(string field)
    {
        return await Sqldb.Queryable<TEntity>().SumAsync<int>(field);
    }

    /// <summary>
    /// 最大值
    /// </summary>
    /// <param name="field">字段名</param>
    /// <typeparam name="TResult">泛型结果</typeparam>
    /// <returns>最大值</returns>
    public async Task<TResult> MaxAsync<TResult>(string field)
    {
        return await Sqldb.Queryable<TEntity>().MaxAsync<TResult>(field);
    }

    /// <summary>
    /// 最小值
    /// </summary>
    /// <param name="field">字段名</param>
    /// <typeparam name="TResult">泛型结果</typeparam>
    /// <returns>最小值</returns>
    public async Task<TResult> MinAsync<TResult>(string field)
    {
        return await Sqldb.Queryable<TEntity>().MinAsync<TResult>(field);
    }

    /// <summary>
    /// 平均值
    /// </summary>
    /// <param name="field">字段名</param>
    /// <returns>平均值</returns>
    public async Task<int> AvgAsync(string field)
    {
        return await Sqldb.Queryable<TEntity>().AvgAsync<int>(field);
    }

    #endregion

    #region 流水号

    public async Task<string> CustomNumberAsync(string key, string prefix = "", int fixedLength = 4,
        string dateFomart = "")
    {
        var listNumber = await CustomNumberAsync(key, 1, prefix, fixedLength, dateFomart);
        return listNumber[0];
    }

    public async Task<List<string>> CustomNumberAsync(string key, int num, string prefix = "", int fixedLength = 4,
        string dateFomart = "")
    {
        List<string> numbers = new List<string>();
        var dateValue = dateFomart == "" ? "" : DateTime.Now.ToString(dateFomart);
        var fix = prefix.ToUpper() + dateValue;
        var maxValue = await Sqldb.Queryable<TEntity>()
            .Where(key + " LIKE '" + fix + "%' AND LEN(" + key + ")=" + (fix.Length + fixedLength)).Select(key)
            .MaxAsync<string>(key);

        if (maxValue == null)
        {
            for (var i = 0; i < num; i++)
            {
                var tempNumber = fix + (i + 1).ToString().PadLeft(fixedLength, '0');
                numbers.Add(tempNumber);
            }
        }
        else
        {
            if (maxValue.Substring(0, maxValue.Length - fixedLength) == prefix + dateValue)
            {
                var tempLast = maxValue.Substring(maxValue.Length - fixedLength);
                for (var i = 0; i < num; i++)
                {
                    var tempNumber = fix + (int.Parse(tempLast) + i + 1).ToString().PadLeft(fixedLength, '0');
                    numbers.Add(tempNumber);
                }
            }
            else
            {
                for (var i = 0; i < num; i++)
                {
                    var tempNumber = fix + (i + 1).ToString().PadLeft(fixedLength, '0');
                    numbers.Add(tempNumber);
                }
            }
        }

        return numbers;
    }

    #endregion

    #region 参数类型转换

    /// <summary>
    /// SqlParameter转SugarParameter
    /// </summary>
    /// <param name="parameters">????</param>
    /// <returns></returns>
    private List<SugarParameter> ConvetParameter(List<SqlParameter> parameters)
    {
        var listParams = new List<SugarParameter>();
        foreach (var p in parameters)
        {
            var par = new SugarParameter(p.ParameterName, p.Value)
            {
                DbType = p.DbType,
                Direction = p.Direction
            };
            if (!string.IsNullOrWhiteSpace(p.TypeName))
            {
                par.TypeName = p.TypeName;
            }

            listParams.Add(par);
        }

        return listParams;
    }

    #endregion
}