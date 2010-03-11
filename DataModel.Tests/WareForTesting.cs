using DataModel.Domain;
using DataModel.Repositories;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace DataModel.Tests
{
    [TestFixture]
    public class WareForTestingRepository_Fixture
    {
        private ISessionFactory _sessionFactory;
        private Producer _producer;
        private WareGroup _wareGroup;
        private  WareType _wareType, _wareType2;
        private  Client _client,_client2;
        private  Worker _worker, _worker2;
        private  WareForTesting _ware,_ware2;
        private IWareForTestingRepository _repository;
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
            _ware = new WareForTesting
                        {
                            WareType = _wareType,
                            ClientBrought = _client,
                            Description = "Потертый телефон",
                            Worker = _worker,
                            Priority = 4
                        };
            _ware2 = new WareForTesting
                         {
                             ClientBrought = _client2,
                             Description = "Нет крышки",
                             WareType = _wareType2,
                             Worker = _worker2,
                             Priority = 3
                         };
            using (ISession session = _sessionFactory.OpenSession())
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
            _repository = new WareForTestingRepository(); 
        }

        [SetUp]
        public void SetupContext()
        {
            new SchemaExport(NHibernateHelper.Configurator).Execute(false, true, false);
            _sessionFactory = NHibernateHelper.SessionFactory;
            CreateInitialData();
        }
        [Test]
        public void CanAddNewWareForTesting()
        {
            var ware = new WareForTesting { Description = "Отличное состояние", WareType = _wareType2,ClientBrought = _client,Worker = _worker2,Priority = 1};
            _repository.Add(ware);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Ware>(ware.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(ware, fromDb);
                Assert.AreEqual(ware.WareType.Name, fromDb.WareType.Name);
                Assert.AreEqual(ware.Description, fromDb.Description);
            }
        }
        [Test]
        public void CanUpdateExistingWareForTesting()
        {
            var ware = _ware;
            ware.Description = "Отличный телефон";
            _repository.Update(ware);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<WareForTesting>(ware.Id);
                Assert.AreEqual(ware.Description, fromDb.Description);
                Assert.AreEqual(ware.Priority,fromDb.Priority);
            }
        }
        [Test]
        public void CanDeleteExistingWareForTesting()
        {
            var ware = _ware2;
            _repository.Remove(ware);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<WareForTesting>(ware.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetWareForTestingById()
        {
            var fromDb = _repository.GetById(_ware.Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_ware, fromDb);
            Assert.AreEqual(_ware.Description, fromDb.Description);
            Assert.AreEqual(_ware.Priority,fromDb.Priority);
        }
        [Test]
        public void CanGetWareForTestingAll()
        {
            var collection = _repository.GetAll();
            Assert.AreEqual(2,collection.Count);
        }
    }
}
