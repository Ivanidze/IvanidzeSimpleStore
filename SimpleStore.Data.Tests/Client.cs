using DataModel.DataAccess;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using System.Linq;
using SimpleStore.Domain;

namespace SimpleStore.Data.Tests
{
  [TestFixture]
    public class ClientDataTest : PersistenceFixtureBase
    {

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

        private IDao<Client> _repository;
        private void CreateInitialData()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var worker in _clients)
                {
                    session.Save(worker);
                }
                transaction.Commit();

            }
            _repository = new Dao<Client>(sessions);
            var repositorysession = sessions.OpenSession();
            CurrentSessionContext.Bind(repositorysession);
        }


        [TestFixtureSetUp]
        public void SetupContext()
        {
            TestFixtureSetUp();
            CreateInitialData();
        }
       
        [Test]
        public void CanAddNewClient()
        {
            using (ISession session = sessions.OpenSession())
            {
            var client = new Client { FIO = "Саня Лохматый", ContactPhone = "5433534" ,Identification = "Паспорт 124432"};
            _repository.Save(client);
           
                var fromDb = session.Get<Client>(client.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreEqual(client, fromDb);
                Assert.AreEqual(client.FIO, fromDb.FIO);
                Assert.AreEqual(client.Identification,fromDb.Identification);
            }
        }
        [Test]
        public void CanUpdateExistingClient()
        {
            var client = _clients[0];
            client.FIO = "Сергей Игнатьевич";
            _repository.Update(client);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Client>(client.Id);
                Assert.AreEqual(client, fromDb);
            }
        }
        [Test]
        public void CanDeleteExistingClient()
        {
            using (ISession session = sessions.OpenSession())
            {
            var client = _clients[2];
            _repository.Delete(client);
            
                var fromDb = session.Get<Client>(client.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetClientById()
        {
            var fromDb = _repository.GetById(_clients[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_clients[1], fromDb);
            Assert.AreEqual(_clients[1], fromDb);

        }
        [Test]
        public void CanGetClientAll()
        {
            var collectionCount = _repository.Count((p) =>p.Id!=-1 );
            Assert.AreEqual(_clients.Length, collectionCount);
        }
    }
}
