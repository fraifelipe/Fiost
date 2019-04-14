﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NHibernate;

namespace AccountService.Infrastructure.Persistence
{
    public interface IUnitOfWork
    {
        void Save<TEntity>(TEntity entity) where TEntity : class;
        void Update(object entity);
        void SaveOrUpdate(object entity);
        void Delete(object entity);
        void Flush();
        T GetById<T>(Guid id);
        IQueryable<T> Query<T>();
        bool Contains(object entity);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        private readonly ISession _session;

        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
            _session = NHibernateHelper.CreateSessionFactory(_configuration.GetSection("ConnectionStrings").Value).OpenSession();
            _session.BeginTransaction();
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class
        {
            _session.Save(entity);
            _session.Transaction.Commit();
        }

        public void Update(object entity)
        {
            _session.Update(entity);
            _session.Transaction.Commit();
        }

        public void SaveOrUpdate(object entity)
        {
            _session.SaveOrUpdate(entity);
            _session.Transaction.Commit();
        }

        public void Delete(object entity)
        {
            _session.Delete(entity);
            _session.Transaction.Commit();
        }

        public void Flush()
        {
            _session.Flush();
        }

        public T GetById<T>(Guid id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        public bool Contains(object entity)
        {
            return _session.Contains(entity);
        }
    }
}