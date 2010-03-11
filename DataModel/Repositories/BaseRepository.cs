using System.Collections.Generic;
using NHibernate;
using NHibernate.LambdaExtensions;
namespace DataModel.Repositories
{
    public class BaseRepository<T>:IBaseRepository<T>
    {
        public  virtual void Add(T obj)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(obj);
                transaction.Commit();
            }
        }

        public virtual void Update(T obj)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(obj);
                transaction.Commit();
            }
        }

        public virtual void Remove(T obj)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(obj);
                transaction.Commit();
            }
        }

        public virtual T GetById(int id)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public ICollection<T> GetAll()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.CreateCriteria(typeof (T)).List<T>();
            }
        }
    }
}
