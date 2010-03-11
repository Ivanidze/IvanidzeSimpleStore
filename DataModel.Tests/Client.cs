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
    public class ClientRepository_Fixture
    {
        private ISessionFactory _sessionFactory;


        private readonly Client[] _clients = new[]
                                                        {
                                                            new Client
                                                                {FIO = "Сергей Бояринцев", ContactPhone = "33-22-33",Identification = "Паспорт 312332432"},
                                                            new Client
                                                                {FIO = "Василий Петров", ContactPhone = "32-23-65",Identification =  "Водительское удостоверение 22451"},
                                                            new Client
                                                                {FIO = "Андрей Иванов", ContactPhone = "54-42-65",Identification = "Читательский билет 42423"},
                                                            new Client
                                                                {FIO = "Сергей Бездарнов",ContactPhone = "33-65-23",Identification = "Лакмусовая бумажка"}
                                                        };
        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var worker in _clients)
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
        public void CanAddNewClient()
        {
            var client = new Client { FIO = "Саня Лохматый", ContactPhone = "5433534" ,Identification = "Паспорт 124432"};
            IClientRepository repository = new ClientRepository();
            repository.Add(client);
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var fromDb = session.Get<Client>(client.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreNotEqual(client, fromDb);
                Assert.AreEqual(client.FIO, fromDb.FIO);
                Assert.AreEqual(client.Identification,fromDb.Identification);
            }
        }
        [Test]
        public void CanUpdateExistingClient()
        {
            var client = _clients[0];
            client.FIO = "Сергей Игнатьевич";
            IClientRepository repository = new ClientRepository();
            repository.Update(client);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Client>(client.Id);
                Assert.AreEqual(client.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void CanDeleteExistingClient()
        {
            var client = _clients[0];
            IClientRepository repository = new ClientRepository();
            repository.Remove(client);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Client>(client.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetClientById()
        {
            IClientRepository repository = new ClientRepository();
            var fromDb = repository.GetById(_clients[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_clients[1], fromDb);
            Assert.AreEqual(_clients[1].FIO, fromDb.FIO);
            Assert.AreEqual(_clients[1].Identification, fromDb.Identification);

        }
    }
}
