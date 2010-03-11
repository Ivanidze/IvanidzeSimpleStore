using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using DataModel.Repositories;
using DataModel.Domain;
namespace DataModel.Tests
{
    [TestFixture]
    public class WareTypeRepository_Fixture
    {
        private ISessionFactory _sessionFactory;

        private WareType _model1, _model2;

        private WareGroup _root, _child1, _child2, _child1_1, _child1_2,_root2;
        private Producer _producer, _producer2;
        [SetUp]
        public void SetupContext()
        {
            new SchemaExport(NHibernateHelper.Configurator).Execute(false, true, false);
            _sessionFactory = NHibernateHelper.SessionFactory;
            CreateInitialData();
        }
        private void CreateInitialData()
        {
            _root = new WareGroup {Name = "Электронные товары"};
            _child1 = new WareGroup {Name = "Мобильные телефоны"};
            _child1_1 = new WareGroup {Name = "Раскладушки"};
            _child1_2 = new WareGroup {Name = "Моноблоки"};
            _child2 = new WareGroup {Name = "Процессоры"};
            _root.AddChild(_child1);
            _root.AddChild(_child2);
            _child1.AddChild(_child1_1);
            _child1.AddChild(_child1_2);
            _root2 = new WareGroup{Name = "Антиквариат"};
            _producer = new Producer {Caption = "Nokia"};
            _producer2 = new Producer {Caption = "Intel"};
            _model1 = new WareType {Name = "N78", WareGroup = _child1_2,Producer = _producer};
            _model2 = new WareType { Name = "T2100", WareGroup = _child2,Producer = _producer2};
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(_root);
                    transaction.Commit();
                }
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(_root2);
                    transaction.Commit();
                }
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(_producer);
                    session.Save(_producer2);
                    transaction.Commit();
                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(_model1);
                    transaction.Commit();

                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(_model2);
                    transaction.Commit();

                }
            }


        }
        [Test]
        public void CanAddNewWareType()
        {
            var wareType = new WareType { Name = "N76",WareGroup = _child1_1 ,Producer = _producer2};
            IWareTypeRepository repository = new WareTypeRepository();
            repository.Add(wareType);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<WareType>(wareType.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(wareType, fromDb);
                Assert.AreEqual(wareType.Name, fromDb.Name);
                Assert.AreEqual(wareType.WareGroup.Name,fromDb.WareGroup.Name);
                Assert.AreEqual(wareType.Producer.Caption,fromDb.Producer.Caption);
            }

        }
        [Test]
        public void CanUpdateModel()
        {
            var wareType = _model1;
            IWareTypeRepository repository = new WareTypeRepository();
            wareType.Name = "N79";
            repository.Update(wareType);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<WareType>(wareType.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreEqual(wareType.Name, wareType.Name);

            }
        }
        [Test]
        public void CanDeleteModel()
        {
            var wareType = _model2;
            IWareTypeRepository repository = new WareTypeRepository();
            repository.Remove(wareType);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<WareType>(wareType.Id);
                Assert.IsNull(fromDb);
            }
        }
    }

  
}
