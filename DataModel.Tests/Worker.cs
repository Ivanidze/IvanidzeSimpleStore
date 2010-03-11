using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using DataModel.Repositories;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace DataModel.Tests
{
    [TestFixture]
    public class WorkerRepository_Fixture
    {
        private ISessionFactory _sessionFactory;


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
        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var worker in _workers)
                {
                    session.Save(worker);
                }
                transaction.Commit();

            }
        }

        [SetUp]
        public void SetupContext()
        {
            new SchemaExport(NHibernateHelper.Configurator).Execute(false, true, false);
            _sessionFactory = NHibernateHelper.SessionFactory;
            CreateInitialData();

        }
        [Test]
        public void CanAddNewWorker()
        {
            var worker = new Worker {FIO = "Саня Лохматый", ContactPhone = "5433534"};
            IWorkerRepository repository = new WorkerRepository();
            repository.Add(worker);
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var fromDb = session.Get<Worker>(worker.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreNotEqual(worker,fromDb);
                Assert.AreEqual(worker.FIO,fromDb.FIO);
            }
        }
        [Test]
        public void CanUpdateExistingWorker()
        {
            var worker = _workers[0];
            worker.FIO = "Сергей Игнатьевич";
            IWorkerRepository repository = new WorkerRepository();
            repository.Update(worker);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Worker>(worker.Id);
                Assert.AreEqual(worker.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void CanDeleteExistingWorker()
        {
            var worker = _workers[0];
            IWorkerRepository repository = new WorkerRepository();
            repository.Remove(worker);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Worker>(worker.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetWorkerById()
        {

            IWorkerRepository repository = new WorkerRepository();
            var fromDb = repository.GetById(_workers[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_workers[1], fromDb);
            Assert.AreEqual(_workers[1].FIO, fromDb.FIO);
        }

    }
}
