using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleStore.Domain.Model.WorkerModels
{

    public interface ISimpleModel
    {
        IList<Worker> GetAllArtists();
    }
}
