// 创建人：魏天华 
// 测试添加代码文件头

using BlogCore.Application.UserInfo.Dtos;
using BlogCore.Core;
using BlogCore.Core.CommonHelper.CommonDto;
using BlogCore.Domain.Comm.Dto;
using BlogCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using witeem.CoreHelper.Redis;

public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
{
    #region 字段
    /// <summary>
    /// 仓储DAL
    /// </summary>
    protected ISugarHelperClient<TEntity> _baseDal;
    #endregion

    #region 通用方法

    public async Task<bool> AddEntityAsync(TEntity entity)
    {
        entity.InitEntity();
        return await _baseDal.AddReturnBoolAsync(entity);
    }

    public async Task<bool> AddEntityListAsync(List<TEntity> entityList)
    {
        entityList.ForEach(u => { u.InitEntity(); });
        return await _baseDal.AddReturnBoolAsync(entityList);
    }

    public async Task<bool> UpdateEntityAsync(TEntity entity, List<string> lstIgnoreColumns = null,
        bool isLock = true)
    {
        entity.EditEntity();
        return await _baseDal.UpdateAsync(entity, lstIgnoreColumns, isLock) > 0;
    }

    public async Task<bool> UpdateEntityListAsync(List<TEntity> entityList)
    {
        entityList.ForEach(u => { u.EditEntity(); });
        return await _baseDal.UpdateAsync(entityList) > 0;
    }


    public async Task<bool> DeleteEntityAsync(TEntity entity)
    {
        entity.DelEntity();
        return await _baseDal.UpdateAsync(entity) > 0;
    }

    public async Task<bool> DeleteEntityListAsync(List<TEntity> entityList)
    {
        entityList.ForEach(u => { u.DelEntity(); });
        return await _baseDal.UpdateAsync(entityList) > 0;
    }

    public async Task<TEntity> QuerySingleAsync(object objId, string columnName = "id")
    {
        return await _baseDal.QuerySingleAsync(objId, columnName);
    }


    public async Task<List<TEntity>> QueryByIdsAsync(List<long> ids, string columnName = "id")
    {
        return await _baseDal.QueryListInAsync(ids, columnName);
    }

    public async Task<List<TEntity>> QueryByIdsAsync(HashSet<long> ids, string columnName = "id")
    {
        return await QueryByIdsAsync(ids.ToList(), columnName);
    }

    public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> whereLambda)
    {
        return await _baseDal.IsExistAsync(whereLambda);
    }

    public async Task<TEntity> QueryFirstAsync(Expression<Func<TEntity, bool>> whereLambda = null)
    {
        return await _baseDal.QueryFirstAsync(whereLambda);
    }

    #endregion
}
