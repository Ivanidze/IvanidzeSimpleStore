using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using uNhAddIns.Entities;

namespace SimpleStore.Data
{
    /// <summary>
    /// Контракт на вычитку из базы данных только для чтения
    /// </summary>
    /// <typeparam name="T">Только персистентные классы</typeparam>
    public interface IReadOnlyDao<T> where T : Entity
    {
        /// <summary>
        /// Вернуть обьект по его id если обьекта нет вернуть null
        /// </summary>
        /// <param name="id">id обьекта</param>
        /// <returns>Обьект или Null</returns>
        T GetById(object id);
        /// <summary>
        /// Вернуть обьект по его id, если нет обьекта будет эксепшен
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>обьект</returns>
        T GetProxy(object id);
        /// <summary>
        /// Вернуть список обьектов удовлетворяющих предикату
        /// </summary>
        /// <param name="predicate">предикат условия</param>
        /// <returns>список обьектов</returns>
        IEnumerable<T> Retrieve(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Вернуть количество обьектов удовлетворяющих условию
        /// </summary>
        /// <param name="predicate">предикат условия</param>
        /// <returns>количество обьектов</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Вернуть все сущности
        /// </summary>
        /// <returns>Список сущностей</returns>
        IEnumerable<T> RetrieveAll();    
    } 
}
