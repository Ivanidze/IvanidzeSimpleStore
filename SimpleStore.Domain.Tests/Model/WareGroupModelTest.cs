using System.Collections.Generic;
using System.Reflection;
using Infrasturcture;
using Moq;
using NUnit.Framework;
using SimpleStore.Data;
using SimpleStore.Domain.Impl;
using uNhAddIns.Adapters;
using uNhAddIns.Adapters.Common;
namespace SimpleStore.Domain.Tests.Model
{
    [TestFixture]
    public class WareGroupModelTest
    {
        private readonly ReflectionConversationalMetaInfoStore _converstionMetaInfoStore = new ReflectionConversationalMetaInfoStore();
        private IConversationalMetaInfoHolder _metaInfo;
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _converstionMetaInfoStore.Add(typeof(WareGroupModel));
            _metaInfo = _converstionMetaInfoStore.GetMetadataFor(typeof(WareGroupModel));
        }
        [Test]
        public void CanCancelWareGroup()
        {
            var dao = new Mock<IDao<WareGroup>>();
            var entityFactory = new Mock<IEntityFactory>();
            var wareGroup = new WareGroup();
            dao.Setup(rep => rep.Refresh(It.IsAny<WareGroup>())).AtMostOnce();
            var createWareGroupModel = new WareGroupModel(dao.Object, entityFactory.Object,
                                                          new Mock<IEntityValidator>().Object);
            createWareGroupModel.CancelWareGroup(wareGroup);
            dao.Verify(daowareGroup => daowareGroup.Refresh(wareGroup));
        }
        [Test]
        public void CanCreateWareGroup()
        {
            var dao = new Mock<IDao<WareGroup>>();
            var entityFactory = new Mock<IEntityFactory>();
            entityFactory.Setup(ent => ent.Create<WareGroup>()).Returns(new WareGroup()).AtMostOnce();
            var createWareGroupModel = new WareGroupModel(dao.Object, entityFactory.Object, new Mock<IEntityValidator>().Object);
            var wareGroup = createWareGroupModel.CreateWareGroup();
            entityFactory.Verify(ent => ent.Create<WareGroup>());
        }
        [Test]
        public void CanLoadAllWareGroups()
        {
            var dao = new Mock<IDao<WareGroup>>();
            var entityFactory = new Mock<IEntityFactory>();
            entityFactory.Setup(ent => ent.Create<WareGroup>()).Returns(new WareGroup()).AtMostOnce();
            var createWareGroupModel = new WareGroupModel(dao.Object, entityFactory.Object, new Mock<IEntityValidator>().Object);
            var wareGroup = createWareGroupModel.GetAllWareGroups();
            dao.Verify(d => d.RetrieveAll());
        }
        [Test]
        public void SaveWareGroupShouldDoesntWorkIfWareGroupInvalid()
        {
            var repository = new Mock<IDao<WareGroup>>();
            var entityValidator = new Mock<IEntityValidator>();
            var entityFactory = new Mock<IEntityFactory>();

            var wareGroup = new WareGroup();

            entityValidator.Setup(ev => ev.IsValid(wareGroup)).Returns(false);

            var albumManagerModel = new WareGroupModel(repository.Object,
                                                          entityFactory.Object,
                                                          entityValidator.Object);

            albumManagerModel.SaveWareGroup(wareGroup);

            entityValidator.Verify(ev => ev.IsValid(wareGroup));
        }
        [Test]
        public void SaveWareGroupShouldWork()
        {
            var repository = new Mock<IDao<WareGroup>>();
            var entityValidator = new Mock<IEntityValidator>();
            var entityFactory = new Mock<IEntityFactory>();
            var wareGroup = new WareGroup();

            entityValidator.Setup(ev => ev.IsValid(wareGroup)).Returns(true);
            repository.Setup(rep => rep.Save(It.IsAny<WareGroup>())).AtMostOnce();

            var albumManagerModel = new WareGroupModel(repository.Object,
                                                          entityFactory.Object,
                                                          entityValidator.Object);

            albumManagerModel.SaveWareGroup(wareGroup);

            entityValidator.Verify(ev => ev.IsValid(wareGroup));
            repository.Verify(rep => rep.Save(wareGroup));
        }
       

    }
}
