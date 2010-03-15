using System.Linq;
using DataModel.DataAccess;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SimpleStore.Domain;

namespace SimpleStore.Data.Tests
{
    [TestFixture]
    public class WorkerDataTest : PersistenceFixtureBase
    {


        private readonly Worker[] _workers = new[]
                                                        {
                                                            new Worker
                                                                {FIO = "Сергей Бояринцев", ContactPhone = "33-22-33"},
                                                            new Worker
                                                                {FIO = "Василий Петров", ContactPhone = "32-23-65"},
                                                            new Worker
                                                                {FIO = "Андрей Иванов", ContactPhone = "54-42-65"},
                                                            new Worker{FIO = "Сергей Бездарнов",ContactPhone = "33-65-23"}
            
                                                        };

        private IDao<Worker> _repository;
        private void CreateInitialData()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var worker in _workers)
                {
                    session.Save(worker);
                }
                transaction.Commit();

            }
            _repository = new Dao<Worker>(sessions);
            var repositorysession = sessions.OpenSession();
            CurrentSessionContext.Bind(repositorysession);
        }

        [SetUp]
        public void SetupContext()
        {
            TestFixtureSetUp();
            CreateInitialData();
        }
        [Test]
        public void CanAddNewWorker()
        {
            var worker = new Worker {FIO = "Саня Лохматый", ContactPhone = "5433534"};
            _repository.Save(worker);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Worker>(worker.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreEqual(worker,fromDb);
            }
        }
        [Test]
        public void CanUpdateExistingWorker()
        {
            var worker = _workers[0];
            worker.FIO = "Сергей Игнатьевич";
            _repository.Update(worker);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Worker>(worker.Id);
                Assert.AreEqual(worker.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void CanDeleteExistingWorker()
        {
            var worker = _workers[0];
            _repository.Delete(worker);

            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Worker>(worker.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetWorkerById()
        {
            var fromDb = _repository.GetById(_workers[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_workers[1], fromDb);
            Assert.AreEqual(_workers[1].FIO, fromDb.FIO);
        }
        [Test]
        public void CanGetWorkerAll()
        {
            var collection = _repository.RetrieveAll();
            Assert.AreEqual(4,collection.Count());
        }
    }
}
