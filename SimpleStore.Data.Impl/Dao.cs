using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate;
using NHibernate.Linq;
using SimpleStore.Data;
using SimpleStore.Domain;
using uNhAddIns.Entities;

namespace DataModel.DataAccess
{
    public class Dao<T>:IDao<T> where T:Entity
    {
        /// <summary>
        /// Текущая сессия для дао
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        public Dao(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        private IQueryable<T> CurrentLinq
        {
            get { return ContextSession.Linq<T>(); }
        }
        private ISession ContextSession
        {
            get
            {
                return _sessionFactory.GetCurrentSession();
            }
        }
        public T GetById(object id)
        {
            return ContextSession.Get<T>(id);
        }

        public T GetProxy(object id)
        {
            return ContextSession.Load<T>(id);
        }

        public IEnumerable<T> Retrieve(Expression<Func<T, bool>> predicate)
        {
            return CurrentLinq.Where(predicate);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return CurrentLinq.Count(predicate);
        }

        public IEnumerable<T> RetrieveAll()
        {
            return CurrentLinq;
        }

        public void Save(T entity)
        {
            var session = ContextSession;
            using (var transaction = session.BeginTransaction())
            {
                session.Save(entity);
                transaction.Commit();
            }
                
        }

        public void Update(T entity)
        {
            var session = ContextSession;
            using (var transaction = session.BeginTransaction())
            {
                session.Update(entity);
                transaction.Commit();
            }
        }

        public void Delete(T entity)
        {
            var session = ContextSession;
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(entity);
                transaction.Commit();
            }
        }

        public void Refresh(T entity)
        {
            ContextSession.Refresh(entity);
        }

        public void Merge(T entity)
        {
            ContextSession.Merge(entity);
        }
    }
}
