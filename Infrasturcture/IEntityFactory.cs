using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uNhAddIns.Entities;

namespace Infrasturcture
{
    public interface IEntityFactory
    {
        T Create<T>() where T : Entity;
    }
}
