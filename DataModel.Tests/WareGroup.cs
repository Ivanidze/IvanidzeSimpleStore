using DataModel.Domain;
using DataModel.Repositories;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace DataModel.Tests
{
    [TestFixture]
    public class WareGroupRepository_Fixture
    {
        private ISessionFactory _sessionFactory;
        private IWareGroupRepository _repository;
        private WareGroup _root, _child1, _child2, _child1_1, _child1_2, _child1_3, _child2_1, _child2_2;



        [SetUp]
        public void SetupContext()
        {

            new SchemaExport(NHibernateHelper.Configurator).Execute(false, true, false);
            _sessionFactory = NHibernateHelper.SessionFactory;

            CreateInitialData();

            _repository = new WareGroupRepository();
        }

        private void CreateInitialData()
        {
            _root = new WareGroup{ Name = "root" };
            _child1 = new WareGroup { Name = "child1" };
            _child2 = new WareGroup { Name = "child2" };
            _child1_1 = new WareGroup { Name = "grand child 1-1" };
            _child1_2 = new WareGroup { Name = "grand child 1-2" };
            _child1_3 = new WareGroup { Name = "grand child 1-3" };
            _child2_1 = new WareGroup { Name = "grand child 2-1" };
            _child2_2 = new WareGroup { Name = "grand child 2-2" };

            _root.AddChild(_child1);
            _root.AddChild(_child2);
            
            _child1.AddChild(_child1_1);
            _child1.AddChild(_child1_2);
            _child1.AddChild(_child1_3);
            _child2.AddChild(_child2_1);
            _child2.AddChild(_child2_2);

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(_root);
                transaction.Commit();
            }
        }
        [Test]
        public void CanAddNewClient()
        {
            var client = new WareGroup { Name = "child new", Parent = _child1_1 };
            _repository.Add(client);
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var fromDb = session.Get<WareGroup>(client.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreNotEqual(client, fromDb);
                Assert.AreEqual(client.Name, fromDb.Name);
                Assert.AreEqual(client.Parent.Name, fromDb.Parent.Name);
            }
        }
        [Test]
        public void CanUpdateExistingWareGroup()
        {
            var wareGroup = _child2_1;
            wareGroup.Name = "Сергей Игнатьевич";
            _repository.Update(wareGroup);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<WareGroup>(wareGroup.Id);
                Assert.AreEqual(wareGroup.Name, fromDb.Name);
            }
        }
        [Test]
        public void CanDeleteExistingWareGroup()
        {
            var wareGroup = _child2_2;
            _repository.Remove(wareGroup);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Client>(wareGroup.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetWareGroupById()
        {
            var id = _root.Id;
            var node = _repository.GetById(id);
            Assert.IsNotNull(node);
            Assert.IsTrue(NHibernateUtil.IsInitialized(node.Parent));
            Assert.IsTrue(NHibernateUtil.IsInitialized(node.Children));
            Assert.IsTrue(NHibernateUtil.IsInitialized(node.Ancestors));
            Assert.IsTrue(NHibernateUtil.IsInitialized(node.Descendants));
            Assert.IsNull(node.Parent);
            Assert.AreEqual(2, node.Children.Count);
            Assert.AreEqual(0, node.Ancestors.Count);
            Assert.AreEqual(7, node.Descendants.Count);
        }

        [Test]
        public void CanGetWareGroupAncestors()
        {
            var id = _child1_3.Id;
            var node = _repository.GetById(id);
            Assert.AreEqual(0,node.Children.Count);
            Assert.AreEqual(2, node.Ancestors.Count);
            Assert.AreEqual(0, node.Descendants.Count);
        }
        [Test]
        public void CanGetWareGroupAll()
        {
          
            var collection = _repository.GetAll();
            Assert.AreEqual(8,collection.Count);
        }
    }
}
