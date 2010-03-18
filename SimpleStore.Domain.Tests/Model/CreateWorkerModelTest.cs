using System.Collections.Generic;
using System.Reflection;
using Infrasturcture;
using Moq;
using NUnit.Framework;
using SimpleStore.Data;
using SimpleStore.Domain.Impl.WorkerModels;
using uNhAddIns.Adapters;
using uNhAddIns.Adapters.Common;
namespace SimpleStore.Domain.Tests.Model
{
    [TestFixture]
    public class CreateWorkerModelTest
    {
        private readonly ReflectionConversationalMetaInfoStore _converstionMetaInfoStore = new ReflectionConversationalMetaInfoStore();
        private IConversationalMetaInfoHolder _metaInfo;
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _converstionMetaInfoStore.Add(typeof(CreateWorkerModel));
            _metaInfo = _converstionMetaInfoStore.GetMetadataFor(typeof(CreateWorkerModel));
        }
        [Test]
        public void CanCancelWorker()
        {
            var dao = new Mock<IDao<Worker>>();
            var entityFactory = new Mock<IEntityFactory>();
            var worker = new Worker();
            dao.Setup(rep => rep.Refresh(It.IsAny<Worker>())).AtMostOnce();
            var createWorkerModel = new CreateWorkerModel(dao.Object, entityFactory.Object,
                                                          new Mock<IEntityValidator>().Object);
            createWorkerModel.CancelWorker(worker);
            dao.Verify(daoworker => daoworker.Refresh(worker));
        }
        [Test]
        public void CanCreateWorker()
        {
            var dao = new Mock<IDao<Worker>>();
            var entityFactory = new Mock<IEntityFactory>();
            entityFactory.Setup(ent => ent.Create<Worker>()).Returns(new Worker()).AtMostOnce();
            var createWorkerModel = new CreateWorkerModel(dao.Object, entityFactory.Object, new Mock<IEntityValidator>().Object);
            var worker = createWorkerModel.CreateWorker();
            entityFactory.Verify(ent => ent.Create<Worker>());
        }
        [Test]
        public void SaveWorkerShouldDoesntWorkIfWorkerInvalid()
        {
            var repository = new Mock<IDao<Worker>>();
            var entityValidator = new Mock<IEntityValidator>();
            var entityFactory = new Mock<IEntityFactory>();

            var worker = new Worker();

            entityValidator.Setup(ev => ev.IsValid(worker)).Returns(false);

            var albumManagerModel = new CreateWorkerModel(repository.Object,
                                                          entityFactory.Object,
                                                          entityValidator.Object);

            albumManagerModel.SaveWorker(worker);

            entityValidator.Verify(ev => ev.IsValid(worker));
        }
        [Test]
        public void SaveWorkerShouldWork()
        {
            var repository = new Mock<IDao<Worker>>();
            var entityValidator = new Mock<IEntityValidator>();
            var entityFactory = new Mock<IEntityFactory>();
            var worker = new Worker();

            entityValidator.Setup(ev => ev.IsValid(worker)).Returns(true);
            repository.Setup(rep => rep.Save(It.IsAny<Worker>())).AtMostOnce();

            var albumManagerModel = new CreateWorkerModel(repository.Object,
                                                          entityFactory.Object,
                                                          entityValidator.Object);

            albumManagerModel.SaveWorker(worker);

            entityValidator.Verify(ev => ev.IsValid(worker));
            repository.Verify(rep => rep.Save(worker));
        }
       

    }
}
