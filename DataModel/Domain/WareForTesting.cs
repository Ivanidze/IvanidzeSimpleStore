using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel.Domain
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
