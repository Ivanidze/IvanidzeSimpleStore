﻿using System.Collections.Generic;

namespace DataModel.Domain
{
    public interface IContragentRepository
    {
        void Add(Contragent contragent);
        void Update(Contragent contragent);
        void Remove(Contragent contragent);
        Contragent GetById(int contragentId);
        Contragent GetByFio(string contragentFio);
        ICollection<Contragent> GetByPartFio(string contragentFioPart);

    }
}
