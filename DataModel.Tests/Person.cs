using System.Collections.Generic;
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


        private readonly Person[] _persons = new[]
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
                foreach (var person in _persons)
                {
                    session.Save(person);
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
        public void CanAddNewPerson()
        {
            var person = new Person { FIO = "Вася Пупкин", ContactPhone = "78-65-65" };
            IPersontRepository repository = new PersonRepository();
            repository.Add(person);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Person>(person.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreNotSame(person, fromDb);
                Assert.AreEqual(person.FIO, fromDb.FIO);
                Assert.AreEqual(person.ContactPhone, fromDb.ContactPhone);
            }
        }
        [Test]
        public void CanUpdateExistingPerson()
        {
            var person = _persons[0];
            person.FIO = "Петр Козлов";
            IPersontRepository repository = new PersonRepository();
            repository.Update(person);
            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Person>(person.Id);
                Assert.AreEqual(person.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void CanDeleteExistingPerson()
        {
            var person = _persons[0];
            IPersontRepository repository = new PersonRepository();
            repository.Remove(person);

            using (ISession session = _sessionFactory.OpenSession())
            {
                var fromDb = session.Get<Person>(person.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetPersonById()
        {

            IPersontRepository repository = new PersonRepository();
            var fromDb = repository.GetById(_persons[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_persons[1], fromDb);
            Assert.AreEqual(_persons[1].FIO, fromDb.FIO);
        }

        [Test]
        public void CanGetPersonByName()
        {
            IPersontRepository repository = new PersonRepository();
            var fromDb = repository.GetByFio(_persons[1].FIO);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_persons[1], fromDb);
            Assert.AreEqual(_persons[1].FIO, fromDb.FIO);

        }
        [Test]
        public void CanGetPersonByPartName()
        {
            IPersontRepository repository = new PersonRepository();
            var fromDb = repository.GetByPartFio("Сергей");
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb.Count, 2);
            Assert.IsTrue(IsInCollection(fromDb, _persons[0]));
            Assert.IsFalse(IsInCollection(fromDb, _persons[1]));
            Assert.IsFalse(IsInCollection(fromDb, _persons[2]));
            Assert.IsTrue(IsInCollection(fromDb, _persons[3]));
        }

        private bool IsInCollection(ICollection<Person> collection, Person searchingPerson)
        {
            return collection.Any(person => person.Id == searchingPerson.Id);
        }
    }
   
}

