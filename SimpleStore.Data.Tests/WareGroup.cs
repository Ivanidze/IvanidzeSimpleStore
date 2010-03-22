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
    public class WareGroupDataTest : PersistenceFixtureBase
    {
        
        private IDao<WareGroup>_repository;
        private WareGroup _root, _child1, _child2, _child1_1, _child1_2, _child1_3, _child2_1, _child2_2;

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

            using (var session = sessions.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(_root);
                session.Save(_child1);
                session.Save(_child2);
                session.Save(_child1_1);
                session.Save(_child1_2);
                session.Save(_child1_3);
                session.Save(_child2_1);
                session.Save(_child2_2);
                transaction.Commit();
                
            }
            _repository = new Dao<WareGroup>(sessions);
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
        public void CanAddNewClient()
        {
            var client = new WareGroup { Name = "child new", Parent = _child1_1 };
            _repository.Save(client);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<WareGroup>(client.Id);
                Assert.IsNotNull(fromDb);
                Assert.AreEqual(client, fromDb);
                
                Assert.AreEqual(client.Parent.Name, fromDb.Parent.Name);
            }
        }
        [Test]
        public void CanUpdateExistingWareGroup()
        {
            var wareGroup = _child2_1;
            wareGroup.Name = "Сергей Игнатьевич";
            _repository.Update(wareGroup);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<WareGroup>(wareGroup.Id);
                Assert.AreEqual(wareGroup.Name, fromDb.Name);
            }
        }
        [Test]
        public void CanDeleteExistingWareGroup()
        {
            var wareGroup = _child2_2;
            _repository.Delete(wareGroup);
            using (ISession session = sessions.OpenSession())
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
            Assert.Contains(_child1,node.Children.ToList());
            Assert.Contains(_child2, node.Children.ToList());
            Assert.IsNull(node.Parent);
            
        }
        [Test]
        public void CanGetWareGroupAll()
        {
          
            var collection = _repository.RetrieveAll();
            Assert.AreEqual(8,collection.Count());
        }
    }
}
