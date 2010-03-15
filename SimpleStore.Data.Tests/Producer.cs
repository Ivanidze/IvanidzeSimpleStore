
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
    public class ProducerDataTesting:PersistenceFixtureBase
    {
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

        private IDao<Producer> _repository;
        private void CreateInitialData()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var producer in _producers)
                {
                    session.Save(producer);
                }
                transaction.Commit();

            }
            _repository = new Dao<Producer>(sessions);
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
        public void CanAddNewProducer()
        {
            var producer = new Producer { Caption = "ATI" };
            _repository.Save(producer);
            using (ISession session = sessions.OpenSession())
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
            _repository.Update(producer);

            using (ISession session = sessions.OpenSession())
            {
                
                var fromDb = session.Get<Producer>(producer.Id);
                Assert.AreEqual(producer.Caption, fromDb.Caption);
            }
        }
        [Test]
        public void CanDeleteExistingContragent()
        {
            var producer = _producers[0];
            _repository.Delete(producer);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Producer>(producer.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetProducerById()
        {
            var fromDb = _repository.GetById(_producers[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_producers[1], fromDb);
            Assert.AreEqual(_producers[1].Caption, fromDb.Caption);
        }

        [Test]
        public void CanGetProducerByCaption()
        {
            var fromDb = _repository.Retrieve((producer)=>(producer.Caption==_producers[1].Caption));
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_producers[1], fromDb);
            Assert.AreEqual(_producers[1].Caption, fromDb.First().Caption);

        }
        [Test]
        public void CanGetProducerAll()
        {
            var collection = _repository.RetrieveAll();
            Assert.AreEqual(_producers.Length,collection.Count());
        }
    }

   
}
