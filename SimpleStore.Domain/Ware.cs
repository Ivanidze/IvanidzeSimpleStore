
using uNhAddIns.Entities;

namespace SimpleStore.Domain
{
    /// <summary>
    /// Базовый класс для товаров
    /// </summary>
    public class Ware:Entity
    {
        /// <summary>
        /// тип товар
        /// </summary>
        public virtual WareType WareType { get; set; }
        /// <summary>
        /// Клиент от которого поступил товар
        /// </summary>
        public virtual Client ClientBrought { get; set; }
        /// <summary>
        /// Описание товара
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// Отвественный работник
        /// </summary>
        public virtual Worker Worker { get; set;}
    }
}
