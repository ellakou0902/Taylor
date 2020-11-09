using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Taylor.Core.DapperExtensions
{
    public class DapperRepositoryBase<TEntity, TPrimaryKey> : AbstractRepositoryBase<TEntity, TPrimaryKey>,IDisposable 
       where TEntity : class, IEntity<TPrimaryKey>
    {
        public Guid Guid = Guid.NewGuid();
        protected IDbConnection _dbConnection;

        public DapperRepositoryBase()
        {
            this._dbConnection = CreateDbConnection();
        }

        public IDbConnection CreateDbConnection()
        {
            //TODO: move to config. Hard code here... ... 
            var ip = "192.168.100.111";
            var dbName = "DMS";
            var userName = "sa";
            var password = "sql2019";
            var strConn = $"Data Source={ip};Initial Catalog={dbName};user id={userName};password={password};";

            var conn = new SqlConnection(strConn);
            return conn;
        }
        public override IQueryable<TEntity> GetAll()
        {
            CheckDbConnection();
            OpenDbConnection();

            var entitys = _dbConnection.GetList<TEntity>().AsQueryable();
            CloseDbConnection();
            return entitys;
        }
        public override TEntity Get(TPrimaryKey id)
        {
            //TODO: 后期需要使用Command 进行进一步封装，实现UnitOfWork
            CheckDbConnection();

            OpenDbConnection();
            var entity = _dbConnection.Get<TEntity>(id);
            CloseDbConnection();

            return entity;
        }

        public override TEntity Insert(TEntity entity)
        {
            //TODO: 后期需要使用Command 进行进一步封装，实现UnitOfWork
            CheckDbConnection();

            OpenDbConnection();
            var key = _dbConnection.Insert(entity);
            var realEntity = _dbConnection.Get<TEntity>(key);
            CloseDbConnection();

            return realEntity;
        }

        public override TEntity Update(TEntity entity)
        {
            CheckDbConnection();
            OpenDbConnection();
            _dbConnection.Update(entity);
            var realEntity = _dbConnection.Get<TEntity>(entity.Id);
            CloseDbConnection();

            return realEntity;
        }
        public override void Delete(TEntity entity)
        {
            CheckDbConnection();
            OpenDbConnection();
            var id = entity.Id;
            var entityForDelete = _dbConnection.Get<TEntity>(id);
            if (entityForDelete == null)
            {
                throw new Exception($"[DapperRepositoryBase]:不存在主键为<{id}>的数据!");
            }
            _dbConnection.Delete<TEntity>(id);
            CloseDbConnection();
        }

        public override void Delete(TPrimaryKey id)
        {
            CheckDbConnection();
            OpenDbConnection();
            var entity = _dbConnection.Get<TEntity>(id);
            if (entity == null)
            {
                throw new Exception($"[DapperRepositoryBase]:不存在主键为<{id}>的数据!");
            }
            _dbConnection.Delete<TEntity>(id);
            CloseDbConnection();
        }

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }

        protected void CheckDbConnection()
        {
            if (_dbConnection == null)
            {
                throw new Exception("[DapperRepositoryBase]: 未初始化的 _dbConnection!");
            }
        }

        protected void OpenDbConnection()
        {
            if (_dbConnection.State == ConnectionState.Closed) _dbConnection.Open();
        }
        protected void CloseDbConnection()
        {
            if (_dbConnection.State != ConnectionState.Closed) _dbConnection.Close();
        }

    }

    public class DapperRepositoryBase<TEntity> : DapperRepositoryBase<TEntity, int>
       where TEntity : class, IEntity<int>
    {
        public DapperRepositoryBase() : base() { }

    }
}
