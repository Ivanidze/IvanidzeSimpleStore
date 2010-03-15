using uNhAddIns.Entities;

namespace SimpleStore.Data
{
    /// <summary>
    /// Контракт для работы с персистентными классами для изменения их
    /// </summary>
    /// <typeparam name="T">персистентый класс</typeparam>
    public interface IDao<T>:IReadOnlyDao<T> where T:Entity
    {
        /// <summary>
        /// Сохранить сущность в базе
        /// </summary>
        /// <param name="entity">сущность</param>
        void Save(T entity);
        /// <summary>
        /// Обновить сущность в базе
        /// </summary>
        /// <param name="entity">сущность</param>
        void Update(T entity);
        /// <summary>
        /// Удалить сущность в базе
        /// </summary>
        /// <param name="entity">сущность</param>
        void Delete(T entity);

        /// <summary>
        /// Перечитать состояние сущности из базы данных
        ///  </summary>
        /// <param name="entity">сущность</param>
        void Refresh(T entity);

        /// <summary>
        /// Слить состояние сущности в текущий Unit of Work
        ///  </summary>
        /// <param name="entity">Сущность</param>
        void Merge(T entity);
    }
}


