using System.Linq;
using DataModel.DataAccess;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using SimpleStore.Domain;

namespace SimpleStore.Data.Tests
{
    [TestFixture]
    public class WareDataTesting:PersistenceFixtureBase
    {
        
        private Producer _producer;
        private WareGroup _wareGroup;
        private  WareType _wareType, _wareType2;
        private  Client _client,_client2;
        private  Worker _worker, _worker2;
        private  Ware _ware,_ware2;
        private IDao<Ware>_repository;
        private void CreateInitialData()
        {
            _wareGroup = new WareGroup {Name = "Мобильный телефон"};
            _producer = new Producer {Caption = "LG"};
            _wareType = new WareType {Name = "i234", Producer = _producer, WareGroup = _wareGroup};
            _wareType2 = new WareType {Name = "4324", Producer = _producer, WareGroup = _wareGroup};
            _client = new Client {FIO = "Розничный клиент",ContactPhone = "нет",Identification = "нет"};
            _client2 = new Client{FIO ="Барыга Леха",ContactPhone = "78-32-43",Identification = "Талон"};
            _worker = new Worker{FIO = "Саша"};
            _worker2 = new Worker {FIO = "Вова"};
            _ware = new Ware
                        {
                            WareType = _wareType,
                            ClientBrought = _client,
                            Description = "Потертый телефон",
                            Worker = _worker
                        };
            _ware2 = new Ware
                         {
                             ClientBrought = _client2,
                             Description = "Нет крышки",
                             WareType = _wareType2,
                             Worker = _worker2
                         };
            using (ISession session = sessions.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {

                session.Save(_wareGroup);
                session.Save(_producer);
                session.Save(_wareType);
                session.Save(_wareType2);
                session.Save(_client);
                session.Save(_client2);
                session.Save(_worker);
                session.Save(_producer); 
                session.Save(_producer);
                session.Save(_worker2);
                session.Save(_ware);
                session.Save(_ware2);
                transaction.Commit();

            }
            _repository = new Dao<Ware>(sessions);
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
        public void CanAddNewWare()
        {
            var ware = new Ware { Description = "Отличное состояние", WareType = _wareType2,ClientBrought = _client,Worker = _worker2};
            _repository.Save(ware);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Ware>(ware.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(ware, fromDb);
                Assert.AreEqual(ware.WareType.Name, fromDb.WareType.Name);
                Assert.AreEqual(ware.Description, fromDb.Description);
            }
        }
        [Test]
        public void CanUpdateExistingWare()
        {
            var ware = _ware;
            ware.Description = "Отличный телефон";
            _repository.Update(ware);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Ware>(ware.Id);
                Assert.AreEqual(ware.Description, fromDb.Description);
            }
        }
        [Test]
        public void CanDeleteExistingWare()
        {
            var ware = _ware2;
            _repository.Delete(ware);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Ware>(ware.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetWareById()
        {

            var fromDb = _repository.GetById(_ware.Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_ware, fromDb);
            Assert.AreEqual(_ware.Description, fromDb.Description);
        }
        [Test]
        public void CanGetWareAll()
        {
            var collection = _repository.RetrieveAll();
            Assert.AreEqual(2,collection.Count());
        }
    }
}
