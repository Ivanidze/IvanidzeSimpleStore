using System.Collections.Generic;
using System.Linq;
using DataModel.DataAccess;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using NUnit.Framework;
using SimpleStore.Domain;
using System.Diagnostics;


namespace SimpleStore.Data.Tests
{

    [TestFixture]
    public class PersonDataTest : PersistenceFixtureBase
    {


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

        private IDao<Person> _repository;
        private void CreateInitialData()
        {
            using (ISession session = sessions.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                foreach (var person in _persons)
                {
                    session.Save(person);
                }
                transaction.Commit();

            }
            _repository = new Dao<Person>(sessions);
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
        public void CanAddNewPerson()
        {
           
            var person = new Person { FIO = "Вася Пупкин", ContactPhone = "78-65-65" };
            _repository.Save(person);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Person>(person.Id);

                Assert.IsNotNull(fromDb);
                Assert.AreEqual(person, fromDb);
                
            }
        }
        [Test]
        public void CanUpdateExistingPerson()
        {
            var person = _persons[0];
            person.FIO = "Петр Козлов";
            _repository.Update(person);
            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Person>(person.Id);
                Assert.AreEqual(person.FIO, fromDb.FIO);
            }
        }
        [Test]
        public void CanDeleteExistingPerson()
        {
            var repositorysession = sessions.OpenSession();
            CurrentSessionContext.Bind(repositorysession);
            var person = _persons[0];
            _repository.Delete(person);

            using (ISession session = sessions.OpenSession())
            {
                var fromDb = session.Get<Person>(person.Id);
                Assert.IsNull(fromDb);
            }

        }
        [Test]
        public void CanGetPersonById()
        {
            var repositorysession = sessions.OpenSession();
            CurrentSessionContext.Bind(repositorysession);
            var fromDb = _repository.GetById(_persons[1].Id);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_persons[1], fromDb);
            Assert.AreEqual(_persons[1].FIO, fromDb.FIO);
        }

        [Test]
        public void CanGetPersonByName()
        {
            var repositorysession = sessions.OpenSession();
            CurrentSessionContext.Bind(repositorysession);
            var fromDb = _repository.Retrieve(person=>person.FIO==_persons[1].FIO);
            Assert.IsNotNull(fromDb);
            Assert.AreNotSame(_persons[1], fromDb);
            Assert.AreEqual(_persons[1], fromDb.First());

        }
        [Test]
        public void CanGetPersonByPartName()
        {
            var repositorysession = sessions.OpenSession();
            CurrentSessionContext.Bind(repositorysession);
            var fromDb = _repository.Count((person) => person.FIO.Contains("Сергей"));
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(fromDb, 2);
        }

       
        [Test]
        public void CanGetPersonAll()
        {
            

            
            int collection = _repository.RetrieveAll().Count();
            Assert.AreEqual(_persons.Length,collection);
        }
    }
   
}

