﻿using System.Collections.Generic;
using System.Linq;
using DataModel.Domain;
using DataModel.Repositories;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NUnit.Framework;

namespace DataModel.Tests
{

    [TestFixture]
    public class PersonRepository_Fixture
    {
        private ISessionFactory _sessionFactory;


        private readonly Person[] _contragents = new[]
                                                        {
                                                            new Person
                                                                {FIO = "Сергей Бояринцев", ContactPhone = "33-22-33"},
                                                            new Person
                                                                {FIO = "Василий Петров", ContactPhone = "32-23-65"},
                                                            new Person
                                                                {FIO = "Андрей Иванов", ContactPhone = "54-42-65"},
                                                            new Person{FIO = "Сергей Бездарнов",ContactPhone = "33-65-23"}
            
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
        public void CanAddNewContragent()
        {
            var contragent = new Person { FIO = "Вася Пупкин", ContactPhone = "78-65-65" };
            IPersontRepository repository = new PersonRepository();
            repository.Add(contragent);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Person>(contragent.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(contragent, fromDb);
                Assert.AreEqual(contragent.FIO, fromDb.FIO);
                Assert.AreEqual(contragent.ContactPhone, fromDb.ContactPhone);
            }
        }
        [Test]
        public void CanUpdateExistingContragent()
        {
            var contragent = _contragents[0];
            contragent.FIO = "Петр Козлов";
            IPersontRepository repository = new PersonRepository();
            repository.Update(contragent);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Person>(contragent.Id);
                Assert.AreEqual(contragent.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void CanDeleteExistingContragent()
        {
            var contragent = _contragents[0];
            IPersontRepository repository = new PersonRepository();
            repository.Remove(contragent);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Person>(contragent.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetContragentById()
        {

            IPersontRepository repository = new PersonRepository();
            var fromDb = repository.GetById(_contragents[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_contragents[1], fromDb);
            Assert.AreEqual(_contragents[1].FIO, fromDb.FIO);
        }

        [Test]
        public void CanGetContragentByName()
        {
            IPersontRepository repository = new PersonRepository();
            var fromDb = repository.GetByFio(_contragents[1].FIO);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_contragents[1], fromDb);
            Assert.AreEqual(_contragents[1].FIO, fromDb.FIO);

        }
        [Test]
        public void CanGetContragentByPartName()
        {
            IPersontRepository repository = new PersonRepository();
            var fromDb = repository.GetByPartFio("Сергей");
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Count, 2);
            Assert.IsTrue(IsInCollection(fromDb, _contragents[0]));
            Assert.IsFalse(IsInCollection(fromDb, _contragents[1]));
            Assert.IsFalse(IsInCollection(fromDb, _contragents[2]));
            Assert.IsTrue(IsInCollection(fromDb, _contragents[3]));
        }

        private bool IsInCollection(ICollection<Person> collection, Person searchingContragent)
        {
            return collection.Any(contragent => contragent.Id == searchingContragent.Id);
        }
    }
   
}
