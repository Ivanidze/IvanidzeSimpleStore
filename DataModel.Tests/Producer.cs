using DataModel.Domain;
using DataModel.Repositories;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace DataModel.Tests
{
    [TestFixture]
    public class ProducerRepositoryFixture
    {
        private ISessionFactory _sessionFactory;
        private readonly Producer[] _producers = new[]
                                                        {
                                                            new Producer
                                                                {Caption = "Nokia"},
                                                            new Producer
                                                                {Caption = "Intel"},
                                                            new Producer
                                                                {Caption = "Nvidia"},
                                                            new Producer{Caption = "LG"}
            
                                                        };
        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var producer in _producers)
                {
                    session.Save(producer);
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
        public void CanAddNewProducer()
        {
            var producer = new Producer { Caption = "ATI" };
            IProducerRepository repository = new ProducerRepository();
            repository.Add(producer);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Producer>(producer.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(producer, fromDb);
                Assert.AreEqual(producer.Caption, fromDb.Caption);
               
            }
        }
        [Test]
        public void CanUpdateExistingProducer()
        {
            var producer = _producers[0];
            producer.Caption = "Nokla";
            IProducerRepository repository = new ProducerRepository();
            repository.Update(producer);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Producer>(producer.Id);
                Assert.AreEqual(producer.Caption, fromDb.Caption);
            }
        }
        [Test]
        public void CanDeleteExistingContragent()
        {
            var producer = _producers[0];
            IProducerRepository repository = new ProducerRepository();
            repository.Remove(producer);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Producer>(producer.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetProducerById()
        {

            IProducerRepository repository = new ProducerRepository();
            var fromDb = repository.GetById(_producers[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_producers[1], fromDb);
            Assert.AreEqual(_producers[1].Caption, fromDb.Caption);
        }

        [Test]
        public void CanGetProducerByCaption()
        {
            IProducerRepository repository = new ProducerRepository();
            var fromDb = repository.GetByCaption(_producers[1].Caption);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_producers[1], fromDb);
            Assert.AreEqual(_producers[1].Caption, fromDb.Caption);

        }
    }

   
}
