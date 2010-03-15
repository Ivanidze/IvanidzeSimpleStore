namespace SimpleStore.Domain
{
    /// <summary>
    /// Товар принятый на тестирование
    /// </summary>
    public class WareForTesting : Ware
    {
        /// <summary>
        /// Приоритет среди других товаров на тестировании
        /// </summary>
        public virtual int Priority { get; set; }
    }
}
