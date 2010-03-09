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
    public class ModelRepository_Fixture
    {
        private ISessionFactory _sessionFactory;

        private Model _model1, _model2;

        private WareGroup _root, _child1, _child2, _child1_1, _child1_2,_root2;

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
            _model1 = new Model {Name = "N78", WareGroup = _child1_2};
            _model2 = new Model {Name = "T2100", WareGroup = _child2};
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
        public void CanAddNewModel()
        {
            var model = new Model { Name = "N76",WareGroup = _child1_1 };
            IModelRepository repository = new ModelRepository();
            repository.Add(model);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Model>(model.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(model, fromDb);
                Assert.AreEqual(model.Name, fromDb.Name);
                Assert.AreEqual(model.WareGroup.Name,fromDb.WareGroup.Name);
            }

        }
        [Test]
        public void CanUpdateModel()
        {
            var model = _model1;
            IModelRepository repository = new ModelRepository();
            model.Name = "N79";
            repository.Update(model);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Model>(model.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreEqual(model.Name, model.Name);

            }
        }
        [Test]
        public void CanDeleteModel()
        {
            var model = _model2;
            IModelRepository repository = new ModelRepository();
            repository.Remove(model);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Model>(model.Id);
                Assert.IsNull(fromDb);
            }
        }
    }

  
}
