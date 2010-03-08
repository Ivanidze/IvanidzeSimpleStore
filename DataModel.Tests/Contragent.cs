using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Cfg;
using DataModel.Domain;
using DataModel.Repositories;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NUnit.Framework;

namespace DataModel.Tests
{

    [TestFixture]
    public class ContragentRepository_Fixture
    {
        private ISessionFactory _sessionFactory;


        private readonly Contragent[] _contragents = new[]
                                                        {
                                                            new Contragent
                                                                {FIO = "Сергей Бояринцев", ContactPhone = "33-22-33"},
                                                            new Contragent
                                                                {FIO = "Василий Петров", ContactPhone = "32-23-65"},
                                                            new Contragent
                                                                {FIO = "Андрей Иванов", ContactPhone = "54-42-65"},
                                                            new Contragent{FIO = "Сергей Бездарнов",ContactPhone = "33-65-23"}
            
                                                        };
        private void CreateInitialData()
        {
            using (ISession session = _sessionFactory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var contragent in _contragents)
                {
                    session.Save(contragent);
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
        public void Can_add_new_Contragent()
        {
            var contragent = new Contragent { FIO = "Вася Пупкин", ContactPhone = "78-65-65" };
            IContragentRepository repository = new ContragentRepository();
            repository.Add(contragent);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Contragent>(contragent.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(contragent, fromDb);
                Assert.AreEqual(contragent.FIO, fromDb.FIO);
                Assert.AreEqual(contragent.ContactPhone, fromDb.ContactPhone);
            }
        }
        [Test]
        public void Can_Update_Existing_Contragent()
        {
            var contragent = _contragents[0];
            contragent.FIO = "Петр Козлов";
            IContragentRepository repository = new ContragentRepository();
            repository.Update(contragent);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Contragent>(contragent.Id);
                Assert.AreEqual(contragent.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void Can_Delete_Existing_Contragent()
        {
            var contragent = _contragents[0];
            IContragentRepository repository = new ContragentRepository();
            repository.Remove(contragent);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Contragent>(contragent.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void Can_Get_Contragent_By_Id()
        {

            IContragentRepository repository = new ContragentRepository();
            var fromDb = repository.GetById(_contragents[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_contragents[1], fromDb);
            Assert.AreEqual(_contragents[1].FIO, fromDb.FIO);
        }

        [Test]
        public void Can_get_Contragent_By_Name()
        {
            IContragentRepository repository = new ContragentRepository();
            var fromDb = repository.GetByFio(_contragents[1].FIO);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_contragents[1], fromDb);
            Assert.AreEqual(_contragents[1].FIO, fromDb.FIO);

        }
        [Test]
        public void Can_get_Contragent_By_PartName()
        {
            IContragentRepository repository = new ContragentRepository();
            var fromDb = repository.GetByPartFio("Сергей");
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Count, 2);
            Assert.IsTrue(IsInCollection(fromDb, _contragents[0]));
            Assert.IsFalse(IsInCollection(fromDb, _contragents[1]));
            Assert.IsFalse(IsInCollection(fromDb, _contragents[2]));
            Assert.IsTrue(IsInCollection(fromDb, _contragents[3]));
        }

        private bool IsInCollection(ICollection<Contragent> collection, Contragent searchingContragent)
        {
            return collection.Any(contragent => contragent.Id == searchingContragent.Id);
        }
    }
   
}

